using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionDirector : MonoBehaviour
{
    public SelectMission selectmission;
    public MonsterDirector monsterDirector;

    [SerializeField]
    int monsterCount;

    void Start()
    {
        selectmission = GameObject.Find("SelectMission").GetComponent<SelectMission>();
        monsterCount = 0;
        switch (selectmission.MissionType)
        {
            case MissionType.Destination:

                break;
            case MissionType.Material:

                break;
            case MissionType.Monster:
                monsterDirector.missionFlag = true;
                break;
            case MissionType.Escort:

                break;
            case MissionType.Convoy:

                break;
            case MissionType.Boss:

                break;
        }
    }
    //∏ÛΩ∫≈Õ
    public bool CheckMonster(int MonsterNum)
    {
        if (selectmission.M_Monster.MonsterNum == MonsterNum)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MonsterCount()
    {
        monsterCount++;
        Debug.Log(monsterCount);
    }

    public void Adjustment_Mission()
    {
        switch (selectmission.MissionType)
        {
            case MissionType.Destination:

                break;
            case MissionType.Material:
                
                break;
            case MissionType.Monster:
                selectmission.monsterCount += monsterCount;
                break;
            case MissionType.Escort:

                break;
            case MissionType.Convoy:

                break;
            case MissionType.Boss:

                break;
        }
    }
}