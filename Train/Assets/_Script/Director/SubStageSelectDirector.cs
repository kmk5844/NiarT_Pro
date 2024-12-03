using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStageSelectDirector : MonoBehaviour
{
    //UI���ؼ� ������ ����
    public SA_PlayerData playerData;
    MissionDataObject SelectSubStageData;

    public void Open_SelectSubStage_Information(MissionDataObject mission)
    {
        Debug.Log("UI Open");
        Debug.Log(mission.Emerging_Monster);
        SelectSubStageData = mission;
        Debug.Log("End");
    }

    public void Start_SelectSubStage()
    {
        playerData.SA_SelectSubStage(SelectSubStageData.SubStage_Num);
        Debug.Log("���� ������");
    }
}
