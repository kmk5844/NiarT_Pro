using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto_Turret : Turret
{
    bool Target_Flag;
    public Transform FireObject;
    public Transform BulletObject;
    Transform Target;
    public float Z;    

    protected override void Start()
    {
        base.Start();

        rotation_TurretFlag = true;
        train_Rotation_Delay = 0.6f;

        Target_Flag = false;
        BulletObject.GetComponent<Bullet>().atk = trainData.Train_Attack;
        lastTime = 0;
    }
    void Update()
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

            if(Z > 1)
            {
                transform.Rotate(new Vector3(0, 0, (train_Rotation_Delay + Item_Rotation_Delay)));
            }else if(Z < -1)
            {
                transform.Rotate(new Vector3(0, 0, -(train_Rotation_Delay + Item_Rotation_Delay)));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, rotZ);
            }
            BulletFire();
        }//Target_Flag가 false라면 되돌아가는 코드
    }

    void BulletFire()
    {
        if (Time.time >= lastTime + (train_Attack_Delay - Item_Attack_Delay))
        {
            Instantiate(BulletObject, FireObject.position, FireObject.rotation, Bullet_List);
            lastTime = Time.time;
        }
    }

    void Turret_Flip()
    {
        if(transform.eulerAngles.z >= -90f && transform.eulerAngles.z < 90f)
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
            if (collision.GetComponent<Monster>().Monster_Type.Equals("Sky"))
            {
                if (collision.GetComponent<Monster>() != null)
                {
                    if (Target == null)
                    {
                        Target = collision.transform;
                    }
                }
            }
            else if (collision.GetComponent<Boss>() != null)
            {
                if (Target == null)
                {
                    Target = collision.transform;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (collision.GetComponent<Monster>() != null)
            {
                if (collision.GetComponent<Monster>().Monster_Type.Equals("Sky"))
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
            else if (collision.GetComponent<Boss>() != null)
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (collision.GetComponent<Monster>() != null)
            {
                if (collision.GetComponent<Monster>().Monster_Type.Equals("Sky"))
                {
                    if (collision.transform == Target)
                    {
                        Target = null;
                    }
                }
            }
            else if (collision.GetComponent<Boss>() != null)
            {
                if (collision.transform == Target)
                {
                    Target = null;
                }
            }
        }
    }
}
