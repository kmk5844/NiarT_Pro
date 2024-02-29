using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StationDirector : MonoBehaviour
{
    Camera mainCam;
    public Transform Train;

    [Header("Lobby")]
    public GameObject UI_Lobby;
    public GameObject UI_BackGround;
    [Header("Click Lobby -> Train Maintenance")]
    public GameObject UI_TrainMaintenance;
    [Header("Click Lobby -> Store")]
    public GameObject UI_Store;
    [Header("Click Lobby -> TrainingRoom")]
    public GameObject UI_TrainingRoom;

    int ui_num;
    int train_num; // 이걸로 이용하여 열차를 사거나 변경이 가능하다.
    //int off_num;
    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        ui_num = 0;
        train_num = 0;
    }

    public void ClickTrainButton(bool flag)
    {
        if (flag) //left
        {
            if (train_num + 1 > Train.childCount - 1)
            {
                train_num = Train.childCount - 1;
            }
            else
            {
                train_num++;
            }
        }
        else //right
        {
            if(train_num - 1 < 0)
            {
                train_num = 0;
            }
            else
            {
                train_num--;
            }
        }

        if (train_num == 0)
        {
            mainCam.transform.position = new Vector3(0, 0, -10);
        }
        else
        {
            mainCam.transform.position = new Vector3(-9 + ((train_num - 1) * -7), 0, -10);
        }
    }

    public void ClickLobbyButton(int num)
    {
        UI_Lobby.gameObject.SetActive(false);
        UI_BackGround.gameObject.SetActive(true);
        switch (num){
            case 1:
                UI_TrainMaintenance.gameObject.SetActive(true);
                ui_num = 1;
                break;
            case 2:
                UI_Store.gameObject.SetActive(true);
                ui_num = 2;
                break;
            case 3:
                UI_TrainingRoom.gameObject.SetActive(true);
                ui_num = 3;
                break;
        }
    }

    public void ClickBackButton()
    {
        ui_num = 0;
        UI_TrainMaintenance.gameObject.SetActive(false);
        UI_Store.gameObject.SetActive(false);
        UI_TrainingRoom.gameObject.SetActive(false);
        UI_BackGround.gameObject.SetActive(false);
        UI_Lobby.gameObject.SetActive(true);
    }
}