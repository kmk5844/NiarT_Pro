using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionDataObject : ScriptableObject
{
    [SerializeField]
    private bool missiondatause;
    public bool MissionDataUse { get { return missiondatause; } }

    [SerializeField]
    private int mission_num;
    public int Mission_Num { get { return mission_num; } }

    [SerializeField]
    private int stage_num;
    public int Stage_Num { get { return stage_num; } }

    [SerializeField]
    private int substage_num;
    public int SubStage_Num { get { return substage_num; } }

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
    public bool StartStageFlag { get { return startstageflag; } }

    [SerializeField]
    private bool nextstageflag;
    public bool NextStageFlag { get { return nextstageflag; } }

    [SerializeField]
    private int wavecount;
    public int Wave_Count { get { return wavecount; } }

    [SerializeField]
    private string substage_status;
    public string SubStage_Status { get { return substage_status; } }

    [SerializeField]
    private bool readyflag; // 스테이지 : true / 특수스테이지 : false
    public bool ReadyFlag { get { return readyflag; } }

/*    [SerializeField]
    private bool lockflag;
    public bool LockFlag { get { return lockflag; } }*/

    [SerializeField]
    private bool stageclearflag;
    public bool StageClearFlag { get { return stageclearflag; } }

    [SerializeField]
    private bool stageopenflag;
    public bool StageOpenFlag { get { return stageopenflag; } }

    [SerializeField]
    private bool storyflag;
    public bool StoryFlag { get { return storyflag; } }
    [SerializeField]
    private string storyStatus;
    public string StoryStatus { get { return storyStatus; } }

    public void Auto_SubStage_Insert(
        int _mission_num,
        int _stage_num, int _substage_num, SubStageType _substage_type,
        int _distance, string _emerging_monster, string _monster_count,
        string _open_substagenum, string _substage_status, bool _stagestartflag, bool _nextstageflag, int _wavecount
        ,bool _storyflag = false, string _storyStatus = "")
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
        wavecount = _wavecount;
        storyflag = _storyflag;
        storyStatus = _storyStatus;
        CheckReadyFlag();
        if (_stagestartflag)
        {
            stageopenflag = true;
        }
        else
        {
            stageopenflag = false;
        }
        Save(true);
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

    public void SubStageLockOn()
    {
        stageopenflag = false;
        Save();
    }

    /*    public void prevLock()
        {
            lockflag = true;
            Save();
        }*/

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
        Save(true);
    }

    public void InitSync(MonoBehaviour runner)
    {
        //lockflag = false;
        stageclearflag = false;
        if (startstageflag)
        {
            stageopenflag = true;
        }
        else
        {
            stageopenflag = false;
        }
        runner.StartCoroutine(SaveSync(true));
    }

    void Save(bool Init = false)
    {
        if(Init == true)
        {
            missiondatause = false;
        }
        else
        {
            missiondatause = true;
        }
        ES3.Save<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_MissionDataUse", missiondatause);
        //ES3.Save<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_LockFlag", lockflag);
        ES3.Save<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_ClearFlag", stageclearflag);
        ES3.Save<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_OpenFlag", stageopenflag);
    }

    IEnumerator SaveSync(bool Init = false)
    {
        if (Init == true)
        {
            missiondatause = false;
        }
        else
        {
            missiondatause = true;
        }
        ES3.Save<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_MissionDataUse", missiondatause);
        yield return new WaitForSeconds(0.001f);
        //ES3.Save<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_LockFlag", lockflag);
        //yield return new WaitForSeconds(0.001f);
        ES3.Save<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_ClearFlag", stageclearflag);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_OpenFlag", stageopenflag);
        yield return new WaitForSeconds(0.001f);
    }


    public void Load()
    {
        missiondatause = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_MissionDataUse");
        //lockflag = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_LockFlag");
        stageclearflag = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_ClearFlag");
        stageopenflag = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_OpenFlag");
    }

    public void LoadSync_Start(MonoBehaviour runner)
    {
        runner.StartCoroutine(LoadSync());
    }

    public bool Load_missionDataUse()
    {
        if(ES3.KeyExists("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_MissionDataUse"))
        {
            missiondatause = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_MissionDataUse");
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator LoadSync()
    {
        //missiondatause = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_MissionDataUse");
        //yield return new WaitForSeconds(0.001f);
        //lockflag = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_LockFlag");
        //yield return new WaitForSeconds(0.001f);
        if (startstageflag)
        {
            stageopenflag = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_OpenFlag", true);
        }
        else
        {
            stageopenflag = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_OpenFlag", false);
        }
        stageclearflag = ES3.Load<bool>("QDO_SubStage_" + mission_num + "_" + stage_num + "_" + substage_num + "_DataObject_ClearFlag",false);
        yield return new WaitForSeconds(0.001f);
    }

    void CheckReadyFlag()
    {
        switch (substage_type)
        {
            case SubStageType.Normal:
                readyflag = true;
                break;
            case SubStageType.Hard:
                readyflag = true;
                break;
            case SubStageType.HardCore:
                readyflag = true;
                break;
            case SubStageType.Boss:
                readyflag = true;
                break;
            case SubStageType.SimpleStation:
                readyflag = false;
                break;
            case SubStageType.Special:
                readyflag = false;
                break;
        }
    }
}

public enum SubStageType
{
    Normal,
    Hard,
    HardCore,
    Boss,
    Special,
    Store,
    Maintenance,
    SimpleStation,
    Error
}