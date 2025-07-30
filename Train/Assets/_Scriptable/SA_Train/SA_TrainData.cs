using System;
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
    private int level_train_maxtrain;
    public int Level_Train_MaxTrain { get {  return level_train_maxtrain; } }

    [SerializeField]
    private int level_train_maxmercenary;
    public int Level_Train_MaxMercenary { get { return level_train_maxmercenary;} }

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
    [SerializeField]
    private int level_trainnumber_50;
    public int Level_TrainNumber_60 { get  { return level_trainnumber_50; } }

    public void SA_Passive_Level_Up(int LevelNum) 
    {
        //LevelNum : 0 = Tier / 1 = train / 2 = mercenary / 3 = maxspeed
        //4 = 방어력 / 5 = 연료 효율성
        switch (LevelNum)
        {
            case (0):
                level_train_enginetier++;
                  Save();
                break;
            case (1):
                level_train_maxtrain++;
                Save(); break;
            case (2):
                level_train_maxmercenary++;
                Save(); break;
            case (3):
                level_train_maxspeed++;
                 Save();
                break;
            case (4):
                level_train_armor++;
                 Save();
                break;
            case (5):
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
            case 5:
                level_trainnumber_50++;
                break;
        }
        Save();
    }

    public int SA_TrainChangeNum(int trainNum)
    {
        if(trainNum >= 90)
        {
            return trainNum;
        }
        else
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
                case 5:
                    return level_trainnumber_50;
                default:
                    return -1;
            }
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

    public void SA_TrainUpgrade_Renewal_2(int TrainNum)
    {
        for (int i = 0; i < Train_Num.Count; i++)
        {
            if (Train_Num[i] == TrainNum)
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
        ES3.Save<int>("SA_TrainData_Data_level_train_enginetier", level_train_enginetier);
        ES3.Save<int>("SA_TrainData_Data_level_train_maxtrain", level_train_maxtrain);
        ES3.Save<int>("SA_TrainData_Data_level_train_maxmercenary", level_train_maxmercenary);
        ES3.Save<int>("SA_TrainData_Data_level_train_maxspeed", level_train_maxspeed);
        ES3.Save<int>("SA_TrainData_Data_level_train_armor", level_train_armor);
        ES3.Save<int>("SA_TrainData_Data_level_train_efficient", level_train_efficient);
        ES3.Save("SA_TrainData_Data_Train_Buy_Num", train_buy_num);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_00", level_trainnumber_00);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_10", level_trainnumber_10);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_20", level_trainnumber_20);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_30", level_trainnumber_30);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_40", level_trainnumber_40);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_50", level_trainnumber_50);
    }

    private IEnumerator SaveSync()
    {
        ES3.Save("SA_TrainData_Data_Train_Num", train_num);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_train_enginetier", level_train_enginetier);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_train_maxtrain", level_train_maxtrain);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_train_maxmercenary", level_train_maxmercenary);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_train_maxspeed", level_train_maxspeed);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_train_armor", level_train_armor);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_train_efficient", level_train_efficient);
        yield return new WaitForSeconds(0.001f);
        ES3.Save("SA_TrainData_Data_Train_Buy_Num", train_buy_num);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_00", level_trainnumber_00);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_10", level_trainnumber_10);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_20", level_trainnumber_20);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_30", level_trainnumber_30);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_40", level_trainnumber_40);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_TrainData_Data_level_trainnumber_50", level_trainnumber_50);
        yield return new WaitForSeconds(0.001f);
    }

    public void Load()
    {
        train_num = ES3.Load<List<int>>("SA_TrainData_Data_Train_Num");
        train_buy_num = ES3.Load<List<int>>("SA_TrainData_Data_Train_Buy_Num");
        level_train_enginetier = ES3.Load<int>("SA_TrainData_Data_level_train_enginetier");
        level_train_maxtrain = ES3.Load<int>("SA_TrainData_Data_level_train_maxtrain");
        level_train_maxmercenary = ES3.Load<int>("SA_TrainData_Data_level_train_maxmercenary");
        level_train_maxspeed = ES3.Load<int>("SA_TrainData_Data_level_train_maxspeed");
        level_train_armor = ES3.Load<int>("SA_TrainData_Data_level_train_armor");
        level_train_efficient = ES3.Load<int>("SA_TrainData_Data_level_train_efficient");
        level_trainnumber_00 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_00");
        level_trainnumber_10 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_10");
        level_trainnumber_20 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_20");
        level_trainnumber_30 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_30");
        level_trainnumber_40 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_40");
        level_trainnumber_50 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_50");
    }

    public IEnumerator LoadSync()
    {
        train_num = ES3.Load<List<int>>("SA_TrainData_Data_Train_Num", new List<int> { 0, 10 });
        train_buy_num = ES3.Load<List<int>>("SA_TrainData_Data_Train_Buy_Num", new List<int> { });
        level_train_enginetier = ES3.Load<int>("SA_TrainData_Data_level_train_enginetier",0);
        level_train_maxtrain = ES3.Load<int>("SA_TrainData_Data_level_train_maxtrain", 0);
        level_train_maxmercenary = ES3.Load<int>("SA_TrainData_Data_level_train_maxmercenary", 0);
        level_train_maxspeed = ES3.Load<int>("SA_TrainData_Data_level_train_maxspeed", 0);
        level_train_armor = ES3.Load<int>("SA_TrainData_Data_level_train_armor", 0);
        level_train_efficient = ES3.Load<int>("SA_TrainData_Data_level_train_efficient", 0);
        level_trainnumber_00 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_00", 0);
        level_trainnumber_10 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_10", 10);
        level_trainnumber_20 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_20", 20);
        level_trainnumber_30 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_30", 30);
        level_trainnumber_40 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_40", 40);
        level_trainnumber_50 = ES3.Load<int>("SA_TrainData_Data_level_trainnumber_50", 50);
        yield return new WaitForSeconds(0.001f);
    }

    public void Init()
    {
        train_num.Clear();
        train_num.Add(0);
        train_num.Add(10);
        train_buy_num.Clear();

        level_train_enginetier = 0;
        level_train_maxtrain = 0;
        level_train_maxmercenary = 0;
        level_train_maxspeed = 0;
        level_train_armor = 0;
        level_train_efficient = 0;
        level_trainnumber_00 = 0;
        level_trainnumber_10 = 10;
        level_trainnumber_20 = 20;
        level_trainnumber_30 = 30;
        level_trainnumber_40 = 40;
        level_trainnumber_50 = 50;
        Save();
    }

    public IEnumerator InitAsync(MonoBehaviour runner)
    {
        train_num.Clear();
        train_num.Add(0);
        train_num.Add(10);
        train_buy_num.Clear();
        level_train_enginetier = 0;
        level_train_maxtrain = 0;
        level_train_maxmercenary = 0;
        level_train_maxspeed = 0;
        level_train_armor = 0;
        level_train_efficient = 0;
        level_trainnumber_00 = 0;
        level_trainnumber_10 = 10;
        level_trainnumber_20 = 20;
        level_trainnumber_30 = 30;
        level_trainnumber_40 = 40;
        level_trainnumber_50 = 50;
        runner.StartCoroutine(SaveSync());
        yield return new WaitForSeconds(0.01f);
    }
}