using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_TrainBoosterData", menuName = "Scriptable/TrainBoosterData", order = 7)]

public class SA_TrainBoosterData : ScriptableObject
{
    [SerializeField]
    private List<int> train_booster_num;
    public List<int> Train_Booster_Num { get { return train_booster_num; } }

    [SerializeField]
    private List<int> train_booster_buy_num;
    public List<int> Train_Booster_Buy_Num { get { return train_booster_buy_num; } }

    [SerializeField]
    private int level_trainboosternumber_00;
    public int Level_TrainBoosterNumber_00 { get { return level_trainboosternumber_00; } }

    [SerializeField]
    private int level_trainboosternumber_10;
    public int Level_TrainBoosterNumber_10 { get { return level_trainboosternumber_10; } }

    [SerializeField]
    private int level_trainboosternumber_20;
    public int Level_TrainBoosterNumber_20 { get { return level_trainboosternumber_20; } }

    [SerializeField]
    private int level_trainboosternumber_30;
    public int Level_TrainBoosterNumber_30 { get { return level_trainboosternumber_30; } }

    [SerializeField]
    private int level_trainboosternumber_40;
    public int Level_TrainBoosterNumber_40 { get { return level_trainboosternumber_40; } }

  
    public void SA_Train_Booster_Upgrade(int trainBoosterNum)
    {
        switch (trainBoosterNum / 10)
        {
            case 0:
                level_trainboosternumber_00++;
                break;
            case 1:
                level_trainboosternumber_10++;
                break;
            case 2:
                level_trainboosternumber_20++;
                break;
            case 3:
                level_trainboosternumber_30++;
                break;
            case 4:
                level_trainboosternumber_40++;
                break;
        }
        Save();
    }

    public int SA_Train_Booster_ChangeNum(int trainBoosterNum)
    {
        switch (trainBoosterNum / 10)
        {
            case 0:
                return level_trainboosternumber_00;
            case 1:
                return level_trainboosternumber_10;
            case 2:
                return level_trainboosternumber_20;
            case 3:
                return level_trainboosternumber_30;
            case 4:
                return level_trainboosternumber_40;
            default:
                return 0;
        }
    }

    public void SA_Train_Booster_Upgrade_Renewal(int index)
    {
        int num = Train_Booster_Num[index];
        for (int i = 0; i < Train_Booster_Num.Count; i++)
        {
            if (Train_Booster_Num[i] == num)
            {
                Train_Booster_Num[i] = Train_Booster_Num[i] + 1;
            }
        }
        Save();
    }

    public void SA_Train_Booster_Upgrade_Renewal_2(int TrainNum)
    {
        for (int i = 0; i < Train_Booster_Num.Count; i++)
        {
            if (Train_Booster_Num[i] == TrainNum)
            {
                Train_Booster_Num[i] = Train_Booster_Num[i] + 1;
            }
        }
        Save();
    }

    public void SA_Train_Booster_Buy(int TrainNum)
    {
        train_booster_buy_num.Add(TrainNum);
        Save();
    }

    public void SA_Train_Booster_Add(int TrainNum)
    {
        train_booster_num.Add(TrainNum);
        Save();
    }

    public void SA_Train_Booster_Insert(int index,int TrainNum)
    {
        train_booster_num.Insert(index, TrainNum);
        Save();
    }

    public void SA_Train_Booster_Change(int index, int chnage)
    {
        train_booster_num[index] = chnage;
        Save();
    }

    public void SA_Train_Booster_Remove(int index)
    {
        train_booster_num.RemoveAt(index);
        Save();
    }

    private void Save()
    {
        ES3.Save("SA_TrainData_Data_Train_Booster_Num", train_booster_num);
        ES3.Save("SA_TrainData_Data_Train_Booster_Buy_Num", train_booster_buy_num);
        ES3.Save("SA_TrainData_Data_level_trainboosternumber_00", level_trainboosternumber_00);
        ES3.Save("SA_TrainData_Data_level_trainboosternumber_10", level_trainboosternumber_10);
        ES3.Save("SA_TrainData_Data_level_trainboosternumber_20", level_trainboosternumber_20);
        ES3.Save("SA_TrainData_Data_level_trainboosternumber_30", level_trainboosternumber_30);
        ES3.Save("SA_TrainData_Data_level_trainboosternumber_40", level_trainboosternumber_40);
    }

    public void Load()
    {
        train_booster_num = ES3.Load<List<int>>("SA_TrainData_Data_Train_Booster_Num");
        train_booster_buy_num = ES3.Load<List<int>>("SA_TrainData_Data_Train_Booster_Buy_Num");
        level_trainboosternumber_00 = ES3.Load<int>("SA_TrainData_Data_level_trainboosternumber_00");
        level_trainboosternumber_10 = ES3.Load<int>("SA_TrainData_Data_level_trainboosternumber_10");
        level_trainboosternumber_20 = ES3.Load<int>("SA_TrainData_Data_level_trainboosternumber_20");
        level_trainboosternumber_30 = ES3.Load<int>("SA_TrainData_Data_level_trainboosternumber_30");
        level_trainboosternumber_40 = ES3.Load<int>("SA_TrainData_Data_level_trainboosternumber_40");
    }

    public void Init()
    {
        train_booster_num.Clear();
        train_booster_buy_num.Clear();

        level_trainboosternumber_00 = 0;
        level_trainboosternumber_10 = 10;
        level_trainboosternumber_20 = 20;
        level_trainboosternumber_30 = 30;
        level_trainboosternumber_40 = 40;
        Save();
    }

    public IEnumerator InitAsync()
    {
        train_booster_num.Clear();
        train_booster_buy_num.Clear();

        level_trainboosternumber_00 = 0;
        level_trainboosternumber_10 = 10;
        level_trainboosternumber_20 = 20;
        level_trainboosternumber_30 = 30;
        level_trainboosternumber_40 = 40;
        Save();
        yield return null;
    }
}