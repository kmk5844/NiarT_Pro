using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.Loading;

public class StationDirector : MonoBehaviour
{
    public GameObject Player_DataObject; // 골드와 포인트 확인
    public GameObject Train_DataObject; // 게임에서 나타낼 기차
    Station_PlayerData playerData;
    Station_TrainData trainData;

    [SerializeField]
    Station_TrainMaintenance Director_TrainMaintenance;
    [SerializeField]
    Station_Store Director_Store;
    [SerializeField]
    Station_TranningRoom Director_TranningRoom;
    [SerializeField]
    Station_GameStart Director_GameStart;

    [Header("Lobby")]
    public GameObject UI_Lobby;
    public GameObject UI_BackGround;
    [Header("Click Lobby -> Train Maintenance")]
    public GameObject UI_TrainMaintenance;
    public ToggleGroup UI_TrainMaintenance_Toggle;
    public GameObject[] UI_TrainMaintenance_Window;
    public GameObject UI_TrainLIst_Window;
    public GameObject UI_TrainInformation_Window;
    public GameObject UI_Home_Button;
    public GameObject UI_Back_Button;
    public GameObject[] UI_MenuAndGear_After;
    [Header("Click Lobby -> Store")]
    public GameObject UI_Store;
    public GameObject[] UI_Store_Window;
    [Header("Click Lobby -> Fortress")]
    public GameObject UI_Fortress;
    public GameObject[] UI_Fortress_Window;
    [Header("Click Lobby -> GameStart")]
    public GameObject UI_GameStart;
    [Header("Coin&Point")]
    public TextMeshProUGUI[] Coin_Text;
    public TextMeshProUGUI[] Point_Text;

    int ui_num;
    int ui_Maintenance_Num;
    int ui_Store_Num;
    int ui_Fortress_Num;

    private void Start()
    {
        Director_TrainMaintenance = transform.GetChild(0).GetComponent<Station_TrainMaintenance>();
        Director_Store = transform.GetChild(1).GetComponent<Station_Store>();
        Director_TranningRoom = transform.GetChild(2).GetComponent<Station_TranningRoom>();
        Director_GameStart = transform.GetChild(3).GetComponent<Station_GameStart>();

        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();

        for(int i = 0; i < Coin_Text.Length; i++)
        {
            Coin_Text[i].text = playerData.Player_Coin.ToString();
            Point_Text[i].text = playerData.Player_Point.ToString();
        }

        ui_num = 0;
        ui_Store_Num = -1;
        TrainMaintenance_ToggleStart();
    }

    private void TrainMaintenance_ToggleStart()
    {
        foreach (var toggle in UI_TrainMaintenance_Toggle.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(TrainMaintenance_ToggleChange);
        }
    }

    private void TrainMaintenance_ToggleChange(bool isOn)
    {
        if (isOn) //isON 제거하면 끄는 순간과 켜는 순간이 2번이나 작동이 된다
        {
            if (ui_num == 1)
            {
                for (int i = 0; i < UI_TrainMaintenance_Toggle.transform.childCount; i++)
                {
                    if (UI_TrainMaintenance_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn)
                    {
                        ui_Maintenance_Num = i;
                        UI_TrainMaintenance_Window[i].SetActive(true);
                        UI_MenuAndGear_After[i].SetActive(true);
                        UI_TrainMaintenance_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                    }
                    else
                    {
                        UI_TrainMaintenance_Window[i].SetActive(false);
                        UI_MenuAndGear_After[i].SetActive(false);
                        UI_TrainMaintenance_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                    }
                }
            }
            Total_Init();
        }
    }
    private void TrainMaintenance_ToggleInit()
    {
        if (ui_num == 1)
        {
            for (int i = 0; i < UI_TrainMaintenance_Toggle.transform.childCount; i++)
            {
                if (i == 0)
                {
                    UI_TrainMaintenance_Window[i].SetActive(true);
                    UI_TrainMaintenance_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
                    UI_TrainMaintenance_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    UI_TrainMaintenance_Window[i].SetActive(false);
                    UI_TrainMaintenance_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
                    UI_TrainMaintenance_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                }
            }
        }
        //Init구간
        Total_Init();
    }

    public void ClickLobbyButton(int num)
    {
        if (num == 4)
        {
            UI_GameStart.SetActive(true);
            Director_GameStart.Check_Train();
            ui_num = 4;
        }
        else
        {
            UI_Lobby.gameObject.SetActive(false);
            UI_BackGround.gameObject.SetActive(true);
            switch (num)
            {
                case 1:
                    UI_TrainMaintenance.gameObject.SetActive(true);
                    ui_num = 1;
                    break;
                case 2:
                    Director_Store.Check_AfterBuy_MercenaryCard();
                    UI_Store.gameObject.SetActive(true);
                    ui_num = 2;
                    break;
                case 3:
                    UI_Fortress.gameObject.SetActive(true);
                    ui_num = 3;
                    break;
            }
        }
    }

    public void Click_StoreButton(int UI_Store_Num)
    {
        ui_Store_Num = UI_Store_Num;
        UI_Store_Window[ui_Store_Num].SetActive(true);
    }

    public void Click_Store_Back_Button()
    {
        UI_Store_Window[ui_Store_Num].SetActive(false);
        ui_Store_Num = -1;
    }

    public void Click_FortressButton(int UI_Fortress_Num)
    {
        ui_Fortress_Num = UI_Fortress_Num;
        UI_Fortress_Window[ui_Fortress_Num].SetActive(true);
    }

    public void Click_Fortress_Back_Button()
    {
        UI_Fortress_Window[ui_Fortress_Num].SetActive(false);
        ui_Fortress_Num = -1;
    }

    public void Click_Information_Button()
    {
        UI_TrainLIst_Window.SetActive(false);
        UI_TrainInformation_Window.SetActive(true);
        UI_TrainMaintenance_Window[ui_Maintenance_Num].SetActive(false);
        UI_Home_Button.SetActive(false);
        UI_Back_Button.SetActive(true);
    }

    public void Click_Information_Back_Button()
    {
        UI_TrainLIst_Window.SetActive(true);
        UI_TrainInformation_Window.SetActive(false);
        UI_TrainMaintenance_Window[ui_Maintenance_Num].SetActive(true);
        UI_Home_Button.SetActive(true);
        UI_Back_Button.SetActive(false);
    }

    public void Click_Home_Button()
    {
        if (ui_num == 1)
        {
            TrainMaintenance_ToggleInit();
            UI_TrainMaintenance.gameObject.SetActive(false);
        }
        else if (ui_num == 2)
        {
            UI_Store.gameObject.SetActive(false);
        }
        else if (ui_num == 3)
        {
            UI_Fortress.gameObject.SetActive(false);
        }
        else if (ui_num == 4)
        {
            UI_GameStart.gameObject.SetActive(false);
        }
        UI_BackGround.gameObject.SetActive(false);

        ui_num = 0; // 꺼져있을 때만 적용
        UI_Lobby.gameObject.SetActive(true);
    }

    public void Total_Init() { 
        Director_TrainMaintenance.Director_Init_TrainChange();
        Director_Store.Director_Init_TrainyBuy();
        Director_Store.Director_Init_MercenaryBuy();
        Director_TranningRoom.Director_Init_MercenaryUpgrade();
        Director_TranningRoom.Director_Init_MercenaryPosition();
    }
}