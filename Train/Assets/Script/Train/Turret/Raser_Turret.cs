using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raser_Turret : MonoBehaviour
{
    bool Target_Flag;
    bool Fire_Flag;
    Transform Target;
    public GameObject test_raser;
    Train_InGame trainData;
    float train_Attack_Delay;
    float train_Attacking_Delay;
    float lastTime;
    public float Z;
    

    void Start()
    {
        Target_Flag = false;
        trainData = transform.GetComponentInParent<Train_InGame>();
        test_raser.GetComponent<Bullet>().atk = trainData.Train_Attack;
        train_Attack_Delay = trainData.Train_Attack_Delay;
        train_Attacking_Delay = 4;

        lastTime = Time.time + 12;
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

            if (Fire_Flag)
            {
                if ((Time.time >= lastTime + train_Attacking_Delay))
                {
                    test_raser.SetActive(false);
                    Fire_Flag = false;
                    lastTime = Time.time;
                }
            }
            else
            {
                if (Z > 0.1f)
                {
                    transform.Rotate(new Vector3(0, 0, 0.05f));
                }
                else if (Z < -0.1f)
                {
                    transform.Rotate(new Vector3(0, 0, -0.05f));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, rotZ);
                }

                if((Time.time >= lastTime + train_Attack_Delay))
                {
                    test_raser.SetActive(true);
                    Fire_Flag = true;
                    lastTime = Time.time;
                }
            }
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
