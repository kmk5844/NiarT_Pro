using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarbageCard : MonoBehaviour
{
    ItemDataObject Item;
    public Image imageSprite;

    public void Setting_item(ItemDataObject item_)
    {
        Item = item_;
        imageSprite.sprite = Item.Item_Sprite;
        Item.Item_Count_UP();
    } 
}
