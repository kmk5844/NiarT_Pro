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
    public Station_Player Player;

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
    [SerializeField]
    Station_Conversion Direcotr_Conversion;

    [Header("특수 플래그")]
    public bool simplestationFlag;
    [SerializeField]
    private GameObject SubStageSelectObject;
    [HideInInspector]
    public PlayerReadyDirector Director_PlayerReadyDirector;

    //[Header("Lobby")]
    //public GameObject UI_Lobby;
    //public GameObject UI_BackGround;

    //1
    [Header("Click Lobby -> Train Maintenance")]
    public GameObject UI_TrainMaintenance;
    public ToggleGroup UI_TrainMaintenance_Toggle;
    public GameObject[] UI_TrainMaintenance_Window;
    public GameObject UI_TrainLIst_Window;
    public GameObject UI_TrainInformation_Window;
    public GameObject UI_Home_Button;
    public GameObject UI_Back_Button;
    public GameObject[] UI_MenuAndGear_After;


/*    [Header("Store+Fortress")]
    public ScrollRect Store_Fortress_ScrollView;
    public RectTransform Store_Fortress_Content;
    public Button Content_Fortress_Button;
    public Button Content_Store_Button;
    bool isMoving = false;*/

    //2 : store
    [Header("Click Lobby -> Store")]
    public GameObject UI_Store;
    public GameObject[] UI_Store_Window;
    public Button[] UI_Store_Button;
    public bool UI_Store_BuyAndSell_Flag;

    //3 : Fortress
    [Header("Click Lobby -> Store&Fortress")]
    public GameObject UI_Fortress;
    //public GameObject[] UI_Train_Lock_Panel;
    //public GameObject[] UI_Store_BackButton;
    //public GameObject[] UI_Fortress_Window;

    //4 : Inventory
    [Header("Click Lobby -> Inventory")]
    public GameObject UI_Inventory;
/*    public Toggle[] UI_Inventory_Toggle;
    public GameObject[] UI_Inventory_Window;
*/
    //5 : GameStart
    [Header("Click Lobby -> GameStart")]
    public GameObject UI_GameStart;

    //6: Converstion
    [Header("Click Lobby -> Conversion")]
    public GameObject UI_Conversion;

    [Header("Click Help")]
    public GameObject[] UI_HelpWindow;
    public bool Help_Flag;


    [Header("Coin&Point")]
    public TextMeshProUGUI[] Coin_Text;
/*    public GameObject Ban_Panel;
    public GameObject Coin_Ban_Text;
    public GameObject Point_Ban_Text;*/

    [Header("Option")]
    public GameObject Option_Object;

    int ui_num;
    public bool Item_Buy_Sell;
    int ui_Maintenance_Num;
    int ui_Inventory_Num;
    [HideInInspector]
    public bool Option_Flag;
    [Header("BGM&SFX")]
    public AudioClip StationBGM;
    public AudioClip EnterSFX;
    public AudioClip ESCSFX;
    public AudioClip BuySFX;
    public AudioClip ErrorSFX;


    [Header("게임")]
    public GameObject[] GameNotice;

    private void Awake()
    {
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        if (playerData.SA_PlayerData.SimpleStation)
        {
            simplestationFlag = true;
            playerData.SA_PlayerData.change_simplestation(false);
            UI_TrainMaintenance_Toggle.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            simplestationFlag = false;
        }

        if (simplestationFlag)
        {
            Director_PlayerReadyDirector = SubStageSelectObject.GetComponent<PlayerReadyDirector>();
        }
    }

    private void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }

        MMSoundManagerSoundPlayEvent.Trigger(StationBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);


        //GameStart에 Stage 표현
        for(int i = 0; i < Coin_Text.Length; i++)
        {
            Coin_Text[i].text = playerData.Player_Coin.ToString();
        }

        ui_num = 0;
        UI_Store_BuyAndSell_Flag = true;
