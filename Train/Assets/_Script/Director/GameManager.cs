using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using System;
using static PixelCrushers.AnimatorSaver;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region �̱���
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
    public SA_StoryLIst StoryData;
    public SA_MissionData MissionData;
    public Quest_DataTable QuestData;
    public GameObject SelectMissionObject;

    Texture2D cursorOrigin;
    Vector2 cursorHotspot_Origin;

    Coroutine DataCoroutine;

    public void Start()
    {
        // V-Sync�� ��Ȱ��ȭ�Ͽ� FPS ������ ����
        QualitySettings.vSyncCount = 0;
        // ������ ����Ʈ�� 60���� ����
        Application.targetFrameRate = 60;

        cursorOrigin = Resources.Load<Texture2D>("Cursor/Origin6464");
        cursorHotspot_Origin = Vector2.zero;
        Cursor.SetCursor(cursorOrigin, cursorHotspot_Origin, CursorMode.Auto);

        DataLoad();

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[LocalData.Local_Index];
        if (PlayerData.Mission_Playing)
        {
            SelectMissionObject.GetComponent<SelectMission>().SetDataSetting(PlayerData, QuestData, MissionData);
            GameObject gm = Instantiate(SelectMissionObject);
            gm.name = "SelectMission";
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }

            if (GameObject.Find("SelectMission"))
            {
                GameObject gm = GameObject.Find("SelectMission");
                Destroy(gm);
            }
            Game_DataReset();
        }
    }

    public void DataLoad()
    {
        if (!ES3.KeyExists("SA_PlayerData_Data_FirstFlag"))
        {
            Game_DataReset();
            //��ǻ�� ������ ����;
        }
        else
        {
            //DataManager.Instance.Load();
            try
            {
                Game_DataLoad();
            }
            catch
            {
                Game_DataReset();
                //��ǻ�� �����ʹ� ������ ���� ;
            }
        }
    }

    public void BeforeGameStart_Enter()
    {
        if (gameData.Information_Scene[PlayerData.New_Stage].BeforeGameStart_Button.Equals("Story"))
        {
            int index = gameData.Information_Scene[PlayerData.New_Stage].BeforeGameStart_StoryIndex;
            if (!StoryData.StoryList[index].Start_Flag)
            {
                LoadingManager.LoadScene("Story");
                StoryData.StoryList[index].ChangeFlag(true);
            }
            else
            {
                //LoadingManager.LoadScene("CharacterSelect");
                LoadingManager.LoadScene("MissionSelect");
            }
        }
        else
        {
            //LoadingManager.LoadScene("CharacterSelect");
            LoadingManager.LoadScene("MissionSelect");
        }

        /*        int index = StoryData.StoryList.FindIndex(x => x.Story_Num == PlayerData.New_Stage);
                if (index != -1 && !StoryData.StoryList[index].Start_Flag)
                {
                    //PlayerData.SA_Story(gameData.Information_Scene[index].BeforeGameStart_StoryIndex);
                    LoadingManager.LoadScene(gameData.Information_Scene[index].BeforeGameStart_Button);
                }
                else
                {
                    LoadingManager.LoadScene("InGame");
                }*/
    }

    public void BeforeStation_Enter()
    {
        if (gameData.Information_Scene[PlayerData.New_Stage].BeforeStation_Button.Equals("Story"))
        {
            int index = gameData.Information_Scene[PlayerData.New_Stage].BeforeStation_StoryIndex;
            Debug.Log(index);
            if (!StoryData.StoryList[index].Start_Flag) // ���丮 ���� ��
            {
                LoadingManager.LoadScene("Story");
                StoryData.StoryList[index].ChangeFlag(true);
            }
            else
            {
                if (!StoryData.StoryList[index].End_Flag)
                {
                    LoadingManager.LoadScene("Story");
                }
                else
                {
                    LoadingManager.LoadScene("MissionSelect");
                }
            }
        }
        else if (gameData.Information_Scene[PlayerData.New_Stage].BeforeStation_Button.Equals("CutScene"))
        {
            int index = gameData.Information_Scene[PlayerData.New_Stage].BeforeStation_StoryIndex;

            if (!DataManager.Instance.playerData.FirstFlag) //Ʃ�丮�� ���� ��.
            {
                if (!StoryData.StoryList[index].Start_Flag) // ���丮 ���� ��,
                {
                    LoadingManager.LoadScene("CutScene");
                }
                else // ���丮 ���� ��,
                {
                    if (!StoryData.StoryList[index].End_Flag)
                    {
                        LoadingManager.LoadScene("Story");
                    }
                    else
                    {
                        LoadingManager.LoadScene("GamePlay_Tutorial");
                    }
                }
            }
            else // Ʃ�丮�� ���� ��,
            {
                if (!PlayerData.Mission_Playing)
                {
                    LoadingManager.LoadScene("Station");
                }
                else
                {
                    LoadingManager.LoadScene("MissionSelect");
                }
            }
        }
        else if (gameData.Information_Scene[PlayerData.New_Stage].BeforeStation_Button.Equals("Demo_End"))
        {
            LoadingManager.LoadScene("Demo_End");
        }
        else
        {
            if (!PlayerData.Mission_Playing)
            {
                LoadingManager.LoadScene("Station");
            }
            else
            {
                LoadingManager.LoadScene("MissionSelect");
            }
        }
        /*        int index = StoryData.StoryList.FindIndex(x => x.Story_Num == PlayerData.New_Stage);
                if (index != -1 && !StoryData.StoryList[index].Start_Flag)
                {
                    //PlayerData.SA_Story(gameData.Information_Scene[index].BeforeStation_StoryIndex);
                    LoadingManager.LoadScene(gameData.Information_Scene[index].BeforeStation_Button);
                    StoryData.StoryList[index].ChangeFlag(true);
                }
                else
                {
                    LoadingManager.LoadScene("Station");
                }*/
    }

    public void Story_End()
    {
        int index = StoryData.StoryList.FindIndex(x => x.Story_Num == PlayerData.Story_Num);
        if (index != -1 && !StoryData.StoryList[index].End_Flag)
        {
            LoadingManager.LoadScene(storyData.Story_Branch[index].Story_End);
            PlayerData.SA_StoryEnd();
            StoryData.StoryList[index].ChangeFlag(false);
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
        for(int i = 0; i < StoryData.StoryList.Count; i++)
        {
            StoryData.StoryList[i].Init();
        }
    }

    public void Game_DataLoad()
    {
         StartCoroutine(LoadGameRoutine());
    }

    public void Game_DataReset()
    {
        SceneManager.LoadScene("LoadingScene_Data");
        StartCoroutine(ResetGameRoutine());
/*        DataManager.Instance.Init();
        StoryFlag_Init();
        SceneManager.LoadScene(0);*/
    }

    private IEnumerator LoadGameRoutine()
    {
        /*SceneManager.LoadScene("LoadingScene_Data");
        yield return new WaitForSeconds(1f);*/

        // ������ �ʱ�ȭ
        yield return StartCoroutine(DataManager.Instance.LoadSync());
        StoryFlag_Init();
        // ���� �ð� ��� (��: 1��)
        yield return new WaitForSeconds(1.0f);

        // �񵿱� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("0.Company");
        while (!asyncLoad.isDone)
        {
            yield return null; // �ε��� �Ϸ�� ������ ���
        }
    }

    private IEnumerator ResetGameRoutine()
    {
        // �ε� ���� ���� �ε�
        /*SceneManager.LoadScene("LoadingScene_Data");
        yield return new WaitForSeconds(1f);*/

        // ������ �ʱ�ȭ
        yield return StartCoroutine(DataManager.Instance.InitAsync());
        StoryFlag_Init();
        // ���� �ð� ��� (��: 1��)
        yield return new WaitForSeconds(1.0f);

        // �񵿱� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("0.Company");
        while (!asyncLoad.isDone)
        {
            yield return null; // �ε��� �Ϸ�� ������ ���
        }
    }
}