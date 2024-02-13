using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [Header("���� ����Ʈ")]
    public Transform List_Train;
    public Train[] Trains;
    int Train_Count;

    [Header("���� ����")]
    [SerializeField]
    int TrainFuel; // ��ü������ ���Ѵ�.
    [SerializeField]
    int TrainSpeed;
    [SerializeField]
    int TrainDistance;
    [SerializeField]
    int TrainWeight; // ��ü������ ���Ѵ�.

    [Header("���� �� ���� ���� ����")]
    public int TrainMaxSpeed;
    public int TrainEfficienl;
    public int TrainEnginePower;

    [Header("���� �� ���� ���� ����")]
    [SerializeField]
    int MaxSpeed;
    [SerializeField]
    int Efficienl;
    [SerializeField]
    int EnginePower;

    [Header("�������� ����")]
    [SerializeField]
    int Destination_Distance;

    [Header("�ð� ����")]
    // ��ġ ������ �ʿ��� ����
    [SerializeField] 
    float lastSpeedTime; //������ �ӵ� �ø� �ð�
    [SerializeField]
    float timeBet; //�ð� ����

    int Level_EngineTier; // km/h�� �����ϴ� ���� �Ŀ�
    int Level_MaxSpeed; // �߽� ���ǵ� ����
    int Level_Armor; // ������ �氨
    int Level_Efficienl; // �⸧ ȿ����

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
            Debug.Log("���� �� ����");
        }

        if (TrainSpeed <= 0)
        {
            TrainSpeed = 0;
            Debug.Log("���� �� ����");
        }

        if (TrainFuel <= 0)
        {
            TrainFuel = 0;
        }
        TrainDistance += TrainSpeed;
    }

    public void Level()
    {
        MaxSpeed = TrainMaxSpeed + ((TrainMaxSpeed * (Level_MaxSpeed *10)) / 100); // ���� ���� ����
        MaxSpeed = MaxSpeed - (TrainWeight / 100000);

        Efficienl = TrainEfficienl - ((TrainEfficienl * (Level_Efficienl * 10)) / 100); // ���� ���� ����
        EnginePower = TrainEnginePower + ((TrainEnginePower * (Level_EngineTier * 10)) / 100); // Ŭ���� ����
    }

    public int Level_ChangeArmor(int trainArmor)
    {
        //������ �氨�̱� ������ Ŭ���� ����
        return trainArmor + (trainArmor * (Level_Armor * 10) / 100);
    }

    public void Game_MonsterHit(int slow)
    {
        TrainSpeed -= slow;
    }
}
