using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemList_Object : MonoBehaviour
{
    public Station_Inventory Inventory_Director;
    public ItemDataObject item;

    public string item_name;
    public string item_information;
    public int item_count;
    public bool item_use;

    [Header("정보 표시")]
    public Image item_icon_object;
    public TextMeshProUGUI item_object_text_count;
    public ItemList_Tooltip item_tooltip_object;

    bool item_information_Flag; // 정보 출력 플래그
    bool item_mouseOver_Flag; // 이미 올려져 있다는 플래그

    private void Start()
    {
        item_information_Flag = false;
        item_name = item.Item_Name;
        item_information = item.Item_Information;
        item_count = item.Item_Count;
        item_use = item.Use_Flag;
        item_icon_object.sprite = item.Item_Sprite;
        item_object_text_count.text = item_count.ToString();
    }

    private void Update()
    {
        if (StationDirector.TooltipFlag)
        {
            if (item_information_Flag)
            {
                item_tooltip_object.Tooltip_ON(item.Item_Sprite,item_name, item_information, item_use, 0);
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
        else
        {
            if (item_information_Flag)
            {
                item_information_Flag = false;
                item_mouseOver_Flag = false;
                item_tooltip_object.Tooltip_Off();
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
        if (item_use)
        {
            Inventory_Director.UseItemStatus_Click(item);
        }
    }

    public bool Check_ItemCount()
    {
       item_count = item.Item_Count;

        if(item_count != 0)
        {
            item_object_text_count.text = item_count.ToString();
            return true;
        }
        else if(item_count == 0)
        {
            return false;
        }
        return false;
    }
}