using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class Engine_Smoke_Effect : MonoBehaviour
{
    public ParticleSystem ParticleObject;
    GameDirector gameDirector;
    ParticleSystem.MainModule mainModule;
    ParticleSystem.SizeOverLifetimeModule sizeMoule;
    /* ParticleSystem Particle;

 */
    float Speed;
    float MaxSpeed;
    float lerp;

    // Start is called before the first frame update
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        MaxSpeed = gameDirector.MaxSpeed;
        mainModule = ParticleObject.GetComponent<ParticleSystem>().main;
        sizeMoule = ParticleObject.GetComponent<ParticleSystem>().sizeOverLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        Speed = gameDirector.TrainSpeed;
        lerp = Speed / MaxSpeed;

        if (gameDirector.gameType == GameType.Ending)
        {
            ChangeEnd();
        }
        else
        {
            UpdateStartLifeTime();
            UpdateSizeOverLifeTime();
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
        float startLifetime = Mathf.Lerp(1f,11f, lerp);
        float simulationSpeed = Mathf.Lerp(2f, 11f, lerp);
        mainModule.startLifetime = startLifetime;
        mainModule.simulationSpeed = simulationSpeed;
    }

    private void UpdateSizeOverLifeTime()
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0f);
        float lastValue = Mathf.Lerp(5f, 8f, lerp);
        curve.AddKey(1f, 1f);
        sizeMoule.size = new ParticleSystem.MinMaxCurve(lastValue, curve);
    }
}
