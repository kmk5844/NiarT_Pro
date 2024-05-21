using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingRoom_Mercenary_Upgrade_Card : MonoBehaviour
{
    [SerializeField]
    private GameObject MercenaryData_Object;
    Station_MercenaryData mercenaryData;

    public int Mercenary_Num;
    public GameObject Mercenary_Image;
    public GameObject Mercenary_NameText;
    public GameObject Mercenary_Buy;

    private void Awake()
    {
        MercenaryData_Object = GameObject.Find("MercenaryData");
        mercenaryData= MercenaryData_Object.GetComponent<Station_MercenaryData>();
        GetComponentInChildren<Toggle>().group = GetComponentInParent<ToggleGroup>();

        Mercenary_Image.GetComponent<Image>().sprite =  mercenaryData.SA_MercenaryData.Mercenary_Head_Image[Mercenary_Num];
        Mercenary_NameText.GetComponent<TextMeshProUGUI>().text = "<size=15>" + mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Name
            + "</size>\n<size=30>Lv." + mercenaryData.Mercenary_Find_Level(Mercenary_Num);
    }

    public void Card_LevleUP()
    {
        Mercenary_NameText.GetComponent<TextMeshProUGUI>().text = "<size=15>" + mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Name
            + "</size>\n<size=35>Lv." + mercenaryData.Mercenary_Find_Level(Mercenary_Num);
    }
}
