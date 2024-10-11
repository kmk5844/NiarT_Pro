using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class Station_TutorialDirector : MonoBehaviour
{
    public SA_Tutorial tutorialData;
    int Tutorial_Index;
    int Count;
    int MaxCount;

    string st = "";
    public LocalizeSpriteEvent StationImage;

    bool ClickFlag;
    public TextMeshProUGUI nextText;

    [SerializeField]
    station_type tutorialType;

    private void Start()
    {
        Count = 0;
        ClickFlag = true;
        StationImage.AssetReference.TableReference = "Tutorial_Table_Asset";
        CheckType();
        StationImage.AssetReference.TableEntryReference = st + Count;
    }

    private void Update()
    {
        if((Input.GetMouseButtonDown(0)|| Input.GetKeyDown(KeyCode.Space)) && ClickFlag)
        {
            ChangeImage();
            if (Count < MaxCount)
            {
                StartCoroutine(ClickDelay());
            }
        }
    }

    void ChangeImage()
    {
        Count++;
        if(Count < MaxCount)
        {
            StationImage.AssetReference.TableEntryReference = st + Count;
        }
        else
        {
            TutorialEnd();
        }
    }

    IEnumerator ClickDelay()
    {
        ClickFlag = false;
        nextText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        nextText.gameObject.SetActive(true);
        ClickFlag = true;
    }

    void CheckType()
    {
        switch (tutorialType)
        {
            case station_type.station:
                Tutorial_Index = 0;
                st = "Station_";
                MaxCount = 5;
                break;
            case station_type.maintenance:
                Tutorial_Index = 1;
                st = "Main_";
                MaxCount = 4;
                break;
            case station_type.store:
                Tutorial_Index = 2;
                st = "Store_";
                MaxCount = 2;
                break;
            case station_type.traning:
                Tutorial_Index = 3;
                st = "Traning_";
                MaxCount = 6;
                break;
            case station_type.map:
                Tutorial_Index = 4;
                st = "Map_";
                MaxCount = 5;
                break;
        }
    }

    void TutorialEnd()
    {
        tutorialData.ChangeFlag(Tutorial_Index);
        gameObject.SetActive(false);
    }

    public void Tutorial_CheckIndex(int i)
    {
        switch (i)
        {
            case 0:
                tutorialType = station_type.station;
                break;
            case 1:
                tutorialType = station_type.maintenance;
                break;
            case 2:
                tutorialType = station_type.store;
                break;
            case 3:
                tutorialType = station_type.traning;
                break;
            case 4:
                tutorialType = station_type.map;
                break;
        }
    }

    public enum station_type
    {
        station,
        maintenance,
        store,
        traning,
        map
    }
}