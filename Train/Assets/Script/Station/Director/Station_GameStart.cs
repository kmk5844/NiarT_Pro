using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Station_GameStart : MonoBehaviour
{
    [Header("������ ����")]
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
            text.text = "��Ȱ�� ���� �÷��̸� ����\n��� ���� ���� �� �밡 �ʿ��մϴ�.";
            GameStart_Button.interactable = false;
        }
        else
        {
            text.text = "���� ������ �����մϴ�.";
            GameStart_Button.interactable = true;
        }
    }

    public void Click_GameStart()
    {
        LoadingManager.LoadScene("CharacterSelect");
    }
}
