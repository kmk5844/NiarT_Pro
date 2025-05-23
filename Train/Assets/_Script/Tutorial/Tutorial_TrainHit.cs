using Cinemachine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_TrainHit : MonoBehaviour
{
    Tutorial_Train train;
    public GameObject Hit_Effect;
    CinemachineImpulseSource impulseObject;
    public AudioClip HitSFX;

    private void Start()
    {
        train = GetComponentInParent<Tutorial_Train>();
        try
        {
            impulseObject = GetComponentInChildren<CinemachineImpulseSource>();
        }
        catch { }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            train.TrainHIt(200, 10);
            if(impulseObject != null)
            {
                CameraShakeManager.instance.CameraShake(impulseObject);
            }
            MMSoundManagerSoundPlayEvent.Trigger(HitSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
            Instantiate(Hit_Effect, collision.transform.localPosition, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}