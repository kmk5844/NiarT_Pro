using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_MissionData", menuName = "Scriptable/MissionData/main", order = 11)]
public class SA_MissionData : ScriptableObject
{
    [SerializeField]
    List<MissionList> stagelist;
    public List<MissionList> StageList { get { return stagelist; } }

    [SerializeField]
    List<bool> mainstage_clearflag;
    public List<bool> MainStage_ClearFlag {  get { return mainstage_clearflag; } }


    public void Editor_MissionList_Init(int maxStage)
    {
        stagelist.Clear();
        mainstage_clearflag.Clear();
        for (int i = 0; i < maxStage; i++)
        {
            MissionList newMissionList = new MissionList
            {
                mainstageNum = i,
                CelarFlag = false,
                Q_Des = new List<MissionDataObject>(),
                Q_Mat = new List<MissionDataObject>(),
                Q_Mon = new List<MissionDataObject>(),
                Q_Esc = new List<MissionDataObject>(),
                Q_Con = new List<MissionDataObject>(),
                Q_Bos = new List<MissionDataObject>(),
            };
            stagelist.Add(newMissionList);
            mainstage_clearflag.Add(false);
        }
    }

    public void Add_List(int MainStageNum, int missionNum, MissionDataObject mission)
    {
        var missionList = stagelist.Find(x => x.mainstageNum == MainStageNum);

        if(missionList.mainstageNum == MainStageNum)
        {
            if(missionNum == 0)
            {
                missionList.Q_Des.Add(mission);
            }else if(missionNum == 1)
            {
                missionList.Q_Mat.Add(mission);
            }
            else if (missionNum == 2)
            {
                missionList.Q_Mon.Add(mission);
            }
            else if (missionNum == 3)
            {
                missionList.Q_Esc.Add(mission);
            }
            else if (missionNum == 4)
            {
                missionList.Q_Con.Add(mission);
            }
            else if (missionNum == 5)
            {
                missionList.Q_Bos.Add(mission);
            }
        }
    }

    public MissionDataObject missionStage(int missionNum, int StageNum, int SubStageNum)
    {
        var missionList = stagelist.Find(x => x.mainstageNum == StageNum);
        if (missionNum == 0)
        {
            return missionList.Q_Des[SubStageNum];
        }
        else if (missionNum == 1)
        {
            return missionList.Q_Mat[SubStageNum];
        }
        else if (missionNum == 2)
        {
            return missionList.Q_Mon[SubStageNum];
        }
        else if (missionNum == 3)
        {
            return missionList.Q_Esc[SubStageNum];
        }
        else if (missionNum == 4)
        {
            return missionList.Q_Con[SubStageNum];
        }
        else if (missionNum == 5)
        {
            return missionList.Q_Bos[SubStageNum];
        }
        return null;
    }

    public void End_SubStage(int MainStageNum)
    {
        mainstage_clearflag[MainStageNum] = true;
        Save();
    }

    void Init()
    {
        
    }

    void Save()
    {

    }

    void Load()
    {

    }
}

[System.Serializable]
public struct MissionList
{
    public int mainstageNum;
    public bool CelarFlag;
    public List<MissionDataObject> Q_Des;
    public List<MissionDataObject> Q_Mat;
    public List<MissionDataObject> Q_Mon;
    public List<MissionDataObject> Q_Esc;
    public List<MissionDataObject> Q_Con;
    public List<MissionDataObject> Q_Bos;
}