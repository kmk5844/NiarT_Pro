using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [Header("������ ����")]
    public SA_TrainData SA_TrainData;
    public SA_PlayerData SA_PlayerData;
    public Game_DataTable EX_GameData;
    public GameObject MonsterDirector;
    List<int> Trian_Num;

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
        timeBet = 0.1f - (EnginePower * 0.001f); //���� �Ŀ��� ���� ����
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
        //���� �� ��ü������ ���ϱ�.

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
                //����ĭ
                TrainObject.transform.position = new Vector3(0.5f, 0, 0);
            }
            else
            {
                //������ĭ
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
