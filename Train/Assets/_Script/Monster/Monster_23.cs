using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_23 : Monster
{
    [SerializeField]
    Vector3 movement;
    int xPos;

    [SerializeField]
    float speed;
    private Vector2 moveDir;
    private float changeTimer = 0f;
    float directionChangeInterval = 1.0f; // 방향 변경 주기
    float randomAngleRange = 45f;
    [HideInInspector]
    public bool BossSignalFlag;
    [HideInInspector]
    Monster_Boss_3 boss3director;
    [HideInInspector]
    public bool BeeHome_Flag = false;

    protected override void Start()
    {
        Monster_Num = 23;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);

        base.Start();
        if(!BeeHome_Flag)
        {
            MonsterDirector_Pos = transform.localPosition;
            Spawn_Init_Pos =
              new Vector2(MonsterDirector_Pos.x + Random.Range(4, 12f),
                    MonsterDirector.MaxPos_Sky.y + 5f);
            transform.localPosition = Spawn_Init_Pos;
        }
        speed = 6f;
        moveDir = Random.insideUnitCircle.normalized;

        xPos = -1;
        Check_ItemSpeedSpawn();
        Monster_coroutine = StartCoroutine(SpawnMonster());
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        Check_ItemSpeedFlag();

        if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            if (!BossSignalFlag)
            {
                BulletFire();
            }
            FlipMonster();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            MonsterMove();
        }
    }

    IEnumerator SpawnMonster()
    {
        float elapsedTime = 0;
        float duration = Random.Range(0.5f, 1f);
        float height = Random.Range(-1f, -3f);

        base.WalkEffect.SetActive(true);

        if (!BeeHome_Flag)
        {
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
        }
        else
        {
            Spawn_Init_Pos = transform.position;
            Vector2 originPos = new Vector2(Random.Range(MonsterDirector.MinPos_Sky.x, MonsterDirector.MaxPos_Sky.x),
                                            Random.Range(MonsterDirector.MinPos_Sky.y, MonsterDirector.MaxPos_Sky.y));
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                float xPos = Mathf.Lerp(Spawn_Init_Pos.x, originPos.x, t);
                float yPos = Mathf.Sin(Mathf.PI * t) * height + Mathf.Lerp(Spawn_Init_Pos.y, originPos.y, t);
                transform.localPosition = new Vector2(xPos, yPos);
                //Debug.Log(yPos);

                yield return null;
            }
        }
        if (monster_gametype != Monster_GameType.CowBoy_Debuff)
        {
            monster_gametype = Monster_GameType.Fighting;
        }
        Monster_coroutine = null;
    }

    void BulletFire()
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
            GameObject bullet = Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, 0);
            warningFlag = false;
            lastTime = Time.time;
        }
    }


    void MonsterMove()
    {
        if (!BossSignalFlag)
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
        else
        {
            // 방향 계산
            Vector3 dir = (boss3director.transform.position - transform.position).normalized;
            // 이동 (Translate 사용)
            transform.Translate(dir * 20f * Time.deltaTime, Space.World);

            // 회전 (이동 방향으로)
            transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

            // 거리 체크해서 가까워지면 파괴
            float distance = Vector3.Distance(transform.position, boss3director.transform.position);
            if (distance <= 1f)
            {
                HealBoss();
            }
        }
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

    public void HealBossSet(GameObject Boss)
    {
        BossSignalFlag = true;
        boss3director = Boss.GetComponent<Monster_Boss_3>();
    }

    void HealBoss()
    {
        boss3director.BossHPHeal();
        Destroy(gameObject);
    }
}