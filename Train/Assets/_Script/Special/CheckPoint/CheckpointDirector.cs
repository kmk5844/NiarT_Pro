using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject CheckPointWindow;
    public GameObject CheckWindow;
    public GameObject SelectStage;

    [Header("Data")]
    public SA_PlayerData playerData;
    public bool clickflag;
    bool startFlag;
    bool InsertFlag;
    bool RewardFlag;
    bool StopFlag;

    [Header("UI")]
    public Slider slider;
    public Image slider_Handle;
    RectTransform slider_Handle_Ract;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        CheckPointWindow.SetActive(false);
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

        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider_Handle_Ract = slider_Handle.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }

        if (!StopFlag)
        {
            if (!InsertFlag)
            {
                if (slider.value > slider.minValue && !clickflag)
                {
                    slider.value -= 0.0005f;
                }

                if (slider.value > slider.maxValue - 0.1f)
                {
                    slider_Handle.raycastTarget = false;
                    InsertFlag = true;
                }
            }

            if (InsertFlag)
            {
                if (slider_Handle_Ract.pivot.x > 0.05f)
                {
                    Vector2 vec = slider_Handle_Ract.pivot - new Vector2(0.005f, 0f);
                    slider_Handle_Ract.pivot = vec;
                }
                else
                {
                    RewardFlag = true;
                }
            }
        }


        if (RewardFlag)
        {
            Reward();
            RewardFlag = false;
            StopFlag = true;
        }
    }

    private void StartEvent()
    {
        CheckPointWindow.SetActive(true);
        startFlag = true;
    }

    public void Reward()
    {
        Debug.Log(playerData.Coin);
        playerData.SA_GameLoseCoin(10);
        CheckWindow.SetActive(true);
        Debug.Log(playerData.Coin);
    }

    public void CheckPointEnd()
    {
        SelectStage.SetActive(true);
    }
}
