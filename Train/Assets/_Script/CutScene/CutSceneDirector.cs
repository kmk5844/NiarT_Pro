using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static MoreMountains.Tools.MMSoundManager;

public class CutSceneDirector : MonoBehaviour
{
    public AudioListener audioListener;
    public bool NewFlag;
    bool additiveFlag;

    public PlayableDirector timelineDirector;
    public LocalizeStringEvent text_Name;
    public LocalizeStringEvent text_Dialog;

    public AudioClip CutSceneBGM;
    public AudioClip BookSoundSFX;
    public AudioClip Bell1_SFX;
    public AudioClip Print_SFX;
    public AudioClip Bell2_SFX;

    [Header("오디오설정")]
    public AudioClip skip_SFX;
    public GameObject eventSystem; 

    int i;
    private void Start()
    {
        Scene currentScene = gameObject.scene;
        if(currentScene != SceneManager.GetActiveScene())
        {
            additiveFlag = true;
            audioListener.enabled = false;
            StationDirector.DicFlag = true;
            Destroy(eventSystem);
        }
        else
        {
            additiveFlag = false;
            audioListener.enabled = true;
        }

        MMSoundManagerSoundPlayEvent.Trigger(CutSceneBGM, MMSoundManager.MMSoundManagerTracks.Music, transform.position, ID : 10);
        i = 0;
        text_Name.StringReference.TableReference = "CutScene_St";
        text_Dialog.StringReference.TableReference = "CutScene_St";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            End();
        }
    }

    public void ChnageText()
    {
        if (!NewFlag)
        {
            text_Dialog.StringReference.TableEntryReference = "Cut_" + i;
            i++;
        }
        else
        {
            text_Name.StringReference.TableEntryReference = "NewCut_Name_" + i;
            text_Dialog.StringReference.TableEntryReference = "NewCut_Dialog_" + i;
            i++;
        }
    }

    public void EndCutScene()
    {
        End();
    }
    void End()
    {
        if (!additiveFlag)
        {
            MMSoundManagerSoundPlayEvent.Trigger(skip_SFX, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position);
            DataManager.Instance.playerData.SA_StoryNum_Chnage(0);
            SceneManager.LoadScene("Story");
            DataManager.Instance.storyData.StoryList[0].ChangeFlag(true);
        }
        else
        {
            MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, 10);
            StationDirector.DicFlag = false;
            StationDirector.Dic_BGM_Flag = true;
            SceneManager.UnloadSceneAsync("CutScene");
        }
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
