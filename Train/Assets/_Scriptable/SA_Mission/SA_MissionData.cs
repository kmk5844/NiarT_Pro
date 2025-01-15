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
    public List<bool> MainStage_ClearFlag { get { return mainstage_clearflag; } }


    public void Editor_MissionList_Init(int maxStage)
    {
        stagelist.Clear();
        mainstage_clearflag.Clear();
        for (int i = 0; i < maxStage; i++)
        {
            MissionList newMissionList = new MissionList
            {
                mainstageNum = i,
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
        Save(MainStageNum);
    }

    public void SubStage_Lose(int MainStageNum, int MissionNum)
    {
        switch(MissionNum)
        {
            case 0:
                foreach(MissionDataObject mission in stagelist[MainStageNum].Q_Des)
                {
                    mission.Init();
                }
                break;
            case 1:
                foreach (MissionDataObject mission in stagelist[MainStageNum].Q_Mat)
                {
                    mission.Init();
                }
                break;
            case 2:
                foreach (MissionDataObject mission in stagelist[MainStageNum].Q_Mon)
                {
                    mission.Init();
                }
                break;
            case 3:
                foreach (MissionDataObject mission in stagelist[MainStageNum].Q_Esc)
                {
                    mission.Init();
                }
                break;
            case 4:
                foreach (MissionDataObject mission in stagelist[MainStageNum].Q_Con)
                {
                    mission.Init();
                }
                break;
            case 5:
                foreach (MissionDataObject mission in stagelist[MainStageNum].Q_Bos)
                {
                    mission.Init();
                }
                break;
        }
    }

    public void Init()
    {
        for(int i = 0; i < stagelist.Count; i++)
        {
            mainstage_clearflag[i] = false;
            foreach (MissionDataObject mission in stagelist[i].Q_Des)
            {
                mission.Init();
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Mat)
            {
                mission.Init();
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Mon)
            {
                mission.Init();
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Esc)
            {
                mission.Init();
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Con)
            {
                mission.Init();
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Bos)
            {
                mission.Init();
            }
            Save(i);
        }
    }

    void Save(int i)
    {
        ES3.Save<bool>("SA_MissionData_" + i + "_clearData", mainstage_clearflag[i]);
    }

    public void Load()
    {
        for(int i = 0; i < stagelist.Count; i++)
        {
            mainstage_clearflag[i] = ES3.Load<bool>("SA_MissionData_" + i + "_clearData");
            foreach(MissionDataObject mission in stagelist[i].Q_Des)
            {
                mission.Load();
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Mat)
            {
                mission.Load();
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Mon)
            {
                mission.Load();
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Esc)
            {
                mission.Load();
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Con)
            {
                mission.Load();
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Bos)
            {
                mission.Load();
            }
        }
    }
}

[System.Serializable]
public struct MissionList
{
    public int mainstageNum;
    public List<MissionDataObject> Q_Des;
    public List<MissionDataObject> Q_Mat;
    public List<MissionDataObject> Q_Mon;
    public List<MissionDataObject> Q_Esc;
    public List<MissionDataObject> Q_Con;
    public List<MissionDataObject> Q_Bos;
}