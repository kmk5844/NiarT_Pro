using JetBrains.Annotations;
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
    //float bossLastTime;
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

    float raser_hit_time;
    float fire_hit_time;
    public bool fire_debuff_flag;
    bool fire_hit_flag;
    int fire_hit_damage;
    int fire_hit_Count;
    int mercenary_atk;

    //Item 부분
    bool Item_ChangeFlag;
    int Item_Persent;

    float Item_Monster_Atk;
    float Item_Monster_AtkDelay;
    protected float Item_Monster_Speed;
    protected bool Item_Monster_ChangeFlag;
    protected bool Item_Mosnter_SpeedFlag;
    protected int Item_Mosnter_SpeedPersent;

    protected virtual void Start()
    {
        lastTime = Time.time;
        mercenary_atk = 0;
        monster_gametype = Monster_GameType.Fighting;
        //bossLastTime = 0f;
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
        Item_Mosnter_SpeedFlag = false;

        if (MonsterDirector.Item_curseFlag)
        {
            Item_Persent = MonsterDirector.Item_cursePersent_Spawn;
            Item_Monster_Atk += Bullet_Atk * (Item_Persent / 100f);
            Item_Monster_AtkDelay += Bullet_Delay * (Item_Persent / 100f);
            Item_Monster_ChangeFlag = true;
            Item_Mosnter_SpeedFlag = true;
            Item_Mosnter_SpeedPersent = Item_Persent;
        }
    }

    protected virtual void Update()
    {
        if (Item_ChangeFlag)
        {
            if (MonsterDirector.Item_curseFlag)
            {
                Item_Monster_Atk += Bullet_Atk * (Item_Persent / 100f);
                Item_Monster_AtkDelay += Bullet_Delay * (Item_Persent / 100f);
                Item_Monster_ChangeFlag = true;
                Item_Mosnter_SpeedFlag = true;
                Item_Mosnter_SpeedPersent = Item_Persent;

                Item_ChangeFlag = false;
            }
            else 
            {
                Item_Monster_Atk -= Bullet_Atk * (Item_Persent / 100f);
                Item_Monster_AtkDelay -= Bullet_Delay * (Item_Persent / 100f);
                Item_Monster_ChangeFlag = true;
                Item_Mosnter_SpeedFlag = false;
                Item_Mosnter_SpeedPersent = 100;

                Item_ChangeFlag = false;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if(monster_gametype == Monster_GameType.CowBoy_Debuff)
        {
            if(transform.position.y > 0.5f)
            {
                transform.Translate(Vector3.down * 0.2f * Time.deltaTime);
            }
        }
    }

    protected void Fire_Debuff()
    {
        if (fire_debuff_flag)
        {
            if(fire_hit_Count < 6) // 변경예정
            {
                if (!fire_hit_flag) {
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

    protected void BulletFire(int x_scale = 0)
    {
        if (Time.time >= lastTime + (Bullet_Delay + Item_Monster_AtkDelay))
        {
            GameObject bullet = Instantiate(Bullet, transform.position, transform.rotation, monster_Bullet_List);
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Target, x_scale);
            Debug.Log((Bullet_Delay + Item_Monster_AtkDelay));
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

    public void grapTrigger()
    {
        monster_gametype = Monster_GameType.CowBoy_Debuff;
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
            gameDirector.Game_Monster_Kill(Monster_Score, Monster_Coin);
            Destroy(gameObject);
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
            gameDirector.Game_Monster_Kill(Monster_Score, Monster_Coin);
            Destroy(gameObject);
        }
    }

    private void Damage_Monster_Trigger_Mercenary(Collider2D collision)
    {
        if(collision.GetComponentInParent<Mercenary>().Type == mercenaryType.Short_Ranged)
        {
            mercenary_atk = collision.GetComponentInParent<Short_Ranged>().unit_Attack;
        }
        HitDamage.GetComponent<Hit_Text_Damage>().damage = mercenary_atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP  - mercenary_atk > 0)
        {
            Monster_HP -= mercenary_atk;
        }
        else
        {
            gameDirector.Game_Monster_Kill(Monster_Score, Monster_Coin);
            Destroy(gameObject);
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
            gameDirector.Game_Monster_Kill(Monster_Score, Monster_Coin);
            Destroy(gameObject);
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
            gameDirector.Game_Monster_Kill(Monster_Score, Monster_Coin);
            Destroy(gameObject);
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
            if (collision.gameObject.name.Equals("Short_AttackCollider")){
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
            if(Time.time > raser_hit_time + 0.5f)
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
            fire_hit_damage = collision.gameObject.GetComponent<Bullet>().atk/2;
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

    //아이템 부분
    public void Item_Monster_CureseFlag(int Persent)
    {
        Item_ChangeFlag = true;
        Item_Persent = Persent;
    }
}

public enum Monster_GameType{
    SpwanStart,
    Fighting,
    CowBoy_Debuff,
    Die,
    GameEnding,
}