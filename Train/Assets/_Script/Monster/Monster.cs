using JetBrains.Annotations;
using Language.Lua;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Monster : MonoBehaviour
{
    Collider2D col;
    protected int Monster_Num;
    public int GetMonsterNum() { return Monster_Num; }
    protected SpriteRenderer monsterSprite;
    Material monsterMat;
    [Header("몬스터 게임타입")]
    [SerializeField]
    public Monster_GameType monster_gametype;
    [Header("데이터 모음")]
    public Game_DataTable EX_GameData;
    public SA_Monster monsterData;

    [Header("몬스터 정보")]
    [SerializeField]
    protected string Monster_Name;
    [SerializeField]
    protected int Monster_HP;
    [SerializeField]
    protected int Monster_Score;
    [SerializeField]
    protected int Monster_Coin;
    [SerializeField]
    protected bool Monster_CountFlag;
    protected Coroutine Monster_coroutine;
    protected bool Monster_TrainSlowType = false;

    [Header("미션")]
    public bool Monster_Mission_MaterialFlag = false;
    public bool Monster_Mission_CountFlag = false;
    bool spawnMaterialFlag = false;
    [SerializeField]
    ItemDataObject material_Item = null;
    GameObject MaterialObject;
    [SerializeField]
    protected int material_drop;

    public string Monster_Type;
    protected Vector2 MonsterDirector_Pos; //몬스터 디렉터에게 받고 지정된 위치
    protected Vector2 Spawn_Init_Pos;  // 스폰 후 초기 위치

    [Header("몬스터 총알 정보")]
    [SerializeField]
    protected GameObject BulletObject;
    [SerializeField]
    public int Bullet_Atk;
    protected float Bullet_Speed;
    protected float Bullet_Delay;
    public float Bullet_Slow;
    protected float lastTime;
    protected Transform monster_Bullet_List;
    protected bool warningFlag;
    public ParticleSystem WarningEffect;
    [SerializeField]
    protected Transform Fire_Zone;

    GameObject HitDamage;

/*    [Header("잔상")]
    public GameObject AfterImage_Particle;
    protected float AfterImage_Particle_LocalScale_X;
    protected float AfterImage_Particle_LocalScale_Y;*/

    GameObject Monster_Kill_Particle;

    protected GameObject player; //플레이어 위치에 따라 플립하는 경우.
    protected GameDirector gameDirector; // 리워드 접수해야함. + slow포함

    protected float EndTime;
    protected bool EndFlag;
    protected bool DieFlag;
    protected bool DestoryFlag;

    float laser_hit_time;
    float fire_hit_time;
    public bool fire_debuff_flag;
    bool fire_hit_flag;
    int fire_hit_damage;
    int fire_hit_Count;
    int mercenary_atk;

    //Item 부분
    bool Item_Curese_ChangeFlag;
    int Item_Curese_Persent;

    [SerializeField]
    protected float Item_Monster_Atk;
    [SerializeField]
    protected float Item_Monster_AtkDelay;
    protected float Item_Monster_Speed;
    protected bool Item_Monster_Speed_ChangeFlag;
    protected bool Item_Monster_SpeedFlag;
    protected int Item_Mosnter_SpeedPersent;

    bool Item_Giant_ChangeFlag;
    bool Item_Giant_ChangeFlag_Scale;
    int Item_Giant_Persent;

    float default_LocalScale_X;
    float default_LocalScale_Y;
    float default_LocalScale_Z;

    public GameObject StunEffect;
    public GameObject WalkEffect;

    public float monster_xPos;
    AudioClip DieSfX;

    MonsterEffect monsterEffect;

    //무한모드
    [SerializeField]
    bool InfiniteMode;
    [SerializeField]
    int InfiniteCount;
    [SerializeField]
    bool Infinite_Status_ChagngeFlag = false;

    protected virtual void Start()
    {
        lastTime = Time.time;
        mercenary_atk = 0;
        monster_xPos = 0;
        monster_gametype = Monster_GameType.SpwanStart;
        col = GetComponent<Collider2D>();

        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        HitDamage = Resources.Load<GameObject>("Monster/Hit_Text");
        Monster_Kill_Particle = Resources.Load<GameObject>("Monster/Monster_Kill_Effect");
        DieSfX = Resources.Load<AudioClip>("Sound/SFX/Monster_Die_SFX");
        monsterEffect = GetComponentInChildren<MonsterEffect>();

        monsterSprite = GetComponent<SpriteRenderer>();
        monsterMat = monsterSprite.material;

        Monster_Name = EX_GameData.Information_Monster[Monster_Num].Monster_Name;
        Monster_HP = EX_GameData.Information_Monster[Monster_Num].Monster_HP;
        Monster_Score = EX_GameData.Information_Monster[Monster_Num].Monster_Score;
        Monster_Coin = EX_GameData.Information_Monster[Monster_Num].Monster_Coin;
        Monster_Type = EX_GameData.Information_Monster[Monster_Num].Monster_Type;
        if(Monster_Type == "Slow")
        {
            Monster_TrainSlowType = true;
        }
        Monster_CountFlag = EX_GameData.Information_Monster[Monster_Num].Monster_CountFlag;
        Bullet_Atk = EX_GameData.Information_Monster[Monster_Num].Monster_Atk;
        Bullet_Speed = EX_GameData.Information_Monster[Monster_Num].Monster_Bullet_Speed;
        Bullet_Delay = EX_GameData.Information_Monster[Monster_Num].Monster_Bullet_Delay;
        Bullet_Slow = EX_GameData.Information_Monster[Monster_Num].Monster_Bullet_Slow;


        if (Monster_Mission_MaterialFlag)
        {
            MaterialObject = Resources.Load<GameObject>("Monster/SupplyMonster_Item");
        }

        monster_Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();

        //AfterImage_Particle_LocalScale_X = AfterImage_Particle.transform.localScale.x;
        //AfterImage_Particle_LocalScale_Y = AfterImage_Particle.transform.localScale.y;

        player = GameObject.FindGameObjectWithTag("Player");
        EndFlag = false;
        DieFlag = false;
        DestoryFlag = false;

        default_LocalScale_X = transform.localScale.x;
        default_LocalScale_Y = transform.localScale.y;
        default_LocalScale_Z = transform.localScale.z;


        //아이템 부분

        Item_Curese_ChangeFlag = false;
        Item_Giant_ChangeFlag = false;
        Item_Giant_ChangeFlag_Scale = false;
        Item_Monster_SpeedFlag = false;


        if (MonsterDirector.Item_curseFlag)
        {
            Item_Curese_Persent = MonsterDirector.Item_cursePersent_Spawn;
            Item_Monster_Atk += Bullet_Atk * (Item_Curese_Persent / 100f);
            Item_Monster_AtkDelay += Bullet_Delay * (Item_Curese_Persent / 100f);
            Item_Mosnter_SpeedPersent = Item_Curese_Persent;
            Item_Monster_Speed_ChangeFlag = true;
            Item_Monster_SpeedFlag = true;
        }

        if (MonsterDirector.Item_giantFlag)
        {
            Item_Giant_ChangeFlag_Scale = false;
            transform.localScale = new Vector3(
                default_LocalScale_X + 0.7f,
                default_LocalScale_Y + 0.7f,
                default_LocalScale_Z + 0.7f
                );
            Item_Giant_Persent = MonsterDirector.Item_giantPersent_Spawn;
            Item_Mosnter_SpeedPersent = Item_Giant_Persent;
            Item_Monster_Speed_ChangeFlag = true;
            Item_Monster_SpeedFlag = true;
        }
        StunEffect.SetActive(false);
    }

    protected virtual void Update()
    {
        if (InfiniteMode)
        {
            if (!Infinite_Status_ChagngeFlag)
            {
                Monster_HP = Monster_HP + (InfiniteCount * 5);
                Bullet_Atk = Bullet_Atk + (InfiniteCount * 10);
                Bullet_Slow = Bullet_Slow + (InfiniteCount * 0.1f);
                Infinite_Status_ChagngeFlag = true;
            }
        }

        if(Monster_HP <= 0)
        {
            monster_gametype = Monster_GameType.Die;
        }

        if (Monster_Mission_MaterialFlag)
        {
            if (Monster_HP <= 0 && !spawnMaterialFlag && Monster_Num != 1)
            {
                if (Random.value <= material_drop / 100f) //material_drop이하면, 재료 아이템을 떨어뜨린다.
                {
                    MaterialObject.GetComponent<SupplyMonster_Item>().ChangeMaterial(material_Item);
                    Instantiate(MaterialObject, transform.position, Quaternion.identity);
                }
                spawnMaterialFlag = true;
            }
        }

        if (Item_Curese_ChangeFlag)
        {
            if (MonsterDirector.Item_curseFlag)
            {
                Item_Monster_Atk += Bullet_Atk * (Item_Curese_Persent / 100f);
                Item_Monster_AtkDelay += Bullet_Delay * (Item_Curese_Persent / 100f);
                Item_Mosnter_SpeedPersent = Item_Curese_Persent;
                Item_Monster_Speed_ChangeFlag = true;
                Item_Monster_SpeedFlag = true;

                Item_Curese_ChangeFlag = false;
            }
            else
            {
                Item_Monster_Atk -= Bullet_Atk * (Item_Curese_Persent / 100f);
                Item_Monster_AtkDelay -= Bullet_Delay * (Item_Curese_Persent / 100f);
                Item_Mosnter_SpeedPersent = Item_Curese_Persent;
                Item_Monster_Speed_ChangeFlag = true;
                Item_Monster_SpeedFlag = false;

                Item_Curese_ChangeFlag = false;
            }
        }

        if (Item_Giant_ChangeFlag)
        {
            if (MonsterDirector.Item_giantFlag)
            {
                Item_Mosnter_SpeedPersent = Item_Giant_Persent;
                Item_Monster_Speed_ChangeFlag = true;
                Item_Monster_SpeedFlag = true;

                Item_Giant_ChangeFlag = false;
            }
            else
            {
                Item_Mosnter_SpeedPersent = Item_Giant_Persent;
                Item_Monster_Speed_ChangeFlag = true;
                Item_Monster_SpeedFlag = false;

                Item_Giant_ChangeFlag = false;
            }
        }

        if (Item_Giant_ChangeFlag_Scale)
        {
            if (MonsterDirector.Item_giantFlag)
            {
                if (transform.localScale.y < default_LocalScale_Y + 0.7f)
                {
                    if (transform.localScale.x > 0)
                    {
                        transform.localScale = new Vector3(
                            transform.localScale.x + (0.08f * Time.deltaTime),
                            transform.localScale.y + (0.08f * Time.deltaTime),
                            transform.localScale.z + (0.08f * Time.deltaTime)
                            );
                    }
                    else
                    {
                        transform.localScale = new Vector3(
                            transform.localScale.x - (0.08f * Time.deltaTime),
                            transform.localScale.y + (0.08f * Time.deltaTime),
                            transform.localScale.z + (0.08f * Time.deltaTime)
                            );
                    }
                }
                else
                {
                    Item_Giant_ChangeFlag_Scale = false;
                    transform.localScale = new Vector3(
                        default_LocalScale_X + 0.7f,
                        default_LocalScale_Y + 0.7f,
                        default_LocalScale_Z + 0.7f
                        );
                }
            }
            else
            {
                if (transform.localScale.y > default_LocalScale_Y)
                {
                    if (transform.localScale.x > 0)
                    {
                        transform.localScale = new Vector3(
                            transform.localScale.x - (0.08f * Time.deltaTime),
                            transform.localScale.y - (0.08f * Time.deltaTime),
                            transform.localScale.z - (0.08f * Time.deltaTime)
                            );
                    }
                    else
                    {
                        transform.localScale = new Vector3(
                            transform.localScale.x + (0.08f * Time.deltaTime),
                            transform.localScale.y - (0.08f * Time.deltaTime),
                            transform.localScale.z - (0.08f * Time.deltaTime)
                            );
                    }
                }
                else
                {
                    Item_Giant_ChangeFlag_Scale = false;
                    transform.localScale = new Vector3(
                        default_LocalScale_X,
                        default_LocalScale_Y,
                        default_LocalScale_Z
                        );
                }
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (monster_gametype == Monster_GameType.CowBoy_Debuff)
        {
            if (transform.position.y > 0.5f)
            {
                transform.Translate(Vector3.down * 0.2f * Time.deltaTime);
            }
        }
        if (monster_gametype == Monster_GameType.Stun_Bullet_Debuff)
        { }
    }

    protected void Fire_Debuff()
    {
        if(monster_gametype != Monster_GameType.Die)
        {
            if (fire_debuff_flag)
            {
                if (fire_hit_Count < 11) // 변경예정
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
        else
        {
            fire_debuff_flag = false;
        }

    }

    IEnumerator Fire_Hit_Corutine()
    {
        fire_hit_flag = true;
        if (fire_hit_Count != 0)
        {
            Damage_Monster_BombAndDron(fire_hit_damage);
        }
        yield return new WaitForSeconds(0.2f);
        fire_hit_Count++;
        fire_hit_flag = false;
    }

    protected void FlipMonster()
    {
        if (player.transform.position.x - transform.position.x < 0f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            //AfterImage_Particle.transform.localScale = new Vector3(-AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            //AfterImage_Particle.transform.localScale = new Vector3(AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }
    } // 공통적으로 적용해야 함

    protected void Total_GameType()
    {
        if (gameDirector.gameType == GameType.Ending)
        {
            if (monster_gametype != Monster_GameType.GameEnding)
            {
                monster_gametype = Monster_GameType.GameEnding;
                EndTime = Time.time;
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
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - hit_atk > 0)
        {
            Monster_HP -= collision.gameObject.GetComponent<Bullet>().atk;
        }
        else
        {
            if (collision.gameObject.GetComponent<Auto_TurretBullet>() != null)
            {
                if (collision.gameObject.GetComponent<Auto_TurretBullet>().MercenaryFlag)
                {
                    PlayerLogDirector.MercenaryKill(collision.gameObject.GetComponent<Auto_TurretBullet>().MercenaryNum);
                }
            }
            MonsterDie();
        }
    }
    private void Damage_Monster_Trigger(Collider2D collision)
    {
        int hit_atk;
        if (collision.gameObject.GetComponent<Bullet>())
        {
            hit_atk = collision.gameObject.GetComponent<Bullet>().atk;
        }
        else
        {
            hit_atk = collision.gameObject.GetComponent<MonsterBullet>().atk;
            hit_atk /= 4;
        }

        HitDamage.GetComponent<Hit_Text_Damage>().damage = hit_atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - hit_atk > 0)
        {
            Monster_HP -= hit_atk;
        }
        else
        {
            MonsterDie();
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
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - mercenary_atk > 0)
        {
            Monster_HP -= mercenary_atk;
        }
        else
        {
            PlayerLogDirector.MercenaryKill(collision.GetComponentInParent<Mercenary>().mercenaryNum);
            MonsterDie();
        }
    }

    public void Damage_Monster_BombAndDron(int Bomb_Atk)
    {
        HitDamage.GetComponent<Hit_Text_Damage>().damage = Bomb_Atk * 2;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - Bomb_Atk * 2 > 0)
        {
            Monster_HP -= Bomb_Atk * 2;
        }
        else
        {
            MonsterDie();
        }
    }

    public void Damage_Monster_Item(int Atk)
    {
        HitDamage.GetComponent<Hit_Text_Damage>().damage = Atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - Atk > 0)
        {
            Monster_HP -= Atk;
        }
        else
        {
            MonsterDie();
        }
    }

    private void Damage_Item_WireEntanglement(Collider2D collision)
    {
        HitDamage.GetComponent<Hit_Text_Damage>().damage = 10;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - 10 > 0)
        {
            Monster_HP -= 10;
        }
        else
        {
            MonsterDie();

        }
    }

    public void Damage_Item_Stun_Bullet(int hit_atk, int delayTime)
    {
        HitDamage.GetComponent<Hit_Text_Damage>().damage = hit_atk;
        HitDamage.GetComponent<Hit_Text_Damage>().Random_X = transform.position.x + Random.Range(-0.5f, 0.5f);
        HitDamage.GetComponent<Hit_Text_Damage>().Random_Y = transform.position.y + Random.Range(0.5f, 1.5f);
        StartCoroutine(MatHitEffect());
        Instantiate(HitDamage, monster_Bullet_List);
        if (Monster_HP - hit_atk > 0)
        {
            Monster_HP -= hit_atk;
        }
        else
        {
            MonsterDie();

        }
        StartCoroutine(Item_Stun_Debuff(delayTime));
    }

    private IEnumerator Item_Stun_Debuff(int delayTime)
    {
        if(monster_gametype != Monster_GameType.Stun_Bullet_Debuff)
        {
            Monster_GameType BeforeGameType = monster_gametype;
            monster_gametype = Monster_GameType.Stun_Bullet_Debuff;
            StunEffect.SetActive(true);
            yield return new WaitForSeconds(5);
            StunEffect.SetActive(false);
            monster_gametype = BeforeGameType;
        }
    }

    public void Item_FlashBang(int delayTime)
    {
        StartCoroutine(Item_Stun_Debuff(delayTime));
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
            if (collision.gameObject.name.Equals("Short_AttackCollider")){
                Damage_Monster_Trigger_Mercenary(collision);
            }
        }
        if (collision.gameObject.tag.Equals("Item"))
        {
            if (collision.gameObject.name.Equals("MiniDron_Col"))
            {
                int atk = collision.GetComponentInParent<Item_MiniDron>().DronAtk;
                Damage_Monster_BombAndDron(atk);
            }
        }
        if (collision.gameObject.tag.Equals("Monster_Bullet"))
        {
            if (collision.gameObject.name.Equals("9(Clone)") && Monster_Num != 9)
            {
                Damage_Monster_Trigger(collision);
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            if (collision.gameObject.name.Equals("Laser"))
            {
                Laser_Hit(collision, true);
            }
            if (collision.gameObject.name.Equals("Fire"))
            {
                Fire_Hit(collision);
            }
            if (collision.gameObject.name.Equals("Acer_Wind"))
            {
                Laser_Hit(collision, true);
            }
        }

        if (collision.gameObject.tag.Equals("Item"))
        {
            if (collision.gameObject.name.Equals("WireEntanglement(Clone)"))
            {
                Laser_Hit(collision, false);
            }
        }
    }
    void Laser_Hit(Collider2D collision, bool RealLaser)
    {
        if (RealLaser)
        {
            if (Time.time > laser_hit_time + 0.3f)
            {
                Damage_Monster_Trigger(collision);
                laser_hit_time = Time.time;
            }
        }
        else
        {
            if(Time.time > laser_hit_time + 0.5f)
            {
                Damage_Item_WireEntanglement(collision);
                laser_hit_time = Time.time;
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


    IEnumerator MatHitEffect()
    {
        monsterMat.SetFloat("_HitEffectBlend", 0.05f);

        float duration = 0.2f; // 올라가는 시간
        float elapsed = 0f;

        // 0 → 0.5
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            monsterMat.SetFloat("_HitEffectBlend", Mathf.Lerp(0f, 0.15f, t));
            yield return null;
        }

        // 0.5에서 0으로
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            monsterMat.SetFloat("_HitEffectBlend", Mathf.Lerp(0.15f, 0f, t));
            yield return null;
        }
    }


    //아이템 부분
    public void Item_Monster_CureseFlag(int Persent)
    {
        Item_Curese_ChangeFlag = true;
        Item_Curese_Persent = Persent;
    }

    public void Item_Monster_GiantFlag(int Persent)
    {
        Item_Giant_ChangeFlag = true;
        Item_Giant_ChangeFlag_Scale = true;
        Item_Giant_Persent = Persent;
    }

    public void MonsterDie(bool CoinZero = false)
    {
        monster_gametype = Monster_GameType.Die;
        Monster_HP = 0;
        DieFlag = true;
        if (CoinZero)
        {
            Monster_Coin = 0;
        }
        bool effectFlag = gameDirector.Game_Monster_Kill(Monster_Coin);

        if (effectFlag)
        {
            monsterEffect.BounsCoinPlay();
        }

        if (Monster_TrainSlowType)
        {
            gameDirector.SetMonsterSlow(false);
        }

        MMSoundManagerSoundPlayEvent.Trigger(DieSfX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        if (Monster_Mission_CountFlag)
        {
            gameDirector.Mission_Monster_Kill();
        }
        Instantiate(Monster_Kill_Particle, transform.localPosition, Quaternion.identity);

        col.enabled = false;
        monsterData.Monster_Dic[Monster_Num].ChangeMonster(false);

        if (SteamAchievement.instance != null)
        {
            SteamAchievement.instance.Achieve("KILL_MONSTER_" + Monster_Num);
        }
        else
        {
            Debug.Log("KILL_MONSTER_" + Monster_Num);
        }

        if (gameDirector.Infinite_Mode)
        {
            gameDirector.Infinite_mosnterCount++;
        }

        start_DieCoroutine();
    }

    void start_DieCoroutine()
    {
        if(Monster_coroutine != null)
        {
            StopCoroutine(Monster_coroutine);
        }

        if (Monster_Type == "Sky")
        {
            StartCoroutine(DieCorutine(true));
        }
        else if (Monster_Type == "Ground" || Monster_Type == "Slow")
        {
            StartCoroutine(DieCorutine(false));
        }
    }

    IEnumerator DieCorutine(bool type)
    {
        float elapsedTime = 0;
        float PosX = transform.localPosition.x;
        float PosY = transform.localPosition.y;

        if (type) //하늘
        {
            float duration = 0.8f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                float xPos = Mathf.Lerp(PosX, PosX  - 20f, t);
                float yPos = Mathf.Sin(Mathf.PI * t) + Mathf.Lerp(PosY, MonsterDirector.MinPos_Ground.y -5f, t);
                transform.localPosition = new Vector2(xPos, yPos);
                yield return null;
            }
        }
        else // 땅
        {
            float duration = 0.6f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                float xPos = Mathf.Lerp(PosX, PosX - 1f, t);
                float yPos = Mathf.Sin(Mathf.PI * t) * 3 + Mathf.Lerp(PosY, MonsterDirector.MinPos_Ground.y - 5f, t);
                float zRot = Mathf.Lerp(0, 20, t);
                transform.localPosition = new Vector2(xPos, yPos);
                transform.localRotation = Quaternion.Euler(0, 0, zRot);  

                yield return null;
            }
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    public int getMonsterAtk()
    {
        return Bullet_Atk;
    }

    public float getMonsterBulletSpeed()
    {
        return Bullet_Speed;
    }


    //--------------미션--------
    public void SettingMission_Material_Monster(ItemDataObject _item , int _drop)
    {
        material_Item = _item;
        material_drop = _drop;
    }

    //--------------무한모드----------
    public void SetInfinite(int count)
    {
        InfiniteMode = true;
        InfiniteCount = count;
    }
}

public enum Monster_GameType{
    SpwanStart,
    Fighting,
    CowBoy_Debuff,
    Stun_Bullet_Debuff,
    Die,
    GameEnding, // 게임 마무리 할 때 필요함.
}