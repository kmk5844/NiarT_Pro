using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDirector : MonoBehaviour
{
    public SA_Tutorial tutorialData;
    bool Tutorial_CheckFlag;
    public Transform TutorialList;

    [SerializeField]
    List<bool> Tutorial_Flag = new List<bool>();
    [SerializeField]
    public GameObject[] Tutorial_Object;

    void Awake()
    {
        Tutorial_CheckFlag = false;
        //tutorialData.Init();
        tutorialData.Load();
        AddFlag();
        Instantiate_Tutorial_Check();
    }

    void AddFlag()
    {
        //index
        Tutorial_Flag.Add(tutorialData.Tu_Station); // 0
        Tutorial_Flag.Add(tutorialData.Tu_Maintenance);//1
        Tutorial_Flag.Add(tutorialData.Tu_Store); // 2
        Tutorial_Flag.Add(tutorialData.Tu_Traning); // 3
        Tutorial_Flag.Add(tutorialData.Tu_MapSelect); // 4
        Tutorial_Flag.Add(tutorialData.Tu_Translate); // 5
    }

    void CheckFlag()
    { 
        Tutorial_Flag[0] = tutorialData.Tu_Station;
        Tutorial_Flag[1] = tutorialData.Tu_Maintenance;
        Tutorial_Flag[2] = tutorialData.Tu_Store;
        Tutorial_Flag[3] = tutorialData.Tu_Traning;
        Tutorial_Flag[4] = tutorialData.Tu_MapSelect;
        Tutorial_Flag[5] = tutorialData.Tu_Translate; 
    }

    void Instantiate_Tutorial_Check()
    {
        for (int i = 0; i < Tutorial_Flag.Count; i++)
        {
            GameObject gm = TutorialList.GetChild(i).gameObject;
            gm.GetComponent<Station_TutorialDirector>();
            if (!Tutorial_Flag[i])
            {
                Tutorial_CheckFlag = true;
                if (i == 0)
                {
                    gm.SetActive(true);
                }
                else
                {
                    gm.SetActive(false);
                }
            }
            else
            {
                gm.SetActive(false);
            }
        }

        if (!Tutorial_CheckFlag)
        {
            TutorialList.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void CheckButton(int i)
    {
        CheckFlag();
        if (gameObject.activeSelf)
        {
            if (!Tutorial_Flag[i])
            {
                TutorialList.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}