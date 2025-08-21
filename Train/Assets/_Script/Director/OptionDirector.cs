using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionDirector : MonoBehaviour
{
    [Header("씬마다 다르게 적용할 필요가 있을 때")]
    public bool StationButtonOFF;

    [Header("UI")]
    public bool isKeyBoardFlag;
    public bool isCreditFlag;

    public GameObject Setting_Menu;
    public GameObject Setting_Sound;
    public GameObject Setting_Screen;

    public GameObject Setting_Menu_StationButton;
    public GameObject Setting_Menu_MainMenuButton;

    public GameObject Check_Stage_Window;
    public GameObject Check_MainMenu_Window;
    public GameObject Check_GameEnd_Window;

    public GameObject KeyBoard_Object;
    public GameObject Credit_Object;

    public Slider BGM_Slider;
    public Slider SFX_Slider;

    [Header("스크린")]
    public TMP_Dropdown resolutionDropdown;
    public int resoutionNum;

    FullScreenMode screenMode;
    List<Resolution> resolutions = new List<Resolution>();

    [Header("언어")]
    public TMP_Dropdown localDropdown;
    LocalManager localManager;

    void Start()
    {
        localManager = GetComponent<LocalManager>();
        InitScreen();
        InitLocal();
        CheckOptionFlag();
        isKeyBoardFlag = false;
        isCreditFlag = false;
        //BGM_Slider.onValueChanged.AddListener(Check_BGM_Audio_Value);
        //SFX_Slider.onValueChanged.AddListener(Check_SFX_Audio_Value);
    }

    private void OnDisable()
    {
        isKeyBoardFlag = false;
        isCreditFlag = false;
        KeyBoard_Object.SetActive(false);
        Credit_Object.SetActive(false);
        if (!StationButtonOFF)
        {
            Click_SettingButton(0);
        }
        CheckWindow_NoButton();
    }

    public void Click_OptionClose()
    {
        this.gameObject.SetActive(false);
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
/*        if (value < 0.00011)
        {
            BGM_Icon_Image.sprite = BGM_Sprite[1];
        }
        else
        {
            BGM_Icon_Image.sprite = BGM_Sprite[0];
        }*/
    }

    public void Check_SFX_Audio_Value(float value)
    {
/*        if (value < 0.00011)
        {
            SFX_Icon_Image.sprite = SFX_Sprite[1];
        }
        else
        {
            SFX_Icon_Image.sprite = SFX_Sprite[0];
        }*/
    }
    //setting 선택
    public void Click_SettingButton(int i)
    {
        if (i == 0)
        {
            Setting_Menu.SetActive(true);
            Setting_Sound.SetActive(false);
            Setting_Screen.SetActive(false);
        }
        else if (i == 1)
        {
            Setting_Menu.SetActive(false);
            Setting_Sound.SetActive(true);
            Setting_Screen.SetActive(false);
        }
        else if (i == 2)
        {
            Setting_Menu.SetActive(false);
            Setting_Sound.SetActive(false);
            Setting_Screen.SetActive(true);
        }
    }


    //Screen구역
    void InitScreen()
    {
        Resolution[] availableResolutions = Screen.resolutions;

        int[,] manualResList = new int[,]
        {
            { 1280, 720 },
            { 1280, 800 },
            { 1440, 900 },
            { 1600, 900 },
            { 1680, 1050 },
            { 1920, 1080 },
            { 1920, 1200 },
            { 2048, 1280 },
            { 2560, 1440 },
            { 2560, 1600 },
            { 2880, 1800 },
            { 3840, 2160 }
        };

        // 수동 해상도 추가 (refresh rate 60으로 고정)
        for (int i = 0; i < manualResList.GetLength(0); i++)
        {
            int width = manualResList[i, 0];
            int height = manualResList[i, 1];

            bool found = false;

            // refreshRateRatio는 Unity 2022 이상에서 사용 가능
            Resolution matched = new Resolution();

            foreach(var r in availableResolutions)
            {
                if(r.width == width && r.height == height && r.refreshRateRatio.numerator >= 59)
                {
                    matched = r;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                if (!resolutions.Exists(r => r.width == matched.width && r.height == matched.height))
                {
                    resolutions.Add(matched);
                }
            }
            else
            {
                // 더 이상 사용 가능한 해상도가 없다면 중단
                break;
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

            //float refreshRate = (float)Window_Screen.refreshRateRatio.numerator / Window_Screen.refreshRateRatio.denominator;
            float refreshRate = 60;
            Window_Option.text = $"{Window_Screen.width} * {Window_Screen.height} {Mathf.RoundToInt(refreshRate)}hz (Windowed)";
            resolutionDropdown.options.Add(Window_Option);
            Full_Option.text = $"{Full_Screen.width} * {Full_Screen.height} {Mathf.RoundToInt(refreshRate)}hz (Full)";
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

    public void DropboxOptionChange_Screen(int x)
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

        Screen.SetResolution(resolutions[resoutionNum / 2].width,
            resolutions[resoutionNum / 2].height,
            screenMode);
    }

    bool Check16To9Ratio(int width, int height)
    {
        float aspectRatio = (float)width / height;
        float targetRatio16_9 = 16.0f / 9.0f;
        float targetRatio16_10 = 16.0f / 10.0f;
        float tolerance = 0.2f;  // 허용 오차 범위 설정
        return Mathf.Abs(aspectRatio - targetRatio16_9) < tolerance || Mathf.Abs(aspectRatio - targetRatio16_10) < tolerance;
    }

    //언어 기본세팅
    void InitLocal()
    {
        localDropdown.options.Clear();

        foreach (string st in localManager.SA_Local.Local_Language)
        {
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = st;
            localDropdown.options.Add(opt);
        }
        localDropdown.value = localManager.index;
    }

    public void DropboxOptionChange_Local(int x)
    {
        localManager.clickOption(x);
    }

    //스테이지 돌아가기
    public void Click_Stage()
    {
        Check_Stage_Window.SetActive(true);
    }

    //메인메뉴로 돌아가기
    public void Click_MainMenu()
    {
        Check_MainMenu_Window.SetActive(true);
    }

    //게임 종료
    public void Click_GameEnd()
    {
        Check_GameEnd_Window.SetActive(true);
    }

    public void CheckWindow_YesButton()
    {
        if (Check_Stage_Window.activeSelf)
        {
            //Debug.Log("스테이지 돌아가기");
        }

        if (Check_MainMenu_Window.activeSelf)
        {
            LoadingManager.LoadScene("1.MainMenu");
        }

        if (Check_GameEnd_Window.activeSelf)
        {
            Application.Quit();
        }
    }

    public void CheckWindow_NoButton()
    {
        if (Check_Stage_Window.activeSelf)
        {
            Check_Stage_Window.SetActive(false);
        }

        if (Check_MainMenu_Window.activeSelf)
        {
            Check_MainMenu_Window.SetActive(false);
        }

        if (Check_GameEnd_Window.activeSelf)
        {
            Check_GameEnd_Window.SetActive(false);
        }
    }

    public void CheckOptionFlag()
    {
        if (StationButtonOFF)
        {
            Setting_Menu_StationButton.SetActive(false);
            Setting_Menu_MainMenuButton.GetComponent<RectTransform>().localPosition = new Vector3(0, -70, 0);
        }
    }
}
