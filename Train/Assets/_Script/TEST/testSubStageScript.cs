using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSubStageScript : MonoBehaviour
{
    public Transform it;
    public SA_MissionData data;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            int j = 0;
            try
            {
                GameObject StageListObject = Resources.Load<GameObject>("UI_SubStageList/" + i + "_Stage/" + j);
                Instantiate(StageListObject, it);
            }
            catch
            {
                Debug.Log("¾øÀ½");
            }

/*            for (int j = 0; j < 6; j++)
            {

            }*/
        }
    }

    public MissionDataObject FindMissionData(int i, int b, int c)
    {
        return data.missionStage(i, b, c);
    }
}
