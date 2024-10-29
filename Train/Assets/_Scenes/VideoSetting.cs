using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoSetting : MonoBehaviour
{
    FullScreenMode screenMode;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();
    public int resoutionNum;


    void Start()
    {
        InitUI();
    }

    //Screen±¸¿ª
    void InitUI()
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


        //resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + " * " + item.height + " " +
                Mathf.CeilToInt(item.refreshRateRatio.numerator) + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }

        resolutionDropdown.RefreshShownValue();

        //fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resoutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resoutionNum].width,
            resolutions[resoutionNum].height,
            screenMode);
    }

    bool CheckMinimumResolution(int width)
    {
        if (width >= 1024)
        {
            return true;
        }

        return false;
    }

    bool Check16To9Ratio(int width, int height)
    {
        float aspectRatio = (float)width / height;
        return Mathf.Approximately(aspectRatio, 16.0f / 9.0f);
    }

}