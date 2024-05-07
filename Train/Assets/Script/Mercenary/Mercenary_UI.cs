using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mercenary_UI : MonoBehaviour
{
    public GameObject MercenaryObject;
    Mercenary_Type Check_Type;
    public int Mercenary_Num;

    public Image Mercenary_Image;
    public Sprite Engineer_Sprite;
    public Slider Mercenary_HP_Slider;
    public Slider Mercenary_Stamina_Slider;
    public Image Face_Image;
    public Sprite[] Mercenary_Face_Image;

    private void Start()
    {
        Check_Type = MercenaryObject.GetComponent<Mercenary_Type>();
        if(Mercenary_Num == 0 || Mercenary_Num == 1)
        {
            Mercenary_Image.sprite = Engineer_Sprite;
        }
        Face_Image.sprite = Mercenary_Face_Image[Mercenary_Num];
    }

    private void Update()
    {
        Mercenary_HP_Slider.value = Check_Type.medic_checkHpParsent / 100f;
        Mercenary_Stamina_Slider.value = Check_Type.medic_checkStaminaParsent / 100f;
    }
}
