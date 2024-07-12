using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuy_Object : MonoBehaviour
{
    public Station_Store StoreDirector;
    public ItemDataObject item;

    public string item_name;
    public string item_information;
    public int item_pride;
    public bool item_use;

    [Header("���� ǥ��")]
    public Image item_icon_object;
    public ItemList_Tooltip item_tooltip_object;

    bool item_information_Flag; // ���� ��� �÷���
    bool item_mouseOver_Flag; // �̹� �÷��� �ִٴ� �÷���

    private void Start()
    {
        item_information_Flag = false;
        item_name = item.Item_Name;
        item_information = item.Item_Information;
        item_pride = item.Item_Buy_Pride;
        item_use = item.Use_Flag;
        item_icon_object.sprite = item.Item_Sprite;
    }

    private void Update()
    {
        if (item_information_Flag)
        {
            item_tooltip_object.Tooltip_ON(item_name, item_information, item_use, item_pride);
            item_mouseOver_Flag = true;
        }
        else
        {
            if (item_mouseOver_Flag)
            {
                item_tooltip_object.Tooltip_Off();
                item_mouseOver_Flag = false;
            }
        }
    }

    public void OnMouseEnter()
    {
        item_information_Flag = true;
    }

    public void OnMouseExit()
    {
        item_information_Flag = false;
    }

    public void OnMouseClick()
    {
        StoreDirector.Open_BuyAndSell_Item_Window(item, true);
    }
}