using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class Station_Conversion : MonoBehaviour
{
    [SerializeField]
    private int RequireItemCount;

    [Header("데이터 관리")]
    public GameObject itemData_object; 
    Station_ItemData itemData;
    public Station_Inventory inventory;

    [Header("변환 전의 윈도우")]
    public GameObject BeforeConvertWindow;
    public ItemDataObject Convertion_Object;

    public ToggleGroup ChoiceMaterial;
    public GameObject[] ChoiceMaterial_Window;
    ToggleGroup[] Material_ToggleGroup;

    public Image Conversion_Material_Image;
    public LocalizeStringEvent Conversion_Material_Name_Text;
    public TextMeshProUGUI Conversion_Material_Count_Text;
    public Image Material_Image_1;
    public TextMeshProUGUI Material_Count_Text_1;
    public LocalizeStringEvent Material_Name_Text_1;
    public Image Material_Image_2;
    public TextMeshProUGUI Material_Count_Text_2;
    public LocalizeStringEvent Material_Name_Text_2;
    public GameObject[] Material_LockPanel_2;

    public LocalizeStringEvent ConvertText;
    public Button Convert_Button;

    ItemDataObject Material_ToggleNum_ItemObject1;
    ItemDataObject Material_ToggleNum_ItemObject2;
    int Material_ToggleNum1;

    [Header("수량 선택")]
    public Button ConvertCountUp;
    public Button ConvertCountDown;
    int convertCount;
    public TextMeshProUGUI CountText;

    [Header("변환 후의 윈도우")]
    public GameObject AfterConvertWindow;
    public Image Result_Icon_Img;
    public LocalizeStringEvent Result_Name_Text;
    public TextMeshProUGUI Result_Count_Text;

    public Sprite Empty_Sprite;
    [HideInInspector]
    public bool AfterConversionFlag;

    private void Start()
    {
        RequireItemCount = 5;
        AfterConversionFlag = false;
        itemData = itemData_object.GetComponent<Station_ItemData>();
        Convertion_Object = itemData.ConvertionMaterial_object;
        {
            ConvertText.StringReference.TableReference = "Station_Table_St";
            Conversion_Material_Name_Text.StringReference.TableReference = "ItemData_Table_St";
            Result_Name_Text.StringReference.TableReference = "ItemData_Table_St";
        }

        Material_ToggleGroup = new ToggleGroup[ChoiceMaterial_Window.Length];
        for (int i = 0; i < ChoiceMaterial_Window.Length; i++)
        {
            Material_ToggleGroup[i] = ChoiceMaterial_Window[i].GetComponent<ToggleGroup>();
            Material_ToggleGroup[i].transform.GetChild(0).GetComponent<ItemUse_Window_53_ToggleObject>().item = itemData.Mercenary_Material_object;
            Material_ToggleGroup[i].transform.GetChild(1).GetComponent<ItemUse_Window_53_ToggleObject>().item = itemData.Common_Train_Material_object;
            Material_ToggleGroup[i].transform.GetChild(2).GetComponent<ItemUse_Window_53_ToggleObject>().item = itemData.Turret_Train_Material_object;
            Material_ToggleGroup[i].transform.GetChild(3).GetComponent<ItemUse_Window_53_ToggleObject>().item = itemData.Booster_Train_Material_object;
        }
        
        convertCount = 1;
        CountText.text = convertCount.ToString();
        ConvertCountDown.onClick.AddListener(() => Button_CountDown());
        ConvertCountUp.onClick.AddListener(() => Button_CountUP());
        ConvertCountDown.interactable = false;

        Convert_Button.onClick.AddListener(() => Button_Convert());

        ChoiceMaterial_ToggleStart();
        ChoiceMaterial_ItemList_ToggleStart();
        Conversion_Material_Image.sprite = Convertion_Object.Item_Sprite;
        Conversion_Material_Name_Text.StringReference.TableEntryReference = "Item_Name_" + Convertion_Object.Num;
        //Conversion_Material_Name_Text.text = Convertion_Object.Item_Name;
        Conversion_Material_Count_Text.text = Convertion_Object.Item_Count.ToString();
        {
            Material_Image_1.sprite = Empty_Sprite;
            Material_Name_Text_1.StringReference.SetReference("Station_Table_St", "UI_Inventory_Use_ConvertMaterial_Init_0");
            //Material_Name_Text_1.text = "버릴 재료";
            Material_Count_Text_1.text = "";
        }
        {
            Material_Image_2.sprite = Empty_Sprite;
            Material_Name_Text_2.StringReference.SetReference("Station_Table_St", "UI_Inventory_Use_ConvertMaterial_Init_1");
            //Material_Name_Text_2.text = "바꿀 재료";
            Material_Count_Text_2.text = "";
        }
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
        foreach(Toggle toggle in Material_ToggleGroup[0].GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(ChoiceMaterial_ItemList_ToggleChange_1);
        }
        foreach (Toggle toggle in Material_ToggleGroup[1].GetComponentsInChildren<Toggle>())
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
            for (int i = 0; i < Material_ToggleGroup[0].transform.childCount; i++)
            {
                if (Material_ToggleGroup[0].transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    anyToggleON = true;
                    Material_ToggleNum1 = i;
                    Material_ToggleNum_ItemObject1 = Material_ToggleGroup[0].transform.GetChild(i).GetComponent<ItemUse_Window_53_ToggleObject>().item;
                }
            }

            for(int i = 0; i < Material_ToggleGroup[1].transform.childCount; i++) //어차피 순회 해야됨, 그렇지 않으면 열리지 않음.
            {
                if(i == Material_ToggleNum1)
                {
                    Material_ToggleGroup[1].transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                    Material_LockPanel_2[i].SetActive(true);
                }
                else
                {
                    Material_ToggleGroup[1].transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                    Material_LockPanel_2[i].SetActive(false);
                }
            }
        }

        if (anyToggleON)
        {
            Material_Image_1.sprite = Material_ToggleNum_ItemObject1.Item_Sprite;
            Material_Name_Text_1.StringReference.SetReference("ItemData_Table_St", "Item_Name_"+ Material_ToggleNum_ItemObject1.Num);
            //Material_Name_Text_1.text = Material_ToggleNum_ItemObject1.Item_Name;
            Material_Count_Text_1.text = Material_ToggleNum_ItemObject1.Item_Count.ToString();
            ChoiceMaterial.transform.GetChild(1).GetComponent<Toggle>().interactable = true;
        }
        else
        {
            {
                Material_Image_1.sprite = Empty_Sprite;
                Material_Name_Text_1.StringReference.SetReference("Station_Table_St", "UI_Inventory_Use_ConvertMaterial_Init_0");
                //Material_Name_Text_1.text = "버릴 재료";
                Material_Count_Text_1.text = "";
            }
            {
                Material_Image_2.sprite = Empty_Sprite;
                Material_Name_Text_2.StringReference.SetReference("Station_Table_St", "UI_Inventory_Use_ConvertMaterial_Init_1");
                //Material_Name_Text_2.text = "바꿀 재료";
                Material_Count_Text_2.text = "";
            }

            Material_ToggleNum_ItemObject1 = null;
            Material_ToggleNum_ItemObject2 = null;
            ChoiceMaterial.transform.GetChild(1).GetComponent<Toggle>().interactable = false;
            for (int i = 0; i < Material_ToggleGroup[1].transform.childCount; i++)
            {
                if (Material_ToggleGroup[1].transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    Material_ToggleGroup[1].transform.GetChild(i).GetComponent<Toggle>().isOn = false;
                }
                Material_ToggleGroup[1].transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                Material_LockPanel_2[i].SetActive(false);
            }
        }
        Check_Button();
    }
    private void ChoiceMaterial_ItemList_ToggleChange_2(bool isON)
    {
        if (isON)
        {
            for (int i = 0; i < Material_ToggleGroup[1].transform.childCount; i++)
            {
                if (Material_ToggleGroup[1].transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    Material_ToggleNum_ItemObject2 = Material_ToggleGroup[1].transform.GetChild(i).GetComponent<ItemUse_Window_53_ToggleObject>().item;
                }
            }
            Material_Image_2.sprite = Material_ToggleNum_ItemObject2.Item_Sprite;
            Material_Name_Text_2.StringReference.SetReference("ItemData_Table_St", "Item_Name_"+ Material_ToggleNum_ItemObject2.Num);
            //Material_Name_Text_2.text = Material_ToggleNum_ItemObject2.Item_Name;
            Material_Count_Text_2.text = Material_ToggleNum_ItemObject2.Item_Count.ToString();
        }
        Check_Button();
    }

    private void Check_Button()
    {
        if (Convertion_Object.Item_Count >= RequireItemCount * convertCount)
        {
            if(Material_ToggleNum_ItemObject1 == null)
            {
                ConvertText.StringReference.TableEntryReference = "UI_Inventory_Use_ConvertMaterial_Information_0";
                //ConvertText.text = ("변환에 사용할 재료를 선택하세요");
                Convert_Button.interactable = false;
            }else{
                if(Material_ToggleNum_ItemObject1.Item_Count >= convertCount)
                {
                    if (Material_ToggleNum_ItemObject2 != null)
                    {
                        ConvertText.StringReference.TableEntryReference = "UI_Inventory_Use_ConvertMaterial_Information_1";
                        //ConvertText.text = ("변환 가능합니다!");
                        Convert_Button.interactable = true;
                    }
                    else
                    {
                        ConvertText.StringReference.TableEntryReference = "UI_Inventory_Use_ConvertMaterial_Information_2";
                        //ConvertText.text = ("변환할 재료를 선택하세요");
                        Convert_Button.interactable = false;
                    }
                }
                else
                {
                    ConvertText.StringReference.TableEntryReference = "UI_Inventory_Use_ConvertMaterial_Information_3";
                    //ConvertText.text = ("재료 수가 부족합니다!");
                    Convert_Button.interactable = false;
                }
            }
        }
        else
        {
            ConvertText.StringReference.TableEntryReference = "UI_Inventory_Use_ConvertMaterial_Information_4";
            //ConvertText.text = ("변환 재료가 부족합니다. (변환 재료 10개당 1개 변환 가능)");
            Convert_Button.interactable = false;
        }
    }

    private void Button_CountUP()
    {
        convertCount++;
        CountText.text = convertCount.ToString();

        if (ConvertCountDown.interactable == false)
        {
            ConvertCountDown.interactable = true;
        }
        Check_Button();
    }

    private void Button_CountDown()
    {
        convertCount--;
        CountText.text = convertCount.ToString();

        if (convertCount == 1)
        {
            ConvertCountDown.interactable = false;
        }
        Check_Button();
    }

    private void Button_Convert()
    {
        Convertion_Object.Item_Count_Down(RequireItemCount * convertCount);
        Material_ToggleNum_ItemObject1.Item_Count_Down(convertCount);
        Material_ToggleNum_ItemObject2.Item_Count_UP(convertCount);

        Result_Icon_Img.sprite = Material_ToggleNum_ItemObject2.Item_Sprite;
        Result_Name_Text.StringReference.TableEntryReference = "Item_Name_" + Material_ToggleNum_ItemObject2.Num;
        //Result_Name_Text.text = Material_ToggleNum_ItemObject2.Item_Name;
        Result_Count_Text.text = Material_ToggleNum_ItemObject2.Item_Count.ToString();
        AfterConversionFlag = true;
        AfterConvertWindow.SetActive(true);
        inventory.Check_ItemList(false, Material_ToggleNum_ItemObject1, convertCount);
        inventory.Check_ItemList(true, Material_ToggleNum_ItemObject2, convertCount);
    }

    public void Button_Check()
    {
        Item_53_Init();
        AfterConvertWindow.SetActive(false);
        AfterConversionFlag = false;
    }

    public void Item_53_Init()
    {
        convertCount = 1;
        CountText.text = convertCount.ToString();
        ConvertCountDown.interactable = false;

        BeforeConvertWindow.SetActive(true);
        AfterConvertWindow.SetActive(false);

        foreach(GameObject window in ChoiceMaterial_Window)
        {
            window.SetActive(false);
        }

        for(int i = 0; i < Material_ToggleGroup[0].transform.childCount; i++)
        {
            Material_ToggleGroup[0].transform.GetChild(i).GetComponent<Toggle>().isOn = false;
        }

        for (int i = 0; i < ChoiceMaterial_Window.Length; i++)
        {
            Material_ToggleGroup[i] = ChoiceMaterial_Window[i].GetComponent<ToggleGroup>();
            Material_ToggleGroup[i].transform.GetChild(0).GetComponent<ItemUse_Window_53_ToggleObject>().Check_Count();
            Material_ToggleGroup[i].transform.GetChild(1).GetComponent<ItemUse_Window_53_ToggleObject>().Check_Count();
            Material_ToggleGroup[i].transform.GetChild(2).GetComponent<ItemUse_Window_53_ToggleObject>().Check_Count();
            Material_ToggleGroup[i].transform.GetChild(3).GetComponent<ItemUse_Window_53_ToggleObject>().Check_Count();
        }

        Material_ToggleNum1 = -1;

        Conversion_Material_Count_Text.text = Convertion_Object.Item_Count.ToString();

        {
            Material_Image_1.sprite = Empty_Sprite;
            Material_Name_Text_1.StringReference.SetReference("Station_Table_St", "UI_Inventory_Use_ConvertMaterial_Init_0");
            //Material_Name_Text_1.text = "버릴 재료";
            Material_Count_Text_1.text = "";
        }
        {
            Material_Image_2.sprite = Empty_Sprite;
            Material_Name_Text_2.StringReference.SetReference("Station_Table_St", "UI_Inventory_Use_ConvertMaterial_Init_1");
            //Material_Name_Text_2.text = "바꿀 재료";
            Material_Count_Text_2.text = "";
        }
        Material_ToggleNum_ItemObject1 = null;
        Material_ToggleNum_ItemObject2 = null;
    }
}