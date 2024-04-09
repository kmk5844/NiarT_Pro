using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDirector : MonoBehaviour
{
    public GameObject GameDirector_Object;
    GameDirector gamedirector;

    [Header("전체적인 UI 오브젝트")]
    public GameObject Game_UI;
    public GameObject Pause_UI;
    public GameObject Win_UI;
    public GameObject Lose_UI;
    public GameObject Option_UI;

    [Header("Win UI 관련된 텍스트")]
    public TextMeshProUGUI Win_Stage_Text;
    public TextMeshProUGUI Win_Score_Text;
    public TextMeshProUGUI Win_Reward_Coin_Text;
    public TextMeshProUGUI Win_Reward_Point_Text;

    [Header("Lose UI 관련된 텍스트")]
    public TextMeshProUGUI Lose_Reward_Coin_Text;

    bool PauseFlag;
    bool OptionFlag;

    private void Start()
    {
        PauseFlag = false;
        OptionFlag = false;
        gamedirector = GameDirector_Object.GetComponent<GameDirector>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!OptionFlag)
            {
                ON_OFF_Pause_UI(PauseFlag);
            }
            else
            {
                Click_Option_Exit();
            }
        }
    }

    public void Open_WIN_UI()
    {
        Game_UI.SetActive(false);
        Win_UI.SetActive(true);
    }

    public void Open_Lose_UI()
    {
        Game_UI.SetActive(false);
        Lose_UI.SetActive(true);
    }

    public void ON_OFF_Pause_UI(bool Flag)
    {
        if (Flag)
        {
            PauseFlag = false;
            Pause_UI.SetActive(false);
        }
        else
        {
            PauseFlag = true;
            Pause_UI.SetActive(true);
        }
    }

    public void Win_Text(int StageNum, string StageName, int Score, int Coin, int Point)
    {
        Win_Stage_Text.text = "Stage" + StageNum + " : " + StageName;
        Win_Score_Text.text = "Total Score : " + Score;
        Win_Reward_Coin_Text.text = "Reward Coin : " + Coin;
        Win_Reward_Point_Text.text = "Reward : Point : " + Point;
    }

    public void Lose_Text(int Coin)
    {
        Lose_Reward_Coin_Text.text = "Reward Coin : " + Coin;
    }

    public void Clcik_Pause()
    {
        gamedirector.PauseButton();
        ON_OFF_Pause_UI(false);
    }

    public void Click_Option()
    {
        gamedirector.GameType_Option(true);
        OptionFlag = true;
        Option_UI.SetActive(true);
    }

    public void Click_Station()
    {
        SceneManager.LoadScene("Station");
    }

    public void Click_Retry()
    {
        SceneManager.LoadScene("InGame");
    }

    public void Click_MainMenu()
    {
        Debug.Log("메인메뉴 이동");
    }

    public void Click_GameExit()
    {
        Application.Quit();
    }

    public void Click_Option_Exit()
    {
        gamedirector.GameType_Option(false);
        OptionFlag = false;
        Option_UI.SetActive(false);
    }

    public void Clcik_Pause_Exit()
    {
        gamedirector.PauseButton();
        ON_OFF_Pause_UI(true);
    }
}
