using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingDirector : MonoBehaviour
{
    public AudioClip BGM;

    void Start()
    {
        if (SteamAchievement.instance != null)
        {
            SteamAchievement.instance.Achieve("EARLY_END");
        }
        else
        {
            Debug.Log("EARLY_END");
        }

    }

    public void EndEvent()
    {
        try
        {
            LoadingManager.LoadScene("1.MainMenu");
        }
        catch
        {
            SceneManager.LoadScene("1.MainMenu");
        }
    }

    public void StartBGM()
    {
        MMSoundManagerSoundPlayEvent.Trigger(BGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: false, ID: 10);
    }

    public void EndBGM()
    {
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, 10);
    }
}
