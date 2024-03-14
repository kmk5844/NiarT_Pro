using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Station_Store : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Player_DataObject;
    Station_PlayerData playerData;
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Mercenary_DataObject;
    Station_MercenaryData mercenaryData;

    [Header("구매창")]
    public GameObject Check_Buy_Panel;
    public TextMeshProUGUI Check_Buy_Text;
    public Button Buy_YesButton;
    public Button Buy_NoButton;
    int Check_Buy_Panel_Num;

    [Header("용병 구매")]
    [SerializeField]
    List<int> Mercenary_Store_Num;
    public Transform Mercenary_Store_Content;
    public GameObject Mercenary_Card;
    public TextMeshProUGUI Mercenary_Card_Information;
    public List <Toggle> Mercenary_Toggle;
    int Toggle_Mercenary_Num; // toggle로 찍힌 카드 안의 숫자
    string Toggle_Mercenary_Name; // toggle로 찍힌 카드 안의 이름


    private void Start()
    {
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        Mercenary_Store_Num = mercenaryData.Mercenary_Store_Num;
        Check_Init_MercenaryCard();
        Mercenary_ToggleStart();
    }

    private void Mercenary_ToggleStart()
    {
        foreach(Toggle toggle in Mercenary_Toggle)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChange);
        }
    }

    private void OnToggleValueChange(bool isOn)
    {
        for(int i = 0; i < Mercenary_Toggle.Count; i++)
        {
            if (Mercenary_Toggle[i].isOn)
            {
                Mercenary_Store_Information_Text(i);
            }
        }
    }

    private void Mercenary_Store_Information_Text(int toggle_num)
    {
        Store_Mercenary_Card Card = Mercenary_Store_Content.GetChild(toggle_num).GetComponent<Store_Mercenary_Card>();
        Toggle_Mercenary_Num = Card.Mercenary_Num;
        Toggle_Mercenary_Name = mercenaryData.EX_Game_Data.Information_Mercenary[Toggle_Mercenary_Num].Name;
        Mercenary_Card_Information.text = Toggle_Mercenary_Name +
            "\n" + mercenaryData.EX_Game_Data.Information_Mercenary[Toggle_Mercenary_Num].Mercenary_Information;
    }

    private void Check_Init_MercenaryCard() // 카드 초기화
    {
        RectTransform ContentSize = Mercenary_Store_Content.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(400 * Mercenary_Store_Num.Count, ContentSize.sizeDelta.y);
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
    }
    private void Store_Buy_MercenaryCard() // 카드 구매 하기
    {
        mercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Add(Toggle_Mercenary_Num);
        Check_AfterBuy_MercenaryCard();
        Close_Buy_Window();
        //Toggle_Mercenary_Num을 가지고 구매하고 리스트 등록
        //구매 후, 체크하고 클로즈
    }
    private void Check_AfterBuy_MercenaryCard() //카드 구매 후, 체크하기
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
            Check_Buy_Text.text = "Would you like to purchase an Train?";
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

        }
        else if (Check_Buy_Panel_Num == 1)
        {
            Buy_YesButton.onClick.RemoveListener(Store_Buy_MercenaryCard);
        }
        else if (Check_Buy_Panel_Num == 2)
        {

        }
    }
}
