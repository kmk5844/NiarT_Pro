using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDirector : MonoBehaviour
{
    public GameObject UI_StoryCheck;
    public GameObject UI_Story_Init_Warning_Check;
    public GameObject UI_Option;
    public GameObject Demo;
    public AudioClip MainMenuBgm;

    bool storyFlag;
    bool optionFlag;
    bool InfiniteFlag;

    private void Start()
    {
        storyFlag = false;
        optionFlag = false;
        InfiniteFlag = false;
        MMSoundManagerSoundPlayEvent.Trigger(MainMenuBgm, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (InfiniteFlag || storyFlag)
            {
                Click_Back_Button();
            }

            if (optionFlag)
            {
                Click_Option_Back();
            }
        }
    }

    public void Click_Story_Check()
    {
        if (!DataManager.Instance.playerData.FirstFlag)
        {
            DataManager.Instance.playerData.SA_CheckFirstFlag();
            GameManager.Instance.BeforeStation_Enter();
        }
        else
        {
            storyFlag = true;
            UI_StoryCheck.SetActive(true);
        }
    }

    public void Click_Stroy_Mode()
    {
        GameManager.Instance.BeforeStation_Enter();
    }

    public void Click_Infinite_Mode()
    {
        InfiniteFlag = true;
        Demo.SetActive(true);
    }

    public void Click_Back_Button()
    {
        if (storyFlag)
        {
            storyFlag = false;
            UI_StoryCheck.SetActive(false);
        }

        if (InfiniteFlag)
        {
            InfiniteFlag = false;
            Demo.SetActive(false);
        }
    }

    public void Click_Init()
    {
        GameManager.Instance.Game_Reset();
    }

    public void Click_Option_Mode()
    {
        optionFlag = true;
        UI_Option.SetActive(true);
    }

    public void Click_Option_Back()
    {
        optionFlag = false;
        UI_Option.SetActive(false);
    }
    public void Click_Exit_Mode()
    {
        Application.Quit();
    }
}
