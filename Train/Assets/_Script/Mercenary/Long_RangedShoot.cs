using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_RangedShoot : MonoBehaviour
{
    bool Target_Flag;
    public Transform BulletObj;
    Transform Bullet_List;
    Transform Target;

    int unit_Attack;
    float unit_Attack_Delay;
    float lastTime;

    Long_Ranged unit;
    public bool isDelaying;

    void Start()
    {
        Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();
        unit = GetComponentInParent<Long_Ranged>();
        unit_Attack = unit.unit_Attack;
        unit_Attack_Delay = unit.unit_Attack_Delay;
        lastTime = 0;
        isDelaying = false;
    }

    void Update()
    {
        if(Target != null)
        {
            Target_Flag = true;
        }
        else
        {
            Target_Flag = false;
        }

        unit.TargetFlag(Target_Flag);

        if (!isDelaying)
        {
            if (Target_Flag)
            {
                Vector3 rot = Target.position - transform.position;
                float rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;

                if (rotZ >= -90 && rotZ <= 90)
                {
                    transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.GetChild(0).transform.localScale = new Vector3(1, -1, 1);
                }


                if (Quaternion.Euler(0, 0, rotZ).eulerAngles.z - transform.rotation.eulerAngles.z > 0.5f)
                {
                    transform.Rotate(new Vector3(0, 0, 0.7f));
                }
                else if (Quaternion.Euler(0, 0, rotZ).eulerAngles.z - transform.rotation.eulerAngles.z < -0.5f)
                {
                    transform.Rotate(new Vector3(0, 0, -0.7f));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, rotZ);
                }
                BulletFire();
            }
            else
            {
                float targetAngle = 0;
                Vector3 rotationEuler = transform.rotation.eulerAngles;
                if (rotationEuler.z >= 0f && rotationEuler.z <= 90f)
                {
                    targetAngle = 0f; // 제 1사분면에 있을 때
                }
                else if (rotationEuler.z > 90f && rotationEuler.z <= 180f)
                {
                    targetAngle = 180f; // 제 2사분면에 있을 때
                }
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle); // 목표 회전을 표현하는 쿼터니언 값
                if (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 0.7f);
                }
            }
        }
    }

    void BulletFire()
    {
        if(Time.time >= lastTime + unit_Attack_Delay)
        {
            BulletObj.GetComponent<Bullet>().atk = unit_Attack;
            Instantiate(BulletObj, transform.position, transform.rotation, Bullet_List);
            unit.workCountUP();
            //여기서 카운트 센다.
            lastTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (Target == null)
            {
                Target = collision.transform;
                lastTime = Time.time;
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
