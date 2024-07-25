using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using System;

public class GameManager : MonoBehaviour
{
    #region ½Ì±ÛÅæ
    private static GameManager instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    public bool Demo;
    public SA_PlayerData PlayerData;
    public SA_StoryData StoryData;
    public SA_LocalData LocalData;
    public List<int> Story_Equals_Stage; //ÀÓ½Ã·Î ³öµÐ°Í

    public void Start()
    {
        if (Demo)
        {
            DataManager.Instance.Init();
        }
        else
        {
            DataManager.Instance.Load();
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[LocalData.Local_Index];
    }

    public void Update()
    {
        if (Demo)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                Game_Reset();
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                PlayerData.SA_GameWinReward(999999, 999999);
            }
        }
    }

    public void Start_Enter()
    {
        if (Story_Equals_Stage.Contains(PlayerData.Stage))
        {
            StoryData.Start_Story(PlayerData.Stage);
        }
        else
        {
            StoryData.Start_Story(-1);
        }
    }

    public void End_Enter()
    {
        if (Story_Equals_Stage.Contains(PlayerData.Stage))
        {
            StoryData.End_Story(PlayerData.Stage);
        }
        else
        {
            StoryData.End_Story(-1);
        }
    }

    public void Demo_End_Enter()
    {
        StoryData.End_Demo(PlayerData.Stage);
    }

    public void Game_Reset()
    {
        DataManager.Instance.Init();
        SceneManager.LoadScene(0);
    }
}