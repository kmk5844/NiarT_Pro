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
    SceneManager sceneManager;

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
            text.text = "At least one fuel train is required for gameplay.";
            GameStart_Button.interactable = false;
        }
        else
        {
            text.text = "Let's Play";
            GameStart_Button.interactable = true;
        }
    }

    public void Click_GameStart()
    {
        SceneManager.LoadScene("InGame");
    }
}
