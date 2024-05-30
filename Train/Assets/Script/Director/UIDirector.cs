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

    //Speed���� ����
    float minRotation;
    float maxRotation;

    float minSpeed;
    float maxSpeed;

    [Header("��ü���� UI ������Ʈ")]
    public GameObject Game_UI;
    public GameObject Pause_UI;
    public GameObject Win_UI;
    public GameObject Lose_UI;
    public GameObject Option_UI;

    [Header("Game UI")]
    public Slider Player_HP_Bar;
    public Slider Distance_Bar;
    public Slider TotalFuel_Bar;
    public TextMeshProUGUI Speed_Text;
    public Transform Speed_Arrow;
    public Transform Team_1;
    public Transform Team_2;
    public GameObject Panel;
    int Team_Index1;
    int Team_Index2;

    [Header("Win UI ���õ� �ؽ�Ʈ")]
    public TextMeshProUGUI Win_Stage_Text;
    public TextMeshProUGUI Win_Score_Text;
    public Image Win_Score_Grade_Image;
    public TextMeshProUGUI Win_Reward_Coin_Text;
    public TextMeshProUGUI Win_Reward_Point_Text;

    [Header("Lose UI ���õ� �ؽ�Ʈ")]
    public TextMeshProUGUI Lose_Reward_Coin_Text;

    bool PauseFlag;
    bool OptionFlag;


    [Header("������������� ��ư �߰�")]
    public Button Retry_Button;
    public Button Station_Button;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gamedirector = GameDirector_Object.GetComponent<GameDirector>();
        mercenarydirector = MercenaryDirector_Object.GetComponent<MercenaryDirector>();

        Team_Index1 = 2;
        Team_Index2 = 0;

        minRotation = 115f;
        maxRotation = -105f;

        minSpeed = 0f;
        maxSpeed = gamedirector.MaxSpeed + 30;

        PauseFlag = false;
        OptionFlag = false;

        if (!mercenarydirector.Team_Flag)
        {
            if(mercenarydirector.Mercenary_Num.Count == 0)
            {
                Team_2.gameObject.SetActive(false);
            }
            Team_1.gameObject.SetActive(false);
            Panel.gameObject.SetActive(false);
        }
        DemoCheck(); // ���߿� ���� ���濹��
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


        if (Input.GetKeyDown(KeyCode.Tab) && mercenarydirector.Team_Flag)
        {
            Vector2 temp = Team_2.transform.position;
            Team_2.transform.position = Team_1.transform.position;
            Team_1.transform.position = temp;
            Change_Team();
        }

        Player_HP_Bar.value = player.Check_HpParsent() / 100f;
        Distance_Bar.value = gamedirector.Check_Distance();
        TotalFuel_Bar.value = gamedirector.Check_Fuel();
        Speed_Text.text = gamedirector.TrainSpeed + "Km/h";
        Speed_Arrow.rotation = Quaternion.Euler(0f, 0f, SpeedToRotation(gamedirector.TrainSpeed));
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

    public void Change_Team()
    {
        if(Team_Index1 == 0 && Team_Index2 == 2)
        {
            Team_1.SetSiblingIndex(Team_Index2);
            Team_2.SetSiblingIndex(Team_Index1);
            Team_Index1 = 2;
            Team_Index2 = 0;
        }
        else if(Team_Index1 == 2 && Team_Index2 == 0)
        {
            Team_1.SetSiblingIndex(Team_Index2);
            Team_2.SetSiblingIndex(Team_Index1);
            Team_Index1 = 0;
            Team_Index2 = 2;
        }
    }
    private float SpeedToRotation(float speed)
    {
        return (speed - minSpeed) * (maxRotation - minRotation) / (maxSpeed - minSpeed) + minRotation;
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