using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using JetBrains.Annotations;
using UnityEngine.Localization;

public class Station_Store : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Player_DataObject;
    Station_PlayerData playerData;
/*    public GameObject Train_DataObject;
    Station_TrainData trainData;*/
/*    public GameObject Mercenary_DataObject;
    Station_MercenaryData mercenaryData;*/
    public GameObject Item_DataObject;
    Station_ItemData itemData;

    public StationDirector stationDirector; 
    public Station_Inventory inventory_director;

/*    [Header("구매 윈도우")]
    public GameObject Check_Buy_Panel;
    public Image Check_Buy_Image;
    public LocalizeStringEvent Check_Buy_Name;
    public TextMeshProUGUI Check_Pride;
    //public TextMeshProUGUI Check_Buy_Text;
    public LocalizeStringEvent Check_Buy_Text;
    public TextMeshProUGUI Check_Buy_Count;
    public Button Buy_YesButton;
    public Button Buy_NoButton;
    int Check_Buy_Panel_Num;
    public Sprite Defualt_Store_Sprite;

    public GameObject Item_Count_Window;
    public Button Button_ItemCount_Plus;
    public Button Button_ItemCount_Minus;
    int item_Count;

    public Button Sell_AllButton;*/

/*    [Header("기차 구매")] // 파츠 구매도 포함
    public int Store_Train_Num;
    public GameObject Store_Train_Part_Card;

    [Header("상점의 기차 구매")]
    public Transform Train_Store_Content;
    List<int> Train_Store_Num;

    [Header("상점의 포탑 구매")]
    public Transform Turret_Store_Content;
    List<int> Turret_Store_Num;

    [Header("상점의 부스터 구매")]
    public Transform Booster_Store_Content;
    List<int> Booster_Store_Num;

    [Header("용병 구매")]
    [SerializeField]
    List<int> Mercenary_Store_Num;
    public Transform Mercenary_Store_Content;
    public GameObject Mercenary_Card;

    [Header("기차와 용병의 툴팁")]
    public StoreList_Tooltip StoreTooltip_Object;
*/
    [Header("아이템 구매하기")]
    public ItemBuy_Object ItemBuyList_Object;
    //public ItemList_Tooltip ItemBuyTooltip_Object;
    public GameObject Item_Buy_Window;

    [Header("아이템 판매하기")]
    public ItemSell_Object ItemSellList_Object;
    //public ItemList_Tooltip ItemSellTooltip_Object;
    public GameObject Item_Sell_Window;

    [HideInInspector]
    public bool Store_BuyAndSell_Window_Flag;
    [HideInInspector]
    public bool ItemSellFlag;

    //----------------------------------------------------------------------------
    [Header("공통")]
    public GameObject Click_ItemObject;
    [SerializeField]
    ItemDataObject Click_ItemDataObjcet;

    public GameObject SelectObject_Before;
    public GameObject SelectObject_After;

    public Image Item_Image;
    public LocalizeStringEvent Item_Name_Text;
    public LocalizeStringEvent Item_Information_Text;

    public GameObject CountObject;
    int CountNum;
    TextMeshProUGUI CountNum_Buy_Text;
    public GameObject CountButton;
    LocalizeStringEvent CountButton_Text;

    public bool Store_CheckFlag;
    public bool Store_BanFlag;

    [Header("아이템 구매 체크")]
    public GameObject BuyCheck_Window;
    public GameObject BuyCheck_Ban_Window;
    public LocalizedString Buy_Local;
    public TextMeshProUGUI Buy_Text;

    [Header("아이템 판매 체크")]
    public GameObject SellCheck_Window;
    public LocalizedString Sell_Local;
    public TextMeshProUGUI Sell_Text;

    private void Start()
    {
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        //trainData = Train_DataObject.GetComponent<Station_TrainData>();
        //mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        itemData = Item_DataObject.GetComponent<Station_ItemData>();
        Item_Name_Text.StringReference.TableReference = "ItemData_Table_St";
        Item_Information_Text.StringReference.TableReference = "ItemData_Table_St";
        /*        Train_Store_Num = trainData.Train_Store_Num;
                Turret_Store_Num = trainData.Train_Turret_Store_Num;
                Booster_Store_Num = trainData.Train_Booster_Store_Num;*/
        //Mercenary_Store_Num = mercenaryData.Mercenary_Store_Num;
        /*        Item_Count_Window.SetActive(false);
                Store_BuyAndSell_Window_Flag = false;

                //로컬라이제이션
                Check_Buy_Name.StringReference.TableReference = "Station_Table_St";
                Check_Buy_Text.StringReference.TableReference = "Station_Table_St";*/

        /*        //기차 구매하기
                Check_Init_TrainCard();
                //터렛 파츠 구매하기
                Check_Init_TurretCard();
                //부스터 파츠 구매하기
                Check_Init_BoosterCard();*/
        /*        //용병 구매하기
                Check_Init_MercenaryCard();*/
        SelectObject_Before.SetActive(true);
        SelectObject_After.SetActive(false);

        //아이템 구매하기
        Check_Init_ItemBuy();
        //아이템 판매하기
        Check_Init_ItemSell();
        Setting_Count_Buy();


/*        GetComponentInParent<StationDirector>().UI_Train_Lock_Panel[0].SetActive(Check_Part_Store_Lock(51));
        GetComponentInParent<StationDirector>().UI_Train_Lock_Panel[1].SetActive(Check_Part_Store_Lock(52));*/
    }

