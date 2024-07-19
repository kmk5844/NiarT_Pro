using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquip_Object : MonoBehaviour
{
    public Station_GameStart GameStartDirector;
    public ItemDataObject item;

    public string item_name;
    public string item_information;
    public int item_count;
    public bool item_use;
    public int item_max;
    public bool item_equip;
    public GameObject Item_Panel;

    [Header("���� ǥ��")]
    public Image item_icon_object;
    public TextMeshProUGUI item_object_text_count;
    public ItemList_Tooltip item_tooltip_object;

    bool item_information_Flag; // ���� ��� �÷���
    bool item_mouseOver_Flag; // �̹� �÷��� �ִٴ� �÷���

    private void Start()
    {
        item_information_Flag = false;
        item_name = item.Item_Name;
        item_information = item.Item_Information;
        item_count = item.Item_Count;
        item_use = item.Use_Flag;
        item_max = item.Max_Equip;
        item_icon_object.sprite = item.Item_Sprite;
        item_object_text_count.text = item_count.ToString();
        Change_EquipFlag();
    }

    private void Update()
    {
        if (item_information_Flag)
        {
            item_tooltip_object.Tooltip_ON(item.Item_Sprite, item_name, item_information, item_use, item_max);
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

    public void Change_EquipFlag()
    {
        if (item_equip)
        {
            Item_Panel.SetActive(true);
        }
        else
        {
            Item_Panel.SetActive(false);
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
        if(!item_equip)
        {
            GameStartDirector.Open_ItemCountWindow(item.Num);
        }
    }
}