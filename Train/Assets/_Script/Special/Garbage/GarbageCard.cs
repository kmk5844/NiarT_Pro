using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class GarbageCard : MonoBehaviour
{
    ItemDataObject Item;
    public Image imageSprite;
    public LocalizeStringEvent itemName;

    public void Setting_item(ItemDataObject item_)
    {
        Item = item_;
        imageSprite.sprite = Item.Item_Sprite;
        itemName.StringReference.TableReference = "ItemData_Table_St";
        itemName.StringReference.TableEntryReference = "Item_Name_" + Item.Num;
        Item.Item_Count_UP();
    } 
}
