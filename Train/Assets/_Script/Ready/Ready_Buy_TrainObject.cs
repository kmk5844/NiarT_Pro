using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class Ready_Buy_TrainObject : MonoBehaviour
{
    public PlayerReadyDirector director;

    public int TrainNum_1;
    public int TrainNum_2;
    public LocalizeStringEvent Name_Text;
    public Image TrainImage;
    public Button Btn;

    private void Start()
    {
        Name_Text.StringReference.TableReference = "ExcelData_Table_St";
        if (TrainNum_1 == 91 /*|| TrainNum_1 == 52*/)
        {
            TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1 + "_" + (TrainNum_2 / 10) * 10);
            if (TrainNum_1 == 91)
            {
                Name_Text.StringReference.TableEntryReference = "Train_Turret_Name_" + (TrainNum_2 / 10);
            }
/*            else if (TrainNum_1 == 52)
            {
                Name_Text.StringReference.TableEntryReference = "Train_Booster_Name_" + (TrainNum_2 / 10);
            }*/
        }
        else
        {
            TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1);
            if(TrainNum_1 < 90)
            {
                Name_Text.StringReference.TableEntryReference = "Train_Name_" + (TrainNum_1 / 10);
            }
            else
            {
                Name_Text.StringReference.TableEntryReference = "Train_Name_" + TrainNum_1;
            }
        }
        Btn.onClick.AddListener(()=>director.Click_Select_Train(TrainNum_1, TrainNum_2));
    }
}