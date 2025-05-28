using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class HelpInformation : MonoBehaviour
{
    public Sprite[] help_sprite;
    public Image HelpImage;
    public LocalizeStringEvent HelpText;

    public string Help_String;
    int help_num;

    private void Start()
    {
        help_num = 0;
        HelpText.StringReference.TableReference = "Tutorial_St";

        Check();
    }

    public void ClickNextButton()
    {
        if (help_num < help_sprite.Length - 1)
        {
            help_num++;
            Check();
        }
    }

    public void ClickPrevButton()
    {
        if(help_num > 0)
        {
            help_num--;
            Check();
        }
    }

    void Check()
    {
        HelpImage.sprite = help_sprite[help_num];
        HelpText.StringReference.TableEntryReference = Help_String + "_" + help_num;
    }
}
