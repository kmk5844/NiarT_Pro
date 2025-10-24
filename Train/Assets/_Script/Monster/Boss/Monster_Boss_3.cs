using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Boss_3 : Boss
{
    Animator ani;

    [SerializeField]
    Boss_PlayType playType;

    bool aniFlag;

    Vector3 Move_Init_Position;

    float move_lastTime = 0f;
    float attack_lastTime = 0f;
    float move_delayTime = 5f;
    float attack_delayTime = 5f;

    int skillNum;

    [SerializeField]
    float speed;
    private Vector2 moveDir;
    private float changeTimer = 0f;
    float directionChangeInterval = 1.0f; // 방향 변경 주기
    float randomAngleRange = 45f;
    // Start is called before the first frame update

    protected override void Start()
    {
        Boss_Num = 3;
        base.Start();

        playType = Boss_PlayType.Spawn;
        col.enabled = false;


        ani = GetComponent<Animator>();
        aniFlag = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected void FixedUpdate()
    {
        Fire_Debuff();

        if (playType == Boss_PlayType.Spawn)
        {
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            transform.Translate(-12f * Time.deltaTime, 0, 0, Space.World);

            if (transform.position.x < MonsterDirector.MinPos_Ground.x - 7f)
            {
                playType = Boss_PlayType.Move;
                transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                transform.rotation = Quaternion.Euler(0, 0, -20);
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
            BossMove();


            if (Time.time >= attack_lastTime + attack_delayTime)
            {
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
            if (!aniFlag)
            {
                TriggerAnimation();
                aniFlag = true;
            }

            skillNum = Random.Range(0, 5);
            if (skillNum == 0)
            {
                StartCoroutine(BeeShiled());
            }

            if (skillNum == 1)
            {
                StartCoroutine(BeeAttack());
            }

            if (skillNum == 2)
            {
                StartCoroutine(BeeHeal());
            }

            if (skillNum == 3)
            {
                StartCoroutine(BeeSpawn());
            }

            if (skillNum == 4)
            {
                StartCoroutine(BeeSlow());
            }
            playType = Boss_PlayType.SKill_Using;
            ResetAni();
        }

        if (playType == Boss_PlayType.SKill_Using)
        {
            if(skillNum == 0)
            {
                // 방어막 스킬 사용 중 로직
            }
            if (skillNum == 1)
            {
                // 공격 스킬 사용 중 로직
            }
            if (skillNum == 2)
            {
                // 치유 스킬 사용 중 로직
            }
            if (skillNum == 3)
            {
                // 소환 스킬 사용 중 로직
            }
            if (skillNum == 4)
            {
                // 느리게 하는 스킬 사용 중 로직
            }

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
        if (pos.y < MonsterDirector.MinPos_Sky.y || pos.y > MonsterDirector.MaxPos_Sky.y)
        {
            moveDir.y *= -1; // 상하 반사
            pos.y = Mathf.Clamp(pos.y, MonsterDirector.MinPos_Sky.y, MonsterDirector.MaxPos_Sky.y);
            transform.position = pos;
        }

        // 일정 주기마다 랜덤 방향 변화
        changeTimer += Time.deltaTime;
        if (changeTimer >= directionChangeInterval)
        {
            changeTimer = 0f;
            directionChangeInterval = Random.Range(0.3f, 0.6f);
            speed = Random.Range(6f, 12f);
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

    void ResetAni()
    {
        aniFlag = false;
    }

    void TriggerAnimation()
    {
        if (playType == Boss_PlayType.Spawn || playType == Boss_PlayType.Move)
        {
            ani.SetTrigger("Move");
        }

        if (playType == Boss_PlayType.Skill)
        {
            ani.SetTrigger("Skill");
        }

        if (playType == Boss_PlayType.Die)
        {
            ani.SetTrigger("Die");
        }
    }

    IEnumerator BeeShiled()
    {
        yield return null;
        ToMove();
    }

    IEnumerator BeeAttack()
    {
        yield return null;
        ToMove();
    }

    IEnumerator BeeHeal()
    {
        yield return null;
        ToMove();
    }

    IEnumerator BeeSpawn()
    {
        yield return null;
        ToMove();
    }

    IEnumerator BeeSlow()
    {
        yield return null;
        ToMove();
    }

    private void ToMove()
    {
        if (playType != Boss_PlayType.Die)
        {
            transform.position = new Vector3(transform.position.x, 10f, 0);
            move_delayTime = Random.Range(5f, 8f);
            skilleffect_flag = false;
            move_lastTime = Time.time;
            playType = Boss_PlayType.Move;
        }
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
