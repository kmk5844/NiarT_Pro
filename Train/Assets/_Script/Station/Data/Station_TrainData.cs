using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Station_TrainData : MonoBehaviour
{
    [Header("기차 데이터 모아놓은 스크립터블")]
    public Game_DataTable EX_Game_Data;
    public Level_DataTable EX_Level_Data;
    public SA_TrainData SA_TrainData;
    public SA_TrainTurretData SA_TrainTurretData;

    [Header("기차 데이터")]
    public List<int> Train_Num;
    public int Level_Train_EngineTier;
    public int Level_Train_MaxTrain;
    public int Level_Train_MaxMercenary;
    public int Level_Train_MaxSpeed;
    public int Level_Train_Armor;
    public int Level_Train_Efficient;

    public int Max_Train_EngineTier;
    public int Max_Train_MaxSpeed;
    public int Max_Train_Armor;
    public int Max_Train_Efficient;
    public int Max_Train_MaxMercenary;
    public int Max_Train_MaxTrain;

    public int Cost_Train_EngineTier;
    public int Cost_Train_MaxTrain;
    public int Cost_Train_MaxMercenary;
    public int Cost_Train_MaxSpeed;
    public int Cost_Train_Armor;
    public int Cost_Train_Efficient;

    public bool Flag_BoosterTrain;
    private void Awake()
    {
        Check_Level_Train();
        Max_Train_EngineTier = EX_Level_Data.Information_Level[Data_Index("Level_Train_EngineTier")].Max_Level;
        Max_Train_MaxTrain = EX_Level_Data.Information_Level[Data_Index("Level_train_MaxTrain")].Max_Level;
        Max_Train_MaxMercenary = EX_Level_Data.Information_Level[Data_Index("Level_Train_MaxMercenary")].Max_Level;
        Max_Train_MaxSpeed = EX_Level_Data.Information_Level[Data_Index("Level_Train_MaxSpeed")].Max_Level;
        Max_Train_Armor = EX_Level_Data.Information_Level[Data_Index("Level_Train_Armor")].Max_Level;
        Max_Train_Efficient = EX_Level_Data.Information_Level[Data_Index("Level_Train_Efficient")].Max_Level;
    }

    private void Check_Level_Train()
    {
        Train_Num = SA_TrainData.Train_Num;
        Level_Train_EngineTier = SA_TrainData.Level_Train_EngineTier;
        Level_Train_MaxTrain = SA_TrainData.Level_Train_MaxTrain;
        Level_Train_MaxMercenary = SA_TrainData.Level_Train_MaxMercenary;
        Level_Train_MaxSpeed = SA_TrainData.Level_Train_MaxSpeed;
        Level_Train_Armor = SA_TrainData.Level_Train_Armor;
        Level_Train_Efficient = SA_TrainData.Level_Train_Efficient;

        Cost_Train_EngineTier = EX_Level_Data.Information_LevelCost[Level_Train_EngineTier].Cost_Level_Train_EngineTier;
        Cost_Train_MaxTrain = EX_Level_Data.Information_LevelCost[Level_Train_MaxTrain].Cost_Level_Train_MaxTrain;
        Cost_Train_MaxMercenary = EX_Level_Data.Information_LevelCost[Level_Train_MaxMercenary].Cost_Level_Train_MaxMercenary;
        Cost_Train_MaxSpeed = EX_Level_Data.Information_LevelCost[Level_Train_MaxSpeed].Cost_Level_Train_MaxSpeed;
        Cost_Train_Armor = EX_Level_Data.Information_LevelCost[Level_Train_Armor].Cost_Level_Train_Armor;
        Cost_Train_Efficient = EX_Level_Data.Information_LevelCost[Level_Train_Efficient].Cost_Level_Train_Efficient;
    }
    public int Check_Cost_Train(int num)
    {
        switch (num)
        {
            case 0:
                return Cost_Train_EngineTier;
            case 1:
                return Cost_Train_MaxTrain;
            case 2:
                return Cost_Train_MaxMercenary;
            case 3:
                return Cost_Train_MaxSpeed;
            case 4:
                return Cost_Train_Armor;
            case 5:
                return Cost_Train_Efficient;
        }
        return -1;
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

    public void Train_Level_Up(int Train_Num)
    {
        SA_TrainData.SA_TrainUpgrade(Train_Num);
        //SA_TrainData.SA_TrainUpgrade_Renewal(index);
        SA_TrainData.SA_TrainUpgrade_Renewal_2(Train_Num);
    }

    public void Train_Turret_Level_Up(int Train_Num)
    {
        SA_TrainTurretData.SA_Train_Turret_Upgrade(Train_Num);
        //SA_TrainTurretData.SA_Train_Turret_Upgrade_Renewal(index);
        SA_TrainTurretData.SA_Train_Turret_Upgrade_Renewal_2(Train_Num);
    }

/*    public void Train_Booster_Level_Up(int Train_Num)
    {
        SA_TrainBoosterData.SA_Train_Booster_Upgrade(Train_Num);
        //SA_TrainBoosterData.SA_Train_Booster_Upgrade_Renewal(index);
        SA_TrainBoosterData.SA_Train_Booster_Upgrade_Renewal_2(Train_Num);
    }*/
}
