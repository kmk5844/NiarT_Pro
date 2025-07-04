using Cinemachine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static PixelCrushers.AnimatorSaver;


//Script Execution Order로 조절 중
public class GameDirector : MonoBehaviour
{
    [Header("게임 타입")]
    public GameType gameType;
    GameType Before_GameType;
    [Header("데이터 모음")]
    public SA_TrainData SA_TrainData;
    public SA_TrainTurretData SA_TrainTurretData;
    public SA_MercenaryData SA_MercenaryData;
    public SA_StageList SA_StageList;
    //StageDataObject StageData;

    //새로운 스테이지 정보
    [SerializeField]
    MissionDataObject SubStageData;
    //다음에 해금할 스테이지 정보
    [SerializeField]
    List<int> NextSubStageNum;
    //잠글 스테이지 정보
    [SerializeField]
    List<int> PrevSubStageNum;

    public SA_PlayerData SA_PlayerData;
    public Game_DataTable EX_GameData;
    public Level_DataTable EX_LevelData;
    public SA_MissionData SA_MissionData;

    [Header("디렉터")]
    public GameObject MonsterDirector_Object;
    public GameObject UI_DirectorObject;
    public GameObject Item_DirectorObject;
    public PolygonCollider2D CameraConfiler;
    public FillDirector fill_director;
    public GameObject MissionDirector_Object;
    public GameObject SkillDirector_Object;
    Vector2[] newPoint;
    MonsterDirector monsterDirector;
    UIDirector uiDirector;
    ItemDirector itemDirector;
    MissionDirector missionDirector;
    SkillDirector skillDirector;
    List<int> Train_Num;
    List<int> Train_Turret_Num;
    int Train_Turret_Count;
    GameObject TrainObject;

    Texture2D cursorOrigin;
    Texture2D cursorAim_UnAtk;
    Texture2D cursorAim_Atk;
    Vector2 cursorHotspot_Origin;
    Vector2 cursorHotspot_Aim;
    [HideInInspector]
    public bool lastFlag;

    [Header("플레이어")]
    [SerializeField]
    GameObject playerObject;
    [HideInInspector]
    public Player player;

    [Header("스테이지 정보")]
    public int Mission_Num;
    public int Stage_Num;
    public int Before_Sub_Num;
    public int Select_Sub_Num;
    
    string Emerging_Monster_String;
    string Emerging_MonsterCount_String;
    [SerializeField]
    private List<int> Emerging_Monster_Sky;
    [SerializeField]
    private List<int> Emerging_Monster_Ground;
    [SerializeField]
    private List<int> Emerging_MonsterCount;
    [Header("스테이지에 따른 백그라운드")]
    public GameObject[] BackGroundList;

    [Header("미션 정보")]
    public bool Mission_Train_Flag;

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
    public bool Station_OnOffFlag;
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
    public AudioClip TrainHitSFX;
    int Change_Train_ID;
    int TrainSFX_ID; // ID : 100;

    [HideInInspector]
    public Image BossGuage;
    public GameObject MiniTurretObject;

    //아이템부분
    bool ItemFlag_14; // 골드 2배

    [Header("음식 이벤트")]
    public bool FoodEffect_Flag_Positive;
    public bool FoodEffect_Flag_Impositive;

    [Header("웨이브")]
    public GameObject SupplyRefresh_ItemObject;
    public float RefreshPersent;
    int RefreshDistance;
    bool waveUIFlag;
    bool firstRefresh;
    bool endRefresh;
    bool SpawnRefreshSupply;
    bool getSupply;
    public bool waveinfoFlag;
    public bool refreshinfoFlag;
    public bool SkillLockFlag;

