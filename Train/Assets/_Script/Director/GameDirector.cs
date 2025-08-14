using Cinemachine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.UI;
using static PixelCrushers.AnimatorSaver;
using static UnityEngine.ParticleSystem;


//Script Execution Order로 조절 중
public class GameDirector : MonoBehaviour
{
    [Header("카메라")]
    public CinemachineVirtualCamera virtualCamera;
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
    public MonsterDirector monsterDirector;
    public UIDirector uiDirector;
    public ItemDirector itemDirector;
    public MissionDirector missionDirector;
    public SkillDirector skillDirector;
    public PolygonCollider2D CameraConfiler;
    public FillDirector fill_director;
    public MercenaryDirector mercenaryDirector;
    Vector2[] newPoint;
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
    bool revivalFlag = false;

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
    List<Train_InGame> Trains;
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

    [Header("수리")]
    bool repairFlag = false;
    int repair_Warning_HPCheck;
    int repair_Max_HPCheck;
    int repairCoolTime;

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

    int Total_Coin;

    float distance_lastSpeedTime;
    float distance_time;

    [Header("스테이지에 따른 백그라운드")]
    public GameObject[] BackGroundList;
    [Header("Satation")]
    public GameObject Station_Object;
    public GameObject[] StationObjectList; 
    bool isStationHideFlag;
    bool isStationShowFlag;
    bool isStationHideEndFlag;
    bool isStationShowEndFlag;

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
    bool ItemFlag_Coin; // 골드 추가
    bool ItemFlag_DoubleCoin; // 골드 2배

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

        Mission_Num = 0;
        Stage_Num = 11;
        Select_Sub_Num = 0;

        //TrainDistance = 70000;

        SubStageData = SA_MissionData.missionStage(Mission_Num, Stage_Num, Select_Sub_Num);

        NextSubStageNum = new List<int>();
        string[] nextSubStageList = SubStageData.Open_SubStageNum.Split(',');
        foreach(string sub in nextSubStageList)
        {
            NextSubStageNum.Add(int.Parse(sub));
        }

