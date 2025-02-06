using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuy_Object : MonoBehaviour
{
    public Station_Store StoreDirector;
    public ItemDataObject item;

    public string item_name;
    public string item_information;
    public int item_pride;
    public bool item_use;
    int item_Count;

    [Header("정보 표시")]
    public Image item_icon_object;
    public TextMeshProUGUI item_Name_text;
    public TextMeshProUGUI item_Pride_text;
    public TextMeshProUGUI item_Count_text;
    public GameObject SelectObject;

    private void Start()
    {
        item_icon_object.sprite = item.Item_Sprite;

        item_name = item.Item_Name;
        item_Name_text.text = item_name;

        item_information = item.Item_Information;

        item_pride = item.Item_Buy_Pride;
        item_Pride_text.text = item_pride + "G";

        item_Count = item.Item_Count;
        item_Count_text.text = item_Count.ToString();

        item_use = item.Use_Flag;
    }

    private void Update()
    {
        if(item_Count != item.Item_Count)
        {
            item_Count = item.Item_Count;
            item_Count_text.text = item_Count.ToString();
        }
    }

    public void Click_Item()
    {
        StoreDirector.Click_ItemCheck(this.gameObject, true);
        SelectObject.SetActive(true);
    }
}