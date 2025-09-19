using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDirector : MonoBehaviour
{
    Train_InGame trainDirector;
    public ParticleSystem FirstEffect;
    public ParticleSystem SecondEffect;

    // Start is called before the first frame update
    void Start()
    {
        trainDirector = GetComponentInParent<Train_InGame>();

    }

    // Update is called once per frame
    void Update()
    {
        if (trainDirector.HP_Parsent < 50f)
        {
            if (!FirstEffect.isPlaying)
            {
                FirstEffect.Play();
            }
        }
        else
        {
            if (FirstEffect.isPlaying)
            {
                FirstEffect.Stop();
            }
        }

        if (trainDirector.HP_Parsent < 25f)
        {
            if (!SecondEffect.isPlaying)
            {
                SecondEffect.Play();
            }
        }
        else
        {
            if (SecondEffect.isPlaying)
            {
                SecondEffect.Stop();
            }
        }
    }
}
