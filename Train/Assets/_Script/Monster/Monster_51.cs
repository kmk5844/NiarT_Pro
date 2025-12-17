using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_51 : Monster
{
    [SerializeField]
    Vector3 movement;
    int xPos;

    [SerializeField]
    float speed;
    bool attackFlag;
    bool atkCorutineFlag;
    public GameObject ShortAtkObject;

    // Start is called before the first frame update
    protected override void Start()
    {
        Monster_Num = 51;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);

        base.Start();
        MonsterDirector_Pos = transform.localPosition;
        Spawn_Init_Pos =
            new Vector2(MonsterDirector_Pos.x + Random.Range(2f, 5f),
                MonsterDirector.MinPos_Ground.y - 5f);

        transform.localPosition = Spawn_Init_Pos;

        speed = 4f;
        xPos = -1;
        Check_ItemSpeedSpawn();
        Monster_coroutine = StartCoroutine(SpawnMonster());
        ShortAtkObject.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, 0, 0, 0);
        //anicon = GetComponent<Animator>();
        attackFlag = false;

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
                FlipMonster_();
            }
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
        {
            attackDelay();
            MonsterMove();
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
            if (!atkCorutineFlag)
            {
                StartCoroutine(atkMove());
            }
        }
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
            float yPos = Mathf.Sin(Mathf.PI * t) * height + Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y + 0.7f, t); ;
            transform.localPosition = new Vector2(xPos, yPos);
            //Debug.Log(yPos);

            yield return null;
        }
        MonsterDirector_Pos = new Vector2(MonsterDirector_Pos.x, MonsterDirector.MinPos_Ground.y + 0.7f);
        transform.localPosition = MonsterDirector_Pos;
        monster_gametype = Monster_GameType.Fighting;
        Monster_coroutine = null;
    }

    IEnumerator atkMove()
    {
        atkCorutineFlag = true;
        speed = 12f;
        yield return new WaitForSeconds(4f);
        lastTime = Time.time;
        speed = 4f;
        atkCorutineFlag = false;
    }

    void FlipMonster_()
    {
        if (xPos > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void MonsterMove()
    {
        if (transform.position.x >= MonsterDirector.MaxPos_Ground.x - 2f)
        {
            xPos = -1; // 오른쪽 한계 → 왼쪽 이동
        }
        else if (transform.position.x <= MonsterDirector.MinPos_Ground.x + 2f)
        {
            xPos = 1;  // 왼쪽 한계 → 오른쪽 이동
        }
        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * (speed - Item_Monster_Speed) * Time.deltaTime);
        // 기차의 거리에 맞춰야 한다.
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
