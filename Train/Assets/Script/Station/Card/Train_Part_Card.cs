using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;


public class Train_Part_Card : MonoBehaviour
{
    [SerializeField]
    private GameObject TrainData_Object;
    Station_TrainData trainData;

    public int Train_Type_Num;
    public int Train_Part_Num;

    public Image Train_Part_Image;
    public LocalizeStringEvent Train_Part_Name;

    private void Awake()
    {
        TrainData_Object = GameObject.Find("TrainData");
        trainData = TrainData_Object.GetComponent<Station_TrainData>();
        GetComponentInChildren<Toggle>().group = GetComponentInParent<ToggleGroup>();
        Train_Part_Name.StringReference.TableReference = "ExcelData_Table_St";

        Train_Part_Num = (Train_Part_Num / 10) * 10;

        if(Train_Type_Num == 51)
        {
            Train_Part_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_51_" + Train_Part_Num);
            Train_Part_Name.StringReference.TableEntryReference = "Train_Turret_Name_" + (Train_Part_Num /10);
            //Train_Part_Name.text = trainData.EX_Game_Data.Information_Train_Turret_Part[Train_Part_Num].Turret_Part_Name;
        }
        else if(Train_Type_Num == 52)
        {
            Train_Part_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_52_" + Train_Part_Num);
            Train_Part_Name.StringReference.TableEntryReference = "Train_Booster_Name_" + (Train_Part_Num/10);
            //Train_Part_Name.text = trainData.EX_Game_Data.Information_Train_Booster_Part[Train_Part_Num].Booster_Part_Name;
        }
    }
}
