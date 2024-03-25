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

    public Transform Train_List;
    public ScrollRect ScrollRect_TrainList;

    [SerializeField]
    Station_TrainMaintenance Director_TrainMaintenance;
    [SerializeField]
    Station_Store Director_Store;
    [SerializeField]
    Station_TranningRoom Director_TranningRoom;

    [Header("Lobby")]
    public GameObject UI_Lobby;
    public GameObject UI_BackGround;
    [Header("Click Lobby -> Train Maintenance")]
    public GameObject UI_TrainMaintenance;
    public ToggleGroup UI_TrainMaintenance_Toggle;
    public GameObject[] UI_TrainMaintenance_Window;
    [Header("Click Lobby -> Store")]
    public GameObject UI_Store;
    public ToggleGroup UI_Store_Toggle;
    public GameObject[] UI_Store_Window;
    [Header("Click Lobby -> TrainingRoom")]
    public GameObject UI_TrainingRoom;
    public ToggleGroup UI_TrainingRoom_Toggle;
    public GameObject[] UI_TrainingRoom_Window;
    [Header("Coin&Point")]
    public TextMeshProUGUI Coin_Text;
    public TextMeshProUGUI Point_Text;

    int ui_num;

    private void Start()
    {
        Director_TrainMaintenance = transform.GetChild(0).GetComponent<Station_TrainMaintenance>();
        Director_Store = transform.GetChild(1).GetComponent<Station_Store>();
        Director_TranningRoom = transform.GetChild(2).GetComponent<Station_TranningRoom>();

        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();

        Coin_Text.text = playerData.Player_Coin + " G";
        Point_Text.text = playerData.Player_Point + " Pt";

        ui_num = 0;

        for (int i = 0; i < trainData.Train_Num.Count; i++)
        {
            GameObject TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_StationLobby/" + trainData.Train_Num[i]), Train_List);
            TrainObject.name = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[i]].Train_Name;
        }
        ResizedContent();
        OnToggleStart();
    }

    private void OnToggleStart()
    {
        foreach (var toggle in UI_TrainMaintenance_Toggle.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
        foreach (var toggle in UI_Store_Toggle.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
        foreach (var toggle in UI_TrainingRoom_Toggle.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn) //isON 제거하면 끄는 순간과 켜는 순간이 2번이나 작동이 된다
        {
            if (ui_num == 1)
            {
                for (int i = 0; i < UI_TrainMaintenance_Toggle.transform.childCount; i++)
                {
                    if (UI_TrainMaintenance_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn)
                    {
                        UI_TrainMaintenance_Window[i].SetActive(true);
                        UI_TrainMaintenance_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                    }
                    else
                    {
                        UI_TrainMaintenance_Window[i].SetActive(false);
                        UI_TrainMaintenance_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                    }
                }
            }
            else if (ui_num == 2)
            {
                
                Director_Store.Check_AfterBuy_MercenaryCard();
                for (int i = 0; i < UI_Store_Toggle.transform.childCount; i++)
                {
                    if (UI_Store_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn)
                    {
                        UI_Store_Window[i].SetActive(true);
                        UI_Store_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                    }
                    else
                    {
                        UI_Store_Window[i].SetActive(false);
                        UI_Store_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                    }
                }
            }
            else if (ui_num == 3)
            {
/*                Director_TranningRoom.Mercenary_Upgarde_Check_Lock();
                Director_TranningRoom.Mercenary_Postion_Check_Lock();*/
                for (int i = 0; i < UI_TrainingRoom_Toggle.transform.childCount; i++)
                {
                    if (UI_TrainingRoom_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn)
                    {
                        UI_TrainingRoom_Window[i].SetActive(true);
                        UI_TrainingRoom_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                    }
                    else
                    {
                        UI_TrainingRoom_Window[i].SetActive(false);
                        UI_TrainingRoom_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                    }
                }
            }
            Total_Init();
        }
    }
    private void OnToggleInit()
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
        else if (ui_num == 2)
        {
            for (int i = 0; i < UI_Store_Toggle.transform.childCount; i++)
            {
                if (i == 0)
                {
                    UI_Store_Window[i].SetActive(true);
                    UI_Store_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
                    UI_Store_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    UI_Store_Window[i].SetActive(false);
                    UI_Store_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
                    UI_Store_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                }
            }
        }
        else if (ui_num == 3)
        {
            for (int i = 0; i < UI_TrainingRoom_Toggle.transform.childCount; i++)
            {
                if (i == 0)
                {
                    UI_TrainingRoom_Window[i].SetActive(true);
                    UI_TrainingRoom_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
                    UI_TrainingRoom_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    UI_TrainingRoom_Window[i].SetActive(false);
                    UI_TrainingRoom_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
                    UI_TrainingRoom_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                }
            }
        }
        //Init구간
        Total_Init();
    }

    public void ClickLobbyButton(int num)
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
        OnToggleInit();
        if(ui_num == 1)
        {
            UI_TrainMaintenance.gameObject.SetActive(false);
        }else if (ui_num == 2)
        {
            UI_Store.gameObject.SetActive(false);
        }else if(ui_num == 3)
        {
            UI_TrainingRoom.gameObject.SetActive(false);
        }
        UI_BackGround.gameObject.SetActive(false);

        ui_num = 0; // 꺼져있을 때만 적용
        UI_Lobby.gameObject.SetActive(true);
    }

    public void Change_Train_List(int train_num, int index) // 바꾸기 클릭 했을 경우
    {
        Destroy(Train_List.GetChild(index).gameObject);
        GameObject changeTrain = Instantiate(Resources.Load<GameObject>("TrainObject_StationLobby/" + train_num), Train_List);
        changeTrain.name = trainData.EX_Game_Data.Information_Train[train_num].Train_Name;
        changeTrain.transform.SetSiblingIndex(index);
    }

    public void Upgrade_Train_List() // 업그레이드 클릭 했을 경우, 전체적으로 바꾼다.
    {
        for (int i = 0; i < Train_List.childCount; i++)
        {
            Destroy(Train_List.GetChild(i).gameObject);
        }

        foreach (int trainNum in trainData.Train_Num)
        {
            GameObject train = Instantiate(Resources.Load<GameObject>("TrainObject_StationLobby/" + trainNum), Train_List);
            train.name = trainData.EX_Game_Data.Information_Train[trainNum].Train_Name;
        }
    }

    public void Add_Train_List()
    {
        GameObject AddTrain = Instantiate(Resources.Load<GameObject>("TrainObject_StationLobby/" + 100), Train_List);
        AddTrain.name = trainData.EX_Game_Data.Information_Train[100].Train_Name;
        ResizedContent();
    }

    private void ResizedContent()
    {
        GridLayoutGroup TrainGrid = Train_List.GetComponent<GridLayoutGroup>();
        RectTransform ContentSize = Train_List.GetComponent<RectTransform>();

        Vector2 cellSize = TrainGrid.cellSize;
        Vector2 spacing = TrainGrid.spacing;

        float width = (cellSize.x + spacing.x) * (Train_List.childCount-1) + TrainGrid.padding.left -250;

        ContentSize.sizeDelta = new Vector2(width, ContentSize.sizeDelta.y);
        ScrollRect_TrainList.normalizedPosition = Vector2.right;
    }

    public void Total_Init() { 
            Director_TrainMaintenance.Director_Init_TrainChange();
            Director_Store.Director_Init_TrainyBuy();
            Director_Store.Director_Init_MercenaryBuy();
            Director_TranningRoom.Director_Init_MercenaryUpgrade();
            Director_TranningRoom.Director_Init_MercenaryPosition();
    }
}