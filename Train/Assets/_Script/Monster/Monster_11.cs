using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_11 : Monster
{
    [SerializeField]
    Vector3 movement;
    int xPos;

    [SerializeField]
    float speed;

    Animator ani;
    public bool attackFlag;
    public GameObject PlayerZone;

    protected override void Start()
    {
        Monster_Num = 11;
        ani = GetComponent<Animator>();
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
    }

    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        FlipMonster();
        Check_ItemSpeedFlag();

        if (monster_gametype == Monster_GameType.Fighting)
        {
/*            if (attackFlag == false)
            {
                //AttackTrigger();
            }*/
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

        if (!PlayerZone.activeSelf)
        {
            attackFlag = true;
            speed = 0f;
        }
        else
        {
            attackFlag = false;
            speed = 4f;
        }

        if (monster_gametype == Monster_GameType.Fighting)
        {
            if (!attackFlag)
            {
                MonsterMove();
            }
        }
    }

    public void AttackTrigger()
    {
        ani.SetTrigger("Attack");
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

        if(player.transform.position.x - 1f > transform.position.x)
        {
            xPos = 1;

        }
        else if(player.transform.position.x + 1f < transform.position.x)
        {
            xPos = -1;
        }

        monster_xPos = xPos;
        movement = new Vector3(xPos, 0f, 0f);

        if (player.transform.position.x - 1f > transform.position.x)
        {
            movement = new Vector3(xPos, 0f, 0f);
        }
        else if (player.transform.position.x + 1f < transform.position.x)
        {
            movement = new Vector3(xPos, 0f, 0f);
        }
        else
        {
            if(xPos == -1)
            {
                transform.position = new Vector2(transform.position.x - 2, transform.position.y);
                xPos = 1;
            }else if(xPos == 1)
            {
                transform.position = new Vector2(transform.position.x + 2, transform.position.y);
                xPos = -1;
            }
        }

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
