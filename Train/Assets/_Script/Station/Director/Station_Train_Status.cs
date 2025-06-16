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
        }
    }
}