/*    public bool Check_Part_Store_Lock(int num = -1)
    {
        if (trainData.SA_TrainData.Train_Buy_Num.Contains(num) || num == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //기차 구매하기
    private void Check_Init_TrainCard()
    {
        Store_Train_Part_Card.GetComponent<Store_Train_Card>().trainData = trainData;
        Store_Train_Part_Card.GetComponent<Store_Train_Card>().storeDirector = GetComponent<Station_Store>();
        Store_Train_Part_Card.GetComponent<Store_Train_Card>().store_tooltip_object = StoreTooltip_Object;
        foreach (int num in Train_Store_Num)
        {
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num = num;
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num2 = -1;
            GameObject Card = Instantiate(Store_Train_Part_Card, Train_Store_Content);
            Card.name = num.ToString();
            if (trainData.SA_TrainData.Train_Buy_Num.Contains(num) == true)
            {
                Card.GetComponent<Store_Train_Card>().Train_Buy.SetActive(true);
            }
        }
    }
    private void Store_Buy_TrainCard(int Train_Num)
    {
        if (playerData.Player_Coin >= trainData.EX_Game_Data.Information_Train[Train_Num].Train_Buy_Cost)
        {
            playerData.Player_Buy_Coin(trainData.EX_Game_Data.Information_Train[Train_Num].Train_Buy_Cost);
            trainData.SA_TrainData.SA_Train_Buy(Train_Num);
            trainData.Check_Buy_Train(Train_Num);
            Check_AfterBuy_TrainCard();
            Check_Player_Coin_Point();
            Close_Buy_Window();
        }
        else
        {
            Ban_Player_Coin_Point(true);
        }
    }
    public void Check_AfterBuy_TrainCard()
    {
        for(int i = 0; i < Train_Store_Content.childCount; i++)
        {
            GameObject Card = Train_Store_Content.GetChild(i).gameObject;
            int Card_Num = Card.GetComponent<Store_Train_Card>().Train_Num;
            if (trainData.SA_TrainData.Train_Buy_Num.Contains(Card_Num)){
                Card.GetComponent<Store_Train_Card>().Train_Buy.SetActive(true);
            }
        }
    }

    //무기 파츠 구매하기
    private void Check_Init_TurretCard()
    {
        Store_Train_Part_Card.GetComponent<Store_Train_Card>().trainData = trainData;
        Store_Train_Part_Card.GetComponent<Store_Train_Card>().storeDirector = GetComponent<Station_Store>();
        Store_Train_Part_Card.GetComponent<Store_Train_Card>().store_tooltip_object = StoreTooltip_Object;
        foreach (int num in Turret_Store_Num)
        {
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num = 51;
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num2 = num;
            GameObject Card = Instantiate(Store_Train_Part_Card, Turret_Store_Content);
            Card.name = "51_" + num.ToString();
            if (trainData.SA_TrainTurretData.Train_Turret_Buy_Num.Contains(num) == true)
            {
                Card.GetComponent<Store_Train_Card>().Train_Buy.SetActive(true);
            }
        }
    }
    private void Store_Buy_TurretCard(int TurretNum)
    {
        if (playerData.Player_Coin >= trainData.EX_Game_Data.Information_Train_Turret_Part[TurretNum].Train_Buy_Cost)
        {
            playerData.Player_Buy_Coin(trainData.EX_Game_Data.Information_Train_Turret_Part[TurretNum].Train_Buy_Cost);
            trainData.SA_TrainTurretData.SA_Train_Turret_Buy(TurretNum);
            trainData.Check_Buy_Turret(TurretNum);
            Check_AfterBuy_TurretCard();
            Check_Player_Coin_Point();
            Close_Buy_Window();
        }
        else
        {
            Ban_Player_Coin_Point(true);
        }
    }
    public void Check_AfterBuy_TurretCard()
    {
        for (int i = 0; i < Turret_Store_Content.childCount; i++)
        {
            GameObject Card = Turret_Store_Content.GetChild(i).gameObject;
            int Card_Num = Card.GetComponent<Store_Train_Card>().Train_Num2;
            if (trainData.SA_TrainTurretData.Train_Turret_Buy_Num.Contains(Card_Num))
            {
                Card.GetComponent<Store_Train_Card>().Train_Buy.SetActive(true);
            }
        }
    }

    //부스터 파츠 구매하기
    private void Check_Init_BoosterCard()
    {
        Store_Train_Part_Card.GetComponent<Store_Train_Card>().trainData = trainData;
        Store_Train_Part_Card.GetComponent<Store_Train_Card>().storeDirector = GetComponent<Station_Store>();
        Store_Train_Part_Card.GetComponent<Store_Train_Card>().store_tooltip_object = StoreTooltip_Object;
        foreach (int num in Booster_Store_Num)
        {
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num = 52;
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num2 = num;
            GameObject Card = Instantiate(Store_Train_Part_Card, Booster_Store_Content);
            Card.name = "52_" + num.ToString();
            if (trainData.SA_TrainBoosterData.Train_Booster_Buy_Num.Contains(num) == true)
            {
                Card.GetComponent<Store_Train_Card>().Train_Buy.SetActive(true);
            }
        }
    }
    private void Store_Buy_BoosterCard(int Booster_Num)
    {
        if (playerData.Player_Coin >= trainData.EX_Game_Data.Information_Train_Booster_Part[Booster_Num].Train_Buy_Cost)
        {
            playerData.Player_Buy_Coin(trainData.EX_Game_Data.Information_Train_Booster_Part[Booster_Num].Train_Buy_Cost);
            trainData.SA_TrainBoosterData.SA_Train_Booster_Buy(Booster_Num);
            trainData.Check_Buy_Booster(Booster_Num);
            Check_AfterBuy_BoosterCard();
            Check_Player_Coin_Point();
            Close_Buy_Window();
        }
        else
        {
            Ban_Player_Coin_Point(true);
        }
    }
    public void Check_AfterBuy_BoosterCard()
    {
        for (int i = 0; i < Booster_Store_Content.childCount; i++)
        {
            GameObject Card = Booster_Store_Content.GetChild(i).gameObject;
            int Card_Num = Card.GetComponent<Store_Train_Card>().Train_Num2;
            if (trainData.SA_TrainBoosterData.Train_Booster_Buy_Num.Contains(Card_Num))
            {
                Card.GetComponent<Store_Train_Card>().Train_Buy.SetActive(true);
            }
        }
    }*/
