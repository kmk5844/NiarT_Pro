using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField]
    protected float Speed;

    [SerializeField]
    public int atk;
    protected Vector3 dir;
    protected Rigidbody2D rid;

    public float slow;
    protected Transform player_target;
    protected Transform Train_List;
    protected Transform Mercenary_List;
    protected int x_scale;

    public MonsterBulletType bulletType;
    bool destoryFlag;
    float destoryTime;
    //bool targetFlag;
    protected virtual void Start()
    {
        rid = GetComponent<Rigidbody2D>();
        player_target = GameObject.FindGameObjectWithTag("Player").transform;
        Train_List = GameObject.Find("Train_List").transform;
        //Mercenary_List = GameObject.Find("Mercenary_List").transform;
        if (destoryFlag)
        {
            Destroy(gameObject, destoryTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Intercept_Bullet"))
        {
            if (!collision.name.Equals("Bomb")) // 부딪치는 순간 삭제하기 때문에, 제외시켜야한다.
            {
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
        }

        if (collision.CompareTag("Zone"))
        {
            if (collision.name.Equals("SoundZone"))
            {
                atk = 0;
                rid.velocity *= 0.5f;
            }
        }
    }

    public void Get_MonsterBullet_Information(int Monster_Atk, float Monster_Slow, float speed, int X_Scale = 0)
    {
        atk = Monster_Atk;
        slow = Monster_Slow;
        Speed = speed;
        x_scale = X_Scale;
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    public void SetDestory(float time)
    {
        destoryFlag = true;
        destoryTime = time;
    }
}

public enum MonsterBulletType
{
    Nomal,
    Poison,
}