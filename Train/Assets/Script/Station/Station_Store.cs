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

    [Header("용병 구매")]
    [SerializeField]
    List<int> Mercenary_Store_Num;
    public Transform Mercenary_Store_Content;
    public GameObject Mercenary_Card;
    public TextMeshProUGUI Mercenary_Card_Information;
    public List <Toggle> Mercenary_Toggle;


    private void Start()
    {
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        Mercenary_Store_Num = mercenaryData.Mercenary_Store_Num;
        Check_Init_MercenaryCard();
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
        //for(int i = 0; i < Mercenary_Toggle)
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
}
