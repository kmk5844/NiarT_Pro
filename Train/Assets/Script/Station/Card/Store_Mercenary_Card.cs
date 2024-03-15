using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store_Mercenary_Card : MonoBehaviour
{
    [SerializeField]
    private GameObject MercenaryData_Object;
    Station_MercenaryData mercenaryData;

    public int Mercenary_Num;
    public GameObject Mercenary_Image;
    public GameObject Mercenary_NameText;
    public GameObject Mercenary_Buy;

    private void Start()
    {
        MercenaryData_Object = GameObject.Find("MercenaryData");
        mercenaryData = MercenaryData_Object.GetComponent<Station_MercenaryData>();
        GetComponentInChildren<Toggle>().group = GetComponentInParent<ToggleGroup>();

        Mercenary_Image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Mercenary_" + Mercenary_Num);
        Mercenary_NameText.GetComponent<TextMeshProUGUI>().text = mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Name
            + "\n<size=25>" + mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Num].Mercenary_Pride + "G</size>";
    }
}
