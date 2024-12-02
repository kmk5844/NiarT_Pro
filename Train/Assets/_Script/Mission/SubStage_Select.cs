using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStage_Select : MonoBehaviour
{
    public bool MainAndSubFlag;
    public QuestDataObject questData;
    
    SubStageSelectDirector subStageSelectDirector;

    private void Start()
    {
        if (MainAndSubFlag)
        {
            subStageSelectDirector = null;
        }
        else
        {
            subStageSelectDirector = gameObject.GetComponentInParent<SubStageSelectDirector>();
        }
    }
}