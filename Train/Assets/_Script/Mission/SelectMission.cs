using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMission : MonoBehaviour
{
    [SerializeField]
    SA_PlayerData playerData;
    [SerializeField]
    Quest_DataTable missionDataTable;

    int stageNum;
    int missionNum;
    string FindString;

    [SerializeField]
    string StageMission;
    string MissionType_String;
    [SerializeField]
    MissionType MissionType;
    [SerializeField]
    string MissionInformation;
    [SerializeField]
    string MissionState;
    [SerializeField]
    int MissionReward;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        stageNum = playerData.Select_Stage;
        missionNum = playerData.Mission_Num;
        FindString = stageNum + ","+ missionNum;
        SetMissionSetting();
        SetMissionType();
    }

    public void SetDataSetting(SA_PlayerData _playerData, Quest_DataTable _missionDataTable)
    {
        playerData = _playerData;
        missionDataTable = _missionDataTable;
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
}

public enum MissionType {
    Destination,
    Material,
    Monster,
    Escort,
    Convoy,
    Boss
}