/*        Store_Fortress_Content.anchoredPosition = new Vector2(0, 0);

        Content_Fortress_Button.onClick.AddListener(ShowNextBackGround);
        Content_Store_Button.onClick.AddListener(ShowPerviousBackGround);*/

        // 기차 정비소와 인벤토리의 토글
        TrainMaintenance_ToggleStart();
/*        Inventory_ToggleStart();*/
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MMSoundManagerSoundPlayEvent.Trigger(ESCSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
            if (Option_Flag)
            {
                Click_Option_Back_Button();
            }
            else if (ui_num == 0)
            {
                Click_Option_Button();
            }
            else if (ui_num == 1)
            {
                if (Help_Flag)
                {
                    Click_HelpClose(0);
                }
                else if (Director_TrainMaintenance.Train_BanFlag)
                {
                    Director_TrainMaintenance.Close_Warning_Window();
                }
                else
                {
                    Click_Home_Button();
                }
            }
            else if (ui_num == 2)
            {
                if (Help_Flag)
                {
                    Click_HelpClose(1);
                }
                else if (Director_Store.Store_CheckFlag) {
                    if (UI_Store_BuyAndSell_Flag)
                    {
                        Director_Store.Close_Buy_Window();
                    }
                    else
                    {
                        Director_Store.Close_Sell_Window();
                    }
                    Director_Store.Close_Buy_Window();
                }
                else if (Director_Store.Store_BanFlag)
                {
                    Director_Store.Close_Ban_Player_Coin();
                }
                else
                {
                    Click_Home_Button();
                }
            }
            else if (ui_num == 3)
            {
                if (Help_Flag)
                {
                    Click_HelpClose(2);
                }
                else if (Director_Fortress.Tranining_BanFlag)
                {
                    Director_Fortress.Close_Warning_Window();
                }
                else
                {
                    Click_Home_Button();
                }
            } else if (ui_num == 4)
            {
                if (Director_Inventory.UseWindowFlag)
                {
                    //Director_Inventory.UseItemStatus_NoButton();
                } else if (Director_Inventory.UseItemWindowFlag)
                {
                    //Director_Inventory.UseItemWindow_BackButton();
                }
                else
                {
                    Click_Home_Button();
                }
            }
            else if (ui_num == 5)
            {
                Click_Home_Button();

                /*                if (Director_GameStart.EquipItemWindowFlag)
                                {
                                    Director_GameStart.Close_ItemCountWindow();
                                } else if (Director_GameStart.EquipItemListFlag)
                                {
                                    Director_GameStart.Close_Inventory_Window();
                                }
                                else if (Director_GameStart.FullMapFlag)
                                {
                                    Director_GameStart.Close_FullMapWindow();
                                }
                                else
                                {
                                    Click_Home_Button();
                                }*/
            }/*else if(ui_num == 6)
            {
                if (Direcotr_Conversion.AfterConversionFlag)
                {
                    Direcotr_Conversion.Button_Check();
                }
                else { 
                    Click_Home_Button();
                }
            }*/
        }
    }

    private void TrainMaintenance_ToggleStart()
    {
        foreach (var toggle in UI_TrainMaintenance_Toggle.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(TrainMaintenance_ToggleChange);
        }
    }

/*    private void Inventory_ToggleStart()
    {
        foreach(Toggle toggle in UI_Inventory_Toggle)
        {
            toggle.onValueChanged.AddListener(Inventory_ToggleChange);
        }
    }*/

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
                    if(i == 0)
                    {
                        Director_TrainMaintenance.TrainBuyWindow_Flag = true;
                    }
                    else
                    {
                        Director_TrainMaintenance.TrainBuyWindow_Flag = false;
                    }
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
            Director_TrainMaintenance.TrainBuyWindow_Flag = true;
        }
    }

