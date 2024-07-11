using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Fire_Turret : Turret
{
    public GameObject Fire_Object;
    public GameObject Fire_Effect;
    SpriteRenderer Fire_Turret_Image;
    ParticleSystem Particle;
    private ParticleSystem.MainModule mainModule;

    float Data_Attack_Delay;
    float train_Attacking_Delay;

    bool LR_Flag;
    public bool Attack_Flag;
    public bool Change_Flag;
    float Turret_Z;
    public float Turret_Min_Z;
    public float Turret_Max_Z;
    protected override void Start()
    {
        base.Start();
        rotation_TurretFlag = true;
        train_Rotation_Delay = 0.35f;
        Fire_Turret_Image = GetComponent<SpriteRenderer>();

        Data_Attack_Delay = train_Attack_Delay;
        train_Attacking_Delay = 5;

        Change_Flag = false;
        Attack_Flag = true;
        LR_Flag = true;
        Particle = Fire_Effect.GetComponent<ParticleSystem>();
        mainModule = Particle.main;
        Fire_Object.GetComponent<Bullet>().atk = trainData.Train_Attack;
        lastTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Turret_Flip();
        Turret_Z = transform.rotation.eulerAngles.z;

        if (Turret_Z > Turret_Max_Z)
        {
            LR_Flag = false;
        }
        if (Turret_Z < Turret_Min_Z)
        {
            LR_Flag = true;
        }

        if (LR_Flag)
        {
            transform.Rotate(new Vector3(0, 0, (train_Rotation_Delay + Item_Rotation_Delay)));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -(train_Rotation_Delay + Item_Rotation_Delay)));
        }
        Fire_Check();
        UpdateVelocityOverLifeTime();
    }

    void Fire_Check()
    {
        Fire_OnOff();
        if (Time.time >= lastTime + train_Attack_Delay && Attack_Flag)
        {
            Attack_Flag = false;
            Change_Flag = true;
            train_Attack_Delay = Data_Attack_Delay;
            lastTime = Time.time;
        }
        else if (Time.time >= lastTime + (train_Attack_Delay - Item_Attack_Delay) && !Attack_Flag)
        {
            Attack_Flag = true;
            Change_Flag = true;
            train_Attack_Delay = train_Attacking_Delay;
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

    void Turret_Flip()
    {
        if (transform.eulerAngles.z >= -90f && transform.eulerAngles.z < 90f)
        {
            Fire_Turret_Image.flipY = false;
        }
        else if (transform.eulerAngles.z >= 90f && transform.eulerAngles.z < 270f)
        {
            Fire_Turret_Image.flipY = true;
        }
        else
        {
            Fire_Turret_Image.flipY = false;
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