/*
    //용병 구매하기
    private void Check_Init_MercenaryCard() // 카드 초기화
    {
        Mercenary_Card.GetComponent<Store_Mercenary_Card>().mercenaryData = mercenaryData;
        Mercenary_Card.GetComponent<Store_Mercenary_Card>().storeDirector = GetComponent<Station_Store>();
        Mercenary_Card.GetComponent<Store_Mercenary_Card>().store_tooltip_object = StoreTooltip_Object;
        foreach (int num in Mercenary_Store_Num)
        {
            Mercenary_Card.GetComponent<Store_Mercenary_Card>().Mercenary_Num = num;
            GameObject Card = Instantiate(Mercenary_Card, Mercenary_Store_Content);
            Card.name = num.ToString();
            if (mercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Contains(num) == true)
            {
                Card.GetComponent<Store_Mercenary_Card>().Mercenary_Buy.SetActive(true);
            }
        }
    }
    private void Store_Buy_MercenaryCard(int Mercenary_Num) // 카드 구매 하기
    {
        if (playerData.Player_Coin >= mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Mercenary_Pride)
        {
            playerData.Player_Buy_Coin(mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Mercenary_Pride);
            mercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Add(Mercenary_Num);
            Check_AfterBuy_MercenaryCard();
            Check_Player_Coin_Point();
            Close_Buy_Window();
        }
        else
        {
            Ban_Player_Coin_Point(true);
        }

    }
    public void Check_AfterBuy_MercenaryCard() //카드 구매 후, 체크하기
    {
        for (int i = 0; i < Mercenary_Store_Content.childCount; i++)
        {
            GameObject Card = Mercenary_Store_Content.GetChild(i).gameObject;
            int Card_Num = Card.GetComponent<Store_Mercenary_Card>().Mercenary_Num;
            if (mercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Contains(Card_Num)){
                Card.GetComponent<Store_Mercenary_Card>().Mercenary_Buy.SetActive(true);
            }
        }
    }
*/


