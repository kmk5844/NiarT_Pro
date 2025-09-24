using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Monster_Boss_1 : Boss
{
    public Transform Fire_Zone;
    public GameObject TentacleObject;
    public GameObject Tentacle_Bomb_Object;
    public GameObject HowlingObject;
    GameObject player;

    [SerializeField]
    Boss_PlayType playType;
    Animator ani;
    bool aniFlag;

    Vector3 Move_Init_Position;
    float move_xPos;
    Vector3 movement;
    float move_speed;

    float move_lastTime;
    float move_delayTime;
    float attack_lastTime;
    float attack_delayTime;

    bool skilleffectflag;

    public GameObject WarkEffect;
    public ParticleSystem FireEffect;
    public ParticleSystem WarningEffect;
    public ParticleSystem DieEffect;
    ParticleSystem[] alldieEffect;
    Vector3 DieEffectOriginPos;
    bool dieEffectFlag;
    protected override void Start()
    {
        Boss_Num = 1;
        ani = GetComponent<Animator>();
        aniFlag = false;
        base.Start();
        move_xPos = 1f;
        move_speed = 3f;
        player = GameObject.FindWithTag("Player");
        transform.position = new Vector3(MonsterDirector.MaxPos_Sky.x + 18f, 0.79f, 32);
        playType = Boss_PlayType.Spawn;

        DieEffectOriginPos = DieEffect.transform.localPosition;
        alldieEffect = DieEffect.GetComponentsInChildren<ParticleSystem>();

        skilleffectflag = false;
        move_delayTime = 5f;
        attack_delayTime = 0.4f;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        Fire_Debuff();

        if (playType == Boss_PlayType.Spawn)
        {
            transform.Translate(-12f * Time.deltaTime, 0, 0);

            if (transform.position.x < MonsterDirector.MinPos_Ground.x - 7f)
            {
                playType = Boss_PlayType.Move;
                ResetAni();

                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                }
                col.enabled = true;
                move_lastTime = Time.time;
                attack_lastTime = Time.time;

                Move_Init_Position = transform.position;
            }
        }

        if (playType == Boss_PlayType.Move)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            if (Move_Init_Position.x - 1 > transform.position.x)
            {
                move_xPos = 1f;
                move_speed = Random.Range(3f, 5f);
            }
            else if (Move_Init_Position.x + 1 < transform.position.x)
            {
                move_xPos = -1f;
                move_speed = Random.Range(1f, 2f);
            }
            movement = new Vector3(move_xPos, 0f, 0f);
            transform.Translate(movement * move_speed * Time.deltaTime);

            //공격
            if (Time.time >= attack_lastTime + attack_delayTime)
            {
                BulletFire();
            }

            if (!skilleffectflag && Time.time >= move_lastTime + move_delayTime - 0.2f)
            {
                skilleffectflag = true;
                WarningEffect.Play();
            }

            //스킬
            if (Time.time >= move_lastTime + move_delayTime)
            {
                playType = Boss_PlayType.Skill;
                ResetAni();
            }
        }

        if (playType == Boss_PlayType.Skill)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            int skillNum = Random.Range(0, 4);

            if (skillNum == 0)
            {
                StartCoroutine(skill_Tentacle(0));
            }
            else if (skillNum == 1)
            {
                StartCoroutine(skill_Tentacle(1));
            }
            else if (skillNum == 2)
            {
                StartCoroutine(skill_Tentacle(2));
            }
            else if (skillNum == 3)
            {
                StartCoroutine(skill_Tentacle(3));
            }

            playType = Boss_PlayType.Skill_Using;
            ResetAni();
        }

        if (DieFlag)
        {
            if (playType != Boss_PlayType.Die)
            {
                playType = Boss_PlayType.Die;
                WarkEffect.SetActive(false);
                ResetAni();
                Destroy(gameObject, 10f);
            }
        }


        if (playType == Boss_PlayType.Die)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            if (!dieEffectFlag)
            {
                StartCoroutine(DieCorutine());
            }

            movement = new Vector3(-2f, 0f, 0f);
            transform.Translate(movement * Time.deltaTime);
        }
    }
    IEnumerator DieCorutine()
    {
        dieEffectFlag = true;
        Vector3 diePos = new Vector3(
           DieEffectOriginPos.x + Random.Range(-2f, 2.2f),
           DieEffectOriginPos.y + Random.Range(-0.2f, -0.8f),
           DieEffectOriginPos.z
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

    IEnumerator skill_Tentacle(int skill)
    {
        float xPos;
        Vector2 pos;
        yield return new WaitForSeconds(1.5f);

        if(skill == 0)
        {
            TentacleObject.GetComponent<Boss1_TentacleObject>().SetSkillNum(0);
            for (int i = 0; i < 25; i++)
            {
                xPos = Random.Range(MonsterDirector.MinPos_Ground.x, MonsterDirector.MaxPos_Ground.x);
                pos = new Vector2(xPos, -1.2f);
                Instantiate(TentacleObject, pos, Quaternion.identity, monster_Bullet_List);
                yield return new WaitForSeconds(0.05f);
            }
        }else if(skill == 1)
        {
            TentacleObject.GetComponent<Boss1_TentacleObject>().SetSkillNum(1);
            Quaternion rot = Quaternion.Euler(0f, 0f, -30f);
            for (int i = 0; i < 12; i++)
            {
                xPos = MonsterDirector.MinPos_Ground.x + (i * 2.5f);
                pos = new Vector2(xPos, -1.2f);
                Instantiate(TentacleObject, pos, rot, monster_Bullet_List);
                yield return new WaitForSeconds(0.05f);
            }
        }else if(skill == 2)
        {
            TentacleObject.GetComponent<Boss1_TentacleObject>().SetSkillNum(2);
            for(int i = 0; i < 30; i++)
            {
                Quaternion rot = Quaternion.Euler(0f, 0f, Random.Range(-30f, 30f));
                xPos = Random.Range(MonsterDirector.MinPos_Ground.x, MonsterDirector.MaxPos_Ground.x);
                pos = new Vector2(xPos, -1.2f);
                Instantiate(TentacleObject, pos, rot, monster_Bullet_List);
                yield return new WaitForSeconds(0.05f);
            }
        }else if(skill == 3)
        {
            TentacleObject.GetComponent<Boss1_TentacleObject>().SetSkillNum(3);
            float playerPos = player.transform.position.x;
            for(int i =  -2; i < 6; i++)
            {
                xPos = playerPos + (i * 2.5f);
                pos = new Vector2(xPos, -1.2f);
                Instantiate(TentacleObject, pos, Quaternion.identity, monster_Bullet_List);
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        ToMove();
    }

    void ResetAni()
    {
        aniFlag = false;
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

    //공격 애니메이션 삽입
    public void BulletFire()
    {
        FireEffect.Emit(30);
        GameObject defaultBullet = Instantiate(Boss_Bullet, Fire_Zone.position, Quaternion.identity, monster_Bullet_List);
        defaultBullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 0);
        attack_lastTime = Time.time;
    }

    //공격 애니메이션 종료 후, 삽입.
    public void ToMove()
    {
        move_delayTime = Random.Range(1.5f, 2.5f);
        move_lastTime = Time.time;
        playType = Boss_PlayType.Move;
        skilleffectflag = false;
        ResetAni();
    }

    enum Boss_PlayType
    {
        Spawn,
        Move,
        Skill,
        Skill_Using,
        Die
    }
}