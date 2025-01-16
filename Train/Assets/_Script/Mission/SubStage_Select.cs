using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubStage_Select : MonoBehaviour
{
    enum stageType
    { Sub,
      Next }; // Main : 0 Sub : 1 NextMain : 2

    [SerializeField]
    stageType selectSubStageType;

    public int MissionNum;
    public int StageNum;
    public int SubStageNum;
    public Sprite OpenStageSprite;
    public GameObject LastSelectObject;

    [Header("Item_Information")]
    public GameObject InformationObject;
    public TextMeshProUGUI type_text;
    public TextMeshProUGUI distance_text;

    [Header("������")]
    [SerializeField]
    MissionDataObject missionData;
    [SerializeField]
    SubStageSelectDirector subStageSelectDirector;

    bool startFlag = false;
    public Sprite[] temporarilySprite;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ClickSubStage);
    }

    private void Start()
    {
        subStageSelectDirector = GetComponentInParent<SubStageSelectDirector>();
        missionData = subStageSelectDirector.missionData.missionStage(MissionNum, StageNum, SubStageNum);
        InformationObject.SetActive(false);
        type_text.text = "Type : " + missionData.SubStage_Type;
        if(missionData.Distance != -1)
        {
            distance_text.text = "Distance : " + missionData.Distance;
        }
        else
        {
            distance_text.text = "";
        }


        switch (missionData.SubStage_Type)
        {
            case SubStageType.Nomal:
                GetComponent<Image>().sprite = temporarilySprite[0];
                break;
            case SubStageType.Hard:
                GetComponent<Image>().sprite = temporarilySprite[1];
                break;
            case SubStageType.HardCore:
                GetComponent<Image>().sprite = temporarilySprite[2];
                break;
            case SubStageType.Boss:
                GetComponent<Image>().sprite = temporarilySprite[3];
                break;
            case SubStageType.SimpleStation:
                GetComponent<Image>().sprite = temporarilySprite[4];
                break;
        }

        if (!missionData.NextStageFlag)
        {
            selectSubStageType = stageType.Sub;
        }
        else
        {
            selectSubStageType= stageType.Next;
        }

        if (selectSubStageType == stageType.Sub)
        {
            if (missionData.StageClearFlag)
            {
                GetComponent<Button>().enabled = false;
            }
            else
            {
                GetComponent<Image>().color = Color.white;
                GetComponent<Button>().enabled = true;
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
            if (missionData.StageOpenFlag)
            {
                GetComponent<Button>().interactable = true;
            }
            else
            {
                GetComponent<Button>().interactable = false;
            }
        }
        startFlag = true;
    }
    private void OnEnable()
    {
        if (startFlag)
        {
            if (subStageSelectDirector.selectNum == SubStageNum)
            {
                LastSelectObject.SetActive(true);
            }
            else
            {
                LastSelectObject.SetActive(false);
            }
        }
    }

    public void ClickSubStage()                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
    {
        if(subStageSelectDirector.InformationObject != null)
        {
            subStageSelectDirector.CancelSubStage();
        }
        subStageSelectDirector.ClickSubStage(InformationObject);
        InformationObject.SetActive(true);
    }

    public void ClickNextItem()
    {
        StartCoroutine(DelayAni());
    }

    IEnumerator DelayAni()
    {
        subStageSelectDirector.missionNum = missionData.Mission_Num;
        subStageSelectDirector.stageNum = missionData.Stage_Num;
        subStageSelectDirector.selectNum = missionData.SubStage_Num;
        subStageSelectDirector.Open_SelectSubStage(missionData);
        yield return new WaitForSeconds(0.1f);
        subStageSelectDirector.Open_ItemTab();
        InformationObject.SetActive(false);
    }
}