/*    private void Store_Buy_Item(ItemDataObject item)
    {
        bool itemAvailability = false;
        if (playerData.Player_Coin >= item.Item_Buy_Pride * item_Count)
        {
            playerData.Player_Buy_Coin(item.Item_Buy_Pride * item_Count);
            item.Item_Count_UP(item_Count);
            itemData.Plus_Inventory_Item(item);
            if (stationDirector.simplestationFlag)
            {
                stationDirector.Director_PlayerReadyDirector.itemListData.Plus_Inventory_Item(item);
            }

            {
                foreach (ItemSell_Object Sell_Object in Item_Sell_Window.GetComponentsInChildren<ItemSell_Object>())
                {
                    if (Sell_Object.item == item) // 구매 시, 아이템 체크
                    {
                        itemAvailability = true;
                        Sell_Object.Check_ItemCount();
                        break;
                    }
                }
                if (!itemAvailability)
                {
                    ItemSellList_Object.item = item;
                    ItemSellList_Object.StoreDirector = GetComponent<Station_Store>();
                    //ItemSellList_Object.item_tooltip_object = ItemSellTooltip_Object;
                    Instantiate(ItemSellList_Object, Item_Sell_Window.transform);
                }
            }
            itemData.Check_ItemChangeFlag();
            Check_Player_Coin_Point();
            Close_Buy_Window();
        }
        else
        {
            Ban_Player_Coin_Point(true);
        }
    }*/

/*    private void Store_Sell_Item(ItemDataObject item, bool AllFlag)
    {
        if (!AllFlag)
        {
            playerData.Player_Get_Coin(item.Item_Sell_Pride * item_Count);
            item.Item_Count_Down(item_Count);
        }
        else
        {
            playerData.Player_Get_Coin(item.Item_Sell_Pride * item.Item_Count);
            item.Item_Count_Down(item.Item_Count);
        }
        foreach(ItemSell_Object itemObject in Item_Sell_Window.GetComponentsInChildren<ItemSell_Object>())
        {
            if(itemObject.item == item)
            {
                if(!itemObject.Check_ItemCount())
                {
                    itemData.Minus_Inventory_Item(item);
                    if (stationDirector.simplestationFlag)
                    {
                        stationDirector.Director_PlayerReadyDirector.itemListData.Minus_Inventory_Item(item);
                    }
                    Destroy(itemObject.gameObject);
                }
            }
        }
        itemData.Check_EquipedItem(item.Num);
        itemData.Check_ItemChangeFlag();
        Check_Player_Coin_Point();
        Close_Buy_Window();
    }*/

