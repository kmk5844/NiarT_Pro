using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PixelCrushers.AnimatorSaver;

public class PrevStageDirector : MonoBehaviour
{
    public SA_PlayerData playerData;
    public SA_MissionData missionData;
    public int missionNum;
    public int stageNum;
    public int Before_Sub_Num;
    public int Select_Sub_Num;
    public List<int> PrevSubStageNum;

    // Start is called before the first frame update
    void Start()
    {
        missionNum = playerData.Mission_Num;
        stageNum = playerData.Select_Stage;
        Before_Sub_Num = playerData.Before_Sub_Stage;
        Select_Sub_Num = playerData.Select_Sub_Stage;
        MissionDataObject PrevStageData = missionData.missionStage(missionNum, stageNum, Before_Sub_Num);
        PrevSubStageNum = new List<int>();
        string[] prevSubStageList = PrevStageData.Open_SubStageNum.Split(',');
        foreach (string sub in prevSubStageList)
        {
            PrevSubStageNum.Add(int.Parse(sub));
        }

        foreach (int substageNum in PrevSubStageNum)
        {
            if (substageNum != Select_Sub_Num)
            {
                MissionDataObject mission = missionData.missionStage(missionNum, stageNum, substageNum);
                if (!mission.StageClearFlag)
                {
                    mission.SubStageLockOn();
                }
            }
        }

        DataManager.Instance.playerData.SA_BeforeSubSelectStage_Save(Select_Sub_Num);
    }
}
