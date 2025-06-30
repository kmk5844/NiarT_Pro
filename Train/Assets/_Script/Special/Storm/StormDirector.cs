using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    bool startFlag;
    private float currentTime = 0f;
    private bool isRunning = false;
    bool minigameFlag = false;

    [Header("UI")]
    public Slider slider;
    public TextMeshProUGUI textTimer;


    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
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
        StormWindow.SetActive(true);
        startFlag = true;
    }

    void Reward()
    {
        int seconds = Mathf.FloorToInt(currentTime % 60);

        if(seconds <= 8)
        {
            Debug.Log("최고로 좋은");
        }else if(seconds > 8 && seconds <= 10)
        {
            Debug.Log("그럭저럭 좋은");
        }
        else if (seconds > 10 && seconds <= 12)
        {
            Debug.Log("좋은");
        }
        else if(seconds > 12 && seconds <= 14)
        {
            Debug.Log("안 좋은");
        }
        else if (seconds > 14)
        {
            Debug.Log("안 좋은");
        }


        Debug.Log(seconds);
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
}