/*    public void Open_Buy_Window(int i, int TrainAndMercenaryNum)
    {
        Store_BuyAndSell_Window_Flag = true;
        Check_Buy_Panel_Num = i;
        Check_Buy_Panel.SetActive(true);
        Sell_AllButton.gameObject.SetActive(false);

        if (i == 0)
        {
            //Check_Buy_Text.text = "구매하시겠습니까?";
            Check_Buy_Image.sprite = Defualt_Store_Sprite;
            Check_Buy_Text.StringReference.TableEntryReference = "UI_Store_Train&Item_Buy";
            Check_Buy_Count.text = "1".ToString();
            if (Store_Train_Num == 0)
            {
                {
                    Check_Buy_Name.StringReference.TableReference = "ExcelData_Table_St";
                    if(TrainAndMercenaryNum == 51 || TrainAndMercenaryNum == 52)
                    {
                        Check_Buy_Name.StringReference.TableEntryReference = "Train_Name_" + TrainAndMercenaryNum;
                    }
                    else
                    {
                        if(TrainAndMercenaryNum < 50)
                        {
                            Check_Buy_Name.StringReference.TableEntryReference = "Train_Name_" + (TrainAndMercenaryNum/10);
                        }
                        else
                        {
                            Check_Buy_Name.StringReference.TableEntryReference = "Train_Name_" + TrainAndMercenaryNum;
                        }
                    }
                    //Check_Buy_Name.text = trainData.EX_Game_Data.Information_Train[TrainAndMercenaryNum].Train_Name;
                    Check_Pride.text = trainData.EX_Game_Data.Information_Train[TrainAndMercenaryNum].Train_Buy_Cost.ToString();
                }
                Buy_YesButton.onClick.AddListener(() => Store_Buy_TrainCard(TrainAndMercenaryNum));
            }
            else if (Store_Train_Num == 1)
            {
                {
                    Check_Buy_Name.StringReference.TableReference = "ExcelData_Table_St";
                    Check_Buy_Name.StringReference.TableEntryReference = "Train_Turret_Name_" + (TrainAndMercenaryNum/ 10);
                    //Check_Buy_Name.text = trainData.EX_Game_Data.Information_Train_Turret_Part[TrainAndMercenaryNum].Turret_Part_Name;
                    Check_Pride.text = trainData.EX_Game_Data.Information_Train_Turret_Part[TrainAndMercenaryNum].Train_Buy_Cost.ToString();
                }
                Buy_YesButton.onClick.AddListener(() => Store_Buy_TurretCard(TrainAndMercenaryNum));
            }
            else if (Store_Train_Num == 2)
            {
                {
                    Check_Buy_Name.StringReference.TableReference = "ExcelData_Table_St";
                    Check_Buy_Name.StringReference.TableEntryReference = "Train_Booster_Name_" + (TrainAndMercenaryNum/10);
                    //Check_Buy_Name.text = trainData.EX_Game_Data.Information_Train_Booster_Part[TrainAndMercenaryNum].Booster_Part_Name;
                    Check_Pride.text = trainData.EX_Game_Data.Information_Train_Booster_Part[TrainAndMercenaryNum].Train_Buy_Cost.ToString();
                }
                Buy_YesButton.onClick.AddListener(() =>Store_Buy_BoosterCard(TrainAndMercenaryNum));
            }
        }
        else if (i == 1)
        {
            //Check_Buy_Text.text = "고용하시겠습니까?";
            Check_Buy_Image.sprite = Defualt_Store_Sprite;
            Check_Buy_Text.StringReference.TableEntryReference = "UI_Store_Mercenary_Buy";
            Check_Buy_Count.text = "1".ToString();
            {
                //Check_Buy_Name.text = trainData.EX_Game_Data.Information_Mercenary[TrainAndMercenaryNum].Name;
                Check_Buy_Name.StringReference.TableReference = "ExcelData_Table_St";
                Check_Buy_Name.StringReference.TableEntryReference = "Mercenary_Name_" + TrainAndMercenaryNum;
                Check_Pride.text = trainData.EX_Game_Data.Information_Mercenary[TrainAndMercenaryNum].Mercenary_Pride.ToString();
            }
            Buy_YesButton.onClick.AddListener(() => Store_Buy_MercenaryCard(TrainAndMercenaryNum));
        }
    }*/

/*    public void Open_BuyAndSell_Item_Window(ItemDataObject item, bool Flag)
    {
        Store_BuyAndSell_Window_Flag = true;
        Check_Buy_Panel_Num = 2;
        Check_Buy_Panel.SetActive(true);
        Item_Count_Window.SetActive(true);
        //Check_Buy_Name.text = item.Item_Name;
        Check_Buy_Name.StringReference.TableReference = "ItemData_Table_St";
        Check_Buy_Name.StringReference.TableEntryReference = "Item_Name_" + item.Num;

        if (Flag)
        {
            Button_ItemCount_Init(true, item);
            Check_Buy_Image.sprite = item.Item_Sprite;
            Check_Pride.text = item.Item_Buy_Pride.ToString();
            //Check_Buy_Text.text = "구매하시겠습니까?";
            Check_Buy_Text.StringReference.TableEntryReference = "UI_Store_Train&Item_Buy";
            Button_ItemCount_Plus.onClick.AddListener(() => Click_ItemCount_Plus(item, Flag));
            Button_ItemCount_Minus.onClick.AddListener(() => Click_ItemCount_Minus(item, Flag));
            Buy_YesButton.onClick.AddListener(() => Store_Buy_Item(item));
            Sell_AllButton.gameObject.SetActive(false);
        }
        else
        {
            Button_ItemCount_Init(false, item);
            Check_Buy_Image.sprite = item.Item_Sprite;
            Check_Pride.text = item.Item_Sell_Pride.ToString();
            //Check_Buy_Text.text = "판매하시겠습니까?";
            Check_Buy_Text.StringReference.TableEntryReference = "UI_Store_Item_Sell";
            Button_ItemCount_Plus.onClick.AddListener(() => Click_ItemCount_Plus(item, Flag));
            Button_ItemCount_Minus.onClick.AddListener(() => Click_ItemCount_Minus(item, Flag));
            Buy_YesButton.onClick.AddListener(()=> Store_Sell_Item(item, false));
            Sell_AllButton.gameObject.SetActive(true);
            Sell_AllButton.onClick.AddListener(() => Store_Sell_Item(item, true));
        }
    }
*/
/*    public void Close_Buy_Window()
    {
        Store_BuyAndSell_Window_Flag = false;
        Check_Buy_Panel.SetActive(false);
        if (Check_Buy_Panel_Num == 0)
        {
            if (Store_Train_Num == 0)
            {
                Buy_YesButton.onClick.RemoveAllListeners();
            }
            else if (Store_Train_Num == 1)
            {
                Buy_YesButton.onClick.RemoveAllListeners();
            }
            else if (Store_Train_Num == 2)
            {
                Buy_YesButton.onClick.RemoveAllListeners();
            }
        }
        else if (Check_Buy_Panel_Num == 1)
        {
            Buy_YesButton.onClick.RemoveAllListeners();
        }
        else if (Check_Buy_Panel_Num == 2)
        {
            Item_Count_Window.SetActive(false);
            Buy_YesButton.onClick.RemoveAllListeners();
            Button_ItemCount_Plus.onClick.RemoveAllListeners();
            Button_ItemCount_Minus.onClick.RemoveAllListeners();
            Sell_AllButton.onClick.RemoveAllListeners();
        }
    }*/
