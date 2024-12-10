using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubStage_Select : MonoBehaviour
{
    enum stageType
    { Main,
      Sub,
      Next }; // Main : 0 Sub : 1 NextMain : 2

    [SerializeField]
    stageType selectSubStageType;

    public int MissionNum;
    public int StageNum;
    public int SubStageNum;
    public GameObject ClearObjcet;

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
        if (selectSubStageType == stageType.Main)
        {
            subStageSelectDirector = null;
        }
        else if(selectSubStageType == stageType.Sub)
        {
            subStageSelectDirector = GetComponentInParent<SubStageSelectDirector>();
            missionData = subStageSelectDirector.missionData.missionStage(MissionNum, StageNum, SubStageNum);

            if (missionData.StageClearFlag)
            {
                ClearObjcet.SetActive(true);
            }
            else
            {
                ClearObjcet.SetActive(false);
            }

            if (missionData.StageOpenFlag)
            {
                GetComponent<Button>().interactable = true;
            }
            else
            {
                GetComponent<Button>().interactable = false;
            }
        }else if(selectSubStageType == stageType.Next)
        {
            subStageSelectDirector = GetComponentInParent<SubStageSelectDirector>();
            if (!subStageSelectDirector.missionData.MainStage_ClearFlag[StageNum])
            {
                GetComponent<Button>().interactable = false;
            }
            else
            {
                GetComponent<Button>().interactable = true;
            }
        }
    }

    public void ClickSubStage()
    {
        if (selectSubStageType == stageType.Main)
        {
            Debug.Log("정거장");
            //subStageSelectDirector.
        }
        else if (selectSubStageType == stageType.Sub)
        {
            subStageSelectDirector.Open_SelectSubStage_Information(missionData);
        }
        else if (selectSubStageType == stageType.Next)
        {
            Debug.Log("정거장");
        }
    }
}