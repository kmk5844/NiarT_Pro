using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Item_MiniDron : MonoBehaviour
{
    public enum MiniDronType { 
        DefaultDron,
        LaserDron,
        MissileDron,
        DeffensDron,
    }

    public MiniDronType type;
    float UpDown;
    public int DronAtk;
    public float DronSpeed;
    Rigidbody2D MiniDronRid2d;
    [SerializeField]
    GameObject SpriteObject;
    Vector2 SpriteObject_InitPos;
    public BoxCollider2D DefaultDron_BoxCollider;
    public GameObject LaserObject;
    [HideInInspector]
    public bool Laser_type;
    bool changeFlag;
    bool FireFlag;
    int FireCount;
    public GameObject MissileBullet;
    public Transform Target;
    public int DronHp;
    public ParticleSystem atkParticle;

    void Start()
    {
        FireCount = 0;
        Laser_type = false;
        if (DronAtk == 0)
        {
            DronAtk = 30;
        }
        MiniDronRid2d = GetComponent<Rigidbody2D>();
        //DefaultDron_BoxCollider = GetComponent<BoxCollider2D>();

        if (type == MiniDronType.DefaultDron)
        {
            DefaultDron_BoxCollider.enabled = true;
            if(LaserObject != null)
            {
                LaserObject.SetActive(false);
            }
            MiniDronRid2d.velocity = new Vector2(2f, 0);
        }
        else if(type == MiniDronType.LaserDron)
        {
            DefaultDron_BoxCollider.enabled = false;
            LaserObject.GetComponent<Raser_TurretBullet>().atk = DronAtk;
            LaserObject.SetActive(true);
            if (!Laser_type)
            {
                MiniDronRid2d.velocity = new Vector2(0, -0.5f);
            }
            else
            {
                MiniDronRid2d.velocity = new Vector2(0, -0.4f);
            }
        }
        else if(type == MiniDronType.MissileDron)
        {
            MiniDronRid2d.velocity = new Vector2(0, -3f);
        }else if(type == MiniDronType.DeffensDron)
        {
            DefaultDron_BoxCollider.enabled = true;
            if (LaserObject != null)
            {
                LaserObject.SetActive(false);
            }
            MiniDronRid2d.velocity = new Vector2(2f, 0);
        }

        UpDown = 1;
        //SpriteObject = transform.GetChild(0).gameObject;
        if(SpriteObject != null)
        { 
            SpriteObject_InitPos = SpriteObject.transform.localPosition;
        }
    }

    void FixedUpdate()
    {
        if(type == MiniDronType.DefaultDron)
        {
            if (SpriteObject.transform.localPosition.y > SpriteObject_InitPos.y + 1)
            {
                UpDown = -1;
            }
            else if(SpriteObject.transform.localPosition.y < SpriteObject_InitPos.y- 1)
            {
                UpDown = 1;
            }
            SpriteObject.transform.position = new Vector2(transform.position.x, SpriteObject.transform.position.y);
            SpriteObject.transform.Translate(new Vector2(0,  UpDown *DronSpeed * Time.deltaTime));

            if (transform.position.x > MonsterDirector.MaxPos_Sky.x)
            {
                Destroy(gameObject, 2f);
            }
        }
        else if(type == MiniDronType.LaserDron)
        {
            SpriteObject.transform.position = new Vector2(transform.position.x, transform.position.y);

            if (transform.position.y < MonsterDirector.MinPos_Ground.y)
            {
                Destroy(gameObject, 2f);
            }
        }else if(type == MiniDronType.MissileDron)
        {
            SpriteObject.transform.position = new Vector2(transform.position.x, transform.position.y);
            if (transform.position.y < 6f && !changeFlag)
            {
                SpriteObject_InitPos = transform.position;
                changeFlag = true;
            }
            else if (changeFlag)
            {
                SpriteObject.transform.position = new Vector2(transform.position.x, SpriteObject.transform.position.y);
                MiniDronRid2d.velocity = new Vector2(0, UpDown* 3f);
                float distance = transform.position.y - SpriteObject_InitPos.y;

                if (distance > 1f)
                {
                    UpDown = -1;
                }
                else if (distance < -1f)
                {
                    UpDown = 1;
                }

                if (FireCount < 5)
                {
                    if (!FireFlag && Target)
                    {
                        StartCoroutine(MissileFire());
                    }
                }
                else
                {
                    MiniDronRid2d.velocity = new Vector2(-5f, UpDown * 3f);
                    if(transform.position.x < SpriteObject_InitPos.x -20f)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        else if (type == MiniDronType.DeffensDron)
        {
            MiniDronRid2d.velocity = new Vector2(UpDown * 5f, 0);
/*            SpriteObject.transform.position = new Vector2(transform.position.x, SpriteObject.transform.position.y);
            SpriteObject.transform.Translate(new Vector2(0, UpDown * 3f * Time.deltaTime)); // 오른쪽 경계에 도달하면 왼쪽으로 방향 전환*/
            if (transform.position.x > MonsterDirector.MaxPos_Sky.x-2f) { UpDown = -1; } // 왼쪽 경계에 도달하면 오른쪽으로 방향 전환
            else if (transform.position.x < MonsterDirector.MinPos_Sky.x + 2f) { UpDown = 1; }
        }
    }

    IEnumerator MissileFire()
    {
        FireFlag = true;
        atkParticle.Play();
        GameObject missile = Instantiate(MissileBullet, transform.position, Quaternion.identity);
        missile.GetComponent<Missile_TurretBullet>().atk = 50;
        missile.GetComponent<Missile_TurretBullet>().monster_target = Target;
        FireCount++;
        yield return new WaitForSeconds(0.5f);
        FireFlag = false;
    }

    public void DeffeceDronSet(int hp)
    {
        DronHp = hp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(type == MiniDronType.DeffensDron && collision.CompareTag("Monster_Bullet"))
        {
            Instantiate(atkParticle, collision.transform.position, Quaternion.identity);
            if (DronHp > 0)
            {
                DronHp -= collision.GetComponent<MonsterBullet>().atk;
            }
            else
            {
                Destroy(gameObject);
            }
            Destroy(collision.gameObject);
        }

        if (type == MiniDronType.MissileDron && changeFlag)
        {
            if (collision.CompareTag("Monster"))
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
        if (type == MiniDronType.MissileDron && changeFlag)
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (type == MiniDronType.MissileDron && changeFlag)
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
}
