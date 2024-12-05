using PixelCrushers.DialogueSystem.Articy.Articy_4_0;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquip_Object : MonoBehaviour
{
    public Station_GameStart GameStartDirector; // 제거 예정
    public SubStageSelectDirector SubDirector;
    public ItemDataObject item;

    public string item_name;
    public string item_information;
    public int item_count;
    public bool item_use;
    public int item_max;
    public bool item_equip;
    public GameObject Item_Panel;
    public GameObject Item_DragImage;

    [Header("정보 표시")]
    public Image item_icon_object;
    public TextMeshProUGUI item_object_text_count;
    public ItemList_Tooltip item_tooltip_object;

    bool item_information_Flag; // 정보 출력 플래그
    bool item_mouseOver_Flag; // 이미 올려져 있다는 플래그

    float mouseHoldTime;
    bool mouseHold;
    bool mouseDrag;

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
            item_tooltip_object.Tooltip_ON(item.Item_Sprite, item.Num, item_use, item_max);
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

        if (mouseHold)
        {
            mouseHoldTime += Time.deltaTime;
        }

        if(mouseHoldTime > 0.5f)
        {
            mouseDrag = true;
            Item_DragImage.SetActive(true);
        }
        else
        {
            mouseDrag = false;
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
        item_tooltip_object = _Tooltip;
        Item_DragImage = _Item_DragImage;
        SubDirector = _SubDirector;
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
        item_information_Flag = true;
    }

    public void OnMouseExit()
    {
        item_information_Flag = false;
    }

    public void OnMouseDown()
    {
        mouseHold = true;
        SubDirector.DragFlag = true;
    }

    public void OnMouseUp()
    {
        mouseHoldTime = 0;
        Item_DragImage.SetActive(false);
        SubDirector.DragFlag = false;
        mouseHold = false;
        mouseDrag = false;
    }

    public void OnMouseClick()
    {
/*        if (!item_equip)
        {
            try
            {
                GameStartDirector.Open_ItemCountWindow(item.Num, false);
            }
            catch
            {
                Debug.Log("정거장 전용 스크립트");
            }
        }*/
    }
}