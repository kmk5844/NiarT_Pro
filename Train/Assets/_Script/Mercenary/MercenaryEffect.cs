using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryEffect : MonoBehaviour
{
    public ParticleSystem HealEffect;

    public void HealEffectPlay()
    {
        HealEffect.Play();
    }
}
