using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_40 : Monster
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
    [SerializeField]
    bool atkFlag = false;

    public GameObject LaserObjcet;

    // Start is called before the first frame update
    protected override void Start()
    {
        Monster_Num = 40;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);

        base.Start();

        float posX = Random.Range(MonsterDirector.MinPos_Sky.x + 4, MonsterDirector.MaxPos_Sky.x - 4);
        MonsterDirector_Pos = new Vector2(posX, 2.5f);
        Spawn_Init_Pos =
          new Vector2(MonsterDirector_Pos.x + Random.Range(4, 12f),
                MonsterDirector.MaxPos_Sky.y + 5f);
        transform.localPosition = Spawn_Init_Pos;
        speed = 3f;
        moveDir = Random.insideUnitCircle.normalized;

        xPos = -1;
        Check_ItemSpeedSpawn();
        LaserObjcet.SetActive(false);
        Monster_coroutine = StartCoroutine(SpawnMonster());
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        Check_ItemSpeedFlag();

        if (!DieFlag)
        {
            if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
            {
                BulletFire();

                if (!atkFlag)
                {
                    FlipMonster();
                }
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!DieFlag)
        {
            if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
            {
                if (!atkFlag)
                {
                    MonsterMove();
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
            if (!atkFlag)
            {
                StartCoroutine(atk());
            }
        }
    }

    IEnumerator atk()
    {
        atkFlag = true;
        // 1) 초기 랜덤 회전
        LaserObjcet.SetActive(true);

        float baseAngle = -40f;

        // 몬스터 플립 기준 각도 설정
        float angle = baseAngle;
        if (transform.localScale.x < 0f)  // 부모(몬스터)가 좌우 반전 상태라면
        {
            angle = 380f;  // 반대 방향 거울 각도
        }

        // 정확한 초기 각도 적용
        LaserObjcet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 회전 방향도 몬스터 기준
        LaserObjcet.GetComponent<MonsterBullet>()
            .Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, 0, 0, 0);

        float rotatedAngle = 0f;
        float rotateSpeed = 40f;

        // 2) 매 프레임마다 회전
        while (true)
        {
            float delta = rotateSpeed * Time.deltaTime;
            rotatedAngle += delta;

            // 레이저들 전부 회전 적용
            LaserObjcet.transform.Rotate(0, 0, delta * -1);

            if (rotatedAngle >= 90f)
                break;

            yield return null; // 다음 프레임까지 기다림
        }

        LaserObjcet.SetActive(false);
        warningFlag = false;
        lastTime = Time.time;
        atkFlag = false;
    }

    void MonsterMove()
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