/*    private void Inventory_ToggleChange(bool isOn)
    {
        if(isOn && ui_num == 4)
        {
            for(int i = 0; i < UI_Inventory_Toggle.Length; i++)
            {
                UI_Inventory_Toggle[UI_Inventory_Toggle.Length-1 - i].transform.SetAsLastSibling();
                if (UI_Inventory_Toggle[i].isOn)
                {
                    ui_Inventory_Num = i;
                    UI_Inventory_Window[i].SetActive(true);
                    UI_Inventory_Toggle[i].interactable = false;
                }
                else
                {
                    UI_Inventory_Window[i].SetActive(false);
                    UI_Inventory_Toggle[i].interactable = true;
                }
            }
            UI_Inventory_Toggle[ui_Inventory_Num].transform.SetAsLastSibling();

            Total_Init();
        }
    }*/

    public int Check_UI_Inventory_Num()
    {
        return ui_Inventory_Num;
    }

/*    private void Inventory_ToggleInit()
    {
        if (ui_num == 4)
        {
            for (int i = 0; i < UI_Inventory_Toggle.Length; i++)
            {
                UI_Inventory_Toggle[UI_Inventory_Toggle.Length-1 - i].transform.SetAsLastSibling();
                if (i == 0)
                {
                    ui_Inventory_Num = i;
                    UI_Inventory_Window[i].SetActive(true);
                    UI_Inventory_Toggle[i].isOn = true;
                    UI_Inventory_Toggle[i].interactable = false;
                }
                else
                {
                    UI_Inventory_Window[i].SetActive(false);
                    UI_Inventory_Toggle[i].isOn = false;
                    UI_Inventory_Toggle[i].interactable = true;
                }
            }
        }
    }*/

    public void ClickLobbyButton(int num)
    {
/*        UI_Lobby.gameObject.SetActive(false);
        UI_BackGround.gameObject.SetActive(true);*/
        Player.OpenUIFlag = true;

        switch (num)
        {
            case 1:
                MMSoundManagerSoundPlayEvent.Trigger(EnterSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
                Director_TrainMaintenance.TrainMainTenance_Flag = true;
                UI_TrainMaintenance.gameObject.SetActive(true);
                StartCoroutine(Director_TrainMaintenance.Check_TrainState_Slider_Buy());
                ui_num = 1;
                Check_Coin();
                break;
            case 2:
                //Director_Store.Check_AfterBuy_MercenaryCard();
                MMSoundManagerSoundPlayEvent.Trigger(EnterSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
                UI_Store.gameObject.SetActive(true);
                ui_num = 2;
                Check_Coin();
                break;
            case 3:
                MMSoundManagerSoundPlayEvent.Trigger(EnterSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
                UI_Fortress.gameObject.SetActive(true);
                ui_num = 3;
                Check_Coin();
                break;
            case 4:
                MMSoundManagerSoundPlayEvent.Trigger(ESCSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
                UI_Inventory.gameObject.SetActive(true);
                ui_num = 4;
                break;
            case 5:
                MMSoundManagerSoundPlayEvent.Trigger(ESCSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
                if(simplestationFlag)
                {
                    SubStageSelectObject.SetActive(true);
                }
                else
                {
                    UI_GameStart.SetActive(true);
                    //Director_GameStart.Check_Train();
                }
                ui_num = 5;
                break;
            case 6:
                UI_Conversion.SetActive(true);
                ui_num = 6;
                break;
        }
        Total_Init();
    }

    public void Click_StoreButton(bool flag) // true = Buy, false = Sell
    {
        if (flag)
        {
            UI_Store_BuyAndSell_Flag = true;
            UI_Store_Window[0].SetActive(true);
            UI_Store_Window[1].SetActive(false);
            UI_Store_Button[0].interactable = false;
            UI_Store_Button[1].interactable = true;
        }
        else
        {
            UI_Store_BuyAndSell_Flag = false;
            UI_Store_Window[0].SetActive(false);
            UI_Store_Window[1].SetActive(true);
            UI_Store_Button[0].interactable = true;
            UI_Store_Button[1].interactable = false;
        }
        Director_Store.Init_Information();
        Director_Store.Set_ButtonText();
    }

/*    public void Click_ItemSellBackButton()
    {
        //Director_Store.Director_Tooltip_Off();
        UI_Store_Window[5].SetActive(false);
        Director_Store.ItemSellFlag = false;
    }*/

    /*IEnumerator Click_StoreButton_Ani(int UI_Store_Num)
    {
        RectTransform Train_Window = UI_Store_Window[UI_Store_Num].GetComponent<RectTransform>();
        float startY = Train_Window.anchoredPosition.y;
        float targetY = 970f;

        float duration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newY = Mathf.Lerp(startY, targetY, t);
            Train_Window.anchoredPosition = new Vector2(Train_Window.anchoredPosition.x, newY);
            yield return null;
        }

        UI_Store_Window[UI_Store_Num].GetComponent<RectTransform>().SetAsLastSibling();
        if(UI_Store_Num == 1)
        {
            Director_Store.Store_Train_Num = UI_Store_Num;
            UI_Train_Lock_Panel[0].SetActive(Director_Store.Check_Part_Store_Lock(51));
        }
        else if(UI_Store_Num == 2)
        {
            Director_Store.Store_Train_Num = UI_Store_Num;
            UI_Train_Lock_Panel[1].SetActive(Director_Store.Check_Part_Store_Lock(52));
        }else if(UI_Store_Num == 0)
        {
            Director_Store.Store_Train_Num = UI_Store_Num;
        }
        else
        {
            Director_Store.Store_Train_Num = -1;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newY = Mathf.Lerp(targetY, startY, t);
            Train_Window.anchoredPosition = new Vector2(Train_Window.anchoredPosition.x, newY);
            yield return null;
        }
    }*/
/*
    public void Click_FortressButton(int UI_Fortress_Num)
    {
        StartCoroutine(Click_FortressButton_Ani(UI_Fortress_Num));
*//*        if (UI_Fortress_Num == 2)
        {
            //카드 비활성화 때문에, Count가 인식이 되지 않는 문제가 발생하여 임시로 둠(데모버전 이후 바꾸기)
            Director_Fortress.GetComponent<Station_Fortress>().Mercenary_Check_Button();
        }*//*     
    }*/

/*    IEnumerator Click_FortressButton_Ani(int UI_Fortress_Num)
    {
        RectTransform Train_Window = UI_Fortress_Window[UI_Fortress_Num].GetComponent<RectTransform>();
        float startY = Train_Window.anchoredPosition.y;
        float targetY = 970f;

        float duration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newY = Mathf.Lerp(startY, targetY, t);
            Train_Window.anchoredPosition = new Vector2(Train_Window.anchoredPosition.x, newY);
            yield return null;
        }

        UI_Fortress_Window[UI_Fortress_Num].GetComponent<RectTransform>().SetAsLastSibling();

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newY = Mathf.Lerp(targetY, startY, t);
            Train_Window.anchoredPosition = new Vector2(Train_Window.anchoredPosition.x, newY);
            yield return null;
        }
    }*/

    public void Click_Home_Button()
    {
        Total_Init();
        Player.OpenUIFlag = false;

        if (ui_num == 1)
        {
            if (Director_TrainMaintenance.PassiveUpgrade_Tooltip != null && Director_TrainMaintenance.PassiveUpgrade_Tooltip.gameObject.activeSelf)
            {
                Director_TrainMaintenance.PassiveUpgrade_Tooltip.Tooltip_Off();
            }
            Director_TrainMaintenance.TrainMainTenance_Flag = false;
            TrainMaintenance_ToggleInit();
            UI_TrainMaintenance.gameObject.SetActive(false);
        }
        else if (ui_num == 2)
        {
            Click_StoreButton(true);
            UI_Store.gameObject.SetActive(false);
/*            ui_store_num = 0;
            Director_Store.Store_Train_Num = 0;
            UI_Store_Window[0].GetComponent<RectTransform>().SetAsLastSibling();
            UI_Fortress_Window[0].GetComponent<RectTransform>().SetAsLastSibling();*/
        }else if(ui_num == 3)
        {
            UI_Fortress.gameObject.SetActive(false);
        }
        else if(ui_num == 4)
        {
            //Inventory_ToggleInit();
            UI_Inventory.gameObject.SetActive(false);
        }
        else if (ui_num == 5)
        {
            if (simplestationFlag)
            {
                SubStageSelectObject.SetActive(false);
            }
            else
            {
                UI_GameStart.SetActive(false);
            }
        }else if(ui_num == 6)
        {
            Direcotr_Conversion.Item_53_Init();
            UI_Conversion.gameObject.SetActive(false);
            //Check_ItemList(false, useItem);
        }
        //UI_BackGround.gameObject.SetActive(false);

        if(ui_num != 4)
        {
            ui_num = 0; // 꺼져있을 때만 적용
        }
        else
        {
            ui_num = 3;
        }
        //UI_Lobby.gameObject.SetActive(true);
    }

    public void Click_Option_Button()
    {
        Option_Object.SetActive(true);
        Option_Flag = true;
    }

    public void Click_Option_Back_Button()
    {
        Option_Object.SetActive(false);
        Option_Flag = false;
    }

    public void Click_MainMenu_Button()
    {
        LoadingManager.LoadScene("1.MainMenu");
    }

    public void Check_Coin()
    {
        for(int i = 0; i <  Coin_Text.Length; i++)
        {
            Coin_Text[i].text = playerData.Player_Coin.ToString();
        }
    }

/*    public void Check_Ban_CoinPoint(bool CoinPoint) // true면 코인 부족, false면 포인트 부족
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
        Ban_Flag = true;
    }

    public void Close_Ban_CoinPoint()
    {
        Ban_Panel.SetActive(false);
        Ban_Flag = false;
    }*/

    public void Total_Init() {
        //Director_TrainMaintenance.Director_Init_TrainChange();
        //Director_TrainMaintenance.Director_Init_TrainPartChange();
        //Director_TrainMaintenance.Direcotr_Init_TrainUpgrade();

/*        UI_Store_Window[0].GetComponent<RectTransform>().SetAsLastSibling();
        UI_Fortress_Window[0].GetComponent<RectTransform>().SetAsLastSibling();*/
/*        Director_Fortress.Director_Init_MercenaryUpgrade();
        Director_Fortress.Director_Init_MercenaryPosition();*/
        if (Station_ItemData.itemChangeFlag)
        {
            Director_Store.Director_Init_ItemSell();
            Director_Inventory.Director_Init_Inventory();
            //Director_GameStart.Director_Init_EquipItem();
            if (simplestationFlag)
            {
                Director_PlayerReadyDirector.Check_Item();
            }
            Station_ItemData.itemChangeFlag = false;
        }
    }

    public void Click_HelpOpen(int x)
    {
        UI_HelpWindow[x].SetActive(true);
        Help_Flag = true;
    }

    public void Click_HelpClose(int x)
    {
        UI_HelpWindow[x].SetActive(false);
        Help_Flag = false;
    }

/*    void ShowPerviousBackGround()
    {
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
            Store_Fortress_Content.anchoredPosition = new Vector2(newX, 0);
            yield return null;
        }

        Store_Fortress_Content.anchoredPosition = new Vector2(-targetX, 0);
        isMoving = false;
    }*/

    public void activeNotice(int i, bool flag)
    {
        GameNotice[i].SetActive(flag);
    }

    public void BuySoundSFX(bool flag)
    {
        if (flag)
        {
            MMSoundManagerSoundPlayEvent.Trigger(BuySFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        }
        else
        {
            MMSoundManagerSoundPlayEvent.Trigger(ErrorSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        }
    }
}