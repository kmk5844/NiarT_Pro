using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainUpgradeList_Button : MonoBehaviour
{
    public Station_TrainMaintenance Director;
    public int Train_Num;
    public int Train_Num2;
    public Image Train_Image;
    TextMeshProUGUI testtext;
    Button Btn;

    private void Start()
    {
        Btn = GetComponent<Button>();
        testtext = GetComponentInChildren<TextMeshProUGUI>();
        Btn.onClick.AddListener(()=> Director.TrainUpgradeList_Button_Click(Train_Num, Train_Num2));


        if(Train_Num == 51)
        {
            testtext.text = Train_Num + "_"+ Train_Num2;
            Train_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_51_" + (Train_Num2 / 10) * 10);
        }
        else if(Train_Num == 52)
        {
            testtext.text = Train_Num + "_" + Train_Num2;
            Train_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_52_" + (Train_Num2 / 10) * 10);
        }
        else
        {
            testtext.text = Train_Num.ToString();
            Train_Image.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Num);
        }
    }
}