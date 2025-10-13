using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrowRoomButton : MonoBehaviour
{
    public ItemDataObject ItemDataObject_Item;
    DrowRoomDirector director;
    public bool flag;
    public Button button;
    public Image image;
    

    public void ClickButton()
    {
        flag = true;
        button.gameObject.SetActive(false);
        ItemDataObject_Item.Item_Count_UP();
        director.PlayerCoinPay();
        director.CheckCard();
    }

    public void SettingItem(ItemDataObject item, DrowRoomDirector dir)
    {
        flag = false;
        ItemDataObject_Item = item;
        image.sprite = ItemDataObject_Item.Item_Sprite;
        director = dir;
    }
}
