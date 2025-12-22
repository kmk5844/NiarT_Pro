using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_61 : Monster
{
    bool lockFlag = false;
    public Transform FireZoneSub;

    bool attackFlag = false;

    protected override void Start()
    {
        Monster_Num = 61;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);
        base.Start();
        MonsterDirector_Pos =
            new Vector2(MonsterDirector.MaxPos_Ground.x + 1.5f,
                MonsterDirector.MaxPos_Ground.y + 2.7f);
        Spawn_Init_Pos =
            new Vector2(MonsterDirector.MaxPos_Ground.x + 20f,
                MonsterDirector.MaxPos_Ground.y + 20f);
        transform.localPosition = Spawn_Init_Pos;
        Check_ItemSpeedSpawn();
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        Monster_coroutine = StartCoroutine(SpawnMonster());
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();

        if (!DieFlag)
        {
            if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
            {
                if (transform.position.x >= MonsterDirector.MaxPos_Ground.x + 1.5f)
                {
                    transform.Translate(Vector2.left * 20f * Time.deltaTime);
                }
                else
                {
                    if (!lockFlag)
                    {
                        gameDirector.SetMonsterSlow(true);
                        base.WalkEffect.SetActive(true);
                        lockFlag = true;
                        lastTime = Time.time;
                    }
                    else
                    {
                        if (!attackFlag)
                        {
                            AttackAni();
                        }
                    }
                }
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
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float xPos = Spawn_Init_Pos.x;
            float yPos = Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y, t);
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
    void AttackAni()
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
            //anicon.SetBool("Atk", true);
            StartCoroutine(BulletFire());
            attackFlag = true;
        }
    }


    IEnumerator BulletFire()
    {
        if (!DieFlag)
        {
            for (int i = 0; i < 10; i++)
            {
                if(DieFlag)
                {
                    break;
                }
                GameObject bullet = null;
                if (i % 2 == 0)
                {
                    bullet = Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
                }
                else
                {
                    bullet = Instantiate(BulletObject, FireZoneSub.position, transform.rotation, monster_Bullet_List);
                }
                bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, 1);
                yield return new WaitForSeconds(0.12f);
            }
            attackFlag = false;
            warningFlag = false;
            lastTime = Time.time;
        }
    }

    void Check_ItemSpeedSpawn()
    {
        if (MonsterDirector.Item_curseFlag)
        {
            Item_Mosnter_SpeedPersent = MonsterDirector.Item_cursePersent_Spawn;
            //Item_Monster_Speed += speed * (Item_Mosnter_SpeedPersent / 100f);
        }
        else if (MonsterDirector.Item_giantFlag)
        {
            Item_Mosnter_SpeedPersent = MonsterDirector.Item_giantPersent_Spawn;
            //Item_Monster_Speed += speed * (Item_Mosnter_SpeedPersent / 100f);
        }
        else
        {
            Item_Monster_Speed = 0;
        }
    }
}