    void Awake()
    {
        gameType = GameType.Starting;
        try
        {
            if (GameManager.Instance.Demo)
            {
                Mission_Num = 0;
                Stage_Num = -1;
            }
            else
            {
                Mission_Num = SA_PlayerData.Mission_Num;
                Stage_Num = SA_PlayerData.Select_Stage;
            }
        }
        catch
        {
            Mission_Num = SA_PlayerData.Mission_Num;
            Stage_Num = SA_PlayerData.Select_Stage;
        }
        Before_Sub_Num = SA_PlayerData.Before_Sub_Stage;
        Select_Sub_Num = SA_PlayerData.Select_Sub_Stage;

/*        Mission_Num = 0;
        Stage_Num = 0;
        Select_Sub_Num = 0;*/

        RefreshPersent = 50;

        //TrainDistance = 70000;

        SubStageData = SA_MissionData.missionStage(Mission_Num, Stage_Num, Select_Sub_Num);
        NextSubStageNum = new List<int>();
        string[] nextSubStageList = SubStageData.Open_SubStageNum.Split(',');
        foreach(string sub in nextSubStageList)
        {
            NextSubStageNum.Add(int.Parse(sub));
        }

        if(Before_Sub_Num != -1)
        {
            MissionDataObject PrevStageData = SA_MissionData.missionStage(Mission_Num, Stage_Num, Before_Sub_Num);
            PrevSubStageNum = new List<int>();
            string[] prevSubStageList = PrevStageData.Open_SubStageNum.Split(',');

            foreach (string sub in prevSubStageList)
            {
                PrevSubStageNum.Add(int.Parse(sub));
            }
        }

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
        missionDirector = MissionDirector_Object.GetComponent<MissionDirector>();
        skillDirector = SkillDirector_Object.GetComponent<SkillDirector>();
        fill_director = GetComponent<FillDirector>();

        cursorAim_UnAtk = Resources.Load<Texture2D>("Cursor/Aim6464_UnAttack");
        cursorAim_Atk = Resources.Load<Texture2D>("Cursor/Aim6464_Attack");
        cursorOrigin = Resources.Load<Texture2D>("Cursor/Origin6464");
        cursorHotspot_Origin = Vector2.zero;
        cursorHotspot_Aim = new Vector2(cursorAim_UnAtk.width / 2, cursorAim_UnAtk.height / 2);
        BossGuage = uiDirector.BossHP_Guage;

        StageBackGround_Setting();
        Stage_Init();
        Train_Init();

        RefreshDistance = (int)(Destination_Distance * (RefreshPersent / 100));

        newPoint = new Vector2[4];
        {
            newPoint[0] = new Vector2(3.5f + 20f, -4);
            newPoint[1] = new Vector2(3.5f + 20f, 14.5f);
            newPoint[2] = new Vector2(-5.47f + (-10.94f * (Train_Count - 1)) - 20f, 14.5f);
            newPoint[3] = new Vector2(-5.47f + (-10.94f * (Train_Count - 1)) - 20f, -4);
        }
        CameraConfiler.points = newPoint;

        RandomStartTime = Random.Range(3f, 5f);
        BossCount = 0;
        lastSpeedTime = 0;
        distance_lastSpeedTime = 0;
        timeBet = 0.1f - (EnginePower * 0.002f); //엔진 파워에 따라 결정
        TrainSpeedUP = 1;
        distance_time = 0.1f;
        ChangeCursor(true);
        ItemFlag_14 = false;
        waveinfoFlag = false;
        refreshinfoFlag = false;

    }

