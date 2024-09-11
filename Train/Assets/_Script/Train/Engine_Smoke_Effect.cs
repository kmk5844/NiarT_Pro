using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Engine_Smoke_Effect : MonoBehaviour
{
    public ParticleSystem ParticleObject;
    GameDirector gameDirector;
    private ParticleSystem.MainModule mainModule;
    /* ParticleSystem Particle;

 */
    float Speed;
    float MaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        MaxSpeed = gameDirector.MaxSpeed;
        mainModule = ParticleObject.GetComponent<ParticleSystem>().main;
    }

    // Update is called once per frame
    void Update()
    {
        Speed = gameDirector.TrainSpeed;

        if (gameDirector.gameType == GameType.Ending)
        {
            ChangeEnd();
        }
        else
        {
            UpdateStartLifeTime();
        }
    }

    private void ChangeEnd()
    {
        mainModule.startColor = Color.black;
        mainModule.startLifetime = 5;
        mainModule.simulationSpeed = 1;
    }

    private void UpdateStartLifeTime()
    {
        float startLifetime = Mathf.Lerp(1f, 8f, Speed / MaxSpeed);
        mainModule.startLifetime = startLifetime;
        float simulationSpeed = Mathf.Lerp(2f, 6f, Speed / MaxSpeed);
        mainModule.simulationSpeed = simulationSpeed;
    }
}
