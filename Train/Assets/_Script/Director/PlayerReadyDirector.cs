using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class PlayerReadyDirector : MonoBehaviour
{
    public Station_TrainData trainData;
    public Station_PlayerData playerData;
    public Station_ItemData itemListData;
    public Station_MercenaryData MercenaryData;

    [Header("UI_Window")]
    public GameObject[] UI_Window;
    int windowCount;
    public GameObject UI_SubStageSelect;

    [Header("-------------Mercenary----------------")]
    [Space(10)]
    public Transform MercenaryList_Ride_Transform;
    public Transform MercenaryList_Transform;
    public GameObject MercenaryList_Ride_Object;
    public GameObject MercenaryList_Object;


    public List<Ready_MercenaryList_Ride> __LIST__MercenaryList_Ride_Object;
    public List<GameObject> __LIST__MercenaryList_Object;
    TMP_Dropdown DropDown_EngineDriver_Type;
    TMP_Dropdown DropDown_Bard_Type;

    [Header("드래그")]
    public GameObject MercenaryDragObject_Director;
    public int MercenaryLIst_Mercenary_Num;
    public int Mercenary_Ride_List_Index;

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

        //Mercenary
        MercenaryLIst_Mercenary_Num = -5;
        Mercenary_Ride_List_Index = -5;
        Instantiate_MercenaryList_Ride_Object();
        Instantiate_MercenaryList_Object();

        //Item
        DragItemCount = 0;
        EmptyItemObject = itemListData.SA_Player_ItemData.EmptyObject;
        Instantiate_Item();
        Instantiate_Equip_Item();
        Init_Item_Information();

    }

    void Update()
    {
        //--------------------------------------------------Item
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
    //--------------------------------------------------Mercenary

    void Instantiate_MercenaryList_Ride_Object()
    {
        int MaxMercenary = trainData.Level_Train_MaxMercenary + 1;
        MercenaryList_Ride_Object.GetComponent<Ready_MercenaryList_Ride>().Director = this;
        MercenaryList_Ride_Object.GetComponent<Ready_MercenaryList_Ride>().mercenaryData = MercenaryData;
        List<int> Mercenary_NumList = MercenaryData.SA_MercenaryData.Mercenary_Num;
        GameObject M_L_R_Object;
        for (int i = 0; i < MaxMercenary; i++)
        {
            MercenaryList_Ride_Object.GetComponent<Ready_MercenaryList_Ride>().List_Index = i;
            if (i < Mercenary_NumList.Count)
            {
                MercenaryList_Ride_Object.GetComponent<Ready_MercenaryList_Ride>().Mercenary_Num = Mercenary_NumList[i];
                M_L_R_Object = Instantiate(MercenaryList_Ride_Object, MercenaryList_Ride_Transform);

                if (Mercenary_NumList[i] == 0)
                {
                    DropDown_EngineDriver_Type = M_L_R_Object.GetComponent<Ready_MercenaryList_Ride>().dropDown.GetComponent<TMP_Dropdown>();
                    DropDown_EngineDriver_Type.onValueChanged.AddListener(Mercenary_Position_EngineDriver_DropDown);

                }else if (Mercenary_NumList[i] == 5)
                {
                    DropDown_Bard_Type = M_L_R_Object.GetComponent<Ready_MercenaryList_Ride>().dropDown.GetComponent<TMP_Dropdown>();
                    DropDown_Bard_Type.onValueChanged.AddListener(Mercenary_Position_Bard_DropDown);
                }
            }
            else
            {
                MercenaryData.SA_MercenaryData.SA_Mercenary_Num_Plus(-1);
                MercenaryList_Ride_Object.GetComponent<Ready_MercenaryList_Ride>().Mercenary_Num = -1;
                M_L_R_Object = Instantiate(MercenaryList_Ride_Object, MercenaryList_Ride_Transform);
            }
            __LIST__MercenaryList_Ride_Object.Add(M_L_R_Object.GetComponent<Ready_MercenaryList_Ride>());
        }
    }


    void Instantiate_MercenaryList_Object()
    {
        MercenaryList_Object.GetComponent<Ready_MercenaryList>().Director = this;
        MercenaryList_Object.GetComponent<Ready_MercenaryList>().playerData = playerData;
        MercenaryList_Object.GetComponent<Ready_MercenaryList>().mercenaryData = MercenaryData;
        MercenaryList_Object.GetComponent<Ready_MercenaryList>().MercenaryDragObject_List = MercenaryDragObject_Director;
        foreach(int num in MercenaryData.Mercenary_Store_Num)
        {
            MercenaryList_Object.GetComponent<Ready_MercenaryList>().Mercenary_Num = num;
            if (MercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Contains(num) == true)
            {
                MercenaryList_Object.GetComponent<Ready_MercenaryList>().BuyFlag = true;
            }
            else
            {
                MercenaryList_Object.GetComponent<Ready_MercenaryList>().BuyFlag = false;
            }
            GameObject M_L_Object = Instantiate(MercenaryList_Object, MercenaryList_Transform);
            M_L_Object.name = num.ToString();
            __LIST__MercenaryList_Object.Add(M_L_Object);
        }
    }

    public void Click_MercenaryList_State(int num)
    {
        switch (num) { 
            case 0:
                foreach (GameObject list_obj in __LIST__MercenaryList_Object)
                {
                    list_obj.GetComponent<Ready_MercenaryList>().ChangeState(0);
                }
                break;
            case 1:
                foreach (GameObject list_obj in __LIST__MercenaryList_Object)
                {
                    list_obj.GetComponent<Ready_MercenaryList>().ChangeState(1);
                }
                break;
            case 2:
                foreach (GameObject list_obj in __LIST__MercenaryList_Object)
                {
                    list_obj.GetComponent<Ready_MercenaryList>().ChangeState(2);
                }
                break;
        }
    }

    public void MercenaryChange()
    {
        if(MercenaryLIst_Mercenary_Num!= -5 && Mercenary_Ride_List_Index != -5)
        {
            __LIST__MercenaryList_Ride_Object[Mercenary_Ride_List_Index].ChangeMercenary(MercenaryLIst_Mercenary_Num);
            MercenaryData.SA_MercenaryData.SA_Mercenary_Change(Mercenary_Ride_List_Index, MercenaryLIst_Mercenary_Num);
        }
        MercenaryLIst_Mercenary_Num = -5;
        Mercenary_Ride_List_Index = -5;
    }

    public void Mercenary_Position_EngineDriver_DropDown(int value)
    {
        MercenaryData.SA_MercenaryData.SA_Change_EngineDriver_Type(value);
    }

    public void Mercenary_Position_Bard_DropDown(int value)
    {
        MercenaryData.SA_MercenaryData.SA_Change_Bard_Type(value);
    }

    //--------------------------------------------------Item

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

    void Instantiate_Equip_Item()
    {
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
    }

    void Init_Item_Information()
    {
        UI_Info_ItemIcon.sprite = EmptyItemObject.Item_Sprite;
        UI_Info_ItemNameText.StringReference.TableReference = "ItemData_Table_St";
        UI_Info_ItemInformationText.StringReference.TableReference = "ItemData_Table_St";
        UI_Info_ItemNameText.StringReference.TableEntryReference = "Item_Name_-1";
        UI_Info_ItemInformationText.StringReference.TableEntryReference = "Item_Information_-1";
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

    //구매 시, 코인 변경 체크
    public void check_PlayerCoin()
    {
        Debug.Log("코인 변경 및 체크");
    }
}