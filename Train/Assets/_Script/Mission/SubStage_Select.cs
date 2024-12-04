using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubStage_Select : MonoBehaviour
{
    public bool MainAndSubFlag;

    public int MissionNum;
    public int StageNum;
    public int SubStageNum;

    [SerializeField]
    MissionDataObject missionData;
    [SerializeField]
    SubStageSelectDirector subStageSelectDirector;

    public List<Button> nextStage;
    //SA_PlayerData playerData;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ClickSubStage);
    }

    private void Start()
    {
        if (MainAndSubFlag)
        {
            subStageSelectDirector = null;
        }
        else
        {
            subStageSelectDirector = GetComponentInParent<SubStageSelectDirector>();
            missionData = subStageSelectDirector.missionData.missionStage(MissionNum, StageNum, SubStageNum);
            if (missionData.StageOpenFlag)
            {
                GetComponent<Button>().interactable = true;
            }
            else
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }

    public void ClickSubStage()
    {
        subStageSelectDirector.Open_SelectSubStage_Information(missionData);
    }
}