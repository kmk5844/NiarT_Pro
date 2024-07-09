using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class Station_Inventory : MonoBehaviour
{
    [Header("데이터 관리")]
    [SerializeField]
    Station_ItemData Data_ItemList;
    [SerializeField]
    SA_PlayerData playerData;
    [Header("생성 관리")]
    public ItemList_Object ItemObject;
    public ItemList_Tooltip TooltipObject;
    [Header("인벤토리에 있는 아이템 리스트")]
    public List<Transform> Transform_ItemList;
    [Header("UI_UseStatus")]
    public GameObject Item_UseStatus_WindowObject;
    public TextMeshProUGUI Item_UseStatus_WindowObject_InformationText;
    public Button Item_UseStatus_YesButton;
    [Header("UI_UseItem")]
    int UI_UseItem_Num;
    public GameObject Item_UseItem_WindowObject;
    public List<GameObject> Item_UseItem_WindowObject_List;

    private void Start()
    {
        UI_UseItem_Num = 0;
        ItemObject.Inventory_Director = GetComponent<Station_Inventory>();
        ItemObject.item_tooltip_object = TooltipObject;

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
                    Instantiate (ItemObject, Transform_ItemList[num]);
                }
                break;
            case 2:
                foreach (ItemDataObject itemDataObject in Data_ItemList.Box_Inventory_ItemList)
                {
                    ItemObject.item = itemDataObject;
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
        Item_UseStatus_WindowObject_InformationText.text = "\"" + itemobject.Item_Name + " \"을 \n사용하시겠습니까?";
        Item_UseStatus_YesButton.onClick.AddListener(() => UseItemStatus_YesButton(itemobject));
        Item_UseStatus_WindowObject.SetActive(true);
    }

    public void UseItemStatus_YesButton(ItemDataObject item)
    {
        Item_UseStatus_WindowObject.SetActive(false);
        Item_UseItem_WindowObject.SetActive(true);
        switch (item.Num)
        {
            case 53:
                UI_UseItem_Num = 0;
                Item_UseItem_WindowObject_List[0].SetActive(true); 
                break;
            case 54:
            case 55:
            case 56:
                UI_UseItem_Num = 1;
                Item_UseItem_WindowObject_List[1].SetActive(true);
                Item_UseItem_WindowObject_List[1].GetComponent<ItemUse_Window_Box>().Random_Box_Open(item.Num, gameObject);
                item.Item_Count_Down();
                Check_ItemList(false, item);
                break;
            case 57:
                UI_UseItem_Num = 2;
                Item_UseItem_WindowObject_List[2].SetActive(true);
                item.Item_Count_Down();
                Check_ItemList(false, item);
                break;
        }
        Item_UseStatus_YesButton.onClick.RemoveAllListeners();
    }


    public void Check_ItemList(bool Flag, ItemDataObject item, int addnum = 1)
    {
        Debug.Log(item);
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
    }
    
    
    public void UseItemStatus_NoButton()
    {
        Item_UseStatus_WindowObject.SetActive(false);
        Item_UseStatus_YesButton.onClick.RemoveAllListeners();
    }

    public void UseItemWindow_BackButton()
    {
        switch (UI_UseItem_Num)
        {
            case 0:
                Item_UseItem_WindowObject.SetActive(false);
                Item_UseItem_WindowObject_List[0].SetActive(false);
                Item_UseItem_WindowObject_List[0].GetComponent<ItemUse_Window_53>().Item_53_Init();

                break;
            case 1:
                Item_UseItem_WindowObject.SetActive(false);
                Item_UseItem_WindowObject_List[1].SetActive(false);
                break;
            case 2:
                Item_UseItem_WindowObject.SetActive(false);
                Item_UseItem_WindowObject_List[2].SetActive(false);
                break;
        }
        UI_UseItem_Num = -1;
    }

    public void Director_Init_Inventory()
    {

    }
}
