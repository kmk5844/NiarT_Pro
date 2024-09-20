using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullMap_StageButton : MonoBehaviour
{
    public Station_GameStart gamestartDirection;

    public StageDataObject stageData;
    public int stageData_Num;

    public GameObject LockPanel;
    Button Btn;
    public GameObject GradeObject;
    Image GradeImg;
    public Sprite[] Grade_Image;
    public GameObject SelectStage_MarkObject;

    public void Load()
    {
        Btn = GetComponent<Button>();
        if (stageData != null)
        {
            stageData_Num = stageData.Stage_Num;
            if (!stageData.Stage_OpenFlag)
            {
                LockPanel.SetActive(true);
                GetComponent<Button>().enabled = false;
            }
            else
            {
                LockPanel.SetActive(false);
                GetComponent<Button>().enabled = true;
            }

            if (stageData.Player_FirstPlay)
            {
                GradeObject.SetActive(true);
                GradeImg = GradeObject.GetComponent<Image>();
                switch (stageData.Player_Grade)
                {
                    case StageDataObject.Grade.S:
                        GradeImg.sprite = Grade_Image[0];
                        break;
                    case StageDataObject.Grade.A:
                        GradeImg.sprite = Grade_Image[1];
                        break;
                    case StageDataObject.Grade.B:
                        GradeImg.sprite = Grade_Image[2];
                        break;
                    case StageDataObject.Grade.C:
                        GradeImg.sprite = Grade_Image[3];
                        break;
                    case StageDataObject.Grade.D:
                        GradeImg.sprite = Grade_Image[4];
                        break;
                    case StageDataObject.Grade.F:
                        GradeImg.sprite = Grade_Image[5];
                        break;
                }
            }
            Btn.onClick.AddListener(FullMap_StageButton_Click);
        }
    }

    void FullMap_StageButton_Click()
    {
        gamestartDirection.StageButton_Click(stageData_Num);
        gamestartDirection.Close_FullMapWindow();
    }
}
