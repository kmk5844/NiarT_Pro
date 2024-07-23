using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUse_Window_53_ToggleObject : MonoBehaviour
{
    public ItemDataObject item;
    public Image Icon_Image;
    public TextMeshProUGUI Icon_Count;

    private void Start()
    {
        Icon_Image.sprite = item.Item_Sprite;
        Icon_Count.text = item.Item_Count.ToString();
    }

    public void Check_Count()
    {
        Icon_Count.text = item.Item_Count.ToString();
    }
}