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
    public Quest_DataTable MISSIONDATATABLE { get {  return missionDataTable; } }   
    [SerializeField]
    SA_MissionData missionData;
    [SerializeField]
    SA_ItemList itemListData;

    int stageNum;
    int missionNum;
    public int MISSIONNUM { get { return missionNum; } }
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
    [SerializeField]
    public int MissionCoinLosePersent;

    public MissionMaterial_State M_Material;
    public MissionMonster_State M_Monster;
    public MissionEscort_State M_Escort;
    public MissionConvoy_State M_Convoy;
    public MissionBoss_State M_Boss;
    public MissionEvent_Flag M_Event;

    public int monsterCount;
    public int bossCount;
    public int materialCount;
    public int Select_Train_Weight;

    [Header("stageSelect")]
    public GameObject StageInitButton;

    public int missionList_Index;

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
        missionList_Index = 0;
        for (int i = 0; i < missionDataTable.Q_List.Count; i++)
        {
            missionList_Index = i;
            if (missionDataTable.Q_List[i].Stage_Mission.Equals(FindString))
            {
                break;
            }
        }
        StageMission = missionDataTable.Q_List[missionList_Index].Stage_Mission;
        MissionType_String = missionDataTable.Q_List[missionList_Index].Quest_Type;
        MissionInformation = missionDataTable.Q_List[missionList_Index].Quest_Information;
        MissionState = missionDataTable.Q_List[missionList_Index].Quest_State;
        MissionReward = missionDataTable.Q_List[missionList_Index].Quest_Reward;
        MissionCoinLosePersent = missionDataTable.Q_List[missionList_Index].Quest_Fail;
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
                //���� ����.
                return true;
            }
            else if (MissionType == MissionType.Monster)
            {
                //���� ����.
                return true;
            }
            else if (MissionType == MissionType.Escort)
            {
                //���� -> ���� ���Ϳ��� üũ�� ����
                return true;
            }
            else if (MissionType == MissionType.Convoy)
            {
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
                if(materialCount >= M_Material.ItemCount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
                ItemDataObject _itemData = itemListData.Item[_num];
                int _count = int.Parse(_state[1]);
                int _drop = int.Parse(_state[2]);
                M_Material.SetSetting(_itemData, _count, _drop);
                break;
            case MissionType.Monster:
                _num = int.Parse(_state[0]);
                _count = int.Parse(_state[1]);
                M_Monster.SetSetting(_num, _count);
                break;
            case MissionType.Escort:
                int _hp = int.Parse(_state[0]);
                int _armor = int.Parse(_state[1]);
                int _moveSpeed = int.Parse(_state[2]);
                M_Escort.SetSetting(_hp, _armor,_moveSpeed);
                break;
            case MissionType.Convoy:
                //missionSelect���� �÷��̾ ���� ������
                break;
            case MissionType.Boss:
                _num = int.Parse(_state[0]);
                _count = int.Parse(_state[1]);
                M_Boss.SetSetting(_num, _count);
                break;
        }
    }

    public void Mission_Sucesses(StageDataObject stage)
    {
        if(missionNum == 4)
        {
            MissionReward = M_Convoy.ConvoyGold;
        }

        if (!stage.Stage_ClearFlag)
        {
            playerData.SA_Get_Coin(MissionReward);
        }
        else
        {
            playerData.SA_Get_Coin(MissionReward / 2);
        }

        MissionEnd(MissionType);
        if (playerData.EventFlag)
        {
            playerData.SA_EventFlag_Off();
        }
        Destroy(this.gameObject);
    }

    public void Mission_Fail()
    {
        MissionEnd(MissionType);
        if (playerData.EventFlag)
        {
            playerData.SA_EventFlag_Off();
        }
        Destroy(this.gameObject);
    }

    [Serializable]
    public struct MissionMaterial_State
    {
        public ItemDataObject itemData;
        public int ItemCount;
        public int ItemDrop;

        public void SetSetting(ItemDataObject item, int _count, int _drop)
        {
            itemData = item;
            ItemCount = _count;
            ItemDrop = _drop;
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
        public int EscortHP;
        public int EscortArmor;
        public int EscortMoveSpeed;
        public void SetSetting(int _hp, int _armor, int _moveSpeed)
        {
            EscortHP = _hp;
            EscortArmor = _armor;
            EscortMoveSpeed = _moveSpeed;
        }
    }

    [Serializable]
    public struct MissionConvoy_State
    {
        public int ConvoyWeight;
        public int ConvoyGold;
        public void SetSetting(int _Weight, int _Gold)
        {
            ConvoyWeight = _Weight;
            ConvoyGold = _Gold;
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

    [Serializable]
    public struct MissionEvent_Flag
    {
        public bool EventFlag;
        public bool TreasureFlag;

        public void Flag_ON(int i)
        {
            EventFlag = true;
            switch (i)
            {
                case 0: 
                    TreasureFlag = true;
                    break;
            }
        }

        public void Flag_OFF(int i)
        {
            EventFlag = false;
            switch (i)
            {
                case 0:
                    TreasureFlag = false;
                    break;
            }
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

        if(mission == MissionType.Material)
        {
            if (!ES3.KeyExists("SelectMission_MonsterCount"))
            {
                MissionEnd(mission); // �ʱ�ȭ ����.
            }

            int num = missionData.MaterialCount;
            if (num == -1)
            {
                materialCount = 0;
            }
            else
            {
                materialCount = num;
            }
        }
        else if (mission == MissionType.Monster)
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
        if(mission== MissionType.Material)
        {
            materialCount = -1;
            Save(mission, materialCount);
        }
        else if(mission == MissionType.Monster)
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