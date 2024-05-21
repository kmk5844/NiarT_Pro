using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_TrainData", menuName = "Scriptable/TrainData", order = 2)]

public class SA_TrainData : ScriptableObject
{
    [SerializeField]
    private List<int> train_num;
    public List<int> Train_Num { get { return train_num; } }
    [SerializeField]
    private int level_train_enginetier;
    public int Level_Train_EngineTier { get { return level_train_enginetier;} }
    [SerializeField]
    private int level_train_maxspeed;
    public int Level_Train_MaxSpeed { get {  return level_train_maxspeed; } }
    [SerializeField]
    private int level_train_armor;
    public int Level_Train_Armor { get { return level_train_armor; } }
    [SerializeField]
    private int level_train_efficient;
    public int Level_Train_Efficient { get { return level_train_efficient; } }

    [SerializeField]
    private List<int> train_buy_num;
    public List<int> Train_Buy_Num { get { return train_buy_num; } }

    [SerializeField]
    private int level_trainnumber_00;
    public int Level_TrainNumber_00 { get { return level_trainnumber_00; } }
    [SerializeField]
    private int level_trainnumber_10;
    public int Level_TrainNumber_10 { get { return level_trainnumber_10; } }
    [SerializeField]
    private int level_trainnumber_20;
    public int Level_TrainNumber_20 { get { return level_trainnumber_20; } }
    [SerializeField]
    private int level_trainnumber_30;
    public int Level_TrainNumber_30 { get { return level_trainnumber_30; } }
    [SerializeField]
    private int level_trainnumber_40;
    public int Level_TrainNumber_40 { get { return level_trainnumber_40; } }

    public void SA_Passive_Level_Up(int LevelNum) //LevelNum : 0 = Tier / 1 = Speed / 2 = Armor / 3 = Efficient
    {
        switch (LevelNum)
        {
            case (0):
                level_train_enginetier++;
                  Save();
                break;
            case (1):
                level_train_maxspeed++;
                  Save();
                break;
            case (2):
                level_train_armor++;
                  Save();
                break;
            case (3):
                level_train_efficient++;
                  Save();
                break;
        }
    }

    public void SA_TrainUpgrade(int trainNum)
    {
        switch(trainNum/10)
        {
            case 0:
                level_trainnumber_00++;
                break;
            case 1:
                level_trainnumber_10++;
                break;
            case 2:
                level_trainnumber_20++;
                break;
            case 3:
                level_trainnumber_30++;
                break;
            case 4:
                level_trainnumber_40++;
                break;
        }
        Save();
    }

    public int SA_TrainChangeNum(int trainNum)
    {
        switch (trainNum / 10)
        {
            case 0:
                return level_trainnumber_00;
            case 1:
                return level_trainnumber_10;
            case 2:
                return level_trainnumber_20;
            case 3:
                return level_trainnumber_30;
            case 4:
                return level_trainnumber_40;
            default:
                return 0;
        }
    }

    public void SA_TrainUpgrade_Renewal(int index)
    {
        int num = Train_Num[index];
        for(int i = 0; i < Train_Num.Count; i++)
        {
            if (Train_Num[i] == num)
            {
                Train_Num[i] = Train_Num[i] + 1;
            }
        }
        Save();
    }

    public void SA_Train_Buy(int TrainNum)
    {
        train_buy_num.Add(TrainNum);
        Save();
    }

    public void SA_Train_Add(int TrainNum)
    {
        train_num.Add(TrainNum);
        Save();
    }

    public void SA_Train_Change(int index, int chnage)
    {
        train_num[index] = chnage;
        Save();
    }

    private void Save()
    {
        ES3.Save("SA_TrainData_Data_Train_Num", train_num);
        PlayerPrefs.SetInt("SA_TrainData_Data_level_train_enginetier", level_train_enginetier);
        PlayerPrefs.SetInt("SA_TrainData_Data_level_train_maxspeed", level_train_maxspeed);
        PlayerPrefs.SetInt("SA_TrainData_Data_level_train_armor", level_train_armor);
        PlayerPrefs.SetInt("SA_TrainData_Data_level_train_efficient", level_train_efficient);
        ES3.Save("SA_TrainData_Data_Train_Buy_Num", train_buy_num);
        PlayerPrefs.SetInt("SA_TrainData_Data_level_trainnumber_00", level_trainnumber_00);
        PlayerPrefs.SetInt("SA_TrainData_Data_level_trainnumber_10", level_trainnumber_10);
        PlayerPrefs.SetInt("SA_TrainData_Data_level_trainnumber_20", level_trainnumber_20);
        PlayerPrefs.SetInt("SA_TrainData_Data_level_trainnumber_30", level_trainnumber_30);
        PlayerPrefs.SetInt("SA_TrainData_Data_level_trainnumber_40", level_trainnumber_40);
    }

    public void Load()
    {
        train_num = ES3.Load<List<int>>("SA_TrainData_Data_Train_Num");
        train_buy_num = ES3.Load<List<int>>("SA_TrainData_Data_Train_Buy_Num");
        level_train_enginetier = PlayerPrefs.GetInt("SA_TrainData_Data_level_train_enginetier");
        level_train_maxspeed =  PlayerPrefs.GetInt("SA_TrainData_Data_level_train_maxspeed");
        level_train_armor = PlayerPrefs.GetInt("SA_TrainData_Data_level_train_armor");
        level_train_efficient =  PlayerPrefs.GetInt("SA_TrainData_Data_level_train_efficient");
        level_trainnumber_00 = PlayerPrefs.GetInt("SA_TrainData_Data_level_trainnumber_00");
        level_trainnumber_10 = PlayerPrefs.GetInt("SA_TrainData_Data_level_trainnumber_10");
        level_trainnumber_20 = PlayerPrefs.GetInt("SA_TrainData_Data_level_trainnumber_20");
        level_trainnumber_30 = PlayerPrefs.GetInt("SA_TrainData_Data_level_trainnumber_30");
        level_trainnumber_40 = PlayerPrefs.GetInt("SA_TrainData_Data_level_trainnumber_40");
    }

    public void Init()
    {
        train_num.Clear();
        train_num.Add(0);
        train_num.Add(10);
        train_buy_num.Clear();

        level_train_enginetier = 0;
        level_train_maxspeed = 0;
        level_train_armor = 0;
        level_train_efficient = 0;
        level_trainnumber_00 = 0;
        level_trainnumber_10 = 10;
        level_trainnumber_20 = 20;
        level_trainnumber_30 = 30;
        level_trainnumber_40 = 40;
        Save();
    }
}