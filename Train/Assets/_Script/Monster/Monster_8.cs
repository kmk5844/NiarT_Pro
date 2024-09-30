using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_8 : Monster
{
    bool isBombFlag;
    bool isBulletFlag;
    public GameObject WarningMark;
    protected override void Start()
    {
        Monster_Num = 8;
        BulletObject = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);
        
        base.Start();
        isBombFlag = false; 
        MonsterDirector_Pos = transform.localPosition;
        Spawn_Init_Pos =
            new Vector2(MonsterDirector_Pos.x + Random.Range(-10f, 10f),
                MonsterDirector.MaxPos_Sky.y + 5f);
        transform.localPosition = Spawn_Init_Pos;

        StartCoroutine(SpawnMonster());
    }
    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();
        FlipMonster();

        if (isBombFlag)
        {
            isBombFlag = false;
            WarningMark.SetActive(true);
        }

        if (WarningMark.GetComponent<Warning_Mark_Ani>().aniFlag && !isBulletFlag)
        {
            isBulletFlag = true;
            BulletObject.GetComponent<MonsterBullet>().Get_MonsterBullet_Information(Bullet_Atk - (int)Item_Monster_Atk, Bullet_Slow, Bullet_Speed, 0);
            BulletObject.GetComponent<Monster_Bullet_Angle>().SetAngle_And_Fire(30);
            Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            BulletObject.GetComponent<Monster_Bullet_Angle>().SetAngle_And_Fire(10);
            Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            BulletObject.GetComponent<Monster_Bullet_Angle>().SetAngle_And_Fire(-10);
            Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            BulletObject.GetComponent<Monster_Bullet_Angle>().SetAngle_And_Fire(-30);
            Instantiate(BulletObject, Fire_Zone.position, transform.rotation, monster_Bullet_List);
            StartCoroutine(monsterDestory());
        }

        if (monster_gametype == Monster_GameType.GameEnding)
        {
            Monster_Ending();
        }
    }

    IEnumerator SpawnMonster()
    {
        float elapsedTime = 0;
        float duration = Random.Range(4f, 6f);
        float shakeAmplitude = 1f; // 좌우 흔들림의 크기 조절 (원하는 값으로 조절)
        float shakeFrequency = 5f;    // 좌우로 흔들리는 횟수 (n번)

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float xPos = Mathf.Lerp(Spawn_Init_Pos.x, MonsterDirector_Pos.x,t);
            float yPos = Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y,t);
            xPos += Mathf.Sin(t * Mathf.PI * 2 * shakeFrequency) * shakeAmplitude;
            transform.localPosition = new Vector2(xPos, yPos);
            if (monster_gametype == Monster_GameType.CowBoy_Debuff)
            {
                break;
            }
            yield return null;
        }
        if (monster_gametype != Monster_GameType.CowBoy_Debuff)
        {
            monster_gametype = Monster_GameType.Fighting;
            isBombFlag = true;
        }
    }

    IEnumerator monsterDestory()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }
}