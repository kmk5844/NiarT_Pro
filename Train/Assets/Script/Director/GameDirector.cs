using Cinemachine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class GameDirector : MonoBehaviour
{
    public GameType gameType;
    [Header("데이터 모음")]
    public SA_TrainData SA_TrainData;
    public SA_TrainTurretData SA_TrainTurretData;
    public SA_TrainBoosterData SA_TrainBoosterData;
    public SA_MercenaryData SA_MercenaryData;
    public SA_PlayerData SA_PlayerData;
    public Game_DataTable EX_GameData;
    public Level_DataTable EX_LevelData;

    [Header("디렉터")]
    public GameObject MonsterDirector;
    public GameObject UI_DirectorObject;
    MonsterDirector monsterDirector;
    UIDirector uiDirector;
    List<int> Train_Num;
    List<int> Train_Turret_Num;
    List<int> Train_Booster_Num;
    int Train_Turret_Count;
    int Train_Booster_Count;
    GameObject TrainObject;

    Texture2D cursorOrigin;
    Texture2D cursorAim;
    Vector2 cursorHotspot_Origin;
    Vector2 cursorHotspot_Aim;

    GameObject playerObject;
    [HideInInspector]
    public Player player;

    [Header("스테이지 정보")]
    [SerializeField]
    public int Stage_Num;
    string Emerging_Monster_String;
    [SerializeField]
    private List<int> Emerging_Monster;
    [SerializeField]
    private int Reward_Point;
    [SerializeField]
    private int Destination_Distance; // 나중에 private변경

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

    [Header("Train_Cam")]
    public Transform TrainCamList;
    public GameObject TrainCam_Prefeb;
    public GameObject Station;

    [Header("BGM")]
    public AudioClip DustWindBGM;
    public AudioClip PlayBGM;
    public AudioClip WinBGM;
    public AudioClip WinSFX;
    public AudioClip LoseSFX;
    bool Change_Win_BGM_Flag;
    int BGM_ID;

    //아이템부분
    bool ItemFlag_14;

    void Awake()
    {
        gameType = GameType.Playing;
        Stage_Num = SA_PlayerData.Stage;
        BGM_ID = 30;
        Change_Win_BGM_Flag = false;
        GameStartFlag = false;
        GameWinFlag = false;
        GameLoseFlag = false;
        SpawnTrainFlag = false;
        monsterDirector = MonsterDirector.GetComponent<MonsterDirector>();
        uiDirector = UI_DirectorObject.GetComponent<UIDirector>();

        cursorOrigin = Resources.Load<Texture2D>("Cursor/Origin6464");
        cursorAim = Resources.Load<Texture2D>("Cursor/Aim6464");
        cursorHotspot_Origin = Vector2.zero;
        cursorHotspot_Aim = new Vector2(cursorAim.width / 2, cursorAim.height / 2);

        Stage_Init();
        Train_Init();
        RandomStartTime = Random.Range(5f, 8f);
        lastSpeedTime = 0;
        distance_lastSpeedTime = 0;
        timeBet = 0.1f - (EnginePower * 0.001f); //엔진 파워에 따라 결정
        TrainSpeedUP = 1;
        distance_time = 0.1f;
        ChangeCursor(true);
        //
        ItemFlag_14 = false;
    }

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        StartTime = Time.time;
        gameType = GameType.Playing;
        MMSoundManagerSoundPlayEvent.Trigger(DustWindBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID: BGM_ID);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameType == GameType.Playing)
            {
                gameType = GameType.Pause;
                Time.timeScale = 0f;
            }
            else if (gameType == GameType.Pause)
            {
                gameType = GameType.Playing;
                Time.timeScale = 1f;
            }
        }

        if (gameType == GameType.Playing)
        {
            ChangeCursor(true);
            uiDirector.Gameing_Text(Total_Score, Total_Coin);
            if (Time.time >= RandomStartTime + StartTime && !GameStartFlag)
            {
                GameStartFlag = true;
                monsterDirector.GameDirector_SpawnFlag = true;
                SoundSequce(PlayBGM);
            }else if (Time.time >= lastSpeedTime + timeBet && GameWinFlag == false)
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
                TrainSpeed = 0;
                GameLoseFlag = true;
                Game_Lose();
            }

            if (!GameWinFlag || !GameLoseFlag)
            {
                if (Time.time >= distance_lastSpeedTime + distance_time)
                {
                    TrainDistance += (int)TrainSpeed;
                    distance_lastSpeedTime = Time.time;
                }
            }
        }
        else if (gameType == GameType.Ending)
        {
            ChangeCursor(false);
            monsterDirector.GameDirector_SpawnFlag = false;
            if (!Change_Win_BGM_Flag)
            {
                SoundSequce(WinBGM);
                Change_Win_BGM_Flag = true;
            }

            if (Time.time >= lastSpeedTime + 0.025f)
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

            if(TrainSpeed <= 40 && TrainSpeed > 35)
            {
                Station.SetActive(true);
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
        Emerging_Monster_String = EX_GameData.Information_Stage[Stage_Num].Emerging_Monster;
        Reward_Point = EX_GameData.Information_Stage[Stage_Num].Reward_Point;
        Destination_Distance = EX_GameData.Information_Stage[Stage_Num].Destination_Distance;
        Emerging_Monster = new List<int>();

        string[] Monster_String = Emerging_Monster_String.Split(',');
        foreach(string M in Monster_String)
        {
            int num;
            if(int.TryParse(M, out num))
            {
                Emerging_Monster.Add(num);
            }
        }
        MonsterDirector.GetComponent<MonsterDirector>().Get_Monster_List(Emerging_Monster);
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
                TrainObject.transform.position = new Vector3(-0.57f, 0.35f, 0);
            }
            else
            {
                //나머지칸
                TrainObject.transform.position = new Vector3(-10.94f * i, 0.35f, 0);
            }
            
            Train_InGame train = TrainObject.GetComponent<Train_InGame>();
            TrainFuel += train.Train_Fuel;
            TrainWeight += train.Train_Weight;
            TrainMaxSpeed += train.Train_MaxSpeed;
            TrainEfficient += train.Train_Efficient;
            TrainEnginePower += train.Train_Engine_Power;
            Instantiate_TrainCam(TrainObject);
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
        MaxSpeed = TrainMaxSpeed + ((TrainMaxSpeed * (Level_MaxSpeed *10)) / 100); // 많을 수록 유리
        MaxSpeed = MaxSpeed - (TrainWeight / 100000); //무게로 인해 speed 감소

        Efficient = TrainEfficient - ((TrainEfficient * (Level_Efficient * 10)) / 100); // 적을 수록 유리
        EnginePower = TrainEnginePower + ((TrainEnginePower * (Level_EngineTier * 10)) / 100); // 클수록 유리
    }

    public void Game_MonsterHit(int slow)
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

    }
    private string Check_Score()
    {
        if (Total_Score >= EX_GameData.Information_Stage[Stage_Num].S_Grade)
        {
            return "S";
        }
        else if (EX_GameData.Information_Stage[Stage_Num].S_Grade > Total_Score && Total_Score >= EX_GameData.Information_Stage[Stage_Num].A_Grade)
        {
            return "A";
        }
        else if (EX_GameData.Information_Stage[Stage_Num].A_Grade > Total_Score && Total_Score >= EX_GameData.Information_Stage[Stage_Num].B_Grade)
        {
            return "B";
        }
        else if (EX_GameData.Information_Stage[Stage_Num].B_Grade > Total_Score && Total_Score >= EX_GameData.Information_Stage[Stage_Num].C_Grade)
        {
            return "C";
        }
        else if (EX_GameData.Information_Stage[Stage_Num].C_Grade > Total_Score && Total_Score >= EX_GameData.Information_Stage[Stage_Num].D_Grade)
        {
            return "D";
        }
        else if (EX_GameData.Information_Stage[Stage_Num].D_Grade > Total_Score)
        {
            return "F";
        }
        return "F";
    }

    private void Change_Game_End(bool WinFlag) // 이겼을 때
    {
        gameType = GameType.GameEnd;
        Time.timeScale = 0f;
        if (WinFlag)
        {
            uiDirector.Win_Text(Stage_Num, Total_Score, Check_Score(), Total_Coin, Reward_Point);
            uiDirector.Open_WIN_UI();
        }
        else
        {
            uiDirector.Lose_Text(Total_Coin);
            uiDirector.Open_Lose_UI();
        }
    }

    private void Game_Win()
    {
        Change_Game_End(true);
        MMSoundManagerSoundPlayEvent.Trigger(WinSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        SA_PlayerData.SA_GameWinReward(Total_Coin, Reward_Point);
    }

    private void Game_Lose()
    {
        Change_Game_End(false);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, BGM_ID);
        MMSoundManagerSoundPlayEvent.Trigger(LoseSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        SA_PlayerData.SA_GameLoseReward(Total_Coin);
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

    private void ChangeCursor(bool flag)
    {
        if (flag) // 게임 진행 중일 때
        {
            Cursor.SetCursor(cursorAim, cursorHotspot_Aim, CursorMode.Auto);
        }
        else // Pause했을 때
        {
            Cursor.SetCursor(cursorOrigin, cursorHotspot_Origin, CursorMode.Auto);
        }
    }

    private void Instantiate_TrainCam(GameObject Train)
    {
        GameObject Cam = Instantiate(TrainCam_Prefeb, TrainCamList);
        Cam.GetComponent<CinemachineVirtualCamera>().Follow = Train.transform;
        Cam.GetComponent<CinemachineVirtualCamera>().LookAt = Train.transform;
        Cam.name = Train.name.Replace("(Clone)", "") + "_Cam";
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

    public void Item_Use_Heal_TrainHP(float persent)
    {
        for(int i = 0; i < Train_List.childCount; i++)
        {
            Train_List.GetChild(i).GetComponent<Train_InGame>().Item_Heal_TrainHP(persent);
        }
    }

    public IEnumerator Item_Train_SpeedUp(int delayTime)
    {
        TrainSpeedUP += 0.5f;
        yield return new WaitForSeconds(delayTime);
        TrainSpeedUP -= 0.5f;
    }

    public IEnumerator Item_Coin_Double(int delayTime)
    {
        ItemFlag_14 = true;
        yield return new WaitForSeconds(delayTime);
        ItemFlag_14 = false;
    }
}


public enum GameType{
    Playing,
    Pause,
    Option,
    Ending,
    GameEnd,
}//점차 늘어갈 예정