    private void Start()
    {
        if (QualitySettings.vSyncCount != 0)
        {
            //Debug.Log("작동");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();

        player.maxRespawnPosition = new Vector3(-0.43f, 0f, 0);
        player.minRespawnPosition = new Vector3(-10.94f * (Train_Num.Count - 1), 0f, 0);

        float Random_Turret_X = Random.Range(player.minRespawnPosition.x, player.maxRespawnPosition.x);
        Instantiate(MiniTurretObject, new Vector2(Random_Turret_X, -0.58f), Quaternion.identity);

        if (FoodEffect_Flag_Positive)
        {
            MaxSpeed += ((MaxSpeed * 3) / 100); // 많을 수록 유리
            Efficient -= ((Efficient * 10) / 100); // 적을 수록 유리
            timeBet = 0.1f - ((EnginePower+1) * 0.001f);

        }

        if (FoodEffect_Flag_Impositive)
        {
            MaxSpeed -= ((MaxSpeed * 3) / 100); // 많을 수록 유리
            Efficient += ((Efficient * 10) / 100); // 적을 수록 유리
            timeBet = 0.1f - ((EnginePower - 1) * 0.001f);
        }

        uiDirector.Gameing_Text(Total_Score, Total_Coin);
        StartTime = Time.time;
        gameType = GameType.Starting;

        MMSoundManagerSoundPlayEvent.Trigger(DustWindBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID: BGM_ID);
        StartCoroutine(TrainStart_SFX());
    }

    void Update()
    {
        //TEST
/*        if (Input.GetKeyDown("]"))
        {
            if (Data_BossFlag)
            {
                TrainSpeed = 1000;
            }
            else
            {
                TrainDistance = 99999999;
            }
        }*/

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameType == GameType.Starting || gameType == GameType.Playing || gameType == GameType.Boss || gameType == GameType.Refreshing)
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

            if (!waveinfoFlag)
            {
                waveinfoFlag = true;
                StartCoroutine(uiDirector.WaveInformation(true));
            }
        }
        else if (gameType == GameType.Playing)
        {
           /* if (Time.time >= StartTime + 0.1f && !isStationHideFlag)
            {
                isStationHideFlag = true;
                StartCoroutine(Hide_And_Show_Station(true));
            }*/

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

            if(RefreshDistance < TrainDistance && !firstRefresh)
            {
                firstRefresh = true;
                gameType = GameType.Refreshing;
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
                        Debug.Log(Emerging_Boss_Monster_Count[BossCount]);
                        monsterDirector.BossStart(Emerging_Boss_Monster_Count[BossCount]);
                    }
                }
            }
        }
        else if(gameType == GameType.Boss)
        {
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
        }else if(gameType == GameType.Refreshing)
        {
            if (!getSupply)
            {
                waveinfoFlag = false;

                if (!refreshinfoFlag)
                {
                    refreshinfoFlag = true;
                }

                if (!monsterDirector.GameDirector_RefreshFlag)
                {
                    monsterDirector.GameDirector_RefreshFlag = true;
                }

                if (monsterDirector.GameDirecotr_AllDieFlag)
                {
                    uiDirector.SKillLock(true);
                    SkillLockFlag = true;
                    if (!waveUIFlag)
                    {
                        StartCoroutine(uiDirector.WaveInformation(false));
                        waveUIFlag = true;
                    }

                    if (Time.time >= lastSpeedTime + 0.05f)
                    {
                        if (TrainSpeed > 40)
                        {
                            TrainSpeed -= TrainSpeedUP * 3;
                        }
                        else
                        {
                            TrainSpeed = 40;
                            if (!SpawnRefreshSupply)
                            {
                                SpawnRefreshSupply = true;
                                float RandomX = Random.Range(player.transform.position.x - 5f, player.transform.position.x + 5f);
                                if(RandomX > MonsterDirector.MaxPos_Ground.x)
                                {
                                    RandomX = MonsterDirector.MaxPos_Ground.x;
                                }
                                else if (RandomX < MonsterDirector.MinPos_Ground.x)
                                {
                                    RandomX = MonsterDirector.MinPos_Ground.x;
                                }

                                Vector2 pos = new Vector2(RandomX, player.transform.position.y + 15);
                                SupplyRefresh_ItemObject.GetComponent<SupplyRefresh_Item>().director = this;
                                Instantiate(SupplyRefresh_ItemObject, pos, Quaternion.identity);
                            }
                        }
                        lastSpeedTime = Time.time;
                    }
                }
                else
                {
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
                }
            }
            else
            {
                if (!waveinfoFlag)
                {
                    waveinfoFlag = true;
                    StartCoroutine(uiDirector.WaveInformation(true));
                }

                if (Time.time >= lastSpeedTime + timeBet)
                {
                    TrainSpeed += TrainSpeedUP;
                    lastSpeedTime = Time.time;
                }

                if(TrainSpeed > 100)
                {
                    if (monsterDirector.GameDirector_RefreshFlag)
                    {
                        monsterDirector.GameDirector_RefreshFlag = false;
                        monsterDirector.GameDirector_SpawnFlag = true;
                        monsterDirector.GameDirecotr_AllDieFlag = false;
                    }
                    uiDirector.SKillLock(false);
                    SkillLockFlag = false;
                    gameType = GameType.Playing;
                }
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
            monsterDirector.GameDirector_SpawnFlag = false;

            if (!monsterDirector.GameDirector_EndingFlag)
            {
                monsterDirector.GameDirector_EndingFlag = true;
            }
            else
            {
                if (monsterDirector.GameDirecotr_AllDieFlag)
                {
                    if (!Change_Win_BGM_Flag)
                    {
                        SoundSequce(WinBGM);
                        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, TrainSFX_ID);
                        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, TrainSFX_ID);
                        TrainSFX_ID += 1;
                        MMSoundManagerSoundPlayEvent.Trigger(TrainStopSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position, ID: TrainSFX_ID);
                        Change_Win_BGM_Flag = true;
                        StartCoroutine(uiDirector.GameClear());
                    }

                    if (Time.time >= lastSpeedTime + 0.025f)
                    {
                        if (TrainSpeed > 0)
                        {
                            TrainSpeed -= TrainSpeedUP * 2;
                        }
                        else
                        {
                            TrainSpeed = 0;
                        }
                        lastSpeedTime = Time.time;
                    }

                    /*if (TrainSpeed <= 74f && TrainSpeed > 72f && !isStationShowFlag)
                    {
                        isStationShowFlag = true;
                        StartCoroutine(Hide_And_Show_Station(false));
                    }*/

                    if (TrainSpeed == 0)
                    {
                        gameType = GameType.GameEnd;
                        Game_Win();
                    }
                }
            }
        }
        else
        {
            ChangeCursor(false);
        }
    }

    private void FixedUpdate()
    {
        if (Station_OnOffFlag)
        {
            if (gameType == GameType.Playing)
            {
                if (Time.time >= StartTime + 0.1f && !isStationHideFlag)
                {
                    isStationHideFlag = true;
                    StartCoroutine(Hide_And_Show_Station(true));
                }
            }

            if (gameType == GameType.Ending)
            {
                if (monsterDirector.GameDirector_EndingFlag)
                {
                    if (monsterDirector.GameDirecotr_AllDieFlag)
                    {
                        if (TrainSpeed <= 74f && TrainSpeed > 72f && !isStationShowFlag)
                        {
                            isStationShowFlag = true;
                            StartCoroutine(Hide_And_Show_Station(false));
                        }
                    }
                }
            }
        }
        else
        {
            if (Station_Object.activeSelf)
            {
                Station_Object.SetActive(false);
            }
        }
    }

    void Stage_Init()
    {
        Emerging_Monster_String = SubStageData.Emerging_Monster;
        Emerging_MonsterCount_String = SubStageData.Monster_Count;
        //Reward_Point = StageData.Reward_Point;
        //Reward_ItemNum = StageData.Reward_Item;
        //Reward_ItemCount = StageData.Reward_Itemcount;
        //Destination_Distance = StageData.Destination_Distance;
        Destination_Distance = SubStageData.Distance;

        Emerging_Monster_Sky = new List<int>();
        Emerging_Monster_Ground = new List<int>();
        Emerging_MonsterCount = new List<int>();
        string[] Monster_String = Emerging_Monster_String.Split(',');
        string[] MonsterCount_String = Emerging_MonsterCount_String.Split(",");
        for(int i = 0; i < Monster_String.Length; i++)
        {
            int num1;
            num1 = int.Parse(Monster_String[i]);

            if(EX_GameData.Information_Monster[num1].Monster_Type == "Sky")
            {
                Emerging_Monster_Sky.Add(num1);
            }
            else if (EX_GameData.Information_Monster[num1].Monster_Type == "Ground")
            {
                Emerging_Monster_Ground.Add(num1);
            }
        }

        for (int i = 0; i < MonsterCount_String.Length; i++)
        {
            int num2;

            num2 = int.Parse(MonsterCount_String[i]);
            Emerging_MonsterCount.Add(num2);
        }

        monsterDirector.Get_Monster_List(Emerging_Monster_Sky, Emerging_Monster_Ground, Emerging_MonsterCount);

        if (SubStageData.SubStage_Type == SubStageType.Boss)
        {
            Data_BossFlag = true;
            /*            Emerging_Boss_String = StageData.Emerging_boss;
                        Emerging_Boss_Monster_Count_String = StageData.Boss_Monster_Count;
                        Emerging_Boss_Distance_String = StageData.Boss_Distance;*/

            string[] Boss_String = SubStageData.SubStage_Status.Split(',');
            Emerging_Boss.Add(int.Parse(Boss_String[0]));
            Emerging_Boss_Distance.Add(int.Parse(Boss_String[1]));
            Emerging_Boss_Monster_Count.Add(int.Parse(Boss_String[2]));
            MonsterDirector_Object.GetComponent<MonsterDirector>().Get_Boss_List(Emerging_Boss);
        }

/*        //MonsterDirector_Object.GetComponent<MonsterDirector>().Get_Monster_List(Emerging_Monster, Emerging_MonsterCount);
        if (Data_BossFlag)
        {
            MonsterDirector_Object.GetComponent<MonsterDirector>().Get_Boss_List(Emerging_Boss);
        }*/
    }

    void StageBackGround_Setting()
    {
        switch (Stage_Num/5) {
            case 0:
            case 1:
                Station_OnOffFlag = true;
                BackGroundList[0].SetActive(true);
                break;
            case 2:
                Station_OnOffFlag = false;
                BackGroundList[1].SetActive(true);
                break;
        }
    }

    void Train_Init()
    {
        Train_Turret_Count = 0;
        Train_Num = SA_TrainData.Train_Num.ToList();
        Train_Turret_Num = SA_TrainTurretData.Train_Turret_Num;

        if (Mission_Train_Flag)
        {
            int index = Random.Range(1, Train_Num.Count+1);
            Train_Num.Insert(index, 90);
        }

        for (int i = 0; i < Train_Num.Count; i++)
        {
            if (Train_Num[i] == 91)
            {
                TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_InGame/91_" + Train_Turret_Num[Train_Turret_Count]), Train_List);
                Train_Turret_Count++;
            }
            /*else if (Train_Num[i] == 52)
            {
                TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_InGame/52_" + Train_Booster_Num[Train_Booster_Count]), Train_List);
                Train_Booster_Count++;
            }*/else if (Train_Num[i] == 90)//호위차량
            {
                TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_InGame/" + Train_Num[i]), Train_List);
                Train_InGame _train = TrainObject.GetComponent<Train_InGame>();
                _train.Max_Train_HP = 15000;
                _train.Train_HP = _train.Max_Train_HP;
                _train.Train_Weight = missionDirector.selectmission.M_Convoy.ConvoyWeight;
                _train.Train_Armor = 30;
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
            train.Train_Index = i;
            TrainFuel += train.Train_Fuel;
            TrainWeight += train.Train_Weight;
            TrainMaxSpeed += train.Train_MaxSpeed;
            TrainEfficient += train.Train_Efficient;
            TrainEnginePower += train.Train_Engine_Power;
            train.trainHitSFX = TrainHitSFX;
        }

        Train_Count = Train_List.childCount;
        Trains = new Train_InGame[Train_Count];

        Level_EngineTier = SA_TrainData.Level_Train_EngineTier;
        Level_MaxSpeed = SA_TrainData.Level_Train_MaxSpeed;
        Level_Efficient = SA_TrainData.Level_Train_Efficient;
        Level();
        SpawnTrainFlag = true;

        Total_TrainFuel = TrainFuel;

        if (Select_Sub_Num != 0)
        {
            TrainFuel = ES3.Load<int>("Train_Curret_Fuel", Total_TrainFuel);
        }
        else
        {
            ES3.Save<int>("Train_Curret_TotalFuel", Total_TrainFuel);
        }
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

    public void Mission_Monster_Kill()
    {
        missionDirector.MonsterCount();
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
    /*    private string Check_Score()
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
            else if (StageData.Grade_Score[1] > Total_Score && Total_Score >= StageData.Grade_Score[0])
            {
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
        }*/

    /*    private int Check_StageDataGrade()
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
        }*/

    private void Game_Win()
    {
        //string grade = Check_Score();
        //StageData.GameEnd(true, Total_Score);//, grade);
        //Change_Game_End(true);

        GameEnd_SavePlayerData();
        SubStage_Clear();
        //lastSubStage_Lock();
        SubStage_LockOff();
        MMSoundManagerSoundPlayEvent.Trigger(WinSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
    }

    void SubStage_Clear()
    {
        SubStageData.SubStage_Clear();
    }

/*    void lastSubStage_Lock()
    {
        foreach(int substageNum in PrevSubStageNum)
        {
            if(substageNum != -1)
            {
                if(substageNum != Select_Sub_Num)
                {
                    MissionDataObject mission = SA_MissionData.missionStage(Mission_Num, Stage_Num, substageNum);
                    mission.prevLock();
                }
            }
        }
    }*/

    void SubStage_LockOff()
    {
        bool Initflag = false;

        if(Select_Sub_Num == 0)
        {
            Before_Sub_Num = -1;
        }

        foreach (int substageNum in NextSubStageNum)
        {
            if (substageNum != -1)
            {
                MissionDataObject mission = SA_MissionData.missionStage(Mission_Num, Stage_Num, substageNum);
                Debug.Log(substageNum);
                mission.SubStageLockOff();
            }
            else
            {
                lastFlag = true;
                Initflag = true;
                SA_MissionData.End_SubStage(Stage_Num);
            }
        }

        if(Before_Sub_Num != -1)
        {
            foreach(int substageNum in PrevSubStageNum)
            {
                if(substageNum != Select_Sub_Num)
                {
                    MissionDataObject mission = SA_MissionData.missionStage(Mission_Num, Stage_Num, substageNum);
                    if (!mission.StageClearFlag)
                    {
                        mission.SubStageLockOn();
                    }
                }
            }
        }

        if (!Initflag)
        {
            SA_PlayerData.SA_BeforeSubSelectStage_Save(Select_Sub_Num);
        }
        else
        {
            SA_PlayerData.SA_BeforeSubSelectStage_Save(-1);
        }

        Change_Game_End(true, lastFlag);
    }

    private void Change_Game_End(bool WinFlag, bool subStage_Last, int LoseNum = -1) // 이겼을 때
    {
        gameType = GameType.GameEnd;
        Time.timeScale = 0f;
        bool chapter_clearFlag = SA_StageList.Stage[Stage_Num].Stage_ClearFlag;
        if (WinFlag)
        {
            missionDirector.Adjustment_Mission(); // 정보 갱신하기.
            if (!subStage_Last)
            {
                bool flag = missionDirector.selectmission.CheckMission(lastFlag);
                if (flag) // 문제 없다.
                {
                    //Debug.Log("작동 완료" + flag);
                    uiDirector.Open_SubSelect();
                    SA_PlayerData.SA_GameWinReward(false, Total_Coin);
                }
                else
                {
                    //문제가 발생했으니 결과창을 띄운다
                    //미션 실패다.
                    //Debug.Log("작업 해야됨" + flag);
                    missionDirector.selectmission.Mission_Fail();
                    SA_PlayerData.SA_MissionPlaying(false);
                    uiDirector.Open_Result_UI(false, Total_Score, Total_Coin, missionDirector.selectmission, chapter_clearFlag, LoseNum);
                }
            }
            else //마지막스테이지일 때
            {
                bool flag = missionDirector.selectmission.CheckMission(lastFlag);
                if (flag)
                { // 미션 통과다
                    //Debug.Log("마지막 작동 완료" + flag);
                    missionDirector.selectmission.Mission_Sucesses(SA_StageList.Stage[Stage_Num]);
                    
                    uiDirector.Open_Result_UI(true, Total_Score, Total_Coin, missionDirector.selectmission, chapter_clearFlag, LoseNum);
                    LastSubStageClear();
                    SA_PlayerData.change_clickStartButton(false);
                    SA_PlayerData.SA_GameWinReward(true, Total_Coin);
                    SA_PlayerData.SA_MissionPlaying(false);
                    SA_MissionData.SubStage_Init(Stage_Num, Mission_Num); // 승리

                    if(SteamAchievement.instance != null)
                    {
                        SteamAchievement.instance.Achieve("CLEAR_STAGE_" + Stage_Num);
                    }
                    else
                    {
                        Debug.Log("CLEAR_STAGE_" + Stage_Num);
                    }

                }
                else // 미션 실패다
                {
                    //Debug.Log("작업 해야됨 - 실패로 간주하고 초기화해야됨");
                    missionDirector.selectmission.Mission_Fail();
                    SA_PlayerData.SA_MissionPlaying(false);
                    uiDirector.Open_Result_UI(false, Total_Score, Total_Coin, missionDirector.selectmission, chapter_clearFlag, LoseNum);
                }
            }
        }
        else // 패배했을 때... //체력 부족 및 기차가 멈췄을 때,
        {
            missionDirector.selectmission.Mission_Fail();
            SA_PlayerData.SA_MissionPlaying(false);
            uiDirector.Open_Result_UI(false, Total_Score, Total_Coin, missionDirector.selectmission, chapter_clearFlag, LoseNum);
        }
        /*
                if (WinFlag && subStage_Last)
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
                                if (ItemNum != -1)
                                {
                                    itemDirector.itemList.Item[ItemNum].Item_Count_UP(ItemCount);
                                    uiDirector.GetItemList_Num.Add(ItemNum);
                                }
                            }
                        }
                    }*/
    }

    public void MissionFail()
    {
        Change_Game_End(false, false, 2);
    }

    private void LastSubStageClear()
    {
        if (SA_PlayerData.New_Stage == SA_PlayerData.Select_Stage)
        {
            SA_StageList.Stage[Stage_Num].Clear_StageChage();
            try
            {
                SA_StageList.Stage[Stage_Num + 1].Open_StageChange();
            }
            catch
            {
                //Debug.Log("종료");
            }
        }
    }

    private void Game_Lose(int losenum)
    {
        Change_Game_End(false, false ,losenum);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, BGM_ID);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, TrainSFX_ID);
        MMSoundManagerSoundPlayEvent.Trigger(LoseSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        //StageData.GameEnd(false, Total_Score);
        SA_PlayerData.SA_GameLoseCoin(missionDirector.selectmission.MissionCoinLosePersent);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, BGM_ID);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, TrainSFX_ID);
        SA_MissionData.SubStage_Init(Stage_Num, Mission_Num); // 패배
    }

    public void Optoin_Stage()
    {
        SA_PlayerData.SA_GameLoseCoin(60f);
        SA_PlayerData.SA_MissionPlaying(false);
        SA_MissionData.SubStage_Init(Stage_Num, Mission_Num); // 정거장 돌아가기
        missionDirector.selectmission.Mission_Fail();
        LoadingManager.LoadScene("Station");
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

    //Refresh부분
    public void RefreshReward()
    {
        int rewardNum = Random.Range(0, 4);
        //0번 연료
        //1번 플레이어 체력
        //2번 기차 체력
        //3번 종합세트
        int rewardPersent = Random.Range(1, 16);

        switch (rewardNum)
        {
            case 0:
                Item_Fuel_Charge(rewardPersent);
                break;
            case 1:
                player.Item_Player_Heal_HP(rewardPersent);
                break;
            case 2:
                Item_Use_Train_Heal_HP(rewardPersent);
                break;
            case 3:
                Item_Fuel_Charge(rewardPersent);
                player.Item_Player_Heal_HP(rewardPersent);
                Item_Use_Train_Heal_HP(rewardPersent);
                break;
        }
        uiDirector.ItemInformation_On(null, true, rewardNum, rewardPersent);
        getSupply = true;
    }

    //Item부분
    public void Item_Fuel_Charge(float persent)
    {
        int fuelCharge = (int)(Total_TrainFuel * (persent / 100f));
        if(TrainFuel + fuelCharge < Total_TrainFuel)
        {
            TrainFuel += fuelCharge;
        }
        else
        {
            TrainFuel = Total_TrainFuel;
        }
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
            float fac = 11f;
            while (Station_Object.transform.localPosition.x > TargetX)
            {
                if(Time.timeScale != 0)
                {
                    Speed = Time.deltaTime * fac + (TrainSpeed / 1000f);
                }
                else
                {
                    Speed = 0;
                }
                Station_Object.transform.localPosition = new Vector2(Station_Object.transform.localPosition.x - Speed, Station_Object.transform.localPosition.y);
                yield return null;
            }
        }
        else
        {
            float fac = 10f;
            while (Station_Object.transform.localPosition.x > TargetX)
            {
                if (Time.timeScale != 0)
                {
                    Speed = Time.deltaTime * fac + (TrainSpeed / 1000f);
                }
                else
                {
                    Speed = 0;
                }
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

    public void GameEnd_SavePlayerData()
    {
        player.GameEnd_PlayerSave();
        GameEnd_TrainSave_Fuel();
        for(int i = 0; i < Train_List.childCount; i++)
        {
            Train_List.GetChild(i).GetComponent<Train_InGame>().GameEnd_TrainSave();
        }
    }

    void GameEnd_TrainSave_Fuel()
    {
        ES3.Save<int>("Train_Curret_Fuel", TrainFuel);
    }

    public void closeOption()
    {
        gameType = Before_GameType;
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Resume, BGM_ID);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Resume, TrainSFX_ID);
        Time.timeScale = 1f;
    }

    //--------------------------event---------------------------
    public void StormDebuff()
    {
        MaxSpeed -= ((MaxSpeed * 3) / 100); // 많을 수록 유리
        Efficient += ((Efficient * 10) / 100); // 적을 수록 유리
        timeBet = 0.1f - ((EnginePower - 1) * 0.001f);
    }
}


public enum GameType{
    Starting,
    Playing,
    Refreshing,
    Boss,
    Pause,
    Option,
    Ending,
    GameEnd,
}//점차 늘어갈 예정