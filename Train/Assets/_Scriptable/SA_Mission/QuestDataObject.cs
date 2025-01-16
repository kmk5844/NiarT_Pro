using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionDataObject : ScriptableObject
{
    [SerializeField]
    private int mission_num;
    public int Mission_Num {  get { return mission_num; } }

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
    private bool startstageflag;
    public bool StartStageFlag {  get {  return startstageflag; } }

    [SerializeField]
    private bool nextstageflag;
    public bool NextStageFlag {  get { return nextstageflag; } }

    [SerializeField]
    private string substage_status;
    public string SubStage_Status { get { return substage_status; } }

    [SerializeField]
    private bool stageclearflag;
    public bool StageClearFlag {  get { return stageclearflag; } }

    [SerializeField]
    private bool stageopenflag;
    public bool StageOpenFlag { get { return stageopenflag; } }

    public void Auto_SubStage_Insert(
        int _mission_num,
        int _stage_num, int _substage_num, SubStageType _substage_type,
        int _distance, string _emerging_monster, string _monster_count,
        string  _open_substagenum, string _substage_status, bool _stagestartflag, bool _nextstageflag)
    {
        mission_num = _mission_num;
        stage_num = _stage_num;
        substage_num = _substage_num;
        substage_type = _substage_type;
        distance = _distance;
        emerging_monster = _emerging_monster;
        monster_count = _monster_count;
        open_substagenum = _open_substagenum;
        substage_status = _substage_status;
        startstageflag = _stagestartflag;
        nextstageflag = _nextstageflag;

        if (_stagestartflag)
        {
            stageopenflag = true;
        }
        else
        {
            stageopenflag = false;
        }

        Save();
    }

    public void SubStage_Clear()
    {
        stageclearflag = true;
        Save();
    }

    public void SubStageLockOff()
    {
        stageopenflag = true;
        Save();
    }

    public void Init()
    {
        stageclearflag = false;
        if (startstageflag)
        {
            stageopenflag = true;
        }
        else
        {
            stageopenflag = false;
        }
        Save();
    }

    void Save()
    {
        ES3.Save<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_ClearFlag", stageclearflag);
        ES3.Save<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_OpenFlag", stageopenflag);
    }

    public void Load()
    {
        stageclearflag = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_ClearFlag");
        stageopenflag = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_OpenFlag");
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