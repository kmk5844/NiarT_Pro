using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeList : MonoBehaviour
{
    ItemDataObject item;
    public Image image;
    public Button button;

    public void SettingItem(ItemDataObject _item, ExchangeDirector _director)
    {
        item = _item;
        image.sprite = item.Item_Sprite;
        button.onClick.AddListener(() => _director.Click_Item(item));
    }
}
