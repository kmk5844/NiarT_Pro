using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    bool Target_Flag;
    public Transform BulletObject;
    Transform Bullet_List;
    Transform Target;
    Train_InGame trainData;
    int train_Attack;
    float train_Attack_Delay;
    float lastTime;
    public float Z;

    void Start()
    {
        Target_Flag = false;
        trainData = transform.GetComponentInParent<Train_InGame>();
        Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();
        BulletObject.GetComponent<Bullet>().atk = transform.GetComponentInParent<Train_InGame>().Train_Attack;
        train_Attack = trainData.Train_Attack;
        train_Attack_Delay = trainData.Train_Attack_Delay;
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
                transform.Rotate(new Vector3(0, 0, 0.7f));
            }else if(Z < -1)
            {
                transform.Rotate(new Vector3(0, 0, -0.7f));
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
        if (Time.time >= lastTime + train_Attack_Delay)
        {
            Instantiate(BulletObject, transform.position, transform.rotation, Bullet_List);
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
            if(Target == null)
            {
                Target = collision.transform;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if(Target == null)
            {
                Target = collision.transform;
            }
            if(collision.transform != Target)
            {
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if(collision.transform == Target)
            {
                Target = null;
            }
        }
    }
}
