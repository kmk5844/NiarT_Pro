using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store_Train_Card : MonoBehaviour
{
    [SerializeField]
    private GameObject TrainData_Object;
    Station_TrainData trainData;

    public int Train_Num;
    public int Train_Num2;
    public GameObject Train_Image;
    public GameObject Train_NameText;
    public GameObject Train_Buy;

    private void Awake()
    {
        TrainData_Object = GameObject.Find("TrainData");
        trainData = TrainData_Object.GetComponent<Station_TrainData>();
        GetComponentInChildren<Toggle>().group = GetComponentInParent<ToggleGroup>();

        if (Train_Num == 51)
        {
            if(Train_Num2 == -1)
            {
                Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
                Train_NameText.GetComponent<TextMeshProUGUI>().text = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Name;
            }
            else
            {
                Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_51_" + Train_Num2);
                Train_NameText.GetComponent<TextMeshProUGUI>().text = trainData.EX_Game_Data.Information_Train_Turret_Part[Train_Num2].Turret_Part_Name;
            }
        }
        else if (Train_Num == 52)
        {
            if (Train_Num2 == -1)
            {
                Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
                Train_NameText.GetComponent<TextMeshProUGUI>().text = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Name;
            }
            else
            {
                Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_52_" + Train_Num2);
                Train_NameText.GetComponent<TextMeshProUGUI>().text = trainData.EX_Game_Data.Information_Train_Booster_Part[Train_Num2].Booster_Part_Name;
            }
        }
        else
        {

            Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
            Train_NameText.GetComponent<TextMeshProUGUI>().text = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Name;
        }
    }
}