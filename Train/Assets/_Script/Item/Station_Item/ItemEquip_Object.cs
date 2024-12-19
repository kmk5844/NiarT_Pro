using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquip_Object : MonoBehaviour
{
    public bool EquipAndInventory; //Equip == True, Inventory : 
    public int EquipObjectNum = -1;
    public Station_GameStart GameStartDirector; // ���� ����
    public SubStageSelectDirector SubDirector;
    public ItemDataObject item;

    public string item_name;
    public string item_information;
    public int item_count;
    public bool item_use;
    public int item_max;
    public bool item_equip;
    public GameObject Item_DragImage;

    [Header("���� ǥ��")]
    public Image item_icon_object;
    public TextMeshProUGUI item_object_text_count;
    //public ItemList_Tooltip item_tooltip_object;

    bool item_information_Flag; // ���� ��� �÷���
    bool item_mouseOver_Flag; // �̹� �÷��� �ִٴ� �÷���

    float mouseHoldTime;

    bool mouseHold;
    bool mouseDrag;

    bool EquipDragFlag;

    private void Start()
    {
        Change_EquipFlag();
    }

    private void Update()
    {
        if (item_information_Flag)
        {
            if (item != null)
            {
                SubDirector.ItemInformation_Setting(item.Item_Sprite, item.name, item_information);
                item_mouseOver_Flag = true;
            }
        }
        else
        {
            if (item_mouseOver_Flag)
            {
                //SubDirector.Tooltip_Off();
                item_mouseOver_Flag = false;
            }
        }

        if (mouseHold)
        {
            mouseHoldTime += Time.deltaTime;
        }

        if (item != null)
        {
            if (mouseHoldTime > 0.1f)
            {
                mouseDrag = true;
                Item_DragImage.SetActive(true);
                SubDirector.DragingItemObject = Item_DragImage;
            }
            else
            {
                mouseDrag = false;
            }
        }
        if (mouseDrag)
        {
            Item_DragImage.transform.position = Input.mousePosition;
            SubDirector.Draging_Item = item;
        }
    }

    public void SetSetting(ItemDataObject _item, ItemList_Tooltip _Tooltip, GameObject _Item_DragImage, SubStageSelectDirector _SubDirector)
    {
        item = _item;
        //item_tooltip_object = _Tooltip;
        Item_DragImage = _Item_DragImage;
        SubDirector = _SubDirector;
        Equip_Item();
    }

/*    public void DragSetting(ItemDataObject _item, GameObject _Item_DragImage)
    {
        item = _item;
        Item_DragImage = _Item_DragImage;
        Equip_Item();
    }*/

    public void Equip_Item()
    {
        item_information_Flag = false;
        item_name = item.Item_Name;
        item_information = item.Item_Information;
        item_count = item.Item_Count;
        item_use = item.Use_Flag;
        item_max = item.Max_Equip;
        item_icon_object.sprite = item.Item_Sprite;
        if (!EquipAndInventory)
        {
            item_object_text_count.text = item_count.ToString();
        }
        else
        {
            if (SubDirector.itemListData.SA_Player_ItemData.Equiped_Item[EquipObjectNum] == -1)
            {
                item_object_text_count.text = "";
            }
            else {
                item_object_text_count.text = SubDirector.itemListData.SA_Player_ItemData.Equiped_Item_Count[EquipObjectNum].ToString();
            }
        }
    }

    public void Init_Item()
    {
        item = SubDirector.itemListData.SA_Player_ItemData.EmptyObject;
        SubDirector.itemListData.SA_Player_ItemData.Empty_Item(EquipObjectNum);
        Equip_Item();
    }

    public void Change_EquipFlag()
    {
/*        if (item_equip)
        {
            Item_Panel.SetActive(true);
        }
        else
        {
            Item_Panel.SetActive(false);
        }*/
    }

    public void OnMouseEnter()
    {
        if (EquipAndInventory && SubDirector.Draging_Item != null)
        {
            if (SubDirector.DragItemCount < 1)
            {
                SubDirector.EndHoldItem = this.gameObject;

                item = SubDirector.Draging_Item;
                Item_DragImage = SubDirector.DragingItemObject;
                SubDirector.OpenItemCountWindow(this, true);
                SubDirector.DragItemCount++;
            }
        }

        if (item != SubDirector.EmptyItemObject)
        {
            if (item != null)
            {
                item_information_Flag = true;
            }
        }
    }

    public void OnMouseExit()
    {
        if (item != SubDirector.EmptyItemObject)
        {
            if (item != null)
            {
                item_information_Flag = false;
            }
        }
    }

    public void OnMouseDown()
    {
        if (item != SubDirector.EmptyItemObject)
        {
            mouseHold = true;
            SubDirector.BeforeHoldItem = this.gameObject;
            if (EquipAndInventory)
            {
                EquipDragFlag = true;
            }
        }
    }

    public void OnMouseUp()
    {
        mouseHoldTime = 0;
        SubDirector.HoldAndDragFlag = true;
        SubDirector.mouseHoldAndDragNotTime = 0f;
        SubDirector.DragItemCount = 0;

        if (Item_DragImage != null)
        {
            Item_DragImage.SetActive(false);
        }

        mouseHold = false;
        mouseDrag = false;
    }
}