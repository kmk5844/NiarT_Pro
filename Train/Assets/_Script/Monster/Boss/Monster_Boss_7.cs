using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Monster_Boss_7 : Boss
{
    [Header("보스 개인 설정")]
    public Transform Fire_Zone;
    GameObject player;
    float Speed;
    public Animator ani;
    public bool aniFlag = false;

    Vector2 MonsterDirector_Pos;
    Vector2 Spawn_Init_Pos;

    [SerializeField]
    Boss_PlayType playType;

    int skillNum;
    float move_lastTime;
    float move_delayTime;
    float attack_lastTime;
    float attack_delayTime;

    bool SpawnFlag = false;
    bool attackFlag = false;

    public ParticleSystem DieEffect;
    ParticleSystem[] alldieEffect;
    Vector3 DieEffectOriginPos;
    bool dieEffectFlag;
    int dieCount;

    [Header("스킬1-머신건")]
    public GameObject MachineGunObject;
    public Transform Fire_Zone_Sp;

    [Header("스킬2-수류방사기")]
    public GameObject WaterObject;

    [Header("스킬3-수류구 내리기")]
    public GameObject WaterBallObject_Down;

    [Header("스킬5-파도 일으키기")]
    public GameObject TideObject;
    bool xScale_Flag = false;
    public Transform Tide_SpawnPos;
    public Transform Tide_SpawnPos2;

    [Header("스킬6-인공 구름")]
    public GameObject CloudObject;

    [Header("스킬7-워터볼")]
    public GameObject MeteorObject;

    [Header("스킬8-미사일 소환")]
    public GameObject MissileObject;
    public Transform Missile_SpawnPos;

    protected override void Start()
    {
        Boss_Num = 7;
        base.Start();
        player = GameObject.FindWithTag("Player");

        MonsterDirector_Pos = new Vector2(transform.position.x, MonsterDirector.MinPos_Ground.y + 4.5f);
        Spawn_Init_Pos =
                new Vector2(MonsterDirector_Pos.x + Random.Range(2f, 5f),
                MonsterDirector.MinPos_Ground.y - 5f);
        transform.localPosition = Spawn_Init_Pos;

        Speed = 1f;

        DieEffectOriginPos = DieEffect.transform.localPosition;
        alldieEffect = DieEffect.GetComponentsInChildren<ParticleSystem>();
        //playType = Boss_PlayType.Spawn;
        move_delayTime = 8f;
        attack_delayTime = 1f;

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

        if (!aniFlag)
        {
            TriggerAnimation();
            aniFlag = true;
        }

        if (playType == Boss_PlayType.Move)
        {
            if (transform.position.x < player.transform.position.x - 4 && transform.position.x < MonsterDirector.MaxPos_Ground.x-1)
            {
                transform.Translate(Vector2.right * Speed * Time.fixedDeltaTime);
                if(ani.GetBool("player"))
                    ani.SetBool("player", false);
            }
            else if (transform.position.x > player.transform.position.x + 4 && transform.position.x > MonsterDirector.MinPos_Ground.x+1)
            {
                transform.Translate(Vector2.left * Speed * Time.fixedDeltaTime);
                if(ani.GetBool("player"))
                    ani.SetBool("player", false);
            }
            else
            {
                if(!ani.GetBool("player"))
                    ani.SetBool("player", true);
            }

            if (Time.time >= attack_lastTime + attack_delayTime)
            {
                if (!attackFlag)
                {
                    StartCoroutine(BulletFire());
                }
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
                playType = Boss_PlayType.Skill;
                ResetAni();
            }
        }

        if (playType == Boss_PlayType.Skill)
        {
            skillNum = Random.Range(0, 8);
            if(skillNum != 1)
            {
                if (!aniFlag)
                {
                    TriggerAnimation();
                    aniFlag = true;
                }
            }
            skillNum = 4;

            if (skillNum == 0)
            {
                StartCoroutine(Skill_0());
            }
            else if (skillNum == 1)
            {
                StartCoroutine(Skill_1());
            }
            else if (skillNum == 2)
            {
                StartCoroutine(Skill_2());
            }
            else if (skillNum == 3)
            {
                StartCoroutine(Skill_3());
            }
            else if (skillNum == 4)
            {
                StartCoroutine(Skill_4());
            }
            else if (skillNum == 5)
            {
                StartCoroutine(Skill_5());
            }
            else if (skillNum == 6)
            {
                StartCoroutine(Skill_6());
            }
            else if (skillNum == 7)
            {
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
            if (playType != Boss_PlayType.Die)
            {
                playType = Boss_PlayType.Die;
                col.enabled = false;
                Destroy(gameObject, 10f);
            }
        }


        if (playType == Boss_PlayType.Die)
        {
            ani.SetTrigger("Die");
            if (!dieEffectFlag)
            {
                StartCoroutine(DieCorutine());
            }
            //DieEffect.Emit(9);
            Vector3 movement = new Vector3(-10f, -4f, 0f);
            transform.Translate(movement * Time.deltaTime);
        }
    }

    void FlipMonster()
    {
        if (player.transform.position.x - transform.position.x < 0f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            xScale_Flag = true;
            //AfterImage_Particle.transform.localScale = new Vector3(-AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            xScale_Flag = false;
            //AfterImage_Particle.transform.localScale = new Vector3(AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }
    }

    IEnumerator BulletFire()
    {
        attackFlag = true;
        for(int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(Boss_Bullet, Fire_Zone.position, Quaternion.identity);
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 15);
            yield return new WaitForSeconds(0.1f);
        }
        attack_lastTime = Time.time;
        attackFlag = false;
    }

    IEnumerator SpawnCorutine()
    {
        SpawnFlag = true;
        float elapsedTime = 0;
        float duration = 1f;
        float height = Random.Range(3f, 5f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float xPos = Mathf.Lerp(Spawn_Init_Pos.x, MonsterDirector_Pos.x, t);
            float yPos = Mathf.Sin(Mathf.PI * t) * height + Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y, t); ;
            transform.localPosition = new Vector2(xPos, yPos);
            //Debug.Log(yPos);

            yield return null;
        }
        MonsterDirector_Pos = new Vector2(MonsterDirector_Pos.x, MonsterDirector.MinPos_Ground.y + 4.5f);
        transform.localPosition = MonsterDirector_Pos;
        col.enabled = true;
        move_lastTime = Time.time;
        attack_lastTime = Time.time;
        playType = Boss_PlayType.Move;
        aniFlag = false;
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

    IEnumerator Skill_0()
    {
        GameObject bullet_ = null;
        for (int i = 0; i < 30; i++)
        {
            if(i %2 == 0)
            {
                bullet_ = Instantiate(Boss_Bullet, Fire_Zone.position, Quaternion.identity);
                bullet_.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 15f);
            }
            else
            {
                bullet_ = Instantiate(Boss_Bullet, Fire_Zone_Sp.position, Quaternion.identity);
                bullet_.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 15f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(5f);
        ToMove();
    }

    IEnumerator Skill_1()
    {
        float moveSpeed = 0.5f;
        float elapsedTime = 0f;
        float delayTime = 6f;
        WaterObject.GetComponent<MonsterBullet>().Get_MonsterBullet_Information((Bullet_Atk / 2), 0f, 0f);
        WaterObject.gameObject.SetActive(true);

        while (true)
        {
            if(elapsedTime < delayTime)
            {
                if (transform.position.x < player.transform.position.x - 4 && transform.position.x < MonsterDirector.MaxPos_Ground.x - 1)
                {
                    transform.Translate(Vector2.right * moveSpeed * Time.fixedDeltaTime);
                }
                else if (transform.position.x > player.transform.position.x + 4 && transform.position.x > MonsterDirector.MinPos_Ground.x + 1)
                {
                    transform.Translate(Vector2.left * moveSpeed * Time.fixedDeltaTime);
                }
                elapsedTime += Time.deltaTime;
            }
            else
            {
                break;
            }
            yield return null;
        }

        WaterObject.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        ToMove();
    }

    IEnumerator Skill_2()
    {
        for (int i = 0; i < 20; i++)
        {
            float randomXPos = Random.Range(-2f, 2f);
            Vector2 pos = new Vector2(player.transform.position.x + randomXPos, MonsterDirector.MaxPos_Sky.y + 4f);
            GameObject fireBall = Instantiate(WaterBallObject_Down, pos, Quaternion.identity);
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
        yield return new WaitForSeconds(1f);
        gameDirector.BossHeal(false);
        ToMove();
    }

    IEnumerator Skill_4()
    {
        for(int i = 0; i < 3; i++)
        {
            if (!xScale_Flag)
            {
                GameObject Tide = Instantiate(TideObject, Tide_SpawnPos.position, Quaternion.identity);
                Tide.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk / 2, 0f, 10f, 1);
                Tide = Instantiate(TideObject, Tide_SpawnPos2.position, Quaternion.identity);
                Tide.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk / 2, 0f, 10f, -1);
            }
            else
            {
                GameObject Tide = Instantiate(TideObject, Tide_SpawnPos.position, Quaternion.identity);
                Tide.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk / 2, 0f, 10f, -1);
                Tide = Instantiate(TideObject, Tide_SpawnPos2.position, Quaternion.identity);
                Tide.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk / 2, 0f, 10f, 1);
            }
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(3f);
        ToMove();
    }

    IEnumerator Skill_5()
    {
        GameObject Cloud_;
        Vector2 spawnPos = new Vector2(
            Random.Range(MonsterDirector.MinPos_Ground.x + 1f, MonsterDirector.MaxPos_Ground.x - 1f),
            MonsterDirector.MinPos_Sky.y + 2f);
        Cloud_ = Instantiate(CloudObject, spawnPos, Quaternion.identity);
        Cloud_.GetComponent<CloudSkill>().SetCloud(Bullet_Atk / 2, 1f, 20);
        yield return new WaitForSeconds(2f);
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
        for(int i = 0; i < 20; i++)
        {
            GameObject Bullet = Instantiate(MissileObject, Missile_SpawnPos.position, Quaternion.identity);
            Bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, 1f, 40f);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2f);
        ToMove();
    }
    void TriggerAnimation()
    {
        if (playType == Boss_PlayType.Spawn || playType == Boss_PlayType.Move)
        {
            ani.SetBool("Skill", false);
        }

        if (playType == Boss_PlayType.Skill)
        {
            ani.SetBool("Skill", true);
        }

        if (playType == Boss_PlayType.Die)
        {
            ani.SetTrigger("Die");
        }
    }

    void ResetAni()
    {
        aniFlag = false;
    }

    private void ToMove()
    {
        if (playType != Boss_PlayType.Die)
        {
            move_delayTime = Random.Range(3f, 6f);
            skilleffect_flag = false;
            attack_lastTime = Time.time;
            move_lastTime = Time.time;
            ResetAni();
            playType = Boss_PlayType.Move;
        }
    }


    enum Boss_PlayType
    {
        Spawn,
        Move,
        Move_Wait,
        Skill,
        Skill_Using,
        Die
    }
}