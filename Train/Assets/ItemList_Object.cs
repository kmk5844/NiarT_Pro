using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemList_Object : MonoBehaviour
{
    public ItemDataObject item;

    public string item_name;
    public string item_information;
    public int item_count;
    public bool item_use;

    [Header("정보 표시")]
    public TextMeshProUGUI item_object_text_count;

    public GameObject item_information_Window;
    TextMeshProUGUI item_information_text_name;
    TextMeshProUGUI item_information_text_information;


    bool item_information_Flag;

    private void Start()
    {
        item_information_Flag = false;
        item_name = item.name;
        item_information = item.Item_Information;
        item_count = item.Item_Count;
        item_information_text_name = item_information_Window.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        item_information_text_information = item_information_Window.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        item_object_text_count.text = item_count.ToString();
        item_information_text_name.text = item_name;
        item_information_text_information.text = item_information;

    }

    private void Update()
    {
        if (item_information_Flag)
        {
            item_information_Window.SetActive(true);
        }
        else
        {
            item_information_Window.SetActive(false);
        }
    }

    public void OnMouseEnter()
    {
        item_information_Flag = true;
    }

    public void OnMouseExit()
    {
        item_information_Flag = false;
    }

    public void OnMouseClick()
    {
        if (item_use)
        {
            Debug.Log("클릭!!");
        }
    }
}