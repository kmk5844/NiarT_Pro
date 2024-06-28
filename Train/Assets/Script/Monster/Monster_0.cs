using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_0 : Monster
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

    protected override void Start()
    {
        base.Start();

        monster_SpawnPos = transform.position;

        speed = Random.Range(0.5f, 2f);
        max_xPos = Random.Range(1, 9);

        xPos = -1f;
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        Check_ItemFlag();
        if (monster_gametype == Monster_GameType.Fighting)
        {
            BulletFire();
            FlipMonster();
        }
        
        if(monster_gametype == Monster_GameType.GameEnding)
        {
            Monster_Ending();
        }

        if (Item_Monster_ChangeFlag)
        {
            if (Item_Mosnter_SpeedFlag)
            {
                Item_Monster_Speed += speed * (Item_Mosnter_SpeedPersent / 100f);
            }
            else
            {
                Item_Monster_Speed -= speed * (Item_Mosnter_SpeedPersent / 100f);
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(monster_gametype == Monster_GameType.Fighting)
        {
            MonsterMove();
        }
    }

    void MonsterMove()
    {
        if (monster_SpawnPos.x - max_xPos > transform.position.x)
        {
            xPos = 1f;
        }
        else if (monster_SpawnPos.x + max_xPos < transform.position.x)
        {
            xPos = -1f;
        }
        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * (speed - Item_Monster_Speed) * Time.deltaTime);
    }

    void Check_ItemFlag()
    {
        if (Item_Monster_ChangeFlag)
        {
            if (Item_Mosnter_SpeedFlag)
            {
                Item_Monster_Speed += speed * (Item_Mosnter_SpeedPersent / 100f);
                Item_Monster_ChangeFlag = false;
            }
            else
            {
                Item_Monster_Speed -= speed * (Item_Mosnter_SpeedPersent / 100f);
                Item_Monster_ChangeFlag = false;
            }
        }
    }
}