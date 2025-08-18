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
    public GameObject item_use_object;

    bool item_information_Flag; // 정보 출력 플래그
    bool item_mouseOver_Flag; // 이미 올려져 있다는 플래그

    private void Start()
    {
        item_information_Flag = false;
        item_name = item.Item_Name;
        item_information = item.Item_Information;
        item_count = item.Item_Count;
        //item_use = item.Use_Flag;
        item_icon_object.sprite = item.Item_Sprite;
        item_object_text_count.text = item_count.ToString();

/*        if (item_use_object != null)
        {
            if (item_use)
            {
                item_use_object.SetActive(true);

                if (item.Num == 53 && item_count < 10)
                {
                    item_use_object.SetActive(false);
                }
            }
            else
            {
                item_use_object.SetActive(false);
            }
        }*/
    }

    private void Update()
    {
        if (item_information_Flag)
        {
            item_tooltip_object.Tooltip_ON(item.Item_Sprite, item.Num, item_use, 0);
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

/*        if (StationDirector.TooltipFlag)
        {

        }
        else
        {
            if (item_information_Flag)
            {
                item_information_Flag = false;
                item_mouseOver_Flag = false;
                item_tooltip_object.Tooltip_Off();
            }
        }*/
    }

    public void OnMouseEnter()
    {
        item_information_Flag = true;
    }

    public void OnMouseExit()
    {
        item_information_Flag = false;
    }

/*    public void OnMouseClick()
    {
        if (item_use)
        {
            Inventory_Director.UseItemStatus_Click(item);
        }
    }*/

    public bool Check_ItemCount()
    {
       item_count = item.Item_Count;

        if(item_count != 0)
        {
            item_object_text_count.text = item_count.ToString();
            /*if(item.Num == 53 && item_count >= 10)
            {
                item_use_object.SetActive(true);
            }
            else
            {
                item_use_object.SetActive(false);
            }*/
            //item_use_object.SetActive(false);
            return true;
        }
        else if(item_count == 0)
        {
            return false;
        }
        return false;
    }
}