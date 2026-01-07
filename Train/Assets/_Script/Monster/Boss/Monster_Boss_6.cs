using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Monster_Boss_6 : Boss
{
    [Header("보스 개인 설정")]
    public Transform Fire_Zone;
    GameObject player;
    float Speed;

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
    
    [Header("스킬6-불꽃나비 날리기")]
    public GameObject FireButterflyObject;

    [Header("스킬7-메테오")]
    public GameObject MeteorObject;

    [Header("스킬8-레이저")]
    public GameObject LaserObject;

    protected override void Start()
    {
        Boss_Num = 6;
        base.Start();
        player = GameObject.FindWithTag("Player");

        MonsterDirector_Pos = new Vector2(transform.position.x, MonsterDirector.MinPos_Ground.y + 1.15f);
        Spawn_Init_Pos =
                new Vector2(MonsterDirector_Pos.x + Random.Range(2f, 5f),
                MonsterDirector.MinPos_Ground.y - 5f);
        transform.localPosition = Spawn_Init_Pos;

        Speed = 4f;

        DieEffectOriginPos = DieEffect.transform.localPosition;
        alldieEffect = DieEffect.GetComponentsInChildren<ParticleSystem>();

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

        if (playType == Boss_PlayType.Move)
        {
            if (Time.time >= attack_lastTime + attack_delayTime)
            {
                BulletFire();
                attack_lastTime = Time.time;
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
            skillNum = 0;

            if (skillNum == 0)
            {
                //폭탄 깔기
            }
            else if (skillNum == 1)
            {
                //화염방사기
            }
            else if (skillNum == 2)
            {
                //화염구 내리기
            }
            else if (skillNum == 3)
            {
                //긴급 수리
            }
            else if (skillNum == 4)
            {
                //화염구 날리기
            }
            else if (skillNum == 5)
            {
                //불꽃나비 날리기
            }
            else if (skillNum == 6)
            {
                //메테오
            }
            else if (skillNum == 7)
            {
                //레이저 발사
            }
        }

        if(playType == Boss_PlayType.Skill_Using)
        {
            //스킬 사용 중
        }

        if (DieFlag)
        {
            if(playType != Boss_PlayType.Die)
            {
                playType = Boss_PlayType.Die;
                col.enabled = false;
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
        }
    }

    void FlipMonster()
    {
        if (player.transform.position.x - transform.position.x < 0f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            //AfterImage_Particle.transform.localScale = new Vector3(-AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            //AfterImage_Particle.transform.localScale = new Vector3(AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }
    }

    void BulletFire()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = Instantiate(Boss_Bullet, Fire_Zone.position, Quaternion.identity);
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 0f);
        }
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
            float yPos = Mathf.Sin(Mathf.PI * t) * height + Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y + 0.7f, t); ;
            transform.localPosition = new Vector2(xPos, yPos);
            //Debug.Log(yPos);

            yield return null;
        }
        MonsterDirector_Pos = new Vector2(MonsterDirector_Pos.x, MonsterDirector.MinPos_Ground.y + 1.15f);
        transform.localPosition = MonsterDirector_Pos;
        col.enabled = true;
        move_lastTime = Time.time;
        attack_lastTime = Time.time;
        playType = Boss_PlayType.Move;
    }

    IEnumerator DieCorutine()
    {
        dieEffectFlag = true;
        Vector2 diePos = new Vector2(
           DieEffectOriginPos.x + Random.Range(-2f, 2f),
           DieEffectOriginPos.y + Random.Range(-1f, 1f)
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
            move_delayTime = Random.Range(3f, 6f);
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