using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDirector_InGame : MonoBehaviour
{
    [SerializeField]
    private GameDirector gameDirector;
    [SerializeField]
    private Story_DataTable EX_StoryData;
    [SerializeField]
    private SA_LocalData sa_localData;
    [SerializeField]
    private SA_StoryLIst sa_storylist;
    public Dialog dialog;

    [SerializeField]
    int index = 0;
    [SerializeField]
    int storyNum = 0;
    [SerializeField]
    int branchNum = 0;
    [SerializeField]
    GameObject Branch;
    
    public float delayTime;

    public void SetStorySetting(int playerNum, string _StoryString)
    {
        string[] StoryString_Split = _StoryString.Split(",");
        index = playerNum;
        storyNum = int.Parse(StoryString_Split[0]);
        branchNum = int.Parse(StoryString_Split[2]);
        bool flag = false;
        if (playerNum == storyNum)
        {
            flag = true;
            gameDirector.SetStory(int.Parse(StoryString_Split[1]), branchNum, flag); // 芭府客 branch 积己
        }
        else
        {
            flag = false;
        }
    }

    public void SetBranch(GameObject _branch)
    {
        Branch = _branch;
        Branch.GetComponent<DialogSystem>().Story_Init_InGame(gameObject, EX_StoryData.Story_Branch[index].Stage_Num, index, branchNum);
        dialog.dialogSystem = Branch.GetComponent<DialogSystem>();
    }

    public void StartStory_Data()
    {
        sa_storylist.StoryList[index].ChangeFlag(true);
    }

    public void EndStory_Data()
    {
        sa_storylist.StoryList[index].ChangeFlag(false);
    }
}
