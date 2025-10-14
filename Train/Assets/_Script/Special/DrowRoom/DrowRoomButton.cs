using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class DrowRoomButton : MonoBehaviour
{
    public ItemDataObject ItemDataObject_Item;
    DrowRoomDirector director;
    public bool flag;
    public Button button;
    public GameObject QuestionIcon;
    public Image image;

    public LocalizeStringEvent ItemName;
    public TextMeshProUGUI GoldText;

    void Start()
    {
        ItemName.StringReference.TableReference = "ItemData_Table_St";
    }

    public void ClickButton()
    {
        flag = true;
        button.interactable = false;
        QuestionIcon.SetActive(false);
        image.gameObject.SetActive(true);
        ItemName.gameObject.SetActive(true);
        ItemDataObject_Item.Item_Count_UP();
        director.PlayerCoinPay();
        director.CheckCard();
    }

    public void SettingItem(ItemDataObject item, DrowRoomDirector dir)
    {
        flag = false;
        ItemDataObject_Item = item;
        image.sprite = ItemDataObject_Item.Item_Sprite;
        ItemName.StringReference.TableEntryReference = "Item_Name_" + item.Num;
        director = dir;
    }

    public void ChangeGold(int num)
    {
        GoldText.text = num + " G";
    }
}
