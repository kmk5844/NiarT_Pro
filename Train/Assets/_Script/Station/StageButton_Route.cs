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

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(()=>stationGameStartDirector.SelectButton());    
    }

    public void ChangeStageNum(int i, StageDataObject _stageData)
    {
        stageData_Num = i;
        stageData = _stageData;

        btn.interactable = !_stageData.Stage_ClearFlag;
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
