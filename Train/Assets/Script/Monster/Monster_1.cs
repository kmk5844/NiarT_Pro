using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_1 : Monster
{
    [SerializeField]
    Vector3 monster_SpawnPos;
    Vector3 movement;
    int xPos;

    [Header("�ӵ�, �ִ� ����")] // ���� ���긦 �����ؾ��� ���ɼ��� ����
    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;

    protected override void Start()
    {
        base.Start();
        transform.position = new Vector3(transform.position.x, -0.625f, transform.position.z);
        monster_SpawnPos = transform.position;

        speed =  0.1f;
        max_xPos = Random.Range(1, 3);

        xPos = -1;
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
            BulletFire(xPos);
            if (xPos > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        if (monster_gametype == Monster_GameType.GameEnding)
        {
            Monster_Ending();
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
            xPos = 1;
        }
        else if (monster_SpawnPos.x + max_xPos < transform.position.x)
        {
            xPos = -1;
        }
        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * (speed - Item_Monster_Speed) * Time.deltaTime); 
        // ������ �Ÿ��� ����� �Ѵ�.
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
