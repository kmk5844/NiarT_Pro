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

    private void Awake()
    {
        MercenaryData_Object = GameObject.Find("MercenaryData");
        mercenaryData = MercenaryData_Object.GetComponent<Station_MercenaryData>();
        Mercenary_Count();
        Mercenary_Image.GetComponent<Image>().sprite = Mercenary_Face_Image[Mercenary_Num];
        Mercenary_NameText.GetComponent<TextMeshProUGUI>().text =
            mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Name;
        if (Mercenary_Num != 0)
        {
            dropDown.SetActive(false);
            Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count.ToString() + "명";
        }
        else
        {
            dropDown.SetActive(true);
            Mercenary_CountText.GetComponent<TextMeshProUGUI>().text = Mercenary_Num_Count + "명<color=red> / 최대 1명</color>";
        }
    }

    private void Mercenary_Count()
    {
        foreach (int M_num in mercenaryData.Mercenary_Num)
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
        if (Mercenary_Num != 0)
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
        if (Mercenary_Num != 0)
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
}
