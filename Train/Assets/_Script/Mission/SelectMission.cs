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
        if (!LastFlag) // �������� �ƴ� ��
        {
            if (MissionType == MissionType.Destination)
            {
                //Debug.Log("������ ������ �޼��ϱ�");
                return true;
            }
            else if (MissionType == MissionType.Material)
            {
                Debug.Log("��� ������");
                //��� ī��Ʈ �߰�
                return true;
            }
            else if (MissionType == MissionType.Monster)
            {
                //���� ����.
                return true;
            }
            else if (MissionType == MissionType.Escort)
            {
                Debug.Log("ȣ���ϱ�");
                //���� -> ���� ���Ϳ��� üũ�� ����
                return true;
            }
            else if (MissionType == MissionType.Convoy)
            {
                Debug.Log("ȣ���ϱ�");
                //���� -> ���� ���Ϳ��� üũ�� ����
                return true;
            }
            else if (MissionType == MissionType.Boss)
            {
                //���� ����
                return true;
            }
        }
        else // �������� ��
        {
            if (MissionType == MissionType.Destination)
            {
                //���� ���� ����
                return true;
            }
            else if (MissionType == MissionType.Material)
            {
                //��� üũ �� bool üũ
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
                //���� üũ �� bool üũ
            }
            else if (MissionType == MissionType.Escort)
            {
                //���� ���� ���߿� �μ����ų� �������� ���ӵ��Ϳ��� �й� Ȯ��
                //������ ���ٸ� true�� ��� ����
                return true;
            }
            else if (MissionType == MissionType.Convoy)
            {                
                //���� ���� ���߿� �μ����ų� �������� ���ӵ��Ϳ��� �й� Ȯ��
                //������ ���ٸ� true�� ��� ����
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
                MissionEnd(mission); // �ʱ�ȭ ����.
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
                MissionEnd(mission); // �ʱ�ȭ ����.
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