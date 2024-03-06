using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Station_TrainMaintenance : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Player_DataObject;
    Station_PlayerData playerData;
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Mercenary_DataObject;
    Station_MercenaryData mercenaryData;

    [Header("패시브 업그레이드 윈도우")]
    public TextMeshProUGUI Passive_Text_0;
    public Button Passive_Button_0;
    public TextMeshProUGUI Passive_Text_1;   
    public Button Passive_Button_1;
    public TextMeshProUGUI Passive_Text_2;
    public Button Passive_Button_2;
    public TextMeshProUGUI Passive_Text_3;
    public Button Passive_Button_3;

    private void Start()
    {
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        Passive_Text(true);
    }

    private void Passive_Text(bool All, int num = 0)
    {
        if (All)
        {
            if (trainData.Level_Train_EngineTier == trainData.Max_Train_EngineTier)
            {
                Passive_Text_0.text = "Lv.MAX\n0G";
                Passive_Button_0.interactable = false;
            }
            else
            {
                Passive_Text_0.text = "Lv." + trainData.Level_Train_EngineTier + "\n" + trainData.Cost_Train_EngineTier + "G";
            }
            if (trainData.Level_Train_MaxSpeed == trainData.Max_Train_MaxSpeed)
            {
                Passive_Text_1.text = "Lv.MAX\n0G";
                Passive_Button_1.interactable = false;
            }
            else
            {
                Passive_Text_1.text = "Lv." + trainData.Level_Train_MaxSpeed + "\n" + trainData.Cost_Train_MaxSpeed + "G";
            }
            if (trainData.Level_Train_Armor == trainData.Max_Train_Armor)
            {
                Passive_Text_2.text = "Lv.MAX\n0G";
                Passive_Button_2.interactable = false;
            }
            else
            {
                Passive_Text_2.text = "Lv." + trainData.Level_Train_Armor + "\n" + trainData.Cost_Train_Armor + "G";
            }
            if (trainData.Level_Train_Efficient == trainData.Max_Train_Efficient)
            {
                Passive_Text_3.text = "Lv.MAX\n0G";
                Passive_Button_3.interactable = false;
            }
            else
            {
                Passive_Text_3.text = "Lv." + trainData.Level_Train_Efficient + "\n" + trainData.Cost_Train_Efficient + "G";
            }
        }
        else
        {
            if (num == 0)
            {
                if (trainData.Level_Train_EngineTier == trainData.Max_Train_EngineTier)
                {
                    Passive_Text_0.text = "Lv.MAX\n0G";
                    Passive_Button_0.interactable = false;
                }
                else
                {
                    Passive_Text_0.text = "Lv." + trainData.Level_Train_EngineTier + "\n" + trainData.Cost_Train_EngineTier + "G";
                }
            }
            else if (num == 1)
            {
                if (trainData.Level_Train_MaxSpeed == trainData.Max_Train_MaxSpeed)
                {
                    Passive_Text_1.text = "Lv.MAX\n0G";
                    Passive_Button_1.interactable = false;
                }
                else
                {
                    Passive_Text_1.text = "Lv." + trainData.Level_Train_MaxSpeed + "\n" + trainData.Cost_Train_MaxSpeed + "G";
                }
            }
            else if (num == 2)
            {
                if (trainData.Level_Train_Armor == trainData.Max_Train_Armor)
                {
                    Passive_Text_2.text = "Lv.MAX\n0G";
                    Passive_Button_2.interactable = false;
                }
                else
                {
                    Passive_Text_2.text = "Lv." + trainData.Level_Train_Armor + "\n" + trainData.Cost_Train_Armor + "G";
                }
            }
            else if (num == 3)
            {
                if (trainData.Level_Train_Efficient == trainData.Max_Train_Efficient)
                {
                    Passive_Text_3.text = "Lv.MAX\n0G";
                    Passive_Button_3.interactable = false;
                }
                else
                {
                    Passive_Text_3.text = "Lv." + trainData.Level_Train_Efficient + "\n" + trainData.Cost_Train_Efficient + "G";
                }
            }
        }
    }

    public void Click_Passive_Upgrade(int i)//LevelNum : 0 = Tier / 1 = Speed / 2 = Armor / 3 = Efficient
    {
        trainData.Passive_Level_Up(i);
        Passive_Text(false, i);
    }
}