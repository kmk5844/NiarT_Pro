using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Engine_Smoke_Effect : MonoBehaviour
{
    public GameObject ParticleObject;
    GameDirector gameDirector;
    ParticleSystem Particle;
    private ParticleSystem.MainModule mainModule;

    float Speed;
    float MaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        Particle = ParticleObject.GetComponent<ParticleSystem>();
        mainModule = Particle.main;
    }

    // Update is called once per frame
    void Update()
    {
        MaxSpeed = gameDirector.MaxSpeed;
        Speed = gameDirector.TrainSpeed;
/*        UpdateStartLifeTime();
        UpdateRoation();
        UpdateStartSpeed();
        UpdateGravity();*/

        if (gameDirector.gameType == GameType.Ending)
        {
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        mainModule.startColor = Color.black;
    }
/*
    private void UpdateRoation()
    {
        float rotation = Mathf.Lerp(-90f, -3f, Speed / MaxSpeed);
        ParticleObject.transform.rotation = Quaternion.Euler(rotation, -90, -90);
    }

    private void UpdateStartLifeTime()
    {
        float startLifetime = Mathf.Lerp(5f, 20f, Speed / MaxSpeed);
        mainModule.startLifetime = startLifetime;
    }

    private void UpdateStartSpeed()
    {
        float startSpeedTime = Mathf.Lerp(1f,5f, Speed/MaxSpeed);
        mainModule.startSpeed = startSpeedTime;
    }

    private void UpdateGravity()
    {
        float gravityModifier = Mathf.Lerp(0f, -0.003f, Speed / MaxSpeed);
        mainModule.gravityModifier = gravityModifier;
    }*/
}
