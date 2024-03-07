using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [Header("생성할 기차")]
    public SA_TrainData SA_TrainData;
    public Game_DataTable EX_TrainData;
    List<int> Trian_Num;

    [Header("기차 리스트")]
    public Transform List_Train;
    public Train_InGame[] Trains;
    int Train_Count;

    [Header("기차 정보")]
    [SerializeField]
    int TrainFuel; // 전체적으로 더한다.
    [SerializeField]
    int TrainSpeed;
    [SerializeField]
    int TrainDistance;
    [SerializeField]
    int TrainWeight;// 전체적으로 더한다.
    [SerializeField]
    int TrainFood;  // 전체적으로 더한다.

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

    [Header("스테이지 정보")]
    [SerializeField]
    int Destination_Distance;

    [Header("시간 정보")]
    [SerializeField] 
    float lastSpeedTime; //마지막 속도 올린 시간
    [SerializeField]
    float timeBet; //시간 차이

    GameObject Respawn;

    int Level_EngineTier; // km/h을 증가하는 엔진 파워
    int Level_MaxSpeed; // 멕스 스피드 조절
    int Level_Efficient; // 기름 효율성

    void Start()
    {
        Trian_Num = SA_TrainData.Train_Num;
        for (int i = 0; i < Trian_Num.Count; i++)
        {
            GameObject TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_InGame/" + Trian_Num[i]), List_Train);
            TrainObject.name = EX_TrainData.Information_Train[Trian_Num[i]].Train_Name;
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
        Respawn.transform.position = new Vector3(List_Train.GetChild(Train_Count/2).transform.position.x, -7, 0);

        for (int i = 0; i < Train_Count; i++)
        {
            Trains[i] = List_Train.GetChild(i).gameObject.GetComponent<Train_InGame>();
            TrainFuel += Trains[i].Train_Fuel;
            TrainWeight += Trains[i].Train_Weight;
            TrainMaxSpeed += Trains[i].Train_MaxSpeed;
            TrainEfficient += Trains[i].Train_Efficient;
            TrainEnginePower += Trains[i].Train_Engine_Power;
            TrainFood += Trains[i].Train_Food; //식량기차
        }

        Level_EngineTier = SA_TrainData.Level_Train_EngineTier;
        Level_MaxSpeed = SA_TrainData.Level_Train_MaxSpeed;
        Level_Efficient = SA_TrainData.Level_Train_Efficient;
        Level();

        lastSpeedTime = 0;
        timeBet = 0.1f - (EnginePower * 0.001f); //엔진 파워에 따라 결정
    }

    void Update()
    {
        if(Time.time >= lastSpeedTime + timeBet)
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
            TrainFood -= 1;
            lastSpeedTime = Time.time;
        }

        if(Destination_Distance < TrainDistance )
        {
            Debug.Log("성공 및 종료");
        }

        if (TrainSpeed <= 0)
        {
            TrainSpeed = 0;
            Debug.Log("실패 및 종료");
        }
        //조금 더 구체적으로 정하기.

        if (TrainFuel <= 0)
        {
            TrainFuel = 0;
        }
        TrainDistance += TrainSpeed;
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
}
