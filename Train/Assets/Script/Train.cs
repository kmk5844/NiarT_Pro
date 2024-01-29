using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Train : MonoBehaviour
{
    [Header("기차 선택")]
    public Train_DataTable trainData;
    public int TrainNum;
    [Header("선택된 기차 정보")]
    public string Train_Name;
    public int Train_HP;
    public int Train_Weight;
    public string Train_Type;
    public int Train_MaxSpeed;
    public int Train_Efficienl;
    public int Train_Engine_Power;
    public int Train_Fuel;
    public int Train_Attack;
    public float Train_Attack_Delay;
    [Header("HP 슬라이더")]
    public Slider HP_Slider;
    [SerializeField]
    int cur_HP;

    // Start is called before the first frame update
    private void Awake()
    {
        Train_Name = trainData.Information_Train[TrainNum].Train_Name;
        Train_HP = trainData.Information_Train[TrainNum].Train_HP;
        cur_HP = Train_HP;
        Train_Weight = trainData.Information_Train[TrainNum].Train_Weight;
        Train_Type = trainData.Information_Train[TrainNum].Train_Type;
        switch (Train_Type)
        {
            case "Engine":
                Train_MaxSpeed = trainData.Information_Train[TrainNum].Train_MaxSpeed;
                Train_Efficienl = trainData.Information_Train[TrainNum].Train_Efficienl;
                Train_Engine_Power = trainData.Information_Train[TrainNum].Train_Engine_Power;
                Train_Fuel = 0;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                break;
            case "Fuel":
                Train_MaxSpeed = 0;
                Train_Efficienl = 0;
                Train_Engine_Power = 0;
                Train_Fuel = trainData.Information_Train[TrainNum].Train_Fuel;
                Train_Attack = 0;
                Train_Attack_Delay = 0;
                break;
            case "Attack":
                Train_MaxSpeed = 0;
                Train_Efficienl = 0;
                Train_Engine_Power = 0;
                Train_Fuel = 0;
                Train_Attack = trainData.Information_Train[TrainNum].Train_Attack;
                Train_Attack_Delay = trainData.Information_Train[TrainNum].Train_Attack_Delay;
                break;
        }
    }

    private void Update()
    {
        HP_Slider.value = cur_HP / Train_HP;
    }
}
