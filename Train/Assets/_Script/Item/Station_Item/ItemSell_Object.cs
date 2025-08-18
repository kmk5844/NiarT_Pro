using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ItemSell_Object : MonoBehaviour
{
    public Station_Store StoreDirector;
    public ItemDataObject item;

    public string item_name;
    public string item_information;
    public int item_Count;
    public int item_pride;

    [Header("정보 표시")]
    public Image item_icon_object;
    public LocalizeStringEvent item_Name_text;
    public TextMeshProUGUI item_Pride_text;
    public TextMeshProUGUI item_Count_text;
    public GameObject SelectObject;

    private void Start()
    {
        item_icon_object.sprite = item.Item_Sprite;

        item_Name_text.StringReference.TableReference = "ItemData_Table_St";
        item_Name_text.StringReference.TableEntryReference = "Item_Name_" + item.Num;

        item_information = item.Item_Information;

        item_pride = item.Item_Sell_Pride;
        item_Pride_text.text = item_pride + "G";

        item_Count = item.Item_Count;
        item_Count_text.text = item_Count.ToString();

        //item_use = item.Use_Flag;
    }
    public void Click_Item()
    {
        StoreDirector.Click_ItemCheck(this.gameObject, false);
        SelectObject.SetActive(true);
    }

    public bool Check_ItemCount()
    {
        item_Count = item.Item_Count;

        if (item_Count != 0)
        {
            item_Count_text.text = item_Count.ToString();
            return true;
        }
        else if (item_Count == 0)
        {
            return false;
        }
        return false;
    }
}