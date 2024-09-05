using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BackLog_object : MonoBehaviour
{
    public TextMeshProUGUI Name_text;
    public TextMeshProUGUI Dialog_text;

    public void SetString(string name, string dialog)
    {
        Name_text.text = name + " : ";
        Dialog_text.text = dialog;
    }
}