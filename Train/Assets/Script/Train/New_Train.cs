using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class New_Train : MonoBehaviour
{
    [Header("공통")]
    public Game_DataTable trainData;
    [SerializeField]
    int Train_Num;
    [SerializeField]
    int Train_Num2;
    bool TurretFlag;
    bool BoosterFlag;
    public string Train_Name;
    public int Train_HP;
    public int Train_Weight;
    public int Train_Armor;
    public string Train_Type;
    int era;

    [Header("엔진")]
    public int Train_MaxSpeed;
    public int Train_Efficient;
    public int Train_Engine_Power;

    [Header("연료")]
    
    public int Train_Fuel;

    [Header("포탑")]
    public int Train_Attack;
    public float Train_Attack_Delay;

    [Header("의무실")]
    public int Train_Heal;

    private void Awake()
    {
        TurretFlag = false;
        BoosterFlag = false;
        era = 100;
        string[] name = gameObject.name.Split('/');
        if (name[0].Equals("51"))
        {
            TurretFlag = true;
            Train_Num = int.Parse(name[0]);
            Train_Num2 = int.Parse(name[1]);
        }else if (name[0].Equals("52"))
        {
            BoosterFlag = true;
            Train_Num = int.Parse(name[0]);
            Train_Num2 = int.Parse(name[1]);
        }
        else
        {
            Train_Num = int.Parse(gameObject.name);
        }

        if (TurretFlag)
        {
            Train_Name = trainData.Information_Train_Turret_Part[Train_Num2].Turret_Part_Name;
            Train_HP = trainData.Information_Train_Turret_Part[Train_Num2].Train_HP;
            Train_Weight = trainData.Information_Train_Turret_Part[Train_Num2].Train_Weight;
            Train_Armor = trainData.Information_Train_Turret_Part[Train_Num2].Train_Armor;
        }
        else if(BoosterFlag)
        {
            Train_Name = trainData.Information_Train_Booster_Part[Train_Num2].Booster_Part_Name;
            Train_HP = trainData.Information_Train_Booster_Part[Train_Num2].Train_HP;
            Train_Weight = trainData.Information_Train_Booster_Part[Train_Num2].Train_Weight;
            Train_Armor = trainData.Information_Train_Booster_Part[Train_Num2].Train_Armor;
        }
        else
        {
            Train_Name = trainData.Information_Train[Train_Num].Train_Name;
            Train_HP = trainData.Information_Train[Train_Num].Train_HP;
            Train_Weight = trainData.Information_Train[Train_Num].Train_Weight;
            Train_Armor = trainData.Information_Train[Train_Num].Train_Armor;
        }
        Train_Type = trainData.Information_Train[Train_Num].Train_Type;
        CheckType();
    }

    private void CheckType()
    {
        switch (Train_Type)
        {
            case "Engine":
                Train_MaxSpeed = trainData.Information_Train[Train_Num].Train_MaxSpeed;
                Train_Efficient = trainData.Information_Train[Train_Num].Train_Efficient;
                Train_Engine_Power = trainData.Information_Train[Train_Num].Train_Engine_Power;
                Train_Fuel = 0;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                Train_Heal = 0;
                break;
            case "Fuel":
                Train_MaxSpeed = 0;
                Train_Efficient = 0;
                Train_Engine_Power = 0;
                Train_Fuel = trainData.Information_Train[Train_Num].Train_Fuel;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                Train_Heal = 0;
                break;
            case "Turret":
                Train_MaxSpeed = 0;
                Train_Efficient = 0;
                Train_Engine_Power = 0;
                Train_Fuel = 0;
                Train_Attack = trainData.Information_Train_Turret_Part[Train_Num2].Train_Attack;
                Train_Attack_Delay = trainData.Information_Train_Turret_Part[Train_Num2].Train_Attack_Delay;
                Train_Heal = 0;
                break;
            case "Medic":
                Train_MaxSpeed = 0;
                Train_Efficient = 0;
                Train_Engine_Power = 0;
                Train_Fuel = 0;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                Train_Heal = trainData.Information_Train[Train_Num].Train_Heal;
                break;
        }
    }
}