        if(Select_Sub_Num != 0)
        {
            if (Before_Sub_Num != -1)
            {
                MissionDataObject PrevStageData = SA_MissionData.missionStage(Mission_Num, Stage_Num, Before_Sub_Num);
                PrevSubStageNum = new List<int>();
                string[] prevSubStageList = PrevStageData.Open_SubStageNum.Split(',');

                foreach (string sub in prevSubStageList)
                {
                    PrevSubStageNum.Add(int.Parse(sub));
                }
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
        isStationHideEndFlag = false;
        isStationShowFlag = false;
        isStationShowEndFlag = false;
        fill_director = GetComponent<FillDirector>();

        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();

        cursorAim_UnAtk = Resources.Load<Texture2D>("Cursor/Aim6464_UnAttack");
        cursorAim_Atk = Resources.Load<Texture2D>("Cursor/Aim6464_Attack");
        cursorOrigin = Resources.Load<Texture2D>("Cursor/Origin6464");
        cursorHotspot_Origin = Vector2.zero;
        cursorHotspot_Aim = new Vector2(cursorAim_UnAtk.width / 2, cursorAim_UnAtk.height / 2);
        ChangeCursor(true);
        BossGuage = uiDirector.BossHP_Guage;

        StageBackGround_Setting();
        Stage_Init();
        Train_Init();

        RefreshPersent = 50;
        RefreshDistance = (int)(Destination_Distance * (RefreshPersent / 100));

        newPoint = new Vector2[4];
        {
            newPoint[0] = new Vector2(3.5f + 20f, -4);
            newPoint[1] = new Vector2(3.5f + 20f, 14.5f);
            newPoint[2] = new Vector2(-5.47f + (-10.94f * (Train_Count - 1)) - 20f, 14.5f);
            newPoint[3] = new Vector2(-5.47f + (-10.94f * (Train_Count - 1)) - 20f, -4);
        }
        CameraConfiler.points = newPoint;

        RandomStartTime = Random.Range(4f, 6f);
        BossCount = 0;
        lastSpeedTime = 0;
        distance_lastSpeedTime = 0;
        //기차 능력에 따라 결정
        timeBet = 0.1f - (EnginePower * 0.002f); //엔진 파워에 따라 결정
        TrainSpeedUP = 1;
        distance_time = 0.1f;

        ItemFlag_Coin = false;
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

        uiDirector.Gameing_Text(Total_Coin);
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
            if (!isStationHideEndFlag)
            {
                if(Time.time >= lastSpeedTime + 0.2f)
                {
                    if (9 >= TrainSpeed)
                    {
                        TrainSpeed += TrainSpeedUP;
                    }
                    lastSpeedTime = Time.time;
                }
            }
            else
            {
                if (Time.time >= RandomStartTime + StartTime && !GameStartFlag)
                {
                    GameStartFlag = true;
                    monsterDirector.GameDirector_SpawnFlag = true;
                    SoundSequce(PlayBGM);
                }
                else if (Time.time >= lastSpeedTime + timeBet && GameWinFlag == false)
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

            if(player.Player_HP <= 0 && revivalFlag)
            {
                StartCoroutine(RevivalEffect(0.1f, 3, 2));
            }

            if ((TrainSpeed <= 0 || (player.Player_HP <= 0 && !revivalFlag) && GameStartFlag && !GameLoseFlag))
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
                        //Debug.Log(Emerging_Boss_Monster_Count[BossCount]);
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

                if(TrainSpeed > 90)
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
                if(TrainSpeed < 0)
                {
                    TrainSpeed = 0;
                }

                int LoseNum = 0;
                if (TrainSpeed <= 0)
                {
                    LoseNum = 0;
                }
                else if (player.Player_HP <= 0)
                {
                    LoseNum = 1;
                }
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

                    if (!isStationShowFlag)
                    {
                        if (Time.time >= lastSpeedTime + 0.025f)
                        {
                            if (TrainSpeed >= 11)
                            {
                                TrainSpeed -= TrainSpeedUP * 2;
                            }
                            else if (TrainSpeed < 12)
                            {
                                TrainSpeed += TrainSpeedUP;
                            }
                            lastSpeedTime = Time.time;
                        }
                    }
                    else
                    {
                        if (Time.time >= lastSpeedTime + 0.6f)
                        {
                            if (TrainSpeed > 1)
                            {
                                TrainSpeed -= TrainSpeedUP;
                            }
                            lastSpeedTime = Time.time;
                        }
                    }


                    /*if (TrainSpeed <= 74f && TrainSpeed > 72f && !isStationShowFlag)
                    {
                        isStationShowFlag = true;
                        StartCoroutine(Hide_And_Show_Station(false));
                    }*/

                    if (isStationShowEndFlag)
                    {
                        gameType = GameType.GameEnd;
                        Game_Win();
                        TrainSpeed = 0;
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
                    if (TrainSpeed < 12f && TrainSpeed >= 10f && !isStationShowFlag)
                    {
                        isStationShowFlag = true;
                        StartCoroutine(Hide_And_Show_Station(false));
                    }
                }
            }
        }
    }

