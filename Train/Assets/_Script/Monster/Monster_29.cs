using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_29 : Monster
{
    Vector3 movement;
    float xPos;

    float speed = 2f;      // 앞으로 이동 속도
    float jumpHeight = 1.5f;   // 점프 높이
    float jumpFrequency = 3f;  // 점프 주기 (Sin 주기 속도)

    private Vector3 startPos;
    private float elapsedTime = 0f;
    bool loading = false;

    private bool isWaiting = false;    // 대기 중인지 여부
    private float waitTime = 0f;       // 대기 시간 카운트
    public float jumpCooldown = 2f;    // 점프 후 기다릴 시간 (초)

    public Sprite[] FishSprite;
    bool atkFlag;

    protected override void Start()
    {
        Monster_Num = 29;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);

        base.Start();
        //transform.localPosition = MonsterDirector.MaxPos_Sky;
        MonsterDirector_Pos = new Vector2(transform.localPosition.x, transform.localPosition.y - 1f);
        Spawn_Init_Pos =
                new Vector2(MonsterDirector_Pos.x + Random.Range(2f, 5f),
                MonsterDirector.MinPos_Ground.y - 5f);
        transform.localPosition = Spawn_Init_Pos;

        speed = 6f;
        jumpHeight = Random.Range(3f, 5f);
        jumpFrequency = Random.Range(3f, 5f);
        xPos = -1f;
        monsterSprite.sprite = FishSprite[0];

        Check_ItemSpeedSpawn();
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
                MonsterMove();
                //FlipMonster();
            }
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    IEnumerator SpawnMonster()
    {
        float elapsedTime = 0;
        float duration = 0.3f;
        base.WalkEffect.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float xPos = Mathf.Lerp(Spawn_Init_Pos.x, MonsterDirector_Pos.x, t);
            float yPos = Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y, t);
            transform.localPosition = new Vector2(xPos, yPos);
            //Debug.Log(yPos);
            yield return null;
        }
        transform.localPosition = MonsterDirector_Pos;
        startPos = new Vector2(transform.position.x, transform.position.y - 1f);

        monster_gametype = Monster_GameType.Fighting;
        Monster_coroutine = null;
    }
    void BulletFire()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, 0);
            bullet.GetComponent<MonsterBullet>().SetSpeed(Random.Range(8f, 12f));
        }
    }


    void MonsterMove()
    {
        if (isWaiting)
        {
            // 기다리는 중
            waitTime += Time.deltaTime;
            if (waitTime >= jumpCooldown)
            {
                // 대기 끝 → 점프 재개
                atkFlag = false;
                isWaiting = false;
                elapsedTime = 0f; // 점프 초기화
                waitTime = 0f;
            }
            else
            {
                monsterSprite.sprite = FishSprite[0];
                if (transform.position.x >= MonsterDirector.MaxPos_Ground.x - 2f)
                {
                    xPos = -1; // 오른쪽 한계 → 왼쪽 이동
                }
                else if (transform.position.x <= MonsterDirector.MinPos_Ground.x + 2f)
                {
                    xPos = 1;  // 왼쪽 한계 → 오른쪽 이동
                }
                movement = new Vector3(xPos, 0f, 0f);
                transform.Translate(movement * (speed - Item_Monster_Speed) * Time.deltaTime);
                return; // 기다리는 동안 이동 없음
            }
        }

        // --- 기존 점프 로직 ---
        elapsedTime += Time.deltaTime * jumpFrequency;

        float sinValue = Mathf.Sin(elapsedTime);
        // Sin 값으로 위아래 점프 (0~1 구간만 쓰면 자연스럽게)
        float yOffset = Mathf.Abs(Mathf.Sin(elapsedTime)) * jumpHeight;
        // X 방향 이동
        float xOffset = xPos * speed * Time.deltaTime;

        transform.Translate(xOffset, yOffset - (transform.position.y - startPos.y), 0f);
        monsterSprite.sprite = FishSprite[1];

        if (!atkFlag && sinValue >= 0.99f) // 최고점 근처 한 번만
        {
            BulletFire();
            atkFlag = true;
        }

        if (!loading && Mathf.Sin(elapsedTime) <= 0f)
        {
            loading = true;
            jumpHeight = Random.Range(4f, 8f);
            jumpFrequency = Random.Range(3f, 6f);
            // 착지 시점 처리 가능
        }

        // 다음 점프가 시작되면 초기화
        if (elapsedTime >= Mathf.PI)
        {
            elapsedTime = 0f;
            loading = false;

            // ✅ 점프 끝 → 기다리는 상태로 전환
            isWaiting = true;
        }

        // 경계 감지 및 방향 반전
        if (transform.position.x >= MonsterDirector.MaxPos_Ground.x - 2f)
        {
            xPos = -1f; // 오른쪽 한계 → 왼쪽 이동
        }
        else if (transform.position.x <= MonsterDirector.MinPos_Ground.x + 2f)
        {
            xPos = 1f;  // 왼쪽 한계 → 오른쪽 이동
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
