using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using System;

[Serializable]
public class storyStage
{
    public int stageNum;
    public bool StartFlag;
    public bool EndFlag;
}

public class GameManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
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
    public Game_DataTable gameData;
    public Story_DataTable storyData;

    public SA_PlayerData PlayerData;
    public SA_LocalData LocalData;
    public List<storyStage> story_List;

    Texture2D cursorOrigin;
    Vector2 cursorHotspot_Origin;

    public void Start()
    {
        cursorOrigin = Resources.Load<Texture2D>("Cursor/Origin6464");
        cursorHotspot_Origin = Vector2.zero;
        Cursor.SetCursor(cursorOrigin, cursorHotspot_Origin, CursorMode.Auto);
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
                PlayerData.SA_Test();
            }
        }
    }

    public void BeforeGameStart_Enter()
    {
        int index = story_List.FindIndex(x => x.stageNum == PlayerData.New_Stage);
        if (index != -1 && !story_List[index].StartFlag)
        {
            LoadingManager.LoadScene(gameData.Information_Scene[index].BeforeGameStart_Button);
            story_List[index].StartFlag = true;
        }
        else
        {
            LoadingManager.LoadScene("InGame");
        }
    }

    public void BeforeStation_Enter()
    {
        int index = story_List.FindIndex(x => x.stageNum == PlayerData.New_Stage);
        if (index != -1 && !story_List[index].StartFlag)
        {
            LoadingManager.LoadScene(gameData.Information_Scene[index].BeforeStation_Button);
            story_List[index].StartFlag = true;
        }
        else
        {
            LoadingManager.LoadScene("Station");
        }
    }

    public void Story_End()
    {
        int index = story_List.FindIndex(x => x.stageNum == PlayerData.New_Stage);
        if (index != -1 && !story_List[index].EndFlag)
        {
            LoadingManager.LoadScene(gameData.Information_Scene[index].Story_End);
            story_List[index].EndFlag = true;
        }
        else
        {
            LoadingManager.LoadScene("Station");
        }
    }

    public void Demo_End_Enter()
    {
        LoadingManager.LoadScene("Demo_End");
    }

    void StoryFlag_Init()
    {
        for(int i = 0; i < story_List.Count; i++)
        {
            story_List[i].StartFlag = false;
            story_List[i].EndFlag = false;
        }
    }

    public void Game_Reset()
    {
        DataManager.Instance.Init();
        StoryFlag_Init();
        SceneManager.LoadScene(0);
    }
}