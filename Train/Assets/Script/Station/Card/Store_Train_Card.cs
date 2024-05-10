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
    public GameObject Train_Image;
    public GameObject Train_NameText;
    public GameObject Train_Buy;

    private void Awake()
    {
        TrainData_Object = GameObject.Find("TrainData");
        trainData = TrainData_Object.GetComponent<Station_TrainData>();
        GetComponentInChildren<Toggle>().group = GetComponentInParent<ToggleGroup>();

        Train_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
        Train_NameText.GetComponent<TextMeshProUGUI>().text = trainData.EX_Game_Data.Information_Train[Train_Num].Train_Name;
    }
}