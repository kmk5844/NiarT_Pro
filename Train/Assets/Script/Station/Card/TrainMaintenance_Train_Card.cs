using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainMaintenance_Train_Card : MonoBehaviour
{
    [SerializeField]
    private GameObject TrainData_Object;
    Station_TrainData trainData;

    public int Train_Num;
    public GameObject Train_Image;
    public GameObject Train_NameText;

    private void Awake()
    {
        TrainData_Object = GameObject.Find("TrainData"); //소환술로 인해 나옴.
        trainData = TrainData_Object.GetComponent<Station_TrainData>();
        GetComponentInChildren<Toggle>().group = GetComponentInParent<ToggleGroup>();

        Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
        Train_NameText.GetComponent<TextMeshProUGUI>().text = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Name
            + "\n<size=25>";// + trainData.EX_Game_Data.Information_Train[Train_Num].Train_Change_Cost + "G</size>";
    }
}