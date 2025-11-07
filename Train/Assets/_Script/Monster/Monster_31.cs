using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_31 : Monster
{
    [SerializeField]
    Vector3 movement;
    float xPos;

    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;
    [SerializeField]
    private Vector2 direction;
    bool attackFlag;
    public Sprite[] FishSprite;
    public MonsterBullet bullet;

    // Start is called before the first frame update
    protected override void Start()
    {
        Monster_Num = 31;

        base.Start();
        MonsterDirector_Pos = new Vector2(transform.localPosition.x, transform.localPosition.y - 2f);
        Spawn_Init_Pos =
            new Vector2(MonsterDirector_Pos.x + Random.Range(2f, 5f),
                MonsterDirector.MinPos_Ground.y - 7f);
        transform.localPosition = Spawn_Init_Pos;

        speed = 2f;
        max_xPos = Random.Range(1, 3);

        xPos = -1;
        Check_ItemSpeedSpawn();
        Monster_coroutine = StartCoroutine(SpawnMonster());
        monsterSprite.sprite = FishSprite[0];
        bullet.atk = Bullet_Atk;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        //FlipMonster();

        if (!DieFlag)
        {
            if (monster_gametype == Monster_GameType.Fighting || monster_gametype == Monster_GameType.GameEnding)
            {
                if(!attackFlag)
                {
                    BulletFire();
                }
                else
                {
                    transform.Translate(direction * speed * Time.deltaTime, Space.World);
                }
            }
        }
        Check_ItemSpeedFlag();
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
            monsterSprite.sprite = FishSprite[1];
            direction = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            speed = 20f;
            Destroy(gameObject, 3f);
            attackFlag = true;
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
        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * (speed - Item_Monster_Speed) * Time.deltaTime);
    }

    IEnumerator SpawnMonster()
    {
        float elapsedTime = 0;
        float duration = 0.3f;
        base.WalkEffect.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float xPos = Mathf.Lerp(Spawn_Init_Pos.x, MonsterDirector_Pos.x, t);
            float yPos = Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y, t);
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
