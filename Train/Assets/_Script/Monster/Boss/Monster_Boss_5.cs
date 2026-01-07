using JetBrains.Annotations;
using PixelCrushers.DialogueSystem;
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

    [Header("광폭화 모드")]
    [SerializeField]
    bool BerserkMode;

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

    public GameObject Skill_Dangger_Player_Object;
    public GameObject Skill_Dangger_Angle_Object;
    public GameObject Skill_Bomb_Object;
    public GameObject Skill_Wind_Object;

    Vector3 dashTargetPos;
    bool SpawnFlag = false;
    bool dashFlag = false;
    bool skillDashFlag = false;

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

            if (Time.time >= move_lastTime + move_delayTime && !dashFlag)
            {
                playType = Boss_PlayType.SKill;
            }
        }

        if(playType == Boss_PlayType.SKill)
        {
            skillNum = Random.Range(0, 5);
            //skillNum = 2;
            if(skillNum == 0)
            {
                //체력 회복 5%
                StartCoroutine(BossHPHeal());
            }
            else if(skillNum == 1)
            {
                //뛰어서 단검을 날림
                Vector2 target = new Vector2(transform.position.x + 5f, transform.position.y);
                StartCoroutine(JumpSkill(target, 8f));
                //1. 플레이어 향해 단검 날리기
                //2. 단검 폭발
                //3. 단검 내리꽂기
            }
            else if(skillNum == 2)
            {
                //검기 날리기
                StartCoroutine(SkillWind());
                //1. 플레이어 향해 검기 날리기
                //2. 좌우로 검기날리기
                //3. 플레이어 위치에서 위 아래로 검기날리기
            }
            else if(skillNum == 3)
            {
                //대쉬하면서 폭탄 날리기
                StartCoroutine(DashBomb());
            }
            else if(skillNum == 4)
            {
                StartCoroutine(DaggerPosition());
            }
            else if(skillNum == 5)
            {
                StartCoroutine(ShotGunPosition());
            }
            playType = Boss_PlayType.Skill_Using;
        }

        if(playType == Boss_PlayType.Skill_Using)
        {
            //playType = Boss_PlayType.Move;
            //move_lastTime = Time.time;
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
    IEnumerator BossHPHeal()
    {
        gameDirector.BossHeal(true);
        int heal = Monster_Max_HP * 5 / 100;

        Monster_HP += heal;
        if (Monster_HP > Monster_Max_HP)
        {
            Monster_HP = Monster_Max_HP;
        }
        yield return new WaitForSeconds(0.5f);
        gameDirector.BossHeal(false);
        ToMove();
    }

    IEnumerator JumpSkill(Vector2 jumpTargetPos, float jumpHeight)
    {
        bool _didHighAction = false;
        int daggerSkill = Random.Range(0, 3);
        daggerSkill = 2;

        Vector2 startPos = transform.position;
        float upDuration = 1f;     // 상승 시간
        float downDuration = 0.5f;   // 하강 시간

        float elapsed = 0f;

        while (elapsed < upDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / upDuration);

            float x = Mathf.Lerp(startPos.x, jumpTargetPos.x, t * 0.5f);
            float y = startPos.y + Mathf.Sin(Mathf.PI * t * 0.5f) * jumpHeight;
            transform.position = new Vector3(x, y, transform.position.z);

            yield return null;
        }

        // 최고점 도달
        if (!_didHighAction)
        {
            _didHighAction = true;
            yield return StartCoroutine(HighAction(daggerSkill));
        }

        elapsed = 0f;
        Vector2 peakPos = transform.position;

        while (elapsed < downDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / downDuration);

            // X는 정상적으로 목표 지점으로 이동
            float x = Mathf.Lerp(peakPos.x, jumpTargetPos.x, t);

            // Y는 피크에서 targetPos.y로 부드럽게 내려오기
            float y = Mathf.Lerp(peakPos.y, jumpTargetPos.y, t);

            transform.position = new Vector3(x, y, transform.position.z);

            yield return null;
        }

        transform.position = jumpTargetPos; // 보정
        yield return new WaitForSeconds(2f);
        ToMove();
    }

    IEnumerator HighAction(int skillIndex)
    {
        if (skillIndex == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3 RandomPos = new Vector2(0, Random.Range(-0.5f, 0.5f));
                GameObject bullet = Instantiate(Skill_Dangger_Player_Object,
                                                Fire_Zone.position + RandomPos,
                                                Quaternion.identity);

                bullet.GetComponent<MonsterBullet>()
                      .Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 30f);

                yield return new WaitForSeconds(0.1f);
            }
        }
        else if (skillIndex == 1)
        {
            for(int i = 0; i < 16; i++)
            {
                GameObject bullet = Instantiate(Skill_Dangger_Angle_Object,
                                               transform.position,
                                               Quaternion.identity);
                bullet.GetComponent<MonsterBullet>()
                      .Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 30f);
                bullet.GetComponent<Monster_Bullet_Angle>()
                      .SetAngle_And_Fire(-45 + (i * 6));
            }
            yield return null;
        }
        else if (skillIndex == 2)
        {
            for(int i = 0; i < 16; i++)
            {
                Vector3 RandomPos = new Vector2(Random.Range(-7f, 7f), 0);
                GameObject bullet = Instantiate(Skill_Dangger_Angle_Object,
                                               (transform.position + RandomPos),
                                               Quaternion.identity);
                bullet.GetComponent<MonsterBullet>()
                      .Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, Random.Range(10f, 40f));
                bullet.GetComponent<Monster_Bullet_Angle>()
                      .SetAngle_And_Fire(0f);
            }
            yield return null;
        }
    }

    IEnumerator DaggerPosition()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 RandomPos = new Vector2(0, Random.Range(-0.8f, 0.8f));
            GameObject bullet = Instantiate(Skill_Dangger_Player_Object,
                                            Fire_Zone.position + RandomPos,
                                            Quaternion.identity);

            bullet.GetComponent<MonsterBullet>()
                  .Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 20f);

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        ToMove();
    }

    IEnumerator ShotGunPosition()
    {
        for(int i = 0; i < 3; i++)
        {
            BulletFire();
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.3f);
        BulletFire();
        BulletFire();
        yield return new WaitForSeconds(5f);
        ToMove();
    }

    IEnumerator DashBomb()
    {
        float offset = (player.transform.position.x > transform.position.x) ? 1f : -1f;
        float targetX = player.transform.position.x + offset;

        targetX = Mathf.Clamp(targetX, MonsterDirector.MinPos_Ground.x + 1f, MonsterDirector.MaxPos_Ground.x - 1f);
        Vector3 dashTargetPos_ = new Vector3(targetX, transform.position.y, transform.position.z);

        Vector3 startPos = transform.position;  // 시작 지점 저장
        float logInterval = 2f;                // 출력 간격 (1m)
        float nextLogDist = logInterval;

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, dashTargetPos_, 35f * Time.deltaTime);

            float traveled = Vector3.Distance(startPos, transform.position);
            float totalDist = Vector3.Distance(startPos, dashTargetPos_);

            if (traveled >= nextLogDist)
            {
                GameObject Bullet = Instantiate(Skill_Bomb_Object,
                                                  transform.position,
                                                  Quaternion.identity);
                Bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, Bullet_Slow, 5f);
                Bullet.GetComponent<Monster_Bullet_Angle>().SetAngle_And_Fire(180f);
                nextLogDist += logInterval;
            }

            if (Vector3.Distance(transform.position, dashTargetPos_) < 0.1f)
            {
                dashFlag = false;
                idleCenterPos = transform.position;

                ToMove();

                break;
            }

            yield return null;
        }
    }

    IEnumerator SkillWind()
    {
        int skillNum = Random.Range(0, 3);
        if(skillNum == 0)
        {
            for(int i = 0; i < 3; i++)
            {
                GameObject wind = Instantiate(Skill_Wind_Object,
                                       transform.position,
                                       Quaternion.identity);
                wind.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, 0f, 12f);
                wind.GetComponent<Boss_5_Wind>().SetPlayer();
                yield return new WaitForSeconds(1f);
            }
        }
        else if(skillNum == 1)
        {
            for(int i = 0; i < 5; i++)
            {
                GameObject wind = Instantiate(Skill_Wind_Object,
                                       transform.position,
                                       Quaternion.identity);
                wind.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, 0f, 12f);
                wind.GetComponent<Boss_5_Wind>().SetAngle(90 + (i * 45));
            }
        }else if(skillNum == 2)
        {
            Vector2 playerPos = player.transform.position;

            GameObject wind = Instantiate(Skill_Wind_Object,
                                       playerPos + new Vector2(0, 8f),
                                       Quaternion.identity);
            wind.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, 0f, 12f);
            wind.GetComponent<Boss_5_Wind>().SetAngle(0);
            wind = Instantiate(Skill_Wind_Object,
                                       playerPos + new Vector2(0, -8f),
                                       Quaternion.identity);
            wind.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk, 0f, 12f);
            wind.GetComponent<Boss_5_Wind>().SetAngle(180);
        }
       
        yield return new WaitForSeconds(3f);
        ToMove();
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