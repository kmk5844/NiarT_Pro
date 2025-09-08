using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton_Route : MonoBehaviour
{
    [HideInInspector]
    public Station_GameStart stationGameStartDirector;
    [SerializeField]
    StageDataObject stageData;
    public int stageData_Num;
    Button btn;

    public int ButtonNum;
    public Sprite[] NumSprite;
    public Image Chapter_Num_Image;
    public GameObject SelectMark;
    public GameObject ClearMark;

    public GameObject ClearSprite;

    private void Start()
    {
        SelectMark.SetActive(false);
        //ClearMark.SetActive(false);
        btn.onClick.AddListener(()=>stationGameStartDirector.SelectStage(ButtonNum));    
    }

    public void OnEnable()
    {
        if(btn == null)
        {
            btn = GetComponent<Button>();
        }
        btn.enabled = stageData.Stage_OpenFlag;
        if (btn.enabled)
        {
            btn.image.color = Color.white;
        }
        else
        {
            btn.image.color = Color.gray;
        }
    }

    public void ChangeStageNum(int i, StageDataObject _stageData)
    {
        stageData_Num = i;
        stageData = _stageData;
        int chapterNum = i / 5;
        Chapter_Num_Image.sprite = NumSprite[chapterNum];
        if (btn != null)
        {
            btn.enabled = stageData.Stage_OpenFlag;
            if (btn.enabled)
            {
                btn.image.color = Color.white;
            }
            else
            {
                btn.image.color = Color.gray;
            }
        }

        if (_stageData.Stage_ClearFlag)
        {
            ClearMark.SetActive(true);
            ClearSprite.SetActive(true);
        }
        else
        {
            ClearMark.SetActive(false);
            ClearSprite.SetActive(false);
        }
    }

    public void SelectMarkOn()
    {
        SelectMark.SetActive(true);
        ClearMark.SetActive(false);
    }

    public void SelectMarkOff()
    {
        SelectMark.SetActive(false);
        if (stageData.Stage_ClearFlag)
        {
            ClearMark.SetActive(true);
        }
    }


    /*    public Station_GameStart gamestartDirection;

        public StageDataObject stageData;
        Button stageButton;
        public GameObject MarkObject;

        public GameObject BossObject;

        private void Start()
        {
            stageButton = GetComponent<Button>();
            if (stageData != null)
            {
                if (!stageData.Stage_OpenFlag)
                {
                    stageButton.enabled= false;
                }
                else
                {
                    stageButton.enabled = true;
                }

                //stageButton.onClick.AddListener(StageButton_Click);
            }
        }

    *//*    void StageButton_Click()
        {
            gamestartDirection.StageButton_Click(stageData_Num);
        }*/
}
