using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionSelectDirector : MonoBehaviour
{
    public GameObject MissionSelectObject_UI;
    missionSelect_Trigger missionselectAni_Director;
    Animator missionselectAni;

    public SA_PlayerData playerData;
    public SA_StageList stageList;
    public Quest_DataTable EX_QuestData;
    int mainStageNum;
    [Header("UI")]
    [SerializeField]
    List<int> missionList_Index;

    [SerializeField]
    List<int> RandomMission;
    public List<MissionSelectButton> buttonList;

    [Header("SubSelectDirector")]
    public GameObject SubStageSelectObject;
    public GameObject SelectMissionObject;

    [Header("Station")]
    public GameObject StationBackCheckWindow;
    public TextMeshProUGUI PlayerGold;

    [Header("Option")]
    bool OptionFlag;
    public GameObject Option;

    int count = 0;

    private void Awake()
    {
        missionselectAni_Director = MissionSelectObject_UI.GetComponentInParent<missionSelect_Trigger>();
        missionselectAni = MissionSelectObject_UI.GetComponentInParent<Animator>();

        mainStageNum = playerData.Select_Stage;
        missionList_Index = new List<int>();
        RandomMission = new List<int>();

        missionList_Index = stageList.Stage[mainStageNum].MissionList;


        if (missionList_Index.Count < 3)
        {
            foreach(int x in missionList_Index)
            {
                RandomMission.Add(x);
                count++;
            }
        }
        else
        {
            do
            {
                //미션 선별
                int x = Random.Range(0, missionList_Index.Count);
                int y = missionList_Index[x];
                if (!RandomMission.Contains(y))
                {
                    RandomMission.Add(y);
                    count++;
                }
            } while (count < 3);
        }
        SelectMissionObject.GetComponent<SelectMission>().SetDataSetting(playerData, EX_QuestData);

        PlayerGold.text = playerData.Coin.ToString();
    }

    private void Start()
    {
        OptionFlag = false;
        if (!playerData.Mission_Playing)
        {
            missionselectAni.enabled = true;
            MissionSelectObject_UI.SetActive(true);
            SubStageSelectObject.SetActive(false);
        }
        else
        {
            missionselectAni.enabled = false;
            MissionSelectObject_UI.SetActive(false);
            SubStageSelectObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!OptionFlag)
            {
                OpenOption_Button();
            }
            else
            {
                CloseOption_Button();
            }
        }
    }

    public IEnumerator MissionDataSet()
    {
        GameObject game_obj = Instantiate(SelectMissionObject);
        game_obj.name = "SelectMission";
        yield return new WaitForSeconds(0.8f);
        missionselectAni_Director.Close_MissionSelect();
        //SubStageSelectObject.SetActive(true);
    }

    public void Open_SubStageSelect()
    {
        SubStageSelectObject.SetActive(true);
    }

    public void OpenOption_Button()
    {
        OptionFlag = true;
        Option.SetActive(true);
    }

    public void CloseOption_Button()
    {
        OptionFlag = false;
        Option.SetActive(false);
    }

    public void OpenBUutton_Station()
    {
        StationBackCheckWindow.SetActive(true);
    }

    public void YesButton_Station()
    {
        SceneManager.LoadScene("Station");
    }

    public void NoButton_Station()
    {
        StationBackCheckWindow.SetActive(false);
    }

    public void SpawnCard()
    {
        for (int i = 0; i < count; i++)
        {
            int missionNum = RandomMission[i];
            string searchString = mainStageNum + "," + missionNum;
            int missionInformation_Num = EX_QuestData.Q_List.FindIndex(x => x.Stage_Mission.Equals(searchString));

            buttonList[i].gameObject.SetActive(true);
            buttonList[i].Mission_SetData(missionNum, EX_QuestData.Q_List[missionInformation_Num].Quest_Type, EX_QuestData.Q_List[missionInformation_Num].Quest_Information, EX_QuestData.Q_List[missionInformation_Num].Quest_Reward);
        }
    }
}
