using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_25 : Monster
{
    [SerializeField]
    Vector3 movement;
    int xPos;

    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;

    bool Spawn_Itme_Flag = false;
    public GameObject Spawn_Item;

    // Start is called before the first frame update
    protected override void Start()
    {
        Monster_Num = 25;

        base.Start();
        MonsterDirector_Pos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        Spawn_Init_Pos =
            new Vector2(MonsterDirector_Pos.x + Random.Range(2f, 5f),
                MonsterDirector.MinPos_Ground.y - 4f);
        transform.localPosition = Spawn_Init_Pos;

        Check_ItemSpeedSpawn();
        Monster_coroutine = StartCoroutine(SpawnMonster());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Total_GameType();
        Fire_Debuff();

        Check_ItemSpeedFlag();
        if (Monster_HP <= 0 && !Spawn_Itme_Flag)
        {
            Spawn_Itme_Flag = true;
            AfterDie_Spawn_Item();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
    public void AfterDie_Spawn_Item()
    {
        Spawn_Item.GetComponent<SupplyMonster_Item>().SpawnMonster_Flag = true;
        Instantiate(Spawn_Item, transform.position, Quaternion.identity);
        Spawn_Item.GetComponent<SupplyMonster_Item>().SpawnMonster_Flag = false;
    }
}
