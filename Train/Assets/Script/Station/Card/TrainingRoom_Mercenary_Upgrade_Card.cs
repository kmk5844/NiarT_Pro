using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class TrainingRoom_Mercenary_Upgrade_Card : MonoBehaviour
{
    [SerializeField]
    private GameObject MercenaryData_Object;
    Station_MercenaryData mercenaryData;

    public int Mercenary_Num;
    public GameObject Mercenary_Image;
    public LocalizeStringEvent Mercenary_NameText;
    public TextMeshProUGUI Mercenary_LevelText;
    public GameObject Mercenary_Buy;

    private void Awake()
    {
        MercenaryData_Object = GameObject.Find("MercenaryData");
        mercenaryData= MercenaryData_Object.GetComponent<Station_MercenaryData>();
        GetComponentInChildren<Toggle>().group = GetComponentInParent<ToggleGroup>();

        Mercenary_Image.GetComponent<Image>().sprite =  mercenaryData.SA_MercenaryData.Mercenary_Head_Image[Mercenary_Num];
        Mercenary_NameText.StringReference.TableReference = "ExcelData_Table_St";

        Mercenary_NameText.StringReference.TableEntryReference = "Mercenary_Name_" + Mercenary_Num;
        Mercenary_LevelText.text = "Lv." + mercenaryData.Mercenary_Find_Level(Mercenary_Num).ToString();
    }

    public void Card_LevleUP()
    {
        Mercenary_LevelText.text = "Lv." + mercenaryData.Mercenary_Find_Level(Mercenary_Num).ToString();
    }
}
