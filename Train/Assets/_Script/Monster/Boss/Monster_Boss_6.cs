using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Coffee.UIExtensions.UIParticleAttractor;

public class Monster_Boss_6 : Boss
{
    [Header("보스 개인 설정")]
    public Transform Fire_Zone;
    GameObject player;
    float Speed;

    Vector2 MonsterDirector_Pos;
    public Transform WorkTrailObject;

    [SerializeField]
    Boss_PlayType playType;

    float move_xPos;
    Vector3 movement;
    float move_speed;

    int skillNum;
    float move_lastTime;
    float move_delayTime;
    float attack_lastTime;
    float attack_delayTime;

    bool SpawnFlag = false;
    bool attackflag = false;

    public ParticleSystem DieEffect;
    ParticleSystem[] alldieEffect;
    Vector3 DieEffectOriginPos;
    bool dieEffectFlag;
    int dieCount;

    [Header("스킬1-폭탄")]
    public GameObject BombObject;

    [Header("스킬2-화염방사기")]
    public GameObject FireObject;

    [Header("스킬3-화염구 내리기")]
    public GameObject FireBallObject_Down;

    [Header("스킬5-화염구 날리기")]
    public GameObject FireBallObject_Player;
    public Transform FireBall_FireZone1;
    public Transform FireBall_FireZone2;

    [Header("스킬6-불꽃나비 날리기")]
    public GameObject FireButterflyObject;

    [Header("스킬7-메테오")]
    public GameObject MeteorObject;

    [Header("스킬8-레이저")]
    public GameObject LaserObject_0;
    public GameObject LaserObject_1;

    protected override void Start()
    {
        Boss_Num = 6;
        base.Start();
        player = GameObject.FindWithTag("Player");

        MonsterDirector_Pos = new Vector2(MonsterDirector.MaxPos_Ground.x + 20f, MonsterDirector.MinPos_Ground.y + 4f);
        transform.localPosition = MonsterDirector_Pos;

        Speed = 4f;

        DieEffectOriginPos = DieEffect.transform.localPosition;
        alldieEffect = DieEffect.GetComponentsInChildren<ParticleSystem>();

        move_delayTime = 10f;
        attack_delayTime = 2f;

        col.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        Fire_Debuff();
        FlipMonster();

        if (playType == Boss_PlayType.Spawn)
        {
            if (!SpawnFlag)
            {
                StartCoroutine(SpawnCorutine());
            }
        }

        if (playType == Boss_PlayType.Move)
        {
            if (MonsterDirector_Pos.x - 2 > transform.position.x)
            {
                move_xPos = 1f;
                move_speed = Random.Range(5f, 7f);
            }
            else if (MonsterDirector_Pos.x + 1 < transform.position.x)
            {
                move_xPos = -1f;
                move_speed = Random.Range(2f, 4f);
            }
            movement = new Vector3(move_xPos, 0f, 0f);
            transform.Translate(movement * move_speed * Time.deltaTime);

            if (Time.time >= attack_lastTime + attack_delayTime && !attackflag)
            {
                StartCoroutine(BulletFire());
            }

            if (Time.time >= move_lastTime + move_delayTime - 1.2f)
            {
                if (!skilleffect_flag)
                {
                    SkillEffect.Play();
                    skilleffect_flag = true;
                }
            }

            if (Time.time >= move_lastTime + move_delayTime)
            {
                playType = Boss_PlayType.SKill;
            }
        }

        if (playType == Boss_PlayType.SKill)
        {
            skillNum = Random.Range(0, 8);

            if (skillNum == 0)
            {
                StartCoroutine(Skill_0());
            }
            else if (skillNum == 1)
            {
                //화염방사기
                StartCoroutine(Skill_1());
            }
            else if (skillNum == 2)
            {
                StartCoroutine(Skill_2());
                //화염구 내리기
            }
            else if (skillNum == 3)
            {
                //긴급 수리
                StartCoroutine(Skill_3());
            }
            else if (skillNum == 4)
            {
                //화염구 날리기
                StartCoroutine(Skill_4());
            }
            else if (skillNum == 5)
            {
                //불꽃나비 날리기
                StartCoroutine(Skill_5());
            }
            else if (skillNum == 6)
            {
                //메테오
                StartCoroutine(Skill_6());
            }
            else if (skillNum == 7)
            {
                //레이저 발사
                StartCoroutine(Skill_7());
            }
            playType = Boss_PlayType.Skill_Using;
        }

        if (playType == Boss_PlayType.Skill_Using)
        {
            //스킬 사용 중
        }

        if (DieFlag)
        {
            if(playType != Boss_PlayType.Die)
            {
                playType = Boss_PlayType.Die;
                col.enabled = false;
                Destroy(gameObject, 10f);
            }
        }


        if (playType == Boss_PlayType.Die)
        {
            if (!dieEffectFlag && dieCount < 4)
            {
                StartCoroutine(DieCorutine());
                dieCount++;
            }
            //DieEffect.Emit(9);
            Vector3 movement = new Vector3(-10f, 0f, 0f);
            transform.Translate(movement * Time.deltaTime);
        }
    }

