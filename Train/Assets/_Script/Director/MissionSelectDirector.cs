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
    public SA_MissionData missionData;
    public Quest_DataTable EX_QuestData;
    int mainStageNum;
    [Header("UI")]
    [SerializeField]
    List<int> missionList_Index;

    [SerializeField]
    List<int> RandomMission;

    public List<MissionSelectButton> buttonList;

    [Header("Test")]
    public GameObject MissionSelect;

    [Header("ReadyDirector")]
    public GameObject ReadyObject;

    [Header("SubSelectDirector")]
    public GameObject SubStageSelectObject;
    public GameObject SelectMissionObject;

    [Header("Station")]
    public GameObject StationBackCheckWindow;
    public TextMeshProUGUI PlayerGold;

    int count = 0;
    int clickMissionNum;

    [Header("Convoy")]
    public Slider Convoy_Slider;
    public GameObject Convoy_Window;
    public TextMeshProUGUI minGoldText;
    public TextMeshProUGUI maxGoldText;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI WeightText;
    public TextMeshProUGUI SpeedText;
    int minGold;
    int maxGold;
    int amountGold;
    int amountWeight;
    int minWeight;
    int trainWeight;
    float rewardGold;
    bool ConvoyFlag = false;

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
        SelectMissionObject.GetComponent<SelectMission>().SetDataSetting(playerData, EX_QuestData, missionData);

        PlayerGold.text = playerData.Coin.ToString();
    }

    private void Start()
    {
        if (!playerData.Mission_Playing)
        {
            missionselectAni.enabled = true;
            MissionSelectObject_UI.SetActive(true);
            ReadyObject.SetActive(false);
            SubStageSelectObject.SetActive(false);
        }
        else
        {
            missionselectAni.enabled = false;
            MissionSelectObject_UI.SetActive(false);
            ReadyObject.SetActive(true);
            SubStageSelectObject.SetActive(false);
        }
    }

    public IEnumerator MissionDataSet()
    {
        GameObject game_obj = Instantiate(SelectMissionObject);
        game_obj.name = "SelectMission";
        yield return new WaitForSeconds(0.02f);
        MissionSelect.SetActive(false);
        ReadyObject.SetActive(true);
        //missionselectAni_Director.Close_MissionSelect();
        //SubStageSelectObject.SetActive(true);
    }

    public void Open_SubStageSelect()
    {
        SubStageSelectObject.SetActive(true);
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

    public void Open_Numerical_Settings_Convoy(int missionNum, string _State)
    {
        ConvoyFlag = true;
        string[] state = _State.Split(',');
        minGold = int.Parse(state[0]);
        maxGold = int.Parse(state[1]);
        amountGold = int.Parse(state[2]);
        minWeight = int.Parse(state[3]);
        amountWeight = int.Parse(state[4]);

        rewardGold = minGold;
        trainWeight = minWeight;
        minGoldText.text = minGold + "G";
        maxGoldText.text = maxGold + "G";
        GoldText.text = minGold + "G";
        WeightText.text = minWeight.ToString();
        SpeedText.text = "- " + (minWeight / 100000);

        Convoy_Slider.minValue = minGold;
        Convoy_Slider.maxValue = maxGold;
        Convoy_Slider.wholeNumbers = true;
        Convoy_Slider.onValueChanged.AddListener(SliderChange_Convoy);
        
        Convoy_Window.SetActive(true);
        clickMissionNum = missionNum;
    }

    void SliderChange_Convoy(float value)
    {
        rewardGold = Mathf.Round(value / amountGold) * amountGold;
        Convoy_Slider.value = rewardGold;
        GoldText.text = rewardGold + "G";

        int x = ((int)rewardGold - minGold) / amountGold;
        trainWeight = (minWeight + (amountWeight * x));
        WeightText.text = trainWeight.ToString();
        SpeedText.text = "- " + (trainWeight / 100000);
    }

    public void Check_Numerical_Settings()
    {
        if(ConvoyFlag)
        {
            ConvoyFlag = false;
            Convoy_Window.SetActive(false);
            SelectMissionObject.GetComponent<SelectMission>().M_Convoy.SetSetting(trainWeight, (int)rewardGold);
        }
        playerData.SA_ClickMission(clickMissionNum);
        playerData.SA_MissionPlaying(true);
        StartCoroutine(MissionDataSet());
    }

    public void SpawnCard()
    {
        for (int i = 0; i < count; i++)
        {
            int missionNum = RandomMission[i];
            string searchString = mainStageNum + "," + missionNum;
            int missionInformation_Num = EX_QuestData.Q_List.FindIndex(x => x.Stage_Mission.Equals(searchString));
            buttonList[i].gameObject.SetActive(true);
            buttonList[i].Mission_SetData(missionNum, EX_QuestData.Q_List[missionInformation_Num].Quest_Type, EX_QuestData.Q_List[missionInformation_Num].Quest_Information, EX_QuestData.Q_List[missionInformation_Num].Quest_Reward, EX_QuestData.Q_List[missionInformation_Num].Quest_State);
        }
    }
}
