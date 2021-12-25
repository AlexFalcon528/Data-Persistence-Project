using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbablyBadWayToHandleProblem : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        MainManager.Instance.BeginGame();
        MainManager.Instance.m_Started = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
