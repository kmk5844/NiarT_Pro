using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDirector : MonoBehaviour
{
    public GameObject GameDirector_Object;
    GameDirector gamedirector;
    public GameObject MercenaryDirector_Object;
    MercenaryDirector mercenarydirector;
    Player player;

    [Header("전체적인 UI 오브젝트")]
    public GameObject Game_UI;
    public GameObject Pause_UI;
    public GameObject Win_UI;
    public GameObject Lose_UI;
    public GameObject Option_UI;

    [Header("Game UI")]
    public Image Player_HP_Bar;
    public Slider Distance_Bar;
    public Image TotalFuel_Bar;
    public TextMeshProUGUI Speed_Text;
    public TextMeshProUGUI Fuel_Text;
    public TextMeshProUGUI Score_Text;
    public TextMeshProUGUI Coin_Text;
    public Slider Speed_Arrow;


    [Header("Win UI 관련된 텍스트")]
    public TextMeshProUGUI Win_Stage_Text;
    public TextMeshProUGUI Win_Score_Text;
    public Image Win_Score_Grade_Image;
    public TextMeshProUGUI Win_Reward_Coin_Text;
    public TextMeshProUGUI Win_Reward_Point_Text;

    [Header("Lose UI 관련된 텍스트")]
    public TextMeshProUGUI Lose_Reward_Coin_Text;

    bool PauseFlag;
    bool OptionFlag;


    [Header("데모버전에서만 버튼 추가")]
    public Button Retry_Button;
    public Button Station_Button;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gamedirector = GameDirector_Object.GetComponent<GameDirector>();
        mercenarydirector = MercenaryDirector_Object.GetComponent<MercenaryDirector>();

        PauseFlag = false;
        OptionFlag = false;


        DemoCheck(); // 나중에 데모 변경예정
    }

    private void Update()
    {
        if(gamedirector.gameType == GameType.Ending || gamedirector.gameType == GameType.GameEnd)
        {

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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

        Player_HP_Bar.fillAmount = player.Check_HpParsent() / 100f;
        TotalFuel_Bar.fillAmount = gamedirector.Check_Fuel();
        Speed_Text.text = (int)gamedirector.TrainSpeed + " Km/h";
        Fuel_Text.text = "Fuel : "+ (int)(gamedirector.Check_Fuel() * 100f) + "%";
        Speed_Arrow.value = gamedirector.TrainSpeed / gamedirector.MaxSpeed;

        Distance_Bar.value = gamedirector.Check_Distance();
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

    public void Gameing_Text(int Score, int Coin)
    {
        Score_Text.text = "Score : " + Score;
        Coin_Text.text = "Coin : " + Coin;
    }

    public void Win_Text(int StageNum, int Score, string Score_Grade,int Coin, int Point)
    {
        Win_Stage_Text.text = "Stage" + (StageNum +1);
        Win_Score_Text.text = "Total Score : " + Score;
        Win_Score_Grade_Image.sprite = Resources.Load<Sprite>("InGame_UI/Grade/" + Score_Grade);
        Win_Reward_Coin_Text.text = "Reward Coin : " + Coin;
        Win_Reward_Point_Text.text = "Reward : Point : " + Point;
    }

    public void Lose_Text(int Coin)
    {
        Lose_Reward_Coin_Text.text = "Reward Coin : " + Coin;
    }

    public void Click_Station()
    {
        GameManager.Instance.Start_Enter();
        //LoadingManager.LoadScene("Station");
    }

    public void Demo_Station()
    {
        GameManager.Instance.Demo_End_Enter();
    }
    public void Click_Retry()
    {
        LoadingManager.LoadScene("InGame");
    }

    public void Click_MainMenu()
    {
        LoadingManager.LoadScene("1.MainMenu");
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

    private void DemoCheck()
    {
        if (gamedirector.Stage_Num == 5)
        {
            Retry_Button.interactable = false;
            Station_Button.onClick.AddListener(() => Demo_Station());
        }
        else
        {
            Station_Button.onClick.AddListener(() => Click_Station());
        }
    }
}