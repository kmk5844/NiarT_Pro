using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Station_Store : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Mercenary_DataObject;
    Station_MercenaryData mercenaryData;

    [Header("윈도우")]
    public GameObject Check_Buy_Panel;
    public TextMeshProUGUI Check_Buy_Text;
    public Button Buy_YesButton;
    public Button Buy_NoButton;
    int Check_Buy_Panel_Num;

    [Header("기차 구매")]
    [SerializeField]
    List<int> Train_Store_Num;
    public Transform Train_Store_Content;
    public ScrollRect ScrollRect_Train;
    public GameObject Train_Card;
    public Button BuyButton_Train;
    public TextMeshProUGUI Train_Card_Information;
    [SerializeField]
    List<Toggle> Train_Toggle;
    int Toggle_Trian_Num;
    string Toggle_Train_Name;


    [Header("용병 구매")]
    [SerializeField]
    List<int> Mercenary_Store_Num;
    public Transform Mercenary_Store_Content;
    public ScrollRect ScrollRect_Mercenary;
    public Button BuyButton_Mercenary;
    public GameObject Mercenary_Card;
    public TextMeshProUGUI Mercenary_Card_Information;
    [SerializeField]
    List <Toggle> Mercenary_Toggle;
    int Toggle_Mercenary_Num; // toggle로 찍힌 카드 안의 숫자
    string Toggle_Mercenary_Name; // toggle로 찍힌 카드 안의 이름


    private void Start()
    {
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        Train_Store_Num = trainData.Train_Store_Num;
        Mercenary_Store_Num = mercenaryData.Mercenary_Store_Num;
        //기차 구매하기
        Check_Init_TrainCard();
        Director_Init_TrainyBuy();
        Train_ToggleStart();
        //용병 구매하기
        Check_Init_MercenaryCard();
        Mercenary_ToggleStart();
    }

    //기차 구매하기
    public void Director_Init_TrainyBuy()
    {
        BuyButton_Train.interactable = false;
        for (int i = 0; i < Train_Toggle.Count; i++)
        {
            Train_Toggle[i].isOn = false;
        }
        ScrollRect_Train.normalizedPosition = Vector2.zero;
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
    public void Train_Store_Information_Text(bool flag, int toggle_num = -1)
    {
        if (flag)
        {
            Store_Train_Card Card = Train_Store_Content.GetChild(toggle_num).GetComponent<Store_Train_Card>();
            Toggle_Trian_Num = Card.Train_Num;
            Toggle_Train_Name = trainData.EX_Game_Data.Information_Train[Toggle_Trian_Num].Train_Name;

            Train_Card_Information.text = Toggle_Train_Name +
                "\n" + trainData.EX_Game_Data.Information_Train[Toggle_Trian_Num].Train_Information;
        }
        else
        {
            Train_Card_Information.text = "Choice Train";
        }

    }
    private void Check_Init_TrainCard()
    {
        foreach (int num in Train_Store_Num)
        {
            Train_Card.GetComponent<Store_Train_Card>().Train_Num = num;
            GameObject Card = Instantiate(Train_Card, Train_Store_Content);
            Card.name = num.ToString();
            Train_Toggle.Add(Card.GetComponentInChildren<Toggle>());
            if (trainData.SA_TrainData.Train_Buy_Num.Contains(num) == true)
            {
                Card.GetComponent<Store_Train_Card>().Train_Buy.SetActive(true);
            }
        }
        ResizedContent(Train_Store_Content, ScrollRect_Train);
    }

    private void Store_Buy_TrainCard()
    {
        trainData.SA_TrainData.Train_Buy_Num.Add(Toggle_Trian_Num);
        trainData.Check_Buy_Train(Toggle_Trian_Num);
        Check_AfterBuy_TrainCard();
        Close_Buy_Window();
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


    //용병 구매하기
    public void Director_Init_MercenaryBuy()
    {
        BuyButton_Mercenary.interactable = false;
        for(int i = 0; i < Mercenary_Toggle.Count; i++)
        {
            Mercenary_Toggle[i].isOn = false;
        }
        ScrollRect_Mercenary.normalizedPosition = Vector2.zero;
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

            Mercenary_Card_Information.text = Toggle_Mercenary_Name +
                "\n" + mercenaryData.EX_Game_Data.Information_Mercenary[Toggle_Mercenary_Num].Mercenary_Information;
        }
        else
        {
            Mercenary_Card_Information.text = "Choice Mercenary";
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
        ResizedContent(Mercenary_Store_Content, ScrollRect_Mercenary);
    }

    private void Store_Buy_MercenaryCard() // 카드 구매 하기
    {
        mercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Add(Toggle_Mercenary_Num);
        Check_AfterBuy_MercenaryCard();
        Close_Buy_Window();
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

    public void Open_Buy_Window(int i)
    {
        Check_Buy_Panel_Num = i;
        Check_Buy_Panel.SetActive(true);
        if(i == 0)
        {
            Check_Buy_Text.text = "Would you like to purchase an " + Toggle_Train_Name + "?";
            Buy_YesButton.onClick.AddListener(Store_Buy_TrainCard);
        }
        else if (i == 1)
        {
            Check_Buy_Text.text = "Would you like to purchase an " + Toggle_Mercenary_Name + "?";
            Buy_YesButton.onClick.AddListener(Store_Buy_MercenaryCard);

        }
        else if(i == 2)
        {
            Check_Buy_Text.text = "Would you like to purchase an Item?";
        }
    }

    public void Close_Buy_Window()
    {
        Check_Buy_Panel.SetActive(false);
        if (Check_Buy_Panel_Num == 0)
        {
            Buy_YesButton.onClick.RemoveListener(Store_Buy_TrainCard);
        }
        else if (Check_Buy_Panel_Num == 1)
        {
            Buy_YesButton.onClick.RemoveListener(Store_Buy_MercenaryCard);
        }
        else if (Check_Buy_Panel_Num == 2)
        {

        }
    }

    //공통 부분
    public void ResizedContent(Transform ScrollContent, ScrollRect Scrollrect)
    {
        GridLayoutGroup Grid = ScrollContent.GetComponent<GridLayoutGroup>();
        Vector2 cellSize = Grid.cellSize;
        Vector2 spacing = Grid.spacing;

        float width = (cellSize.x + spacing.x)  * (ScrollContent.childCount -1) -800;
        RectTransform ContentSize = ScrollContent.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2 (width, ContentSize.sizeDelta.y);
        Scrollrect.normalizedPosition = Vector2.zero;
    }
}
