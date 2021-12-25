using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text HighScoreText;
    public Text ScoreText;
    public GameObject GameOverText;
    
    public bool m_Started = false;
    private int m_Points;
    private Vector3 BallStart;
    public static bool m_GameOver = false;
    public string PlayerName;
    public int HighScore;
    public string HighScorePlayerName;
    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }
    // Start is called before the first frame update
    void StartNew()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        GameOverText.GetComponent<Text>().enabled = false;
        m_GameOver = false;
        m_Points = 0;
    }
    public void BeginGame()
    {
        Ball = GameObject.Find("Ball").GetComponent<Rigidbody>();
        HighScoreText = GameObject.Find("BestScoreText").GetComponent<Text>();
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        GameOverText = GameObject.Find("GameoverText");
        StartNew();
        Load();
        HighScoreText.text = $"High score: {HighScorePlayerName} , {HighScore}";

    }
    private void Update()
    {
        if (!m_Started)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("Woo Yeah");
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            //Debug.Log(m_GameOver);
            GameOverText.GetComponent<Text>().enabled = m_GameOver;
            //Debug.Log("So way back in the mine");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("Aw man");
                Save();
                SceneManager.LoadScene(0);
                SceneManager.UnloadSceneAsync(1);
                m_Started = false;
                m_GameOver = false;
            }
            
        }
        if (HighScoreText != null && m_Points > HighScore)
        {
            HighScoreText.text = $"{PlayerName} : {m_Points}";
        }

    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        //Debug.Log(m_GameOver);
        m_GameOver = true;
        //Debug.Log(m_GameOver);
        
    }
    [System.Serializable]
    class SaveData
    {
        public string SavedPlayerName;
        public int SavedHighScore;
    }
    public void Save()
    {
        SaveData data = new SaveData();
        data.SavedPlayerName = PlayerName;
        if (m_Points > HighScore)
        {
            data.SavedPlayerName = PlayerName;
            data.SavedHighScore = m_Points;
        }
        else
        {
            data.SavedPlayerName = HighScorePlayerName;
            data.SavedHighScore = HighScore;
        }
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScorePlayerName = data.SavedPlayerName;
            HighScore = data.SavedHighScore;
        }
    }
 }
