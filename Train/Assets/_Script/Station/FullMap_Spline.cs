using Newtonsoft.Json.Serialization;
using Radishmouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class FullMap_Spline : MonoBehaviour
{
    public Station_GameStart gameStartDirection;
    public SA_StageList sa_stagelist;
    public GameObject FullMap_StageButton_Object;
    Vector3[] pos;
    FullMap_StageButton[] stageButton;

    int lastIndex;

    private void Start()
    {
        UILineRenderer ui_line = GetComponent<UILineRenderer>();
        pos = new Vector3[ui_line.points.Length];
        for(int i = 0; i < ui_line.points.Length; i++)
        {
            pos[i] = ui_line.points[i];
        }
        lastIndex = gameStartDirection.Select_StageNum;
        CreateObject();
    }

    void CreateObject()
    {
        Vector3 lastPosition;
        stageButton = new FullMap_StageButton[sa_stagelist.Stage.Count];

        for (int i = 0; i < sa_stagelist.Stage.Count; i++)
        {
            lastPosition = pos[i];
            GameObject obj = Instantiate(FullMap_StageButton_Object, transform);
            obj.transform.localPosition = lastPosition;
            stageButton[i] = obj.GetComponent<FullMap_StageButton>();
            stageButton[i].gamestartDirection = gameStartDirection;
            stageButton[i].stageData = sa_stagelist.Stage[i];
            stageButton[i].Load();
            if(lastIndex == i)
            {
                stageButton[i].SelectStage_MarkObject.SetActive(true);
            }
        }
    }

    public void OpenFullMap()
    {
        stageButton[lastIndex].SelectStage_MarkObject.SetActive(false);
        stageButton[gameStartDirection.Select_StageNum].SelectStage_MarkObject.SetActive(true);
        lastIndex = gameStartDirection.Select_StageNum;
    }
}
