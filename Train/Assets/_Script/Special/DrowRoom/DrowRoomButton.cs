using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrowRoomButton : MonoBehaviour
{
    public ItemDataObject ItemDataObject;
    DrowRoomDirector director;
    public bool flag;
    public Button button;
    public Image image;

    public void ClickButton()
    {
        flag = true;
        button.gameObject.SetActive(false);
        director.CheckCard();
    }

    public void SettingItem(ItemDataObject item, DrowRoomDirector dir)
    {
        flag = false;
        ItemDataObject = item;
        image.sprite = ItemDataObject.Item_Sprite;
        director = dir;
    }
}
