using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intercept_Missile_Turret : MonoBehaviour
{
    bool Target_Flag;
    public Transform FireObject;
    public Transform BulletObject;
    Transform Bullet_List;
    public Transform Target;
    float train_Attack_Delay;
    float lastTime;
    public float Z;

    void Start()
    {
        Target_Flag = false;
        Train_InGame trainData = transform.GetComponentInParent<Train_InGame>();
        Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();
        BulletObject.GetComponent<Bullet>().atk = trainData.Train_Attack;
        train_Attack_Delay = trainData.Train_Attack_Delay;
        lastTime = 0;
    }
    void Update()
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
    }

    void BulletFire()
    {
        if (Time.time >= lastTime + train_Attack_Delay)
        {
            BulletObject.GetComponent<Missile_TurretBullet>().monster_target = Target;
            Instantiate(BulletObject, FireObject.position, FireObject.rotation, Bullet_List);
            lastTime = Time.time;
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
