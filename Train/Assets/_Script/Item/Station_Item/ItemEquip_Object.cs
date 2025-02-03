using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquip_Object : MonoBehaviour
{
    public bool EquipAndInventory; //Equip == True, Inventory : 
    public int EquipObjectNum = -1;
    public Station_GameStart GameStartDirector; // 제거 예정
    public PlayerReadyDirector playerReadyDirector;
    public ItemDataObject item;

    public string item_name;
    public string item_information;
    public int item_count;
    public bool item_use;
    public int item_max;
    public bool item_equip;
    public GameObject Item_DragImage;
    public GameObject Item_MouseOver_Frame;

    [Header("정보 표시")]
    public Image item_icon_object;
    public TextMeshProUGUI item_object_text_count;
    //public ItemList_Tooltip item_tooltip_object;

    bool item_information_Flag; // 정보 출력 플래그
    bool item_mouseOver_Flag; // 이미 올려져 있다는 플래그

    float mouseHoldTime;

    bool mouseHold;
    bool mouseDrag;

    private void Start()
    {
        Item_MouseOver_Frame.SetActive(false);
    }

    private void Update()
    {
        if (item_information_Flag)
        {
            if (item != null)
            {
                playerReadyDirector.ItemInformation_Setting(item.Item_Sprite, item.Num);
                item_mouseOver_Flag = true;
                Item_MouseOver_Frame.SetActive(true);
            }
        }
        else
        {
            if (item_mouseOver_Flag)
            {
                item_mouseOver_Flag = false;
                Item_MouseOver_Frame.SetActive(false);
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
                playerReadyDirector.DragingItemObject = Item_DragImage;
            }
            else
            {
                mouseDrag = false;
            }
        }

        if (mouseDrag)
        {
            Item_DragImage.transform.position = Input.mousePosition;
            playerReadyDirector.Draging_Item = item;
        }
    }

    public void SetSetting(ItemDataObject _item, GameObject _Item_DragImage, PlayerReadyDirector _playerReadyDirector)
    {
        item = _item;
        Item_DragImage = _Item_DragImage;
        playerReadyDirector = _playerReadyDirector;
        Equip_Item();
    }

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
            if (playerReadyDirector.itemListData.SA_Player_ItemData.Equiped_Item[EquipObjectNum] == -1)
            {
                item_object_text_count.text = "";
            }
            else {
                item_object_text_count.text = playerReadyDirector.itemListData.SA_Player_ItemData.Equiped_Item_Count[EquipObjectNum].ToString();
            }
        }
    }

    public void Init_Item()
    {
        item = playerReadyDirector.itemListData.SA_Player_ItemData.EmptyObject;
        playerReadyDirector.itemListData.SA_Player_ItemData.Empty_Item(EquipObjectNum);
        Equip_Item();
    }

    public void OnMouseEnter()
    {
        if (EquipAndInventory && playerReadyDirector.Draging_Item != null)
        {
            item = playerReadyDirector.Draging_Item;
            Item_DragImage = playerReadyDirector.DragingItemObject;

            if (playerReadyDirector.HoldAndDragFlag_Item)
            {
                playerReadyDirector.DragItemCount++;
                playerReadyDirector.mouseOnEquipedFlag = true;
            }

            if (playerReadyDirector.DragItemCount == 3)
            {
                playerReadyDirector.EndHoldItem = this.gameObject;
                playerReadyDirector.OpenItemCountWindow(this, true);
                playerReadyDirector.DragItemCount++;
            }
        }

        if (item != playerReadyDirector.EmptyItemObject)
        {
            if (item != null)
            {
                item_information_Flag = true;
            }
        }
    }


    public void OnMouseExit()
    {
        if (item != playerReadyDirector.EmptyItemObject)
        {
            if (item != null)
            {
                item_information_Flag = false;
            }
        }
    }

    public void OnMouseDown()
    {
        if (item != playerReadyDirector.EmptyItemObject)
        {
            mouseHold = true;

            playerReadyDirector.BeforeHoldItem = this.gameObject;
            playerReadyDirector.DragItemCount = 0;

            playerReadyDirector.DragItemCount++; // +1
        }
    }

    public void OnMouseUp()
    {
        mouseHoldTime = 0;
        playerReadyDirector.HoldAndDragFlag_Item = true;
        playerReadyDirector.mouseHoldAndDragNotTime = 0f;

        if (Item_DragImage != null)
        {
            Item_DragImage.SetActive(false);
        }

        playerReadyDirector.DragItemCount++; // +1
        StartCoroutine(CheckDelay());

        mouseHold = false;
        mouseDrag = false;
    }

    IEnumerator CheckDelay()
    {
        yield return new WaitForSeconds(0.00001f);
        playerReadyDirector.CheckFlag = true;
    }
}