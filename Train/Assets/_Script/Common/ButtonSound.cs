using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clip;
    Button btn; 

    void Start()
    {
        if (GetComponent<Button>() != null)
        {
            btn = GetComponent<Button>();
            if (clip == null)
            {
                Debug.Log("클립 없다 삽입해라");
            }
            else
            {
                btn.onClick.AddListener(btnSound);
            }
        }
    }

    public void btnSound()
    {
        MMSoundManagerSoundPlayEvent.Trigger(clip, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
    }
}
