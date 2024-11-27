using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_6 : Monster
{
    Vector3 movement;

    [SerializeField]
    int xPos;
    float moveSpeed;
    float max_xPos;
    Vector3 monsterScale;
/*
    [SerializeField]
    monster6_Type monsterType;
*/
    float typeTime;
    float jumpTime;

    protected override void Start()
    {
        Monster_Num = 6;
        typeTime = 0f;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);
        base.Start();

        //monsterType = monster6_Type.move;
        MonsterDirector_Pos = transform.localPosition;

        xPos = -1;
        moveSpeed = 2f;
        max_xPos = 3f;
        monsterScale = transform.localScale;
        Check_ItemSpeedSpawn();
        monster_gametype = Monster_GameType.Fighting;
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
        }

        if (monster_gametype == Monster_GameType.GameEnding)
        {
            Monster_Ending();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            MonsterMove();
          /*  if (monsterType == monster6_Type.move)
            {
                typeTime += Time.deltaTime;

                if(Time.time >= typeTime + 5f)
                {
                    monsterType = monster6_Type.jump_up;
                    typeTime = 0f;
                    jumpTime = 0f;
                }
            }

            if(monsterType == monster6_Type.jump_up)
            {
                if(jumpTime < 0.5f)
                {
                    jumpTime += Time.deltaTime;
                    float t = jumpTime / 0.5f;
                    float y = Mathf.Lerp(transform.position.y, transform.position.y + 2, t);
                    transform.position = new Vector3(transform.position.x, y, 0);
                }
                else
                {
                    monsterType = monster6_Type.jump_down;
                    jumpTime = 0f;
                }
            }

            if(monsterType == monster6_Type.jump_attack)
            {
            }

            if(monsterType == monster6_Type.jump_down)
            {
                if(jumpTime < 0.5f)
                {
                    jumpTime += Time.deltaTime;
                    float t = jumpTime / 0.5f;
                    float y = Mathf.Lerp(transform.position.y, transform.position.y + 2, t);
                    transform.position = new Vector3(transform.position.x, y, 0);
                }
                else
                {
                    monsterType = monster6_Type.move;
                    jumpTime = 0f;
                }
            }*/
        }

        if(monster_gametype == Monster_GameType.GameEnding)
        {
            Monster_Ending();
        }
    }

    void MonsterMove()
    {
        if (xPos > 0)
        {
            transform.localScale = new Vector3(monsterScale.x, monsterScale.y, monsterScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-monsterScale.x, monsterScale.y, monsterScale.z);
        }

        if (transform.position.x > MonsterDirector.MaxPos_Ground.x || MonsterDirector_Pos.x + max_xPos < transform.position.x)
        {
            xPos = -1;
            MonsterDirector_Pos = transform.position;
            max_xPos = Random.Range(1f, 4f);
        }
        else if (transform.position.x < MonsterDirector.MinPos_Ground.x || MonsterDirector_Pos.x - max_xPos > transform.position.x)
        {
            xPos = 1;
            MonsterDirector_Pos = transform.position;
            max_xPos = Random.Range(1f, 4f);
        }

        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * (moveSpeed - Item_Monster_Speed) * Time.deltaTime);
    }

    void BulletFire()
    {
        if (Time.time > lastTime + (Bullet_Delay + Item_Monster_AtkDelay))
        {
            GameObject bullet = Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, xPos);
            lastTime = Time.time;
        }
    }
    void Check_ItemSpeedSpawn()
    {
        if (MonsterDirector.Item_curseFlag)
        {
            Item_Mosnter_SpeedPersent = MonsterDirector.Item_cursePersent_Spawn;
            Item_Monster_Speed += moveSpeed * (Item_Mosnter_SpeedPersent / 100f);
        }
        else if (MonsterDirector.Item_giantFlag)
        {
            Item_Mosnter_SpeedPersent = MonsterDirector.Item_giantPersent_Spawn;
            Item_Monster_Speed += moveSpeed * (Item_Mosnter_SpeedPersent / 100f);

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
                Item_Monster_Speed += moveSpeed * (Item_Mosnter_SpeedPersent / 100f);
                Item_Monster_Speed_ChangeFlag = false;
            }
            else
            {
                Item_Monster_Speed = 0;
                Item_Monster_Speed_ChangeFlag = false;
            }
        }
    }

    enum monster6_Type
    {
        move,
        jump_up,
        jump_attack,
        jump_down,
    }
}
