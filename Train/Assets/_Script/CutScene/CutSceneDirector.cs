using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutSceneDirector : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public LocalizeStringEvent text;

    public AudioClip CutSceneBGM;
    public AudioClip BookSoundSFX;
    public AudioClip Bell1_SFX;
    public AudioClip Print_SFX;
    public AudioClip Bell2_SFX;

    int i;
    private void Start()
    {
        Cursor.visible = false;
        i = 0;
        text.StringReference.TableReference = "CutScene_St";
        MMSoundManagerSoundPlayEvent.Trigger(CutSceneBGM, MMSoundManager.MMSoundManagerTracks.Music, transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown("]"))
        {
            DataManager.Instance.playerData.SA_StoryNum_Chnage(0);
            SceneManager.LoadScene("Story");
            DataManager.Instance.storyData.StoryList[0].ChangeFlag(true);
        }
    }

    public void ChnageText()
    {
        text.StringReference.TableEntryReference = "Cut_" + i;
        i++;
    }

    public void EndCutScene()
    {
        Cursor.visible = true;
        DataManager.Instance.playerData.SA_StoryNum_Chnage(0);
        SceneManager.LoadScene("Story");
        DataManager.Instance.storyData.StoryList[0].ChangeFlag(true);
    }

    public void SFX(int i)
    {
        if(i == 0)
        {
            MMSoundManagerSoundPlayEvent.Trigger(BookSoundSFX, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
        }else if(i == 1)
        {
            MMSoundManagerSoundPlayEvent.Trigger(Bell1_SFX, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
        }else if(i == 2)
        {
            MMSoundManagerSoundPlayEvent.Trigger(Print_SFX, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
        }else if(i == 3)
        {
            MMSoundManagerSoundPlayEvent.Trigger(Bell2_SFX, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
        }
    }
}
