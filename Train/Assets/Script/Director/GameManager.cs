using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
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

    public SA_PlayerData PlayerData;
    public SA_StoryData StoryData;
    public List<int> Story_Equals_Stage; //임시로 놔둔것

    public void Start()
    {
        DataManager.Instance.Init();
        //데모버전에서 적용 / 정식에서는  Load();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlayerData.SA_GameWinReward(9999, 9999);
        }
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            Game_Reset();
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