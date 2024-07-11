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
    public GameObject Result_UI;
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


    [Header("Result UI 관련된 텍스트")]
    public TextMeshProUGUI[] Result_Text_List; //0. Stage, 1. Score, 2. Gold, 3. Rank 4. Point
    public Image Result_Image; //win or lose
    public Sprite Result_Win_Image;
    public Sprite Result_Lose_Image;

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


        //DemoCheck(); // 나중에 데모 변경예정
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

    public void Open_Result_UI(bool Win, int StageNum, int Score, int Coin, string Score_Grade,int Point)
    {
        Result_Text_List[0].text = "스테이지 " + (StageNum + 1);
        Result_Text_List[1].text = "점수 : " + Score + "점";
        Result_Text_List[2].text = "획득 골드 : " + Coin + "원";
        if (Win)
        {
            Result_Text_List[3].text = "랭크 : " + Score_Grade;
            Result_Text_List[4].text = "플레이어 포인트 : " + Point;
            Result_Text_List[4].gameObject.SetActive(true);
            Result_Image.sprite = Result_Win_Image;
        }
        else
        {
            Result_Text_List[3].text = "랭크 : F";
            Result_Text_List[4].gameObject.SetActive(false);
            Result_Image.sprite = Result_Lose_Image;
        }
        Game_UI.SetActive(false);
        Result_UI.SetActive(true);
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