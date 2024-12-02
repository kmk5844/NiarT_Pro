using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelectDirector : MonoBehaviour
{
    public SA_PlayerData playerData;
    public SA_StageList stageList;
    public Quest_DataTable EX_QuestData;
    int mainStageNum;
    [SerializeField]
    List<int> missionList_Index;

    [SerializeField]
    List<int> RandomMission;
    public List<MissionSelectButton> buttonList;

    private void Awake()
    {
        mainStageNum = playerData.Select_Stage;
        missionList_Index = new List<int>();
        RandomMission = new List<int>();

        missionList_Index = stageList.Stage[mainStageNum].MissionList;

        int count = 0;

        do
        {
            int x = Random.Range(0, missionList_Index.Count + 1);
            if (!RandomMission.Contains(x))
            {
                RandomMission.Add(x);
                count++;
            }
        }while (count < 3);

        count = 0;
        foreach(int k in RandomMission)
        {
            string searchString = mainStageNum + "," + k;
            int i = EX_QuestData.Q_List.FindIndex(x => x.Stage_Mission.Equals(searchString));

            buttonList[count].Mission_SetData(k, EX_QuestData.Q_List[i].Quest_Type, EX_QuestData.Q_List[i].Quest_Information, EX_QuestData.Q_List[i].Quest_Reward);
            count++;
        }
    }
}
