using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station_Maintenance_TrainNum_Button : MonoBehaviour
{
    public GameObject Train_Image;
    public GameObject Train_Check_Image;
    public GameObject TrainNum_Object;
    Image Num_UI_Image;
    public Sprite [] Num_Sprite;

    public void ChangeNumSprite(int Num)
    {
        Num_UI_Image = TrainNum_Object.GetComponent<Image>();
        Num_UI_Image.sprite = Num_Sprite[Num];
        Num_UI_Image.SetNativeSize();
    }

    public void ChekcButton(bool flag)
    {
        if (flag)
        {
            Train_Image.SetActive(false);
            Train_Check_Image.SetActive(true);
        }
        else
        {
            Train_Image.SetActive(true);
            Train_Check_Image.SetActive(false);
        }
    }
}
