using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Coffee.UIExtensions.UIParticleAttractor;

public class Monster_7 : Monster
{
    Vector3 movement;
    [SerializeField]
    float speed;

    Monster7_State moveType;
    Vector2 jumpBefore_Pos;
    Vector2 jump_Pos;
    Vector2 jumpAfter_Pos;
    Animator ani;

    protected override void Start()
    {
        Monster_Num = 7;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);
        ani = GetComponent<Animator>();
        moveType = Monster7_State.BeforeFighting;

        base.Start();
        MonsterDirector_Pos = new Vector2(transform.localPosition.x, transform.localPosition.y -1f);
        Spawn_Init_Pos =
            new Vector2(MonsterDirector.MaxPos_Ground.x + 15f,
             MonsterDirector.MaxPos_Ground.y - 1f);
        transform.localPosition = Spawn_Init_Pos;

        speed = 5f; // 추후에 랜덤으로 바뀔 예정

        Check_ItemSpeedSpawn();
        Monster_coroutine = StartCoroutine(SpawnMonster());
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        Check_ItemSpeedFlag();
    }

    protected override void FixedUpdate()
    {
        if(monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            if(moveType == Monster7_State.back)
            {
                if(transform.localPosition.x > MonsterDirector_Pos.x - 12f)
                {
                    movement = new Vector3(-1f, 0f, 0f);
                    transform.Translate(movement * (speed - Item_Monster_Speed) * Time.deltaTime);
                }
                else
                {
                    jumpBefore_Pos = transform.localPosition;
                    moveType = Monster7_State.jump_Before;
                }
            }

            if(moveType == Monster7_State.jump_Before)
            {
                if(transform.localPosition.x < jumpBefore_Pos.x + 5f)
                {
                    movement = new Vector3(1f, 0f, 0f);
                    transform.Translate(movement * ((speed + 3f) - Item_Monster_Speed) * Time.deltaTime);
                }
                else
                {
                    jump_Pos = transform.localPosition;
                    moveType = Monster7_State.jump;
                    ani.SetTrigger("Jump_Ani");
                    Monster_coroutine = StartCoroutine(Jump());
                }
            }

            if(moveType == Monster7_State.jump_After)
            {
                if(transform.localPosition.x >  jumpAfter_Pos.x - 3f)
                {
                    movement = new Vector3(-1f, 0f, 0f);
                    transform.Translate(movement * (speed - Item_Monster_Speed) * Time.deltaTime);
                }
                else
                {
                    moveType = Monster7_State.back;
                }
            }

        }
    }

    IEnumerator SpawnMonster()
    {
        float elapsedTime = 0;
        float duration = 1.5f;
    
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime  / duration);
            float xPos = Mathf.Lerp(Spawn_Init_Pos.x, MonsterDirector_Pos.x, t);
            transform.localPosition = new Vector2(xPos, transform.localPosition.y);
            yield return null;
        }
        monster_gametype = Monster_GameType.Fighting;
        moveType = Monster7_State.back;
        Monster_coroutine = null;
    }

    IEnumerator Jump()
    {
        bool bulletFlag = false;
        float elapsedTime = 0f;
        float jump_duration = 1f;
        float height = Random.Range(5f, 10f);
        jumpAfter_Pos = new Vector2(jump_Pos.x + Random.Range(5f, 8f), transform.position.y);
        while (elapsedTime < jump_duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / jump_duration);
            float xPos = Mathf.Lerp(jump_Pos.x, jumpAfter_Pos.x , t);
            float yPos = Mathf.Sin(Mathf.PI * t) * height + jump_Pos.y;

            if (t >= 0.5f && !bulletFlag)
            {
                bulletFlag = true;
                BulletFire();
            }

            /*if (monster_gametype != Monster_GameType.GameEnding)
            {
                
            }*/

            transform.localPosition = new Vector2(xPos, yPos);
            yield return null;
        }
        MonsterDirector_Pos = new Vector2(jumpAfter_Pos.x + Random.Range(-2f, 2f), transform.position.y);
        if(MonsterDirector_Pos.x - 5f <= MonsterDirector.MinPos_Ground.x)
        {
            MonsterDirector_Pos = new Vector2(MonsterDirector.MinPos_Ground.x + Random.Range(6f, 12f), transform.position.y);
        }
        else if(MonsterDirector_Pos.x >= MonsterDirector.MaxPos_Ground.x + 1f)
        {
            MonsterDirector_Pos = new Vector2(MonsterDirector.MaxPos_Ground.x - Random.Range(1f, 3f), transform.position.y);
        }

        moveType = Monster7_State.back;
        Monster_coroutine = null;

    }

    public void BulletFire()
    {
        if(monster_gametype != Monster_GameType.Die)
        {
            GameObject bullet = Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, 0);
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

    public enum Monster7_State
    {
        BeforeFighting,


        back,
        jump_Before,
        jump,
        jump_After
    }
}
