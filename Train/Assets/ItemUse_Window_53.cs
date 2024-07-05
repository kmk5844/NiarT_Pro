using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUse_Window_53 : MonoBehaviour
{
    public ItemDataObject Convertion_Object;

    public GameObject BeforeConvertWindow;
    public GameObject AfterConvertWindow;

    public ToggleGroup ChoiceMaterial;
    public GameObject[] ChoiceMaterial_Window;

    public ToggleGroup Material_ToggleGroup_1;
    public ToggleGroup Material_ToggleGroup_2;

    public TextMeshProUGUI Conversion_Material_Text;
    public TextMeshProUGUI Materail_Text_1;
    public TextMeshProUGUI Materail_Text_2;

    public TextMeshProUGUI ConvertText;
    public Button Convert_Button;

    ItemDataObject Material_ToggleNum_ItemObject1;
    ItemDataObject Material_ToggleNum_ItemObject2;
    int Material_ToggleNum1;

    int convertCount;
    public TextMeshProUGUI CountText;
    public Button ConvertCountUp;
    public Button ConvertCountDown;

    //After
    public TextMeshProUGUI ResultText;
    public TextMeshProUGUI ResultText2;

    private void Start()
    {
        convertCount = 1;
        CountText.text = convertCount + "��";
        ConvertCountDown.onClick.AddListener(() => Button_CountDown());
        ConvertCountUp.onClick.AddListener(() => Button_CountUP());
        ConvertCountDown.interactable = false;

        Convert_Button.onClick.AddListener(() => Button_Convert());

        ChoiceMaterial_ToggleStart();
        ChoiceMaterial_ItemList_ToggleStart();
        Conversion_Material_Text.text = "ID : " + Convertion_Object.Num + "\n" + Convertion_Object.Item_Count;
        Materail_Text_1.text = "?";
        Materail_Text_2.text = "?";
        Material_ToggleNum1 = -1;
        Check_Button();
    }

    private void ChoiceMaterial_ToggleStart()
    {
        foreach(Toggle toggle in ChoiceMaterial.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(ChoiceMaterial_ToggleChange);
        }
    }

    private void ChoiceMaterial_ItemList_ToggleStart()
    {
        foreach(Toggle toggle in Material_ToggleGroup_1.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(ChoiceMaterial_ItemList_ToggleChange_1);
        }
        foreach (Toggle toggle in Material_ToggleGroup_2.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(ChoiceMaterial_ItemList_ToggleChange_2);
        }
    }

    private void ChoiceMaterial_ToggleChange(bool isON)
    {
        if (isON)
        {
            for (int i = 0; i < ChoiceMaterial.transform.childCount; i++)
            {
                if (ChoiceMaterial.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    ChoiceMaterial_Window[i].SetActive(true);
                }
                else
                {
                    ChoiceMaterial_Window[i].SetActive(false);
                }
            }
        }
    }

    
    private void ChoiceMaterial_ItemList_ToggleChange_1(bool isON)
    {
        bool anyToggleON = false;
        
        if (isON)
        {
            for (int i = 0; i < Material_ToggleGroup_1.transform.childCount; i++)
            {
                if (Material_ToggleGroup_1.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    anyToggleON = true;
                    Material_ToggleNum1 = i;
                    Material_ToggleNum_ItemObject1 = Material_ToggleGroup_1.transform.GetChild(i).GetComponent<ItemUse_Window_53_ToggleObject>().item;
                }
            }

            for(int i = 0; i < Material_ToggleGroup_2.transform.childCount; i++) //������ ��ȸ �ؾߵ�, �׷��� ������ ������ ����.
            {
                if(i == Material_ToggleNum1)
                {
                    Material_ToggleGroup_2.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    Material_ToggleGroup_2.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                }
            }
        }

        if (anyToggleON)
        {
            Materail_Text_1.text = "ID : " + Material_ToggleNum_ItemObject1.Num + "\n" + Material_ToggleNum_ItemObject1.Item_Count;
            ChoiceMaterial.transform.GetChild(1).GetComponent<Toggle>().interactable = true;
        }
        else
        {
            Materail_Text_1.text = "?";
            Materail_Text_2.text = "?";
            Material_ToggleNum_ItemObject1 = null;
            Material_ToggleNum_ItemObject2 = null;
            ChoiceMaterial.transform.GetChild(1).GetComponent<Toggle>().interactable = false;
            for (int i = 0; i < Material_ToggleGroup_2.transform.childCount; i++)
            {
                if (Material_ToggleGroup_2.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    Material_ToggleGroup_2.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
                }
                Material_ToggleGroup_2.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
            }
        }
        Check_Button();
    }
    private void ChoiceMaterial_ItemList_ToggleChange_2(bool isON)
    {
        if (isON)
        {
            for (int i = 0; i < Material_ToggleGroup_2.transform.childCount; i++)
            {
                if (Material_ToggleGroup_2.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    Material_ToggleNum_ItemObject2 = Material_ToggleGroup_2.transform.GetChild(i).GetComponent<ItemUse_Window_53_ToggleObject>().item;
                }
            }
            Materail_Text_2.text = "ID : " + Material_ToggleNum_ItemObject2.Num + "\n" + Material_ToggleNum_ItemObject2.Item_Count;
        }
        Check_Button();
    }

    private void Check_Button()
    {
        if (Convertion_Object.Item_Count >= 10 * convertCount)
        {
            if(Material_ToggleNum_ItemObject1 == null)
            {
                ConvertText.text = ("��ȯ�� ����� ��Ḧ �����ϼ���");
                Convert_Button.interactable = false;
            }else{
                if(Material_ToggleNum_ItemObject1.Item_Count >= convertCount)
                {
                    if (Material_ToggleNum_ItemObject2 != null)
                    {
                        ConvertText.text = ("��ȯ �����մϴ�!");
                        Convert_Button.interactable = true;
                    }
                    else
                    {
                        ConvertText.text = ("��ȯ�� ��Ḧ �����ϼ���");
                        Convert_Button.interactable = false;
                    }
                }
                else
                {
                    ConvertText.text = ("��� ���� �����մϴ�!");
                    Convert_Button.interactable = false;
                }
            }
        }
        else
        {
            ConvertText.text = ("��ȯ ��ᰡ �����մϴ�. (��ȯ ��� 10���� 1�� ��ȯ ����)");
            Convert_Button.interactable = false;
        }
    }

    private void Button_CountUP()
    {
        convertCount++;
        CountText.text = convertCount + "��";

        if (ConvertCountDown.interactable == false)
        {
            ConvertCountDown.interactable = true;
        }
        Check_Button();
    }

    private void Button_CountDown()
    {
        convertCount--;
        CountText.text = convertCount + "��";

        if(convertCount == 1)
        {
            ConvertCountDown.interactable = false;
        }
        Check_Button();
    }

    private void Button_Convert()
    {
        Convertion_Object.Item_Count_Down(10 * convertCount);
        Material_ToggleNum_ItemObject1.Item_Count_Down(convertCount);
        Material_ToggleNum_ItemObject2.Item_Count_UP(convertCount);
        BeforeConvertWindow.SetActive(false);
        
        ResultText.text = "ID : " + Material_ToggleNum_ItemObject2.Num + "\n" + Material_ToggleNum_ItemObject2.Item_Count;
        ResultText2.text = "<color=red>" + Material_ToggleNum_ItemObject2.Item_Name + "</color>�� ��ȯ �����߽��ϴ�.";
        AfterConvertWindow.SetActive(true);
    }

    public void Item_53_Init()
    {
        convertCount = 1;
        CountText.text = convertCount + "��";
        ConvertCountDown.interactable = false;

        BeforeConvertWindow.SetActive(true);
        AfterConvertWindow.SetActive(false);

        foreach(GameObject window in ChoiceMaterial_Window)
        {
            window.SetActive(false);
        }

        for(int i = 0; i < Material_ToggleGroup_1.transform.childCount; i++)
        {
            Material_ToggleGroup_1.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
        }
        Material_ToggleNum1 = -1;

        Conversion_Material_Text.text = "ID : " + Convertion_Object.Num + "\n" + Convertion_Object.Item_Count;
        Materail_Text_1.text = "?";
        Materail_Text_2.text = "?";

        Material_ToggleNum_ItemObject1 = null;
        Material_ToggleNum_ItemObject2 = null;
    }
}