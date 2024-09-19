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
        Bullet = Resources.Load<GameObject>("Bullet/Monster/" + Monster_Num);
        
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
            /*Instantiate(Bullet);
            Instantiate(Bullet);
            Instantiate(Bullet);
            Instantiate(Bullet);*/
            Debug.Log("ÃÑ¾Ë¹ß»ç1");
            Debug.Log("ÃÑ¾Ë¹ß»ç2");
            Debug.Log("ÃÑ¾Ë¹ß»ç3");
            Debug.Log("ÃÑ¾Ë¹ß»ç4");
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
        float shakeAmplitude = 1f; // ÁÂ¿ì Èçµé¸²ÀÇ Å©±â Á¶Àý (¿øÇÏ´Â °ªÀ¸·Î Á¶Àý)
        float shakeFrequency = 5f;    // ÁÂ¿ì·Î Èçµé¸®´Â È½¼ö (n¹ø)

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float xPos = Mathf.Lerp(Spawn_Init_Pos.x, MonsterDirector_Pos.x,t);
            float yPos = Mathf.Lerp(Spawn_Init_Pos.y, MonsterDirector_Pos.y,t);
            xPos += Mathf.Sin(t * Mathf.PI * 2 * shakeFrequency) * shakeAmplitude;
            transform.localPosition = new Vector2(xPos, yPos);
            yield return null;
        }
        isBombFlag = true;
    }
}