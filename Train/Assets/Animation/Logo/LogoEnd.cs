using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoEnd : MonoBehaviour
{
    public AudioClip TrainSound;
    void Start()
    {
        PlayTrainSound();
    }

    void PlayTrainSound()
    {
        MMSoundManagerSoundPlayEvent.Trigger(TrainSound, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position);
    }

    public void Logo_End()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
