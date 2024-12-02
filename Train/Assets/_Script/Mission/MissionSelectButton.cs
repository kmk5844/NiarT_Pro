using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelectButton : MonoBehaviour
{
    public SA_PlayerData playerData;

    public int missionNum;
    public string MissionType;
    public string MissionInformation;
    public int MissionReward;

    public void Mission_SetData(int _missionNum, string type, string information, int reward)
    {
        missionNum = _missionNum;
        MissionType = type;
        MissionInformation = information;
        MissionReward = reward;
    }
    
    public void ClickMission()
    {
        playerData.SA_ClickMission(missionNum);
    }
}