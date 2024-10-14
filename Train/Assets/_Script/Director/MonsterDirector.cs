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

    [Header("몬스터 정보 및 리스트")]
    public Transform Monster_List;

    [SerializeField]
    Transform[] Test_Monster_List;


    public Transform Boss_List;
    List<int> Emerging_Monster_List;
    List<int> Emerging_MonsterCount_List;
    [SerializeField]
    List<int> Emerging_Boss_List;

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

    bool isSpawing = false;
    bool isSupplySpawing = false;

    [Header("몬스터 한도 설정")]
    public int MaxMonsterNum;
    [SerializeField]
    public static int MonsterNum;
    int SupplyMonsterNum;
    int item_MonsterCount;

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

    private void Awake()
    {
        BossCount = 0;
        MonsterNum = 0;
        GameDirector_SpawnFlag = false;
        GameDirector_BossFlag = false;
        GameDirector_Boss_SpawnFlag = false;
        Item_curseFlag = false;

    }
    // Start is called before the first frame update
    void Start()
    {
        Item_curseFlag = false;

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
        //Debug.Log(MonsterNum);
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
            if(SupplyMonsterNum < 1 && !isSupplySpawing)
            {
                StartCoroutine(AppearSupplyMonster());
            }
        }


/*        if (GameDirector_SpawnFlag && !GameDirector_BossFlag)
        {
            MonsterNum = Monster_List.childCount;
            if (MonsterNum < MaxMonsterNum + item_MonsterCount && !isSpawing)
            {
                StartCoroutine(AppearMonster(false));
            }
        }

        if(GameDirector_SpawnFlag && GameDirector_BossFlag)
        {
            MonsterNum = Monster_List.childCount;
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
*/
    }

    IEnumerator AppearMonster(bool Bossflag)
    {
        isSpawing = true;
        if (!Bossflag)
        {
            yield return new WaitForSeconds(Random.Range(0.8f, 1.1f));
            int MonsterRandomIndex = Random.Range(0, Emerging_Monster_List.Count);
            if (Test_Monster_List[MonsterRandomIndex].childCount != Emerging_MonsterCount_List[MonsterRandomIndex])
            {
                Check_Sky_OR_Ground_Monster(Emerging_Monster_List[MonsterRandomIndex], Bossflag, MonsterRandomIndex);
            }
        }
        else
        {
            yield return new WaitForSeconds(2f);
            Check_Sky_OR_Ground_Monster(Emerging_Boss_List[BossCount], Bossflag);
        }
        isSpawing = false;
    }

    IEnumerator AppearSupplyMonster() 
    {
        isSupplySpawing = true;
        yield return new WaitForSeconds(Random.Range(10f, 15f));
        Random_xPos = Random.Range(MinPos_Sky.x, MaxPos_Sky.x);
        Random_yPos = Random.Range(MinPos_Sky.y, MaxPos_Sky.y);
        if (GameDirector_SpawnFlag == true)
        {
            Instantiate(SupplyMonster_Object, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, SupplyMonster_List);
        }
        isSupplySpawing = false;
    }

    private void Check_Sky_OR_Ground_Monster(int Monster_Num, bool bossFlag, int index = -1)
    {
        if (EX_GameData.Information_Monster[Monster_Num].Monster_Type == "Sky")
        {
            Random_xPos = Random.Range(MinPos_Sky.x, MaxPos_Sky.x);
            Random_yPos = Random.Range(MinPos_Sky.y, MaxPos_Sky.y);
        }
        else if (EX_GameData.Information_Monster[Monster_Num].Monster_Type == "Ground")
        {
            Random_xPos = Random.Range(MinPos_Ground.x, MaxPos_Ground.x);
            Random_yPos = Random.Range(MinPos_Ground.y, MaxPos_Ground.y);
        }
        GameObject _Monster = null;
        if (!bossFlag)
        {
            string monster_name = EX_GameData.Information_Monster[Monster_Num].Monster_Name;
            _Monster = Resources.Load<GameObject>("Monster/" + Monster_Num+ "_"+ monster_name);
            if (GameDirector_SpawnFlag == true)
            {
                Instantiate(_Monster, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, Test_Monster_List[index]);
            }
        }
        else
        {
            string monster_name = EX_GameData.Information_Boss[Monster_Num].Monster_Name;
            _Monster = Resources.Load<GameObject>("Boss/" + Monster_Num + "_" + monster_name);
            if(GameDirector_SpawnFlag == true)
            {
                Instantiate(_Monster, new Vector3(Random_xPos, Random_yPos, 0), Quaternion.identity, Boss_List);
            }
        }
    }

    public void Get_Monster_List(List<int> GameDirector_Monster_List, List<int>GameDirector_MonsterCount_List)
    {
        Emerging_Monster_List = GameDirector_Monster_List;
        Emerging_MonsterCount_List = GameDirector_MonsterCount_List;
        Test_Monster_List = new Transform[Emerging_Monster_List.Count];
        for(int i = 0; i < Emerging_Monster_List.Count; i++)
        {
            GameObject gm = new GameObject();
            gm.name = "Monster_" + Emerging_Monster_List[i] + "_List";
            Test_Monster_List[i] = gm.transform;
        }

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
        item_MonsterCount -= count;
        yield return new WaitForSeconds(delayTime);
        item_MonsterCount += count;
    }

    public IEnumerator Item_Monster_GreedFlag(int count, int delayTime)
    {
        item_MonsterCount += count;
        yield return new WaitForSeconds(delayTime);
        item_MonsterCount -= count;
    }

    public IEnumerator Item_Use_Monster_CureseFlag(int Persent, int delayTime)
    {
        Item_cursePersent_Spawn = Persent;
        Item_curseFlag = true;
        for (int i = 0; i < Monster_List.childCount; i++)
        {
            Monster monster = Monster_List.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_CureseFlag(Persent);
        }
        yield return new WaitForSeconds(delayTime);
        Item_curseFlag = false;
        for (int i = 0; i < Monster_List.childCount; i++)
        {
            Monster monster = Monster_List.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_CureseFlag(Persent);
        }
    }

    public IEnumerator Item_Use_Monster_GiantFlag(int Persent, int delayTime)
    {
        Item_giantPersent_Spawn = Persent;
        Item_giantFlag = true;
        for(int i = 0; i < Monster_List.childCount; i++)
        {
            Monster monster = Monster_List.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_GiantFlag(Persent);
        }
        yield return new WaitForSeconds(delayTime);
        Item_giantFlag = false;
        for (int i = 0; i < Monster_List.childCount; i++)
        {
            Monster monster = Monster_List.GetChild(i).GetComponent<Monster>();
            monster.Item_Monster_GiantFlag(Persent);
        }
    }
}