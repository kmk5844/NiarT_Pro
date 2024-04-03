using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [Header("데이터 모음")]
    public SA_TrainData SA_TrainData;
    public SA_PlayerData SA_PlayerData;
    public Game_DataTable EX_GameData;
    public GameObject MonsterDirector;
    List<int> Trian_Num;

    [Header("스테이지 정보")]
    public int Stage_Num;
    public string Stage_Name;
    string Emerging_Monster_String;
    [SerializeField]
    private List<int> Emerging_Monster;
    [SerializeField]
    private int Reward_Point;
    [SerializeField]
    private int Destination_Distance; // 나중에 private변경

    [Header("기차 리스트")]
    public Transform List_Train;
    public Train_InGame[] Trains;
    int Train_Count;

    [Header("기차 정보")]
    [SerializeField]
    int TrainFuel; // 전체적으로 더한다.
    public int TrainSpeed;
    public int TrainDistance;
    [SerializeField]
    int TrainWeight;// 전체적으로 더한다.

    [Header("레벨 업 적용 전의 기차")]
    public int TrainMaxSpeed;
    public int TrainEfficient;
    public int TrainEnginePower;

    [Header("레벨 업 적용 후의 기차")]
    [SerializeField]
    int MaxSpeed;
    [SerializeField]
    int Efficient;
    [SerializeField]
    int EnginePower;

    [Header("시간 정보")]
    [SerializeField] 
    float lastSpeedTime; //마지막 속도 올린 시간
    [SerializeField]
    float timeBet; //시간 차이

    GameObject Respawn;

    int Level_EngineTier; // km/h을 증가하는 엔진 파워
    int Level_MaxSpeed; // 멕스 스피드 조절
    int Level_Efficient; // 기름 효율성

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

    void Start()
    {
        Stage_Num = SA_PlayerData.Stage;
        GameStartFlag = false;
        GameWinFlag = false;
        GameLoseFlag = false;
        uiDirector = UI_DirectorObject.GetComponent<UIDirector>();

        Stage_Init();
        Train_Init();

        lastSpeedTime = 0;
        timeBet = 0.1f - (EnginePower * 0.001f); //엔진 파워에 따라 결정
    }

    void Update()
    {
        if(Time.time > 5 && !GameStartFlag)
        {
            GameStartFlag = true;
        }

        if(Time.time >= lastSpeedTime + timeBet && GameWinFlag == false)
        {
            if(MaxSpeed >= TrainSpeed)
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

        if(Destination_Distance < TrainDistance && !GameWinFlag)
        {
            GameWinFlag = true;
            Game_Win();
        }

        if (TrainSpeed <= 0 && GameStartFlag && !GameLoseFlag)
        {
            TrainSpeed = 0;
            GameLoseFlag = true;
            Game_Lose();
        }
        //조금 더 구체적으로 정하기.

        if (TrainFuel <= 0)
        {
            TrainFuel = 0;
        }

        if(!GameWinFlag || !GameLoseFlag)
        {
            TrainDistance += TrainSpeed;
        }
        else
        {
            TrainSpeed = 0;
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
                //엔진칸
                TrainObject.transform.position = new Vector3(0.5f, 0, 0);
            }
            else
            {
                //나머지칸
                TrainObject.transform.position = new Vector3((1 + (10 * i)) * -1, 0, 0);
            }
            TrainObject.GetComponent<Train_InGame>().TrainNum = Trian_Num[i];
        }

        Train_Count = List_Train.childCount;
        Trains = new Train_InGame[Train_Count];
        Respawn = GameObject.FindGameObjectWithTag("Respawn");
        Respawn.transform.localScale = new Vector3(20 * Train_Count, 1, 0);
        Respawn.transform.position = new Vector3(List_Train.GetChild(Train_Count / 2).transform.position.x, -7, 0);

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
        Level();
    }
    public void Level()
    {
        MaxSpeed = TrainMaxSpeed + ((TrainMaxSpeed * (Level_MaxSpeed *10)) / 100); // 많을 수록 유리
        MaxSpeed = MaxSpeed - (TrainWeight / 100000);

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
        Total_Coin += GetCoin;
    }

    private void Game_Win()
    {
        uiDirector.Open_WIN_UI();
        SA_PlayerData.SA_GameWinReward(Total_Coin, Reward_Point);
    }

    private void Game_Lose()
    {
        uiDirector.Open_Lose_UI();
    }
}
