using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionSelectButton : MonoBehaviour
{
    public SA_PlayerData playerData;
    public MissionSelectDirector missionSelectDirector;

    [Header("UI")]
    public TextMeshProUGUI MissionType_Text;
    public TextMeshProUGUI MissionInformation_Text;
    public TextMeshProUGUI MissionReward_Text;


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

        MissionType_Text.text = MissionType;
        MissionInformation_Text.text = MissionInformation;
        MissionReward_Text.text = MissionReward.ToString();
    }
    
    public void ClickMission()
    {
        playerData.SA_ClickMission(missionNum);
        playerData.SA_MissionPlaying(true);
        missionSelectDirector.Open_SubSelectObject();
    }
}