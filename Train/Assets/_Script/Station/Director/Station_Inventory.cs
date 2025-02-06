using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using System;
public class Station_Inventory : MonoBehaviour
{
    StationDirector stationDirector;
    [Header("데이터 관리")]
    [SerializeField]
    Station_ItemData Data_ItemList;
    [Header("생성 관리")]
    public ItemList_Object ItemObject;
    public ItemList_Tooltip TooltipObject;
    [Header("인벤토리에 있는 아이템 리스트")]
    public List<Transform> Transform_ItemList;
    [Header("UI_UseStatus")]
    public GameObject Item_UseStatus_WindowObject;
    public Button tem_UseStatus_AllButton;
    public LocalizeStringEvent Item_UseStatus_Name;
    public Image Item_UseStatus_Icon;
    public TextMeshProUGUI Item_UseStatus_Count_Text;
    public Button Item_UseStatus_YesButton;
    [Header("UI_UseItem")]
    int UI_UseItem_Num;
    public GameObject Item_UseItem_WindowObject;
    public List<GameObject> Item_UseItem_WindowObject_List;
    public GameObject[] Item_UseIcon;  // 0 : 메인 화면, 1 : 일반, 2 : 박스, 3 : 재료

    ItemDataObject useItem;
    [HideInInspector]
    public bool UseWindowFlag;
    public bool UseItemWindowFlag;

    private void Start()
    {
        stationDirector = GetComponentInParent<StationDirector>();

        UI_UseItem_Num = -1;
        ItemObject.Inventory_Director = GetComponent<Station_Inventory>();
        ItemObject.item_tooltip_object = TooltipObject;

        UseWindowFlag = false;
        UseItemWindowFlag = false;

        Item_UseStatus_Name.StringReference.TableReference = "ItemData_Table_St";

        for (int i = 0; i < Transform_ItemList.Count; i++)
        {
            Spawn_Item(i);
        }
    }

    void Spawn_Item(int num)
    {
        switch(num)
        {
            case 0:
                foreach (ItemDataObject itemDataObject in Data_ItemList.Equipment_Inventory_ItemList)
                {
                    ItemObject.item = itemDataObject;
                    Instantiate(ItemObject, Transform_ItemList[num]);
                }
                break;
            case 1:
                foreach (ItemDataObject itemDataObject in Data_ItemList.Common_Inventory_ItemList)
                {
                    ItemObject.item = itemDataObject;
                    Check_Item_Init_Use(1, itemDataObject);
                    Instantiate (ItemObject, Transform_ItemList[num]);
                }
                break;
            case 2:
                foreach (ItemDataObject itemDataObject in Data_ItemList.Box_Inventory_ItemList)
                {
                    ItemObject.item = itemDataObject;
                    Check_Item_Init_Use(2, itemDataObject);
                    Instantiate(ItemObject, Transform_ItemList[num]);
                }
                break;
            case 3:
                foreach (ItemDataObject itemDataObject in Data_ItemList.Material_Inventory_ItemList)
                {
                    ItemObject.item = itemDataObject;
                    Instantiate(ItemObject, Transform_ItemList[num]);
                }
                break;
            case 4:
                foreach (ItemDataObject itemDataObject in Data_ItemList.Quest_Inventory_ItemList)
                {
                    ItemObject.item = itemDataObject;
                    Instantiate(ItemObject, Transform_ItemList[num]);
                }
                break;
        }
    }

    public void UseItemStatus_Click(ItemDataObject itemobject)
    {
        UseWindowFlag = true;
        tem_UseStatus_AllButton.gameObject.SetActive(false);

        Item_UseStatus_Name.StringReference.TableEntryReference = "Item_Name_" + itemobject.Num;
        Item_UseStatus_Icon.sprite = itemobject.Item_Sprite;

        // 한번에 까는 버튼 추가
        if (itemobject.Num == 54 || itemobject.Num == 55 || itemobject.Num == 56)
        {
            tem_UseStatus_AllButton.gameObject.SetActive(true);
            tem_UseStatus_AllButton.onClick.AddListener(() => UseItemStatus_AllButton(itemobject));
        }
        else if(itemobject.Num == 57)
        {
            tem_UseStatus_AllButton.gameObject.SetActive(true);
            tem_UseStatus_AllButton.onClick.AddListener(() => UseItemStatus_AllButton(itemobject));
        }
        else
        {
            tem_UseStatus_AllButton.gameObject.SetActive(false);
        }

        // 낱개 사용 시, 버튼
        Item_UseStatus_YesButton.onClick.AddListener(() => UseItemStatus_YesButton(itemobject));
        Item_UseStatus_WindowObject.SetActive(true);
    }

