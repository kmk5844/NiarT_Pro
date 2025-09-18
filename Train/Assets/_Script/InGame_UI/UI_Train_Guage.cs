using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UI_Train_Guage : MonoBehaviour
{
    int num;

    // 0: None, 1: Medic, 2: Turret, 3: Booster, 4: Self_Turret, 5: Supply, 6: Repair, 7: TurretFactory, 8: DronFactory, 9: FuelSignal, 10: Hangar, 11: IronPlateFactory, 12: TurretUpgrade
    Train_InGame Train_Data;
    public Image HP_Guage;
    public Image Special_Guage;
    public GameObject ON_Object;

    Turret turret;
    Booster_Train booster;
    SelfTurret_Train self_turret;
    Supply_Train supply;
    Repair_Train repair;
    TurretFactory_Train turretFactory;
    DronFactoryTrain dronFactory;
    FuelSignalTrain fuelSignal;
    Hangar_Train hangar;
    IronPlateFactory ironPlateFactory;
    TurretUpgradeTrain turretUpgrade;

    float timer;

    void Start()
    {
        GetComponent<Canvas>().sortingLayerName = "Train";
        Train_Data = GetComponentInParent<Train_InGame>();
        
        num = 0;
        if (Train_Data.Train_Type.Equals("Medic"))
        {
            num = 1;
        }
        if (Train_Data.Train_Type.Equals("Turret"))
        {
            num = 2;
            turret = Train_Data.GetComponentInChildren<Turret>();
        }
        if (Train_Data.Train_Type.Equals("Booster"))
        {
            num = 3;
            booster = Train_Data.GetComponentInChildren<Booster_Train>();
        }
        if (Train_Data.Train_Type.Equals("Self_Turret"))
        {
            num = 4;
            self_turret = Train_Data.GetComponentInChildren<SelfTurret_Train>();
        }
        if (Train_Data.Train_Type.Equals("Supply"))
        {
            num = 5;
            supply = Train_Data.GetComponentInChildren<Supply_Train>();
        }
        if(Train_Data.Train_Type.Equals("Repair"))
        {
            num = 6;
            repair = Train_Data.GetComponentInChildren<Repair_Train>();
        }
        if (Train_Data.Train_Type.Equals("TurretFactory"))
        {
            num = 7;
            turretFactory = Train_Data.GetComponentInChildren<TurretFactory_Train>();
        }
        if (Train_Data.Train_Type.Equals("DronFactory"))
        {
            num = 8;
            dronFactory = Train_Data.GetComponentInChildren<DronFactoryTrain>();
        }
        if(Train_Data.Train_Type.Equals("FuelSignal"))
        {
            num = 9;
            fuelSignal = Train_Data.GetComponentInChildren<FuelSignalTrain>();
        }
        if (Train_Data.Train_Type.Equals("Hangar"))
        {
            num = 10;
            hangar = Train_Data.GetComponentInChildren<Hangar_Train>();
        }
        if (Train_Data.Train_Type.Equals("IronPlateFactory"))
        {
            num = 11;
            ironPlateFactory = Train_Data.GetComponentInChildren<IronPlateFactory>();

        }
        if(Train_Data.Train_Type.Equals("TurretUpgrade"))
        {
            num = 12;
            turretUpgrade = Train_Data.GetComponentInChildren<TurretUpgradeTrain>();
        }


        int level = Train_Data.CheckLevel();
    }

    void Update()
    {
        HP_Guage.fillAmount = Train_Data.HP_Parsent / 100f;
        if (!Train_Data.isCooltimeCheckFlag)
        {
            ON_Object.SetActive(true);
        }
        else
        {
            ON_Object.SetActive(false);
        }

        Check_fillAmount();
    }

    void Check_fillAmount()
    {
        if(num == 0)
        {
            Special_Guage.fillAmount = 0f;
        }
        else if(num == 1)
        {
            float HealAmout = Train_Data.Train_Heal / Train_Data.Max_Train_Heal;
            Special_Guage.fillAmount = HealAmout;
        }
        else if(num == 2)
        {
            Special_Guage.fillAmount = turret.Bullet_Delay_Percent();
        }
        else if(num == 3)
        {
            float FuelAmout = booster.BoosterFuel / booster.Data_BoosterFuel;
            Special_Guage.fillAmount = FuelAmout;
        }else if(num == 4)
        {
            if (!self_turret.isAtacking)
            {
                float FuelAmout = (float)self_turret.SelfTurretTrain_Fuel / (float)self_turret.Max_SelfTurretTrain_Fuel;
                Special_Guage.fillAmount = FuelAmout;
            }
            else
            {
                Special_Guage.fillAmount = self_turret.fillAmount_Time;
            }
        }
        else if(num == 5)
        {
            float FuelAmout = (float)supply.SupplyTrain_Fuel / (float)supply.Max_SupplyTrain_Fuel;
            Special_Guage.fillAmount = FuelAmout;
        }else if(num == 6)
        {
            float TimeAmout = (float)repair.elapsed / (float)repair.SpawnTime;
            Special_Guage.fillAmount = TimeAmout;
        }else if(num == 7)
        {
            /*float TimeAmout = (float)turretFactory.elasped / (float)turretFactory.SpawnTime;
            Special_Guage.fillAmount = TimeAmout;*/

            int elapsedSeconds = Mathf.FloorToInt(turretFactory.elasped);
            float spawnSeconds = (float)Mathf.FloorToInt(turretFactory.SpawnTime);  // float으로 변환

            Special_Guage.fillAmount = (elapsedSeconds % spawnSeconds) / spawnSeconds;
            /*            float TimeAmount = turretFactory.elasped / turretFactory.SpawnTime;
                        Special_Guage.fillAmount = Mathf.Clamp01(TimeAmount);*/
        }
        else if(num == 8)
        {
            int elapsedSeconds = Mathf.FloorToInt(dronFactory.elapsed);
            float spawnSeconds = (float)Mathf.FloorToInt(dronFactory.SpawnTime);  // float으로 변환
            Special_Guage.fillAmount = (elapsedSeconds % spawnSeconds) / spawnSeconds;
            /*
                        Special_Guage.fillAmount = (elapsedSeconds % spawnSeconds) / spawnSeconds;
                        float TimeAmout = (float)dronFactory.elapsed / (float)dronFactory.SpawnTime;*/
        }
        else if(num == 9)
        {
            int elapsedSeconds = Mathf.FloorToInt(fuelSignal.elapsed);
            float spawnSeconds = (float)Mathf.FloorToInt(fuelSignal.SpawnTime);

            // UI 게이지: 부드럽게 0~1 범위로
            Special_Guage.fillAmount = Mathf.Clamp01(elapsedSeconds / spawnSeconds);

        }else if(num == 10)
        {
            int elapsedSeconds = Mathf.FloorToInt(hangar.elapsed);
            float spawnSeconds = (float)Mathf.FloorToInt(hangar.SpawnTime);  // float으로 변환
            Special_Guage.fillAmount = Mathf.Clamp01(elapsedSeconds / spawnSeconds);
            /*            float TimeAmout = (float)hangar.elapsed / (float)hangar.SpawnTime;
                        Special_Guage.fillAmount = TimeAmout;*/
        }
        else if(num == 11)
        {
            int elapsedSeconds = Mathf.FloorToInt(ironPlateFactory.elapsed);
            float spawnSeconds = (float)Mathf.FloorToInt(ironPlateFactory.SpawnTime);  // float으로 변환
            Special_Guage.fillAmount = Mathf.Clamp01(elapsedSeconds / spawnSeconds);
           /* float TimeAmout = (float)ironPlateFactory.elapsed / (float)ironPlateFactory.SpawnTime;
            Special_Guage.fillAmount = TimeAmout;*/
        }else if(num == 12)
        {
            int elapsedSeconds = Mathf.FloorToInt(turretUpgrade.elapsed);
            float spawnSeconds = (float)Mathf.FloorToInt(turretUpgrade.SpawnTime);  // float으로 변환
            Special_Guage.fillAmount = Mathf.Clamp01(elapsedSeconds / spawnSeconds);
            /* float TimeAmout = (float)turretUpgrade.elapsed / (float)turretUpgrade.SpawnTime;
             Special_Guage.fillAmount = TimeAmout;*/
        }
    }
}
