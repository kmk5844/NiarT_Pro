using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clip;
    Button btn;
    public bool onClickFlag;

    void Start()
    {
        if (GetComponent<Button>() != null)
        {
            btn = GetComponent<Button>();
            if (clip == null)
            {
                //Debug.Log("클립 없다 삽입해라 : " + gameObject.GetComponentInParent<GameObject>().name +"/" + gameObject.name); 
            }
            if (clip != null)
            {
                if (!onClickFlag)
                {
                    btn.onClick.AddListener(btnSound);
                }
            }
        }
    }

    public void btnSound()
    {
        MMSoundManagerSoundPlayEvent.Trigger(clip, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
    }
}