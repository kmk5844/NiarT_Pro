using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class BlackMarketCard : MonoBehaviour
{
    BlackMarketDirector director;
    ItemDataObject itemData;
    int index;

    public Image ItemImage;
    public Button ItemBuyButton;
    public LocalizeStringEvent ItemNameText;
    public TextMeshProUGUI ItemCountText;
    public TextMeshProUGUI ItemGoldText;
    public TextMeshProUGUI ItemSaleText;
    public TextMeshProUGUI ItemParsentText;

    public void SettingCard(int _index, BlackMarketDirector _director, ItemDataObject _itemData, int count, int sale)
    {
        index = _index;
        director = _director;
        itemData = _itemData;

        ItemImage.sprite = itemData.Item_Sprite;
        ItemNameText.StringReference.TableReference = "ItemData_Table_St";
        ItemNameText.StringReference.TableEntryReference = "Item_Name_" + _itemData.Num;
        ItemCountText.text = "x" + count;
        ItemGoldText.text = (count * itemData.Item_Buy_Pride) + " G";
        ItemSaleText.text = ((count * itemData.Item_Buy_Pride) * (100 - sale) / 100) + " G";
        ItemParsentText.text = sale + "%";
        ItemBuyButton.onClick.AddListener(() => Click());
        ItemBuyButton.interactable = true;
    }

    public void Click()
    {
        if (director.CheckItem(index))
        {
            director.ClickItem(index);
            ItemBuyButton.interactable = false;
            ItemImage.gameObject.SetActive(false);
            ItemCountText.gameObject.SetActive(false);
            ItemParsentText.gameObject.SetActive(false);
        }
        else
        {
            director.WarningPrideWindow();
        }
    }
}
