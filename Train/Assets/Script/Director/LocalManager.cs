using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalManager : MonoBehaviour
{
    bool isChanging;
    int index;
    int Local_Max;

    private void Awake()
    {
        Local_Max = LocalizationSettings.AvailableLocales.Locales.Count;
        index = 1; // Default가 한국어
        ChangeLocale();
    }

    public void Click_Next_Local()
    {
        if(index + 1 == Local_Max)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        ChangeLocale();
    }

    public void Click_Prev_Local()
    {
        if(index == 0)
        {
            index = Local_Max - 1;
        }
        else
        {
            index--;
        }
        ChangeLocale();
    }

    private void ChangeLocale()
    {
        if (isChanging)
            return;
        StartCoroutine(ChangeRoutine());
    }

    IEnumerator ChangeRoutine()
    {
        isChanging = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        isChanging = false;
    }
}
