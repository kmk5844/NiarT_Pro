using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mercenary_UI : MonoBehaviour
{
    public GameObject MercenaryObject;
    Mercenary_Type Check_Type;

    public Image Mercenary_Image;
    public Slider Mercenary_HP_Slider;
    public Slider Mercenary_Stamina_Slider;

    private void Start()
    {
        Check_Type = MercenaryObject.GetComponent<Mercenary_Type>();
    }

    private void Update()
    {
        Mercenary_HP_Slider.value = Check_Type.medic_checkHpParsent / 100f;
        Mercenary_Stamina_Slider.value = Check_Type.medic_checkStaminaParsent / 100f;
    }
}
