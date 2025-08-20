using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Station_Train_Status : MonoBehaviour
{
    public CommonStatus common;
    public EngineStatus engine;
    public FuelStatus fuel;
    public MedicStatus medic;
    public Self_TurretStatus self;
    public SupplyStatus supply;
    public BoosterStatus booster;
    public TurretStatus turret;
    public RepairStatus repair;
    public TurretFactoryStatus turretFactory;
    public DronFactoryStatus dronFactory;
    public FuelSignalStatus fuelSignal;
    public HangarStatus hangar;
    public IronPlateFactoryStatus ironFactory;
    public TurretUpgradeStatus turretUpgrade;

    [Serializable]
    public struct CommonStatus
    {
        public GameObject CommonObj;
        public TextMeshProUGUI HP_Text;
        public TextMeshProUGUI Weight_Text;
        public TextMeshProUGUI Armor_Text;
    }

    [Serializable]
    public struct EngineStatus
    {
        public GameObject EngineObj;
        public TextMeshProUGUI MaxSpeed_Text;
        public TextMeshProUGUI Efficient_Text;
        public TextMeshProUGUI EnginePower_Text;
    }

    [Serializable]
    public struct FuelStatus
    {
        public GameObject FuelObj;
        public TextMeshProUGUI Fuel_Text;
    }

    [Serializable]
    public struct MedicStatus
    {
        public GameObject MedicObj;
        public TextMeshProUGUI TotalAmout_Text;
        public TextMeshProUGUI Healamout_Text;
        public TextMeshProUGUI HealSpeed_Text;
    }

    [Serializable]
    public struct Self_TurretStatus
    {
        public GameObject SelfObj;
        public TextMeshProUGUI FuelAmount_Text;
        public TextMeshProUGUI Atk_Text;
        public TextMeshProUGUI AtkDelay_Text;
        public TextMeshProUGUI UseTime_Text;
    }

    [Serializable]
    public struct SupplyStatus
    { 
        public GameObject SupplyObj;
        public TextMeshProUGUI FuelAmount_Text;
        public TextMeshProUGUI Supply_Level_Text;
        public TextMeshProUGUI Supply_Count_Text;
    }
    [Serializable]
    public struct BoosterStatus
    {
        public GameObject BoosterObj;
        public TextMeshProUGUI WarningSpeed_Text;
        public TextMeshProUGUI BoosterFuel_Text;
        public TextMeshProUGUI UseFuel_Text;
        public TextMeshProUGUI SpeedUp_Text;
    }

    [Serializable]
    public struct TurretStatus
    {
        public GameObject TurrteObj;
        public TextMeshProUGUI Atk_Text;
        public TextMeshProUGUI AtkDelay__Text;
    }

    [Serializable]
    public struct RepairStatus
    {
        public GameObject RepairObj;
        public TextMeshProUGUI Repair_CoolTime_Text;
        public TextMeshProUGUI Repair_Heal_Text;
        public TextMeshProUGUI Repair_Heal_Count_Text;
    }

    [Serializable]
    public struct TurretFactoryStatus
    {
        public GameObject TurretFactoryObj;
        public TextMeshProUGUI TurretFactory_CoolTime_Text;
        public TextMeshProUGUI TurretFactory_Count_Text;
    }

    [Serializable]
    public struct DronFactoryStatus
    {
        public GameObject DronFactoryObj;
        public TextMeshProUGUI DronFactory_CoolTime_Text;
        public TextMeshProUGUI DronFactory_MinHP_Text;
        public TextMeshProUGUI DronFactory_MaxHP_Text;
    }

    [Serializable]
    public struct FuelSignalStatus
    {
        public GameObject FuelSignalObj;
        public TextMeshProUGUI FuelSignal_CoolTime_Text;
        public TextMeshProUGUI FuelSignal_MinFuel_Text;
        public TextMeshProUGUI FuelSignal_MaxFuel_Text;
    }

    [Serializable]
    public struct HangarStatus
    {
        public GameObject HangarObj;
        public TextMeshProUGUI Hangar_CoolTime_Text;
        public TextMeshProUGUI Hangar_Shelter1_Text;
        public TextMeshProUGUI Hangar_Shelter2_Text;
        public TextMeshProUGUI Hangar_Shelter3_Text;
    }

    [Serializable]
    public struct IronPlateFactoryStatus
    {
        public GameObject IronFactoryObj;
        public TextMeshProUGUI IronFactory_CoolTime_Text;
        public TextMeshProUGUI IronFactory_HP_Text; // 200
    }

    [Serializable]
    public struct TurretUpgradeStatus
    {
        public GameObject TurretUpgradeObj;
        public TextMeshProUGUI TurretUpgrade_CoolTime_Text;
        public TextMeshProUGUI TurretUpgrade_Persent_Text;
        public TextMeshProUGUI TurretUpgrade_DelayTime_Text;
    }

    public void Setting_TrainCommon(int hp, int weight, int armor)
    {
        if (!common.CommonObj.activeSelf)
        {
            common.CommonObj.SetActive(true);
        }
        common.HP_Text.text = hp.ToString();
        common.Weight_Text.text = weight.ToString();
        common.Armor_Text.text = armor.ToString();
    }

    public void Setting_TrainStatus(string Type, string[] status)
    {
        if (engine.EngineObj.activeSelf)
        {
            engine.EngineObj.SetActive(false);
        }
        if (fuel.FuelObj.activeSelf)
        {
            fuel.FuelObj.SetActive(false);
        }
        if (medic.MedicObj.activeSelf)
        {
            medic.MedicObj.SetActive(false);
        }
        if (self.SelfObj.activeSelf)
        {
            self.SelfObj.SetActive(false);
        }
        if (supply.SupplyObj.activeSelf)
        {
            supply.SupplyObj.SetActive(false);  
        }
        if (booster.BoosterObj.activeSelf)
        {
            booster.BoosterObj.SetActive(false);
        }
        if (turret.TurrteObj.activeSelf)
        {
            turret.TurrteObj.SetActive(false);
        }
        if (repair.RepairObj.activeSelf)
        {
            repair.RepairObj.SetActive(false);
        }
        if (turretFactory.TurretFactoryObj.activeSelf)
        {
            turretFactory.TurretFactoryObj.SetActive(false);
        }
        if (dronFactory.DronFactoryObj.activeSelf)
        {
            dronFactory.DronFactoryObj.SetActive(false);
        }
        if (fuelSignal.FuelSignalObj.activeSelf)
        {
            fuelSignal.FuelSignalObj.SetActive(false);
        }
        if (hangar.HangarObj.activeSelf)
        {
            hangar.HangarObj.SetActive(false);
        }
        if (ironFactory.IronFactoryObj.activeSelf)
        {
            ironFactory.IronFactoryObj.SetActive(false);
        }
        if (turretUpgrade.TurretUpgradeObj.activeSelf)
        {
            turretUpgrade.TurretUpgradeObj.SetActive(false);
        }


        switch (Type)
        {
            case "Engine":
                engine.EngineObj.SetActive(true);
                engine.MaxSpeed_Text.text = status[0];
                engine.Efficient_Text.text = status[1];
                engine.EnginePower_Text.text = status[2];
                break;
            case "Fuel":
                fuel.FuelObj.SetActive(true);       
                fuel.Fuel_Text.text = status[0];
                break;
            case "Medic":
                medic.MedicObj.SetActive(true);
                medic.TotalAmout_Text.text = status[0];
                medic.Healamout_Text.text = status[1];
                medic.HealSpeed_Text.text = status[2];
                break;
            case "Self_Turret":
                self.SelfObj.SetActive(true);
                self.FuelAmount_Text.text = status[0];
                self.Atk_Text.text = status[1];
                self.AtkDelay_Text.text = status[2];
                self.UseTime_Text.text = status[3];
                break;
            case "Supply":
                supply.SupplyObj.SetActive(true);  
                supply.FuelAmount_Text.text   = status[0];
                supply.Supply_Level_Text.text = status[1];
                supply.Supply_Count_Text.text = status[2];
                break;
            case "Booster":
                booster.BoosterObj.SetActive(true);
                booster.WarningSpeed_Text.text = status[0];
                booster.BoosterFuel_Text.text = status[1];
                booster.UseFuel_Text.text   = status[2];
                booster.SpeedUp_Text.text = status[3];   
                break;
            case "Turret":
                turret.TurrteObj.SetActive(true);
                turret.Atk_Text.text= status[0];
                turret.AtkDelay__Text.text = status[1];
                break;
            case "Repair":
                repair.RepairObj.SetActive(true);
                repair.Repair_CoolTime_Text.text = status[0];
                repair.Repair_Heal_Text.text = status[1];
                repair.Repair_Heal_Count_Text.text = status[2];
                break;
            case "TurretFactory":
                turretFactory.TurretFactoryObj.SetActive(true);
                turretFactory.TurretFactory_CoolTime_Text.text = status[0];
                turretFactory.TurretFactory_Count_Text.text = status[1];
                break;
            case "DronFactory":
                dronFactory.DronFactoryObj.SetActive(true);
                dronFactory.DronFactory_CoolTime_Text.text = status[0];
                dronFactory.DronFactory_MinHP_Text.text = (int.Parse(status[1]) * 100).ToString();
                dronFactory.DronFactory_MaxHP_Text.text = (int.Parse(status[2]) * 100).ToString();
                break;
            case "FuelSignal":
                fuelSignal.FuelSignalObj.SetActive(true);
                fuelSignal.FuelSignal_CoolTime_Text.text = status[0];
                fuelSignal.FuelSignal_MinFuel_Text.text = status[1];
                fuelSignal.FuelSignal_MaxFuel_Text.text = status[2];
                break;
            case "Hangar":
                hangar.HangarObj.SetActive(true);
                hangar.Hangar_CoolTime_Text.text = status[0];
                hangar.Hangar_Shelter1_Text.text = status[1];
                hangar.Hangar_Shelter2_Text.text = status[2];
                hangar.Hangar_Shelter3_Text.text = status[3];
                break;
            case "IronPlateFactory":
                ironFactory.IronFactoryObj.SetActive(true);
                ironFactory.IronFactory_CoolTime_Text.text = status[0];
                ironFactory.IronFactory_HP_Text.text = "200";
                break;
            case "TurretUpgrade":
                turretUpgrade.TurretUpgradeObj.SetActive(true);
                turretUpgrade.TurretUpgrade_CoolTime_Text.text = status[0];
                turretUpgrade.TurretUpgrade_Persent_Text.text = status[1];
                turretUpgrade.TurretUpgrade_DelayTime_Text.text = status[2];
                break;
        }
    }
}
