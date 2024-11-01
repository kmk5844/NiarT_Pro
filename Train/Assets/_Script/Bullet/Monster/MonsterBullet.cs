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

    //bool targetFlag;
    protected virtual void Start()
    {
        rid = GetComponent<Rigidbody2D>();
        player_target = GameObject.FindGameObjectWithTag("Player").transform;
        Train_List = GameObject.Find("Train_List").transform;
        //Mercenary_List = GameObject.Find("Mercenary_List").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Intercept_Bullet"))
        {
            if (!collision.name.Equals("Bomb")) // �ε�ġ�� ���� �����ϱ� ������, ���ܽ��Ѿ��Ѵ�.
            {
                Destroy(gameObject);
                Destroy(collision.gameObject);
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
}

public enum MonsterBulletType
{
    Nomal,
    Poison,

}