using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StationDirector : MonoBehaviour
{
    Camera mainCam;
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
    int train_num; // 이걸로 이용하여 열차를 사거나 변경이 가능하다.
    Vector3 default_Train_Scail;
    Vector3 default_Train_Position;
    private void Start()
    {
        Director_TrainMaintenance = transform.GetChild(0).GetComponent<Station_TrainMaintenance>();
        Director_Store = transform.GetChild(1).GetComponent<Station_Store>();
        Director_TranningRoom = transform.GetChild(2).GetComponent<Station_TranningRoom>();

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();

        Coin_Text.text = playerData.Player_Coin + " G";
        Point_Text.text = playerData.Player_Point + " Pt";

        ui_num = 0;
        train_num = 0;
        Train_Data_Num = trainData.Train_Num;
        for (int i = 0; i < Train_Data_Num.Count; i++)
        {
            GameObject TrainObject = Instantiate(Resources.Load<GameObject>("TrainObject_Station/" + Train_Data_Num[i]), Train_List);
            TrainObject.name = trainData.EX_Game_Data.Information_Train[Train_Data_Num[i]].Train_Name;
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
        //float cameraSize = mainCam.orthographicSize;
        Bounds bounds = CalculateBounds(Train_List.gameObject);
        float scaleFactor = mainCam.orthographicSize/ bounds.size.x * 3;

        // 타겟 오브젝트의 크기 조정
        Train_List.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        Train_List.position = new Vector3((Train_List.GetChild(0).transform.position.x + Train_List.GetChild(Train_List.childCount-1).transform.position.x) / 2 * -1, 1, 0);
        default_Train_Scail = Train_List.localScale;
        default_Train_Position = Train_List.position;
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
        if(ui_num == 1)
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
        else if(ui_num == 2)
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
        else if(ui_num == 3)
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

    public void ClickTrainButton(bool flag)
    {
        if (flag) //left
        {
            if (train_num + 1 > Train_List.childCount - 1)
            {
                train_num = Train_List.childCount - 1;
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
            mainCam.transform.position = new Vector3(Train_List.GetChild(0).transform.position.x + 1, 0, -10);
        }
        else
        {
            mainCam.transform.position = new Vector3(Train_List.GetChild(train_num).transform.position.x - 1, 0, -10);
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
        if(ui_num == 1)
        {

        }
        else
        {

        }
    }

    private Bounds CalculateBounds(GameObject target)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(target.transform.position, Vector3.zero);

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }

    private Bounds CalculateChildBounds(GameObject target)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(target.transform.position, Vector3.zero);

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }

    private void ScaleChildren(GameObject target, float scaleFactor)
    {
        foreach (Transform child in target.transform)
        {
            child.localScale *= scaleFactor;
        }
    }
}