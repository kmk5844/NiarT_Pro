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

    [Header("µ•¿Ã≈Õ")]
    [SerializeField]
    MissionDataObject missionData;
    [SerializeField]
    SubStageSelectDirector subStageSelectDirector;

    bool startFlag = false;

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
        distance_text.text = "Distance : " + missionData.Distance;

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
                Color customColor = new Color(0.45f, 0.11f, 0.11f);
                GetComponent<Image>().color = customColor;
                GetComponent<Button>().enabled = false;
            }
            else
            {
                GetComponent<Image>().color = Color.white;
                GetComponent<Button>().enabled = true;
            }

            if (missionData.StageOpenFlag)
            {
                GetComponent<Image>().sprite = OpenStageSprite;
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
                GetComponent<Image>().sprite = OpenStageSprite;
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
        subStageSelectDirector.selectNum = missionData.SubStage_Num;
        subStageSelectDirector.Open_SelectSubStage(missionData);
        yield return new WaitForSeconds(0.1f);
        subStageSelectDirector.Open_ItemTab();
        InformationObject.SetActive(false);
    }
}