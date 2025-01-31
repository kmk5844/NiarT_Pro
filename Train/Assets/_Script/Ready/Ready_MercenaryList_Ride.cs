using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class Ready_MercenaryList_Ride : MonoBehaviour
{
    public PlayerReadyDirector Director;
    public Station_MercenaryData mercenaryData;
    [SerializeField]
    private SA_LocalData SA_LocalData;

    public Image BackGround_RayCast;

    public int List_Index;
    public int Mercenary_Num;
    public Image Num_Image;
    public LocalizeStringEvent Mercenary_Name;
    public Image Mercenary_Image;
    public GameObject dropDown;

    [SerializeField]
    Sprite[] Num_Image_Sprite;

    int Local_Index;
    [SerializeField]
    LocalizedString[] LocalString_EngineDriver;
    [SerializeField]
    LocalizedString[] LocalString_Bard;


    private void Awake()
    {
        Local_Index = SA_LocalData.Local_Index;
        Mercenary_Name.StringReference.TableReference = "ExcelData_Table_St";
    }

    private void Start()
    {
        Check_IndexNum();
        Mercenary_Image.sprite = Resources.Load<Sprite>("Sprite/Mercenary/" + Mercenary_Num);
        Mercenary_Name.StringReference.TableEntryReference = "Mercenary_Name_" + Mercenary_Num;

        if (Mercenary_Num == -1 || !mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Passive)
        {
            dropDown.SetActive(false);
        }
        else
        {
            DropDown_Option(mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Type);
            dropDown.SetActive(true);
        }
    }

    private void Update()
    {
        if(Mercenary_Num != -1)
        {
            if (mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Passive)
            {
                if (Local_Index != SA_LocalData.Local_Index)
                {
                    DropDown_Option_Change(mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Type);
                    Local_Index = SA_LocalData.Local_Index;
                }
            }
        }

        if(Director.MercenaryLIst_Mercenary_Num == -5)
        {
            BackGround_RayCast.raycastTarget = false;
        }
        else
        {
            BackGround_RayCast.raycastTarget = true;
        }
    }

    public void ChangeMercenary(int Mer_Num)
    {
        int Before_Mercenary_Num = Mercenary_Num;
        Mercenary_Num = Mer_Num;
        Mercenary_Image.sprite = Resources.Load<Sprite>("Sprite/Mercenary/" + Mercenary_Num);
        Mercenary_Name.StringReference.TableEntryReference = "Mercenary_Name_" + Mercenary_Num;

        if (Mercenary_Num == -1 || !mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Passive)
        {
            dropDown.SetActive(false);
        }
        else
        {
            DropDown_Option(mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Type);
            dropDown.SetActive(true);
        }
        mercenaryData.SA_MercenaryData.SA_Mercenary_Change(List_Index, Mer_Num);
        Director.Check_MercenaryList(Before_Mercenary_Num);
        Director.Check_Mercenary_Max();
    }

    private void DropDown_Option(string M_type)
    {
        dropDown.GetComponent<TMP_Dropdown>().ClearOptions();
        TMP_Dropdown options = dropDown.GetComponent<TMP_Dropdown>();
        List<string> optionList = new List<string>();
        Director.ChangeDrowDown_Option(options, M_type);
        switch (M_type)
        {
            case "Engine_Driver":
                optionList.Add(LocalString_EngineDriver[0].GetLocalizedString());
                optionList.Add(LocalString_EngineDriver[1].GetLocalizedString());
                optionList.Add(LocalString_EngineDriver[2].GetLocalizedString());
                options.AddOptions(optionList);
                options.value = mercenaryData.SA_MercenaryData.SA_Get_EngineDriver_Type_DropDown_Value();
                break;
            case "Bard":
                optionList.Add(LocalString_Bard[0].GetLocalizedString());
                optionList.Add(LocalString_Bard[1].GetLocalizedString());
                optionList.Add(LocalString_Bard[2].GetLocalizedString());
                options.AddOptions(optionList);
                options.value = mercenaryData.SA_MercenaryData.SA_Get_Bard_Type_DropDown_Value();
                break;
        }
    }

    private void DropDown_Option_Change(string M_type)
    {
        TMP_Dropdown options = dropDown.GetComponent<TMP_Dropdown>();
        switch (M_type)
        {
            case "Engine_Driver":
                options.options[0].text = LocalString_EngineDriver[0].GetLocalizedString();
                options.options[1].text = LocalString_EngineDriver[1].GetLocalizedString();
                options.options[2].text = LocalString_EngineDriver[2].GetLocalizedString();

                options.captionText.text = options.options[options.value].text;
                break;
            case "Bard":
                options.options[0].text = LocalString_Bard[0].GetLocalizedString();
                options.options[1].text = LocalString_Bard[1].GetLocalizedString();
                options.options[2].text = LocalString_Bard[2].GetLocalizedString();

                options.captionText.text = options.options[options.value].text;
                break;
        }
    }

    void Check_IndexNum()
    {
        Num_Image.sprite = Num_Image_Sprite[List_Index];
    }

    public void Mercenary_Quit_Button()
    {
        ChangeMercenary(-1);
    }

    public void OnMouseEnter()
    {
        if(Director.MercenaryLIst_Mercenary_Num != -5)
        {
            Director.Mercenary_Ride_List_Index = List_Index;
        }
    }

    public void OnMouseExit()
    {
        if(Director.MercenaryLIst_Mercenary_Num != -5)
        {
            Director.Mercenary_Ride_List_Index = -5;
        }
    }
}