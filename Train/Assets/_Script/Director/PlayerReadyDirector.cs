using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class PlayerReadyDirector : MonoBehaviour
{
    public Station_ItemData itemListData;

    [Header("UI_Window")]
    public GameObject[] UI_Window;
    int windowCount;
    public GameObject UI_SubStageSelect;

    [Header("----------------Item------------------")]
    [Space(10)]
    [Header("UI_ItemInformation")]
    public Image UI_Info_ItemIcon;
    public LocalizeStringEvent UI_Info_ItemNameText;
    public LocalizeStringEvent UI_Info_ItemInformationText;

    [Header("UI_ItemCount")]
    public GameObject UI_ItemCount;
    public Image UI_ItemIcon;
    public Slider UI_ItemCountSlider;
    public TextMeshProUGUI UI_ItemCountText;
    public TextMeshProUGUI UI_ItemMaxText;
    public Button UI_ItemCount_YesButton;
    public Button UI_ItemCount_NoButton;
    public ItemEquip_Object Count_ItemEquipObjcet;
    UnityEngine.Events.UnityAction listner_Button_Yes;
    UnityEngine.Events.UnityAction listner_Button_No;

    [Header("아이템 관리")]
    public Transform Inventory_ItemList;
    public GameObject Inventory_ItemObject;
    public ItemEquip_Object[] Equip_ItemObject;
    public Transform Inventory_DragItemList;
    public GameObject Inventory_DragObject;
    public GameObject DragingItemObject;
    public ItemDataObject Draging_Item;

    public bool HoldAndDragFlag;
    public bool CheckFlag;
    public bool mouseOnEquipedFlag;
    [HideInInspector]
    public float mouseHoldAndDragNotTime;
    [HideInInspector]
    public int DragItemCount;
    [HideInInspector]
    public ItemDataObject EmptyItemObject;
    public GameObject BeforeHoldItem;
    public GameObject EndHoldItem;

    void Start()
    {
        windowCount = 0;
        UI_Window[0].SetActive(true);
        UI_Window[1].SetActive(false);
        UI_Window[2].SetActive(false);

        DragItemCount = 0;      
        Instantiate_Item();

        int itemNum = 0;
        for (int i = 0; i < 3; i++)
        {
            itemNum = itemListData.SA_Player_ItemData.Equiped_Item[i];
            ItemDataObject equiped_item;
            if (itemNum == -1)
            {
                equiped_item = itemListData.SA_Player_ItemData.EmptyObject;
            }
            else
            {
                equiped_item = itemListData.SA_ItemList.Item[itemNum];
            }
            Inventory_DragObject.GetComponent<Image>().sprite = equiped_item.Item_Sprite;
            GameObject drag = Instantiate(Inventory_DragObject, Inventory_DragItemList);
            drag.SetActive(false);
            Equip_ItemObject[i].SetSetting(equiped_item, drag, this);
        }
        EmptyItemObject = itemListData.SA_Player_ItemData.EmptyObject;
        UI_Info_ItemIcon.sprite = EmptyItemObject.Item_Sprite;
        UI_Info_ItemNameText.StringReference.TableReference = "ItemData_Table_St";
        UI_Info_ItemInformationText.StringReference.TableReference = "ItemData_Table_St";
        UI_Info_ItemNameText.StringReference.TableEntryReference = "Item_Name_-1";
        UI_Info_ItemInformationText.StringReference.TableEntryReference = "Item_Information_-1";
    }

    void Update()
    {
        if (CheckFlag)
        {
            //true는 이미 작동 중이다.
            if (!mouseOnEquipedFlag)
            {
                if (BeforeHoldItem.GetComponent<ItemEquip_Object>().EquipAndInventory)
                {
                    BeforeHoldItem.GetComponent<ItemEquip_Object>().Init_Item();
                }
            }
            mouseOnEquipedFlag = false;
            CheckFlag = false;
        }

        if (HoldAndDragFlag)
        {
            if (mouseHoldAndDragNotTime < 0.001f)
            {
                mouseHoldAndDragNotTime += Time.deltaTime;
            }
            else
            {
                Draging_Item = null;
                HoldAndDragFlag = false;
            }
        }
    }

    public void OpenItemCountWindow(ItemEquip_Object item, bool Flag)
    {
        Count_ItemEquipObjcet = item;
        UI_ItemIcon.sprite = Count_ItemEquipObjcet.item.Item_Sprite;
        UI_ItemCountSlider.minValue = 0;
        if (Count_ItemEquipObjcet.item.Max_Equip > Count_ItemEquipObjcet.item.Item_Count)
        {
            UI_ItemCountSlider.maxValue = Count_ItemEquipObjcet.item.Item_Count;
        }
        else
        {
            UI_ItemCountSlider.maxValue = Count_ItemEquipObjcet.item.Max_Equip;
        }
        UI_ItemCountSlider.value = 0;

        UI_ItemCountText.color = Color.black;
        UI_ItemCountText.text = "0";
        UI_ItemMaxText.text = ((int)UI_ItemCountSlider.maxValue).ToString();
        UI_ItemCountSlider.onValueChanged.AddListener(ItemCount_ChangeText);

        UI_ItemCount.SetActive(true);
        listner_Button_Yes = () => ItemCount_YesButton(Count_ItemEquipObjcet, Flag);
        UI_ItemCount_YesButton.onClick.AddListener(listner_Button_Yes);
        listner_Button_No = () => ItemCount_NoButton(Count_ItemEquipObjcet);
        UI_ItemCount_NoButton.onClick.AddListener(listner_Button_No);
    }

    public void CloseItemCountWindow()
    {
        UI_ItemCount.SetActive(false);
        UI_ItemCountSlider.onValueChanged.RemoveListener(ItemCount_ChangeText);
        UI_ItemCount_YesButton.onClick.RemoveListener(listner_Button_Yes);
        UI_ItemCount_NoButton.onClick.RemoveListener(listner_Button_No);
        if (EndHoldItem != null)
        {
            EndHoldItem = null;
        }
    }

    public void ItemCount_YesButton(ItemEquip_Object item, bool Flag)
    {
        int itemCount = (int)UI_ItemCountSlider.value;
        if (itemCount == 0)
        {
            item.Init_Item();
            CloseItemCountWindow();
        }
        else
        {
            int listIndex = itemListData.SA_Player_ItemData.Equiped_Item.IndexOf(item.item.Num);
            if (Flag)
            {
                if (listIndex != -1)
                {
                    if (item.EquipObjectNum != listIndex)
                    {
                        Equip_ItemObject[listIndex].Init_Item();
                    }
                }
            }
            itemListData.SA_Player_ItemData.Equip_Item(item.EquipObjectNum, item.item, itemCount);
            item.Equip_Item(); //장착 아이템 오브젝트 정보 변경
            CloseItemCountWindow();
        }
    }

    public void ItemCount_NoButton(ItemEquip_Object item)
    {
        item.Init_Item();
        CloseItemCountWindow();
    }

    void ItemCount_ChangeText(float value)
    {
        if (value <= 0)
        {
            UI_ItemCountText.color = Color.black;
        }
        else
        {
            UI_ItemCountText.color = Color.red;
        }
        UI_ItemCountText.text = value.ToString();
    }

    public void ItemInformation_Setting(Sprite itemSprite, int itemNum)
    {
        UI_Info_ItemIcon.sprite = itemSprite;
        UI_Info_ItemNameText.StringReference.TableEntryReference = "Item_Name_" + itemNum;
        UI_Info_ItemInformationText.StringReference.TableEntryReference = "Item_Information_" + itemNum;
    }



    //구매 및 판매시, 발동
    public void Check_Item()
    {
        foreach (ItemEquip_Object item in Inventory_ItemList.GetComponentsInChildren<ItemEquip_Object>())
        {
            Destroy(item.gameObject);
        }
        Instantiate_Item();
    }

    private void Instantiate_Item()
    {
        foreach (ItemDataObject item in itemListData.Equipment_Inventory_ItemList)
        {
            Inventory_DragObject.GetComponent<Image>().sprite = item.Item_Sprite;
            GameObject drag = Instantiate(Inventory_DragObject, Inventory_DragItemList);
            drag.SetActive(false);
            Inventory_ItemObject.GetComponent<ItemEquip_Object>().SetSetting(item, drag, this);
            Instantiate(Inventory_ItemObject, Inventory_ItemList);
        }
    }


    //UI Change Button

    public void NextButton()
    {
        if(windowCount < 2)
        {
            windowCount++;
            UI_Window[windowCount - 1].SetActive(false);
            UI_Window[windowCount].SetActive(true);
        }
        else
        {
            windowCount = 0;
            UI_Window[2].SetActive(false);
            UI_Window[0].SetActive(true);
        }
    }

    public void PrevButton()
    {
        if(windowCount > 0)
        {
            windowCount--;
            UI_Window[windowCount + 1].SetActive(false);
            UI_Window[windowCount].SetActive(true);
        }
        else
        {
            windowCount = 2;
            UI_Window[0].SetActive(false);
            UI_Window[2].SetActive(true);
        }
    }

    public void ItemTab_StartButton()
    {
        gameObject.SetActive(false);
        UI_SubStageSelect.SetActive(true);
    }
}