using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    public int Monster_Num;
    [Header("게임디렉터의 게임타입")]
    [SerializeField]
    protected Monster_GameType monster_gametype;
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

    GameObject HitDamage;
    [Header("서서히 만드는 스프라이트")]
    public List<SpriteRenderer> sprite_List;

    GameObject player; //플레이어 위치에 따라 플립하는 경우.
    GameDirector gameDirector; // 리워드 접수해야함.

    protected float EndTime;
    protected bool EndFlag;
    protected bool DestoryFlag;
    float End_Delay;

    protected virtual void Start()
    {
        lastTime = Time.time;
        monster_gametype = Monster_GameType.Fighting;
        bossLastTime = 0f;
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        HitDamage = Resources.Load<GameObject>("Monster/Hit_Text");

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
        End_Delay = Random.Range(0f, 1.5f);
        EndFlag = false;
        DestoryFlag = false;
        sprite_List.Add(GetComponent<SpriteRenderer>());
        if(transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                sprite_List.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
            }
        }
    }

    protected void BulletFire(int x_scale = 0)
    {
        if (Time.time >= lastTime + Bullet_Delay)
        {
            GameObject bullet = Instantiate(Bullet, transform.position, transform.rotation, monster_Bullet_List);
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, Target, x_scale);
            lastTime = Time.time;
        }
    } // 공통적으로 적용해야 함. -> 변경 예정

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

    protected void Total_GameType()
    {
        if (gameDirector.gameType == GameType.Ending)
        {
            if(monster_gametype != Monster_GameType.GameEnding)
            {
                monster_gametype = Monster_GameType.GameEnding;
                EndTime = Time.time;
            }
        }
    }

    protected void Monster_Ending()
    {
        if(Time.time > EndTime + End_Delay)
        {
            if (!EndFlag)
            {
                EndFlag = true;
                EndTime = Time.time;
            }

            float elapsedTime = Time.time - EndTime;
            float alphaPercent = Mathf.Clamp01(elapsedTime / 2f);

            foreach(SpriteRenderer sprite in sprite_List)
            {
                sprite.color = new Color(1, 1, 1, 1 - alphaPercent);
            }

            if(elapsedTime >= 1f && !!DestoryFlag)
            {
                DestoryFlag = true;
                Destroy(gameObject);
            }   
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // 공통적으로 적용해야됨.
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            HitDamage.GetComponent<Hit_Text_Damage>().damage = collision.gameObject.GetComponent<Bullet>().atk;
            HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
            HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
            Instantiate(HitDamage);
            if (Monster_HP > 0)
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

public enum Monster_GameType{
    SpwanStart,
    Fighting,
    Die,
    GameEnding,
}