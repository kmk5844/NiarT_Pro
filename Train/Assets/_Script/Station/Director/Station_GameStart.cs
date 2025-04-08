using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Localization.Components;

public class Station_GameStart : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Player_DataObject;
    Station_PlayerData stationPlayerData;
    SA_PlayerData saPlayerData;
    [SerializeField]
    SA_StageList saStageData;
    [SerializeField]
    List<StageDataObject> stagelist;

    [Header("UI")]
    public Image ChapterImage;
    public Sprite[] ChapterSprite;
    public Button nextButton;
    public Button prevButton;
    public Button CancelButton;
    public Button SelectButton;
    public Transform ChapterButton_List;

    [Header("스테이지 선택")]
    public int SelectChapterNum;
    public int maxChapterNum;
    List<StageButton_Route> stageButton;

    int clickNum;
    bool FirstFlag = false;

    private void Awake()
    {
        stagelist = saStageData.Stage;
    }

    private void Start()
    {
        stationPlayerData = Player_DataObject.GetComponent<Station_PlayerData>();
        saPlayerData = stationPlayerData.SA_PlayerData;

        SelectChapterNum = (saPlayerData.New_Stage / 5);
        maxChapterNum = 3;


        stageButton = new List<StageButton_Route>();

        Scene scene = SceneManager.GetActiveScene();
        if(scene.name != "SimpleStation")
        {
            for (int i = 0; i < ChapterButton_List.childCount; i++)
            {
                StageButton_Route _StageButton = ChapterButton_List.GetChild(i).GetComponent<StageButton_Route>();
                _StageButton.ButtonNum = i;
                stageButton.Add(_StageButton);
            }
            ButtonSetting();
            UpdateButtonState();
        }
  
        FirstFlag = true;
    }

    public void Click_nextChapter()
    {
        if (SelectChapterNum < maxChapterNum - 1)
        {
            SelectChapterNum++;
        }
        UpdateButtonState();
    }

    public void Click_prevChapter()
    {
        if (SelectChapterNum > 0)
        {
            SelectChapterNum--;
        }
        UpdateButtonState();
    }

    private void ButtonSetting()
    {
        for (int i = 0; i < stageButton.Count; i++)
        {
            stageButton[i].stationGameStartDirector = this;
        }
        SelectButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(false);
    }

    private void UpdateButtonState()
    {
        ChapterImage.sprite = ChapterSprite[SelectChapterNum];

        nextButton.interactable = SelectChapterNum < maxChapterNum - 1;
        prevButton.interactable = SelectChapterNum > 0;

        for (int i = 0; i < stageButton.Count; i++)
        {
            StartCoroutine(Data_Update(i));
        }

        if (FirstFlag)
        {
            CancelSelectStage();
        }
    }

    IEnumerator Data_Update(int i)
    {
        int num = (5 * SelectChapterNum) + i;
        yield return null;
        stageButton[i].ChangeStageNum(num, stagelist[num]);
    }

    public void SelectStage(int buttonNum)
    {
        if(clickNum != -1)
        {
            stageButton[clickNum].SelectMarkOff();
            clickNum = -1;
        }
        CancelButton.gameObject.SetActive(true);
        SelectButton.gameObject.SetActive(true);
        clickNum = buttonNum;
        stageButton[clickNum].SelectMarkOn();
    }

    public void CancelSelectStage()
    {
        if (clickNum != -1)
        {
            CancelButton.gameObject.SetActive(false);
            SelectButton.gameObject.SetActive(false);
            stageButton[clickNum].SelectMarkOff();
            clickNum = -1;
        }
    }
    
    public void ClickGameStart()
    {
        saPlayerData.SA_SelectLevel(stageButton[clickNum].stageData_Num);
        try
        {
            LoadingManager.LoadScene("MissionSelect");
        }
        catch
        {
            SceneManager.LoadScene("MissioNSelect");
        }
    }
}