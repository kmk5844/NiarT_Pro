using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_TrainTurretData", menuName = "Scriptable/TrainTurretData", order = 6)]

public class SA_TrainTurretData : ScriptableObject
{
    [SerializeField]
    private List<int> train_turret_num;
    public List<int> Train_Turret_Num { get { return train_turret_num; } }

    [SerializeField]
    private List<int> train_turret_buy_num;
    public List<int> Train_Turret_Buy_Num { get { return train_turret_buy_num; } }

    [SerializeField]
    private int level_trainturretnumber_00;
    public int Level_TrainTurretNumber_00 { get { return level_trainturretnumber_00; } }

    [SerializeField]
    private int level_trainturretnumber_10;
    public int Level_TrainTurretNumber_10 { get { return level_trainturretnumber_10; } }

    [SerializeField]
    private int level_trainturretnumber_20;
    public int Level_TrainTurretNumber_20 { get { return level_trainturretnumber_20; } }

    [SerializeField]
    private int level_trainturretnumber_30;
    public int Level_TrainTurretNumber_30 { get { return level_trainturretnumber_30; } }

    [SerializeField]
    private int level_trainturretnumber_40;
    public int Level_TrainTurretNumber_40 { get { return level_trainturretnumber_40; } }

    [SerializeField]
    private int level_trainturretnumber_50;
    public int Level_TrainTurretNumber_50 { get { return level_trainturretnumber_50; } }

    [SerializeField]
    private int level_trainturretnumber_60;
    public int Level_TrainTurretNumber_60 { get { return level_trainturretnumber_60; } }

    [SerializeField]
    private int level_trainturretnumber_70;
    public int Level_TrainTurretNumber_70 { get { return level_trainturretnumber_70; } }

    public void SA_Train_Turret_Upgrade(int trainNum)
    {
        switch (trainNum / 10)
        {
            case 0:
                level_trainturretnumber_00++;
                break;
            case 1:
                level_trainturretnumber_10++;
                break;
            case 2:
                level_trainturretnumber_20++;
                break;
            case 3:
                level_trainturretnumber_30++;
                break;
            case 4:
                level_trainturretnumber_40++;
                break;
            case 5:
                level_trainturretnumber_50++;
                break;
            case 6:
                level_trainturretnumber_60++;
                break;
            case 7:
                level_trainturretnumber_70++;
                break;
        }
        Save();
    }

    public int SA_Train_Turret_ChangeNum(int trainTurretNum)
    {
        switch (trainTurretNum / 10)
        {
            case 0:
                return level_trainturretnumber_00;
            case 1:
                return level_trainturretnumber_10;
            case 2:
                return level_trainturretnumber_20;
            case 3:
                return level_trainturretnumber_30;
            case 4:
                return level_trainturretnumber_40;
            case 5:
                return level_trainturretnumber_50;
            case 6:
                return level_trainturretnumber_60;
            case 7:
                return level_trainturretnumber_70;
            default:
                return 0;
        }
    }

    public void SA_Train_Turret_Upgrade_Renewal(int index)
    {
        int num = Train_Turret_Num[index];
        for (int i = 0; i < Train_Turret_Num.Count; i++)
        {
            if (Train_Turret_Num[i] == num)
            {
                Train_Turret_Num[i] = Train_Turret_Num[i] + 1;
            }
        }
        Save();
    }

    public void SA_Train_Turret_Upgrade_Renewal_2(int TrainNum)
    {
        for (int i = 0; i < Train_Turret_Num.Count; i++)
        {
            if (Train_Turret_Num[i] == TrainNum)
            {
                Train_Turret_Num[i] = Train_Turret_Num[i] + 1;
            }
        }
        Save();
    }

    public void SA_Train_Turret_Buy(int TrainNum)
    {
        train_turret_buy_num.Add(TrainNum);
        Save();
    }

    public void SA_Train_Turret_Add(int TrainNum)
    {
        train_turret_num.Add(TrainNum);
        Save();
    }
    public void SA_Train_Turret_Insert(int index,int TrainNum)
    {
        train_turret_num.Insert(index,TrainNum);
        Save();
    }

    public void SA_Train_Turret_Change(int index, int chnage)
    {
        train_turret_num[index] = chnage;
        Save();
    }

    public void SA_Train_Turret_Remove(int index)
    {
        train_turret_num.RemoveAt(index);
        Save();
    }

    private void Save()
    {
        ES3.Save("SA_TrainData_Data_Train_Turret_Num", train_turret_num);
        ES3.Save("SA_TrainData_Data_Train_Turret_Buy_Num", train_turret_buy_num);
        ES3.Save("SA_TrainData_Data_level_trainturretnumber_00", level_trainturretnumber_00);
        ES3.Save("SA_TrainData_Data_level_trainturretnumber_10", level_trainturretnumber_10);
        ES3.Save("SA_TrainData_Data_level_trainturretnumber_20", level_trainturretnumber_20);
        ES3.Save("SA_TrainData_Data_level_trainturretnumber_30", level_trainturretnumber_30);
        ES3.Save("SA_TrainData_Data_level_trainturretnumber_40", level_trainturretnumber_40);
        ES3.Save("SA_TrainData_Data_level_trainturretnumber_50", level_trainturretnumber_50);
        ES3.Save("SA_TrainData_Data_level_trainturretnumber_60", level_trainturretnumber_60);
        ES3.Save("SA_TrainData_Data_level_trainturretnumber_70", level_trainturretnumber_70);
    }

    public void Load()
    {
        train_turret_num = ES3.Load<List<int>>("SA_TrainData_Data_Train_Turret_Num");
        train_turret_buy_num = ES3.Load<List<int>>("SA_TrainData_Data_Train_Turret_Buy_Num");
        level_trainturretnumber_00 = ES3.Load<int>("SA_TrainData_Data_level_trainturretnumber_00");
        level_trainturretnumber_10 = ES3.Load<int>("SA_TrainData_Data_level_trainturretnumber_10");
        level_trainturretnumber_20 = ES3.Load<int>("SA_TrainData_Data_level_trainturretnumber_20");
        level_trainturretnumber_30 = ES3.Load<int>("SA_TrainData_Data_level_trainturretnumber_30");
        level_trainturretnumber_40 = ES3.Load<int>("SA_TrainData_Data_level_trainturretnumber_40");
        level_trainturretnumber_50 = ES3.Load<int>("SA_TrainData_Data_level_trainturretnumber_50");
        level_trainturretnumber_60 = ES3.Load<int>("SA_TrainData_Data_level_trainturretnumber_60");
        level_trainturretnumber_70 = ES3.Load<int>("SA_TrainData_Data_level_trainturretnumber_70");
    }

    public void Init()
    {
        train_turret_num.Clear();
        train_turret_buy_num.Clear();

        level_trainturretnumber_00 = 0;
        level_trainturretnumber_10 = 10;
        level_trainturretnumber_20 = 20;
        level_trainturretnumber_30 = 30;
        level_trainturretnumber_40 = 40;
        level_trainturretnumber_50 = 50;
        level_trainturretnumber_60 = 60;
        level_trainturretnumber_70 = 70;
        Save();
    }
}