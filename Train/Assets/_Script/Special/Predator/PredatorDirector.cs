using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PredatorDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject PredatorWindow;
    public GameObject SelectStage;
    public GameObject ResultWindow_S;
    public GameObject ResultWindow_F;

    [Header("Data")]
    public SA_PlayerData playerData;
    public float MaxTime;
    private float elapsedTime = 0f;
    private int displayTime = 0;
    int count = 0;
    int MaxCount;
    bool TimeFlag = false;
    bool playFlag = false;
    int ChanceCount = 0;
    int MaxChanceCount = 2;
    bool startFlag;

    [Header("UI")]
    public Transform IconList;
    public PredatorIcon Icon;
    public Slider slider;
    public TextMeshProUGUI TimeText;
    public Transform ChanceList;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        PredatorWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }

        MaxCount = Random.Range(30, 51);
        elapsedTime = MaxTime;
        TimeText.text = MaxTime + "s";

        slider.value = 0;
        slider.maxValue = MaxCount;

        for(int i = 0; i < 5; i++)
        {
            int RandomNum = Random.Range(0, 5);
            Icon.Setting(RandomNum);
            Instantiate(Icon, IconList);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }

        if (TimeFlag)
        {
            elapsedTime -= Time.deltaTime;

            int currentTime = Mathf.FloorToInt(elapsedTime);
            if (currentTime != displayTime)
            {
                displayTime = currentTime;
                TimeText.text = displayTime + "s";
            }

            if(currentTime <= 0f)
            {
                NotHitIcon();
                playFlag = true;
            }
        }

        if (!playFlag && startFlag)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Check(0);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Check(1);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Check(2);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Check(3);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Check(4);
            }
        }

        if (count == MaxCount && !playFlag)
        {
            Success();
            playFlag = true;
        }
    }
    private void StartEvent()
    {
        PredatorWindow.SetActive(true);
        startFlag = true;
    }

    void Check(int _num)
    {
        if (!TimeFlag)
        {
            TimeFlag = true;
        }

        if (IconList.GetChild(0).GetComponent<PredatorIcon>().num == _num)
        {
            HitIcon();
        }
        else
        {
            NotHitIcon();
        }
        slider.value = count;
    }

    void HitIcon()
    {
        Destroy(IconList.GetChild(0).gameObject);
        count++;
        if( count< MaxCount - 4)
        {
            int RandomNum = Random.Range(0, 5);
            Icon.Setting(RandomNum);
            Instantiate(Icon, IconList);
        }
    }

    void NotHitIcon()
    {
        if(ChanceCount < MaxChanceCount)
        {
            ChanceList.GetChild(ChanceCount).gameObject.SetActive(false);
            ChanceCount++;
        }
        else
        {
            ChanceList.GetChild(ChanceCount).gameObject.SetActive(false);
            if (TimeFlag)
            {
                TimeFlag = false;
            }
            Player_Loss_Status();
            playFlag = true;
            ResultWindow_F.SetActive(true);
        }
    }

    void Player_Loss_Status()
    {
        //HP
        int Player_HP;
        try
        {
            Player_HP = ES3.Load<int>("Player_Curret_HP");
        }
        catch
        {
            Player_HP = 5000;
        }
        Player_HP = Player_HP * 90 / 100;
        ES3.Save("Player_Curret_HP", Player_HP);
        //연료
        int TrainFuel;
        TrainFuel = ES3.Load<int>("Train_Curret_Fuel");
        TrainFuel = TrainFuel * 90 / 100;
        ES3.Save("Train_Curret_Fuel", TrainFuel);
        //돈
        playerData.SA_GameLoseCoin(10);
    }

    void Success()
    {
        if (TimeFlag)
        {
            TimeFlag = false;
        }
        ResultWindow_S.SetActive(true);
    }

    public void PredatorEnd()
    {
        SelectStage.SetActive(true);
    }
}
