using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Localization.Components;
using JetBrains.Annotations;
using System.Linq;

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
    public StageButton_Route[] ChapterButton_List;
    public StageButton_Route Boss_ChapterButton;
    public StageButton_Route NoneBoss_ChapterButton;
    bool ExceptionFlag;

    [Header("스테이지 선택")]
    public int SelectChapterNum;
    public int maxChapterNum;
    List<StageButton_Route> stageButton;
    public int[] Exception_Number;

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
        maxChapterNum = 10;

        stageButton = new List<StageButton_Route>();

        for (int i = 0; i < 4; i++)
        {
            StageButton_Route _StageButton = ChapterButton_List[i];
            _StageButton.ButtonNum = i;
            stageButton.Add(_StageButton);
        }

        ExceptionFlag = true;
        ExceptionFlag = !Exception_Number.Contains(SelectChapterNum);
        Boss_ChapterButton.ButtonNum = 4;
        NoneBoss_ChapterButton.ButtonNum = 4;
        Boss_ChapterButton.gameObject.SetActive(ExceptionFlag);
        NoneBoss_ChapterButton.gameObject.SetActive(!ExceptionFlag);

        ButtonSetting();
        UpdateButtonState();
  
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
        Boss_ChapterButton.stationGameStartDirector = this;
        NoneBoss_ChapterButton.stationGameStartDirector= this;

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

        StartCoroutine(Data_Update_Boss());

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

    IEnumerator Data_Update_Boss()
    {
        int num = (5 * SelectChapterNum) + 4;
        ExceptionFlag = true;
        ExceptionFlag = !Exception_Number.Contains(SelectChapterNum);
        Boss_ChapterButton.gameObject.SetActive(ExceptionFlag);
        NoneBoss_ChapterButton.gameObject.SetActive(!ExceptionFlag);
        yield return null;
        Boss_ChapterButton.ChangeStageNum(num, stagelist[num]);
        NoneBoss_ChapterButton.ChangeStageNum(num, stagelist[num]);
    }

    public void SelectStage(int buttonNum)
    {
        if(clickNum == 4)
        {
            Boss_ChapterButton.SelectMarkOff();
            NoneBoss_ChapterButton.SelectMarkOff();
            clickNum = -1;
        }
        else if(clickNum != -1)
        {
            stageButton[clickNum].SelectMarkOff();
            clickNum = -1;
        }
        CancelButton.gameObject.SetActive(true);

        int num = (5 * SelectChapterNum) + buttonNum;
        if (stagelist[num].Stage_ClearFlag)
        {
            SelectButton.gameObject.SetActive(false);
        }
        else
        {
            SelectButton.gameObject.SetActive(true);
        }

        clickNum = buttonNum;
        
        if(clickNum == 4)
        {
            Boss_ChapterButton.SelectMarkOn();
            NoneBoss_ChapterButton.SelectMarkOn();
        }
        else
        {
            stageButton[clickNum].SelectMarkOn();
        }
    }

    public void CancelSelectStage()
    {
        if (clickNum != -1)
        {
            CancelButton.gameObject.SetActive(false);
            SelectButton.gameObject.SetActive(false);
            if(clickNum == 4)
            {
                Boss_ChapterButton.SelectMarkOff();
                NoneBoss_ChapterButton.SelectMarkOff();
            }
            else
            {
                stageButton[clickNum].SelectMarkOff();
            }
            clickNum = -1;
        }
    }
    
    public void ClickGameStart()
    {
        if(clickNum == 4)
        {
            if (ExceptionFlag)
            {
                saPlayerData.SA_SelectLevel(Boss_ChapterButton.stageData_Num);
            }
            else
            {
                saPlayerData.SA_SelectLevel(NoneBoss_ChapterButton.stageData_Num);
            }
        }
        else
        {
            saPlayerData.SA_SelectLevel(stageButton[clickNum].stageData_Num);
        }
        GameManager.Instance.BeforeGameStart_Enter();
/*        try
        {
            LoadingManager.LoadScene("MissionSelect");
        }
        catch
        {
            SceneManager.LoadScene("MissioNSelect");
        }*/
    }
}