using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supply_Train : MonoBehaviour
{
    Train_InGame SupplyTrain;
    GameDirector gameDirector;

    public int SupplyTrain_Fuel;
    public int Max_SupplyTrain_Fuel;

    bool FuelFlag;
    public bool UseFlag;

    float timebet;
    float lastTime;

    int supply_grade;
    int supply_count;

    public GameObject Dron;

    private void Start()
    {
        SupplyTrain = GetComponentInParent<Train_InGame>();
        gameDirector = SupplyTrain.gameDirector;

        SupplyTrain_Fuel = 0;
        Max_SupplyTrain_Fuel = int.Parse(SupplyTrain.trainData_Special_String[0]);
        supply_grade = int.Parse(SupplyTrain.trainData_Special_String[1]);
        supply_count = int.Parse(SupplyTrain.trainData_Special_String[2]);

        FuelFlag = false;
        UseFlag = false;

        timebet = 0.05f;
        lastTime = Time.time;
    }

    private void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if (!SupplyTrain.DestoryFlag)
            {
                if (!FuelFlag)
                {
                    if (Time.time > lastTime + timebet)
                    {
                        if (SupplyTrain_Fuel < Max_SupplyTrain_Fuel)
                        {
                            if (gameDirector.TrainFuel > 0)
                            {
                                SupplyTrain_Fuel += 1;
                                gameDirector.TrainFuel -= 1;
                            }
                            lastTime = Time.time;
                        }
                        else if (SupplyTrain_Fuel >= Max_SupplyTrain_Fuel)
                        {
                            SupplyTrain_Fuel = Max_SupplyTrain_Fuel;
                            FuelFlag = true;
                            UseFlag = true;
                            lastTime = Time.time;
                        }
                    }
                }
            }
        }
    }

    public void UseSupply()
    {
        FuelFlag = false;
        UseFlag = false;
        Dron.GetComponentInChildren<Supply_Train_Dron>().SupplyDron_SetData(supply_grade, supply_count);
        Instantiate(Dron);
        SupplyTrain_Fuel = 0;
        lastTime = Time.time;
    }
}