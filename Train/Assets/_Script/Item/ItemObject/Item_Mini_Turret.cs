using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Mini_Turret : MonoBehaviour
{
    bool Target_Flag;
    public GameObject MiniTurret;
    public Transform FireObject;
    public Transform BulletObject;
    Transform Target;
    Transform BulletList;
    float train_Attack_Delay;
    float lastTime;
    public float Z;
    public bool Default_Turret;

    void Start()
    {
        Target_Flag = false;
        BulletObject.GetComponent<Bullet>().atk = 15;
        train_Attack_Delay = 0.25f;
        BulletList = GameObject.Find("Bullet_List").transform;
        lastTime = 0;
        if (!Default_Turret)
        {
           Destroy(MiniTurret, 20f);
        }
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
            float rotZ;
            if (rot.x > 0)
            { 
                rotZ = Mathf.Atan2(rot.y +0.15f, rot.x) * Mathf.Rad2Deg;
            }
            else
            {
                rotZ = Mathf.Atan2(rot.y -0.135f, rot.x) * Mathf.Rad2Deg;
            }

            Debug.Log(rot);

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
                transform.Rotate(new Vector3(0, 0, 2.5f));
            }
            else if (Z < -1)
            {
                transform.Rotate(new Vector3(0, 0, -2.5f));
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
            Instantiate(BulletObject, FireObject.position, FireObject.rotation, BulletList);
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
