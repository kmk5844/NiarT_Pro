using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster_Train : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;
    public bool BoosterFlag;   // 부스터 사용 중
    public bool FuelFlag;      // 연료 다 채워지면 부스터 사용 가능
    public bool Corutine_BoosterFlag; //코루틴 전용 플래그
    float SpeedPercent;
    int WarningSpeed;   // -> %로 따짐. OO% 이하인 경우
    public float BoosterFuel;
    [HideInInspector]
    public float Data_BoosterFuel;    // 부스터 사용을 위한 저장하는 연료량
    int UseFuel;        // 부스터 사용을 위한 연료 소모량

    int BoosterSpeedUP; // 연료가 바닥날 때까지 올린다.

    float timebet;
    float lastTime;

    public GameObject effectObject;

    void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector;

        WarningSpeed = int.Parse(trainData.trainData_Special_String[0]);
        Data_BoosterFuel = int.Parse(trainData.trainData_Special_String[1]);
        UseFuel = int.Parse(trainData.trainData_Special_String[2]);
        BoosterSpeedUP = int.Parse(trainData.trainData_Special_String[3]);
        Corutine_BoosterFlag = false;
        effectObject.SetActive(false);

        timebet = 0.05f;
        lastTime = Time.time;
    }

    void Update()
    {
        SpeedPercent = (gameDirector.TrainSpeed) / (gameDirector.MaxSpeed) * 100;
        if(gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss || gameDirector.gameType == GameType.Refreshing)
        {
            if (!trainData.DestoryFlag)
            {
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
                        lastTime = Time.time;
                    }
                }
                else if (FuelFlag && BoosterFlag)
                {
                    if (Time.time > lastTime + timebet)
                    {
                        if (BoosterFuel > 0)
                        {
                            gameDirector.TrainSpeed += BoosterSpeedUP;
                            BoosterFuel -= UseFuel; // 소모량은 2배로 한다.
                            lastTime = Time.time;
                            effectObject.SetActive(true);
                        }
                        else
                        {
                            BoosterFuel = 0;
                            FuelFlag = false;
                            BoosterFlag = false;
                            lastTime = Time.time;
                            effectObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