/*
    public void Button_ItemCount_Init(bool Flag,ItemDataObject item)
    {
        item_Count = 1;
        Check_Buy_Count.text = item_Count.ToString();
        Button_ItemCount_Minus.interactable = false;
        if (Flag)
        {
            Button_ItemCount_Plus.interactable = true;
        }
        else
        {
            if (item.Item_Count <= 1)
            {
                Button_ItemCount_Plus.interactable = false;
            }
            else
            {
                Button_ItemCount_Plus.interactable = true;
            }
        }
    }*/

    //공통 부분
    public void ResizedContent_H(Transform ScrollContent, ScrollRect Scrollrect)
    {
        GridLayoutGroup Grid = ScrollContent.GetComponent<GridLayoutGroup>();
        Vector2 cellSize = Grid.cellSize;
        Vector2 spacing = Grid.spacing;

        float wide = (cellSize.x * (ScrollContent.childCount/ 9)) + (spacing.x * ((ScrollContent.childCount/ 9) - 1)) ;
        RectTransform ContentSize = ScrollContent.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(wide, ContentSize.sizeDelta.y);
        Scrollrect.horizontalNormalizedPosition = 0f;
    }

    private void Check_Player_Coin_Point()
    {
        transform.GetComponentInParent<StationDirector>().Check_Coin();
    }

