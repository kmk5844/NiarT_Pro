using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_13 : Monster
{
    [SerializeField]
    Vector3 movement;
    float xPos;
    Vector2 Init_Move;

    [SerializeField]
    float speed;

    bool MaxMinFlag;

    bool AttackFlag;

    protected override void Start()
    {
        Monster_Num = 13;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);
        
        base.Start();
        transform.localPosition = MonsterDirector.MaxPos_Sky;
        MonsterDirector_Pos = transform.localPosition;
        MaxMinFlag = true;
        monster_13_FlipMonster();

        Spawn_Init_Pos =
            new Vector2(MonsterDirector_Pos.x + Random.Range(4, 12f),
                MonsterDirector.MaxPos_Sky.y + 5f);
        transform.localPosition = Spawn_Init_Pos;

        speed = Random.Range(0.5f, 2f);
        AttackFlag = false;
        xPos = -1f;
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
            if (!AttackFlag) {
                Attack();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            MonsterMove();
        }
    }

    void Attack()
    {
        if (Time.time >= lastTime + (Bullet_Delay + Item_Monster_AtkDelay) && monster_gametype != Monster_GameType.Die)
        {
            AttackFlag = true;
            Monster_coroutine = StartCoroutine(FireCorutine());
        }
    }

    void MonsterMove()
    {
        if (!AttackFlag)
        {
            float offset = Mathf.Sin(Time.time * 3f) * 1f; // 2 : 스피드, 1.0f 이동거리
            Vector2 movement = new Vector2(0f, offset);
            transform.Translate(movement * Time.deltaTime);
        }
    }

    IEnumerator FireCorutine()
    {
        AttackFlag = true;
        Vector2 check_1 = new Vector2(MonsterDirector.MaxPos_Sky.x, MonsterDirector.MaxPos_Sky.y -2);
        Vector2 check_2 = new Vector2(MonsterDirector.MinPos_Sky.x, MonsterDirector.MaxPos_Sky.y -2);
        float distance;
        float StartTime = Time.time;
        bool BulletFlag = false;
        if (MaxMinFlag)
        {
            distance = Vector3.Distance(check_1, check_2);
        }
        else
        {
            distance = Vector3.Distance(check_2, check_1);
        }

        while (true)
        {
            float elapsed = (Time.time - StartTime) * 20f;
            float t = elapsed / distance;
            if (t >= 1.0f)
            {
                if (MaxMinFlag)
                {
                    MaxMinFlag = false;
                }
                else
                {
                    MaxMinFlag = true;
                }
                monster_13_FlipMonster();
                break;
            }

            if (monster_gametype != Monster_GameType.GameEnding)
            { 
                if (t >= 0.4f && !BulletFlag)
                {
                    GameObject bullet = Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
                    bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, 0);
                    StartCoroutine(BulletFire(bullet));
                    BulletFlag = true;
                }
            }
           
            Vector2 newPos;
            if (MaxMinFlag)
            {
                newPos = Vector2.Lerp(check_1, check_2, t);
            }
            else
            {
                newPos = Vector2.Lerp(check_2, check_1, t);
            }
            newPos.y += Mathf.Cos(2*t * Mathf.PI * 1) * 1.2f;
            transform.position = newPos;

            yield return null;
        }
        AttackFlag = false;
        lastTime = Time.time;
        Monster_coroutine = null;
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
        Init_Move = transform.localPosition;
        Monster_coroutine = null;
    }

    IEnumerator BulletFire(GameObject _Bullet)
    {
        Instantiate(_Bullet, Fire_Zone.transform.position, Quaternion.identity, monster_Bullet_List);
        yield return new WaitForSeconds(0.2f);
        Instantiate(_Bullet, Fire_Zone.transform.position, Quaternion.identity, monster_Bullet_List);
    }

    void monster_13_FlipMonster()
    {
        if (MaxMinFlag)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            //AfterImage_Particle.transform.localScale = new Vector3(-AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            //AfterImage_Particle.transform.localScale = new Vector3(AfterImage_Particle_LocalScale_X, AfterImage_Particle_LocalScale_Y, 1);
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
