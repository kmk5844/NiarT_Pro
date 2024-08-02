using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    protected int Monster_Score;
    [SerializeField]
    protected int Monster_Coin;

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
    int mercenary_atk;


    protected virtual void Start()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        HitDamage = Resources.Load<GameObject>("Monster/Hit_Text");
        DieFlag = false;

        Monster_Name = EX_GameData.Information_Boss[Boss_Num].Monster_Name;
        Monster_HP = EX_GameData.Information_Boss[Boss_Num].Monster_HP;
        Monster_Score = EX_GameData.Information_Boss[Boss_Num].Monster_Score;
        Monster_Coin = EX_GameData.Information_Boss[Boss_Num].Monster_Coin;
        Boss_Bullet = Resources.Load<GameObject>("Bullet/Monster/Boss" + Boss_Num);
        Bullet_Atk = EX_GameData.Information_Boss[Boss_Num].Monster_Atk;
        Bullet_Speed = EX_GameData.Information_Boss[Boss_Num].Monster_Bullet_Speed;
        Bullet_Delay = EX_GameData.Information_Boss[Boss_Num].Monster_Bullet_Delay;
        Bullet_Slow = EX_GameData.Information_Boss[Boss_Num].Monster_Bullet_Slow;
        monster_Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();

        local_Scale = transform.localScale;
        col = transform.GetComponent<PolygonCollider2D>();
        player_pos = GameObject.FindGameObjectWithTag("Player").transform;
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
            gameDirector.Gmae_Boss_Kill(Monster_Score, Monster_Coin);
            //
            Change_DieFlag();
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
            gameDirector.Gmae_Boss_Kill(Monster_Score, Monster_Coin);
            //
            Change_DieFlag();
        }
    }

    private void Damage_Monster_Trigger_Mercenary(Collider2D collision)
    {
        if (collision.GetComponentInParent<Mercenary>().Type == mercenaryType.Short_Ranged)
        {
            mercenary_atk = collision.GetComponentInParent<Short_Ranged>().unit_Attack;
        }
        HitDamage.GetComponent<Hit_Text_Damage>().damage = mercenary_atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - mercenary_atk > 0)
        {
            Monster_HP -= mercenary_atk;
        }
        else
        {
            gameDirector.Gmae_Boss_Kill(Monster_Score, Monster_Coin);
            //
            Change_DieFlag();
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
            gameDirector.Gmae_Boss_Kill(Monster_Score, Monster_Coin);
            //
            Change_DieFlag();
        }
    }

    private void Damage_Item_WireEntanglement(Collider2D collision)
    {
        HitDamage.GetComponent<Hit_Text_Damage>().damage = 2;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - 2 > 0)
        {
            Monster_HP -= 2;
        }
        else
        {
            gameDirector.Gmae_Boss_Kill(Monster_Score, Monster_Coin);
            //
            Change_DieFlag();
        }
    }

    public void Damage_Item_Stun_Bullet(int hit_atk, int delayTime)
    {
        HitDamage.GetComponent<Hit_Text_Damage>().damage = hit_atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - hit_atk > 0)
        {
            Monster_HP -= hit_atk;
        }
        else
        {
            gameDirector.Gmae_Boss_Kill(Monster_Score, Monster_Coin);
            //

            Change_DieFlag();
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            if (collision.gameObject.name.Equals("Short_AttackCollider"))
            {
                Damage_Monster_Trigger_Mercenary(collision);
            }
        }
        if (collision.gameObject.tag.Equals("Item"))
        {
            if (collision.gameObject.name.Equals("MiniDron(Clone)"))
            {
                int atk = collision.GetComponent<Item_MiniDron>().DronAtk;
                Damage_Monster_BombAndDron(atk);
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

        if (collision.gameObject.tag.Equals("Item"))
        {
            if (collision.gameObject.name.Equals("WireEntanglement(Clone)"))
            {
                Raser_Hit(collision, false);
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
        else
        {
            if (Time.time > raser_hit_time + 0.5f)
            {
                Damage_Item_WireEntanglement(collision);
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
    }
}
