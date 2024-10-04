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
                Debug.Log("Ŭ�� ���� �����ض�");
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
