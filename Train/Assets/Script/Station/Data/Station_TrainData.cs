using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station_TrainData : MonoBehaviour
{
    [Header("기차 데이터 모아놓은 스크립터블")]
    public Game_DataTable EX_Game_Data;
    public Level_DataTable EX_Level_Data;
    public SA_TrainData SA_TrainData;

    [Header("기차 데이터")]
    public List<int> Train_Num;
    public int Level_Train_EngineTier;
    public int Level_Train_MaxSpeed;
    public int Level_Train_Armor;
    public int Level_Train_Efficient;

    public int Max_Train_EngineTier;
    public int Max_Train_MaxSpeed;
    public int Max_Train_Armor;
    public int Max_Train_Efficient;

    public int Cost_Train_EngineTier;
    public int Cost_Train_MaxSpeed;
    public int Cost_Train_Armor;
    public int Cost_Train_Efficient;

    [Header("엔진 티어에 따라 달라지는 Max")]
    public int Max_Train_MaxMercenary;
    public int Max_Train_MaxTrain;

    [Header("기존 상점 리스트")]
    public List<int> Train_Store_Num;

    private void Awake()
    {
        Check_Level_Train();
        Check_Store_Train();
        Max_Train_EngineTier = EX_Level_Data.Information_Level[Data_Index("Level_Train_EngineTier")].Max_Level;
        Max_Train_MaxSpeed = EX_Level_Data.Information_Level[Data_Index("Level_Train_MaxSpeed")].Max_Level;
        Max_Train_Armor = EX_Level_Data.Information_Level[Data_Index("Level_Train_Armor")].Max_Level;
        Max_Train_Efficient = EX_Level_Data.Information_Level[Data_Index("Level_Train_Efficient")].Max_Level;
    }

    private void Check_Level_Train()
    {
        Train_Num = SA_TrainData.Train_Num;
        Level_Train_EngineTier = SA_TrainData.Level_Train_EngineTier;
        Level_Train_MaxSpeed = SA_TrainData.Level_Train_MaxSpeed;
        Level_Train_Armor = SA_TrainData.Level_Train_Armor;
        Level_Train_Efficient = SA_TrainData.Level_Train_Efficient;

        Cost_Train_EngineTier = EX_Level_Data.Information_LevelCost[Level_Train_EngineTier].Cost_Level_Train_EngineTier;
        Cost_Train_MaxSpeed = EX_Level_Data.Information_LevelCost[Level_Train_MaxSpeed].Cost_Level_Train_MaxSpeed;
        Cost_Train_Armor = EX_Level_Data.Information_LevelCost[Level_Train_Armor].Cost_Level_Train_Armor;
        Cost_Train_Efficient = EX_Level_Data.Information_LevelCost[Level_Train_Efficient].Cost_Level_Train_Efficient;

        Max_Train_MaxMercenary = EX_Level_Data.Level_Max_EngineTier[Level_Train_EngineTier].Max_Mercenary;
        Max_Train_MaxTrain = EX_Level_Data.Level_Max_EngineTier[Level_Train_EngineTier].Max_Train;
    }

    private void Check_Store_Train()
    {
        foreach(Info_Train train in EX_Game_Data.Information_Train)
        {
            if (train.Store)
            {
                Train_Store_Num.Add(train.Number);
            }
        }
    }

    public void Passive_Level_Up(int LevelNum)//LevelNum : 0 = Tier / 1 = Speed / 2 = Armor / 3 = Efficient
    {
        SA_TrainData.SA_Passive_Level_Up(LevelNum);
        Check_Level_Train();
    }

    private int Data_Index(string str)
    {
        int index = EX_Level_Data.Information_Level.FindIndex(x => x.Level_Name.Equals(str));
        return index;
    }
}
