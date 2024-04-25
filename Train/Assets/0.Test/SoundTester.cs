using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
public class SoundTester : MonoBehaviour
{
    public AudioClip my_Clip;

    private AudioSource _myAudioSource;

    private void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            PlayMySound();
        }
        
        if (Input.GetKeyDown("l"))
        {
            StopMySound();
        }
    }

    public void PlayMySound()
    {
        _myAudioSource = MMSoundManagerSoundPlayEvent.Trigger(my_Clip, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position,persistent:true, loop:true);

/*        MMSoundManagerPlayOptions option;
        option = MMSoundManagerPlayOptions.Default;
        option.Volume = 0.5f;
        option.Loop = true;

        MMSoundManagerSoundPlayEvent.Trigger(my_Clip, option);

        MMSoundManager.Instance.PlaySound(my_Clip, option);*/
    }

    public void StopMySound()
    {
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, 0,_myAudioSource);
    }
}
