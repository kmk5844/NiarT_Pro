using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Station_GameStart : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public Button GameStart_Button;
    public TextMeshProUGUI text;

    int Fuel_Count;

    void Awake()
    {
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
    }

    public void Check_Train()
    {
        Fuel_Count = 0;
        for(int i = 0; i < trainData.Train_Num.Count; i++)
        {
            if(trainData.Train_Num[i] / 10 == 1)
            {
                Fuel_Count++;
            }
        }

        if(Fuel_Count == 0)
        {
            text.text = "원활한 게임 플레이를 위해\n적어도 연료 기차 한 대가 필요합니다.";
            GameStart_Button.interactable = false;
        }
        else
        {
            text.text = "게임 시작이 가능합니다.";
            GameStart_Button.interactable = true;
        }
    }

    public void Click_GameStart()
    {
        LoadingManager.LoadScene("CharacterSelect");
    }
}
