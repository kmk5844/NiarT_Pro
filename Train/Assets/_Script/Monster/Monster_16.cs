using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_16 : Monster
{
    Vector3 movement;
    float xPos;

    [SerializeField]
    float speed;
    [SerializeField]
    bool AttackFlag;
    Animator anicon;
    Vector2 startPos;

    float elapsedTime = 0f;
    [SerializeField]
    bool isLoopFinished;

    protected override void Start()
    {
        Monster_Num = 16;
        Bullet_Delay = 6;
        //BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);

        base.Start();
        //transform.localPosition = MonsterDirector.MaxPos_Sky;
        MonsterDirector_Pos = transform.localPosition;
        Spawn_Init_Pos =
                    new Vector2(MonsterDirector_Pos.x + Random.Range(4, 12f),
                MonsterDirector.MaxPos_Sky.y);
        transform.localPosition = Spawn_Init_Pos;

        speed = Random.Range(6f, 10f);
        xPos = -1f;
        AttackFlag = false;

        Check_ItemSpeedSpawn();
        Monster_coroutine = StartCoroutine(SpawnMonster());
        anicon = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        Check_ItemSpeedFlag();

        if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            if (!AttackFlag)
            {
                attack();
            }
            FlipMonster();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            if (!AttackFlag)
            {
                MonsterMove();
                isLoopFinished = false;
            }
            else
            {
                if (!isLoopFinished)
                {
                    attackMove();
                }
            }
        }
    }

    IEnumerator SpawnMonster()
    {
        float elapsedTime = 0;
        float duration = Random.Range(0.5f, 1f);
        float height = Random.Range(-1f, -3f);

        base.WalkEffect.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float xPos = Mathf.Lerp(Spawn_Init_Pos.x, MonsterDirector_Pos.x, t);
            float yPos = Mathf.Sin(Mathf.PI * t) * height + Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y, t);
            transform.localPosition = new Vector2(xPos, yPos);
            //Debug.Log(yPos);

            yield return null;
        }
        if (monster_gametype != Monster_GameType.CowBoy_Debuff)
        {
            monster_gametype = Monster_GameType.Fighting;
        }
        Monster_coroutine = null;
    }

    public void attack()
    {
        if (Time.time >= lastTime + (Bullet_Delay + Item_Monster_AtkDelay) - 0.65f)
        {
            if (!warningFlag)
            {
                WarningEffect.Play();
                warningFlag = true;
            }
        }

        if (Time.time >= lastTime + (Bullet_Delay + Item_Monster_AtkDelay) && monster_gametype != Monster_GameType.Die)
        {
            //GameObject bullet = Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            //bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, 0);
            if (!AttackFlag)
            {
                attackTrigger();
                warningFlag = false;
            }
        }
    }

    void attackMove()
    {
        float speed = 10f;    // 회전 속도
        float radius = 1.5f;      // 루프 반경

        elapsedTime += Time.deltaTime * speed ;

        // 한 바퀴(2π)까지만 움직임
        if (elapsedTime >= 2 * Mathf.PI)
        {
            isLoopFinished = true;
            AttackFlag = false;                // 루프 끝나면 공격 종료
            elapsedTime = 0f;                  // 다음 공격 대비 초기화
            transform.localPosition = startPos;
            lastTime = Time.time;
            return;
        }
        float startAngle = Mathf.PI * 3 / 2; // 시작 각도 (90도)
        float angle;
        if(xPos > 0)
        {
            angle = elapsedTime + startAngle;
        }
        else
        {
            angle = startAngle - elapsedTime;
        }

        // 원형 회전
        float xCircle = Mathf.Cos(angle) * radius;
        float yCircle = Mathf.Sin(angle) * radius;
        // 최종 위치 계산
        float x = startPos.x + xCircle;
        float y = startPos.y + yCircle + radius;

        transform.position = new Vector2(x, y);
    }

    public void AniStart()
    {
        Debug.Log("AniStart");
        startPos = transform.localPosition;

    }

    public void BulletFire()
    {
        Debug.Log("BulletFire");
    }

    public void attackTrigger()
    {
        AttackFlag = true;
        anicon.SetBool("Attack", true);
    }

    public void attackTriggerEnd()
    {
        //AttackFlag = false;
        anicon.SetBool("Attack", false);
    }

    void MonsterMove()
    {
        if (transform.position.x >= MonsterDirector.MaxPos_Sky.x)
        {
            xPos = -1f; // 오른쪽 한계 도달 → 왼쪽으로 이동
        }
        else if (transform.position.x <= MonsterDirector.MinPos_Sky.x)
        {
            xPos = 1f; // 왼쪽 한계 도달 → 오른쪽으로 이동
        }
        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * (speed - Item_Monster_Speed) * Time.deltaTime);
    }

    void Check_ItemSpeedSpawn()
    {
        if (MonsterDirector.Item_curseFlag)
        {
            Item_Mosnter_SpeedPersent = MonsterDirector.Item_cursePersent_Spawn;
            Item_Monster_Speed += speed * (Item_Mosnter_SpeedPersent / 100f);
        }
        else if (MonsterDirector.Item_giantFlag)
        {
            Item_Mosnter_SpeedPersent = MonsterDirector.Item_giantPersent_Spawn;
            Item_Monster_Speed += speed * (Item_Mosnter_SpeedPersent / 100f);

        }
        else
        {
            Item_Monster_Speed = 0;
        }
    }

    void Check_ItemSpeedFlag()
    {
        Item_Monster_Speed_ChangeFlag = base.Item_Monster_Speed_ChangeFlag;
        Item_Monster_SpeedFlag = base.Item_Monster_SpeedFlag;
        if (Item_Monster_Speed_ChangeFlag)
        {
            if (Item_Monster_SpeedFlag)
            {
                Item_Monster_Speed += speed * (Item_Mosnter_SpeedPersent / 100f);
                Item_Monster_Speed_ChangeFlag = false;
            }
            else
            {
                Item_Monster_Speed = 0;
                Item_Monster_Speed_ChangeFlag = false;
            }
        }
    }
}