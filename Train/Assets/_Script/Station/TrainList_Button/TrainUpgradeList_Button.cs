using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TrainUpgradeList_Button : MonoBehaviour
{
    public Station_TrainMaintenance Director;
    public LocalizeStringEvent Train_Text;
    public int Train_Num;
    public int Train_Num2;
    public Image Train_Image;
    Button Btn;

    private void Start()
    {
        Btn = GetComponent<Button>();
        Btn.onClick.AddListener(()=> Director.TrainUpgradeList_Button_Click(Train_Num, Train_Num2, gameObject));
        Train_Text.StringReference.TableReference = "ExcelData_Table_St";

        if (Train_Num == 51)
        {
            Train_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_51_" + (Train_Num2 / 10) * 10);
            Train_Text.StringReference.TableEntryReference = "Train_Turret_Name_" + (Train_Num2 / 10);
        }
        else if(Train_Num == 52)
        {
            Train_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_52_" + (Train_Num2 / 10) * 10);
            Train_Text.StringReference.TableEntryReference = "Train_Booster_Name_" + (Train_Num2 / 10);
        }
        else
        {
            Train_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
            Train_Text.StringReference.TableEntryReference = "Train_Name_" + (Train_Num / 10);
        }
    }

    public void Upgrade(int num, int num2)
    {
        Train_Num = num;
        Train_Num2 = num2;

        if (Train_Num == 51)
        {
            Train_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_51_" + (Train_Num2 / 10) * 10);
        }
        else if (Train_Num == 52)
        {
            Train_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_52_" + (Train_Num2 / 10) * 10);
        }
        else
        {
            Train_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
        }
    }
}