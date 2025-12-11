using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StorySelectButton : MonoBehaviour
{
    DialogSystem dialogSystem;
    public int SelectButtonNum;
    public TextMeshProUGUI StoryText;

    public void Setting_Director(DialogSystem dialog)
    {
        if (!dialogSystem)
        {
            dialogSystem = dialog;
        }
    }

    public void Setting(string str)
    {
        gameObject.SetActive(true);
        StoryText.text = str;
    }

    public void Init()
    {
        gameObject.SetActive(false);

    }

    public void onClick()
    {
        dialogSystem.SetSelectButtonNum(SelectButtonNum);
        //Debug.Log("Å¬¸¯");
    }
}