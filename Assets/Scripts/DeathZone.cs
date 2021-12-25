using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public MainManager Manager;

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Got our pickaxe swingin from side to side");
        Destroy(other.gameObject);
        Manager.GameOver();
    }
}
