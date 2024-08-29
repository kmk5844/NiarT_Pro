using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class FullMap_Spline : MonoBehaviour
{
    public Station_GameStart gameStartDirection;
    public SA_StageList sa_stagelist;
    public GameObject[] FullMap_StageButton_Object;

    private void OnEnable()
    {
        SplineInstantiate sp = GetComponent<SplineInstantiate>();
        FullMap_StageButton_Object = new GameObject[sa_stagelist.Stage.Count];

        for (int i = 0; i < sa_stagelist.Stage.Count; i++)
        {
            FullMap_StageButton_Object[i] = sp.transform.GetChild(0).GetChild(i).gameObject;
        }

        for(int i = 0; i < sa_stagelist.Stage.Count; i++)
        {
            FullMap_StageButton_Object[i].GetComponent<FullMap_StageButton>().gamestartDirection = gameStartDirection;
            FullMap_StageButton_Object[i].GetComponent<FullMap_StageButton>().stageData = sa_stagelist.Stage[i];
            FullMap_StageButton_Object[i].GetComponent<FullMap_StageButton>().Load();
        }
        FullMap_StageButton_Object[gameStartDirection.Select_StageNum].GetComponent<FullMap_StageButton>().SelectStage_MarkObject.SetActive(true);
    }
}