/*    private void Ban_Player_Coin_Point(bool Flag)
    {
        transform.GetComponentInParent<StationDirector>().Check_Ban_CoinPoint(Flag);
        Close_Buy_Window();
    }
*/
/*    public void Director_Tooltip_Off()
    {
        //StoreTooltip_Object.Tooltip_Off();
        ItemBuyTooltip_Object.Tooltip_Off(); 
        ItemSellTooltip_Object.Tooltip_Off(); 
    }
*/
    //----------------------------------------------------------------------------
    void Setting_Count_Buy()
    {
        Init_Information();
        CountNum_Buy_Text = CountObject.GetComponentInChildren<TextMeshProUGUI>();
        CountButton_Text = CountButton.GetComponentInChildren<LocalizeStringEvent>();
        CountButton_Text.StringReference.TableReference = "Station_Table_St";
        CountButton_Text.StringReference.TableEntryReference = "UI_Store_ItemBuy_Button";
        CountNum = 1;
        Change_CountNum_Buy_Text();
    }

    public void Set_ButtonText()
    {
        if (stationDirector.UI_Store_BuyAndSell_Flag)
        {
            CountButton_Text.StringReference.TableEntryReference = "UI_Store_ItemBuy_Button";
        }
        else
        {
            CountButton_Text.StringReference.TableEntryReference = "UI_Store_ItemSell_Button";
        }
    }

    public void Click_ItemCheck(GameObject item, bool flag)
    {
        Cancel_SelectItem();
        Click_ItemObject = item;
        ItemDataObject itemData;
        if (flag)
        {
            itemData = item.GetComponent<ItemBuy_Object>().item;
            Click_ItemDataObjcet = Click_ItemObject.GetComponent<ItemBuy_Object>().item;
        }
        else
        {
            itemData = item.GetComponent<ItemSell_Object>().item;
            Click_ItemDataObjcet = Click_ItemObject.GetComponent<ItemSell_Object>().item;
        }

        if (SelectObject_Before.activeSelf)
        {
            SelectObject_Before.SetActive(false);
            SelectObject_After.SetActive(true);
        }

        Item_Image.sprite = itemData.Item_Sprite;
        Item_Name_Text.StringReference.TableEntryReference = "Item_Name_" + itemData.Num;
        Item_Information_Text.StringReference.TableEntryReference = "Item_Information_" + itemData.Num;
        CountButton_On();
    }

    void CountButton_On()
    {
        if (!CountObject.activeSelf)
        {
            CountObject.SetActive(true);
            CountButton.SetActive(true);
        }
        CountNum = 1;
        Change_CountNum_Buy_Text();
    }

    void Change_CountNum_Buy_Text()
    {
        CountNum_Buy_Text.text = CountNum.ToString(); 
    }

    public void Open_Check_Window()
    {
        Store_CheckFlag = true;
        if (stationDirector.UI_Store_BuyAndSell_Flag)
        {
            BuyCheck_Window.SetActive(true);
            Buy_Local.Arguments = new object[] { CountNum };
            Buy_Local.StringChanged += UpdateText;
            Buy_Local.RefreshString();
        }
        else
        {
            SellCheck_Window.SetActive(true);
            Sell_Local.Arguments = new object[] { CountNum };
            Sell_Local.StringChanged += UpdateText;
            Sell_Local.RefreshString();
        }
    }

    void UpdateText(string value)
    {
        if (stationDirector.UI_Store_BuyAndSell_Flag){
            Buy_Text.text = Buy_Local.GetLocalizedString();
        }
        else
        {
            Sell_Text.text = Sell_Local.GetLocalizedString();
        }
    }

    public void Close_Buy_Window()
    {
        Store_CheckFlag = false;
        BuyCheck_Window.SetActive(false);
    }

