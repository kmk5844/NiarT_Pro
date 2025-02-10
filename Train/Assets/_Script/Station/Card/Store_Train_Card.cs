using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class Store_Train_Card : MonoBehaviour
{
    [SerializeField]
    public Station_TrainData trainData;
    public Station_Store storeDirector;

    public int Train_Num;
    public int Train_Num2;
    public GameObject Train_Image;
    public LocalizeStringEvent Train_NameText;
    public GameObject Train_Buy;
    string train_name;
    string train_information;
    int train_pride;

    [Header("정보 표시")]
    public StoreList_Tooltip store_tooltip_object;

    [SerializeField]
    bool Train_Information_Flag;
    [SerializeField]
    bool Train_mouseOver_Flag;

    private void Start()
    {
        Train_NameText.StringReference.TableReference = "ExcelData_Table_St";

        if (Train_Num == 51)
        {
            if(Train_Num2 == -1)
            {
                Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
                train_name = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Name;
                train_information = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Information;
                train_pride = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Buy_Cost;
                
                Train_NameText.StringReference.TableEntryReference = "Train_Name_51";
                //Train_NameText.GetComponent<TextMeshProUGUI>().text = train_name;
            }
            else
            {
                Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_51_" + Train_Num2);
                train_name = trainData.EX_Game_Data.Information_Train_Turret_Part[Train_Num2].Turret_Part_Name;
                train_information = trainData.EX_Game_Data.Information_Train_Turret_Part[Train_Num2].Train_Information;
                train_pride = trainData.EX_Game_Data.Information_Train_Turret_Part[Train_Num2].Train_Buy_Cost;
                
                Train_NameText.StringReference.TableEntryReference = "Train_Turret_Name_" + (Train_Num2 / 10);
                //Train_NameText.GetComponent<TextMeshProUGUI>().text =train_name;
            }
        }
        else if (Train_Num == 52)
        {
            if (Train_Num2 == -1)
            {
                Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
                train_name = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Name;
                train_information = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Information;
                train_pride = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Buy_Cost;
                
                Train_NameText.StringReference.TableEntryReference = "Train_Name_52";
                //Train_NameText.GetComponent<TextMeshProUGUI>().text = train_name;
            }
            else
            {
                Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_52_" + Train_Num2);
                train_name = trainData.EX_Game_Data.Information_Train_Booster_Part[Train_Num2].Booster_Part_Name;
                train_information = trainData.EX_Game_Data.Information_Train_Booster_Part[Train_Num2].Train_Information;
                train_pride = trainData.EX_Game_Data.Information_Train_Booster_Part[Train_Num2].Train_Buy_Cost;
                
                Train_NameText.StringReference.TableEntryReference = "Train_Booster_Name_" + (Train_Num2 / 10);
                //Train_NameText.GetComponent<TextMeshProUGUI>().text = train_name;
            }
        }
        else
        {
            Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
            train_name = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Name;
            train_information = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Information;
            train_pride = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Buy_Cost;
            
            if(Train_Num < 50)
            {
                Train_NameText.StringReference.TableEntryReference = "Train_Name_" + (Train_Num/10);
            }
            else
            {
                Train_NameText.StringReference.TableEntryReference = "Train_Name_" + Train_Num;
            }
            //Train_NameText.GetComponent<TextMeshProUGUI>().text = train_name;
        }
    }

    private void Update()
    {
/*        if (StationDirector.TooltipFlag)
        {
            if (Train_Information_Flag)
            {
                store_tooltip_object.Tooltip_ON(true, train_pride, Train_Num, Train_Num2);
                Train_mouseOver_Flag = true;
            }
            else
            {
                if (Train_mouseOver_Flag)
                {
                    store_tooltip_object.Tooltip_Off();
                    Train_mouseOver_Flag = false;
                }
            }
        }
        else
        {
            if (Train_Information_Flag)
            {
                Train_Information_Flag = false;
                Train_mouseOver_Flag = false;
                store_tooltip_object.Tooltip_Off();
            }
        }*/
    }

    public void OnMouseEnter()
    {
        if (Train_Buy.activeSelf != true)
        {
            Train_Information_Flag = true;
        }
    }

    public void OnMouseExit()
    {
        if (Train_Buy.activeSelf != true)
        {
            Train_Information_Flag = false;
        }
    }

/*    public void OnPointerClick(PointerEventData eventData)
    {
        if (Train_Buy.activeSelf != true)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if (Train_Num == 51)
                {
                    if (Train_Num2 == -1)
                    {
                        storeDirector.Open_Buy_Window(0, Train_Num);
                    }
                    else
                    {
                        storeDirector.Open_Buy_Window(0, Train_Num2);
                    }
                }
                else if (Train_Num == 52)
                {
                    if (Train_Num2 == -1)
                    {
                        storeDirector.Open_Buy_Window(0, Train_Num);
                    }
                    else
                    {
                        storeDirector.Open_Buy_Window(0, Train_Num2);
                    }
                }
                else
                {
                    storeDirector.Open_Buy_Window(0, Train_Num);
                }
            }
        }
    }   */
}