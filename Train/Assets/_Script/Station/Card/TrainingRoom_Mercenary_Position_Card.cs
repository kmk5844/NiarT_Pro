using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
public class TrainingRoom_Mercenary_Position_Card : MonoBehaviour
{
    [SerializeField]
    private GameObject MercenaryData_Object;
    Station_MercenaryData mercenaryData;
    [SerializeField]
    private SA_LocalData SA_LocalData;

    public int Mercenary_Num;
    public GameObject Mercenary_Image;
    public LocalizeStringEvent Mercenary_NameText;
    public GameObject Mercenary_CountText;
    public GameObject Mercenary_MaxText;
    public Button PlusButton;
    public Button MinusButton;
    public GameObject dropDown;
    public int Mercenary_Num_Count;
    public int Mercenary_Max_Count;
    public Sprite[] Mercenary_Face_Image;

    int Local_Index;
    [SerializeField]
    LocalizedString[] LocalString_EngineDriver;
    [SerializeField]
    LocalizedString[] LocalString_Bard;

    private void Awake()
    {
        Local_Index = SA_LocalData.Local_Index;

        MercenaryData_Object = GameObject.Find("MercenaryData");
        mercenaryData = MercenaryData_Object.GetComponent<Station_MercenaryData>();
        Mercenary_Count();
        Mercenary_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Mercenary/" + Mercenary_Num);
        Mercenary_NameText.StringReference.TableReference = "ExcelData_Table_St";
        Mercenary_NameText.StringReference.TableEntryReference = "Mercenary_Name_" + Mercenary_Num;

        Mercenary_Max_Count = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Max_Mercenary;

        if (!mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Passive)
        {
            dropDown.SetActive(false);
        }
        else
        {
            DropDown_Option(mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Type);
            dropDown.SetActive(true);
        }
        Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count.ToString();
        if (mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Max_Mercenary != -1)
        {
            Mercenary_MaxText.GetComponent<TextMeshProUGUI>().text = "/ Max " + Mercenary_Max_Count;
        }
        else
        {
            Mercenary_MaxText.SetActive(false);
            Mercenary_CountText.transform.localPosition = new Vector3(-80, -75, 0);
        }
    }

    private void Update()
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

    private void Mercenary_Count()
    {
        foreach (int M_num in mercenaryData.SA_MercenaryData.Mercenary_Num)
        {
            if (M_num == Mercenary_Num)
            {
                Mercenary_Num_Count++;
            }
        }
    }

    public void Plus_Count()
    {
        Mercenary_Num_Count++;
        Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count.ToString();
    }

    public void Minus_Count()
    {
        Mercenary_Num_Count--;
        Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count.ToString();
    }

    private void DropDown_Option(string M_type)
    {
        TMP_Dropdown options = dropDown.GetComponent<TMP_Dropdown>();
        List<string> optionList = new List<string>();

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
}
