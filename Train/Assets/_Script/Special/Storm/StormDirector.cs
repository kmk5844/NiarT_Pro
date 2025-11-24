using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class StormDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject StormWindow;
    public GameObject SelectStage;
    public GameObject CheckWindow;

    [Header("Data")]
    public SA_TrainData trainData;
    public SA_Event eventData;
    bool startFlag;
    private float currentTime = 0f;
    private bool isRunning = false;
    bool minigameFlag = false;
    int Debuff_Parsent;

    [Header("UI")]
    public Slider slider;
    public TextMeshProUGUI textTimer;
    public LocalizeStringEvent CheckWindowText;

    [Header("Tutorial")]
    public GameObject Tutorial_Object;

    private void Awake()
    {
        Special_Story.Story_Init(null, 8, 0, 0);
        StormWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 0)
        {
            //Debug.Log("작동");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
        CheckWindowText.StringReference.TableReference = "SpecialStage_St";
        CheckWindowText.StringReference.TableEntryReference = "Storm_Reward";
        textTimer.text = string.Format("{0:00}:{1:00}.{2:000}", 0, 0, 0);

        slider.maxValue = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }

        if (!minigameFlag && startFlag)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickSpace();
            }

            if (slider.value > slider.minValue)
            {
                slider.value -= 0.0215f;
            }

            if (slider.value >= slider.maxValue-0.5)
            {
                slider.value = slider.maxValue;
                isRunning = false;
                minigameFlag = true;
                Reward();

            }

            if (isRunning)
            {
                currentTime += Time.deltaTime;
                UpdateTimeText();
            }
        }
    }

    private void StartEvent()
    {
        Tutorial_Object.SetActive(true);
    }

    public void TutorialEnd()
    {
        Tutorial_Object.SetActive(false);
        StormWindow.SetActive(true);
        startFlag = true;
    }

    void Reward()
    {
        int seconds = Mathf.FloorToInt(currentTime % 60);

        if(seconds <= 10)
        {
            Debuff_Parsent = 2;
        }else if(seconds > 10 && seconds <= 14)
        {
            Debuff_Parsent = 5;
        }
        else if (seconds > 14 && seconds <= 16)
        {
            Debuff_Parsent = 7;
        }
        else if(seconds > 16 && seconds <= 20)
        {
            Debuff_Parsent = 10;
        }
        else if (seconds > 20)
        {
            Debuff_Parsent = 15;
        }
        Reward_Debuff(Debuff_Parsent);
        CheckWindowText.StringReference.Arguments = new object[] { Debuff_Parsent };
        CheckWindowText.RefreshString();
        CheckWindow.SetActive(true);
    }

    void ClickSpace()
    {
        if (!isRunning)
        {
            isRunning = true;
        }
        slider.value += 2;
    }

    void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime * 1000) % 1000);

        textTimer.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    void Reward_Debuff(int Parsent)
    {
        int Train_HP;
        int TrainFuel;
        for(int i = 0; i < trainData.Train_Num.Count; i++)
        {
            Train_HP = ES3.Load<int>("Train_Curret_HP_TrainIndex_" + i);
            Train_HP = Train_HP * (100 - Parsent) / 100;
            ES3.Save<int>("Train_Curret_HP_TrainIndex_" + i, Train_HP);
        }
        TrainFuel = ES3.Load<int>("Train_Curret_Fuel");
        TrainFuel = TrainFuel * (100 - Parsent) / 100;
        ES3.Save("Train_Curret_Fuel", TrainFuel);
        eventData.StormDebuff();
    }

    public void StormEnd()
    {
        SelectStage.SetActive(true);
    }
}