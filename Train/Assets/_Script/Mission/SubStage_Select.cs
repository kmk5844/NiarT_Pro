using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStage_Select : MonoBehaviour
{
    public bool MainAndSubFlag;
    public MissionDataObject missionData;
    
    SubStageSelectDirector subStageSelectDirector;
    //SA_PlayerData playerData;


    private void Start()
    {
        if (MainAndSubFlag)
        {
            subStageSelectDirector = null;
        }
        else
        {
            subStageSelectDirector = gameObject.GetComponentInParent<SubStageSelectDirector>();
            //playerData = subStageSelectDirector.playerData;
        }
    }

    public void ClickSubStage()
    {
        subStageSelectDirector.Open_SelectSubStage_Information(missionData);
    }
}