    void Stage_Init()
    {
        Emerging_Monster_String = SubStageData.Emerging_Monster;
        Emerging_MonsterCount_String = SubStageData.Monster_Count;
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

            string[] Boss_String = SubStageData.SubStage_Status.Split(',');
            Emerging_Boss.Add(int.Parse(Boss_String[0]));
            Emerging_Boss_Distance.Add(int.Parse(Boss_String[1]));
            Emerging_Boss_Monster_Count.Add(int.Parse(Boss_String[2]));
            monsterDirector.Get_Boss_List(Emerging_Boss);
        }
    }

    void StageBackGround_Setting()
    {
        int _stageNum = EX_GameData.Information_Stage[Stage_Num].GameBackGround;
        BackGroundList[_stageNum].SetActive(true);
        StationObjectList[_stageNum].SetActive(true);
    }

    void Train_Init()
    {
        Trains = new List<Train_InGame>();
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
            else if (Train_Num[i] == 90)//호위차량
            {
                TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_InGame/" + Train_Num[i]), Train_List);
                Train_InGame _train = TrainObject.GetComponent<Train_InGame>();
                _train.Max_Train_HP = 20000;
                _train.Train_HP = _train.Max_Train_HP;
                try
                {
                    _train.Train_Weight = missionDirector.selectmission.M_Convoy.ConvoyWeight;
                }
                catch
                {
                    Debug.Log("테스트");
                    _train.Train_Weight = 10000;
                }
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
            train.trainHitSFX = TrainHitSFX;

            if(train.Train_Type.Equals("Engine"))
            {
                TrainMaxSpeed = train.Train_MaxSpeed;
                TrainEfficient = train.Train_Efficient;
                TrainEnginePower = train.Train_Engine_Power;
            }else if (train.Train_Type.Equals("Fuel"))
            {
                TrainFuel += train.Train_Fuel;
            }
            TrainWeight += train.Train_Weight;
            Trains.Add(train);
        }

        Train_Count = Train_List.childCount;

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

    public void Game_Monster_Kill(int GetCoin)
    {
        if (ItemFlag_DoubleCoin)
        {
            Total_Coin += GetCoin * 2;
        }else if (ItemFlag_Coin)
        {
            Total_Coin += (GetCoin + Random.Range(0, 201));
        }
        else
        {
            Total_Coin += GetCoin;
        }

        uiDirector.Gameing_Text(Total_Coin);
    }

    public void Mission_Monster_Kill()
    {
        missionDirector.MonsterCount();
    }

    public void Gmae_Boss_Kill(int GetCoin) //보스는 2배 적용 X
    {
        if(!ItemFlag_Coin)
        {
            Total_Coin += GetCoin;
        }
        else
        {
            int coin = Random.Range(0,201);
            Total_Coin += (GetCoin + coin);
        }
        BossCount++;
        monsterDirector.BossDie();
        uiDirector.BossHP_Object.SetActive(false);
        uiDirector.Gameing_Text(Total_Coin);
        gameType = GameType.Playing;
        SoundSequce(PlayBGM);
    }
   

    private void Game_Win()
    {
        GameEnd_SavePlayerData();
        SubStage_Clear();
        SubStage_LockOff();
        MMSoundManagerSoundPlayEvent.Trigger(WinSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
    }

    void SubStage_Clear()
    {
        SubStageData.SubStage_Clear();
    }

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
                    uiDirector.Open_Result_UI(false, Total_Coin, missionDirector.selectmission, chapter_clearFlag, LoseNum);
                }
            }
            else //마지막스테이지일 때
            {
                bool flag = missionDirector.selectmission.CheckMission(lastFlag);
                if (flag)
                { // 미션 통과다
                    //Debug.Log("마지막 작동 완료" + flag);
                    missionDirector.selectmission.Mission_Sucesses(SA_StageList.Stage[Stage_Num]);
                    
                    uiDirector.Open_Result_UI(true, Total_Coin, missionDirector.selectmission, chapter_clearFlag, LoseNum);
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
                    uiDirector.Open_Result_UI(false, Total_Coin, missionDirector.selectmission, chapter_clearFlag, LoseNum);
                }
            }
        }
        else // 패배했을 때... //체력 부족 및 기차가 멈췄을 때,
        {
            missionDirector.selectmission.Mission_Fail();
            SA_PlayerData.SA_MissionPlaying(false);
            uiDirector.Open_Result_UI(false, Total_Coin, missionDirector.selectmission, chapter_clearFlag, LoseNum);
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

    public void DestoryEngine()
    {
        int LoseNum = 3;
        Game_Lose(LoseNum);
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

    public void FuelSignalSupply(int parsent)
    {
        Item_Fuel_Charge(parsent);
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

    public void Item_Spawn_Train_BulletproofPlate(int hp)
    {
        for(int i = 0; i < Train_List.childCount; i++)
        {
            Train_List.GetChild(i).GetComponent<Train_InGame>().Item_Spawn_IronPlate(hp);
        }
    }

    public void Item_Train_Armor_Up(int delayTime, int parsent)
    {
        for(int i = 0; i < Train_List.childCount; i++)
        {
            Train_List.GetChild(i).GetComponent<Train_InGame>().Item_Armor_Up(delayTime, parsent);
        }
    }

    public void Item_Use_Lucky_Coin()
    {
        int Rand = Random.Range(0, 2);
        StartCoroutine(uiDirector.CoinAni(Rand));
    }

    public void Item_Use_Lucky_Dice()
    {
        int Rand = Random.Range(1, 7);
        StartCoroutine(uiDirector.DiceAni(Rand));
    }

    public void Item_Use_Map(int persent)
    {
        TrainDistance += (int)(Destination_Distance * (persent / 100.0));
    }

    public IEnumerator Item_Train_SpeedUp(float delayTime, float SpeedUp = 0.5f)
    {
        TrainSpeedUP += SpeedUp;
        yield return new WaitForSeconds(delayTime);
        TrainSpeedUP -= SpeedUp;
    }

    public IEnumerator Item_Coin_Plus(int delayTime)
    {
        ItemFlag_Coin = true;
        yield return new WaitForSeconds(delayTime);
        ItemFlag_Coin = false;
    }

    public IEnumerator Item_Double_Coin(int delayTime)
    {
        ItemFlag_DoubleCoin = true;
        yield return new WaitForSeconds(delayTime);
        ItemFlag_DoubleCoin = false;
    }

    public void Item_Dice_Reward(int num)
    {
        if (num == 1)
        {
            player.Item_Player_Heal_HP(15f);
        }
        else if (num == 2)
        {
            Item_Use_Train_Heal_HP(15f);
        }
        else if (num == 3)
        {
            Item_Fuel_Charge(15f);
        }
        else if (num == 4)
        {
            Total_Coin += 1000;
            uiDirector.Gameing_Text(Total_Coin);
        }
        else if (num == 5)
        {
            itemDirector.Item_Reward_Equiped();
        }
        else if (num == 6)
        {
            Item_Fuel_Charge(10f);
            player.Item_Player_Heal_HP(10f);
            Item_Use_Train_Heal_HP(10f);
        }
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
            StartX = 40f;
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
            float fac = 3.6f;
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
            isStationHideEndFlag = true;
            StartTime = Time.time;
        }
        else
        {
            TrainSpeed = 0;
            isStationShowEndFlag = true;
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

    //----------------수리-------------------
    public void EngineerSet(int w_hp, int m_hp,  int cooltime)
    {
        repairFlag = true;
        repair_Warning_HPCheck = w_hp;
        repair_Max_HPCheck = m_hp;
        repairCoolTime = cooltime;
        foreach (Train_InGame train in Trains)
        {
            train.RepairEngineerSet(w_hp, cooltime);
        }
    }

    public void EngineerCall(Train_InGame train)
    {
        mercenaryDirector.Engineer_Call(train);
    }

    //--------------------------event---------------------------
    public void StormDebuff()
    {
        MaxSpeed -= ((MaxSpeed * 3) / 100); // 많을 수록 유리
        Efficient += ((Efficient * 10) / 100); // 적을 수록 유리
        timeBet = 0.1f - ((EnginePower - 1) * 0.001f);
    }

    //---------------------------revival-------------------
    public void Revival_PlayerSet()
    {
        revivalFlag = true;
    }

    public void Revival_PlayerUse()
    {
        itemDirector.RevivalUse();
    }

    IEnumerator RevivalEffect(float slowTarget, float slowDuration, float restoreDuration)
    {
        player.revival_Effect_Flag = true;

        // 현재 타임스케일에서 slowTarget까지 점점 감소
        Revival_PlayerUse();

        float startScale = Time.timeScale;
        float elapsed = 0f;
        float CameraZoom_Init = virtualCamera.m_Lens.OrthographicSize;
        float originalFixedDelta = Time.fixedDeltaTime;
        float fixedDeltaStart = Time.fixedDeltaTime;

        float SmootherStep(float x)
        {
            //return x * x * x * (x * (6f * x - 15f) + 10f);
            //return x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
            return x * x * x * x * (35f - 84f * x + 70f * x * x - 20f * x * x * x);
        }

        // --- 느려지는 메인 루프 ---
        while (elapsed < slowDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float rawT = Mathf.Min(elapsed / slowDuration, 1f);
            //float t = Mathf.Clamp01(elapsed / slowDuration);
            float smoothT = SmootherStep(rawT);

            Time.timeScale = Mathf.Clamp(Mathf.Lerp(startScale, slowTarget, smoothT), 0.01f, 1.5f);
            Time.fixedDeltaTime = Mathf.Clamp(Mathf.Lerp(fixedDeltaStart, originalFixedDelta * slowTarget, smoothT), 0.0001f, 0.05f);
            if(virtualCamera.m_Lens.OrthographicSize > 5.01f)
            {
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(CameraZoom_Init, 5f, smoothT);
            }
            else
            {
                virtualCamera.m_Lens.OrthographicSize = 5f;
            }

                yield return null;
        }



        // --- 최종 보정 ---
        Time.timeScale = slowTarget;
        Time.fixedDeltaTime = originalFixedDelta * slowTarget;
        virtualCamera.m_Lens.OrthographicSize = 5f;
        // --- HP 회복 ---
        while (player.Player_HP < (player.Max_HP / 2))
        {
            player.Player_HP += 1;
            yield return new WaitForSecondsRealtime(0.1f); // timeScale 무시
        }

        // --- 빠르게 복귀 ---
        elapsed = 0f;
        startScale = Time.timeScale;

        while (elapsed < restoreDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / restoreDuration);

            Time.timeScale = Mathf.Lerp(startScale, 1f, t);
            Time.fixedDeltaTime = originalFixedDelta * Time.timeScale;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(5f, 11f, t);
            yield return null;
        }

        // 마지막 값 보정
        Time.timeScale = 1f;
        virtualCamera.m_Lens.OrthographicSize = 11f;
        Time.fixedDeltaTime = originalFixedDelta;

        player.revival_Effect_Flag = false;
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