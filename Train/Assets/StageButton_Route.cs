using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton_Route : MonoBehaviour
{
    public StageDataObject stageData;
    public TextMeshProUGUI StageNum_Text;
    public GameObject RoadObject;
    public GameObject LockPanel;
    public GameObject GradeObject;
    Image GradeImg;
    public Sprite[] Grade_Image; // S, A, B, C, D, F

    private void Start()
    {
        if(stageData != null)
        {
            StageNum_Text.text = stageData.Stage_Num.ToString();
            if (!stageData.Stage_OpenFlag)
            {
                LockPanel.SetActive(true);
            }
            else
            {
                LockPanel.SetActive(false);
            }
            if (stageData.Player_FirstPlay)
            {
                GradeObject.SetActive(true);
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
        }
    }
}
