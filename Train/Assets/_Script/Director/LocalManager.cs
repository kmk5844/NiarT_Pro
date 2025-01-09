using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalManager : MonoBehaviour
{
    public SA_LocalData SA_Local;
    bool isChanging;
    public int index;
    int Local_Max;

    private void Awake()
    {
        Local_Max = LocalizationSettings.AvailableLocales.Locales.Count;
        SA_Local.Load();
        index = SA_Local.Local_Index; // Default°¡ ¿µ¾î
        ChangeLocale();
    }

    public void clickOption(int x)
    {
        index = x;
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
        SA_Local.SA_Change_Local(index);
        isChanging = false;
    }
}