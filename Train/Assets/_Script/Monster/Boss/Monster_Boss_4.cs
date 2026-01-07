using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static Coffee.UIExtensions.UIParticleAttractor;

public class Monster_Boss_4 : Boss
{
    GameObject player;

    [SerializeField]
    Boss_PlayType playType;

    Vector3 Move_Init_Position;

    float move_lastTime = 0f;
    float attack_lastTime = 0f;
    float move_delayTime = 3f;
    float attack_delayTime;

    int skillNum;

    [SerializeField]
    float speed;
    private Vector2 moveDir;
    private float changeTimer = 0f;
    float directionChangeInterval = 2.0f; // 방향 변경 주기
    float randomAngleRange = 45f;
    // Start is called before the first frame update

    public Transform Fire_Zone;
    public GameObject SkillBigBullet;
    public GameObject SkillBullet;
    public Transform Monster_List_Ground;
    public GameObject SkillMonster;
    public GameObject WatarPillar;

    public ParticleSystem DieEffect;
    ParticleSystem[] alldieEffect;
    Vector3 DieEffectOriginPos;
    bool dieEffectFlag;

    protected override void Start()
    {
        Boss_Num = 4;
        base.Start();

        playType = Boss_PlayType.Spawn;
        col.enabled = false;
        moveDir = Random.insideUnitCircle.normalized;
        Monster_List_Ground = GameObject.Find("Monster_List(Ground)").transform;
        player = GameObject.FindWithTag("Player");

        DieEffectOriginPos = DieEffect.transform.localPosition;
        alldieEffect = DieEffect.GetComponentsInChildren<ParticleSystem>();
        attack_delayTime = 2f;
        speed = 4f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected void FixedUpdate()
    {
        Fire_Debuff();
        FlipMonster();
        if (playType == Boss_PlayType.Spawn)
        {

            transform.Translate(-12f * Time.deltaTime, 0, 0, Space.World);

            if (transform.position.x < MonsterDirector.MinPos_Ground.x + 2f)
            {
                playType = Boss_PlayType.Move;
                transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                col.enabled = true;
                move_lastTime = Time.time;
                attack_lastTime = Time.time;

                Move_Init_Position = transform.position;
            }
        }

        if (playType == Boss_PlayType.Move)
        {
            BossMove();

            
            if (Time.time >= attack_lastTime + attack_delayTime)
            {
                BulletFire();
                //소환술
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
                playType = Boss_PlayType.Skill;
            }
        }

        if (playType == Boss_PlayType.Skill)
        {
            skillNum = Random.Range(0, 4);

            if (skillNum == 0)
            {
                StartCoroutine(BigBulletSkill());
            }

            if (skillNum == 1)
            {
                StartCoroutine(BubbleSkill());
            }

            if (skillNum == 2)
            {
                StartCoroutine(SpawnFish());
            }

            if (skillNum == 3)
            {
                StartCoroutine (pillarSkill());
            }

            playType = Boss_PlayType.SKill_Using;
        }

        if (playType == Boss_PlayType.SKill_Using)
        {
            // 스킬 사용 중 로직
        }

        if (DieFlag)
        {
            if (playType != Boss_PlayType.Die)
            {
                playType = Boss_PlayType.Die;
                Destroy(gameObject, 10f);
            }
        }

        if (playType == Boss_PlayType.Die)
        {
            Vector3 movement;
            if (!dieEffectFlag)
            {
                StartCoroutine(DieCorutine());
            }

            //DieEffect.Emit(9);
            movement = new Vector3(-3f, -7f, 0f);
            transform.Translate(movement * Time.deltaTime);
        }
    }

    void BossMove()
    {
        // 이동
        transform.Translate(moveDir * speed * Time.deltaTime);

        // 영역 경계 체크
        Vector3 pos = transform.position;
        if (pos.x < MonsterDirector.MinPos_Sky.x || pos.x > MonsterDirector.MaxPos_Sky.x)
        {
            moveDir.x *= -1; // 좌우 반사
            pos.x = Mathf.Clamp(pos.x, MonsterDirector.MinPos_Sky.x, MonsterDirector.MaxPos_Sky.x);
            transform.position = pos;
        }
        if (pos.y < MonsterDirector.MinPos_Sky.y + 1.5f || pos.y > MonsterDirector.MaxPos_Sky.y + 1.5f)
        {
            moveDir.y *= -1; // 상하 반사
            pos.y = Mathf.Clamp(pos.y, MonsterDirector.MinPos_Sky.y + 1.5f, MonsterDirector.MaxPos_Sky.y + 1.5f);
            transform.position = pos;
        }

        // 일정 주기마다 랜덤 방향 변화
        changeTimer += Time.deltaTime;
        if (changeTimer >= directionChangeInterval)
        {
            changeTimer = 0f;
            directionChangeInterval = Random.Range(0.2f, 0.5f);
            speed = Random.Range(2f, 4f);
            // 기존 방향에서 ±randomAngleRange 정도 랜덤 회전
            float angle = Random.Range(-randomAngleRange, randomAngleRange);
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            Vector2 newDir = new Vector2(moveDir.x * cos - moveDir.y * sin,
                                         moveDir.x * sin + moveDir.y * cos).normalized;
            moveDir = newDir;
        }
    }

    public void BulletFire()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject defaultBullet = Instantiate(Boss_Bullet, Fire_Zone.position, Quaternion.identity, monster_Bullet_List);
            defaultBullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 0, 0);
            defaultBullet.GetComponent<MonsterBullet>().SetSpeed(Random.Range(8f, 15f));
            attack_lastTime = Time.time;
        }
    }

    IEnumerator BigBulletSkill()
    {
        GameObject Skill = Instantiate(SkillBigBullet, Fire_Zone.position, Quaternion.identity, monster_Bullet_List);
        Skill.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk * 6, 1, 20, 0);
        //attack_lastTime = Time.time;
        yield return new WaitForSeconds(0.2f);
        ToMove();
    }

    IEnumerator BubbleSkill()
    {
        float Pos = MonsterDirector.MinPos_Ground.x;
        while (true)
        {
            Pos += 1.8f;
            Vector2 newPos = new Vector2(Pos, MonsterDirector.MaxPos_Sky.y + 1);
            GameObject Skill = Instantiate(SkillBullet, newPos, Quaternion.identity, monster_Bullet_List);
            Skill.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk / 2, 1, 20, 0);
            Skill.GetComponent<MonsterBullet>().SetSpeed(Random.Range(3f, 6f));
            if (Pos > MonsterDirector.MaxPos_Ground.x) 
                break;
        }

        yield return new WaitForSeconds(0.5f);
        ToMove();
        //attack_lastTime = Time.time;
    }

    IEnumerator SpawnFish()
    {
        for(int i = 0; i < 10; i++)
        {
            float rndX = Random.Range(MonsterDirector.MinPos_Ground.x, MonsterDirector.MaxPos_Ground.x);
            Instantiate(SkillMonster, new Vector3(rndX, MonsterDirector.MinPos_Ground.y, 0), Quaternion.identity, Monster_List_Ground);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.5f);
        ToMove();
    }

    IEnumerator pillarSkill()
    {
        for (int i = 0; i < 5; i++)
        {
            float rndX = Random.Range(MonsterDirector.MinPos_Ground.x, MonsterDirector.MaxPos_Ground.x);
            GameObject watar = Instantiate(WatarPillar, new Vector3(rndX, MonsterDirector.MinPos_Ground.y, 0), Quaternion.identity, Monster_List_Ground);
            watar.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk / 2, 1, 20, 0);
            watar.GetComponent<MonsterBullet>().SetDestory(Random.Range(3f, 6f));
        }
        yield return new WaitForSeconds(0.3f);
        ToMove();
    }

    private void ToMove()
    {
        if (playType != Boss_PlayType.Die)
        {
            move_delayTime = Random.Range(2f, 5f);
            move_lastTime = Time.time;
            skilleffect_flag = false;
            playType = Boss_PlayType.Move;
        }
    }

    private void FlipMonster()
    {
        if (player.transform.position.x - transform.position.x< 0f)
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
    IEnumerator DieCorutine()
    {
        dieEffectFlag = true;
        Vector2 diePos = new Vector2(
           DieEffectOriginPos.x + Random.Range(-0.8f, 0.8f),
           DieEffectOriginPos.y + Random.Range(-0.8f, 0.8f)
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
    enum Boss_PlayType
    {
        Spawn,
        Move,
        Move_Wait,
        Skill,
        SKill_Using,
        Die
    }
}
