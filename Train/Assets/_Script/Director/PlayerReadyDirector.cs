using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using static PixelCrushers.DialogueSystem.ActOnDialogueEvent;
using static UnityEditor.Progress;

public class PlayerReadyDirector : MonoBehaviour
{
    public Station_TrainData trainData;
    SA_TrainData sa_trainData;
    SA_TrainTurretData sa_trainturretData;
    SA_TrainBoosterData sa_trainBoosterData;

    public Station_PlayerData playerData;
    public Station_ItemData itemListData;
    public Station_MercenaryData MercenaryData;

    [Header("UI_Window")]
    public GameObject[] UI_Window;
    int windowCount;
    public GameObject UI_SubStageSelect;

    [Header("-----------------UI-------------------")]
    [Space(10)]
    public GameObject OptionObject;
    bool Option_Flag;

    [Header("-------------Train----------------")]
    [Space(10)]
    public GameObject Using_TrainObject;
    public Transform Using_TrainList;
    public GameObject Buy_TrainObject;
    public Transform[] Buy_TrainList;

    [SerializeField]
    List<Ready_Using_TrainList_Object> __List__usingtrain;
    int Select_TrainNum_1;
    int Select_TrainNum_2;
    int Sub_TrainNum_Turret;
    int Sub_TrainNum_Booster;
    

    [Header("-------------Mercenary----------------")]
    [Space(10)]
    public Transform MercenaryList_Ride_Transform;
    public Transform MercenaryList_Transform;
    public GameObject MercenaryList_Ride_Object;
    public GameObject MercenaryList_Object;

    public List<Ready_MercenaryList_Ride> __LIST__MercenaryList_Ride_Object;
    public List<Ready_MercenaryList> __LIST__MercenaryList_Object;
    TMP_Dropdown dropDown;

    [Header("드래그")]
    public GameObject MercenaryDragObject_Director;
    public int MercenaryLIst_Mercenary_Num;
    public int Mercenary_Ride_List_Index;

    [Header("정보")]
    public GameObject Mercenary_Information_Object;
    bool Mercenary_Information_Flag;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI RideText;

    public GameObject Mercenary_GoldBen_Object;
    bool Mercenary_GoldBen_Flag;

    [Header("툴팁")]
    public GameObject Mercenary_Information_Tooltip;
    public bool HoldAndDragFlag_Mercenary;

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

    public bool HoldAndDragFlag_Item;
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
        Option_Flag = false;
        windowCount = 0;
        UI_Window[0].SetActive(true);
        UI_Window[1].SetActive(false);
        UI_Window[2].SetActive(false);

        sa_trainData = trainData.SA_TrainData;
        sa_trainturretData = trainData.SA_TrainTurretData;
        sa_trainBoosterData = trainData.SA_TrainBoosterData;

        //Train
        Instantiate_Using_Train_List();
        Instantiate_Buy_train_List();

        //Mercenary
        Check_PlayerCoin();
        MercenaryLIst_Mercenary_Num = -5;
        Mercenary_Ride_List_Index = -5;
        Instantiate_MercenaryList_Ride_Object();
        Instantiate_MercenaryList_Object();
        Check_Mercenary_Max();

