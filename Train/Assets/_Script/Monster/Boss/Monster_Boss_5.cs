using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Coffee.UIExtensions.UIParticleAttractor;

public class Monster_Boss_5 : Boss
{
    [Header("보스 개인 설정")]
    public GameObject GunObject;
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
    float dash_lastTime;
    float dash_delayTime;
    float attack_lastTime;
    float attack_delayTime;

    float jumpHeight = 3f;      // 점프 높이
    float jumpDuration = 0.7f;  // 점프 시간

    private bool isJumping = false;
    private float jumpStartTime;
    private Vector3 jumpStartPos;
    private Vector3 jumpTargetPos;
    bool didHighAction = false;
    int JumpDashNum = 0;

    public GameObject SKill_Object;
    Vector3 dashTargetPos;
    bool SpawnFlag = false;
    bool dashFlag = false;

    private float idleMoveRange = 1.5f; // 왔다갔다 이동 범위
    private Vector3 idleCenterPos;
    private bool idleDirectionRight = true;

    public ParticleSystem DieEffect;
    ParticleSystem[] alldieEffect;
    Vector3 DieEffectOriginPos;
    bool dieEffectFlag;
    int dieCount;

    protected override void Start()
    {
        Boss_Num = 5;
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
        dash_delayTime = 2f;
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

        if(playType == Boss_PlayType.Spawn)
        {
            if (!SpawnFlag)
            {
                StartCoroutine(SpawnCorutine());
            }
        }

        if(playType == Boss_PlayType.Move)
        {
            FlipMonster();
            if (Time.time >= attack_lastTime + attack_delayTime)
            {
                if (!dashFlag)
                {
                    BulletFire();
                }
                attack_lastTime = Time.time;
            }

            if(Time.time >= dash_lastTime + dash_delayTime && !dashFlag)
            {
                dashFlag = true;

                JumpDashNum = Random.Range(0, 2);

                if(JumpDashNum == 0)
                {
                    // 목표 위치 설정 (플레이어 위치 ± 3)
                    float offset = (player.transform.position.x > transform.position.x) ? 1f : -1f;
                    float targetX = player.transform.position.x + offset;

                    // 최소/최대 이동 제한 적용
                    targetX = Mathf.Clamp(targetX, MonsterDirector.MinPos_Ground.x + 1f, MonsterDirector.MaxPos_Ground.x - 1f);

                    dashTargetPos = new Vector3(targetX, transform.position.y, transform.position.z);
                }
            }

            if (!dashFlag)
            {
                IdleMove();
            }
            else
            {
                if(JumpDashNum == 0)
                {
                    DashMove();
                }
                else
                {
                    if (!isJumping)
                    {
                        JumpMove();
                    }
                    else
                    {
                        JumpProcess();
                    }
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
                playType = Boss_PlayType.SKill;
            }
        }

        if(playType == Boss_PlayType.SKill)
        {
            //skillNum = Random.Range(0, 5);
            skillNum = 0;
            if(skillNum == 0)
            {
                Debug.Log("스킬 사용");
            }
            playType = Boss_PlayType.Skill_Using;
        }

        if(playType == Boss_PlayType.Skill_Using)
        {
            playType = Boss_PlayType.Move;
            move_lastTime = Time.time;
        }

        if (DieFlag)
        {
            if(playType != Boss_PlayType.Die)
            {
                playType = Boss_PlayType.Die;
                col.enabled = false;
            }
        }

        if(playType == Boss_PlayType.Die)
        {
            if (!dieEffectFlag && dieCount < 4)
            {
                StartCoroutine(DieCorutine());
                dieCount++;
            }
            //DieEffect.Emit(9);
        }
    }

    void IdleMove()
    {
        // 좌우로 오락가락
        float direction = idleDirectionRight ? 0.5f : -0.5f;
        transform.Translate(Vector2.right * direction * Speed * Time.deltaTime);

        // 범위 벗어나면 반전
        if (Mathf.Abs(transform.position.x - idleCenterPos.x) >= idleMoveRange)
        {
            idleDirectionRight = !idleDirectionRight;
        }
    }

    void DashMove()
    {
        //여기서 콜라이더를 킨다.
        transform.position = Vector3.MoveTowards(transform.position, dashTargetPos, (35f) * Time.deltaTime);

        // 목표 지점 도달 시 대시 종료
        if (Vector3.Distance(transform.position, dashTargetPos) < 0.1f)
        {
            dashFlag = false;
            idleCenterPos = transform.position; // 새로운 idle 기준 위치 갱신
            dash_lastTime = Time.time;
            //여기서 콜라이더를 끄면 됨
        }
    }
    void JumpMove()
    {
        isJumping = true;
        jumpStartTime = Time.time;
        jumpStartPos = transform.position;

        // 플레이어 방향으로 이동
        float dir = (player.transform.position.x > transform.position.x) ? 1f : -1f;
        jumpTargetPos = jumpStartPos + new Vector3(dir * 5f, 0, 0); // 3만큼 이동
    }

    void JumpProcess()
    {
        float t = (Time.time - jumpStartTime) / jumpDuration;

        if(!didHighAction && t >= 0.5f)
        {
            didHighAction = true;
            BulletFire();
        }

        if (t >= 1f)
        {
            transform.position = jumpTargetPos;
            idleCenterPos = transform.position;
            didHighAction = false;
            isJumping = false;
            dashFlag = false;
            dash_lastTime = Time.time;
            return;
        }

        // 좌우 이동 (Lerp)
        float x = Mathf.Lerp(jumpStartPos.x, jumpTargetPos.x, t);

        // Y는 Sin 기반 점프 곡선
        float y = jumpStartPos.y + Mathf.Sin(Mathf.PI * t) * jumpHeight;

        transform.position = new Vector3(x, y, transform.position.z);
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
        for(int i = 0; i < 5; i++)
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