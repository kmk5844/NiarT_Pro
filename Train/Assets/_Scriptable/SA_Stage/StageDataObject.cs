using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataObject : ScriptableObject
{
    [SerializeField]
    private bool stagedatause;
    public bool StageDataUse {  get { return stagedatause; } }

    [SerializeField]
    private int stage_num;
    public int Stage_Num { get { return stage_num; } }

    [SerializeField]
    private List<int> missionlist;
    public List<int> MissionList {  get { return missionlist; } }

    [SerializeField]
    bool stage_clearflag;
    public bool Stage_ClearFlag {  get {  return stage_clearflag; } }

    [SerializeField]
    bool stage_openflag;
    public bool Stage_OpenFlag { get { return stage_openflag; } }

    public void Auto_Stage_Insert(int _stage_num, string _missionList)
    {
        stage_num = _stage_num;
        
        string[] _missionList_Split = _missionList.Split(',');
        missionlist = new List<int>();
        for (int i = 0; i < _missionList_Split.Length; i++)
        {
            int k = int.Parse(_missionList_Split[i]);
            missionlist.Add(k);
        }

        stage_clearflag = false;
        Save(true);
    }

    public void Clear_StageChage()
    {
        if (!stage_clearflag)
        {
            stage_clearflag = true;
        }
        Save();
    }

    public void Open_StageChange()
    {
        if (!stage_openflag)
        {
            stage_openflag = true;
        }
        Save();
    }
    public void Save(bool Init = false)
    {
        if(Init == true)
        {
            stagedatause = false;
        }
        else
        {
            stagedatause = true;
        }
        ES3.Save<bool>("Stage_" + stage_num + "_stage_stagedatause", stagedatause);
        ES3.Save<bool>("Stage_" + stage_num + "_stage_openflag", stage_openflag);
        ES3.Save<bool>("Stage_" + stage_num + "_stage_clearflag", stage_clearflag);
    }

    public void Load()
    {
        stagedatause = ES3.Load<bool>("Stage_" + stage_num + "_stage_stagedatause");
        stage_openflag = ES3.Load<bool>("Stage_" + stage_num + "_stage_openflag");
        stage_clearflag = ES3.Load<bool>("Stage_" + stage_num + "_stage_clearflag");
    }
    
    public void Init()
    {
        stage_clearflag = false;
        if (stage_num == 0)
        {
            stage_openflag = true;
        }
        Save(true);
    }
}
