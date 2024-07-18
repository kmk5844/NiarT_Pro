using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;
using System;

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
    public Button[] Equiped_Button;


    public GameObject Inventory_Window;
    public GameObject ItemCount_Window;
    public Image ItemImage;
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI MaxText;
    public Button Button_Plus;
    public Button Button_Minus;
    public Button Button_Equip;
    public Button Button_ItemCountChange;
    public Button Button_ItemEmpty;

    public Button GameStart_Button;
    public TextMeshProUGUI TrainText;

    int Fuel_Count;

    int Item_Count; // 가지고 나갈 갯수
    int Max_Count; // 최대 갯수
    int itemObject_Count; // 게임오브젝트가 가지고 있는 갯수

    int Equipment_Button_Num;

    [HideInInspector]
    public bool EquipItemFlag;

    private void Start()
    {
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        itemData = Item_DataObject.GetComponent<Station_ItemData>();
        itemEquip_object.GameStartDirector = GetComponent<Station_GameStart>();
        itemEquip_object.item_tooltip_object = itemTooltip_object;
        for(int i = 0; i < Equiped_Button.Length; i++)
        {
            Equiped_ImageAndCount(i);
        }
        Spawn_Item();
    }

    private void Equiped_ImageAndCount(int buttonNum)
    {
        int num = itemData.SA_Player_ItemData.Equiped_Item[buttonNum];
        if (num == -1)
        {
            Equiped_Button[buttonNum].GetComponent<Image>().sprite = itemData.SA_Player_ItemData.EmptyObject.Item_Sprite;
            Equiped_Button[buttonNum].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        else
        {
            Equiped_Button[buttonNum].GetComponent<Image>().sprite = itemData.SA_ItemList.Item[num].Item_Sprite;
            Equiped_Button[buttonNum].GetComponentInChildren<TextMeshProUGUI>().text = itemData.SA_Player_ItemData.Equiped_Item_Count[buttonNum].ToString();
        }
    }

    private void Spawn_Item()
    {
        foreach (ItemDataObject item in itemData.Equipment_Inventory_ItemList)
        {
            itemEquip_object.item = item;
            if (itemData.SA_Player_ItemData.Equiped_Item[0] == item.Num)
            {
                itemEquip_object.item_equip = true;
            }else if (itemData.SA_Player_ItemData.Equiped_Item[1] == item.Num)
            {
                itemEquip_object.item_equip = true;
            }else if (itemData.SA_Player_ItemData.Equiped_Item[2] == item.Num)
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
            TrainText.text = "원활한 게임 플레이를 위해\n적어도 연료 기차 한 대가 필요합니다.";
            GameStart_Button.interactable = false;
        }
        else
        {
            TrainText.text = "게임 시작이 가능합니다.";
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
        StartCoroutine(Open_Inventory_Window_Ani(num));
    }

    IEnumerator Open_Inventory_Window_Ani(int num)
    {
        EquipItemFlag = true;

        RectTransform Item_Window = Inventory_Window.GetComponent<RectTransform>();
        float startX = Item_Window.anchoredPosition.x;
        float targetX = -620f;
        if(Item_Window.anchoredPosition.x != targetX)
        {
            float duration = 0.1f;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                float newX = Mathf.Lerp(startX, targetX, t);
                Item_Window.anchoredPosition = new Vector2(newX, Item_Window.anchoredPosition.y);
                yield return null;
            }
        }
        else
        {
            Button_ItemCountChange.onClick.RemoveAllListeners(); // 버그 방지용 (이거 없이도 작동이 되긴 되나 버그 방지용으로 만듦)
            Button_ItemEmpty.onClick.RemoveAllListeners();
        }

        Button_ItemCountChange.onClick.AddListener(() => Open_ItemCountWindow
            (itemData.SA_Player_ItemData.Equiped_Item[num]));
        Button_ItemEmpty.onClick.AddListener(() => Click_EmptyItem(num));
        GameStart_Button.gameObject.SetActive(false);
    }

    public void Close_Inventory_Window()
    {
        EquipItemFlag = false;
        StartCoroutine(Close_Inventory_Window_Ani());
    }

    IEnumerator Close_Inventory_Window_Ani()
    {
        RectTransform Item_Window = Inventory_Window.GetComponent<RectTransform>();
        float startX = Item_Window.anchoredPosition.x;
        float targetX = -1300f;

        float duration = 0.1f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newX = Mathf.Lerp(startX, targetX, t);
            Item_Window.anchoredPosition = new Vector2(newX, Item_Window.anchoredPosition.y);
            yield return null;
        }

        Button_ItemCountChange.onClick.RemoveAllListeners();
        Button_ItemEmpty.onClick.RemoveAllListeners();
        Equipment_Button_Num = -1;
        GameStart_Button.gameObject.SetActive(true);
    }

    private void Click_EmptyItem(int num)
    {
        foreach (ItemEquip_Object itemList_Object in ItemList_Window.GetComponentsInChildren<ItemEquip_Object>())
        {
            if (itemList_Object.item.Num == itemData.SA_Player_ItemData.Equiped_Item[num])
            {
                itemList_Object.item_equip = false;
                itemList_Object.Change_EquipFlag();
                break;
            }
        }
        itemData.SA_Player_ItemData.Empty_Item(num);
        Equiped_ImageAndCount(num);
    }

    public void Open_ItemCountWindow(int item_Num)
    {
        ItemDataObject item = itemData.SA_ItemList.Item[item_Num];
        Item_Count = 1;
        ItemImage.sprite = item.Item_Sprite;
        ItemNameText.text = item.Item_Name;
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
        if (itemData.SA_Player_ItemData.Equiped_Item[Equipment_Button_Num] != -1)
        {
            foreach (ItemEquip_Object itemList_Object in ItemList_Window.GetComponentsInChildren<ItemEquip_Object>())
            {
                if (itemList_Object.item.Num == itemData.SA_Player_ItemData.Equiped_Item[Equipment_Button_Num])
                {
                    itemList_Object.item_equip = false;
                    itemList_Object.Change_EquipFlag();
                    break;
                }
            }
        }
        itemData.SA_Player_ItemData.Equip_Item(Equipment_Button_Num, item, Item_Count);
        Equiped_ImageAndCount(Equipment_Button_Num);

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
