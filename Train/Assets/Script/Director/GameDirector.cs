using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [Header("기차 리스트")]
    public Transform List_Train;
    public Train[] Trains;
    int Train_Count;

    [Header("기차 정보")]
    [SerializeField]
    int TrainFuel; // 전체적으로 더한다.
    [SerializeField]
    int TrainSpeed;
    [SerializeField]
    int TrainDistance;
    [SerializeField]
    int TrainWeight; // 전체적으로 더한다.

    [Header("레벨 업 적용 전의 기차")]
    public int TrainMaxSpeed;
    public int TrainEfficienl;
    public int TrainEnginePower;

    [Header("레벨 업 적용 후의 기차")]
    [SerializeField]
    int MaxSpeed;
    [SerializeField]
    int Efficienl;
    [SerializeField]
    int EnginePower;

    [Header("스테이지 정보")]
    [SerializeField]
    int Destination_Distance;

    [Header("시간 정보")]
    // 수치 결정이 필요한 구간
    [SerializeField] 
    float lastSpeedTime; //마지막 속도 올린 시간
    [SerializeField]
    float timeBet; //시간 차이

    int Level_EngineTier; // km/h을 증가하는 엔진 파워
    int Level_MaxSpeed; // 멕스 스피드 조절
    int Level_Armor; // 데미지 경감
    int Level_Efficienl; // 기름 효율성

    void Start()
    {
        Train_Count = List_Train.childCount;
        Trains = new Train[Train_Count];
        for (int i = 0; i < Train_Count; i++)
        {
            Trains[i] = List_Train.GetChild(i).gameObject.GetComponent<Train>();
            TrainFuel += Trains[i].Train_Fuel;
            TrainWeight += Trains[i].Train_Weight;
            TrainMaxSpeed += Trains[i].Train_MaxSpeed;
            TrainEfficienl += Trains[i].Train_Efficienl;
            TrainEnginePower += Trains[i].Train_Engine_Power;
        }

        Level_Train Level_Data = GetComponent<Level_Train>();
        Level_EngineTier = Level_Data.Level_Train_EngineTier;
        Level_MaxSpeed = Level_Data.Level_Train_MaxSpeed;
        Level_Armor = Level_Data.Level_Train_Armor;
        Level_Efficienl = Level_Data.Level_Train_Efficienl;
        Level();

        lastSpeedTime = 0;
        timeBet = 0.1f - (EnginePower * 0.001f);
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
                    TrainFuel -= Efficienl;
                }
            }
            else
            {
                if (TrainFuel > 0)
                {
                    TrainFuel -= Efficienl;
                }
            }
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

        Efficienl = TrainEfficienl - ((TrainEfficienl * (Level_Efficienl * 10)) / 100); // 적을 수록 유리
        EnginePower = TrainEnginePower + ((TrainEnginePower * (Level_EngineTier * 10)) / 100); // 클수록 유리
    }

    public int Level_ChangeArmor(int trainArmor)
    {
        //데미지 경감이기 때문에 클수록 유리
        return trainArmor + (trainArmor * (Level_Armor * 10) / 100);
    }

    public void Game_MonsterHit(int slow)
    {
        TrainSpeed -= slow;
    }
}
