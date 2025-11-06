using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_12 : Monster
{
    [SerializeField]
    Vector3 movement;
    int xPos;

    [SerializeField]
    float speed;

    Animator ani;
    public bool attackFlag;
    public bool warningMarkFlag;
    public GameObject WarningMark;
    Warning_Mark_Ani warningMark_ani;
    Transform[] warningmark_object;
    float[] Rand_rotation;

    protected override void Start()
    {
        Monster_Num = 12;
        ani = GetComponent<Animator>();
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);
        player = GameObject.FindWithTag("Player");
        warningMark_ani = WarningMark.GetComponent<Warning_Mark_Ani>();
        Rand_rotation = new float[3];
        warningmark_object = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            warningmark_object[i] = WarningMark.transform.GetChild(i).transform;
        }
        base.Start();

        Spawn_Init_Pos =
            new Vector2(MonsterDirector_Pos.x + Random.Range(2f, 5f),
                MonsterDirector.MinPos_Ground.y - 5f);
        transform.localPosition = Spawn_Init_Pos;

        speed = 2f;

        xPos = -1;
        Check_ItemSpeedSpawn();
        Monster_coroutine = StartCoroutine(SpawnMonster());
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        FlipMonster();
        Check_ItemSpeedFlag();

        if (!DieFlag)
        {
            if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
            {
                attackDelay();

                if (xPos > 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
        }
        

        if (attackFlag)
        {
            ani.SetBool("AttackFlag", true);
        }
        else
        {
            ani.SetBool("AttackFlag", false);
        }

        if (warningMarkFlag)
        {
            if (!warningMark_ani.aniFlag)
            {
                attackFlag = true;
            }
            else
            {
                warningMarkFlag = false;
                warningMark_ani.aniFlag = false;
                WarningMark.SetActive(false);
                BulletFire();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            if (!attackFlag)
            {
                MonsterMove();
            }
        }
    }

    void attackDelay()
    {
        if (Time.time >= lastTime + (Bullet_Delay + Item_Monster_AtkDelay) - 0.65f)
        {
            if (!warningFlag)
            {
                WarningEffect.Play();
                warningFlag = true;
            }
        }

        if (Time.time >= lastTime + (Bullet_Delay + Item_Monster_AtkDelay))
        {
            if (!warningMarkFlag)
            {
                for(int i = 0; i < 3; i++)
                {
                    if (xPos > 0)
                    {
                        Rand_rotation[i] = Random.Range(-40f, -80f);
                        warningmark_object[i].localRotation = Quaternion.Euler(0, 0, -Rand_rotation[i]);
                    }
                    else
                    {
                        Rand_rotation[i] = Random.Range(40f, 80f);
                        warningmark_object[i].localRotation = Quaternion.Euler(0, 0, Rand_rotation[i]);
                    }
                }
            }
            warningMarkFlag = true;
            WarningMark.SetActive(true);
        }
    }

    void BulletFire()
    {
        if(monster_gametype != Monster_GameType.GameEnding && monster_gametype != Monster_GameType.Die)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject bullet = BulletObject;
                bullet.GetComponent<Monster_Bullet_Angle>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, 0);
                bullet.GetComponent<Monster_Bullet_Angle>().SetAngle_And_Fire(Rand_rotation[i]);
                bullet.GetComponent<Monster_Bullet_Angle>().SetAngle_And_Fire(-Rand_rotation[i]);
                Instantiate(bullet, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            }
        }
        attackFlag = false;
        warningFlag = false;
        lastTime = Time.time;
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

    void MonsterMove()
    {
        /*        if (transform.position.x > MonsterDirector.MaxPos_Ground.x || MonsterDirector_Pos.x  < transform.position.x)
                {
                    xPos = -1;
                }
                else if (transform.position.x < MonsterDirector.MinPos_Ground.x || MonsterDirector_Pos.x > transform.position.x)
                {
                    xPos = 1;
                }*/

        if (player.transform.position.x - 1f > transform.position.x)
        {
            xPos = 1;

        }
        else if (player.transform.position.x + 1f < transform.position.x)
        {
            xPos = -1;
        }

        monster_xPos = xPos;
        movement = new Vector3(xPos, 0f, 0f);

        if (player.transform.position.x - 1f > transform.position.x)
        {
            movement = new Vector3(xPos, 0f, 0f);
            ani.SetBool("IdleFlag", false);
        }
        else if (player.transform.position.x + 1f < transform.position.x)
        {
            movement = new Vector3(xPos, 0f, 0f);
            ani.SetBool("IdleFlag", false);
        }
        else
        {
            movement = Vector3.zero;
            ani.SetBool("IdleFlag", true);
        }

        transform.Translate(movement * (speed - Item_Monster_Speed) * Time.deltaTime);
        // 기차의 거리에 맞춰야 한다.
    }

    IEnumerator SpawnMonster()
    {
        float elapsedTime = 0;
        float duration = 1f;
        float height = Random.Range(3f, 5f);
        base.WalkEffect.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float xPos = Mathf.Lerp(Spawn_Init_Pos.x, MonsterDirector_Pos.x, t);
            float yPos = Mathf.Sin(Mathf.PI * t) * height + Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y, t); ;
            transform.localPosition = new Vector2(xPos, yPos);
            //Debug.Log(yPos);

            yield return null;
        }
        transform.localPosition = MonsterDirector_Pos;
        monster_gametype = Monster_GameType.Fighting;
        Monster_coroutine = null;
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