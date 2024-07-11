using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Station_Store : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Player_DataObject;
    Station_PlayerData playerData;
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Mercenary_DataObject;
    Station_MercenaryData mercenaryData;
    public GameObject Item_DataObject;
    Station_ItemData itemData;

    public Station_Inventory inventory_director;


    [Header("윈도우")]
    public GameObject Check_Buy_Panel;
    public TextMeshProUGUI Check_Buy_Text;
    public Button Buy_YesButton;
    public Button Buy_NoButton;
    int Check_Buy_Panel_Num;

    [Header("아이템 갯수")]
    public GameObject Item_Count_Window;
    public Button Button_ItemCount_Plus;
    public Button Button_ItemCount_Minus;
    public TextMeshProUGUI Item_Count_Text;
    int item_Count;

    [Header("상점")] // 파츠 구매도 포함
    [SerializeField]
    List<Toggle> Store_Train_List_Toggle;
    [SerializeField]
    List<GameObject> Store_Train_List_Window;
    int Store_Train_Num;

    public GameObject Select_Train_Blueprint;
    public Image Select_Train_Sprite;
    public GameObject Store_Train_Part_Card;

    public GameObject Train_Information_Object;
    public TextMeshProUGUI Train_Information_Text;
    public TextMeshProUGUI Train_Information_Cost;
    public Button BuyButton_Train;
    public GameObject Part_Lock_Panel;

    [Header("상점의 기차 구매")]
    public ScrollRect ScrollRect_Train;
    public Transform Train_Store_Content;
    List<int> Train_Store_Num;
    [SerializeField]
    List<Toggle> Train_Toggle;
    int Select_Toggle_Train_Num;
    int Toggle_Train_Num;
    string Toggle_Train_Name;
    int Toggle_Train_Cost;

    [Header("상점의 포탑 구매")]
    public ScrollRect ScrollRect_Turret;
    public Transform Turret_Store_Content;
    List<int> Turret_Store_Num;
    [SerializeField]
    List<Toggle> Turret_Toggle;
    int Select_Toggle_Turret_Num;
    int Toggle_Turret_Num;
    string Toggle_Turret_Name;
    int Toggle_Turret_Cost;

    [Header("상점의 부스터 구매")]
    public ScrollRect ScrollRect_Booster;
    public Transform Booster_Store_Content;
    List<int> Booster_Store_Num;
    [SerializeField]
    List<Toggle> Booster_Toggle;
    int Select_Toggle_Booster_Num;
    int Toggle_Booster_Num;
    string Toggle_Booster_Name;
    int Toggle_Booster_Cost;

    [Header("용병 구매")]
    [SerializeField]
    List<int> Mercenary_Store_Num;
    public Transform Mercenary_Store_Content;
    public ScrollRect ScrollRect_Mercenary;
    public GameObject Mercenary_Card;

    public GameObject Mercenary_Information_Object;
    public TextMeshProUGUI Mercenary_Information_Text;

    public TextMeshProUGUI Mercenary_Information_Cost;
    public Button BuyButton_Mercenary;
    [SerializeField]
    List <Toggle> Mercenary_Toggle;
    int Select_Toggle_Mercenary_Num;
    int Toggle_Mercenary_Num; // toggle로 찍힌 카드 안의 숫자
    string Toggle_Mercenary_Name; // toggle로 찍힌 카드 안의 이름

    [Header("아이템 구매")]
    [SerializeField]
    List<Toggle> Store_Item_List_Toggle;
    [SerializeField]
    List<GameObject> Store_Item_List_Window;
    int Store_Item_List_Num;

    [Header("아이템 구매하기")]
    public ItemBuy_Object ItemBuyList_Object;
    public ItemList_Tooltip ItemBuyTooltip_Object;
    public GameObject Item_Buy_Window;

    [Header("아이템 판매하기")]
    public ItemSell_Object ItemSellList_Object;
    public ItemList_Tooltip ItemSellTooltip_Object;
    public GameObject Item_Sell_Window;

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
        //기차 스토어 토글
        StoreTrainList_ToggleStart();
        StoreTrainList_Toggle_Init();
        //기차 구매하기
        Check_Init_TrainCard();
        Director_Init_TrainBuy();
        Train_ToggleStart();
        //터렛 파츠 구매하기
        Check_Init_TurretCard();
        Director_Init_TurretBuy();
        Turret_ToggleStart();
        //부스터 파츠 구매하기
        Check_Init_BoosterCard();
        Director_Init_BoosterBuy();
        Booster_ToggleStart();
        //용병 구매하기
        Check_Init_MercenaryCard();
        Mercenary_ToggleStart();
        //아이템 토글
        StoreItemList_ToggleStart();
        ItemList_Toggle_Init();
        //아이템 구매하기
        Check_Init_ItemBuy();
        //아이템 판매하기
        Check_Init_ItemSell();
    }

    public void StoreTrainList_Toggle_Init()
    {
        Store_Train_Num = 0;
        for(int i = 0; i < Store_Train_List_Toggle.Count; i++)
        {
            if(i == 0)
            {
                Store_Train_List_Toggle[i].isOn = true;
                Store_Train_List_Toggle[i].interactable = false;
                Store_Train_List_Window[i].SetActive(true);
            }
            else
            {
                Store_Train_List_Toggle[i].isOn = false;
                Store_Train_List_Toggle[i].interactable = true;
                Store_Train_List_Window[i].SetActive(false);
            }
        }
    }

    private void StoreTrainList_ToggleStart()
    {
        foreach(var toggle in Store_Train_List_Toggle)
        {
            toggle.onValueChanged.AddListener(StoreTrainList_OnToggleValueChange);
        }
    }

    private void StoreTrainList_OnToggleValueChange(bool isOn)
    {
        if (isOn)
        {
            for(int i = 0; i < Store_Train_List_Toggle.Count; i++)
            {
                if (Store_Train_List_Toggle[i].isOn)
                {
                    Select_Init();
                    Store_Train_Num = i;
                    Store_Train_List_Toggle[i].isOn = true;
                    Store_Train_List_Toggle[i].interactable = false;
                    if(i == 1)
                    {
                        Check_Part_Store_Lock(51);
                    }
                    else if(i == 2)
                    {
                        Check_Part_Store_Lock(52);
                    }
                    else
                    {
                        Check_Part_Store_Lock();
                    }
                    Store_Train_List_Window[i].SetActive(true);
                    
                    // 여기서 체크
                }
                else
                {
                    Store_Train_List_Toggle[i].isOn = false;
                    Store_Train_List_Toggle[i].interactable = true;
                    Store_Train_List_Window[i].SetActive(false);
                }
            }
        }
    }
    private void Select_Init()
    {
        foreach(Toggle toggle in Train_Toggle)
        {
            toggle.isOn = false;
        }
        foreach(Toggle toggle in Turret_Toggle)
        {
            toggle.isOn = false;
        }
        foreach(Toggle toggle in Booster_Toggle)
        {
            toggle.isOn = false;
        }
    }
    private void Check_Part_Store_Lock(int num = -1)
    {
        if (trainData.SA_TrainData.Train_Buy_Num.Contains(num) || num == -1)
        {
            Part_Lock_Panel.SetActive(false);
        }
        else
        {
            Part_Lock_Panel.SetActive(true);
        }
    }

    //기차 구매하기
    public void Director_Init_TrainBuy()
    {
        BuyButton_Train.interactable = false;
        for (int i = 0; i < Train_Toggle.Count; i++)
        {
            Train_Toggle[i].isOn = false;
        }
        ScrollRect_Train.normalizedPosition = Vector2.up;
    }
    private void Train_ToggleStart()
    {
        foreach(Toggle toggle in Train_Toggle)
        {
            toggle.onValueChanged.AddListener(Train_OnToggleValueChange);
        }
    }
    private void Train_OnToggleValueChange(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < Train_Toggle.Count; i++)
            {
                if (Train_Toggle[i].isOn)
                {
                    Train_Store_Information_Text(true,i);
                    Select_Toggle_Train_Num = i;
                }
            }
            BuyButton_Train.interactable = true;
        }
        else
        {
            Train_Store_Information_Text(false);
            BuyButton_Train.interactable = false;
        }
    }
    private void Train_Store_Information_Text(bool flag, int toggle_num = -1)
    {
        if (flag)
        {
            Store_Train_Card Card = Train_Store_Content.GetChild(toggle_num).GetComponent<Store_Train_Card>();
            Toggle_Train_Num = Card.Train_Num;
            Toggle_Train_Name = trainData.EX_Game_Data.Information_Train[Toggle_Train_Num].Train_Name;
            Toggle_Train_Cost = trainData.EX_Game_Data.Information_Train[Toggle_Train_Num].Train_Buy_Cost;

            //설계도 켜짐
            Select_Train_Blueprint.SetActive(true);
            Select_Train_Sprite.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Toggle_Train_Num);

            //기차 정보 켜짐
            Train_Information_Object.SetActive(true);
            Train_Information_Text.text = "<color=black><b>"+Toggle_Train_Name +
                "</color></b><size=36>\n\n" + trainData.EX_Game_Data.Information_Train[Toggle_Train_Num].Train_Information.Replace("\\n", "\n");

            //Cost 정보 켜짐
            Train_Information_Cost.text = Toggle_Train_Cost + "G";
        }
        else
        {
            //설계도 꺼짐
            Select_Train_Blueprint.SetActive(false);
            //기차 정보 꺼짐
            Train_Information_Object.SetActive(false);
            //Cost 정보 꺼짐
            Train_Information_Cost.text = "0G";
        }

    }
    private void Check_Init_TrainCard()
    {
        foreach (int num in Train_Store_Num)
        {
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num = num;
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num2 = -1;
            GameObject Card = Instantiate(Store_Train_Part_Card, Train_Store_Content);
            Card.name = num.ToString();
            Train_Toggle.Add(Card.GetComponentInChildren<Toggle>());
            if (trainData.SA_TrainData.Train_Buy_Num.Contains(num) == true)
            {
                Card.GetComponent<Store_Train_Card>().Train_Buy.SetActive(true);
            }
        }
        ResizedContent_V(Train_Store_Content, ScrollRect_Train);
    }
    private void Store_Buy_TrainCard()
    {
        if(playerData.Player_Coin >= trainData.EX_Game_Data.Information_Train[Toggle_Train_Num].Train_Buy_Cost)
        {
            playerData.Player_Buy_Coin(trainData.EX_Game_Data.Information_Train[Toggle_Train_Num].Train_Buy_Cost);
            trainData.SA_TrainData.SA_Train_Buy(Toggle_Train_Num);
            trainData.Check_Buy_Train(Toggle_Train_Num);
            Train_Toggle[Select_Toggle_Train_Num].isOn = false;
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
    public void Director_Init_TurretBuy()
    {
        BuyButton_Train.interactable = false;
        for (int i = 0; i < Turret_Toggle.Count; i++)
        {
            Turret_Toggle[i].isOn = false;
        }
        ScrollRect_Turret.normalizedPosition = Vector2.up;
    }
    private void Turret_ToggleStart()
    {
        foreach (Toggle toggle in Turret_Toggle)
        {
            toggle.onValueChanged.AddListener(Turret_OnToggleValueChange);
        }
    }
    private void Turret_OnToggleValueChange(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < Turret_Toggle.Count; i++)
            {
                if (Turret_Toggle[i].isOn)
                {
                    Turret_Store_Information_Text(true, i);
                    Select_Toggle_Turret_Num = i;
                }
            }
            BuyButton_Train.interactable = true;
        }
        else
        {
            Turret_Store_Information_Text(false);
            BuyButton_Train.interactable = false;
        }
    }
    private void Turret_Store_Information_Text(bool flag, int toggle_num = -1)
    {
        if (flag)
        {
            Store_Train_Card Card = Turret_Store_Content.GetChild(toggle_num).GetComponent<Store_Train_Card>();
            Toggle_Turret_Num = Card.Train_Num2;
            Toggle_Turret_Name = trainData.EX_Game_Data.Information_Train_Turret_Part[Toggle_Turret_Num].Turret_Part_Name;
            Toggle_Turret_Cost = trainData.EX_Game_Data.Information_Train_Turret_Part[Toggle_Turret_Num].Train_Buy_Cost;

            //설계도 켜짐
            Select_Train_Blueprint.SetActive(true);
            Select_Train_Sprite.sprite = Resources.Load<Sprite>("Sprite/Train/Train_51_" + Toggle_Turret_Num);

            //기차 정보 켜짐
            Train_Information_Object.SetActive(true);
            Train_Information_Text.text = "<color=black><b>" + Toggle_Turret_Name +
                "</color></b><size=36>\n\n" + trainData.EX_Game_Data.Information_Train_Turret_Part[Toggle_Turret_Num].Train_Information.Replace("\\n", "\n");

            //Cost 정보
            Train_Information_Cost.text = Toggle_Turret_Cost + "G";
        }
        else
        {
            //설계도 꺼짐
            Select_Train_Blueprint.SetActive(false);
            //기차 정보 꺼짐
            Train_Information_Object.SetActive(false);
            //Cost 정보 꺼짐
            Train_Information_Cost.text = "0G";
        }
    }
    private void Check_Init_TurretCard()
    {
        foreach (int num in Turret_Store_Num)
        {
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num = 51;
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num2 = num;
            GameObject Card = Instantiate(Store_Train_Part_Card, Turret_Store_Content);
            Card.name = "51_" + num.ToString();
            Turret_Toggle.Add(Card.GetComponentInChildren<Toggle>());
            if (trainData.SA_TrainTurretData.Train_Turret_Buy_Num.Contains(num) == true)
            {
                Card.GetComponent<Store_Train_Card>().Train_Buy.SetActive(true);
            }
        }
        ResizedContent_V(Turret_Store_Content, ScrollRect_Turret);
    }
    private void Store_Buy_TurretCard()
    {
        if (playerData.Player_Coin >= trainData.EX_Game_Data.Information_Train_Turret_Part[Toggle_Turret_Num].Train_Buy_Cost)
        {
            playerData.Player_Buy_Coin(trainData.EX_Game_Data.Information_Train_Turret_Part[Toggle_Turret_Num].Train_Buy_Cost);
            trainData.SA_TrainTurretData.SA_Train_Turret_Buy(Toggle_Turret_Num);
            trainData.Check_Buy_Turret(Toggle_Turret_Num);
            Turret_Toggle[Select_Toggle_Turret_Num].isOn = false;
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
    public void Director_Init_BoosterBuy()
    {
        BuyButton_Train.interactable = false;
        for (int i = 0; i < Booster_Toggle.Count; i++)
        {
            Booster_Toggle[i].isOn = false;
        }
        ScrollRect_Booster.normalizedPosition = Vector2.up;
    }
    private void Booster_ToggleStart()
    {
        foreach (Toggle toggle in Booster_Toggle)
        {
            toggle.onValueChanged.AddListener(Booster_OnToggleValueChange);
        }
    }
    private void Booster_OnToggleValueChange(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < Booster_Toggle.Count; i++)
            {
                if (Booster_Toggle[i].isOn)
                {
                    Booster_Store_Information_Text(true, i);
                    Select_Toggle_Booster_Num = i;
                }
            }
            BuyButton_Train.interactable = true;
        }
        else
        {
            Booster_Store_Information_Text(false);
            BuyButton_Train.interactable = false;
        }
    }
    private void Booster_Store_Information_Text(bool flag, int toggle_num = -1)
    {
        if (flag)
        {
            Store_Train_Card Card = Booster_Store_Content.GetChild(toggle_num).GetComponent<Store_Train_Card>();
            Toggle_Booster_Num = Card.Train_Num2;
            Toggle_Booster_Name = trainData.EX_Game_Data.Information_Train_Booster_Part[Toggle_Booster_Num].Booster_Part_Name;
            Toggle_Booster_Cost = trainData.EX_Game_Data.Information_Train_Booster_Part[Toggle_Booster_Num].Train_Buy_Cost;

            //설계도 켜짐
            Select_Train_Blueprint.SetActive(true);
            Select_Train_Sprite.sprite = Resources.Load<Sprite>("Sprite/Train/Train_52_" + Toggle_Booster_Num);

            //기차 정보 켜짐
            Train_Information_Object.SetActive(true);
            Train_Information_Text.text = "<color=black><b>" + Toggle_Booster_Name +
                "</color></b><size=36>\n\n" + trainData.EX_Game_Data.Information_Train_Booster_Part[Toggle_Booster_Num].Train_Information.Replace("\\n", "\n");

            //Cost 정보
            Train_Information_Cost.text = Toggle_Booster_Cost + "G";
        }
        else
        {
            //설계도 꺼짐
            Select_Train_Blueprint.SetActive(false);
            //기차 정보 꺼짐
            Train_Information_Object.SetActive(false);
            //Cost 정보 꺼짐
            Train_Information_Cost.text = "0G";
        }
    }
    private void Check_Init_BoosterCard()
    {
        foreach (int num in Booster_Store_Num)
        {
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num = 52;
            Store_Train_Part_Card.GetComponent<Store_Train_Card>().Train_Num2 = num;
            GameObject Card = Instantiate(Store_Train_Part_Card, Booster_Store_Content);
            Card.name = "52_" + num.ToString();
            Booster_Toggle.Add(Card.GetComponentInChildren<Toggle>());
            if (trainData.SA_TrainBoosterData.Train_Booster_Buy_Num.Contains(num) == true)
            {
                Card.GetComponent<Store_Train_Card>().Train_Buy.SetActive(true);
            }
        }
        ResizedContent_V(Booster_Store_Content, ScrollRect_Booster);
    }
    private void Store_Buy_BoosterCard()
    {
        if (playerData.Player_Coin >= trainData.EX_Game_Data.Information_Train_Booster_Part[Toggle_Booster_Num].Train_Buy_Cost)
        {
            playerData.Player_Buy_Coin(trainData.EX_Game_Data.Information_Train_Booster_Part[Toggle_Booster_Num].Train_Buy_Cost);
            trainData.SA_TrainBoosterData.SA_Train_Booster_Buy(Toggle_Booster_Num);
            trainData.Check_Buy_Booster(Toggle_Booster_Num);
            Booster_Toggle[Select_Toggle_Booster_Num].isOn = false;
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

    //용병 구매하기
    public void Director_Init_MercenaryBuy()
    {
        BuyButton_Mercenary.interactable = false;
        for(int i = 0; i < Mercenary_Toggle.Count; i++)
        {
            Mercenary_Toggle[i].isOn = false;
        }
        ScrollRect_Mercenary.normalizedPosition = Vector2.up;
    }
    private void Mercenary_ToggleStart()
    {
        foreach(Toggle toggle in Mercenary_Toggle)
        {
            toggle.onValueChanged.AddListener(Mercenary_OnToggleValueChange);
        }
    }
    private void Mercenary_OnToggleValueChange(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < Mercenary_Toggle.Count; i++)
            {
                if (Mercenary_Toggle[i].isOn)
                {
                    Mercenary_Store_Information_Text(true, i);
                    Select_Toggle_Mercenary_Num = i;
                }
            }
            BuyButton_Mercenary.interactable = true;
        }
        else
        {
            Mercenary_Store_Information_Text(false);
            BuyButton_Mercenary.interactable = false;
        }

    }
    private void Mercenary_Store_Information_Text(bool flag, int toggle_num = -1)
    {
        if (flag)
        {
            Store_Mercenary_Card Card = Mercenary_Store_Content.GetChild(toggle_num).GetComponent<Store_Mercenary_Card>();
            Toggle_Mercenary_Num = Card.Mercenary_Num;
            Toggle_Mercenary_Name = mercenaryData.EX_Game_Data.Information_Mercenary[Toggle_Mercenary_Num].Name;

            Mercenary_Information_Text.text = "<color=black><b>" + Toggle_Mercenary_Name +
                "</color></b>\n<size=30>" + mercenaryData.EX_Game_Data.Information_Mercenary[Toggle_Mercenary_Num].Mercenary_Information.Replace("\\n", "\n");
            Mercenary_Information_Cost.text = mercenaryData.EX_Game_Data.Information_Mercenary[Toggle_Mercenary_Num].Mercenary_Pride + "G";
            Mercenary_Information_Object.SetActive(true);
        }
        else
        {
            Mercenary_Information_Object.SetActive(false);
        }

    }
    private void Check_Init_MercenaryCard() // 카드 초기화
    {
        foreach (int num in Mercenary_Store_Num)
        {
            Mercenary_Card.GetComponent<Store_Mercenary_Card>().Mercenary_Num = num;
            GameObject Card = Instantiate(Mercenary_Card, Mercenary_Store_Content);
            Card.name = num.ToString();
            Mercenary_Toggle.Add(Card.GetComponentInChildren<Toggle>());
            if (mercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Contains(num) == true)
            {
                Card.GetComponent<Store_Mercenary_Card>().Mercenary_Buy.SetActive(true);
            }
        }
        ResizedContent_V(Mercenary_Store_Content, ScrollRect_Mercenary);
    }
    private void Store_Buy_MercenaryCard() // 카드 구매 하기
    {
        if (playerData.Player_Coin >= mercenaryData.EX_Game_Data.Information_Mercenary[Toggle_Mercenary_Num].Mercenary_Pride)
        {
            playerData.Player_Buy_Coin(mercenaryData.EX_Game_Data.Information_Mercenary[Toggle_Mercenary_Num].Mercenary_Pride);
            mercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Add(Toggle_Mercenary_Num);
            Check_AfterBuy_MercenaryCard();
            Mercenary_Toggle[Select_Toggle_Mercenary_Num].isOn = false;
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
        for(int i = 0; i < Mercenary_Store_Content.childCount; i++)
        {
            GameObject Card = Mercenary_Store_Content.GetChild(i).gameObject;
            int Card_Num = Card.GetComponent<Store_Mercenary_Card>().Mercenary_Num;
            if (mercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Contains(Card_Num)){
                Card.GetComponent<Store_Mercenary_Card>().Mercenary_Buy.SetActive(true);
            }
        }
    }
    //아이템 구매
    public void ItemList_Toggle_Init()
    {
        Store_Item_List_Num = 0;
        for(int i = 0; i < Store_Item_List_Toggle.Count; i++)
        {
            if(i == 0)
            {
                Store_Item_List_Toggle[i].isOn = true;
                Store_Item_List_Toggle[i].interactable = false;
                Store_Item_List_Window[i].SetActive(true);
            }
            else
            {
                Store_Item_List_Toggle[i].isOn = false;
                Store_Item_List_Toggle[i].interactable = true;
                Store_Item_List_Window[i].SetActive(false);
            }
        }
    }

    private void StoreItemList_ToggleStart()
    {
        foreach(var toggle in Store_Item_List_Toggle)
        {
            toggle.onValueChanged.AddListener(StoreItemList_OnToggleValueChange);
        }
    }

    private void StoreItemList_OnToggleValueChange(bool isOn)
    {
        if (isOn)
        {
            for(int i = 0; i < Store_Item_List_Toggle.Count; i++)
            {
                if (Store_Item_List_Toggle[i].isOn)
                {
                    Store_Item_List_Num = i;
                    Store_Item_List_Toggle[i].isOn = true;
                    Store_Item_List_Toggle[i].interactable = false;
                    Store_Item_List_Window[i].SetActive(true);
                }
                else
                {
                    Store_Item_List_Toggle[i].isOn = false;
                    Store_Item_List_Toggle[i].interactable = true;
                    Store_Item_List_Window[i].SetActive(false);
                }
            }
        }
    }


    //아이템 구매 부분
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
                    ItemSellList_Object.item_tooltip_object = ItemSellTooltip_Object;
                    Instantiate(ItemSellList_Object, Item_Sell_Window.transform);
                }
            }
            itemData.Check_ItemChangeFlag();
            Close_Buy_Window();
        }
        else
        {
            Ban_Player_Coin_Point(true);
        }
    }

    //아이템 판매 부분
    private void Check_Init_ItemSell()
    {
        ItemSellList_Object.StoreDirector = GetComponent<Station_Store>();
        ItemSellList_Object.item_tooltip_object = ItemSellTooltip_Object;
        foreach(ItemDataObject item in itemData.Store_Sell_itemList)
        {
            ItemSellList_Object.item = item;
            Instantiate(ItemSellList_Object, Item_Sell_Window.transform);
        }
    }

    public void Director_Init_ItemSell()
    {
        foreach(ItemSell_Object item in Item_Sell_Window.GetComponentsInChildren<ItemSell_Object>())
        {
            Destroy(item.gameObject);
        }
        Check_Init_ItemSell();
    }

    private void Store_Sell_Item(ItemDataObject item)
    {
        playerData.Player_Get_Coin(item.Item_Sell_Pride * item_Count);
        item.Item_Count_Down(item_Count);
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
        itemData.Check_ItemChangeFlag();
        Close_Buy_Window();
    }

    public void Open_Buy_Window(int i)
    {
        Check_Buy_Panel_Num = i;
        Check_Buy_Panel.SetActive(true);
        if(i == 0)
        {
            if(Store_Train_Num == 0)
            {
                Check_Buy_Text.text = "\n" + Toggle_Train_Name + " 설계도를 구매하시겠습니까?";
                Buy_YesButton.onClick.AddListener(Store_Buy_TrainCard);
            }else if(Store_Train_Num == 1)
            {
                Check_Buy_Text.text = "\n" + Toggle_Turret_Name + " 설계도를 구매하시겠습니까?";
                Buy_YesButton.onClick.AddListener(Store_Buy_TurretCard);
            }else if(Store_Train_Num == 2)
            {
                Check_Buy_Text.text = "\n" + Toggle_Booster_Name + " 설계도를 구매하시겠습니까?";
                Buy_YesButton.onClick.AddListener(Store_Buy_BoosterCard);
            }

        }
        else if (i == 1)
        {
            Check_Buy_Text.text = "\n" + Toggle_Mercenary_Name + " 고용하시겠습니까?";
            Buy_YesButton.onClick.AddListener(Store_Buy_MercenaryCard);
        }
    }

    public void Open_BuyAndSell_Item_Window(ItemDataObject item, bool Flag)
    {
        Check_Buy_Panel_Num = 2;
        Check_Buy_Panel.SetActive(true);
        Item_Count_Window.SetActive(true);
        if (Flag)
        {
            Button_ItemCount_Init(true, item);
            Check_Buy_Text.text = item.Item_Name + "을(를) 구매하시겠습니까?";
            Button_ItemCount_Plus.onClick.AddListener(() => Click_ItemCount_Plus(item));
            Button_ItemCount_Minus.onClick.AddListener(() => Click_ItemCount_Minus(item));
            Buy_YesButton.onClick.AddListener(() => Store_Buy_Item(item));
        }
        else
        {
            Button_ItemCount_Init(false, item);
            Check_Buy_Text.text = item.Item_Name + "을(를) 판매하시겠습니까?";
            Button_ItemCount_Plus.onClick.AddListener(() => Click_ItemCount_Plus(item));
            Button_ItemCount_Minus.onClick.AddListener(() => Click_ItemCount_Minus(item));
            Buy_YesButton.onClick.AddListener(()=> Store_Sell_Item(item));
        }
    }

    public void Close_Buy_Window()
    {
        Check_Buy_Panel.SetActive(false);
        if (Check_Buy_Panel_Num == 0)
        {
            if (Store_Train_Num == 0)
            {
                Buy_YesButton.onClick.RemoveListener(Store_Buy_TrainCard);
            } else if (Store_Train_Num == 1) 
            {
                Buy_YesButton.onClick.RemoveListener(Store_Buy_TurretCard);
            }
            else if (Store_Train_Num == 2)
            {
                Buy_YesButton.onClick.RemoveListener(Store_Buy_BoosterCard);
            }
        }
        else if (Check_Buy_Panel_Num == 1)
        {
            Buy_YesButton.onClick.RemoveListener(Store_Buy_MercenaryCard);
        }
        else if (Check_Buy_Panel_Num == 2)
        {
            Item_Count_Window.SetActive(false);
            Buy_YesButton.onClick.RemoveAllListeners();
            Button_ItemCount_Plus.onClick.RemoveAllListeners();
            Button_ItemCount_Minus.onClick.RemoveAllListeners();
        }
    }

    private void Click_ItemCount_Plus(ItemDataObject item)
    {
        item_Count++;
        Item_Count_Text.text = item_Count.ToString();
        Check_ItemCount(item);
    }

    private void Click_ItemCount_Minus(ItemDataObject item)
    {
        item_Count--;
        Item_Count_Text.text = item_Count.ToString();
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

        if(Store_Item_List_Num == 0)
        {
            if(playerData.Player_Coin > item.Item_Buy_Pride * (item_Count + 1))
            {
                Button_ItemCount_Plus.interactable = true;
            }
            else
            {
                Button_ItemCount_Plus.interactable = false;
            }
            // 자신이 가지고 있는 골드로 제한


        }
        else if (Store_Item_List_Num == 1)
        {
            if(item.Item_Count != item_Count)
            {
                Button_ItemCount_Plus.interactable = true;
            }
            else
            {
                Button_ItemCount_Plus.interactable = false;
            }
            //자신이 가지고 있는 아이템 갯수를 제한
        }
    }

    public void Button_ItemCount_Init(bool Flag,ItemDataObject item)
    {
        item_Count = 1;
        Item_Count_Text.text = item_Count.ToString();
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


    //공통 부분
    public void ResizedContent_V(Transform ScrollContent, ScrollRect Scrollrect)
    {
        GridLayoutGroup Grid = ScrollContent.GetComponent<GridLayoutGroup>();
        Vector2 cellSize = Grid.cellSize;
        Vector2 spacing = Grid.spacing;

        float hight = (cellSize.y + spacing.y) * ScrollContent.childCount;
        RectTransform ContentSize = ScrollContent.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(ContentSize.sizeDelta.x, hight);
        Scrollrect.normalizedPosition = Vector2.up;
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
}