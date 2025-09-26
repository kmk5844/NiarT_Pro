using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffect : MonoBehaviour
{
    public ParticleSystem BounsCoinParticle;
    
    public void BounsCoinPlay()
    {
        BounsCoinParticle.Play();
    }
}
