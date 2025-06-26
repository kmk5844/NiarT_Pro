using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;
    bool startFlag = false;

    [Header("데이터")]
    public SA_PlayerData playerData;
    public SA_Event eventData;

    [Header("Window")]
    public GameObject OasisWindow;
    public GameObject SelectStage;

    [Header("UI")]
    public Button[] oasisButton;
    public Button NextButton;

    [SerializeField]
    List<int> RandNum = new List<int>();

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        OasisWindow.SetActive(false);
    }
    private void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }


        SettingInit();
    }

    private void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }

    private void StartEvent()
    {
        OasisWindow.SetActive(true);
        startFlag = true;
    }

    void SettingInit()
    {
        int count = 0;
        while (true)
        {
            int rand = Random.Range(0, 8);
            if (!RandNum.Contains(rand))
            {
                RandNum.Add(rand);
                count++;
            }

            if (count == 3)
            {
                break;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            int cardNum = i;
            int x = RandNum[i];
            oasisButton[i].onClick.AddListener(() => oasissButton_Click(cardNum, x));
        }
        NextButton.onClick.AddListener(NextStation);
        NextButton.gameObject.SetActive(false);
    }

    void oasissButton_Click(int i, int x)
    {
        int randNum = x;
        if(i == 0)
        {
            oasisButton[0].enabled = false;
            oasisButton[1].gameObject.SetActive(false);
            oasisButton[2].gameObject.SetActive(false);
        }
        else if (i == 1) {
            oasisButton[1].enabled = false;
            oasisButton[0].gameObject.SetActive(false);
            oasisButton[2].gameObject.SetActive(false);
        }
        else if(i == 2)
        {
            oasisButton[2].enabled = false;
            oasisButton[0].gameObject.SetActive(false);
            oasisButton[1].gameObject.SetActive(false);
        }

        eventData.Choice_Oasis(randNum);
        NextButton.gameObject.SetActive(true);
    }

    void NextStation()
    {
        SelectStage.SetActive(true);
    }
}