using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_20 : Monster
{
    bool lockFlag = false;
    protected override void Start()
    {
        Monster_Num = 20;
        base.Start();
        MonsterDirector_Pos =
            new Vector2(MonsterDirector.MaxPos_Ground.x + 10f,
                MonsterDirector.MaxPos_Ground.y + 2);
        Spawn_Init_Pos =
            new Vector2(MonsterDirector.MaxPos_Ground.x + 20f,
                MonsterDirector.MaxPos_Ground.y - 10f);
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
        float duration = 1.5f;
        float height = 5f;


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
