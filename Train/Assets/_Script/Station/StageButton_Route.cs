using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton_Route : MonoBehaviour
{
    public Station_GameStart gamestartDirection;

    public StageDataObject stageData;
    public int stageData_Num;
    Button stageButton;
    public TextMeshProUGUI StageNum_Text;
    public GameObject RoadObject;
    public GameObject LockPanel;
    public GameObject GradeObject;
    Image GradeImg;
    public Sprite[] Grade_Image; // S, A, B, C, D, F
    public GameObject MarkObject;

    public GameObject BossObject;

    private void Start()
    {
        stageButton = GetComponent<Button>();
        if (stageData != null)
        {
            stageData_Num = stageData.Stage_Num;
            StageNum_Text.text = stageData.Stage_Num.ToString();
            if (!stageData.Stage_OpenFlag)
            {
                LockPanel.SetActive(true);
                RoadObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 170f / 255f);
                stageButton.enabled= false;
            }
            else
            {
                LockPanel.SetActive(false);
                RoadObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
                stageButton.enabled = true;
            }

/*            if (stageData.Player_FirstPlay)
            {
                GradeObject.SetActive(true);
                GradeImg = GradeObject.GetComponent<Image>();
*//*                switch (stageData.Player_Grade)
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
                }*//*
            }*/

/*            if (stageData.Boss_Flag)
            {
                BossObject.SetActive(true);
            }
            else
            {
                BossObject.SetActive(false);
            }*/
            stageButton.onClick.AddListener(StageButton_Click);
        }
    }

    void StageButton_Click()
    {
        gamestartDirection.StageButton_Click(stageData_Num);
    }
}
