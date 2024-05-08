using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mercenary_On_Board_Card : MonoBehaviour
{
    public int Mercenary_Num;
    public Transform Image_Y;
    public Transform Image_Z;
    public Image Mercenary_BackGround;
    public Image Mercenary_Image;
    public Sprite[] Mercenary_BackGround_Image;
    public Sprite[] Mercenary_Face_Image;

    private void Start()
    {
        Image_Y.localPosition = new Vector2(0, Random.Range(-14, 14f));
        Image_Z.Rotate(0, 0, Random.Range(-12f, 12f));

        Mercenary_BackGround.sprite = Mercenary_BackGround_Image[Random.Range(0, Mercenary_BackGround_Image.Length)];
        Mercenary_Image.sprite = Mercenary_Face_Image[Mercenary_Num];
    }
}
