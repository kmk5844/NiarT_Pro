using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionDirector : MonoBehaviour
{
    public SelectMission selectmission;
    public UIDirector uiDirector;
    public GameDirector gameDirector;
    public MonsterDirector monsterDirector;
    public MercenaryDirector mercenaryDirector;

    bool countFlag;
    bool trainFlag;
    [SerializeField]
    int monsterCount; // 몬스터와 보스와 같이, 카운팅

    //Script Execution Order로 조절 중
    void Awake()
    {
        try
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
                    countFlag = true;
                    monsterDirector.missionFlag_monster = true;
                    break;
                case MissionType.Escort:
                    mercenaryDirector.SetEscort(selectmission.M_Escort.EscortHP, selectmission.M_Escort.EscortArmor, selectmission.M_Escort.EscortMoveSpeed);
                    break;
                case MissionType.Convoy:
                    gameDirector.Mission_Train_Flag = true;
                    break;
                case MissionType.Boss:
                    countFlag = true;
                    monsterDirector.missionFlag_boss = true;
                    break;
            }
            uiDirector.CheckMissionInformation(selectmission.MissionInformation);

            if (countFlag)
            {
                monsterCount = selectmission.monsterCount;
                uiDirector.missionCountText_text.text = "Count : " + monsterCount;
            }
            else
            {
                uiDirector.missionCountText_text.text = "";
            }
        }
        catch
        {
            Debug.Log("테스트");
        }
    }

    //몬스터
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

    //Boss
    public bool CheckBoss(int BossNum)
    {
        if(selectmission.M_Boss.BossNum == BossNum)
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
        uiDirector.missionCountText_text.text = "Count : " + monsterCount;
    }

    public void stageEnd()
    {
        selectmission.Save(selectmission.MissionType, monsterCount);
    }

    public void Adjustment_Mission()
    {
        switch (selectmission.MissionType)
        {
            case MissionType.Destination:
                break;
            case MissionType.Material:
                //몬스터 재료
                break;
            case MissionType.Monster:
                selectmission.monsterCount += monsterCount;
                break;
            case MissionType.Escort:
                break;
            case MissionType.Convoy:
                //운반하기
                break;
            case MissionType.Boss:
                selectmission.bossCount += monsterCount;
                break;
        }
        stageEnd();
    }
}