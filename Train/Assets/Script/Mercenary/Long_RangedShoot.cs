using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_RangedShoot : MonoBehaviour
{
    bool Target_Flag;
    public Transform Bullet;
    Transform Bullet_List;
    Transform Target;

    int unit_Attack;
    float unit_Attack_Delay;
    float lastTime;
    
    Long_Ranged unit;
    public bool isDelaying; // 아마 탈진 때문에 적었을 가능성이 큼

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
                if (Quaternion.Euler(0, 0, rotZ).eulerAngles.z - transform.rotation.eulerAngles.z > 1)
                {
                    transform.Rotate(new Vector3(0, 0, 0.7f));
                }
                else if (Quaternion.Euler(0, 0, rotZ).eulerAngles.z - transform.rotation.eulerAngles.z < -1)
                {
                    transform.Rotate(new Vector3(0, 0, -0.7f));
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, rotZ);
                }
                BulletFire();
            }// 활 원위치 하는 것
        }
    }

    void BulletFire()
    {
        if(Time.time >= lastTime + unit_Attack_Delay)
        {
            Instantiate(Bullet, transform.position, transform.rotation, Bullet_List);
            unit.Shoot_Stamina();
            lastTime = Time.time;
        };
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
