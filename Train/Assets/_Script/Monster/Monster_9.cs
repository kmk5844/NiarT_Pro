using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_9 : Monster
{

    [SerializeField]
    Vector3 movement;
    int xPos;

    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;
    Animator ani;
    bool attackFlag;

    int Random_BulletCount;

    // Start is called before the first frame update
    protected override void Start()
    {
        Monster_Num = 9;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);
        ani = GetComponent<Animator>();

        base.Start();
        MonsterDirector_Pos = transform.localPosition;
        Spawn_Init_Pos =
            new Vector2(MonsterDirector_Pos.x + Random.Range(2f, 5f),
                MonsterDirector.MinPos_Ground.y - 5f);
        transform.localPosition = Spawn_Init_Pos;

        speed = 1f;
        max_xPos = Random.Range(1, 3);

        xPos = -1;
        Check_ItemSpeedSpawn();
        Monster_coroutine = StartCoroutine(SpawnMonster());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        FlipMonster();

        Check_ItemSpeedFlag();

        if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            if (attackFlag == false)
            {
                AttackTrigger();
            }
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

    void AttackTrigger()
    {
        if (Time.time >= lastTime + (Bullet_Delay + Item_Monster_AtkDelay))
        {
            Random_BulletCount = Random.Range(2, 6);
            ani.SetTrigger("Attack");
            attackFlag = true;
        }
    }

    void _BulletFire()
    {
        if(monster_gametype != Monster_GameType.GameEnding)
        {
            for (int i = 0; i < Random_BulletCount; i++)
            {
                GameObject bullet = BulletObject;
                Bullet_Speed = Random.Range(15f, 25f);
                float Bullet_Angle = Random.Range(180f, -180f);
                bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, xPos);
                bullet.GetComponent<Monster_Bullet_Angle>().SetAngle_And_Fire(Bullet_Angle);
                Instantiate(bullet, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            }
        }
        lastTime = Time.time;
        attackFlag = false;
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
        if (transform.position.x > MonsterDirector.MaxPos_Ground.x || MonsterDirector_Pos.x + max_xPos < transform.position.x)
        {
            xPos = -1;
        }
        else if (transform.position.x < MonsterDirector.MinPos_Ground.x || MonsterDirector_Pos.x - max_xPos > transform.position.x)
        {
            xPos = 1;
        }
        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * (speed - Item_Monster_Speed) * Time.deltaTime);
        // 기차의 거리에 맞춰야 한다.
    }

    IEnumerator SpawnMonster()
    {
        float elapsedTime = 0;
        float duration = 1f;
        float height = Random.Range(3f, 5f);

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