        //Item
        DragItemCount = 0;
        EmptyItemObject = itemListData.SA_Player_ItemData.EmptyObject;
        Instantiate_Item();
        Instantiate_Equip_Item();
        Init_Item_Information();

    }

    void Update()
    {
        //--------------------------------------------------UI
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (Mercenary_Information_Flag)
            {
                Click_Close_Mercenary_Information();
            }else if (Mercenary_GoldBen_Flag)
            {
                Close_Mercenary_GoldBne_Window();
            }
            else
            {
                if (!Option_Flag)
                {
                    OpenOption_Button();
                }
                else
                {
                    CloseOption_Button();
                }
            }

        }

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

        if (HoldAndDragFlag_Item)
        {
            if (mouseHoldAndDragNotTime < 0.001f)
            {
                mouseHoldAndDragNotTime += Time.deltaTime;
            }
            else
            {
                Draging_Item = null;
                HoldAndDragFlag_Item = false;
            }
        }
    }

    //--------------------------------------------------Train //시작 조건이 trainData[]에서 -1이 없어야 한다.

    private void Instantiate_Using_Train_List()
    {
        int TrainIndex = 0;
        int TurretIndex = 0;
        int BoosterIndex = 0;
        
        Ready_Using_TrainList_Object usi = Using_TrainObject.GetComponent<Ready_Using_TrainList_Object>();
        usi.director = this;

        int max = trainData.Level_Train_MaxTrain;
        int count = sa_trainData.Train_Num.Count;
        __List__usingtrain = new List<Ready_Using_TrainList_Object>();

        for (int i = 0; i < max + 2; i++)
        {
            if (i < count)
            {
                if (sa_trainData.Train_Num[i] == 51)
                {
                    usi.Setting(i, 51, sa_trainturretData.Train_Turret_Num[TurretIndex],false);
                    TurretIndex++;
                }
                else if (sa_trainData.Train_Num[i] == 52)
                {
                    usi.Setting(i, 52, sa_trainturretData.Train_Turret_Num[BoosterIndex], false);
                    BoosterIndex++;
                }else if (sa_trainData.Train_Num[i] == -1)
                {
                    usi.Setting(i, -1, -1, true);
                }
                else
                {
                    usi.Setting(i, sa_trainData.Train_Num[TrainIndex], -1, false);
                }
                TrainIndex++;
            }
            else
            {
                usi.Setting(i, -1, -1, true);
                sa_trainData.SA_Train_Add(-1);
            }
            Ready_Using_TrainList_Object Instantiate_usi = Instantiate(usi, Using_TrainList);
            __List__usingtrain.Add(Instantiate_usi);
        }
        ResizeContent_UsingTrainContent(max);
    }
    void ResizeContent_UsingTrainContent(int Count)
    {
        RectTransform ContentSize = Using_TrainList.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2((131 * Count), 100);
        Using_TrainList.GetComponentInParent<ScrollRect>().horizontalNormalizedPosition = 1f;
    }

    private void Instantiate_Buy_train_List()
    {
        Ready_Buy_TrainObject buy = Buy_TrainObject.GetComponent<Ready_Buy_TrainObject>();
        buy.director = this;

        buy.TrainNum_1 = 10;
        Instantiate(buy, Buy_TrainList[0]);
        Instantiate(buy, Buy_TrainList[1]);

        for (int i =0; i < sa_trainData.Train_Buy_Num.Count; i++)
        {
            buy.TrainNum_1 = sa_trainData.Train_Buy_Num[i];
            Instantiate(buy, Buy_TrainList[0]);
            Instantiate(buy, Buy_TrainList[1]);
        }

        for(int i = 0; i < sa_trainturretData.Train_Turret_Buy_Num.Count; i++)
        {
            buy.TrainNum_1 = 51;
            buy.TrainNum_2 = sa_trainturretData.Train_Turret_Buy_Num[i];
            Instantiate(buy, Buy_TrainList[0]);
            Instantiate(buy, Buy_TrainList[2]);
        }

        for(int i = 0; i < sa_trainBoosterData.Train_Booster_Buy_Num.Count; i++)
        {
            buy.TrainNum_1 = 52;
            buy.TrainNum_2 = sa_trainBoosterData.Train_Booster_Buy_Num[i];
            Instantiate(buy, Buy_TrainList[0]);
            Instantiate(buy, Buy_TrainList[3]);
        }
    }

    public void Click_Change_Train(int index)
    {
        Ready_Using_TrainList_Object obj = __List__usingtrain[index];
        obj.Change_TrainNum(Select_TrainNum_1, Select_TrainNum_2);
        obj.Change_TrainImage();
        Change_TrainData(index);
        Change_ListSlelectFlag(false);
    }

    public void Click_Select_Train(int TrainNum_1, int TrainNum_2)
    {
        Select_TrainNum_1 = TrainNum_1;
        Select_TrainNum_2 = TrainNum_2;
        Change_ListSlelectFlag(true);
    }

    void Change_TrainData(int index)
    {
        int num = sa_trainData.Train_Num[index];
        Check_Index(index);

        if (Select_TrainNum_1 == 51)
        {
            if (num == 51)
            {
                sa_trainturretData.SA_Train_Turret_Change(Sub_TrainNum_Turret, Select_TrainNum_2);
            }
            else if (num == 52)
            {
                sa_trainData.SA_Train_Change(index, 51);
                sa_trainturretData.SA_Train_Turret_Insert(Sub_TrainNum_Turret, Select_TrainNum_2);
                sa_trainBoosterData.SA_Train_Booster_Remove(Sub_TrainNum_Booster);
            }
            else
            {
                sa_trainData.SA_Train_Change(index, 51);
                sa_trainturretData.SA_Train_Turret_Insert(Sub_TrainNum_Turret, Select_TrainNum_2);
            }
        }
        else if(Select_TrainNum_1 == 52)
        {
            if (num == 51)
            {
                sa_trainData.SA_Train_Change(index, 52);
                sa_trainBoosterData.SA_Train_Booster_Insert(Sub_TrainNum_Booster, Select_TrainNum_2);
                sa_trainturretData.SA_Train_Turret_Remove(Sub_TrainNum_Turret);            }
            else if (num == 52)
            {
                sa_trainBoosterData.SA_Train_Booster_Change(Sub_TrainNum_Booster, Select_TrainNum_2);
            }
            else
            {
                sa_trainData.SA_Train_Change(index, 52);
                sa_trainBoosterData.SA_Train_Booster_Insert(Sub_TrainNum_Booster, Select_TrainNum_2);
            }
        }
        else
        {
            sa_trainData.SA_Train_Change(index, Select_TrainNum_1);
            if(num == 51)
            {
                sa_trainturretData.SA_Train_Turret_Remove(Sub_TrainNum_Turret);
            }else if(num == 52)
            {
                sa_trainBoosterData.SA_Train_Booster_Remove(Sub_TrainNum_Booster);
            }
        }
    }

    void Check_Index(int index)
    {
        Sub_TrainNum_Turret = 0;
        Sub_TrainNum_Booster = 0;
        for (int i = 0; i < index; i++) {
            if (sa_trainData.Train_Num[i] == 51)
            {
                Sub_TrainNum_Turret++;
            }else if (sa_trainData.Train_Num[i] == 52)
            {
                Sub_TrainNum_Booster++;
            }
        }
    }

    void Change_ListSlelectFlag(bool flag)
    {
        foreach (Ready_Using_TrainList_Object obj in __List__usingtrain)
        {
            obj.SelectFlag_Change(flag);
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
        ResizeContent_RideContent(MaxMercenary);
        for (int i = 0; i < MaxMercenary; i++)
        {
            MercenaryList_Ride_Object.GetComponent<Ready_MercenaryList_Ride>().List_Index = i;
            if (i < Mercenary_NumList.Count)
            {
                MercenaryList_Ride_Object.GetComponent<Ready_MercenaryList_Ride>().Mercenary_Num = Mercenary_NumList[i];
                M_L_R_Object = Instantiate(MercenaryList_Ride_Object, MercenaryList_Ride_Transform);
                dropDown = M_L_R_Object.GetComponent<Ready_MercenaryList_Ride>().dropDown.GetComponent<TMP_Dropdown>();

                if (Mercenary_NumList[i] == 0)
                {
                    dropDown.onValueChanged.AddListener(Mercenary_Position_EngineDriver_DropDown);

                }else if (Mercenary_NumList[i] == 5)
                {
                    dropDown.onValueChanged.AddListener(Mercenary_Position_Bard_DropDown);
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

    void ResizeContent_RideContent(int Count)
    {
        RectTransform ContentSize = MercenaryList_Ride_Transform.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(-410 + (90 * Count), 130);
    }

    void Instantiate_MercenaryList_Object()
    {
        MercenaryList_Object.GetComponent<Ready_MercenaryList>().Director = this;
        MercenaryList_Object.GetComponent<Ready_MercenaryList>().playerData = playerData;
        MercenaryList_Object.GetComponent<Ready_MercenaryList>().mercenaryData = MercenaryData;
        MercenaryList_Object.GetComponent<Ready_MercenaryList>().MercenaryDragObject_List = MercenaryDragObject_Director;
        MercenaryList_Object.GetComponent<Ready_MercenaryList>().Information_Tooltip = Mercenary_Information_Tooltip;
        foreach (int num in MercenaryData.Mercenary_Store_Num)
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
            __LIST__MercenaryList_Object.Add(M_L_Object.GetComponent<Ready_MercenaryList>());
        }
    }

    public void Click_MercenaryList_State(int num)
    {
        switch (num) {
            case 0:
                foreach (Ready_MercenaryList list_obj in __LIST__MercenaryList_Object)
                {
                    list_obj.ChangeState(0);
                }
                break;
            case 1:
                foreach (Ready_MercenaryList list_obj in __LIST__MercenaryList_Object)
                {
                    list_obj.ChangeState(1);
                }
                break;
            case 2:
                foreach (Ready_MercenaryList list_obj in __LIST__MercenaryList_Object)
                {
                    list_obj.ChangeState(2);
                }
                break;
        }
    }

    public void MercenaryChange()
    {
        if (MercenaryLIst_Mercenary_Num != -5 && Mercenary_Ride_List_Index != -5)
        {
            __LIST__MercenaryList_Ride_Object[Mercenary_Ride_List_Index].ChangeMercenary(MercenaryLIst_Mercenary_Num);
            //MercenaryData.SA_MercenaryData.SA_Mercenary_Change(Mercenary_Ride_List_Index, MercenaryLIst_Mercenary_Num);
        }
        MercenaryLIst_Mercenary_Num = -5;
        Mercenary_Ride_List_Index = -5;
    }

    public void ChangeDrowDown_Option(TMP_Dropdown _dropDown, string M_type)
    {
        switch (M_type)
        {
            case "Engine_Driver":
                _dropDown.onValueChanged.RemoveAllListeners();
                _dropDown.onValueChanged.AddListener(Mercenary_Position_EngineDriver_DropDown);
                break;
            case "Bard":
                _dropDown.onValueChanged.RemoveAllListeners();
                _dropDown.onValueChanged.AddListener(Mercenary_Position_Bard_DropDown);
                break;
            default:
                _dropDown.onValueChanged.RemoveAllListeners();
                break;
        }
    }

    public void Click_Open_Mercenary_Information()
    {
        Mercenary_Information_Object.SetActive(true);
        Mercenary_Information_Flag = true;
    }

    public void Click_Close_Mercenary_Information()
    {
        Mercenary_Information_Object.SetActive(false);
        Mercenary_Information_Flag = false;
    }

    public void Open_Mercenary_GoldBen_Window()
    {
        Mercenary_GoldBen_Object.SetActive(true);
        Mercenary_GoldBen_Flag = true;
    }

    public void Close_Mercenary_GoldBne_Window()
    {
        Mercenary_GoldBen_Object.SetActive(false);
        Mercenary_GoldBen_Flag = false;
    }

    public void Mercenary_Position_EngineDriver_DropDown(int value)
    {
        MercenaryData.SA_MercenaryData.SA_Change_EngineDriver_Type(value);
    }

    public void Mercenary_Position_Bard_DropDown(int value)
    {
        MercenaryData.SA_MercenaryData.SA_Change_Bard_Type(value);
    }

    public void Check_MercenaryList(int MercenaryNum)
    {
        if (MercenaryNum != -1)
        {
            __LIST__MercenaryList_Object[MercenaryNum].Check_PassiveFlag();
        }
    }

    public void Check_PlayerCoin()
    {
        GoldText.text = playerData.Player_Coin.ToString();
    }

    public void Check_Mercenary_Max()
    {
        int count = 0;
        for (int i = 0; i < __LIST__MercenaryList_Ride_Object.Count; i++)
        {
            if (__LIST__MercenaryList_Ride_Object[i].Mercenary_Num != -1)
            {
                count++;
            }
        }
        int max = trainData.Level_Train_MaxMercenary + 1;

        RideText.text = count + " / " + max;
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

    //아이템 구매 및 판매시, 발동
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
        if (windowCount < 2)
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
        if (windowCount > 0)
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

    public void OpenOption_Button()
    {
        Option_Flag = true;
        OptionObject.SetActive(true);
    }

    public void CloseOption_Button()
    {
        Option_Flag = false;
        OptionObject.SetActive(false);
    }


}