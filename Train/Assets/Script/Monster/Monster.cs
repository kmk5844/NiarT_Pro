using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    public int Monster_Num;

    [Header("데이터 모음")]
    public Game_DataTable EX_GameData;

    [Header("몬스터 정보")]
    [SerializeField]
    protected string Monster_Name;
    [SerializeField]
    protected int Monster_HP;
    [SerializeField]
    protected int Monster_Score;
    [SerializeField]
    protected int Monster_Coin;

    [Header("몬스터 총알 정보")]
    [SerializeField]
    protected GameObject Bullet;
    int Bullet_Atk;
    [SerializeField]
    protected float Bullet_Delay;
    [SerializeField]
    protected int Bullet_Slow;
    float lastTime;
    float bossLastTime;
    Transform monster_Bullet_List;

    [Header("타겟")]
    [SerializeField]
    protected string Target;

    GameObject player; //플레이어 위치에 따라 플립하는 경우.
    GameDirector gameDirector; // 리워드 접수해야함.

    protected virtual void Start()
    {
        lastTime = Time.time;
        bossLastTime = 0f;
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        Monster_Name = EX_GameData.Information_Monster[Monster_Num].Monster_Name;
        Monster_HP = EX_GameData.Information_Monster[Monster_Num].Monster_HP;
        Monster_Score = EX_GameData.Information_Monster[Monster_Num].Monster_Score;
        Monster_Coin = EX_GameData.Information_Monster[Monster_Num].Monster_Coin;
        Bullet = Resources.Load<GameObject>(EX_GameData.Information_Monster[Monster_Num].Monster_Bullet);
        Bullet_Atk = EX_GameData.Information_Monster[Monster_Num].Monster_Atk;
        Bullet_Delay = EX_GameData.Information_Monster[Monster_Num].Monster_Bullet_Delay;
        Bullet_Slow = EX_GameData.Information_Monster[Monster_Num].Monster_Bullet_Slow;
        monster_Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();
        Target = EX_GameData.Information_Monster[Monster_Num].Monster_Target;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected void BulletFire(int x_scale = 0)
    {
        if (Time.time >= lastTime + Bullet_Delay)
        {
            GameObject bullet = Instantiate(Bullet, transform.position, transform.rotation, monster_Bullet_List);
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, Target, x_scale);
            lastTime = Time.time;
        }
    } // 공통적으로 적용해야 함.

    protected void DemoBulletFire(int x_scale = 0)
    {
        if (Time.time >= lastTime + Bullet_Delay)
        {
            for(int i = 0; i < 3; i++)
            {
                GameObject bullet = Instantiate(Bullet, transform.position, transform.rotation, monster_Bullet_List);
                bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, Target, x_scale);
            }
            lastTime = Time.time;
        }
    }

    protected void FlipMonster()
    {
        if (player.transform.position.x - transform.position.x < 0f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    } // 공통적으로 적용해야 함

    private void OnCollisionEnter2D(Collision2D collision) // 공통적으로 적용해야됨.
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            if(Monster_HP > 0)
            {
                Monster_HP -= collision.gameObject.GetComponent<Bullet>().atk;
            }
            else
            {
                gameDirector.Game_Monster_Kill(Monster_Score, Monster_Coin);
                Destroy(gameObject);
            }
            Destroy(collision.gameObject);
        }
    }
}
