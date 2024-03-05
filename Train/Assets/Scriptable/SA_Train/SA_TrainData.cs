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
}
