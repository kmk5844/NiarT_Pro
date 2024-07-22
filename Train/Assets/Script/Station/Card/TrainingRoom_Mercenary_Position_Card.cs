using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingRoom_Mercenary_Position_Card : MonoBehaviour
{
    [SerializeField]
    private GameObject MercenaryData_Object;
    Station_MercenaryData mercenaryData;

    public int Mercenary_Num;
    public GameObject Mercenary_Image;
    public GameObject Mercenary_NameText;
    public GameObject Mercenary_CountText;
    public Button PlusButton;
    public Button MinusButton;
    public GameObject dropDown;
    public int Mercenary_Num_Count;
    public Sprite[] Mercenary_Face_Image;
    bool PassiveFlag;

    private void Start()
    {
        MercenaryData_Object = GameObject.Find("MercenaryData");
        mercenaryData = MercenaryData_Object.GetComponent<Station_MercenaryData>();
        Mercenary_Count();
        Mercenary_Image.GetComponent<Image>().sprite = mercenaryData.SA_MercenaryData.Mercenary_Head_Image[Mercenary_Num];
        Mercenary_NameText.GetComponent<TextMeshProUGUI>().text =
            mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Name;
        if (!mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].DropDown)
        {
            dropDown.SetActive(false);
            PassiveFlag = false;
            Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count.ToString() + "명";
        }
        else
        {
            DropDown_Option(mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Type);
            dropDown.SetActive(true);
            PassiveFlag = true;
            Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count + "명<color=red> / 최대 1명</color>";
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
        if (!PassiveFlag)
        {
            Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count.ToString() + "명";
        }
        else
        {
            Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count + "명<color=red> / 최대 1명</color>";
        }
    }

    public void Minus_Count()
    {
        Mercenary_Num_Count--;
        if (!PassiveFlag)
        {
            Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count.ToString() + "명";
        }
        else
        {
            Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count + "명<color=red> / 최대 1명</color>";
        }
    }

    public void Button_OpenClose(bool flag)
    {
        if (flag)
        {
            PlusButton.interactable = false;
            MinusButton.interactable = true;
        }
        else
        {
            PlusButton.interactable = true;
            MinusButton.interactable = false;
        }
    }

    private void DropDown_Option(string M_type)
    {
        TMP_Dropdown options = dropDown.GetComponent<TMP_Dropdown>();
        List<string> optionList = new List<string>();

        switch (M_type)
        {
            case "Engine_Driver":
                optionList.Add("기차 속도 증가");
                optionList.Add("연료 효율성 증가");
                optionList.Add("기차 방어력 증가");
                options.AddOptions(optionList);
                options.value = mercenaryData.SA_MercenaryData.SA_Get_EngineDriver_Type_DropDown_Value();
                break;
            case "Bard":
                optionList.Add("유닛 체력 증가");
                optionList.Add("유닛 공격력 증가");
                optionList.Add("유닛 방어력 증가");
                options.AddOptions(optionList);
                options.value = mercenaryData.SA_MercenaryData.SA_Get_Bard_Type_DropDown_Value();
                break;
        }
    }
}
