using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [Header("스크린")]
    public TMP_Dropdown resolutionDropdown;

    public Sprite[] DropDown_Sprite;
    Image DropDown_Image;

    FullScreenMode screenMode;
    List<Resolution> resolutions = new List<Resolution>();
    public int resoutionNum;

    void Start()
    {
        DropDown_Image = resolutionDropdown.GetComponent<Image>();

        InitScreen();
        isKeyBoardFlag = false;
        isCreditFlag = false;
        BGM_Slider.onValueChanged.AddListener(Check_BGM_Audio_Value);
        SFX_Slider.onValueChanged.AddListener(Check_SFX_Audio_Value);
    }

    private void Update()
    {
        if (resolutionDropdown.transform.childCount > 3)
        {
            DropDown_Image.sprite = DropDown_Sprite[1];
        }
        else
        {
            DropDown_Image.sprite = DropDown_Sprite[0];
        }
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
        if (value < 0.00011)
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

    //Screen구역
    void InitScreen()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (CheckMinimumResolution(Screen.resolutions[i].width)
                && Check16To9Ratio(Screen.resolutions[i].width, Screen.resolutions[i].height))
            {
                if (Screen.resolutions[i].refreshRateRatio.numerator == 60)
                {
                    resolutions.Add(Screen.resolutions[i]);
                }
            }
        }

        resolutionDropdown.options.Clear();

        int optionNum = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            Resolution Window_Screen = resolutions[i];
            Resolution Full_Screen = resolutions[i];

            TMP_Dropdown.OptionData Window_Option = new TMP_Dropdown.OptionData();
            TMP_Dropdown.OptionData Full_Option = new TMP_Dropdown.OptionData();
            Window_Option.text = Window_Screen.width + " * " + Window_Screen.height + " " +
              Mathf.CeilToInt(Window_Screen.refreshRateRatio.numerator) + "hz(Windowed)";
            Full_Option.text = Full_Screen.width + " * " + Full_Screen.height + " " +
              Mathf.CeilToInt(Full_Screen.refreshRateRatio.numerator) + "hz(Full)";
            resolutionDropdown.options.Add(Window_Option);
            resolutionDropdown.options.Add(Full_Option);

            if (Window_Screen.width == Screen.width && Window_Screen.height == Screen.height)
            {
                if (!Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow))
                {
                    resolutionDropdown.value = optionNum;
                }
            }
            optionNum++;
            if (Full_Screen.width == Screen.width && Full_Screen.height == Screen.height)
            {
                if (Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow))
                {
                    resolutionDropdown.value = optionNum;
                }
            }
            optionNum++;
        }

        resolutionDropdown.RefreshShownValue();
    }

    public void DropboxOptionChange(int x)
    {
        resoutionNum = x;
        if (x % 2 == 0)
        {
            screenMode = FullScreenMode.Windowed;
        }
        else if (x % 2 == 1)
        {
            screenMode = FullScreenMode.FullScreenWindow;
        }

        DropDown_Image.sprite = DropDown_Sprite[0];

        Screen.SetResolution(resolutions[resoutionNum / 2].width,
            resolutions[resoutionNum / 2].height,
            screenMode);
    }

    bool CheckMinimumResolution(int width)
    {
        if (width >= 1280)
        {
            return true;
        }

        return false;
    }

    bool Check16To9Ratio(int width, int height)
    {
        float aspectRatio = (float)width / height;
        float targetRatio = 16.0f / 9.0f;
        float tolerance = 0.19f;  // 허용 오차 범위 설정
        return Mathf.Abs(aspectRatio - targetRatio) < tolerance;
    }
}
