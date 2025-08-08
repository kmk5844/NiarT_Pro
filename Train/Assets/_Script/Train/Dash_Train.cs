using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Train : MonoBehaviour
{
    Train_InGame DashTrain;
    GameDirector gameDirector;

    public int DashTrain_Fuel;
    [HideInInspector]
    public int Max_DashTrain_Fuel;

    bool FuelFlag;
    public bool UseFlag;

    float timebet;
    float lastTime;

    private void Start()
    {
        DashTrain = GetComponentInParent<Train_InGame>();
        gameDirector = DashTrain.gameDirector;

        DashTrain_Fuel = 0;
        //Max_DashTrain_Fuel = DashTrain.Train_Dash_UseFuel;

        FuelFlag = false;
        UseFlag = false;

        timebet = 0.05f;
        lastTime = Time.time;
    }

    private void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if (!FuelFlag)
            {
                if(Time.time > lastTime + timebet)
                {
                    if(DashTrain_Fuel < Max_DashTrain_Fuel)
                    {
                        if(gameDirector.TrainFuel > 0)
                        {
                            DashTrain_Fuel += 1;
                            gameDirector.TrainFuel -= 1;
                        }
                        lastTime = Time.time;
                    }
                    else if (DashTrain_Fuel >= Max_DashTrain_Fuel)
                    {
                        DashTrain_Fuel = Max_DashTrain_Fuel;
                        FuelFlag = true;
                        UseFlag = true;
                        lastTime = Time.time;
                    }
                }
            }
        }
    }

    public void UseDash()
    {
        FuelFlag = false;
        UseFlag = false;

        DashTrain_Fuel = 0;
        lastTime = Time.time;
    }
}
