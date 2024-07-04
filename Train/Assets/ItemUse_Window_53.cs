using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUse_Window_53 : MonoBehaviour
{
    public ItemDataObject[] MaterialObject;

    public GameObject BeforeConvertWindow;
    public GameObject AfterConvertWindow;

    public ToggleGroup ChoiceMaterial;
    public GameObject[] ChoiceMaterial_Window;

    public ToggleGroup Material_ToggleGroup_1;
    public ToggleGroup Material_ToggleGroup_2;
    [SerializeField]
    ItemDataObject Material_ToggleNum_ItemObject1;
    [SerializeField]
    ItemDataObject Material_ToggleNum_ItemObject2;
    [SerializeField]
    int Material_ToggleNum1;
    [SerializeField]
    int Material_ToggleNum2;

    private void Start()
    {
        ChoiceMaterial_ToggleStart();
        ChoiceMaterial_ItemList_ToggleStart();
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
            for(int i = 0; i < ChoiceMaterial.transform.childCount; i++)
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
        if (isON)
        {
            for(int i = 0; i < Material_ToggleGroup_1.transform.childCount; i++)
            {
                if (Material_ToggleGroup_1.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    Material_ToggleNum1 = i;
                    Material_ToggleNum_ItemObject1 = Material_ToggleGroup_1.transform.GetChild(i).GetComponent<ItemUse_Window_53_ToggleObject>().item;
                }
            }

            for(int i = 0; i < Material_ToggleGroup_2.transform.childCount; i++)
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
    }
    private void ChoiceMaterial_ItemList_ToggleChange_2(bool isON)
    {
        if (isON)
        {
            for (int i = 0; i < Material_ToggleGroup_2.transform.childCount; i++)
            {
                if (Material_ToggleGroup_2.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    Material_ToggleNum2 = i;
                    Material_ToggleNum_ItemObject2 = Material_ToggleGroup_2.transform.GetChild(i).GetComponent<ItemUse_Window_53_ToggleObject>().item;
                }
            }
        }
    }
}
