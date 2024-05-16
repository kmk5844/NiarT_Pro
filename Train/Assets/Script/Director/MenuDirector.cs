using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDirector : MonoBehaviour
{
    public GameObject UI_Option;
    public GameObject Demo;
    public AudioClip MainMenuBgm;

    private void Start()
    {
        MMSoundManagerSoundPlayEvent.Trigger(MainMenuBgm, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
    }

    public void Click_Stroy_Mode()
    {
        LoadingManager.LoadScene("Station");
    }

    public void Click_Infinite_Mode()
    {
        Demo.SetActive(true);
    }

    public void Click_Back_Button()
    {
        Demo.SetActive(false);
    }

    public void Click_Option_Mode()
    {
        UI_Option.SetActive(true);
    }

    public void Click_Option_Back()
    {
        UI_Option.SetActive(false);
    }
    public void Click_Exit_Mode()
    {
        Application.Quit();
    }
}
