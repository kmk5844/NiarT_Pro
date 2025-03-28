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
    SA_StageList saStageData;

    [Header("UI")]
    public Image ChapterImage;
    public Sprite[] ChapterSprite;
    public Button nextButton;
    public Button prevButton;

    [Header("스테이지 선택")]
    public int SelectChapterNum;
    public int maxChapterNum;
    public StageButton_Route[] stageButton;

    private void Start()
    {
        ButtonSetting();
        stationPlayerData = Player_DataObject.GetComponent<Station_PlayerData>();
        saPlayerData = stationPlayerData.SA_PlayerData;
        saStageData = stationPlayerData.SA_StageList;

        SelectChapterNum = (saPlayerData.New_Stage / 5);
        maxChapterNum = 3;

        UpdateButtonState();
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
        for (int i = 0; i < stageButton.Length; i++)
        {
            stageButton[i].stationGameStartDirector = this;
        }
    }

    private void UpdateButtonState()
    {
        ChapterImage.sprite = ChapterSprite[SelectChapterNum];

        nextButton.interactable = SelectChapterNum < maxChapterNum - 1;
        prevButton.interactable = SelectChapterNum > 0;

        for (int i = 0; i < stageButton.Length; i++)
        {
            int num = (5 * SelectChapterNum) + i;
            stageButton[i].ChangeStageNum(num, saStageData.Stage[num]);
        }
    }

    public void SelectButton()
    {
        Debug.Log("선택");
    }
}