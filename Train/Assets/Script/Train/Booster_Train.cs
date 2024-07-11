using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster_Train : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;
    public bool BoosterFlag;   // �ν��� ��� ��
    public bool FuelFlag;      // ���� �� ä������ �ν��� ��� ����
    public bool Corutine_BoosterFlag; //�ڷ�ƾ ���� �÷���
    float SpeedPercent;
    int WarningSpeed;   // -> %�� ����. OO% ������ ���
    public float BoosterFuel;
    [HideInInspector]
    public float Data_BoosterFuel;    // �ν��� ����� ���� �����ϴ� ���ᷮ
    int UseFuel;        // �ν��� ����� ���� ���� �Ҹ�

    int BoosterSpeedUP; // ���ᰡ �ٴڳ� ������ �ø���.

    float timebet;
    float lastTime;
    int count;


    void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector.GetComponent<GameDirector>();

        WarningSpeed = trainData.Train_WarningSpeed;
        Data_BoosterFuel = trainData.Train_BoosterFuel;
        UseFuel = trainData.Train_UseFuel;
        BoosterSpeedUP = trainData.Train_BoosterSpeedUP;
        Corutine_BoosterFlag = false;

        timebet = 0.05f;
        lastTime = Time.time;
    }

    void Update()
    {
        SpeedPercent = (gameDirector.TrainSpeed) / (gameDirector.MaxSpeed) * 100;
        if (!FuelFlag && !BoosterFlag)
        {
            if (Time.time > lastTime + timebet)
            {
                if (BoosterFuel < Data_BoosterFuel)
                {
                    if (gameDirector.TrainFuel > 0)
                    {
                        BoosterFuel += 1;
                        gameDirector.TrainFuel -= 1;
                    }
                    lastTime = Time.time;
                }
                else if (BoosterFuel >= Data_BoosterFuel)
                {
                    BoosterFuel = Data_BoosterFuel;
                    FuelFlag = true;
                    lastTime = Time.time;
                }
            }
        }
        else if (FuelFlag && !BoosterFlag)
        {
            if (WarningSpeed > SpeedPercent)
            {
                BoosterFlag = true;
                Debug.Log("�ν��� ON");
                lastTime = Time.time;
            }
        }
        else if (FuelFlag && BoosterFlag)
        {
            if(Time.time > lastTime + timebet)
            {
                if(BoosterFuel > 0)
                {
                    gameDirector.TrainSpeed += BoosterSpeedUP;
                    BoosterFuel -= UseFuel; // �Ҹ��� 2��� �Ѵ�.
                    Debug.Log("�ν��� ONNNNNNNNN");
                    lastTime = Time.time;
                }
                else
                {
                    BoosterFuel = 0;
                    FuelFlag = false;
                    BoosterFlag = false;
                    Debug.Log("�ν��� OFF");
                    lastTime = Time.time;
                }
            }
        }
    }
}
