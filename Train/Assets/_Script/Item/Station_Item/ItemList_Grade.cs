using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList_Grade : MonoBehaviour
{
    public ItemDataObject Item;
    public ItemList_Tooltip ItemList_ToolTip_object;
    Image ItemImage;

    bool item_information_Flag; // 정보 출력 플래그
    bool item_mouseOver_Flag; // 이미 올려져 있다는 플래그
    public bool item_Change_Flag;

    private void Start()
    {
        ItemImage = GetComponent<Image>();
    }

    private void Update()
    {
/*        if (item_Change_Flag)
        {
            item_Change_Flag = false;
            ItemImage.sprite = Item.Item_Sprite;
        }

        if (StationDirector.TooltipFlag)
        {
            if(Item.Num != -1) //비어있는 숫자가 아니라면..
            {
                if (item_information_Flag)
                {
                    ItemList_ToolTip_object.Tooltip_ON(Item.Item_Sprite, Item.Num, false, 0);
                    item_mouseOver_Flag = true;
                }
                else
                {
                    if (item_mouseOver_Flag)
                    {
                        ItemList_ToolTip_object.Tooltip_Off();
                        item_mouseOver_Flag = false;
                    }
                }
            }
        }
        else
        {
            if (item_information_Flag)
            {
                item_information_Flag = false;
                item_mouseOver_Flag = false;
                ItemList_ToolTip_object.Tooltip_Off();
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
}
