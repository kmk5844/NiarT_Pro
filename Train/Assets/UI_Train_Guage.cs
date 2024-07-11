using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UI_Train_Guage : MonoBehaviour
{
    Train_InGame Train_Data;
    public Image HP_Guage;
    public Image Special_Guage;
    public GameObject ON_Object;
    Turret turret;
    Booster_Train booster;

    float timer;

    void Start()
    {
        Train_Data = GetComponentInParent<Train_InGame>();
        if (Train_Data.Train_Type.Equals("Turret"))
        {
            turret = Train_Data.GetComponentInChildren<Turret>();
        }
        if (Train_Data.Train_Type.Equals("Booster"))
        {
            booster = Train_Data.GetComponentInChildren<Booster_Train>();
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


        if (Train_Data.Train_Type.Equals("Medic"))
        {
            float HealAmout = Train_Data.Train_Heal / Train_Data.Max_Train_Heal;
            Special_Guage.fillAmount = HealAmout;
        }
        else if (Train_Data.Train_Type.Equals("Turret"))
        {
            Special_Guage.fillAmount = turret.Bullet_Delay_Percent();
        }
        else if (Train_Data.Train_Type.Equals("Booster"))
        {
            float FuelAmout = booster.BoosterFuel / booster.Data_BoosterFuel;
            Special_Guage.fillAmount = FuelAmout;
        }
        else
        {
            Special_Guage.fillAmount = 0f;
        }
    }
}
