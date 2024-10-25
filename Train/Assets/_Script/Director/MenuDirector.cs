using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class MenuDirector : MonoBehaviour
{
    public GameObject UI_Option;
    public GameObject StartWindow;
    Animator startwindow_ani;
    public GameObject Init_Button;

    public LocalizeStringEvent Check_Information_Text;

    public AudioClip MainMenuBgm;

    bool startFlag;
    bool storyFlag;
    bool optionFlag;
    bool InfiniteFlag;

    public Button[] menuButton;

    private void Start()
    {
        Check_Information_Text.StringReference.TableReference = "MainMenu_Table_St";
        startwindow_ani = StartWindow.GetComponent<Animator>();
        storyFlag = false;
        optionFlag = false;
        InfiniteFlag = false;
        MMSoundManagerSoundPlayEvent.Trigger(MainMenuBgm, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (startFlag)
            {
                if (storyFlag || InfiniteFlag)
                {
                    Click_Back_Button();
                }
                else
                {
                    Click_Start_Back();
                }
            }
            

            if (optionFlag)
            {
                Click_Option_Back();
            }
        }
    }

    public void Click_Start_Mode()
    {
        startFlag = true;
        startwindow_ani.SetTrigger("StartMode_Open");
        Click_Common_Button(false);
    }

    public void Click_Start_Back()
    {
        Click_Common_Button(true);
        startFlag = false;
        startwindow_ani.SetTrigger("StartMode_Close");
        storyFlag = false;
        startwindow_ani.SetBool("StoryMode", false);
        InfiniteFlag = false;
        startwindow_ani.SetBool("InfinityMode", false);
    }

    public void Click_Story_Mode()
    {
        storyFlag = true;
        startwindow_ani.SetBool("StoryMode", true);
        if (!DataManager.Instance.playerData.FirstFlag)
        {
            //처음인 경우
            Check_Information_Text.StringReference.TableEntryReference = "UI_Story_First";
            //Check_Information_Text.text = "처음";
            Init_Button.SetActive(false);
        }
        else
        {
            //데이터가 저장이 되어있을 경우
            Check_Information_Text.StringReference.TableEntryReference = "UI_Story_Next";
            //Check_Information_Text.text = "이어서";
            Init_Button.SetActive(true);
        }
    }

    public void Click_Story_Mode_Back()
    {
        storyFlag = false;
        startwindow_ani.SetBool("StoryMode", false);
    }

    public void Click_Story_YesButton()
    {
        GameManager.Instance.BeforeStation_Enter();
    }

    public void Click_Infinite_Mode()
    {
        InfiniteFlag = true;
        startwindow_ani.SetBool("InfinityMode", true);
        Check_Information_Text.StringReference.TableEntryReference = "UI_Infinty_Ban";
        //Check_Information_Text.text = "금지";
    }

    public void Click_Back_Button()
    {
        if (storyFlag)
        {
            storyFlag = false;
            startwindow_ani.SetBool("StoryMode", false);
        }

        if (InfiniteFlag)
        {
            InfiniteFlag = false;
            startwindow_ani.SetBool("InfinityMode", false);
        }
    }

    public void Click_Common_Button(bool flag)
    {
        foreach(Button btn in menuButton)
        {
            btn.interactable = flag;
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
        Click_Common_Button(false);
    }

    public void Click_Option_Back()
    {
        optionFlag = false;
        UI_Option.SetActive(false);
        Click_Common_Button(true);
    }
    public void Click_Exit_Mode()
    {
        Application.Quit();
    }
}
