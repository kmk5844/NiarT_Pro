using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

public class Lose_UIDirector : MonoBehaviour
{
    public UIDirector uidirector;
    public GameObject LoseMemo;
    public LocalizeStringEvent LoseText;

    private void Start()
    {
        LoseMemo.SetActive(false);
        LoseText.StringReference.TableReference = "InGame_Table_St";
    }

    public void Fail_EvnetTrigger()
    {
        if (uidirector.LoseFlag)
        {
            LoseMemo.SetActive(true);
            LoseText.StringReference.TableEntryReference = "UI_Lose_Text_" + uidirector.LoseText_Num;
        }
    }
}
