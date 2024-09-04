using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDirector : MonoBehaviour
{
    public GameObject UI_Option;
    public GameObject Demo;
    public AudioClip MainMenuBgm;

    bool optionFlag;
    bool InfiniteFlag;

    private void Start()
    {
        optionFlag = false;
        InfiniteFlag = false;
        MMSoundManagerSoundPlayEvent.Trigger(MainMenuBgm, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (InfiniteFlag)
            {
                Click_Back_Button();
            }

            if (optionFlag)
            {
                Click_Option_Back();
            }

        }
    }

    public void Click_Stroy_Mode()
    {
        GameManager.Instance.Start_Enter();
    }

    public void Click_Infinite_Mode()
    {
        InfiniteFlag = true;
        Demo.SetActive(true);
    }

    public void Click_Back_Button()
    {
        InfiniteFlag = false;
        Demo.SetActive(false);
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
