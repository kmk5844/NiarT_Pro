using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubStageSelectDirector : MonoBehaviour
{
    //UI���ؼ� ������ ����
    public SA_PlayerData playerData;
    public SA_StageList stageList;
    public Quest_DataTable EX_QuestData;
    public SA_MissionData missionData;

    MissionDataObject SelectSubStageData;
    int SelectSubStageNum;
    [SerializeField]
    List<int> NextSubStageNum;

    [Header("UI")]
    public TextMeshProUGUI UI_MissionInformation;
    public TextMeshProUGUI UI_MainStageText;

    public GameObject UI_MapTab;
    public GameObject UI_SubStageSelect;
    public GameObject UI_ItemTab;
    public GameObject UI_MissionCancelWindow;

    public int missionNum;
    public int stageNum;
    public int selectNum;

    [Header("UI_StageInformation")]
    public GameObject StageInitButton;
    public GameObject InformationObject;

    [Header("Option")]
    public GameObject Option;
    bool optionFlag;
    bool cancelFlag;

    private void Start()
    {
        //itemListData = GetComponent<Station_ItemData>();
        selectNum = -1;

        UI_MainStageText.text = "Stage" + (playerData.Select_Stage + 1);

        StageInitButton.SetActive(false);
    }

    private void OnDisable()
    {
        if (UI_MissionCancelWindow.activeSelf)
        {
            UI_MissionCancelWindow.SetActive(false);
        }
    }

    public void OnEnable()
    {
        int missionNum = playerData.Mission_Num;
        int selectStageNum = playerData.Select_Stage;
        string searchString = playerData.Select_Stage + "," + missionNum;
        int missionInformation_Num = EX_QuestData.Q_List.FindIndex(x => x.Stage_Mission.Equals(searchString));

        UI_MissionInformation.text = EX_QuestData.Q_List[missionInformation_Num].Quest_Information;
        GameObject StageListObject = Resources.Load<GameObject>("UI_SubStageList/" + selectStageNum + "_Stage/" + missionNum);
        if(UI_SubStageSelect.transform.childCount < 2)
        {
            Instantiate(StageListObject, UI_SubStageSelect.transform);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if (optionFlag)
            {
                CloseOption();
            }

            if (cancelFlag)
            {
                No_MissionCancel();
            }
        }
    }

    public void Open_SelectSubStage(MissionDataObject mission)
    {
        SelectSubStageData = mission;
        SelectSubStageNum = mission.SubStage_Num;
        playerData.SA_SelectSubStage(SelectSubStageNum);
        SpecialStage_Check();
    }

    public void Open_SpecialStage()
    {
        if (SelectSubStageData.SubStage_Type == SubStageType.SimpleStation)
        {
            SpeacialStage_Clear();
            SpecialStage_LockOff();
            SceneManager.LoadScene("SimpleStation");
        }
    }

    public void Start_SelectSubStage()
    {
        if(SelectSubStageData.SubStage_Type != SubStageType.SimpleStation) // ����
        {
            if (SceneManager.GetActiveScene().name != "CharacterSelect")
            {
                SceneManager.LoadScene("CharacterSelect");
            }
            else
            {
                this.gameObject.SetActive(false);
                UI_ItemTab.SetActive(false);
            }
        }
    }

    void SpeacialStage_Clear()
    {
        SelectSubStageData.SubStage_Clear();
    }

    void SpecialStage_LockOff()
    {
        foreach(int subStageNum in NextSubStageNum)
        {
            if(subStageNum != -1)
            {
                MissionDataObject mission = missionData.missionStage(missionNum, stageNum, subStageNum);
                mission.SubStageLockOff();
            }
        }
    }

    void SpecialStage_Check()
    {
        string[] nextSubStageList = SelectSubStageData.Open_SubStageNum.Split(',');
        if(NextSubStageNum == null)
        {
            NextSubStageNum = new List<int>();
        }
        else
        {
            NextSubStageNum.Clear();
        }
        
        foreach (string sub in nextSubStageList)
        {
            NextSubStageNum.Add(int.Parse(sub));
        }
    }


 
    public void ClickMissionCancel()
    {
        cancelFlag = true;
        UI_MissionCancelWindow.SetActive(true);
        stageNum = playerData.Select_Stage;
        missionNum = playerData.Mission_Num;
    }

    public void Yes_MissionCancel()
    {
        playerData.SA_MissionPlaying(false);
        playerData.SA_GameLoseCoin(60f);
        missionData.SubStage_Lose(stageNum, missionNum);
        GameObject gm = GameObject.Find("SelectMission");
        Destroy(gm);
        SceneManager.LoadScene("Station");
    }

    public void No_MissionCancel()
    {
        cancelFlag = false;
        UI_MissionCancelWindow.SetActive(false);
    }



    public void ClickSubStage(GameObject _informationObject)
    {
        InformationObject = _informationObject;
        StageInitButton.SetActive(true);
    }

    public void CancelSubStage()
    {
        InformationObject.SetActive(false);
        InformationObject = null;
        StageInitButton.SetActive(false);
    }

    public void OpenOption()
    {
        optionFlag = true;
        Option.SetActive(true);
    }

    public void CloseOption()
    {
        optionFlag = false;
        Option.SetActive(false);
    }
}