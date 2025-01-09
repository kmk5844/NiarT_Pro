using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class Station_Store : MonoBehaviour
{
    [Header("������ ����")]
    public GameObject Player_DataObject;
    Station_PlayerData playerData;
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Mercenary_DataObject;
    Station_MercenaryData mercenaryData;
    public GameObject Item_DataObject;
    Station_ItemData itemData;

    public Station_Inventory inventory_director;

    [Header("���� ������")]
    public GameObject Check_Buy_Panel;
    public Image Check_Buy_Image;
    public LocalizeStringEvent Check_Buy_Name;
    public TextMeshProUGUI Check_Buy_Pride;
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

    public Button Sell_AllButton;

    [Header("���� ����")] // ���� ���ŵ� ����
    public int Store_Train_Num;
    public GameObject Store_Train_Part_Card;

    [Header("������ ���� ����")]
    public Transform Train_Store_Content;
    List<int> Train_Store_Num;

    [Header("������ ��ž ����")]
    public Transform Turret_Store_Content;
    List<int> Turret_Store_Num;

    [Header("������ �ν��� ����")]
    public Transform Booster_Store_Content;
    List<int> Booster_Store_Num;

    [Header("�뺴 ����")]
    [SerializeField]
    List<int> Mercenary_Store_Num;
    public Transform Mercenary_Store_Content;
    public GameObject Mercenary_Card;

    [Header("������ �뺴�� ����")]
    public StoreList_Tooltip StoreTooltip_Object;

    [Header("������ �����ϱ�")]
    public ItemBuy_Object ItemBuyList_Object;
    public ItemList_Tooltip ItemBuyTooltip_Object;
    public GameObject Item_Buy_Window;

    [Header("������ �Ǹ��ϱ�")]
    public ItemSell_Object ItemSellList_Object;
    public ItemList_Tooltip ItemSellTooltip_Object;
    public GameObject Item_Sell_Window;

    [HideInInspector]
    public bool Store_BuyAndSell_Window_Flag;
    [HideInInspector]
    public bool ItemSellFlag;
    private void Start()
    {
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        itemData = Item_DataObject.GetComponent<Station_ItemData>();
        Train_Store_Num = trainData.Train_Store_Num;
        Turret_Store_Num = trainData.Train_Turret_Store_Num;
        Booster_Store_Num = trainData.Train_Booster_Store_Num;
        Mercenary_Store_Num = mercenaryData.Mercenary_Store_Num;
        Item_Count_Window.SetActive(false);
        Store_BuyAndSell_Window_Flag = false;

        //���ö������̼�
        Check_Buy_Name.StringReference.TableReference = "Station_Table_St";
        Check_Buy_Text.StringReference.TableReference = "Station_Table_St";

        //���� �����ϱ�
        Check_Init_TrainCard();
        //�ͷ� ���� �����ϱ�
        Check_Init_TurretCard();
        //�ν��� ���� �����ϱ�
        Check_Init_BoosterCard();
        //�뺴 �����ϱ�
        Check_Init_MercenaryCard();
        //������ �����ϱ�
        Check_Init_ItemBuy();
        //������ �Ǹ��ϱ�
        Check_Init_ItemSell();

        GetComponentInParent<StationDirector>().UI_Train_Lock_Panel[0].SetActive(Check_Part_Store_Lock(51));
        GetComponentInParent<StationDirector>().UI_Train_Lock_Panel[1].SetActive(Check_Part_Store_Lock(52));
    }

    public bool Check_Part_Store_Lock(int num = -1)
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

    //���� �����ϱ�
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

    //���� ���� �����ϱ�
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

    //�ν��� ���� �����ϱ�
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
    }

    //�뺴 �����ϱ�
    private void Check_Init_MercenaryCard() // ī�� �ʱ�ȭ
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
    private void Store_Buy_MercenaryCard(int Mercenary_Num) // ī�� ���� �ϱ�
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
    public void Check_AfterBuy_MercenaryCard() //ī�� ���� ��, üũ�ϱ�
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

    //������ ���� �κ�
    private void Check_Init_ItemBuy()
    {
        ItemBuyList_Object.StoreDirector = GetComponent<Station_Store>();
        ItemBuyList_Object.item_tooltip_object = ItemBuyTooltip_Object;
        foreach (ItemDataObject item in itemData.Store_Buy_itemList)
        {
            ItemBuyList_Object.item = item;
            Instantiate(ItemBuyList_Object, Item_Buy_Window.transform);
        }
    }

    private void Store_Buy_Item(ItemDataObject item)
    {
        bool itemAvailability = false;
        if (playerData.Player_Coin >= item.Item_Buy_Pride * item_Count)
        {
            playerData.Player_Buy_Coin(item.Item_Buy_Pride * item_Count);
            item.Item_Count_UP(item_Count);
            itemData.Plus_Inventory_Item(item);
            {
                foreach (ItemSell_Object Sell_Object in Item_Sell_Window.GetComponentsInChildren<ItemSell_Object>())
                {
                    if (Sell_Object.item == item) // ���� ��, ������ üũ
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
                    ItemSellList_Object.item_tooltip_object = ItemSellTooltip_Object;
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
    }

    //������ �Ǹ� �κ�
    private void Check_Init_ItemSell()
    {
        ItemSellList_Object.StoreDirector = GetComponent<Station_Store>();
        ItemSellList_Object.item_tooltip_object = ItemSellTooltip_Object;
        int Total_ItemCount = 0;
        foreach(ItemDataObject item in itemData.Store_Sell_itemList)
        {
            ItemSellList_Object.item = item;
            Instantiate(ItemSellList_Object, Item_Sell_Window.transform);
            Total_ItemCount++;
            
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

    private void Store_Sell_Item(ItemDataObject item, bool AllFlag)
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
                    Destroy(itemObject.gameObject);
                }
            }
        }
        itemData.Check_EquipedItem(item.Num);
        itemData.Check_ItemChangeFlag();
        Check_Player_Coin_Point();
        Close_Buy_Window();
    }

    public void Open_Buy_Window(int i, int TrainAndMercenaryNum)
    {
        Store_BuyAndSell_Window_Flag = true;
        Check_Buy_Panel_Num = i;
        Check_Buy_Panel.SetActive(true);
        Sell_AllButton.gameObject.SetActive(false);

        if (i == 0)
        {
            //Check_Buy_Text.text = "�����Ͻðڽ��ϱ�?";
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
                    Check_Buy_Pride.text = trainData.EX_Game_Data.Information_Train[TrainAndMercenaryNum].Train_Buy_Cost.ToString();
                }
                Buy_YesButton.onClick.AddListener(() => Store_Buy_TrainCard(TrainAndMercenaryNum));
            }
            else if (Store_Train_Num == 1)
            {
                {
                    Check_Buy_Name.StringReference.TableReference = "ExcelData_Table_St";
                    Check_Buy_Name.StringReference.TableEntryReference = "Train_Turret_Name_" + (TrainAndMercenaryNum/ 10);
                    //Check_Buy_Name.text = trainData.EX_Game_Data.Information_Train_Turret_Part[TrainAndMercenaryNum].Turret_Part_Name;
                    Check_Buy_Pride.text = trainData.EX_Game_Data.Information_Train_Turret_Part[TrainAndMercenaryNum].Train_Buy_Cost.ToString();
                }
                Buy_YesButton.onClick.AddListener(() => Store_Buy_TurretCard(TrainAndMercenaryNum));
            }
            else if (Store_Train_Num == 2)
            {
                {
                    Check_Buy_Name.StringReference.TableReference = "ExcelData_Table_St";
                    Check_Buy_Name.StringReference.TableEntryReference = "Train_Booster_Name_" + (TrainAndMercenaryNum/10);
                    //Check_Buy_Name.text = trainData.EX_Game_Data.Information_Train_Booster_Part[TrainAndMercenaryNum].Booster_Part_Name;
                    Check_Buy_Pride.text = trainData.EX_Game_Data.Information_Train_Booster_Part[TrainAndMercenaryNum].Train_Buy_Cost.ToString();
                }
                Buy_YesButton.onClick.AddListener(() =>Store_Buy_BoosterCard(TrainAndMercenaryNum));
            }
        }
        else if (i == 1)
        {
            //Check_Buy_Text.text = "����Ͻðڽ��ϱ�?";
            Check_Buy_Image.sprite = Defualt_Store_Sprite;
            Check_Buy_Text.StringReference.TableEntryReference = "UI_Store_Mercenary_Buy";
            Check_Buy_Count.text = "1".ToString();
            {
                //Check_Buy_Name.text = trainData.EX_Game_Data.Information_Mercenary[TrainAndMercenaryNum].Name;
                Check_Buy_Name.StringReference.TableReference = "ExcelData_Table_St";
                Check_Buy_Name.StringReference.TableEntryReference = "Mercenary_Name_" + TrainAndMercenaryNum;
                Check_Buy_Pride.text = trainData.EX_Game_Data.Information_Mercenary[TrainAndMercenaryNum].Mercenary_Pride.ToString();
            }
            Buy_YesButton.onClick.AddListener(() => Store_Buy_MercenaryCard(TrainAndMercenaryNum));
        }
    }

    public void Open_BuyAndSell_Item_Window(ItemDataObject item, bool Flag)
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
            Check_Buy_Pride.text = item.Item_Buy_Pride.ToString();
            //Check_Buy_Text.text = "�����Ͻðڽ��ϱ�?";
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
            Check_Buy_Pride.text = item.Item_Sell_Pride.ToString();
            //Check_Buy_Text.text = "�Ǹ��Ͻðڽ��ϱ�?";
            Check_Buy_Text.StringReference.TableEntryReference = "UI_Store_Item_Sell";
            Button_ItemCount_Plus.onClick.AddListener(() => Click_ItemCount_Plus(item, Flag));
            Button_ItemCount_Minus.onClick.AddListener(() => Click_ItemCount_Minus(item, Flag));
            Buy_YesButton.onClick.AddListener(()=> Store_Sell_Item(item, false));
            Sell_AllButton.gameObject.SetActive(true);
            Sell_AllButton.onClick.AddListener(() => Store_Sell_Item(item, true));
        }
    }

    public void Close_Buy_Window()
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
    }

    private void Click_ItemCount_Plus(ItemDataObject item, bool Flag)
    {
        item_Count++;
        Check_Buy_Count.text = item_Count.ToString();
        if (Flag)
        {
            Check_Buy_Pride.text = (item.Item_Buy_Pride * item_Count).ToString();
        }
        else
        {
            Check_Buy_Pride.text = (item.Item_Sell_Pride * item_Count).ToString();
        }
        Check_ItemCount(item);
    }

    private void Click_ItemCount_Minus(ItemDataObject item, bool Flag)
    {
        item_Count--;
        Check_Buy_Count.text = item_Count.ToString();
        if (Flag)
        {
            Check_Buy_Pride.text = (item.Item_Buy_Pride * item_Count).ToString();
        }
        else
        {
            Check_Buy_Pride.text = (item.Item_Sell_Pride * item_Count).ToString();
        }
        Check_ItemCount(item);
    }

    public void Check_ItemCount(ItemDataObject item)
    {
        if(item_Count == 1)
        {
            Button_ItemCount_Minus.interactable = false;
        }
        else
        {
            Button_ItemCount_Minus.interactable = true;
        }

        if (!ItemSellFlag)
        {
            if (playerData.Player_Coin > item.Item_Buy_Pride * (item_Count + 1))
            {
                Button_ItemCount_Plus.interactable = true;
            }
            else
            {
                Button_ItemCount_Plus.interactable = false;
            }
            // �ڽ��� ������ �ִ� ���� ����

        }
        else
        {
            if (item.Item_Count != item_Count)
            {
                Button_ItemCount_Plus.interactable = true;
            }
            else
            {
                Button_ItemCount_Plus.interactable = false;
            }
            //�ڽ��� ������ �ִ� ������ ������ ����
        }
    }

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
    }

    //���� �κ�
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
        transform.GetComponentInParent<StationDirector>().Check_CoinAndPoint();
    }

    private void Ban_Player_Coin_Point(bool Flag)
    {
        transform.GetComponentInParent<StationDirector>().Check_Ban_CoinPoint(Flag);
        Close_Buy_Window();
    }

    public void Director_Tooltip_Off()
    {
        StoreTooltip_Object.Tooltip_Off();
        ItemBuyTooltip_Object.Tooltip_Off(); 
        ItemSellTooltip_Object.Tooltip_Off(); 
    }
}