using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class Store_Mercenary_Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    public Station_MercenaryData mercenaryData;
    public Station_Store storeDirector;

    public int Mercenary_Num;
    public GameObject Mercenary_Image;
    public LocalizeStringEvent Mercenary_NameText;
    public GameObject Mercenary_Buy;
    int mercenary_pride;

    [Header("정보 표시")]
    public StoreList_Tooltip store_tooltip_object;

    bool Mercenary_Information_Flag;
    bool Mercenary_mouseOver_Flag;


    private void Start()
    {
        Mercenary_Image.GetComponent<Image>().sprite = mercenaryData.SA_MercenaryData.Mercenary_Head_Image[Mercenary_Num];
        Mercenary_NameText.StringReference.TableReference = "ExcelData_Table_St";
        Mercenary_NameText.StringReference.TableEntryReference = "Mercenary_Name_" + Mercenary_Num;
        //mercenary_name = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Name;
        //Mercenary_NameText.GetComponent<TextMeshProUGUI>().text = mercenary_name;
        mercenary_pride = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Mercenary_Pride;
    }

    private void Update()
    {
        if (StationDirector.TooltipFlag)
        {
            if (Mercenary_Information_Flag)
            {
                store_tooltip_object.Tooltip_ON(false, mercenary_pride, Mercenary_Num);
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
        else
        {
            if (Mercenary_Information_Flag)
            {
                Mercenary_Information_Flag = false;
                Mercenary_mouseOver_Flag = false;
                store_tooltip_object.Tooltip_Off();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Mercenary_Buy.activeSelf != true)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                storeDirector.Open_Buy_Window(1, Mercenary_Num);
            }
        }
    }
}
