using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Sprite OpenStageSprite;

    [Header("Item_Information")]
    public GameObject InformationObject;
    public TextMeshProUGUI type_text;
    public TextMeshProUGUI distance_text;

    [Header("µ•¿Ã≈Õ")]
    [SerializeField]
    MissionDataObject missionData;
    [SerializeField]
    SubStageSelectDirector subStageSelectDirector;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ClickSubStage);
    }

    private void Start()
    {
        subStageSelectDirector = GetComponentInParent<SubStageSelectDirector>();
        if (selectSubStageType == stageType.Sub)
        {
            missionData = subStageSelectDirector.missionData.missionStage(MissionNum, StageNum, SubStageNum);
            InformationObject.SetActive(false);
            type_text.text = "Type : " + missionData.SubStage_Type;
            distance_text.text = "Distance : " + missionData.Distance;

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
        if (selectSubStageType == stageType.Sub)
        {
            if(subStageSelectDirector.InformationObject != null)
            {
                subStageSelectDirector.CancelSubStage();
            }
            subStageSelectDirector.ClickSubStage(InformationObject);
            InformationObject.SetActive(true);
        }
    }

    public void ClickNextItem()
    {
        subStageSelectDirector.Open_SelectSubStage(missionData);
        subStageSelectDirector.Open_ItemTab();
        InformationObject.SetActive(false);
    }
}