    void FlipMonster()
    {
        if (player.transform.position.x - transform.position.x < 0f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            WorkTrailObject.localScale = new Vector3(WorkTrailObject.localScale.x, WorkTrailObject.localScale.y, -Mathf.Abs(WorkTrailObject.localScale.z));
            //AfterImage_Particle.transform.localScale = new Vector3(-AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            WorkTrailObject.localScale = new Vector3(WorkTrailObject.localScale.x, WorkTrailObject.localScale.y, Mathf.Abs(WorkTrailObject.localScale.z));
            //AfterImage_Particle.transform.localScale = new Vector3(AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }
    }

    IEnumerator BulletFire()
    {
        attackflag = true;
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = Instantiate(Boss_Bullet, Fire_Zone.position, Quaternion.identity, monster_Bullet_List);
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 15);
            yield return new WaitForSeconds(0.1f);
        }
        attack_lastTime = Time.time;
        attackflag = false;
    }

    IEnumerator SpawnCorutine()
    {
        SpawnFlag = true;

        while (true)
        {
            transform.Translate(-12f * Time.deltaTime, 0, 0);
            if (transform.position.x < MonsterDirector.MinPos_Ground.x - 7f)
            {
                MonsterDirector_Pos = transform.position;
                playType = Boss_PlayType.Move;
                break;
            }
            yield return null;
        }

        col.enabled = true;
        move_xPos = -1f;
        move_speed = Random.Range(1f, 2f);
        move_lastTime = Time.time;
        attack_lastTime = Time.time;
        playType = Boss_PlayType.Move;
    }

    IEnumerator Skill_0()
    {
        bool posFlag = false;
        float moveSpeed = 20f;
        float bombTimer = 0f;

        while (true)
        {
            if (!posFlag) {
                if (transform.position.x < MonsterDirector.MaxPos_Ground.x + 10f)
                {
                    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                }
                else
                {
                    posFlag = true;
                }
            }
            else
            {
                if (transform.position.x > MonsterDirector_Pos.x)
                {
                    transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                }
                else
                {
                    break;
                }
            }

            bombTimer += Time.deltaTime;

            if(bombTimer >= 0.2f)
            {
                if(transform.position.x < MonsterDirector.MaxPos_Ground.x && transform.position.x > MonsterDirector.MinPos_Ground.x)
                {
                    bombTimer = 0f;
                    GameObject Bullet = Instantiate(BombObject, new Vector2(transform.position.x, MonsterDirector.MinPos_Ground.y + 0.5f), Quaternion.identity);
                    Bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk / 2, 0f, 0f);
                }
            }

            yield return null;
        }

        ToMove();
    }

    IEnumerator Skill_1()
    {
        bool posFlag = false;
        float moveSpeed = 10;
        FireObject.GetComponent<MonsterBullet>().Get_MonsterBullet_Information((Bullet_Atk /2), 0f, 0f);
        FireObject.gameObject.SetActive(true);

        while (true)
        {
            if (!posFlag)
            {
                if (transform.position.x < MonsterDirector.MaxPos_Ground.x + 10f)
                {
                    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                }
                else
                {
                    posFlag = true;
                }
            }
            else
            {
                if (transform.position.x > MonsterDirector_Pos.x)
                {
                    transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                }
                else
                {
                    break;
                }
            }
            yield return null;
        }
        FireObject.gameObject.SetActive(false);
        ToMove();
    }

    IEnumerator Skill_2()
    {
        for(int i = 0; i < 20; i++)
        {
            float randomXPos = Random.Range(-2f, 2f);
            Vector2 pos = new Vector2(player.transform.position.x + randomXPos, MonsterDirector.MaxPos_Sky.y + 4f);
            GameObject fireBall = Instantiate(FireBallObject_Down, pos, Quaternion.identity);
            fireBall.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk / 2, 1f, Random.Range(8f, 17f));
            yield return new WaitForSeconds(0.1f);
        }

        ToMove();
    }

    IEnumerator Skill_3()
    {
        gameDirector.BossHeal(true);
        int heal = Monster_Max_HP * 10 / 100;

        Monster_HP += heal;
        if (Monster_HP > Monster_Max_HP)
        {
            Monster_HP = Monster_Max_HP;
        }
        yield return new WaitForSeconds(0.5f);
        gameDirector.BossHeal(false);
        ToMove();
    }

    IEnumerator Skill_4()
    {
        for(int i = 0; i < 25; i++)
        {
            GameObject Bullet_;
            if(i % 2 == 0)
            {
                Bullet_ = Instantiate(FireBallObject_Player, FireBall_FireZone1.position, Quaternion.identity);
            }
            else
            {
                Bullet_ = Instantiate(FireBallObject_Player, FireBall_FireZone2.position, Quaternion.identity);
            }
            Bullet_.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 25);
            yield return new WaitForSeconds(0.05f);
        }
        ToMove();
    }

    IEnumerator Skill_5()
    {
        for(int i = 0; i < 10; i++)
        {
            Vector2 pos = new Vector2(Fire_Zone.position.x, Fire_Zone.position.y + Random.Range(-3f, -1f));
            GameObject bullet = Instantiate(FireButterflyObject, pos, Quaternion.identity);
            bullet.GetComponent<Monster_ButterFly_Bullet>().Get_MonsterBullet_Information(Bullet_Atk / 2, 1f, Random.Range(15f,20f), 1);
            yield return new WaitForSeconds(0.1f);
        }
        ToMove();
    }

    IEnumerator Skill_6()
    {
        GameObject Bullet = Instantiate(MeteorObject, new Vector2(Fire_Zone.position.x + 4f, Fire_Zone.position.y + 6f), Quaternion.identity);
        Bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk * 4, 20f, 3f);
        yield return new WaitForSeconds(10f);
        ToMove();
    }

    IEnumerator Skill_7()
    {
        GameObject bullet_ = null;

        for (int i = 0; i < 8; i++)
        {
            int bulletNum = Random.Range(0, 2);
            if (bulletNum == 0)
            {
                bullet_ = Instantiate(LaserObject_0, new Vector2(Fire_Zone.position.x, Fire_Zone.position.y + Random.Range(-2f, 2f)), Quaternion.identity);
            }
            else if (bulletNum == 1)
            {
                bullet_ = Instantiate(LaserObject_1, new Vector2(Fire_Zone.position.x, Fire_Zone.position.y + Random.Range(-2f, 2f)), Quaternion.identity);
            }
            bullet_.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk * 3, 0f, 15f);
            yield return new WaitForSeconds(1f);
        }

        ToMove();
    }

    IEnumerator DieCorutine()
    {
        dieEffectFlag = true;
        Vector2 diePos = new Vector2(
           DieEffectOriginPos.x + Random.Range(-2f, 2f),
           DieEffectOriginPos.y + Random.Range(-4.5f, 3.5f)
        );
        DieEffect.transform.localPosition = diePos;

        foreach (var ps in alldieEffect)
        {
            var emission = ps.emission;

            ParticleSystem.Burst burst = emission.GetBurst(0);
            float burstCount = burst.count.constant; // 인스펙터 Burst에 적힌 값

            ps.Emit((int)burstCount);
        }

        float EmitTime = Random.Range(0.1f, 0.2f);
        yield return new WaitForSeconds(EmitTime); // 0.2초 간격으로 터짐
        dieEffectFlag = false;
    }


    private void ToMove()
    {
        if (playType != Boss_PlayType.Die)
        {
            move_delayTime = Random.Range(4f, 8f);
            skilleffect_flag = false;
            attack_lastTime = Time.time;
            move_lastTime = Time.time;
            //ResetAni();
            playType = Boss_PlayType.Move;
        }
    }
    enum Boss_PlayType
    {
        Spawn,
        Move,
        Move_Wait,
        SKill,
        Skill_Using,
        Die
    }
}