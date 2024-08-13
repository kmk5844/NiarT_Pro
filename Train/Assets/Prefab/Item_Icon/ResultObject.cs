using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultObject : MonoBehaviour
{
    public ItemDataObject item;

    public string item_name;
    public string item_information;

    [Header("정보 표시")]
    public Image item_icon_object;
    public ResultItem_Tooltip item_tooltip_object;

    bool item_information_Flag; // 정보 출력 플래그
    bool item_mouseOver_Flag; // 이미 올려져 있다는 플래그

    private void Start()
    {
        item_information_Flag = false;
        item_name = item.Item_Name;
        item_information = item.Item_Information;
        item_icon_object.sprite = item.Item_Sprite;
    }

    private void FixedUpdate()
    {
        if (item_information_Flag)
        {
            item_tooltip_object.Tooltip_ON(item.Item_Sprite, item.Num, false, 0);
            item_mouseOver_Flag = true;
        }
        else
        {
            if (item_mouseOver_Flag)
            {
                item_tooltip_object.Tooltip_Off();
            }
        }
    }

    public void OnMouseEnter()
    {
        item_information_Flag = true;
        item_tooltip_object.Tooltip_ON(item.Item_Sprite, item.Num, false, 0);
    }

    public void OnMouseExit()
    {
        item_information_Flag = false;
        item_tooltip_object.Tooltip_Off();
    }
}
