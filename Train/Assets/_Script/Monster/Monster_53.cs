using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_53 : Monster
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

    public GameObject Spawn_Item;
    bool Spawn_Itme_Flag;

    protected override void Start()
    {
        Monster_Num = 53;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);

        base.Start();
        MonsterDirector_Pos = transform.localPosition;
        Spawn_Init_Pos =
          new Vector2(MonsterDirector_Pos.x + Random.Range(4, 12f),
                MonsterDirector.MaxPos_Sky.y + 5f);
        transform.localPosition = Spawn_Init_Pos;
        speed = 10f;
        moveDir = Random.insideUnitCircle.normalized;

        xPos = -1;
        Spawn_Itme_Flag = false;
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
                FlipMonster();
            }
        }

        if(Monster_HP <= 0 && !Spawn_Itme_Flag)
        {
            Spawn_Itme_Flag = true;
            AfterDie_Spawn_Item();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!DieFlag)
        {
            if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
            {
                MonsterMove();
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
    public void AfterDie_Spawn_Item()
    {
        Spawn_Item.GetComponent<SupplyMonster_Item>().SpawnMonster_Flag = true;
        Instantiate(Spawn_Item, transform.position, Quaternion.identity);
        Spawn_Item.GetComponent<SupplyMonster_Item>().SpawnMonster_Flag = false;
    }
}