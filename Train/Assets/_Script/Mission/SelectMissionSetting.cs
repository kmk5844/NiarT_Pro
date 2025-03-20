using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMissionSetting : MonoBehaviour
{
    [SerializeField]
    private int MissionNum;
    [SerializeField]
    private int StageNum;

    public Transform RailList;
    public GameObject RailObject;
    public Transform MapList;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < MapList.childCount; i++)
        {
            MapList.GetChild(i).GetComponent<SubStage_Select>().settingDirector = this;
            MapList.GetChild(i).GetComponent<SubStage_Select>().MissionNum = MissionNum;
            MapList.GetChild(i).GetComponent<SubStage_Select>().StageNum = StageNum;
            MapList.GetChild(i).GetComponent<SubStage_Select>().SubStageNum = i;
        }
    }
}
