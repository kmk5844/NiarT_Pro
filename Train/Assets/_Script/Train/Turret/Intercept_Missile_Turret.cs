using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intercept_Missile_Turret : Turret
{
    bool Target_Flag;
    public Transform FireObject;
    public Transform BulletObject;
    public Transform Target;
    public float Z;

    protected override void Start()
    {
        base.Start();
        Target_Flag = false;
        BulletObject.GetComponent<Bullet>().atk = trainData.Train_Attack;
    }
    void Update()
    {
        if(!trainData.DestoryFlag)
        {
            if (Target != null)
            {
                Target_Flag = true;
            }
            else
            {
                Target_Flag = false;
            }

            if (Target_Flag)
            {
                BulletFire();
            }
            RotateTurret(Target_Flag);
        }
    }

    void BulletFire()
    {
        if (Time.time >= lastTime + (train_Attack_Delay - Item_Attack_Delay))
        {
            BulletObject.GetComponent<Missile_TurretBullet>().monster_target = Target;
            Instantiate(BulletObject, FireObject.position, FireObject.rotation, Bullet_List);
            lastTime = Time.time;
        }
    }

    void RotateTurret(bool Flag)
    {
        if (Flag)
        {
            if (transform.rotation.eulerAngles.z < 20)
            {
                transform.Rotate(new Vector3(0, 0, 0.25f));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 20);
            }
        }
        else
        {
            if (transform.rotation.eulerAngles.z <= 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 1);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, -0.25f));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            if (Target == null)
            {
                Target = collision.transform;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            if (Target == null)
            {
                Target = collision.transform;
            }
            if (collision.transform != Target)
            {
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            if (collision.transform == Target)
            {
                Target = null;
            }
        }
    }
}
