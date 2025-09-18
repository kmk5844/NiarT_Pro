using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_SpriteWarning : MonoBehaviour
{
    public SpriteRenderer[] trainSprite;
    Train_InGame train;
    public GameObject WarningEffect;


    private void Start()
    {
        train = GetComponent<Train_InGame>();
        trainSprite = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (train.warningFlag)
        {
            if (WarningEffect && !WarningEffect.activeSelf)
            {
                WarningEffect.SetActive(true);
            }
        }
        else
        {
            if (WarningEffect && WarningEffect.activeSelf)
            {
                WarningEffect.SetActive(false);
            }
        }

        if (train.redSpriteFlag)
        {
            foreach (SpriteRenderer sprite in trainSprite)
            {
                sprite.color = Color.red;
            }
        }
        else
        {
            foreach (SpriteRenderer sprite in trainSprite)
            {
                sprite.color = Color.white;
            }
        }
    }
}
