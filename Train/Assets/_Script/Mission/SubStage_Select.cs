using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubStage_Select : MonoBehaviour
{
    enum stageType
    { Sub,
      Next }; // Main : 0 Sub : 1 NextMain : 2

    stageType selectSubStageType;

    public SelectMissionSetting settingDirector;
    public int MissionNum;
    public int StageNum;
    public int SubStageNum;
    public Sprite OpenStageSprite;
    public GameObject LastSelectObject;
    public GameObject LastFlag;

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
            case SubStageType.Food:
                GetComponent<Image>().sprite = temporarilySprite[4];
                break;
            case SubStageType.Treasure:
                GetComponent<Image>().sprite = temporarilySprite[4];
                break;
            default:
                GetComponent<Image>().sprite = temporarilySprite[0];
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

        SpawnRail();
        Transform parent = InformationObject.transform.parent.parent;
        InformationObject.transform.SetParent(parent);
        InformationObject.transform.SetAsLastSibling();
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
        if (missionData.ReadyFlag)
        {
            subStageSelectDirector.Start_SelectSubStage();
        }
        else
        {
            subStageSelectDirector.Open_SpecialStage();
        }
        InformationObject.SetActive(false);
    }

    void SpawnRail()
    {
        Transform objA = this.transform;
        Transform objB;
        RectTransform Rail = settingDirector.RailObject.GetComponent<RectTransform>();
        string [] rail_String = missionData.Open_SubStageNum.Split(",");
        for(int i = 0; i < rail_String.Length; i++)
        {
            int rail_num = int.Parse(rail_String[i]);
            if(rail_num == -1)
            {
                LastFlag.SetActive(true);
                break;
            }
            objB = settingDirector.MapList.GetChild(rail_num).transform;
            RectTransform newRail = Instantiate(Rail, settingDirector.RailList);
            newRail.position = (objA.position + objB.position)/2;

            Vector3 direction = objB.localPosition - objA.localPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            newRail.rotation = Quaternion.Euler(0,0,angle);

            newRail.sizeDelta = new Vector2(direction.magnitude, newRail.sizeDelta.y);
        }
    }
}