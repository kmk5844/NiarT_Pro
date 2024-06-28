using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Turret : Turret
{
    bool Target_Flag;
    public Transform FireObject;
    public Transform BulletObject;
    public Transform BulletObject_Fire;
    Transform Target;
    public float Z;

    protected override void Start()
    {
        base.Start();
        rotation_TurretFlag = true;
        train_Rotation_Delay = 0.75f;

        BulletObject_Fire.name = "Fire_Arrow";
        Target_Flag = false;
        BulletObject.GetComponent<Bullet>().atk = trainData.Train_Attack;
        BulletObject_Fire.GetComponent<Bullet>().atk = trainData.Train_Attack;
    }

    private void Update()
    {
        Turret_Flip();
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
            Vector3 rot = Target.position - transform.position;
            float rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
            Z = Quaternion.Euler(0, 0, rotZ).eulerAngles.z - transform.rotation.eulerAngles.z;

            if (Z > 180f)
            {
                Z -= 360f;
            }
            else if (Z < -180f)
            {
                Z += 360f;
            }

            if (Z > 1)
            {
                transform.Rotate(new Vector3(0, 0, (train_Rotation_Delay + Item_Rotation_Delay)));
            }
            else if (Z < -1)
            {
                transform.Rotate(new Vector3(0, 0, -(train_Rotation_Delay + Item_Rotation_Delay)));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, rotZ);
            }
            BulletFire();
        }//Target_Flag�� false��� �ǵ��ư��� �ڵ�
    }

    void BulletFire()
    {
        int Random_Arrow = Random.Range(0, 11);
        if (Time.time >= lastTime + (train_Attack_Delay - Item_Attack_Delay))
        {
            if(Random_Arrow == 10 || Random_Arrow == 5)
            {
                Instantiate(BulletObject_Fire, FireObject.position, FireObject.rotation, Bullet_List);
            }
            else
            {
                Instantiate(BulletObject, FireObject.position, FireObject.rotation, Bullet_List);
            }
            lastTime = Time.time;
        }
    }

    void Turret_Flip()
    {
        if (transform.eulerAngles.z >= -90f && transform.eulerAngles.z < 90f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (transform.eulerAngles.z >= 90f && transform.eulerAngles.z < 270f)
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (Target == null)
            {
                Target = collision.transform;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
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
        if (collision.CompareTag("Monster"))
        {
            if (collision.transform == Target)
            {
                Target = null;
            }
        }
    }
}