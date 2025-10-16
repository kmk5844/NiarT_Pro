using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ExchangeList : MonoBehaviour
{
    ItemDataObject item;
    public Image image;
    public Button button;
    public TextMeshProUGUI item_count_Text;

    public void SettingItem(ItemDataObject _item, ExchangeDirector _director)
    {
        item = _item;
        image.sprite = item.Item_Sprite;
        item_count_Text.text = "X " + item.Item_Count;
        button.onClick.AddListener(() => _director.Click_Item(item));
    }
}
