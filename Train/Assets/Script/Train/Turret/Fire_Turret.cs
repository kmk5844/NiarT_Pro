using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Fire_Turret : MonoBehaviour
{
    Train_InGame trainData;
    public GameObject Fire_Object;

    public GameObject Fire_Effect;
    ParticleSystem Particle;
    private ParticleSystem.MainModule mainModule;

    bool LR_Flag;
    public bool Attack_Flag;
    public bool Change_Flag;
    float Turret_Z;
    float train_Attack_Delay;
    float lastTime;
    public float Turret_Min_Z;
    public float Turret_Max_Z;


    // Start is called before the first frame update
    void Start()
    {
        Change_Flag = false;
        Attack_Flag = true; 
        LR_Flag = true;
        Particle = Fire_Effect.GetComponent<ParticleSystem>();
        mainModule = Particle.main;
        trainData = transform.GetComponentInParent<Train_InGame>();
        Fire_Object.GetComponent<Bullet>().atk = trainData.Train_Attack;
        train_Attack_Delay = trainData.Train_Attack_Delay;
        lastTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Turret_Z = transform.rotation.eulerAngles.z;

        if (Turret_Z > Turret_Max_Z)
        {
            LR_Flag = false;
        }
        if (Turret_Z < Turret_Min_Z)
        {
            LR_Flag |= true;
        }

        if (LR_Flag)
        {
            transform.Rotate(new Vector3(0, 0, 0.35f));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -0.35f));
        }
        Fire_Check();
        UpdateVelocityOverLifeTime();
    }

    void Fire_Check()
    {
        Fire_OnOff();
        if (Time.time >= lastTime + 5 && Attack_Flag)
        {
            Attack_Flag = false;
            Change_Flag = true; 
            lastTime = Time.time;
        }
        else if(Time.time >= lastTime + train_Attack_Delay && !Attack_Flag)
        {
            Attack_Flag = true;
            Change_Flag = true;
            lastTime = Time.time;
        }
    }

    void Fire_OnOff()
    {
        if (Change_Flag)
        {
            if (Attack_Flag)
            {
                Fire_Object.SetActive(true);
                Particle.Play();
            }
            else
            {
                Fire_Object.SetActive(false);
                Particle.Stop();
            }
            Change_Flag = false;        
        }
    }

    private void UpdateVelocityOverLifeTime()
    {
        float Oribital_Z = Mathf.Lerp(0.8f, -0.8f, Turret_Z / Turret_Max_Z);
        float Particle_Z = Mathf.Lerp(60f, -60f, Turret_Z / Turret_Max_Z);
        var vel = Particle.velocityOverLifetime;
        vel.orbitalY = Oribital_Z;
        mainModule.startRotation = Particle_Z / 50f;
    }
}
