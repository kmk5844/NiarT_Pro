using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionDirector : MonoBehaviour
{
    public bool isKeyBoardFlag;
    public bool isCreditFlag;

    public GameObject KeyBoard_Object;
    public GameObject Credit_Object;

    public Slider BGM_Slider;
    public Slider SFX_Slider;
    public Image BGM_Icon_Image;
    public Image SFX_Icon_Image;
    public Sprite[] BGM_Sprite;
    public Sprite[] SFX_Sprite;

    void Start()
    {
        isKeyBoardFlag = false;
        isCreditFlag = false;
        BGM_Slider.onValueChanged.AddListener(Check_BGM_Audio_Value);
        SFX_Slider.onValueChanged.AddListener(Check_SFX_Audio_Value);
    }
    private void OnDisable()
    {
        isKeyBoardFlag = false;
        isCreditFlag = false;
        KeyBoard_Object.SetActive(false);
        Credit_Object.SetActive(false);
    }

    public void Click_OpenKeyBoard()
    {
        isKeyBoardFlag = true;
        KeyBoard_Object.SetActive(true);
    }

    public void Click_OpenCredit()
    {
        isCreditFlag = true;
        Credit_Object.SetActive(true);
    }

    public void Click_CloseKeyBoard()
    {
        isKeyBoardFlag = false;
        KeyBoard_Object.SetActive(false);
    }

    public void Click_CloseCredit()
    {
        isCreditFlag = false;
        Credit_Object.SetActive(false);
    }

    public void Check_BGM_Audio_Value(float value)
    {
        if(value < 0.00011)
        {
            BGM_Icon_Image.sprite = BGM_Sprite[1];
        }
        else
        {
            BGM_Icon_Image.sprite = BGM_Sprite[0];
        }
    }

    public void Check_SFX_Audio_Value(float value)
    {
        if (value < 0.00011)
        {
            SFX_Icon_Image.sprite = SFX_Sprite[1];
        }
        else
        {
            SFX_Icon_Image.sprite = SFX_Sprite[0];
        }
    }
}
