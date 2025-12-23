using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_56 : Monster
{
    Vector3 movement;
    float xPos;

    [Header("속도, 최대 길이")] // 몬스터 무브를 변경해야할 가능성이 높음
    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;

    bool atkFlag;

    public Transform SubFireZone;

    protected override void Start()
    {
        Monster_Num = 56;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);

        base.Start();
        MonsterDirector_Pos = transform.localPosition;
        Spawn_Init_Pos =
            new Vector2(MonsterDirector_Pos.x + Random.Range(4, 12f),
                MonsterDirector.MaxPos_Sky.y + 5f);
        transform.localPosition = Spawn_Init_Pos;

        xPos = -1f;
        speed = 6f;
        Check_ItemSpeedSpawn();
        atkFlag = false;
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
                if (!atkFlag)
                {
                    BulletFire();
                }
                FlipMonster();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            if (!atkFlag)
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

    void BulletFire()
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
            if (!atkFlag)
            {
                StartCoroutine(atk());
            }
        }
    }

    IEnumerator atk()
    {
        atkFlag = true;
        for (int i = 0; i < 12; i++)
        {
            GameObject bullet = null;
            if (i % 2 == 0)
            {
                 bullet = Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            }
            else
            {
                 bullet = Instantiate(BulletObject, SubFireZone.position, transform.rotation, monster_Bullet_List);
            }
            bullet.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, 0);
            yield return new WaitForSeconds(0.4f);
        }
        warningFlag = false;
        lastTime = Time.time;
        atkFlag = false;
    }

    void MonsterMove()
    {
        if (MonsterDirector_Pos.x - max_xPos > transform.position.x)
        {
            xPos = 1f;
        }
        else if (MonsterDirector_Pos.x + max_xPos < transform.position.x)
        {
            xPos = -1f;
        }

        float amplitude = 0.5f;     // y 이동 폭
        float frequency = 4f;       // 흔들리는 속도

        float yPos = Mathf.Sin(Time.time * frequency) * amplitude;

        movement = new Vector3(0f, yPos, 0f);
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