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
        UpdateStartLifeTime();
        UpdateVelocityOverLifeTime();
        if(gameDirector.gameType == GameType.Ending)
        {
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        mainModule.startColor = Color.black;
    }

    private void UpdateStartLifeTime()
    {
        float startLifetime = Mathf.Lerp(0.5f, 2f, Speed / MaxSpeed);
        mainModule.startLifetime = startLifetime;
    }

    private void UpdateVelocityOverLifeTime()
    {
        float velocityY = Mathf.Lerp(0f, -2.4f, Speed / MaxSpeed);
        float offsetZ = Mathf.Lerp(0f, -1f,  Speed / MaxSpeed);
        var vel = Particle.velocityOverLifetime;
        vel.orbitalY = velocityY;
        vel.orbitalOffsetZ = offsetZ;
    }
}
