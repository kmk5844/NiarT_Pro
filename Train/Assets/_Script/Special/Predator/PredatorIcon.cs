using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PredatorIcon : MonoBehaviour
{
    public Sprite[] IconSprite;
    public int num;
    public Image IconImage;


    public void Setting(int _num)
    {
        num = _num;
        IconImage.sprite = IconSprite[num];
    }
}