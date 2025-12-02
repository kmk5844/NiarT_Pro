using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_36 : Monster
{
    Vector3 movement;
    float xPos;

    private Vector3 startPos;
    private float elapsedTime = 0f;
    float speed;

    bool atkFlag = false;

    public float amplitude = 0.5f;     // 진폭 (위아래 높이)
    public float frequency = 20f;     // 파동 주기 (1초에 몇 번)

    private float timeOffset;
    float max_xPos;

    public GameObject LaserObject;

    protected override void Start()
    {
        Monster_Num = 36;

        base.Start();
        //transform.localPosition = MonsterDirector.MaxPos_Sky;
        MonsterDirector_Pos = transform.localPosition;
        if(MonsterDirector_Pos.y < 7f)
        {
            MonsterDirector_Pos = new Vector2(MonsterDirector_Pos.x, Random.Range(7f, MonsterDirector.MaxPos_Sky.y));
        }
        Spawn_Init_Pos =
            new Vector2(MonsterDirector_Pos.x + Random.Range(4, 12f),
                MonsterDirector.MaxPos_Sky.y + 5f);
        transform.localPosition = Spawn_Init_Pos;

        xPos = -1f;
        max_xPos = Random.Range(4f, 10f);
        timeOffset = Random.Range(0f, Mathf.PI * 2f); // 개체마다 위상 다르게

        speed = Random.Range(2f, 3f);
        amplitude = Random.Range(0.4f, 0.8f);
        frequency = Random.Range(0.5f, 1f);

        Check_ItemSpeedSpawn();
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
                //BulletFire();
                FlipMonster();
            }
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!DieFlag)
        {
            if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
            {
                BulletFire();
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
            StartCoroutine(LaserOn());
        }
    }

    IEnumerator LaserOn()
    {
        atkFlag = false;
        LaserObject.SetActive(true);
        LaserObject.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, 0, 0, 0);
        yield return new WaitForSeconds(4f);
        LaserObject.SetActive(false);
        warningFlag = false;
        atkFlag = true;
        lastTime = Time.time;
    }

    void MonsterMove()
    {
        if (transform.position.x >= MonsterDirector.MaxPos_Sky.x)
        {
            xPos = -1; // 오른쪽 한계 → 왼쪽 이동
        }
        else if (transform.position.x <= MonsterDirector.MinPos_Sky.x)
        {
            xPos = 1;  // 왼쪽 한계 → 오른쪽 이동
        }

        // 시간에 따른 파동 계산
        float sinY = Mathf.Sin((Time.time * frequency) + timeOffset) * amplitude;

        // X축 이동 (좌/우)
        float moveX = xPos * speed * Time.deltaTime;

        // Translate를 이용해 이동
        transform.Translate(moveX, sinY * speed * Time.deltaTime, 0);
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