/*    public void Open_Sell_Window()
    {
        SellCheck_Window.SetActive(true);
        SellCheck_Text.text = CountNum + "개를 판매하시겠습니까?";
    }*/

    public void Close_Sell_Window()
    {
        Store_CheckFlag = false;
        SellCheck_Window.SetActive(false);
    }

    //아이템 구매 부분
    private void Check_Init_ItemBuy()
    {
        ItemBuyList_Object.StoreDirector = GetComponent<Station_Store>();
        foreach (ItemDataObject item in itemData.Store_Buy_itemList)
        {
            ItemBuyList_Object.item = item;
            Instantiate(ItemBuyList_Object, Item_Buy_Window.transform);
        }
        Resize_ListContent(0, itemData.Store_Buy_itemList.Count);

    }

    public void Click_ItemBuy()
    {
        bool itemAvailability = false;

        if (playerData.Player_Coin >= Click_ItemDataObjcet.Item_Buy_Pride * CountNum)
        {
            playerData.Player_Buy_Coin(Click_ItemDataObjcet.Item_Buy_Pride * CountNum);
            Click_ItemDataObjcet.Item_Count_UP(CountNum);
            itemData.Plus_Inventory_Item(Click_ItemDataObjcet);
            if (stationDirector.simplestationFlag)
            {
                stationDirector.Director_PlayerReadyDirector.itemListData.Plus_Inventory_Item(Click_ItemDataObjcet);
            }

            {
                foreach (ItemSell_Object Sell_Object in Item_Sell_Window.GetComponentsInChildren<ItemSell_Object>())
                {
                    if (Sell_Object.item == Click_ItemDataObjcet) // 구매 시, 아이템 체크
                    {
                        itemAvailability = true;
                        Sell_Object.Check_ItemCount();
                        break;
                    }
                }
                if (!itemAvailability)
                {
                    ItemSellList_Object.item = Click_ItemDataObjcet;
                    ItemSellList_Object.StoreDirector = GetComponent<Station_Store>();
                    Instantiate(ItemSellList_Object, Item_Sell_Window.transform);
                }
            }
            itemData.Check_ItemChangeFlag();
            Check_Player_Coin_Point();
            Cancel_SelectItem();
            Init_Information();
            Close_Buy_Window();
        }
        else
        {
            Ban_Player_Coin();
        }
    }

    //아이템 판매 부분
    private void Check_Init_ItemSell()
    {
        ItemSellList_Object.StoreDirector = GetComponent<Station_Store>();
        //ItemSellList_Object.item_tooltip_object = ItemSellTooltip_Object;
        foreach (ItemDataObject item in itemData.Store_Sell_itemList)
        {
            ItemSellList_Object.item = item;
            Instantiate(ItemSellList_Object, Item_Sell_Window.transform);
        }
        Resize_ListContent(1, itemData.Store_Sell_itemList.Count);
    }

    void Resize_ListContent(int num, int count)
    {
        if(num == 0)
        {
            RectTransform rect = Item_Buy_Window.GetComponent<RectTransform>();

            if (count % 3 != 0)
            {
                rect.sizeDelta = new Vector2(0, 50 + (145 * ((count / 3) + 1)));
            }
            else
            {
                rect.sizeDelta = new Vector2(0, 50 + (145 * (count / 3)));
            }
        }
        else if(num == 1)
        {
            RectTransform rect = Item_Sell_Window.GetComponent<RectTransform>();

            if (count % 3 != 0)
            {
                rect.sizeDelta = new Vector2(0, 45 + (145 * ((count / 3) + 1)));
            }
            else
            {
                rect.sizeDelta = new Vector2(0, 45 + (145 * (count / 3)));
            }

        }
    }

    public void Director_Init_ItemSell()
    {
        foreach (ItemSell_Object item in Item_Sell_Window.GetComponentsInChildren<ItemSell_Object>())
        {
            Destroy(item.gameObject);
        }
        Check_Init_ItemSell();
    }


    public void Click_ItemSell()
    {
        playerData.Player_Get_Coin(Click_ItemDataObjcet.Item_Sell_Pride * CountNum);
        Click_ItemDataObjcet.Item_Count_Down(CountNum);
 
        foreach (ItemSell_Object itemObject in Item_Sell_Window.GetComponentsInChildren<ItemSell_Object>())
        {
            if (itemObject.item == Click_ItemDataObjcet)
            {
                if (!itemObject.Check_ItemCount())
                {
                    itemData.Minus_Inventory_Item(Click_ItemDataObjcet);
                    if (stationDirector.simplestationFlag)
                    {
                        stationDirector.Director_PlayerReadyDirector.itemListData.Minus_Inventory_Item(Click_ItemDataObjcet);
                    }
                    Destroy(itemObject.gameObject);
                }
            }
        }
        itemData.Check_EquipedItem(Click_ItemDataObjcet.Num);
        itemData.Check_ItemChangeFlag();
        Check_Player_Coin_Point();
        Cancel_SelectItem();
        Init_Information();
        Close_Sell_Window();
    }

    public void Up_CountNum(bool check)
    {
        if(!stationDirector.UI_Store_BuyAndSell_Flag)
        {
            if(CountNum < Click_ItemDataObjcet.Item_Count)
            {
                CountNum++;
            }
            else
            {
                CountNum = Click_ItemDataObjcet.Item_Count;
            }
        }
        else
        {
            CountNum++;
        }

        if (check)
        {
            Change_CountNum_Buy_Text();
        }
    }

    public void Down_CountNum(bool check)
    {
        if (CountNum > 1)
        {
            CountNum--;
        }
        else
        {
            CountNum = 1;
        }

        if (check)
        {
            Change_CountNum_Buy_Text();
        }
    }
    
    public void Init_Information()
    {
        Cancel_SelectItem();
        SelectObject_Before.SetActive(true);
        SelectObject_After.SetActive(false);
        Click_ItemObject = null;
        Click_ItemDataObjcet = null;
        //Item_Name_Text.text = "";
        Item_Image.sprite = null;
        //Item_Information_Text.text = "";
        CountObject.SetActive(false);
        CountButton.SetActive(false);
    }

    void Cancel_SelectItem()
    {
        GameObject Before_ItemObject = Click_ItemObject;
        if (Before_ItemObject != null)
        {
            if (Before_ItemObject.GetComponent<ItemBuy_Object>())
            {
                Before_ItemObject.GetComponent<ItemBuy_Object>().SelectObject.SetActive(false);
            }
            else if (Before_ItemObject.GetComponent<ItemSell_Object>())
            {
                Before_ItemObject.GetComponent<ItemSell_Object>().SelectObject.SetActive(false);
            }
        }
    }

    void Ban_Player_Coin()
    {
        Close_Buy_Window();
        Store_BanFlag = true;
        BuyCheck_Ban_Window.SetActive(true);
    }

    public void Close_Ban_Player_Coin()
    {
        Store_BanFlag = false;  
        BuyCheck_Ban_Window.SetActive(false);
    }
}