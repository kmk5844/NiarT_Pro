using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UI_Train_Guage : MonoBehaviour
{
    int num;

    Train_InGame Train_Data;
    public Image HP_Guage;
    public Image Special_Guage;
    public GameObject ON_Object;
    Turret turret;
    Booster_Train booster;
    SelfTurret_Train self_turret;
    Dash_Train dash;
    Supply_Train supply;

    public Image LevelImage;
    public Sprite[] Level;

    float timer;

    void Start()
    {
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
    }

    void Update()
    {
        HP_Guage.fillAmount = Train_Data.HP_Parsent / 100f;
        if (Train_Data.HP_Parsent < 30f)
        {
            timer += Time.deltaTime;
            if (timer >= 0.3f)
            {
                ON_Object.SetActive(!ON_Object.activeSelf);
                timer = 0f;
            }
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
            float FuelAmout = (float)self_turret.SelfTurretTrain_Fuel / (float)self_turret.Max_SelfTurretTrain_Fuel;
            Special_Guage.fillAmount = FuelAmout;
        }
        else if(num == 5)
        {
            float FuelAmout = (float)supply.SupplyTrain_Fuel / (float)supply.Max_SupplyTrain_Fuel;
            Special_Guage.fillAmount = FuelAmout;
        }
    }
}
