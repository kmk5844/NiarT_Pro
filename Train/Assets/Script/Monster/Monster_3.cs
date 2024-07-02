using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_3 : Monster
{
    [SerializeField]
    Vector3 monster_SpawnPos;
    Vector3 movement;
    float xPos;

    [Header("속도, 최대 길이")] // 몬스터 무브를 변경해야할 가능성이 높음
    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;

    [SerializeField]
    float frequency;
    [SerializeField]
    float amplitude;

    protected override void Start()
    {
        base.Start();

        monster_SpawnPos = transform.position;

        speed = Random.Range(0.5f, 2f);
        max_xPos = Random.Range(1, 9);
        frequency = Random.Range(5f, 10f);
        amplitude = Random.Range(0.5f, 1f);

        xPos = -1f;
        Check_ItemSpeedSpawn();
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        Check_ItemSpeedFlag();

        if (monster_gametype == Monster_GameType.Fighting)
        {
            BulletFire();
            FlipMonster();
        }

        if (monster_gametype == Monster_GameType.GameEnding)
        {
            Monster_Ending();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (monster_gametype == Monster_GameType.Fighting)
        {
            MonsterMove();
        }
    }

    void MonsterMove()
    {
        float yPos = Mathf.Sin(Time.time * frequency) * amplitude;

        if (monster_SpawnPos.x - max_xPos > transform.position.x)
        {
            xPos = 1f;
        }
        else if (monster_SpawnPos.x + max_xPos < transform.position.x)
        {
            xPos = -1f;
        }
        movement = new Vector3(xPos, yPos, 0f);
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
