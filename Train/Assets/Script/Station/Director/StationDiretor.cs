using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.Loading;
using MoreMountains.Tools;

public class StationDirector : MonoBehaviour
{
    public GameObject Player_DataObject; // 골드와 포인트 확인
    public GameObject Train_DataObject; // 게임에서 나타낼 기차
    Station_PlayerData playerData;

    [SerializeField]
    Station_TrainMaintenance Director_TrainMaintenance;
    [SerializeField]
    Station_Store Director_Store;
    [SerializeField]
    Station_Fortress Director_Fortress;
    [SerializeField]
    Station_Inventory Director_Inventory;
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
    bool Information_Flag;

    [Header("Store+Fortress")]
    public ScrollRect Store_Fortress_ScrollView;
    public RectTransform Store_Fortress_Content;
    public Button Content_Fortress_Button;
    public Button Content_Store_Button;
    bool isMoving = false;

    [Header("Click Lobby -> Store")]
    public GameObject UI_StoreAndFortress;
    public GameObject UI_Store_ButtonList;
    public GameObject[] UI_Store_Window;
    bool StoreWindow_Flag;
    [Header("Click Lobby -> Fortress")]
    public GameObject[] UI_Fortress_Window;
    bool Fortress_Flag;
    [Header("Click Lobby -> Inventory")]
    public GameObject UI_Inventory;
    public ToggleGroup UI_Inventory_Toggle;
    public GameObject[] UI_Inventory_Window;
    [Header("Click Lobby -> GameStart")]
    public GameObject UI_GameStart;
    public TextMeshProUGUI Stage_Text;
    [Header("Coin&Point")]
    public TextMeshProUGUI[] Coin_Text;
    public TextMeshProUGUI[] Point_Text;
    public GameObject Ban_Panel;
    public GameObject Coin_Ban_Text;
    public GameObject Point_Ban_Text;
    [Header("Option")]
    public GameObject Option_Object;

    int ui_num;
    int ui_Maintenance_Num;
    int ui_Store_Num;
    int ui_Fortress_Num;
    int ui_Inventory_Num;
    [Header("BGM")]
    public AudioClip StationBGM;

    [Header("Animation")]
    public Animator Ani_Store;

