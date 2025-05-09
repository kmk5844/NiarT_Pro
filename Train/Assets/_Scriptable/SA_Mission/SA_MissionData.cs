using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    int materialcount;
    public int MaterialCount { get {  return materialcount; } }

    [SerializeField]
    int monstercount;
    public int MonsterCount { get {  return monstercount; } }

    [SerializeField]
    int bosscount;
    public int BossCount { get { return bosscount; } }

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

    public void SubStage_Init(int MainStageNum, int MissionNum)
    {
        //Debug.Log(MainStageNum + " , "+ MissionNum);
        switch (MissionNum)
        {
            case 0:
                foreach(MissionDataObject mission in stagelist[MainStageNum].Q_Des)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.Init();
                    }
                }
                break;
            case 1:
                foreach (MissionDataObject mission in stagelist[MainStageNum].Q_Mat)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.Init();
                    }
                }
                break;
            case 2:
                foreach (MissionDataObject mission in stagelist[MainStageNum].Q_Mon)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.Init();
                    }
                }
                break;
            case 3:
                foreach (MissionDataObject mission in stagelist[MainStageNum].Q_Esc)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.Init();
                    }
                }
                break;
            case 4:
                foreach (MissionDataObject mission in stagelist[MainStageNum].Q_Con)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.Init();
                    }
                }
                break;
            case 5:
                foreach (MissionDataObject mission in stagelist[MainStageNum].Q_Bos)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.Init();
                    }
                }
                break;
        }
    }

    public void Init()
    {
        for (int i = 0; i < stagelist.Count; i++)
        {
            mainstage_clearflag[i] = false;
            foreach (MissionDataObject mission in stagelist[i].Q_Des)
            {
                if (mission.MissionDataUse)
                {
                    mission.Init();
                }
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Mat)
            {
                if (mission.MissionDataUse)
                {
                    mission.Init();
                }
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Mon)
            {
                if (mission.MissionDataUse)
                {
                    mission.Init();
                }
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Esc)
            {
                if (mission.MissionDataUse)
                {
                    mission.Init();
                }
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Con)
            {
                if (mission.MissionDataUse)
                {
                    mission.Init();
                }
            }
            foreach (MissionDataObject mission in stagelist[i].Q_Bos)
            {
                if (mission.MissionDataUse)
                {
                    mission.Init();
                }
            }
            Save(i);
        }
        monstercount = -1;
        bosscount = -1;
        foreach (MissionType mission in Enum.GetValues(typeof(MissionType)))
        {
            SelectMission_Save(mission);
        }
    }

    public IEnumerator InitAsync(MonoBehaviour runner)
    {
        for(int i = 0; i < stagelist.Count; i++) //실행 전, 데이터 체크하고, 진행 (삭제 금지)
        {
            if (!ES3.KeyExists("SA_MissionData_" + i + "_clearData"))
            {
                Save(i);
            }
        }

        for (int i = 0; i < stagelist.Count; i++)
        {
            if (mainstage_clearflag[i])
            {
                mainstage_clearflag[i] = false;
            }
            else
            {
                foreach (MissionDataObject mission in stagelist[i].Q_Des)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.InitSync(runner);
                    }
                    yield return null;
                }
                foreach (MissionDataObject mission in stagelist[i].Q_Mat)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.InitSync(runner);
                    }
                    yield return null;
                }
                foreach (MissionDataObject mission in stagelist[i].Q_Mon)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.InitSync(runner);
                    }
                    yield return null;
                }
                foreach (MissionDataObject mission in stagelist[i].Q_Esc)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.InitSync(runner);
                    }
                    yield return null;
                }
                foreach (MissionDataObject mission in stagelist[i].Q_Con)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.InitSync(runner);
                    }
                    yield return null;
                }
                foreach (MissionDataObject mission in stagelist[i].Q_Bos)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.InitSync(runner);
                    }
                    yield return null;
                }
                Debug.Log("전체 초기화 종료 :" + i);
                break;
            }
            Save(i);
            yield return null;
        }
        monstercount = -1;
        bosscount = -1;
        foreach (MissionType mission in Enum.GetValues(typeof(MissionType)))
        {
            SelectMission_Save(mission);
        }
        yield return null;
    }

    void Save(int i)
    {
        ES3.Save<bool>("SA_MissionData_" + i + "_clearData", mainstage_clearflag[i]);
    }

    public void SelectMission_SetData(MissionType mission, int num)
    {
        if (mission == MissionType.Monster)
        {
            monstercount = num;
        }
        else if (mission == MissionType.Boss)
        {
            bosscount = num;
        }
    }

    public void SelectMission_Save(MissionType mission)
    {
        if (mission == MissionType.Monster)
        {
            ES3.Save<int>("SelectMission_MonsterCount", monstercount);
            //Debug.Log("몬스터 카운트 저장 완료!");
        }
        else if (mission == MissionType.Boss)
        {
            ES3.Save<int>("SelectMission_BossCount", bosscount);
            //Debug.Log("보스 카운트 저장 완료!");
        }
    }

    public void SelectMission_Load(MissionType mission)
    {
        if (mission == MissionType.Monster)
        {
            monstercount = ES3.Load<int>("SelectMission_MonsterCount");
        }
        else if (mission == MissionType.Boss)
        {
            bosscount = ES3.Load<int>("SelectMission_BossCount");
        }
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

    public IEnumerator LoadSync(MonoBehaviour runner)
    {
        SA_PlayerData playerData = DataManager.Instance.playerData;
        int selectNum = playerData.Select_Stage;

        for (int i = 0; i < stagelist.Count; i++)
        {
            mainstage_clearflag[i] = ES3.Load<bool>("SA_MissionData_" + i + "_clearData");

            if (mainstage_clearflag[i] == false)
            {
                break;
            }

            if (i % 5 == 0)
            {
                yield return new WaitForSeconds(0.001f);
            }
        }

        if (playerData.Mission_Playing)
        {
            foreach (MissionDataObject mission in stagelist[selectNum].Q_Des)
            {
                bool flag = mission.Load_missionDataUse();
                if (flag)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.LoadSync_Start(runner);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                yield return null;
            }
            foreach (MissionDataObject mission in stagelist[selectNum].Q_Mat)
            {
                bool flag = mission.Load_missionDataUse();
                if (flag)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.LoadSync_Start(runner);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                yield return null;
            }
            foreach (MissionDataObject mission in stagelist[selectNum].Q_Mon)
            {
                bool flag = mission.Load_missionDataUse();
                if (flag)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.LoadSync_Start(runner);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                yield return null;
            }
            foreach (MissionDataObject mission in stagelist[selectNum].Q_Esc)
            {
                bool flag = mission.Load_missionDataUse();
                if (flag)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.LoadSync_Start(runner);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                yield return null;
            }
            foreach (MissionDataObject mission in stagelist[selectNum].Q_Con)
            {
                bool flag = mission.Load_missionDataUse();
                if (flag)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.LoadSync_Start(runner);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                yield return null;
            }
            foreach (MissionDataObject mission in stagelist[selectNum].Q_Bos)
            {
                bool flag = mission.Load_missionDataUse();
                if (flag)
                {
                    if (mission.MissionDataUse)
                    {
                        mission.LoadSync_Start(runner);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                yield return null;
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