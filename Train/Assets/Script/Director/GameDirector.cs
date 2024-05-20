using Cinemachine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public GameType gameType;
    [Header("������ ����")]
    public SA_TrainData SA_TrainData;
    public SA_PlayerData SA_PlayerData;
    public Game_DataTable EX_GameData;
    public GameObject MonsterDirector;
    List<int> Trian_Num;

    Texture2D cursorOrigin;
    Texture2D cursorAim;
    Vector2 cursorHotspot_Origin;
    Vector2 cursorHotspot_Aim;

    [Header("�������� ����")]
    public int Stage_Num;
    public string Stage_Name;
    string Emerging_Monster_String;
    [SerializeField]
    private List<int> Emerging_Monster;
    [SerializeField]
    private int Reward_Point;
    [SerializeField]
    private int Destination_Distance; // ���߿� private����

    [Header("���� ����Ʈ")]
    public Transform List_Train;
    public Train_InGame[] Trains;
    int Train_Count;

    [Header("���� ����")]
    [SerializeField]
    int TrainFuel; // ��ü������ ���Ѵ�.
    int Total_TrainFuel;
    public int TrainSpeed;
    public int TrainDistance;
    [SerializeField]
    int TrainWeight;// ��ü������ ���Ѵ�.

    [Header("���� �� ���� ���� ����")]
    public int TrainMaxSpeed;
    public int TrainEfficient;
    public int TrainEnginePower;

    [Header("���� �� ���� ���� ����")]
    [SerializeField]
    int MaxSpeed;
    [SerializeField]
    int Efficient;
    [SerializeField]
    int EnginePower;

    [Header("�ð� ����")]
    [SerializeField]
    float lastSpeedTime; //������ �ӵ� �ø� �ð�
    [SerializeField]
    float timeBet; //�ð� ����
    float StartTime;
    int RandomStartTime;

    GameObject Respawn;

    int Level_EngineTier; // km/h�� �����ϴ� ���� �Ŀ�
    int Level_MaxSpeed; // �߽� ���ǵ� ����
    int Level_Efficient; // �⸧ ȿ����

    public bool GameStartFlag;
    bool GameWinFlag;
    bool GameLoseFlag;

    [SerializeField]
    int Total_Score;
    [SerializeField]
    int Total_Coin;

    [Header("UI")]
    public GameObject UI_DirectorObject;
    UIDirector uiDirector;

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
    public float Train_Min_X;

    [Header("���͸���Ʈ")]
    public GameObject Monster_List;

    void Awake()
    {
        gameType = GameType.Playing;
        Stage_Num = SA_PlayerData.Stage;
        BGM_ID = 30;
        Change_Win_BGM_Flag = false;
        GameStartFlag = false;
        GameWinFlag = false;
        GameLoseFlag = false;
        uiDirector = UI_DirectorObject.GetComponent<UIDirector>();
        cursorOrigin = Resources.Load<Texture2D>("Cursor/Origin6464");
        cursorAim = Resources.Load<Texture2D>("Cursor/Aim6464");
        cursorHotspot_Origin = Vector2.zero;
        cursorHotspot_Aim = new Vector2(cursorAim.width / 2, cursorAim.height / 2);

        Stage_Init();
        Train_Init();
        RandomStartTime = Random.Range(5, 10);
        lastSpeedTime = 0;
        distance_lastSpeedTime = 0;
        timeBet = 0.1f - (EnginePower * 0.001f); //���� �Ŀ��� ���� ����
        distance_time = 0.1f;
        ChangeCursor(true);
    }

    private void Start()
    {
        StartTime = Time.time;
        gameType = GameType.Playing;
        MMSoundManagerSoundPlayEvent.Trigger(DustWindBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID: BGM_ID);
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
        MMSoundManagerSoundFadeEvent.Trigger(MMSoundManagerSoundFadeEvent.Modes.StopFade, BGM_ID-1, 3f, 0f, new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));

        // frees the sound at the end (still using that same ID)
        yield return MMCoroutine.WaitFor(3f);
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, BGM_ID-1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameType == GameType.Playing)
            {
                gameType = GameType.Pause;
                Time.timeScale = 0f;
            }else if(gameType == GameType.Pause)
            {
                gameType = GameType.Playing;
                Time.timeScale = 1f;
            }
        }

       if(gameType == GameType.Playing)
        {
            ChangeCursor(true);
            if (Time.time >= RandomStartTime + StartTime && !GameStartFlag)
            {
                GameStartFlag = true;
                MonsterDirector.GetComponent<MonsterDirector>().GameDirector_StartFlag = true;
                SoundSequce(PlayBGM);
            }else if (Time.time >= lastSpeedTime + timeBet && GameWinFlag == false)
            {
                if (MaxSpeed >= TrainSpeed)
                {
                    if (TrainFuel > 0)
                    {
                        TrainSpeed++;
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
           
            if (Destination_Distance < TrainDistance && !GameWinFlag)
            {
                GameWinFlag = true;
                gameType = GameType.Ending;
            }

            if (TrainSpeed <= 0 && GameStartFlag && !GameLoseFlag)
            {
                TrainSpeed = 0;
                GameLoseFlag = true;
                Game_Lose();
            }

            if (TrainFuel <= 0)
            {
                TrainFuel = 0;
            }

            if (!GameWinFlag || !GameLoseFlag)
            {
                if (Time.time >= distance_lastSpeedTime + distance_time)
                {
                    TrainDistance += TrainSpeed;
                    distance_lastSpeedTime = Time.time;
                }
            }
            else
            {
                TrainSpeed = 0;
            }
        }
        else if (gameType == GameType.Ending)
        {
            if (!Change_Win_BGM_Flag)
            {
                SoundSequce(WinBGM);
                Monster_List.SetActive(false);
                Change_Win_BGM_Flag = true;
            }

            if (Time.time >= lastSpeedTime + 0.03f)
            {
                if (TrainSpeed > 0)
                {
                    TrainSpeed -= 1;
                }
                else
                {
                    TrainSpeed = 0;
                }
                lastSpeedTime = Time.time;
            }

            if(TrainSpeed <= 50 && TrainSpeed > 45)
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
        Stage_Name = EX_GameData.Information_Stage[Stage_Num].Stage_Name;
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
        Trian_Num = SA_TrainData.Train_Num;
        for (int i = 0; i < Trian_Num.Count; i++)
        {
            GameObject TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_InGame/" + Trian_Num[i]), List_Train);
            TrainObject.name = EX_GameData.Information_Train[Trian_Num[i]].Train_Name;
            if (i == 0)
            {
                //����ĭ
                TrainObject.transform.position = new Vector3(-0.57f, 0.35f, 0);
            }
            else
            {
                //������ĭ
                TrainObject.transform.position = new Vector3(-10.94f * i, 0.35f, 0);
            }
            TrainObject.GetComponent<Train_InGame>().TrainNum = Trian_Num[i];
            Instantiate_TrainCam(TrainObject);
        }

        Train_Count = List_Train.childCount;
        Trains = new Train_InGame[Train_Count];
        Respawn = GameObject.FindGameObjectWithTag("Respawn");
        Respawn.transform.localScale = new Vector3(15 * Train_Count, 1, 0);
        Respawn.transform.position = new Vector3(List_Train.GetChild(Train_Count / 2).transform.position.x, -3, 0);

        for (int i = 0; i < Train_Count; i++)
        {
            Trains[i] = List_Train.GetChild(i).gameObject.GetComponent<Train_InGame>();
            TrainFuel += Trains[i].Train_Fuel;
            TrainWeight += Trains[i].Train_Weight;
            TrainMaxSpeed += Trains[i].Train_MaxSpeed;
            TrainEfficient += Trains[i].Train_Efficient;
            TrainEnginePower += Trains[i].Train_Engine_Power;
        }

        Level_EngineTier = SA_TrainData.Level_Train_EngineTier;
        Level_MaxSpeed = SA_TrainData.Level_Train_MaxSpeed;
        Level_Efficient = SA_TrainData.Level_Train_Efficient;
        Total_TrainFuel = TrainFuel;
        Level();
    }
    public void Level()
    {
        MaxSpeed = TrainMaxSpeed + ((TrainMaxSpeed * (Level_MaxSpeed *10)) / 100); // ���� ���� ����
        MaxSpeed = MaxSpeed - (TrainWeight / 100000);

        Efficient = TrainEfficient - ((TrainEfficient * (Level_Efficient * 10)) / 100); // ���� ���� ����
        EnginePower = TrainEnginePower + ((TrainEnginePower * (Level_EngineTier * 10)) / 100); // Ŭ���� ����
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

    public void Engine_Driver_Passive(Engine_Driver_Type type, int EngineDriver_value, bool survival) //������ ������ ����� �Ǿ��ִ�.
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
        Total_Coin += GetCoin;
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

    private void Change_Game_End()
    {
        gameType = GameType.GameEnd;
        Time.timeScale = 0f;
    }

    private void Game_Win()
    {
        Change_Game_End();
        uiDirector.Win_Text(Stage_Num, Stage_Name, Total_Score, Check_Score(),Total_Coin, Reward_Point);
        uiDirector.Open_WIN_UI();

        MMSoundManagerSoundPlayEvent.Trigger(WinSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        SA_PlayerData.SA_GameWinReward(Total_Coin, Reward_Point);
    }

    private void Game_Lose()
    {
        Change_Game_End();
        uiDirector.Lose_Text(Total_Coin);
        uiDirector.Open_Lose_UI();

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

    public void PauseButton()
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
        if (flag) // ���� ���� ���� ��
        {
            Cursor.SetCursor(cursorAim, cursorHotspot_Aim, CursorMode.ForceSoftware);
        }
        else // Pause���� ��
        {
            Cursor.SetCursor(cursorOrigin, cursorHotspot_Origin, CursorMode.ForceSoftware);
        }
    }

    private void Instantiate_TrainCam(GameObject Train)
    {
        GameObject Cam = Instantiate(TrainCam_Prefeb, TrainCamList);
        Cam.GetComponent<CinemachineVirtualCamera>().Follow = Train.transform;
        Cam.GetComponent<CinemachineVirtualCamera>().LookAt = Train.transform;
        Cam.name = Train.name + "_Cam";
    }
}


public enum GameType{
    Playing,
    Pause,
    Option,
    Ending,
    GameEnd,
}//���� �þ ����