using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDirector : MonoBehaviour
{
    // 스테이지 정보 나온 후, 스테이지에 따라 몬스터 변경해야함
    // 그리고 엑셀에 몬스터 정보도 나와야 한다.
    [HideInInspector]
    public bool Test_Flag;
    [SerializeField]
    int Test_MonsterCount;

    public Game_DataTable EX_GameData;
    public SA_PlayerData SA_PlayerData;
    public MissionDirector missionDirector;

    [Header("몬스터 정보 및 리스트")]
    public Transform Monster_List_Sky;
    public Transform Monster_List_Ground;

    public Transform Boss_List;
    List<int> Emerging_Monster_List_Sky;
    List<int> Emerging_Monster_List_Ground;
    List<int> Emerging_MonsterCount_List;
    [SerializeField]
    List<int> Emerging_Boss_List;

    [Header("미션")]
    public bool missionFlag_material = false;
    public bool missionFlag_monster = false;
    public bool missionFlag_boss = false;
    ItemDataObject missionMaterial_Item;
    int missionMaterial_Drop;

    [Header("보급 몬스터 정보 및 리스트")]
    public Transform SupplyMonster_List;
    public GameObject SupplyMonster_Object;

    [Header("몬스터 공중 스폰 설정")]
    public static Vector2 MaxPos_Sky;
    public static Vector2 MinPos_Sky;

    [Header("몬스터 지상 스폰 설정")]
    public static Vector2 MaxPos_Ground;
    public static Vector2 MinPos_Ground;

    public bool GameDirector_SpawnFlag;
    public bool GameDirector_BossFlag;
    public bool GameDirector_Boss_SpawnFlag;
    public bool GameDirector_RefreshFlag;
    //종료 시
    public bool GameDirector_EndingFlag;
    public bool GameDirecotr_AllDieFlag;

    bool isSpawing = false;
    bool isSupplySpawing = false;

    [Header("몬스터 한도 설정")]
    public int MaxMonsterNum;
    public int MonsterNum;
    int SupplyMonsterNum;
    int item_MonsterCount;
    int item_MonsterCount_Sky;
    int item_MonsterCount_Ground;

    [Header("기차 정보")]
    public Transform Train_List;
    int TrainCount;

    float Random_xPos;
    float Random_yPos;

    int BossCount;

    //Item부분
    [HideInInspector]
    public static bool Item_curseFlag;
    [HideInInspector]
    public static int Item_cursePersent_Spawn;
    [HideInInspector]
    public static bool Item_giantFlag;
    [HideInInspector]
    public static int Item_giantPersent_Spawn;
    [HideInInspector]
    public List<GameObject> Item_Scarecrow_List;


    [Header("Sound")]
    public AudioClip SpawnSFX;

    private void Awake()
    {
        BossCount = 0;
        MonsterNum = 0;
        GameDirector_SpawnFlag = false;
        GameDirector_BossFlag = false;
        GameDirector_Boss_SpawnFlag = false;
        GameDirector_EndingFlag = false;
        GameDirecotr_AllDieFlag = false;
        Item_curseFlag = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        Item_curseFlag = false;
        Item_cursePersent_Spawn = 0;
        Item_giantFlag = false;
        Item_giantPersent_Spawn = 0;

        item_MonsterCount = 0;
        TrainCount = Train_List.childCount;
        MaxPos_Sky = new Vector2(6f, 9f);
        MinPos_Sky = new Vector2(-7.97f + (-10.94f * (TrainCount - 1)), 3f);

        MaxPos_Ground = new Vector2(3.5f, -1.19f);
        MinPos_Ground = new Vector2(-5.47f + (-10.94f * (TrainCount-1)), -1.19f);
        //몬스터 소환 위치가 달라진다.
        //기차 길이에 따라 정해야한다.
    }

    // Update is called once per frame
    void Update()
    {
        MonsterNum = Monster_List_Ground.childCount + Monster_List_Sky.childCount;

        if (GameDirector_RefreshFlag)
        {
            if (GameDirector_SpawnFlag)
            {
                GameDirector_SpawnFlag = false;
            }

            if (!GameDirector_SpawnFlag)
            {
                SupplyMonsterNum = SupplyMonster_List.childCount;
                if (SupplyMonsterNum == 0 && MonsterNum <= 0)
                {
                    if (!GameDirecotr_AllDieFlag)
                    {
                        GameDirecotr_AllDieFlag = true;
                    }
                }
            }
        }

        //Debug.Log(MonsterNum);
        if (!GameDirector_EndingFlag)
        {
            if (GameDirector_SpawnFlag)
            {
                if (!GameDirector_BossFlag)
                {
                    //MonsterNum = Monster_List.childCount;
                    if (MonsterNum < MaxMonsterNum + item_MonsterCount && !isSpawing)
                    {
                        StartCoroutine(AppearMonster(false));
                    }
                }
                else
                {
                    //MonsterNum = Monster_List.childCount;
                    if (MonsterNum < MaxMonsterNum && !isSpawing)
                    {
                        StartCoroutine(AppearMonster(false));
                    }

                    if (!GameDirector_Boss_SpawnFlag)
                    {
                        StartCoroutine(AppearMonster(true));
                        GameDirector_Boss_SpawnFlag = true;
                    }
                }

                SupplyMonsterNum = SupplyMonster_List.childCount;
                if (SupplyMonsterNum < 1 && !isSupplySpawing)
                {
                    StartCoroutine(AppearSupplyMonster());
                }
            }
        }
        else
        {
            if (!GameDirector_SpawnFlag)
            {
                SupplyMonsterNum = SupplyMonster_List.childCount;
                if(SupplyMonsterNum == 0 && MonsterNum <= 0)
                {
                    if (!GameDirecotr_AllDieFlag)
                    {
                        GameDirecotr_AllDieFlag = true;
                    }
                }
            }
        }
    }

    IEnumerator AppearMonster(bool Bossflag)
    {
        isSpawing = true;
        if (!Bossflag)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
            int skyORground = Random.Range(0, 2);
            
            if(skyORground == 0 && Monster_List_Sky.childCount < Emerging_MonsterCount_List[0] + item_MonsterCount_Sky)
            {
                int MonsterRandomIndex = Random.Range(0, Emerging_Monster_List_Sky.Count);
                Check_Sky_OR_Ground_Monster(Emerging_Monster_List_Sky[MonsterRandomIndex], Bossflag, false);
            }

            if (skyORground == 1 && Monster_List_Ground.childCount < Emerging_MonsterCount_List[1] + item_MonsterCount_Ground)
            {
                int MonsterRandomIndex = Random.Range(0, Emerging_Monster_List_Ground.Count);
                Check_Sky_OR_Ground_Monster(Emerging_Monster_List_Ground[MonsterRandomIndex], Bossflag, true);
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            Check_Sky_OR_Ground_Monster(Emerging_Boss_List[BossCount], Bossflag);
        }
        isSpawing = false;
    }

    IEnumerator AppearSupplyMonster() 
    {
        isSupplySpawing = true;
        yield return new WaitForSeconds(Random.Range(4f, 8f));
        Random_xPos = Random.Range(MinPos_Sky.x+10f, MaxPos_Sky.x-10f);
        Random_yPos = Random.Range(MinPos_Sky.y, MaxPos_Sky.y);
        if (GameDirector_SpawnFlag == true)
        {
            Instantiate(SupplyMonster_Object, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, SupplyMonster_List);
        }
        isSupplySpawing = false;
    }

    private void Check_Sky_OR_Ground_Monster(int Monster_Num, bool bossFlag, bool monsterType = false)
    {
        if (!monsterType)
        {
            Random_xPos = Random.Range(MinPos_Sky.x, MaxPos_Sky.x);
            Random_yPos = Random.Range(MinPos_Sky.y, MaxPos_Sky.y);
        }
        else
        {
            Random_xPos = Random.Range(MinPos_Ground.x, MaxPos_Ground.x);
            Random_yPos = Random.Range(MinPos_Ground.y, MaxPos_Ground.y);
        }

        GameObject _Monster = null;
        if (!bossFlag)
        {
            string monster_name = EX_GameData.Information_Monster[Monster_Num].Monster_Name;
            _Monster = Resources.Load<GameObject>("Monster/" + Monster_Num + "_" + monster_name);

            if (missionFlag_material) // 보급 번호
            {
                _Monster.GetComponent<Monster>().Monster_Mission_MaterialFlag = true;
                _Monster.GetComponent<Monster>().SettingMission_Material_Monster(missionMaterial_Item, missionMaterial_Drop);
            }

            if (missionFlag_monster)
            {
                _Monster.GetComponent<Monster>().Monster_Mission_CountFlag = missionDirector.CheckMonster(Monster_Num);
            }

            if (GameDirector_SpawnFlag == true)
            {
                if (!monsterType)
                {
                    Instantiate(_Monster, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, Monster_List_Sky);
                }
                else
                {
                    Instantiate(_Monster, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, Monster_List_Ground);
                }
            }
        }
        else
        {
            string monster_name = EX_GameData.Information_Boss[Monster_Num].Monster_Name;
            _Monster = Resources.Load<GameObject>("Boss/" + Monster_Num + "_" + monster_name);

            if (missionFlag_boss)
            {
                _Monster.GetComponent<Boss>().Boss_MissionFlag = missionDirector.CheckBoss(Monster_Num);
            }

            if (GameDirector_SpawnFlag == true)
            {
                Instantiate(_Monster, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, Boss_List);
            }

            if (SteamAchievement.instance != null)
            {
                SteamAchievement.instance.Achieve("MEET_BOSS_" + Monster_Num);
            }
            else
            {
                Debug.Log("MEET_BOSS_" + Monster_Num);
            }
        }

        MMSoundManagerSoundPlayEvent.Trigger(SpawnSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
    }


    public void Get_Monster_List(List<int> GameDirector_Monster_List_Sky, List<int> GameDirector_Monster_List_Ground, List<int>GameDirector_MonsterCount_List)
    {
        Emerging_Monster_List_Sky = GameDirector_Monster_List_Sky;
        Emerging_Monster_List_Ground = GameDirector_Monster_List_Ground;

        Emerging_MonsterCount_List = GameDirector_MonsterCount_List;

        if (Test_Flag)
        {
            MaxMonsterNum = Test_MonsterCount;
        }
        else
        {
            MaxMonsterNum = 0;
            foreach(int M in Emerging_MonsterCount_List) {
                MaxMonsterNum += M;
            }
        }
    }

    public void Get_Boss_List(List<int> GameDirector_Boss_List)
    {
        Emerging_Boss_List = GameDirector_Boss_List;
    }

    public void BossStart(int boss_monsterCount)
    {
        GameDirector_BossFlag = true;
        GameDirector_Boss_SpawnFlag = false;
        MaxMonsterNum = boss_monsterCount;
    }

    public void BossDie()
    {
        GameDirector_BossFlag = false;
        GameDirector_Boss_SpawnFlag = false;
        MaxMonsterNum = 0;
        foreach (int M in Emerging_MonsterCount_List)
        {
            MaxMonsterNum += M;
        }
        BossCount++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(MaxPos_Sky.x, MaxPos_Sky.y, 0), new Vector3(MaxPos_Sky.x, MinPos_Sky.y, 0));
        Gizmos.DrawLine(new Vector3(MaxPos_Sky.x, MinPos_Sky.y, 0), new Vector3(MinPos_Sky.x, MinPos_Sky.y, 0));
        Gizmos.DrawLine(new Vector3(MinPos_Sky.x, MinPos_Sky.y, 0), new Vector3(MinPos_Sky.x, MaxPos_Sky.y, 0));
        Gizmos.DrawLine(new Vector3(MinPos_Sky.x, MaxPos_Sky.y, 0), new Vector3(MaxPos_Sky.x, MaxPos_Sky.y, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(MaxPos_Ground.x, MaxPos_Ground.y, 0), new Vector3(MaxPos_Ground.x, MinPos_Ground.y, 0));
        Gizmos.DrawLine(new Vector3(MaxPos_Ground.x, MinPos_Ground.y, 0), new Vector3(MinPos_Ground.x, MinPos_Ground.y, 0));
        Gizmos.DrawLine(new Vector3(MinPos_Ground.x, MinPos_Ground.y, 0), new Vector3(MinPos_Ground.x, MaxPos_Ground.y, 0));
        Gizmos.DrawLine(new Vector3(MinPos_Ground.x, MaxPos_Ground.y, 0), new Vector3(MaxPos_Ground.x, MaxPos_Ground.y, 0));
    }

    //아이템부분
    public IEnumerator Item_Monster_FearFlag(int count, int delayTime)
    {
        item_MonsterCount -= count * 2;
        item_MonsterCount_Sky -= count;
        item_MonsterCount_Ground -= count;
        yield return new WaitForSeconds(delayTime);
        item_MonsterCount += count * 2;
        item_MonsterCount_Sky += count;
        item_MonsterCount_Ground += count;
    }

    public IEnumerator Item_Monster_GreedFlag(int count, int delayTime)
    {
        item_MonsterCount += count * 2;
        item_MonsterCount_Sky += count;
        item_MonsterCount_Ground += count;
        yield return new WaitForSeconds(delayTime);
        item_MonsterCount -= count * 2;
        item_MonsterCount_Sky -= count;
        item_MonsterCount_Ground -= count;
    }

    public IEnumerator Item_Use_Monster_CureseFlag(int Persent, int delayTime)
    {
        Item_cursePersent_Spawn = Persent;
        Item_curseFlag = true;
        for (int i = 0; i < Monster_List_Sky.childCount; i++)
        {
            Monster monster = Monster_List_Sky.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_CureseFlag(Persent);
        }
        for (int i = 0; i < Monster_List_Ground.childCount; i++)
        {
            Monster monster = Monster_List_Ground.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_CureseFlag(Persent);
        }
        yield return new WaitForSeconds(delayTime);
        Item_curseFlag = false;
        for (int i = 0; i < Monster_List_Sky.childCount; i++)
        {
            Monster monster = Monster_List_Sky.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_CureseFlag(Persent);
        }
        for (int i = 0; i < Monster_List_Ground.childCount; i++)
        {
            Monster monster = Monster_List_Ground.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_CureseFlag(Persent);
        }
    }

    public void Item_Scarecrow(GameObject scarecrow)
    {
        Item_Scarecrow_List.Add(scarecrow);
    }

    public void Item_Scarecrow_Die(GameObject gm)
    {
        Item_Scarecrow_List.Remove(gm);
    }

    public IEnumerator Item_Use_Monster_GiantFlag(int Persent, int delayTime)
    {
        Item_giantPersent_Spawn = Persent;
        Item_giantFlag = true;
        for(int i = 0; i < Monster_List_Sky.childCount; i++)
        {
            Monster monster = Monster_List_Sky.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_GiantFlag(Persent);
        }
        for (int i = 0; i < Monster_List_Ground.childCount; i++)
        {
            Monster monster = Monster_List_Ground.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_GiantFlag(Persent);
        }
        yield return new WaitForSeconds(delayTime);
        Item_giantFlag = false;
        for (int i = 0; i < Monster_List_Sky.childCount; i++)
        {
            Monster monster = Monster_List_Sky.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_GiantFlag(Persent);
        }
        for (int i = 0; i < Monster_List_Ground.childCount; i++)
        {
            Monster monster = Monster_List_Ground.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_GiantFlag(Persent);
        }
    }


    //미션 부분
    public void SettingMission_Material_MonsterDirector(ItemDataObject _item, int _drop)
    {
        missionMaterial_Item = _item;
        missionMaterial_Drop = _drop;
    }
}