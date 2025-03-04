using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMission : MonoBehaviour
{
    [SerializeField]
    SA_PlayerData playerData;
    [SerializeField]
    Quest_DataTable missionDataTable;
    [SerializeField]
    SA_MissionData missionData;

    int stageNum;
    int missionNum;
    string FindString;

    [SerializeField]
    public string StageMission;
    string MissionType_String;
    [SerializeField]
    public MissionType MissionType;
    [SerializeField]
    public string MissionInformation;
    [SerializeField]
    public string MissionState;
    [SerializeField]
    public int MissionReward;

    public MissionMaterial_State M_Material;
    public MissionMonster_State M_Monster;
    public MissionEscort_State M_Escort;
    public MissionConvoy_State M_Convoy;
    public MissionBoss_State M_Boss;

    public int monsterCount;
    public int bossCount;
    public int Select_Train_Weight;

    [Header("stageSelect")]
    public GameObject StageInitButton;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        stageNum = playerData.Select_Stage;
        missionNum = playerData.Mission_Num;
        FindString = stageNum + ","+ missionNum;
        SetMissionSetting();
        SetMissionType();
        SetMissionState();
        Load(MissionType);
    }

    public void SetDataSetting(SA_PlayerData _playerData, Quest_DataTable _missionDataTable, SA_MissionData _missionData)
    {
        playerData = _playerData;
        missionDataTable = _missionDataTable;
        missionData = _missionData;
    }

    void SetMissionSetting()
    {
        int index = 0;
        for (int i = 0; i < missionDataTable.Q_List.Count; i++)
        {
            index = i;
            if (missionDataTable.Q_List[i].Stage_Mission.Equals(FindString))
            {
                break;
            }
        }
        StageMission = missionDataTable.Q_List[index].Stage_Mission;
        MissionType_String = missionDataTable.Q_List[index].Quest_Type;
        MissionInformation = missionDataTable.Q_List[index].Quest_Information;
        MissionState = missionDataTable.Q_List[index].Quest_State;
        MissionReward = missionDataTable.Q_List[index].Quest_Reward;
    }

    void SetMissionType()
    {
        switch (MissionType_String)
        {
            case "Destination":
                MissionType = MissionType.Destination;
                break;   
            case "Material":
                MissionType = MissionType.Material;
                break;     
            case "Monster":
                MissionType = MissionType.Monster;
                break;   
            case "Escort":
                MissionType = MissionType.Escort;
                break;       
            case "Convoy":
                MissionType = MissionType.Convoy;
                break;     
            case "Boss":
                MissionType = MissionType.Boss;
                break;
        }
    }

    public bool CheckMission(bool LastFlag)
    {
        if (!LastFlag) // 마지막이 아닐 때
        {
            if (MissionType == MissionType.Destination)
            {
                //Debug.Log("목적지 무사히 달성하기");
                return true;
            }
            else if (MissionType == MissionType.Material)
            {
                Debug.Log("재료 모으기");
                //재료 카운트 추가
                return true;
            }
            else if (MissionType == MissionType.Monster)
            {
                //문제 없음.
                return true;
            }
            else if (MissionType == MissionType.Escort)
            {
                Debug.Log("호위하기");
                //진행 -> 게임 디렉터에서 체크할 예정
                return true;
            }
            else if (MissionType == MissionType.Convoy)
            {
                Debug.Log("호송하기");
                //진행 -> 게임 디렉터에서 체크할 예정
                return true;
            }
            else if (MissionType == MissionType.Boss)
            {
                //문제 없음
                return true;
            }
        }
        else // 마지막일 때
        {
            if (MissionType == MissionType.Destination)
            {
                //조건 없이 도착
                return true;
            }
            else if (MissionType == MissionType.Material)
            {
                //재료 체크 후 bool 체크
                return true;
            }
            else if (MissionType == MissionType.Monster)
            {
                if(monsterCount >= M_Monster.MonsterCount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //몬스터 체크 후 bool 체크
            }
            else if (MissionType == MissionType.Escort)
            {
                //만약 게임 도중에 부서지거나 망가지면 게임디렉터에서 패배 확정
                //문제가 없다면 true로 계속 진행
                return true;
            }
            else if (MissionType == MissionType.Convoy)
            {                
                //만약 게임 도중에 부서지거나 망가지면 게임디렉터에서 패배 확정
                //문제가 없다면 true로 계속 진행
                return true;
            }
            else if (MissionType == MissionType.Boss)
            {
                if(bossCount >= M_Boss.BossCount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    void SetMissionState()
    {
        string[] _state = MissionState.Split(',');
        switch (MissionType)
        {
            case MissionType.Destination:
                break;
            case MissionType.Material:
                int _num = int.Parse(_state[0]);
                int _count = int.Parse(_state[1]);
                M_Material.SetSetting(_num, _count);
                break;
            case MissionType.Monster:
                _num = int.Parse(_state[0]);
                _count = int.Parse(_state[1]);
                M_Monster.SetSetting(_num, _count);
                break;
            case MissionType.Escort:
                _num = int.Parse(_state[0]);
                int _hp = int.Parse(_state[1]);
                M_Escort.SetSetting(_num, _hp);
                break;
            case MissionType.Convoy:

                break;
            case MissionType.Boss:
                _num = int.Parse(_state[0]);
                _count = int.Parse(_state[1]);
                M_Boss.SetSetting(_num, _count);
                break;
        }
    }

    public void Mission_Sucesses()
    {
        playerData.SA_Get_Coin(MissionReward);
        MissionEnd(MissionType);
        Destroy(this.gameObject);
    }

    public void Mission_Fail()
    {
        MissionEnd(MissionType);
        Destroy(this.gameObject);
    }

    [Serializable]
    public struct MissionMaterial_State
    {
        public int ItemNum;
        public int ItemCount;

        public void SetSetting(int _num, int _count)
        {
            ItemNum = _num;
            ItemCount = _count;
        }
    }

    [Serializable]
    public struct MissionMonster_State
    {
        public int MonsterNum;
        public int MonsterCount;

        public void SetSetting(int _num, int _count)
        {
            MonsterNum = _num;
            MonsterCount = _count;
        }
    }

    [Serializable]
    public struct MissionEscort_State
    {
        public int EscortNum;
        public int EscortHP;
        public void SetSetting(int _num, int _hp)
        {
            EscortNum = _num;
            EscortHP = _hp;
        }
    }

    [Serializable]
    public struct MissionConvoy_State
    {
        public int ConvoyHP;
        public int ConvoyWeight;
        public void SetSetting(int _hp, int _Weight)
        {
            ConvoyHP = _hp;
            ConvoyWeight = _Weight;
        }
    }

    [Serializable]
    public struct MissionBoss_State
    {
        public int BossNum;
        public int BossCount;

        public void SetSetting(int _num, int _count)
        {
            BossNum = _num;
            BossCount = _count;
        }
    }

    public void Save(MissionType mission, int monsterCount)
    {
        missionData.SelectMission_SetData(mission, monsterCount);
        missionData.SelectMission_Save(mission);
    }

    public void Load(MissionType mission)
    {
        missionData.SelectMission_Load(mission);

        if (mission == MissionType.Monster)
        {
            if (!ES3.KeyExists("SelectMission_MonsterCount"))
            {
                MissionEnd(mission); // 초기화 전용.
            }

            int num = missionData.MonsterCount;
            if(num == -1)
            {
                monsterCount = 0;
            }
            else
            {
                monsterCount = num;
            }
        }
        else if (mission == MissionType.Boss)
        {
            if (!ES3.KeyExists("SelectMission_BossCount"))
            {
                MissionEnd(mission); // 초기화 전용.
            }

            int num = missionData.BossCount;
            if(num == -1)
            {
                bossCount = 0;
            }
            else
            {
                bossCount = num;
            }
        }
    }

    public void MissionEnd(MissionType mission)
    {
        if(mission == MissionType.Monster)
        {
            monsterCount = -1;
            Save(mission, monsterCount);
        }
        else if(mission == MissionType.Boss)
        {
            bossCount = -1;
            Save(mission, monsterCount);
        }
    }
}

public enum MissionType {
    Destination,
    Material,
    Monster,
    Escort,
    Convoy,
    Boss
}