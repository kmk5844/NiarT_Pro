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
    public SA_TrainBoosterData SA_TrainBoosterData;

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


    [Header("기존 상점 리스트")]
    public List<int> Train_Store_Num;

    [Header("기존 변경 리스트")]
    public List<int> Train_Change_Num;

    [Header("기존 포탑 상점 리스트")]
    public List<int> Train_Turret_Store_Num;

    [Header("기존 부스터 상점 리스트")]
    public List<int> Train_Booster_Store_Num;

    [Header("기존 포탑 변경 리스트")]
    public List<int> Train_Turret_Part_Change_Num;

    [Header("기존 부스터 변경 리스트")]
    public List<int> Train_Booster_Part_Change_Num;

    public bool Flag_BoosterTrain;
    private void Awake()
    {
        Check_Level_Train();
        Check_Store_Train();
        Check_Store_Turret_Part();
        Check_Store_Booster_Part();
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

    public void Check_Store_Train()
    {
        foreach(Info_Train train in EX_Game_Data.Information_Train)
        {
            if (train.Store)
            {
                Train_Store_Num.Add(train.Number);
            }
            else
            {
                if (train.Change)
                {
                    Train_Change_Num.Add(train.Number);
                }
            }
        }
        Train_Change_Num = Train_Change_Num.Concat(SA_TrainData.Train_Buy_Num).ToList();
    }

    public void Check_Store_Turret_Part()
    {
        foreach(Info_Train_Turret_Part Turret in EX_Game_Data.Information_Train_Turret_Part)
        {
            if (Turret.Store)
            {
                Train_Turret_Store_Num.Add(Turret.Num);
            }
            else
            {
                if (Turret.Change)
                {
                    Train_Turret_Part_Change_Num.Add(Turret.Num);
                }
            }
        }
        Train_Turret_Part_Change_Num = Train_Turret_Part_Change_Num.Concat(SA_TrainTurretData.Train_Turret_Buy_Num).ToList();
    }
    public void Check_Store_Booster_Part()
    {
        foreach(Info_Train_Booster_Part Booster in EX_Game_Data.Information_Train_Booster_Part)
        {
            if(Booster.Store)
            {
                Train_Booster_Store_Num.Add(Booster.Num);
            }
            else
            {
                if (Booster.Change)
                {
                    Train_Booster_Part_Change_Num.Add(Booster.Num);

                }
            }
        }
        Train_Booster_Part_Change_Num = Train_Booster_Part_Change_Num.Concat(SA_TrainBoosterData.Train_Booster_Buy_Num).ToList();
    }

    public void Check_Buy_Train(int Num)
    {
        Train_Change_Num.Add(Num);
    }

    public void Check_Buy_Turret(int Num)
    {
        Train_Turret_Part_Change_Num.Add(Num);
    }

    public void Check_Buy_Booster(int Num)
    {
        Train_Booster_Part_Change_Num.Add(Num);
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

    public void Train_Turret_Level_Up(int Train_Num, int index)
    {
        SA_TrainTurretData.SA_Train_Turret_Upgrade(Train_Num);
        SA_TrainTurretData.SA_Train_Turret_Upgrade_Renewal(index);
    }

    public void Train_Booster_Level_Up(int Train_Num, int index)
    {
        SA_TrainBoosterData.SA_Train_Booster_Upgrade(Train_Num);
        SA_TrainBoosterData.SA_Train_Booster_Upgrade_Renewal(index);
    }
}
