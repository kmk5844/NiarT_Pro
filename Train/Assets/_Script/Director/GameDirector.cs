using Cinemachine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    [Header("Test")]
    public bool Test_Flag;
    public bool Monster_Off_Flag;
    public int Test_Distance;
    public List<int> Test_Monster_List;

    [Header("게임 타입")]
    public GameType gameType;
    GameType Before_GameType;
    [Header("데이터 모음")]
    public SA_TrainData SA_TrainData;
    public SA_TrainTurretData SA_TrainTurretData;
    public SA_TrainBoosterData SA_TrainBoosterData;
    public SA_MercenaryData SA_MercenaryData;
    public SA_StageList SA_StageList;
    StageDataObject StageData;
    public SA_PlayerData SA_PlayerData;
    public Game_DataTable EX_GameData;
    public Level_DataTable EX_LevelData;

    [Header("디렉터")]
    public GameObject MonsterDirector_Object;
    public GameObject UI_DirectorObject;
    public GameObject Item_DirectorObject;
    public PolygonCollider2D CameraConfiler;
    public FillDirector fill_director;
    Vector2[] newPoint;
    MonsterDirector monsterDirector;
    UIDirector uiDirector;
    ItemDirector itemDirector;
    List<int> Train_Num;
    List<int> Train_Turret_Num;
    List<int> Train_Booster_Num;
    int Train_Turret_Count;
    int Train_Booster_Count;
    GameObject TrainObject;

    Texture2D cursorOrigin;
    Texture2D cursorAim_UnAtk;
    Texture2D cursorAim_Atk;
    Vector2 cursorHotspot_Origin;
    Vector2 cursorHotspot_Aim;

    [Header("플레이어")]
    [SerializeField]
    GameObject playerObject;
    [HideInInspector]
    public Player player;

    [Header("스테이지 정보")]
    [SerializeField]
    public int Stage_Num;
    string Emerging_Monster_String;
    string Emerging_MonsterCount_String;
    [SerializeField]
    private List<int> Emerging_Monster;
    [SerializeField]
    private List<int> Emerging_MonsterCount;
    [SerializeField]
    private int Reward_Point;


    [SerializeField]
    string Reward_ItemNum;
    [SerializeField]
    string Reward_ItemCount;
    int Reward_Num;

    [SerializeField]
    private int Destination_Distance; // 나중에 private변경
    bool Data_BossFlag;
    string Emerging_Boss_String;
    [SerializeField]
    private List<int> Emerging_Boss;
    string Emerging_Boss_Monster_Count_String;
    [SerializeField]
    private List<int> Emerging_Boss_Monster_Count;
    string Emerging_Boss_Distance_String;
    [SerializeField]
    private List<int> Emerging_Boss_Distance;
    int BossCount;

    [Header("기차 리스트")]
    public Transform Train_List;
    Train_InGame[] Trains;
    int Train_Count;

    [Header("기차 정보")]
    public int TrainFuel; // 전체적으로 더한다.
    int Total_TrainFuel;
    public float TrainSpeed;
    float TrainSpeedUP;
    public int TrainDistance;
    [SerializeField]
    int TrainWeight;// 전체적으로 더한다.
    public bool SpawnTrainFlag;

    [Header("레벨 업 적용 전의 기차")]
    [SerializeField]
    int TrainMaxSpeed;
    [SerializeField]
    int TrainEfficient;
    [SerializeField]
    int TrainEnginePower;

    [Header("레벨 업 적용 후의 기차")]
    [SerializeField]
    public float MaxSpeed;
    [SerializeField]
    int Efficient;
    [SerializeField]
    int EnginePower;

    float lastSpeedTime; //마지막 속도 올린 시간
    float timeBet; //시간 차이
    float StartTime;
    float RandomStartTime;

    GameObject Respawn;

    int Level_EngineTier; // km/h을 증가하는 엔진 파워
    int Level_MaxSpeed; // 멕스 스피드 조절
    int Level_Efficient; // 기름 효율성

    public bool GameStartFlag;
    bool GameWinFlag;
    bool GameLoseFlag;

    int Total_Score;
    int Total_Coin;

    float distance_lastSpeedTime;
    float distance_time;

    [Header("Satation")]
    public GameObject Station_Object;
    bool isStationHideFlag;
    bool isStationShowFlag;

    [Header("BGM")]
    public AudioClip DustWindBGM;
    public AudioClip PlayBGM;
    public AudioClip BossBGM;
    public AudioClip WinBGM;
    bool Change_Win_BGM_Flag;
    int BGM_ID;

    [Header("SFX")]
    public AudioClip TrainStartSFX; // ID : 100;
    public AudioClip TrainLoopSFX;
    public AudioClip TrainStopSFX;
    public AudioClip WarningSFX; // ID : 40;
    public AudioClip WinSFX;
    public AudioClip LoseSFX;
    int Change_Train_ID;
    int TrainSFX_ID; // ID : 100;

    [HideInInspector]
    public Image BossGuage;

    //아이템부분
    bool ItemFlag_14; // 골드 2배
    void Awake()
    {
        gameType = GameType.Starting;
        Stage_Num = SA_PlayerData.Select_Stage;
        StageData = SA_StageList.Stage[Stage_Num];
        BGM_ID = 30;
        TrainSFX_ID = 100;
        Change_Win_BGM_Flag = false;
        GameStartFlag = false;
        GameWinFlag = false;
        GameLoseFlag = false;
        SpawnTrainFlag = false;
        isStationHideFlag = false;
        isStationShowFlag = false;
        monsterDirector = MonsterDirector_Object.GetComponent<MonsterDirector>();
        uiDirector = UI_DirectorObject.GetComponent<UIDirector>();
        itemDirector = Item_DirectorObject.GetComponent<ItemDirector>();
        fill_director = GetComponent<FillDirector>();

        cursorAim_UnAtk = Resources.Load<Texture2D>("Cursor/Aim6464_UnAttack");
        cursorAim_Atk = Resources.Load<Texture2D>("Cursor/Aim6464_Attack");
        cursorOrigin = Resources.Load<Texture2D>("Cursor/Origin6464");
        cursorHotspot_Origin = Vector2.zero;
        cursorHotspot_Aim = new Vector2(cursorAim_UnAtk.width / 2, cursorAim_UnAtk.height / 2);
        BossGuage = uiDirector.BossHP_Guage;

        Stage_Init();
        Train_Init();

        newPoint = new Vector2[4];
        {
            newPoint[0] = new Vector2(3.5f + 20f, -4);
            newPoint[1] = new Vector2(3.5f + 20f, 14.5f);
            newPoint[2] = new Vector2(-5.47f + (-10.94f * (Train_Count - 1)) - 20f, 14.5f);
            newPoint[3] = new Vector2(-5.47f + (-10.94f * (Train_Count - 1)) - 20f, -4);
        }
        CameraConfiler.points = newPoint;

        RandomStartTime = Random.Range(6f, 8f);
        BossCount = 0;
        lastSpeedTime = 0;
        distance_lastSpeedTime = 0;
        timeBet = 0.1f - (EnginePower * 0.001f); //엔진 파워에 따라 결정
        //Debug.Log(timeBet);
        TrainSpeedUP = 1;
        distance_time = 0.1f;
        ChangeCursor(true);
        ItemFlag_14 = false;
        if (Test_Flag)
        {
            monsterDirector.Test_Flag = true;
            Destination_Distance = Test_Distance;

            if(Test_Monster_List.Count == 0)
            {
                Test_Monster_List.Add(0);
            }

            if (Monster_Off_Flag)
            {
                monsterDirector.Monster_List.gameObject.SetActive(false);
                monsterDirector.SupplyMonster_List.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();

        player.maxRespawnPosition = new Vector3(-0.43f, 0f, 0);
        player.minRespawnPosition = new Vector3(-10.94f * (Train_Num.Count - 1), 0f, 0);

        uiDirector.Gameing_Text(Total_Score, Total_Coin);
        StartTime = Time.time;
        gameType = GameType.Starting;
        
        MMSoundManagerSoundPlayEvent.Trigger(DustWindBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID: BGM_ID);
        StartCoroutine(TrainStart_SFX());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameType == GameType.Starting || gameType == GameType.Playing || gameType == GameType.Boss)
            {
                Before_GameType = gameType;
                gameType = GameType.Pause;
                MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Pause, BGM_ID);
                MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Pause, TrainSFX_ID);
                Time.timeScale = 0f;
            }
            else if (gameType == GameType.Pause)
            {
                gameType = Before_GameType;
                MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Resume, BGM_ID);
                MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Resume, TrainSFX_ID);
                Time.timeScale = 1f;
            }
        }

        if(gameType == GameType.Starting)
        {
            if(Time.time >= StartTime + 1f)
            {
                gameType = GameType.Playing;
                StartTime = Time.time;
            }

        }else if (gameType == GameType.Playing)
        {
            ChangeCursor(true);
            if(Time.time >= StartTime + 0.1f && !isStationHideFlag)
            {
                isStationHideFlag = true;
                StartCoroutine(Hide_And_Show_Station(true));
            }

            if (Time.time >= RandomStartTime + StartTime && !GameStartFlag)
            {
                GameStartFlag = true;
                monsterDirector.GameDirector_SpawnFlag = true;
                SoundSequce(PlayBGM);
            }else if(Time.time >= lastSpeedTime + timeBet && GameWinFlag == false)
            {
                if (MaxSpeed >= TrainSpeed)
                {
                    if (TrainFuel > 0)
                    {
                        TrainSpeed += TrainSpeedUP;
                        TrainFuel -= Efficient;
                    }
                }
                else
                {
                    if (TrainFuel > 0)
                    {
                        TrainFuel -= Efficient;
                    }
                }
                lastSpeedTime = Time.time;
            }

            if (TrainFuel <= 0)
            {
                TrainFuel = 0;
            }

            if (Destination_Distance < TrainDistance && !GameWinFlag)
            {
                GameWinFlag = true;
                gameType = GameType.Ending;
            }

            if ((TrainSpeed <= 0 || player.Player_HP <= 0) && GameStartFlag && !GameLoseFlag)
            {
                int LoseNum = 0;
                if(TrainSpeed <= 0)
                {
                    LoseNum = 0;
                }
                else if(player.Player_HP <= 0)
                {
                    LoseNum = 1;
                }
                TrainSpeed = 0;
                GameLoseFlag = true;
                Game_Lose(LoseNum);
            }

            if (!GameWinFlag || !GameLoseFlag)
            {
                if (Time.time >= distance_lastSpeedTime + distance_time)
                {
                    TrainDistance += (int)TrainSpeed;
                    distance_lastSpeedTime = Time.time;
                }
            }

            if (Data_BossFlag)
            {
                if (BossCount < Emerging_Boss_Distance.Count)
                {
                    if (((float)TrainDistance / (float)Destination_Distance * 100f) > Emerging_Boss_Distance[BossCount])
                    {
                        gameType = GameType.Boss;
                        StartCoroutine(Boss_Waring_Mark());
                        monsterDirector.BossStart(Emerging_Boss_Monster_Count[BossCount]);
                    }
                }
            }
        }
        else if(gameType == GameType.Boss)
        {
            ChangeCursor(true);
            if (Time.time >= lastSpeedTime + timeBet && GameWinFlag == false)
            {
                if (MaxSpeed >= TrainSpeed)
                {
                    if (TrainFuel > 0)
                    {
                        TrainSpeed += TrainSpeedUP;
                        TrainFuel -= Efficient;
                    }
                }
                else
                {
                    if (TrainFuel > 0)
                    {
                        TrainFuel -= Efficient;
                    }
                }
                lastSpeedTime = Time.time;
            }

            if (TrainFuel <= 0)
            {
                TrainFuel = 0;
            }

            if ((TrainSpeed <= 0 || player.Player_HP <= 0) && GameStartFlag && !GameLoseFlag)
            {
                int LoseNum = 0;
                if (TrainSpeed <= 0)
                {
                    LoseNum = 0;
                }
                else if (player.Player_HP <= 0)
                {
                    LoseNum = 1;
                }
                TrainSpeed = 0;
                GameLoseFlag = true;
                Game_Lose(LoseNum);
            }
        }
        else if (gameType == GameType.Ending)
        {
            ChangeCursor(false);
            monsterDirector.GameDirector_SpawnFlag = false;
            if (!Change_Win_BGM_Flag)
            {
                SoundSequce(WinBGM);
                MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, TrainSFX_ID);
                MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, TrainSFX_ID);
                TrainSFX_ID += 1;
                MMSoundManagerSoundPlayEvent.Trigger(TrainStopSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position, ID:TrainSFX_ID);
                Change_Win_BGM_Flag = true;
                StartCoroutine(uiDirector.GameClear());
            }

            if (Time.time >= lastSpeedTime + 0.03f)
            {
                if (TrainSpeed > 0)
                {
                    TrainSpeed -= TrainSpeedUP;
                }
                else
                {
                    TrainSpeed = 0;
                }
                lastSpeedTime = Time.time;
            }

            if(TrainSpeed <= 75f && TrainSpeed > 73f && !isStationShowFlag)
            {
                isStationShowFlag = true;
                StartCoroutine(Hide_And_Show_Station(false));
            }

            if(TrainSpeed == 0)
            {
                gameType = GameType.GameEnd;
                Game_Win();
            }
        }
        else
        {
            ChangeCursor(false);
        }
    }
    void Stage_Init()
    {
        Emerging_Monster_String = StageData.Emerging_Monster;
        Emerging_MonsterCount_String = StageData.Monster_Count;
        Reward_Point = StageData.Reward_Point;

        Reward_ItemNum = StageData.Reward_Item;
        Reward_ItemCount = StageData.Reward_Itemcount;
        Reward_Num = -1;

        Destination_Distance = StageData.Destination_Distance;
        Emerging_Monster = new List<int>();
        Emerging_MonsterCount = new List<int>();
        string[] Monster_String = Emerging_Monster_String.Split(',');
        string[] MonsterCount_String = Emerging_MonsterCount_String.Split(",");
        for(int i = 0; i < Monster_String.Length; i++)
        {
            int num1;
            int num2;

            num1 = int.Parse(Monster_String[i]);
            num2 = int.Parse(MonsterCount_String[i]);

            Emerging_Monster.Add(num1);
            Emerging_MonsterCount.Add(num2);
        }
        
        /*foreach(string M in Monster_String)
        {
            int num;
            if(int.TryParse(M, out num))
            {
                Emerging_Monster.Add(num);
            }
        }*/

        Data_BossFlag = StageData.Boss_Flag;
        if (Data_BossFlag)
        {
            Emerging_Boss_String = StageData.Emerging_boss;
            Emerging_Boss_Monster_Count_String = StageData.Boss_Monster_Count;
            Emerging_Boss_Distance_String = StageData.Boss_Distance;

            string[] Boss_String = Emerging_Boss_String.Split(',');
            foreach(string M in Boss_String)
            {
                int num;
                if(int.TryParse(M, out num))
                {
                    Emerging_Boss.Add(num);
                }
            }
            string[] Boss_Count_String = Emerging_Boss_Monster_Count_String.Split(',');
            foreach(string M in Boss_Count_String)
            {
                int num;
                if(int.TryParse(M, out num)) { 
                    Emerging_Boss_Monster_Count.Add(num);
                }
            }
            string[] Boss_Distance_String = Emerging_Boss_Distance_String.Split(',');
            foreach(string M in Boss_Distance_String)
            {
                int num;
                if(int.TryParse(M, out num))
                {
                    Emerging_Boss_Distance.Add(num);
                }
            }
            MonsterDirector_Object.GetComponent<MonsterDirector>().Get_Boss_List(Emerging_Boss);
        }


        if (Test_Flag)
        {
            List<int> NullObject = new List<int>();
            NullObject.Add(1);
            MonsterDirector_Object.GetComponent<MonsterDirector>().Get_Monster_List(Test_Monster_List, NullObject);
        }
        else
        {
            MonsterDirector_Object.GetComponent<MonsterDirector>().Get_Monster_List(Emerging_Monster, Emerging_MonsterCount);
            if (Data_BossFlag)
            {
                MonsterDirector_Object.GetComponent<MonsterDirector>().Get_Boss_List(Emerging_Boss);
            }
        }
    }
    void Train_Init()
    {
        Train_Turret_Count = 0;
        Train_Booster_Count = 0;
        Train_Num = SA_TrainData.Train_Num;
        Train_Turret_Num = SA_TrainTurretData.Train_Turret_Num;
        Train_Booster_Num = SA_TrainBoosterData.Train_Booster_Num;
        for (int i = 0; i < Train_Num.Count; i++)
        {
            if (Train_Num[i] == 51)
            {
                TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_InGame/51_" + Train_Turret_Num[Train_Turret_Count]), Train_List);
                Train_Turret_Count++;
            }
            else if (Train_Num[i] == 52)
            {
                TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_InGame/52_" + Train_Booster_Num[Train_Booster_Count]), Train_List);
                Train_Booster_Count++;
            }
            else
            {
                TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_InGame/" + Train_Num[i]), Train_List);
            }

            if (i == 0)
            {
                //엔진칸
                TrainObject.transform.position = new Vector3(-0.43f, 0f, 0);
            }
            else
            {
                //나머지칸
                TrainObject.transform.position = new Vector3(-10.94f * i, 0f, 0);
            }
            
            Train_InGame train = TrainObject.GetComponent<Train_InGame>();
            TrainFuel += train.Train_Fuel;
            TrainWeight += train.Train_Weight;
            TrainMaxSpeed += train.Train_MaxSpeed;
            TrainEfficient += train.Train_Efficient;
            TrainEnginePower += train.Train_Engine_Power;
        }

        Train_Count = Train_List.childCount;
        Trains = new Train_InGame[Train_Count];
        Respawn = GameObject.FindGameObjectWithTag("Respawn");
        Respawn.transform.localScale = new Vector3(25 * Train_Count, 1, 0);
        Respawn.transform.position = new Vector3(Train_List.GetChild(Train_Count / 2).transform.position.x, -3, 0);

        Level_EngineTier = SA_TrainData.Level_Train_EngineTier;
        Level_MaxSpeed = SA_TrainData.Level_Train_MaxSpeed;
        Level_Efficient = SA_TrainData.Level_Train_Efficient;
        Total_TrainFuel = TrainFuel;
        Level();
        SpawnTrainFlag = true;
    }
    public void Level()
    {
        MaxSpeed = TrainMaxSpeed + ((TrainMaxSpeed * Level_MaxSpeed) / 100); // 많을 수록 유리
        MaxSpeed = MaxSpeed - (TrainWeight / 100000); //무게로 인해 speed 감소

        Efficient = TrainEfficient - (Level_Efficient / 2); // 적을 수록 유리
        EnginePower = (TrainEnginePower + Level_EngineTier);
    }

    public void Game_MonsterHit(float slow)
    {
        TrainSpeed -= slow;
    }

    public void Destroy_train_weight(int weight)
    {
        TrainWeight -= weight;
        Level();
    }

    public void Engine_Driver_Passive(Engine_Driver_Type type, int EngineDriver_value, bool survival) //방어력은 별개로 등록이 되어있다.
    {
        switch (type)
        {
            case Engine_Driver_Type.speed:
                if (survival)
                {
                    MaxSpeed += EngineDriver_value;
                }
                else
                {
                    MaxSpeed -= EngineDriver_value;
                }
                break;
            case Engine_Driver_Type.fuel:
                if (survival)
                {
                    Efficient -= EngineDriver_value;
                }
                else
                {
                    Efficient += EngineDriver_value;
                }
                break;
        }
    }

    public void Game_Monster_Kill(int GetScore, int GetCoin)
    {
        Total_Score += GetScore;
        if (!ItemFlag_14)
        {
            Total_Coin += GetCoin;
        }
        else
        {
            Total_Coin += 2 * GetCoin;
        }
        uiDirector.Gameing_Text(Total_Score, Total_Coin);
    }

    public void Gmae_Boss_Kill(int GetScore, int GetCoin)
    {
        Total_Score += GetScore;
        if(!ItemFlag_14)
        {
            Total_Coin += GetCoin;
        }
        else
        {
            Total_Coin += 2 * GetCoin;
        }
        BossCount++;
        monsterDirector.BossDie();
        uiDirector.BossHP_Object.SetActive(false);
        uiDirector.Gameing_Text(Total_Score, Total_Coin);
        gameType = GameType.Playing;
        SoundSequce(PlayBGM);
    }
    private string Check_Score()
    {
        if (Total_Score >= StageData.Grade_Score[4])
        {
            Reward_Num = 4;
            return "S";
        }
        else if (StageData.Grade_Score[4] > Total_Score && Total_Score >= StageData.Grade_Score[3])
        {
            Reward_Num = 3;
            return "A";
        }
        else if (StageData.Grade_Score[3] > Total_Score && Total_Score >= StageData.Grade_Score[2])
        {
            Reward_Num = 2;
            return "B";
        }
        else if (StageData.Grade_Score[2] > Total_Score && Total_Score >= StageData.Grade_Score[1])
        {
            Reward_Num = 1;
            return "C";
        }
        else if (StageData.Grade_Score[1] > Total_Score && Total_Score >= StageData.Grade_Score[0]){
            Reward_Num = 0;
            return "D";
        }
        else if (StageData.Grade_Score[0] > Total_Score)
        {
            Reward_Num = -1;
            return "F";
        }
        Reward_Num = -1;
        return "F";
    }

    private int Check_StageDataGrade()
    {
        switch (StageData.Player_Grade) {

            case StageDataObject.Grade.S:
                return 4;
            case StageDataObject.Grade.A:
                return 3;
            case StageDataObject.Grade.B:
                return 2;
            case StageDataObject.Grade.C:
                return 1;
            case StageDataObject.Grade.D:
                return 0;
            case StageDataObject.Grade.F:
                return -1;
        }
        return -1;
    }

    private void Change_Game_End(bool WinFlag, int LoseNum = -1) // 이겼을 때
    {
        gameType = GameType.GameEnd;
        Time.timeScale = 0f;
        if (WinFlag)
        {
            string[] ItemList = Reward_ItemNum.Split(',');
            string[] ItemList_Count = Reward_ItemCount.Split(",");
            List<int> Item_List = new List<int>();
            int ItemNum;
            int ItemCount;
            if (!StageData.Player_FirstPlay)
            {
                for (int i = 0; i < Reward_Num + 1; i++)
                {
                    ItemNum = int.Parse(ItemList[i]);
                    ItemCount = int.Parse(ItemList_Count[i]);
                    if (ItemNum != -1)
                    {
                        itemDirector.itemList.Item[ItemNum].Item_Count_UP(ItemCount);
                        uiDirector.GetItemList_Num.Add(ItemNum);
                    }
                }
            }
            else
            {
                int BeforeGradeNum = Check_StageDataGrade();
                if(BeforeGradeNum < Reward_Num)
                {
                    for(int i = BeforeGradeNum + 1; i < Reward_Num + 1; i++)
                    {
                        ItemNum = int.Parse(ItemList[i]);
                        ItemCount = int.Parse(ItemList_Count[i]);
                        Debug.Log(ItemList[i]);
                        if (ItemNum != -1)
                        {
                            itemDirector.itemList.Item[ItemNum].Item_Count_UP(ItemCount);
                            uiDirector.GetItemList_Num.Add(ItemNum);
                        }
                    }
                }
            }
        }

        uiDirector.Open_Result_UI(WinFlag, Stage_Num, Total_Score, Total_Coin, Check_Score(), Reward_Point, LoseNum);
    }

    private void Game_Win()
    {
        string grade = Check_Score();
        Change_Game_End(true);
        StageData.GameEnd(true, Total_Score, grade);

        MMSoundManagerSoundPlayEvent.Trigger(WinSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        
        if(SA_PlayerData.New_Stage == SA_PlayerData.Select_Stage)
        {
            try
            {
                SA_StageList.Stage[Stage_Num + 1].New_Stage_Chage();
            }
            catch
            {
                Debug.Log("종료");
            }
        }
        SA_PlayerData.SA_GameWinReward(Total_Coin, Reward_Point);
    }

    private void Game_Lose(int losenum)
    {
        Change_Game_End(false, losenum);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, BGM_ID);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, TrainSFX_ID);
        MMSoundManagerSoundPlayEvent.Trigger(LoseSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        StageData.GameEnd(false, Total_Score);
        SA_PlayerData.SA_GameLoseReward(Total_Coin);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, BGM_ID);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, TrainSFX_ID);
    }

    public void GameType_Option(bool flag)
    {
        if (flag)
        {
            gameType = GameType.Option;
        }
        else
        {
            gameType = GameType.Pause;
        }
    }

    public float Check_Distance()
    {
        return (float)TrainDistance / (float)Destination_Distance;
    }

    public float Check_Fuel()
    {
        return (float)TrainFuel / (float)Total_TrainFuel;
    }

    public void ChangeCursor(bool flag , bool atkFlag = false)
    {
        if (flag) // 게임 진행 중일 때
        {
            if (!atkFlag)
            {
                Cursor.SetCursor(cursorAim_UnAtk, cursorHotspot_Aim, CursorMode.ForceSoftware);
            }
            else
            {
                Cursor.SetCursor(cursorAim_Atk, cursorHotspot_Aim, CursorMode.ForceSoftware);
            }
        }
        else // Pause했을 때
        {
            Cursor.SetCursor(cursorOrigin, cursorHotspot_Origin, CursorMode.Auto);
        }
    }
    public void SoundSequce(AudioClip audio)
    {
        StartCoroutine(Change_Sound(audio));
    }

    private IEnumerator Change_Sound(AudioClip audio)
    {
        BGM_ID++;

        // plays the sound, notice we pass it a unique ID
        MMSoundManagerPlayOptions options;
        options = MMSoundManagerPlayOptions.Default;
        options.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Music;
        options.ID = BGM_ID;
        options.Loop = true;
        MMSoundManagerSoundPlayEvent.Trigger(audio, options);

        // starts to fade it out (using the ID we passed earlier)
        yield return null;
        MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.StopFade, BGM_ID - 1, 3f, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));

        // frees the sound at the end (still using that same ID)
        yield return MMCoroutine.WaitFor(3f);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, BGM_ID - 1);
    }

    //Item부분
    public void Item_Fuel_Charge(float persent)
    {
        TrainFuel += (int)(Total_TrainFuel * (persent / 100f));
    }

    public void Item_Use_Train_Heal_HP(float persent)
    {
        for(int i = 0; i < Train_List.childCount; i++)
        {
            Train_List.GetChild(i).GetComponent<Train_InGame>().Item_Train_Heal_HP(persent);
        }
    }

    public void Item_Use_Train_Turret_All_SpeedUP(float persent, float delayTime)
    {
        for(int i = 0; i < Train_List.childCount; i++)
        {
            Train_InGame train = Train_List.GetChild(i).GetComponent<Train_InGame>();
            StartCoroutine(train.Item_Train_Turret_SpeedUP(persent, delayTime));
        }
    }

    public IEnumerator Item_Train_SpeedUp(float delayTime, float SpeedUp = 0.5f)
    {
        TrainSpeedUP += SpeedUp;
        yield return new WaitForSeconds(delayTime);
        TrainSpeedUP -= SpeedUp;
    }

    public IEnumerator Item_Coin_Double(int delayTime)
    {
        ItemFlag_14 = true;
        yield return new WaitForSeconds(delayTime);
        ItemFlag_14 = false;
    }

    IEnumerator Boss_Waring_Mark()
    {
        int Warning_ID = BGM_ID + 10;
        MMSoundManagerSoundPlayEvent.Trigger(WarningSFX, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID: Warning_ID, fade:true, fadeInitialVolume: 0.5f, fadeDuration :0.3f);
        uiDirector.WarningObject.SetActive(true);
        MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.StopFade, Warning_ID, 3f, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));
        yield return new WaitForSeconds(3f);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, Warning_ID);
        SoundSequce(BossBGM);
        float fadeDuration = 1f;
        float fadeElapsed = 0f;
        Image warningImage = uiDirector.WarningObject.GetComponent<Image>();
        Color originalColor = warningImage.color;

        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeElapsed / fadeDuration);
            warningImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 완전히 투명하게 만든 후 WarningObject 비활성화
        warningImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        uiDirector.WarningObject.SetActive(false);

        warningImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        yield return new WaitForSeconds(1.5f);


        uiDirector.BossHP_Guage.fillAmount = 1f;
        uiDirector.BossHP_Object.SetActive(true);
    }

    IEnumerator Hide_And_Show_Station(bool flag)
    {
        float StartX;
        float TargetX;
        float Speed;

        if (flag)
        {
            StartX = 6f;
            TargetX = -36f;
        }
        else
        {
            StartX = 36f;
            TargetX = 6f;
        }

        Station_Object.transform.localPosition = new Vector2(StartX, Station_Object.transform.localPosition.y);
        Station_Object.SetActive(true);

        if (flag)
        {
        float fac = 0.001f;
            while (Station_Object.transform.localPosition.x > -36f)
            {
                Speed = Time.deltaTime * fac + (TrainSpeed / 5000f);
                Station_Object.transform.localPosition = new Vector2(Station_Object.transform.localPosition.x - Speed, Station_Object.transform.localPosition.y);
                yield return null;
            }
        }
        else
        {
        float fac = 0.6f;
            while (Station_Object.transform.localPosition.x > 6f)
            {
                Speed = Time.deltaTime * fac + (TrainSpeed / 4000f);
                Station_Object.transform.localPosition = new Vector2(Station_Object.transform.localPosition.x - Speed, Station_Object.transform.localPosition.y);
                yield return null;
            }
        }

        if (flag)
        {
            Station_Object.SetActive(false);
        }
    }
    
    IEnumerator TrainStart_SFX()
    {
        MMSoundManagerSoundPlayEvent.Trigger(TrainStartSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position, loop: false, ID: TrainSFX_ID);
        yield return MMCoroutine.WaitFor(TrainStartSFX.length - 1.4f);
        TrainSFX_ID += 1;
        MMSoundManagerSoundPlayEvent.Trigger(TrainLoopSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position, loop: true, ID: TrainSFX_ID);
        yield return MMCoroutine.WaitFor(1.8f);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, TrainSFX_ID-1);
    }

    public IEnumerator Train_MasSpeedChange(int Add_Speed,float During)
    {
        MaxSpeed += Add_Speed;
        yield return new WaitForSeconds(During);
        MaxSpeed -= Add_Speed;
    }
}


public enum GameType{
    Starting,
    Playing,
    Boss,
    Pause,
    Option,
    Ending,
    GameEnd,
}//점차 늘어갈 예정