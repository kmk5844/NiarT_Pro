using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    protected int Boss_Num;
    public Game_DataTable EX_GameData;
    GameDirector gameDirector;
    protected PolygonCollider2D col;
    protected bool DieFlag;
    
    [Header("보스 정보")]
    [SerializeField]
    protected string Monster_Name;
    [SerializeField]
    protected int Monster_HP;
    protected int Monster_Max_HP;
    [SerializeField]
    protected int Monster_Score;
    [SerializeField]
    protected int Monster_Coin;
    [SerializeField]
    protected Image Boss_Guage;
    public string Monster_Type;
    public bool Boss_MissionFlag = false;

    [Header("보스 총알 정보")]
    [SerializeField]
    protected GameObject Boss_Bullet;
    protected int Bullet_Atk;
    protected float Bullet_Speed;
    protected float Bullet_Delay;
    protected float Bullet_Slow;
    protected float lastTime;
    protected Transform monster_Bullet_List;

    GameObject HitDamage;
    protected Transform player_pos;
    protected Vector3 local_Scale;

    float raser_hit_time;
    float fire_hit_time;
    public bool fire_debuff_flag;
    bool fire_hit_flag;
    int fire_hit_damage;
    int fire_hit_Count;

    protected virtual void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        HitDamage = Resources.Load<GameObject>("Monster/Hit_Text");
        DieFlag = false;

        Monster_Name = EX_GameData.Information_Boss[Boss_Num].Monster_Name;
        Monster_HP = EX_GameData.Information_Boss[Boss_Num].Monster_HP;
        Monster_Max_HP = Monster_HP;
        Monster_Score = EX_GameData.Information_Boss[Boss_Num].Monster_Score;
        Monster_Coin = EX_GameData.Information_Boss[Boss_Num].Monster_Coin;
        Monster_Type = EX_GameData.Information_Boss[Boss_Num].Monster_Type;
        Boss_Bullet = Resources.Load<GameObject>("Bullet/Monster/BossBullet/" + Boss_Num);
        Boss_Guage = gameDirector.BossGuage;
        Bullet_Atk = EX_GameData.Information_Boss[Boss_Num].Monster_Atk;
        Bullet_Speed = EX_GameData.Information_Boss[Boss_Num].Monster_Bullet_Speed;
        Bullet_Delay = EX_GameData.Information_Boss[Boss_Num].Monster_Bullet_Delay;
        Bullet_Slow = EX_GameData.Information_Boss[Boss_Num].Monster_Bullet_Slow;
        monster_Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();

        local_Scale = transform.localScale;
        col = transform.GetComponent<PolygonCollider2D>();
        player_pos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
       Boss_Guage.fillAmount = ((float)Monster_HP / (float)Monster_Max_HP);
    }
    

    private void Change_DieFlag()
    {
        DieFlag = true;
        col.enabled = false;
    }

    private void Damage_Monster_Collsion(Collision2D collision)
    {
        int hit_atk = collision.gameObject.GetComponent<Bullet>().atk;
        HitDamage.GetComponent<Hit_Text_Damage>().damage = hit_atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - hit_atk > 0)
        {
            Monster_HP -= collision.gameObject.GetComponent<Bullet>().atk;
        }
        else
        {
            BossDie();
        }
    }
    private void Damage_Monster_Trigger(Collider2D collision)
    {
        int hit_atk = collision.gameObject.GetComponent<Bullet>().atk;
        HitDamage.GetComponent<Hit_Text_Damage>().damage = hit_atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - hit_atk > 0)
        {
            Monster_HP -= collision.gameObject.GetComponent<Bullet>().atk;
        }
        else
        {
            BossDie();
        }
    }

    public void Damage_Monster_BombAndDron(int Bomb_Atk)
    {
        HitDamage.GetComponent<Hit_Text_Damage>().damage = Bomb_Atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - Bomb_Atk > 0)
        {
            Monster_HP -= Bomb_Atk;
        }
        else
        {
            BossDie();
        }
    }

    void BossDie()
    {
        if (Boss_MissionFlag && !DieFlag)
        {
            gameDirector.Mission_Monster_Kill();
        }
        gameDirector.Gmae_Boss_Kill(Monster_Score, Monster_Coin);
        Change_DieFlag();
    }

    private void OnCollisionEnter2D(Collision2D collision) // 공통적으로 적용해야됨.
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            if (!collision.gameObject.name.Equals("Ballon_Bullet_Turret(Clone)"))
            {
                if (collision.gameObject.name.Equals("Fire_Arrow(Clone)"))
                {
                    FireArrow_Hit(collision);
                }
                else if (collision.gameObject.name.Equals("Fire_Bullet(Clone)"))
                {
                    FireArrow_Hit(collision);
                }
                else
                {
                    Damage_Monster_Collsion(collision);
                }
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            if (collision.gameObject.name.Equals("Raser"))
            {
                Raser_Hit(collision, true);
            }
            if (collision.gameObject.name.Equals("Fire"))
            {
                Fire_Hit(collision);
            }
        }
    }

    void Raser_Hit(Collider2D collision, bool RealRaser)
    {
        if (RealRaser)
        {
            if (Time.time > raser_hit_time + 0.3f)
            {
                Damage_Monster_Trigger(collision);
                raser_hit_time = Time.time;
            }
        }
    }

    void Fire_Hit(Collider2D collision)
    {
        if (Time.time > fire_hit_time + 0.3f)
        {
            fire_hit_damage = collision.gameObject.GetComponent<Bullet>().atk / 2;
            if (!fire_debuff_flag)
            {
                fire_debuff_flag = true;
                fire_hit_Count = 0;
            }
            else
            {
                fire_hit_Count = 0;
            }
            Damage_Monster_Trigger(collision);
            fire_hit_time = Time.time;
        }
    }

    void FireArrow_Hit(Collision2D collision)
    {
        fire_hit_damage = collision.gameObject.GetComponent<Bullet>().atk / 2;
        if (!fire_debuff_flag)
        {
            fire_debuff_flag = true;
            fire_hit_Count = 0;
        }
        else
        {
            fire_hit_Count = 0;
        }
        Damage_Monster_Collsion(collision);
        fire_hit_time = Time.time;
    }

    protected void Fire_Debuff()
    {
        if (fire_debuff_flag)
        {
            if (fire_hit_Count < 6) // 변경예정
            {
                if (!fire_hit_flag)
                {
                    StartCoroutine(Fire_Hit_Corutine());
                }
            }
            else
            {
                fire_debuff_flag = false;
            }
        }
    }

    IEnumerator Fire_Hit_Corutine()
    {
        fire_hit_flag = true;
        if (fire_hit_Count != 0)
        {
            Damage_Monster_BombAndDron(fire_hit_damage);
        }
        yield return new WaitForSeconds(1f);
        fire_hit_Count++;
        fire_hit_flag = false;
    }
}
