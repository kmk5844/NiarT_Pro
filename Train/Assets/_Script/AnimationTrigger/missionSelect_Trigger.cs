using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionSelect_Trigger : MonoBehaviour
{
    public MissionSelectDirector missionSelectDirector;
    Animator ani;
    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void SpawnSelectCard()
    {
        missionSelectDirector.SpawnCard();
    }

    public void Close_MissionSelect()
    {
        ani.SetBool("Off", true);
    }

    public void SetActiveFalse_MissionSelect()
    {

    }

    public void Open_SubStageSelect()
    {
        missionSelectDirector.MissionSelectObject_UI.SetActive(false);
        missionSelectDirector.Open_SubStageSelect();
    }
}
