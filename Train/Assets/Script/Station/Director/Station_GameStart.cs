using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class Station_GameStart : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Item_DataObject;
    Station_ItemData itemData;
    
    [Header("UI")]
    public ItemEquip_Object itemEquip_object;
    public ItemList_Tooltip itemTooltip_object;
    public Transform ItemList_Window;

    public GameObject Inventory_Window;
    public GameObject ItemCount_Window;
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI MaxText;
    public Button Button_Plus;
    public Button Button_Minus;
    public Button Button_Equip;
    public Button Button_ItemCountChange;

    public Button GameStart_Button;
    public TextMeshProUGUI text;

    int Fuel_Count;

    int Item_Count; // 가지고 나갈 갯수
    int Max_Count; // 최대 갯수
    int itemObject_Count; // 게임오브젝트가 가지고 있는 갯수

    int Equipment_Button_Num;
    private void Start()
    {
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        itemData = Item_DataObject.GetComponent<Station_ItemData>();
        itemEquip_object.GameStartDirector = GetComponent<Station_GameStart>();
        itemEquip_object.item_tooltip_object = itemTooltip_object;
        Spawn_Item();
    }

    private void Spawn_Item()
    {
        foreach (ItemDataObject item in itemData.Equipment_Inventory_ItemList)
        {
            itemEquip_object.item = item;
            if (itemData.SA_Player_ItemData.Equiped_Item[0] == item)
            {
                itemEquip_object.item_equip = true;
            }else if (itemData.SA_Player_ItemData.Equiped_Item[1] == item)
            {
                itemEquip_object.item_equip = true;
            }else if (itemData.SA_Player_ItemData.Equiped_Item[2] == item)
            {
                itemEquip_object.item_equip = true;
            }
            else
            {
                itemEquip_object.item_equip = false;
            }
            Instantiate(itemEquip_object, ItemList_Window);
        }
    }

    public void Check_Train()
    {
        Fuel_Count = 0;
        for(int i = 0; i < trainData.Train_Num.Count; i++)
        {
            if(trainData.Train_Num[i] / 10 == 1)
            {
                Fuel_Count++;
            }
        }

        if(Fuel_Count == 0)
        {
            text.text = "원활한 게임 플레이를 위해\n적어도 연료 기차 한 대가 필요합니다.";
            GameStart_Button.interactable = false;
        }
        else
        {
            text.text = "게임 시작이 가능합니다.";
            GameStart_Button.interactable = true;
        }
    }

    public void Click_GameStart()
    {
        LoadingManager.LoadScene("CharacterSelect");
    }

    public void Open_Inventory_Window(int num)
    {
        Equipment_Button_Num = num;
        Button_ItemCountChange.onClick.AddListener(() => Open_ItemCountWindow
        (itemData.SA_Player_ItemData.Equiped_Item[num]));
        Inventory_Window.SetActive(true);
    }

    public void Close_Inventory_Window()
    {
        Button_ItemCountChange.onClick.RemoveAllListeners();
        Equipment_Button_Num = -1;
        Inventory_Window.SetActive(false);
    }

    public void Open_ItemCountWindow(ItemDataObject item)
    {
        Item_Count = 1;
        CountText.text = Item_Count.ToString();
        itemObject_Count = item.Item_Count;
        Max_Count = item.Max_Equip;
        CheckCount();
        MaxText.text = "Max : " + Max_Count;
        ItemCount_Window.SetActive(true);
        Button_Equip.onClick.AddListener(() => Equip_Item(item));
    }

    public void Equip_Item(ItemDataObject item)
    {
        if (itemData.SA_Player_ItemData.Equiped_Item[Equipment_Button_Num].Item_Type != Information_Item_Type.Empty)
        {
            foreach (ItemEquip_Object itemList_Object in ItemList_Window.GetComponentsInChildren<ItemEquip_Object>())
            {
                if (itemList_Object.item == itemData.SA_Player_ItemData.Equiped_Item[Equipment_Button_Num])
                {
                    itemList_Object.item_equip = false;
                    itemList_Object.Change_EquipFlag();
                    break;
                }
            }
        }
        itemData.SA_Player_ItemData.Equip_Item(Equipment_Button_Num, item, Item_Count);

        foreach (ItemEquip_Object itemList_Object in ItemList_Window.GetComponentsInChildren<ItemEquip_Object>())
        {
            if(itemList_Object.item == item)
            {
                itemList_Object.item_equip = true;
                itemList_Object.Change_EquipFlag();
                break;
            }
        }

        Close_Inventory_Window();
        Close_ItemCountWindow();
    }

    public void Close_ItemCountWindow()
    {
        ItemCount_Window.SetActive(false);
        Button_ItemCountChange.onClick.RemoveAllListeners();
        Button_Equip.onClick.RemoveAllListeners();
    }

    public void CountPlus()
    {
        Item_Count++;
        CountText.text = Item_Count.ToString();
        CheckCount();
    }

    public void CountMInus()
    {
        Item_Count--;
        CountText.text = Item_Count.ToString();
        CheckCount();
    }

    private void CheckCount()
    {
        if(Item_Count == 1)
        {
            Button_Minus.interactable = false;
        }
        else
        {
            Button_Minus.interactable = true;
        }

        if(itemObject_Count > Max_Count)
        {
            if (Item_Count == Max_Count)
            {
                Button_Plus.interactable = false;
            }
            else
            {
                Button_Plus.interactable = true;
            }
        }
        else
        {
            if(Item_Count == itemObject_Count)
            {
                Button_Plus.interactable = false;
            }
            else
            {
                Button_Plus.interactable = true;
            }
        }
    }

    public void Director_Init_EquipItem()
    {
        foreach(ItemEquip_Object _item in ItemList_Window.GetComponentsInChildren<ItemEquip_Object>())
        {
            Destroy(_item.gameObject);
        }

        Spawn_Item();
    }
}
