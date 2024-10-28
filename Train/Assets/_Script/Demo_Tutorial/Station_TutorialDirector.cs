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

    public GameObject[] TutorialObject;

    bool ClickFlag;
    public TextMeshProUGUI nextText;

    private void Start()
    {
        Count = 0;
        MaxCount = TutorialObject.Length;
        ClickFlag = true;
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
            TutorialObject[Count-1].SetActive(false);
            //StationImage.AssetReference.TableEntryReference = st + Count;
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

    void TutorialEnd()
    {
        tutorialData.ChangeFlag(Tutorial_Index);
        gameObject.SetActive(false);
    }
}