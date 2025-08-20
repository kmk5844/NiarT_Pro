using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Stroy_FadeOut : MonoBehaviour
{
    public StoryDirector storydirector;
    public void FadeOUt()
    {
        if (!storydirector.additiveFlag)
        {
            GameManager.Instance.Story_End();
        }else{
            MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, 10);
            StationDirector.DicFlag = false;
            StationDirector.Dic_BGM_Flag = true;
            SceneManager.UnloadSceneAsync("Story");
        }
    }
}