    public void UseItemStatus_YesButton(ItemDataObject item)
    {
        UseWindowFlag = false;
        UseItemWindowFlag = true;

        Item_UseStatus_WindowObject.SetActive(false);
        Item_UseItem_WindowObject.SetActive(true);
        switch (item.Num)
        {
            case 54:
            case 55:
            case 56:
                UI_UseItem_Num = 0;
                Item_UseItem_WindowObject_List[0].SetActive(true);
                Item_UseItem_WindowObject_List[0].GetComponent<ItemUse_Window_Box>().Random_Box_Open(item.Num, gameObject);
                item.Item_Count_Down();
                Check_ItemList(false, item);
                break;
            case 57:
                UI_UseItem_Num = 1;
                Item_UseItem_WindowObject_List[1].SetActive(true);
                Item_UseItem_WindowObject_List[1].GetComponent<ItemUse_Window_57>().GetPoint(1);
                item.Item_Count_Down();
                Check_ItemList(false, item);
                break;
        }
        Item_UseStatus_YesButton.onClick.RemoveAllListeners();
        tem_UseStatus_AllButton.onClick.RemoveAllListeners();
    }

    public void UseItemStatus_AllButton(ItemDataObject item)
    {
        UseWindowFlag = false;
        UseItemWindowFlag = true;

        Item_UseStatus_WindowObject.SetActive(false);
        Item_UseItem_WindowObject.SetActive(true);
        switch (item.Num)
        {
            case 54:
            case 55:
            case 56:
                UI_UseItem_Num = 3;
                Item_UseItem_WindowObject_List[2].SetActive(true);
                Item_UseItem_WindowObject_List[2].GetComponent<ItemUse_Window_Box_All>().Random_Box_All_Open(item.Num, item.Item_Count, gameObject);
                item.Item_Count_Down(item.Item_Count);
                Check_ItemList(false, item);
                break;
            case 57:
                UI_UseItem_Num = 2;
                Item_UseItem_WindowObject_List[1].SetActive(true);
                Item_UseItem_WindowObject_List[1].GetComponent<ItemUse_Window_57>().GetPoint(item.Item_Count);
                item.Item_Count_Down(item.Item_Count);
                Check_ItemList(false, item);
                break;
        }
        Item_UseStatus_YesButton.onClick.RemoveAllListeners();
        tem_UseStatus_AllButton.onClick.RemoveAllListeners();
    }

    public void Check_ItemList(bool Flag, ItemDataObject item, int addnum = 1)
    {
        int num = 0;
        if (item.Item_Type == Information_Item_Type.Equipment)
        {
            num = 0;
        }
        if (item.Item_Type == Information_Item_Type.Inventory)
        {
            num = 1;
        }
        if (item.Item_Type == Information_Item_Type.Box)
        {
            num = 2;
        }
        if (item.Item_Type == Information_Item_Type.Material)
        {
            num = 3;
        }
        if (item.Item_Type == Information_Item_Type.Quset)
        {
            num = 4;
        }

        if (Flag) // Plus
        {
            ItemObject.item = item;
            if(item.Item_Count - addnum == 0)
            {
                Data_ItemList.Plus_Inventory_Item(item);
                Instantiate(ItemObject, Transform_ItemList[num]);
            }
            else
            {
                foreach(ItemList_Object Inventory_Item in Transform_ItemList[num].GetComponentsInChildren<ItemList_Object>())
                {
                    if(Inventory_Item.item == item)
                    {
                        Inventory_Item.Check_ItemCount();
                    }
                }
            }
        }
        else // Minus
        {
            foreach (ItemList_Object Inventory_Item in Transform_ItemList[num].GetComponentsInChildren<ItemList_Object>())
            {
                if (Inventory_Item.item == item)
                {
                    if (!Inventory_Item.Check_ItemCount())
                    {
                        Destroy(Inventory_Item.gameObject);
                        Data_ItemList.Minus_Inventory_Item(item);
                    }
                    break;
                }
            }
        }
        Check_Item_Use();
        Data_ItemList.Check_ItemChangeFlag();
    }


