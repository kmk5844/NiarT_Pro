using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSubStageScript : MonoBehaviour
{
    public Transform it;
    public SA_StageList stageList;
    public SA_MissionData data;
    int count = 0;
    int maxcount = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < stageList.Stage.Count - 1; i++)
        {
            //int j = 0;
            for (int j = 0; j < stageList.Stage[i].MissionList.Count; j++)
            {
                try
                {
                    GameObject InstantObject = Instantiate(Resources.Load<GameObject>("UI_SubStageList/" + i + "_Stage/" + stageList.Stage[i].MissionList[j]), it);
                    InstantObject.name = i + "_" + stageList.Stage[i].MissionList[j];
                    InstantObject.SetActive(false);
                    maxcount++;
                }
                catch
                {
                    Debug.Log("문제 발생 : " + i + "_stage/" + stageList.Stage[i].MissionList[j]);
                }
            }
        }
        Debug.Log(maxcount);
        it.GetChild(count).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(count == 0)
            {
                it.GetChild(0).gameObject.SetActive(false);
                count = maxcount - 1;
                it.GetChild(count).gameObject.SetActive(true);
            }
            else
            {
                it.GetChild(count).gameObject.SetActive(false);
                count--;
                it.GetChild(count).gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(count == maxcount - 1 )
            {
                it.GetChild(maxcount - 1).gameObject.SetActive(false);
                count = 0;
                it.GetChild(count).gameObject.SetActive(true);
            }
            else
            {
                it.GetChild(count).gameObject.SetActive(false);
                count++;
                it.GetChild(count).gameObject.SetActive(true);
            }
        }
    }

    public MissionDataObject FindMissionData(int i, int b, int c)
    {
        return data.missionStage(i, b, c);
    }
}
