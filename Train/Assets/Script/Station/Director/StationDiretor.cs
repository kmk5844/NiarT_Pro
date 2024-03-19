using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StationDirector : MonoBehaviour
{
    public GameObject Player_DataObject;
    public GameObject Train_DataObject;

    Station_PlayerData playerData;
    Station_TrainData trainData;

    List<int> Train_Data_Num;
    public Transform Train_List;

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
        Train_Data_Num = trainData.Train_Num;

        for (int i = 0; i < Train_Data_Num.Count; i++)
        {
            GameObject TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_Station/" + Train_Data_Num[i]), Train_List);
            TrainObject.name = trainData.EX_Game_Data.Information_Train[Train_Data_Num[i]].Train_Name;
            TrainObject.transform.position = Vector3.zero;
            if (i == 0)
            {
                //엔진칸
                TrainObject.transform.position = new Vector3(0f, 1, 0);
            }
            else
            {
                //나머지칸
                TrainObject.transform.position = new Vector3((1 + (10.5f * i)) * -1, 1, 0);
            }
        }
        CenterObject();
        ResizeObject();
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
                Director_TranningRoom.Mercenary_Upgarde_Check_Lock();
                Director_TranningRoom.Mercenary_Postion_Check_Lock();
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
        Zomm_In_Out_Train();
    }

    public void ClickBackButton()
    {
        OnToggleInit();
        ui_num = 0;
        UI_TrainMaintenance.gameObject.SetActive(false);
        UI_Store.gameObject.SetActive(false);
        UI_TrainingRoom.gameObject.SetActive(false);
        UI_BackGround.gameObject.SetActive(false);
        UI_Lobby.gameObject.SetActive(true);
        Zomm_In_Out_Train();
    }

    private void Zomm_In_Out_Train()
    {
        if (ui_num == 1)
        {

        }
        else
        {

        }
    }

    public void Add_Train_List()
    {
        Vector3 position = Train_List.GetChild(Train_List.childCount - 1).transform.position;
        GameObject AddTrain = Instantiate(Resources.Load<GameObject>("TrainObject_Station/" + 1), Train_List);
        AddTrain.name = trainData.EX_Game_Data.Information_Train[1].Train_Name;
        AddTrain.transform.position = new Vector3(position.x, position.y, position.z);
    }

    public void Change_Train_List(int train_num, int index)
    {
        Vector3 position = Train_List.GetChild(index).position;
        Destroy(Train_List.GetChild(index).gameObject);
        GameObject changeTrain = Instantiate(Resources.Load<GameObject>("TrainObject_Station/" + train_num), Train_List);
        changeTrain.name = trainData.EX_Game_Data.Information_Train[train_num].Train_Name;
        changeTrain.transform.position = position;
        changeTrain.transform.SetSiblingIndex(index);
    }

    public void CenterObject()
    {
        int childCount = Train_List.childCount;
        Vector3 CenterPosition = Vector3.zero;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = Train_List.transform.GetChild(i);
            CenterPosition += child.localPosition;
        }
        CenterPosition /= childCount;
        for (int i = 0; i < Train_List.childCount; i++)
        {
            Train_List.GetChild(i).position += CenterPosition * -1;
        }
        Train_List.position += Vector3.up;
    }

    private void ResizeObject()
    {
        float num = Train_List.childCount * 0.1f;
        Train_List.localScale = Vector3.one - new Vector3(num, num, num);
    }
}