    private void Start()
    {
        MMSoundManagerSoundPlayEvent.Trigger(StationBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);

        playerData = Player_DataObject.GetComponent<Station_PlayerData>();

        //GameStart에 Stage 표현
        for(int i = 0; i < Coin_Text.Length; i++)
        {
            Coin_Text[i].text = playerData.Player_Coin.ToString();
            Point_Text[i].text = playerData.Player_Point.ToString();
        }
        Stage_Text.text = "Stage " + playerData.SA_PlayerData.Stage; 

        ui_num = 0;
        ui_Store_Num = -1;

        Store_Fortress_Content.anchoredPosition = new Vector2(0, 0);

        Content_Fortress_Button.onClick.AddListener(ShowNextBackGround);
        Content_Store_Button.onClick.AddListener(ShowPerviousBackGround);

        // 기차 정비소와 인벤토리의 토글
        TrainMaintenance_ToggleStart();
        Inventory_ToggleStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ui_num == 1)
            {
                if (Director_TrainMaintenance.Part_Window_Flag)
                {
                    Director_TrainMaintenance.Click_Part_Back_Button();
                }
                else if (Information_Flag)
                {
                    Click_Information_Back_Button();
                }
                else
                {
                    Click_Home_Button();
                }
            }
            else if (ui_num == 2)
            {
                if (StoreWindow_Flag)
                {
                    Click_Store_Back_Button();
                }
                else
                {
                    Click_Home_Button();
                }
            }
            else if (ui_num == 3)
            {
                if (Fortress_Flag)
                {
                    Click_Fortress_Back_Button();
                }
                else
                {
                    Click_Home_Button();
                }
            }else if(ui_num == 4)
            {
                Click_Home_Button();
            }
            else if (ui_num == 5)
            {
                Click_Home_Button();
            }
        }
    }

    private void TrainMaintenance_ToggleStart()
    {
        foreach (var toggle in UI_TrainMaintenance_Toggle.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(TrainMaintenance_ToggleChange);
        }
    }

    private void Inventory_ToggleStart()
    {
        foreach(var toggle in UI_Inventory_Toggle.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(Inventory_ToggleChange);
        }
    }

    private void TrainMaintenance_ToggleChange(bool isOn)
    {
        if (isOn && ui_num == 1) //isON 제거하면 끄는 순간과 켜는 순간이 2번이나 작동이 된다
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
    }

    private void Inventory_ToggleChange(bool isOn)
    {
        if(isOn && ui_num == 4)
        {
            for(int i = 0; i < UI_Inventory_Toggle.transform.childCount; i++)
            {
                if (UI_Inventory_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    ui_Inventory_Num = i;
                    UI_Inventory_Window[i].SetActive(true);
                    UI_Inventory_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    UI_Inventory_Window[i].SetActive(false);
                    UI_Inventory_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                }
            }
            Total_Init();
        }
    }

    public int Check_UI_Inventory_Num()
    {
        return ui_Inventory_Num;
    }

    private void Inventory_ToggleInit()
    {
        if (ui_num == 4)
        {
            for (int i = 0; i < UI_TrainMaintenance_Toggle.transform.childCount; i++)
            {
                if (i == 0)
                {
                    UI_Inventory_Window[i].SetActive(true);
                    UI_Inventory_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
                    UI_Inventory_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    UI_Inventory_Window[i].SetActive(false);
                    UI_Inventory_Toggle.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
                    UI_Inventory_Toggle.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                }
            }
        }
    }

    public void ClickLobbyButton(int num)
    {
        if (num == 5)
        {
            UI_GameStart.SetActive(true);
            Director_GameStart.Check_Train();
            ui_num = 5;
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
                    Check_CoinAndPoint();
                    break;
                case 2:
                    Director_Store.Check_AfterBuy_MercenaryCard();
                    Store_Fortress_Content.anchoredPosition = new Vector2(0, 0);
                    UI_StoreAndFortress.gameObject.SetActive(true);
                    ui_num = 2;
                    Check_CoinAndPoint();
                    break;
                case 3:
                    Store_Fortress_Content.anchoredPosition = new Vector2(-1920, 0);
                    UI_StoreAndFortress.gameObject.SetActive(true);
                    ui_num = 3;
                    Check_CoinAndPoint();
                    break;
                case 4:
                    UI_Inventory.gameObject.SetActive(true);
                    ui_num = 4;
                    break;
            }
        }
        Total_Init();
    }

    public void Click_StoreButton(int UI_Store_Num)
    {
        Ani_Store.SetInteger("StoreClose", -1);
        StoreWindow_Flag = true;
        ui_Store_Num = UI_Store_Num;
        /*        UI_Store_Window[ui_Store_Num].SetActive(true);
                UI_Store_ButtonList.SetActive(false);*/
        Ani_Store.SetInteger("StoreOpen", ui_Store_Num);
    }

    public void Click_Store_Back_Button()
    {
        StoreWindow_Flag = false;
        Ani_Store.SetInteger("StoreOpen", -1);
        /*        UI_Store_Window[ui_Store_Num].SetActive(false);
                UI_Store_ButtonList.SetActive(true);*/
        if (ui_Store_Num == 0)
        {
            Director_Store.Init_StoreButton();
        }
        else if (ui_Store_Num == 2)
        {
            Director_Store.Init_ItemButton();
        }
        Ani_Store.SetInteger("StoreClose", ui_Store_Num);
        ui_Store_Num = -1;
    }

    public void Click_FortressButton(int UI_Fortress_Num)
    {
        Fortress_Flag = true;
        ui_Fortress_Num = UI_Fortress_Num;
        UI_Fortress_Window[ui_Fortress_Num].SetActive(true);
        if(UI_Fortress_Num == 2)
        {
           //카드 비활성화 때문에, Count가 인식이 되지 않는 문제가 발생하여 임시로 둠(데모버전 이후 바꾸기)
           Director_Fortress.GetComponent<Station_Fortress>().Mercenary_Check_Button();
        }
    }

    public void Click_Fortress_Back_Button()
    {
        Fortress_Flag = false;
        UI_Fortress_Window[ui_Fortress_Num].SetActive(false);
        ui_Fortress_Num = -1;
    }

    public void Click_Information_Button()
    {
        Information_Flag = true;
        UI_TrainLIst_Window.SetActive(false);
        UI_TrainInformation_Window.SetActive(true);
        UI_TrainMaintenance_Window[ui_Maintenance_Num].SetActive(false);
        UI_Home_Button.SetActive(false);
        UI_Back_Button.SetActive(true);
        Director_TrainMaintenance.Current_Train_Information();
    }

    public void Click_Information_Back_Button()
    {
        Information_Flag = false;
        UI_TrainLIst_Window.SetActive(true);
        UI_TrainInformation_Window.SetActive(false);
        UI_TrainMaintenance_Window[ui_Maintenance_Num].SetActive(true);
        UI_Home_Button.SetActive(true);
        UI_Back_Button.SetActive(false);
    }

    public void Click_Home_Button()
    {
        Total_Init();

        if (ui_num == 1)
        {
            TrainMaintenance_ToggleInit();
            UI_TrainMaintenance.gameObject.SetActive(false);
        }
        else if (ui_num == 2)
        {
            UI_StoreAndFortress.gameObject.SetActive(false);
        }
        else if (ui_num == 3)
        {
            UI_StoreAndFortress.gameObject.SetActive(false);
        }else if(ui_num == 4)
        {
            Inventory_ToggleInit();
            UI_Inventory.gameObject.SetActive(false);
        }
        else if (ui_num == 5)
        {
            UI_GameStart.gameObject.SetActive(false);
        }
        UI_BackGround.gameObject.SetActive(false);

        ui_num = 0; // 꺼져있을 때만 적용
        UI_Lobby.gameObject.SetActive(true);
    }

    public void Click_Option_Button()
    {
        Option_Object.SetActive(true);
    }

    public void Click_Option_Back_Button()
    {
        Option_Object.SetActive(false);
    }

    public void Click_MainMenu_Button()
    {
        LoadingManager.LoadScene("1.MainMenu");
    }

    public void Check_CoinAndPoint()
    {
        for(int i = 0; i <  Coin_Text.Length; i++)
        {
            Coin_Text[i].text = playerData.Player_Coin.ToString();
            Point_Text[i].text = playerData.Player_Point.ToString();
        }
    }

    public void Check_Ban_CoinPoint(bool CoinPoint) // true면 코인 부족, false면 포인트 부족
    {
        Ban_Panel.SetActive(true);
        if (CoinPoint)
        {
            Coin_Ban_Text.SetActive(true);
            Point_Ban_Text.SetActive(false);
        }
        else
        {
            Coin_Ban_Text.SetActive(false);
            Point_Ban_Text.SetActive(true);
        }
    }

    public void Close_Ban_CoinPoint()
    {
        Ban_Panel.SetActive(false);
    }

    public void Total_Init() {
        Close_StoreAndFortress_Window();
        Director_TrainMaintenance.Director_Init_TrainChange();
        Director_TrainMaintenance.Director_Init_TrainPartChange();
        Director_Store.Init_StoreButton();
        Director_Store.Init_ItemButton();
        Director_Fortress.Director_Init_MercenaryUpgrade();
        Director_Fortress.Director_Init_MercenaryPosition();
        if (Station_ItemData.itemChangeFlag)
        {
            Director_Store.Director_Init_ItemSell();
            Director_Inventory.Director_Init_Inventory();
            Director_GameStart.Director_Init_EquipItem();
            Station_ItemData.itemChangeFlag = false;
        }
        Director_Store.Director_Init_ItemSell_ScrollView();
    }

    void ShowPerviousBackGround()
    {
        Debug.Log(ui_num);
        if (ui_num > 1 && !isMoving)
        {
            isMoving = true;
            ui_num--;
            Total_Init();
            StartCoroutine(SmoothMoveContent(ui_num));
        }
    }

    void ShowNextBackGround()
    {
        if (ui_num < 3 &&!isMoving)
        {
            isMoving = true;
            ui_num++;
            Total_Init();
            StartCoroutine(SmoothMoveContent(ui_num));
        }
    }

    void Close_StoreAndFortress_Window()
    {
        foreach (GameObject window in UI_Store_Window)
        {
            if(window.activeSelf == true)
            {
                window.SetActive(false);
            }
        }

        foreach(GameObject window in UI_Fortress_Window)
        {
            if (window.activeSelf == true)
            {
                window.SetActive(false);
            }
        }
    }

    IEnumerator SmoothMoveContent(int targetIndex)
    {
        float startX = Store_Fortress_Content.anchoredPosition.x;
        float targetX = ((targetIndex-2) * 1920); // 각 배경사진의 가로 길이만큼 이동

        float duration = 0.3f; // 이동에 걸리는 시간 (초)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newX = Mathf.Lerp(startX, -targetX, t);
            Debug.Log(newX);
            Store_Fortress_Content.anchoredPosition = new Vector2(newX, 0);
            yield return null;
        }

        Store_Fortress_Content.anchoredPosition = new Vector2(-targetX, 0);
        isMoving = false;
    }
}