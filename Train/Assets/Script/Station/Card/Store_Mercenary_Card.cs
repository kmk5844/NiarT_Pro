using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store_Mercenary_Card : MonoBehaviour
{
    [SerializeField]
    public Station_MercenaryData mercenaryData;
    public Station_Store storeDirector;

    public int Mercenary_Num;
    public GameObject Mercenary_Image;
    public GameObject Mercenary_NameText;
    public GameObject Mercenary_Buy;
    string mercenary_name;
    string mercenary_information;
    int mercenary_pride;

    [Header("정보 표시")]
    public StoreList_Tooltip store_tooltip_object;

    bool Mercenary_Information_Flag;
    bool Mercenary_mouseOver_Flag;


    private void Start()
    {
        Mercenary_Image.GetComponent<Image>().sprite = mercenaryData.SA_MercenaryData.Mercenary_Head_Image[Mercenary_Num];
        mercenary_name = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Name;
        Mercenary_NameText.GetComponent<TextMeshProUGUI>().text = mercenary_name;
        mercenary_information = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Mercenary_Information;
        mercenary_pride = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Mercenary_Pride;
    }

    private void Update()
    {
        if (Mercenary_Information_Flag)
        {
            store_tooltip_object.Tooltip_ON(mercenary_name, mercenary_information, mercenary_pride);
            Mercenary_mouseOver_Flag = true;
        }
        else
        {
            if (Mercenary_mouseOver_Flag)
            {
                store_tooltip_object.Tooltip_Off();
                Mercenary_mouseOver_Flag = false;
            }
        }
    }

    public void OnMouseEnter()
    {
        if (Mercenary_Buy.activeSelf != true)
        {
            Mercenary_Information_Flag = true;
        }
    }

    public void OnMouseExit()
    {
        if (Mercenary_Buy.activeSelf != true)
        {
            Mercenary_Information_Flag = false;
        }
    }

    public void OnMouseClick()
    {
        if (Mercenary_Buy.activeSelf != true)
        {
            storeDirector.Open_Buy_Window(1, Mercenary_Num);
        }
    }
}