    public void UseItemStatus_NoButton()
    {
        UseWindowFlag = false;

        Item_UseStatus_WindowObject.SetActive(false);
        Item_UseStatus_YesButton.onClick.RemoveAllListeners();
    }

    public void UseItemWindow_BackButton()
    {
        UseItemWindowFlag = false;
        Debug.Log(UI_UseItem_Num);
        switch (UI_UseItem_Num)
        {
/*            case 0:
                Item_UseItem_WindowObject.SetActive(false);
                Item_UseItem_WindowObject_List[0].SetActive(false);

                break;*/
            case 0: // 박스 한개
                Item_UseItem_WindowObject.SetActive(false);
                Item_UseItem_WindowObject_List[0].SetActive(false);
                break;
            case 1: // 스킬북 한개
                Item_UseItem_WindowObject.SetActive(false);
                Item_UseItem_WindowObject_List[1].SetActive(false);
                break;
            case 2: // 스킬북 여러개
                Item_UseItem_WindowObject.SetActive(false);
                Item_UseItem_WindowObject_List[1].SetActive(false);
                break;
            case 3: // 박스 여러개
                Item_UseItem_WindowObject.SetActive(false);
                Item_UseItem_WindowObject_List[2].SetActive(false);
                Item_UseItem_WindowObject_List[2].GetComponent<ItemUse_Window_Box_All>().Item_BoxAll_Init();
                break;
        }
        UI_UseItem_Num = -1;
        stationDirector.Check_Coin();
    }
    public void Director_Init_Inventory()
    {
        foreach (ItemList_Object _itemObejct in Transform_ItemList[0].GetComponentsInChildren<ItemList_Object>())
        {
            Destroy(_itemObejct.gameObject);
        }
        foreach (ItemList_Object _itemObejct in Transform_ItemList[1].GetComponentsInChildren<ItemList_Object>())
        {
            Destroy(_itemObejct.gameObject);
        }
        foreach (ItemList_Object _itemObejct in Transform_ItemList[2].GetComponentsInChildren<ItemList_Object>())
        {
            Destroy(_itemObejct.gameObject);
        }
        foreach (ItemList_Object _itemObejct in Transform_ItemList[3].GetComponentsInChildren<ItemList_Object>())
        {
            Destroy(_itemObejct.gameObject);
        }
        foreach (ItemList_Object _itemObejct in Transform_ItemList[4].GetComponentsInChildren<ItemList_Object>())
        {
            Destroy(_itemObejct.gameObject);
        }


        for (int i = 0; i < Transform_ItemList.Count; i++)
        {
            Spawn_Item(i);
        }
    }

    private void Check_Item_Init_Use(int Num, ItemDataObject item)
    {
        if (Item_UseIcon[Num].activeSelf != true)
        {
            if (item.Use_Flag && item.Item_Count > 0)
            {
                Item_UseIcon[Num].SetActive(true);

                if (Item_UseIcon[0].activeSelf != true)
                {
                    Item_UseIcon[0].SetActive(true);
                }
            }
        }
    }

    private void Check_Item_Use()
    {
        for(int i = 1; i < 3; i++)
        {
            Item_UseIcon[i].SetActive(false);
            foreach (ItemList_Object item_object in Transform_ItemList[i].GetComponentsInChildren<ItemList_Object>())
            {
                if (Item_UseIcon[i].activeSelf != true)
                {
                    if (item_object.item.Use_Flag && item_object.item.Item_Count > 0)
                    {
                        Item_UseIcon[i].SetActive(true);
                    }
                }
            }
        }

        if (Item_UseIcon[1].activeSelf || Item_UseIcon[2].activeSelf)
        {
             Item_UseIcon[0].SetActive(true);
        }
        else
        {
             Item_UseIcon[0].SetActive(false);
        }
    }
}
