using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Missile_Turret : MonoBehaviour
{
    public GameObject Turret;
    public float SpawnTime;
    public int Attack;
    public float Attack_Delay;

    bool Target_Flag;
    Transform BulletList;
    float lastTime;
    Transform Target;
    public GameObject BulletObject;
    public Transform FireObject;
    float Z;

    // Start is called before the first frame update
    void Start()
    {
        Target_Flag = false;
        BulletList = GameObject.Find("Bullet_List").transform;
        BulletObject.GetComponent<Missile_TurretBullet>().atk = Attack;
        lastTime = 0;
    }

    // Update is called once per frame
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
                rotZ = Mathf.Atan2(rot.y + 0.15f, rot.x) * Mathf.Rad2Deg;
            }
            else
            {
                rotZ = Mathf.Atan2(rot.y - 0.135f, rot.x) * Mathf.Rad2Deg;
            }

            //Debug.Log(rot);

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
        if (Time.time >= lastTime + Attack_Delay)
        {
            BulletObject.GetComponent<Missile_TurretBullet>().monster_target = Target;

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
