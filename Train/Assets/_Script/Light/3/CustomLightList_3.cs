using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLightList_3 : MonoBehaviour
{
    GameDirector gameDirector;
    public float Speed;
    private void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    private void Update()
    {
        Speed = gameDirector.TrainSpeed;
    }
}