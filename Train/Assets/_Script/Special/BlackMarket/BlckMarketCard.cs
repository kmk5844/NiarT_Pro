using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackMarketCard : MonoBehaviour
{
    BlackMarketDirector director;
    ItemDataObject itemData;
    int index;

    public Image ItemImage;
    public Button ItemBuyButton;
    public TextMeshProUGUI ItemCountText;
    public TextMeshProUGUI ItemGoldText;
    public TextMeshProUGUI ItemSaleText;
    public TextMeshProUGUI ItemParsentText;

    public void SettingCard(BlackMarketDirector _director, ItemDataObject _itemData, int count, int sale)
    {
        director = _director;
        itemData = _itemData;

        ItemImage.sprite = itemData.Item_Sprite;
        ItemCountText.text = "x" + count;
        ItemGoldText.text = (count * itemData.Item_Buy_Pride).ToString();
        ItemSaleText.text = ((count * itemData.Item_Buy_Pride) * (100 - sale) / 100).ToString();
        ItemParsentText.text = sale.ToString();
        ItemBuyButton.onClick.AddListener(() => Click());
    }

    public void Click()
    {
        if (director.CheckItem(index)){
            director.ClickItem(index);
            ItemBuyButton.interactable = false;
        }
        else
        {
            director.WarningPrideWindow();
        }
    }
}
