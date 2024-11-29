using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDataObject : ScriptableObject
{
    [SerializeField]
    private int stage_num;
    public int Stage_Num { get { return stage_num; } }
    
    [SerializeField]
    private int substage_num;
    public int SubStage_Num {  get { return substage_num; } }

    [SerializeField]
    private SubStageType substage_type;
    public SubStageType SubStage_Type { get { return substage_type; } }

    [SerializeField]
    private int distance;
    public int Distance { get { return distance; } }

    [SerializeField]
    private string emerging_monster;
    public string Emerging_Monster { get { return emerging_monster; } }

    [SerializeField]
    private string monster_count;
    public string Monster_Count { get { return monster_count; } }

    [SerializeField]
    private string open_substagenum;
    public string Open_SubStageNum { get { return open_substagenum; } }

    [SerializeField]
    private string substage_status;
    public string SubStage_Status { get { return substage_status; } }

    public void Auto_SubStage_Insert(
        int _stage_num, int _substage_num, SubStageType _substage_type,
        int _distance, string _emerging_monster, string _monster_count,
        string  _open_substagenum, string _substage_status)
    {
        stage_num = _stage_num;
        substage_num = _substage_num;
        substage_type = _substage_type;
        distance = _distance;
        emerging_monster = _emerging_monster;
        monster_count = _monster_count;
        open_substagenum = _open_substagenum;
        substage_status = _substage_status;
    }
}

public enum SubStageType
{
    Nomal,
    Hard,
    HardCore,
    Boss,
    Oasis,
    Treasure,
    Store,
    Maintenance,
    SimpleStation,
    Error
}