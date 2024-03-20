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
                break;
            case (1):
                level_train_maxspeed++;
                break;
            case (2):
                level_train_armor++;
                break;
            case (3):
                level_train_efficient++;
                break;
        }
    }

    public void TrainUpgrade(int trainNum)
    {
        switch(trainNum%10)
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
    }
}
