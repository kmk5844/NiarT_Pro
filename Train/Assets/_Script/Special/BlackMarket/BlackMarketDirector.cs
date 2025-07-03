using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackMarketDirector : MonoBehaviour
{
    public SA_ItemList itemList;
    public ItemDataObject[] item_Arr;
    public int[] itemCount_Arr;
    public int[] Persent_Arr;

    [Header("Card")]
    public Image[] ItemImage;
    public TextMeshProUGUI[] ItemCountText;
    public TextMeshProUGUI[] ItemGoldText;
    public TextMeshProUGUI[] ItemSaleText;
    public TextMeshProUGUI[] ItemParsentText;

    // Start is called before the first frame update
    void Start()
    {
        SettingCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SettingCard()
    {
        item_Arr = new ItemDataObject[3];
        itemCount_Arr = new int[3];
        Persent_Arr = new int[3];
        for (int i = 0; i < 3; i++)
        {
            int num = Random.Range(0, 20);
            int count = Random.Range(0,6);
            int persent = Random.Range(5, 50);
            ItemDataObject _Item = itemList.Item[num];
            item_Arr[i] = _Item;
            itemCount_Arr[i]= count;
            ItemImage[i].sprite = _Item.Item_Sprite;
            ItemCountText[i].text = "x" + count;
            ItemGoldText[i].text = (count * _Item.Item_Buy_Pride).ToString();
            ItemSaleText[i].text = ((count * _Item.Item_Buy_Pride) * (100 - persent) /100).ToString();
            ItemParsentText[i].text = persent.ToString();
        }
    }
}
