using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using DG.Tweening;
using System.Linq;
using UnityEngine.Localization;


public class Station_TrainMaintenance : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Player_DataObject;
    Station_PlayerData playerData;

    public GameObject Item_DataObject;
    Station_ItemData itemData;

    [Header("UI에서 나타나는 기차")]
    public Transform UI_TrainList;
    public Transform UI_TrainButtonList;
    public GameObject[] Train_Button;

    public int UI_Train_Num;
    int UI_Init_Train_Turret_Num;
    public int UI_Init_Train_Booster_Num;
    int UI_Train_Turret_Num;
    int UI_Train_Booster_Num;
    bool UI_Train_Turret_Flag;
    bool UI_Train_Booster_Flag;
    public TextMeshProUGUI UI_TrainLevel_Text;
    public TextMeshProUGUI UI_TrainMax_Text;

    [Header("기차 변경 윈도우")]
    public Button Change_Button;
    public Button Add_Button;
    [SerializeField]
    List<int> Train_Change_Num;
    public ScrollRect ScrollRect_ChangeTrain;
    public Transform Train_Change_Content;
    public GameObject Train_Card;
    int Max_Train;
    [SerializeField]
    List<Toggle> Train_Toggle;
    int Toggle_Train_Num;
    bool ChangeFlag;
    public TextMeshProUGUI Cost_Add_Text;

    [Header("파츠 변경 윈도우")]
    public List<int> Train_Turret_Part_Change_Num;
    public List<int> Train_Booster_Part_Change_Num;
    public ScrollRect ScrollRect_Turret_Part;
    public ScrollRect ScrollRect_Booster_Part;
    public GameObject UI_Train_Part_Window;
    public LocalizeStringEvent UI_Train_Part_Text;
    public GameObject Part_Card;
    public Transform Turret_Part_Content;
    public Transform Booster_Part_Content;
    public GameObject Part_Ban;
    int Toggle_Turret_Part_Num;
    int Toggle_Booster_Part_Num;
    [SerializeField]
    List<Toggle> Turret_Part_Toggle;
    [SerializeField]
    List<Toggle> Booster_Part_Toggle;
    public Button Part_Change_Button;
    bool Equip_Part_Flag;
    public bool Part_Window_Flag;
    
/*    [Header("기차 업그레이드 윈도우")]
    public TextMeshProUGUI Before_Text;
    public TextMeshProUGUI After_Text;
    public Button Upgrade_Button;
    public Image Material_Image;
    public TextMeshProUGUI[] Upgrade_Text; // 0 : Coin, 1 : 가지고 있는 재료템, 2: 재료 충족 조건*/

    [Header("기차 구매")]
    public TMP_Dropdown TrainBuy_DropDown;
    int List_TrainType_Num;
    int List_Before_TrainType_Num;
    int Train_Buy_Num;
    int Train_Before_Buy_Num;
    public int[] CommonTrain_NumberArray;
    public int[] TurretTrain_NumberArray;
    public int[] BoosterTrain_NumberArray;
    Sprite[] CommonTrain_Image;
    Sprite[] TurretTrain_Image;
    Sprite[] BoosterTrain_Image;
    public LocalizeStringEvent Train_Name_Buy_Text;
    public LocalizeStringEvent Train_Information_Buy_Text;
    public TextMeshProUGUI Train_Pride_Text;
    public Image Train_MainImage;
    public Image Train_NextImage_1;
    public Image Train_NextImage_2;
    public Button Train_BuyButton;
    public GameObject TrainPart_Lock_Object;

    [Header("기차 구매 - 트레인 정보")]
    public float MaxHP;
    public float MaxWeight;
    public float MaxArmor;
    public Slider Slider_Buy_HP;
    public Slider Slider_Buy_Weight;
    public Slider Slider_Buy_Armor;

    [Header("기차 구매 - 트레인 리스트")]
    public Transform CommonTrain_List_Transform;
    public Transform TurretTrain_List_Transform;
    public Transform BoosterTrain_List_Transform;
    public GameObject Train_List_Button;

    [Header("기차 업그레이드")]
    public SA_TrainData sa_trainData;
    public SA_TrainTurretData sa_trainturretData;
    public SA_TrainBoosterData sa_trainboosterData;
    int Train_Upgrade_Num1;
    int Train_Upgrade_Num2;
    public GameObject UpgradeWindow;
    public LocalizeStringEvent Train_Name_Upgrade_Text;
    public LocalizeStringEvent Train_Information_Upgrade_Text;
    public Sprite[] Level_Sprite;
    public Image Before_LevelImage;
    public Image After_LevelImage;
    public Image Train_MainImage_Upgrade;
    public Button Train_Upgrade_Button;
    public TextMeshProUGUI Train_Upgrade_CostText;
    int TrainUpgrade_trainLevel;
    int TrainUpgrade_cost;

    [Header("기차 업그레이드 - 트레인 정보")]
    public Slider[] Slider_Upgrade_Before_HP;
    public Slider[] Slider_Upgrade_Before_Weight;
    public Slider[] Slider_Upgrade_Before_Armor;
    public Slider Slider_Upgrade_After_HP;
    public Slider Slider_Upgrade_After_Weight;
    public Slider Slider_Upgrade_After_Armor;
    public TextMeshProUGUI Plus_HP_Text;
    public TextMeshProUGUI Plus_Weight_Text;
    public TextMeshProUGUI Plus_Armor_Text;

    [Header("기차 업그레이드 - 트레인 리스트")]
    public TMP_Dropdown TrainUpgradeList_DropDown;
    int TrainUpgradeList_BeforeNum;
    public GameObject TrainUpgradeList_Button_Object;

    public Transform[] TrainUgpradeList_Content;

    [Header("패시브 업그레이드")]
    public TextMeshProUGUI[] Passive_Level_Text;
    public TextMeshProUGUI[] Passive_Cost_Text;
    public Button[] Passive_Button;

    [Header("공통")]
    public GameObject Warning_Coin_Window;
    public SA_LocalData localData;
    int local_Index;
    [SerializeField]
    LocalizedString[] LocalString_TrainType;

    private void Start()
    {
        Setting_TrainImage();
        Setting_TrainType_DropDown_Buy();
        Setting_TrainUpgarde();
        Setting_TrainUpgradeList_Button();
        Setting_TrainType_DropDown_Upgrade();
        DropDown_Option_Change();

        local_Index = localData.Local_Index;

        Part_Window_Flag = false;
        UI_Train_Num = 0;
        UI_Init_Train_Turret_Num = 0;
        UI_Init_Train_Booster_Num = 0;
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        itemData = Item_DataObject.GetComponent<Station_ItemData>();
        Max_Train = trainData.Level_Train_MaxTrain + 2;
        UI_TrainMax_Text.text = "Train MAX : " + Max_Train;
/*        Train_Change_Num = trainData.Train_Change_Num;
        Train_Turret_Part_Change_Num = trainData.Train_Turret_Part_Change_Num;
        Train_Booster_Part_Change_Num = trainData.Train_Booster_Part_Change_Num;*/
        UI_Train_Part_Text.StringReference.TableReference = "ExcelData_Table_St";
        //UI 기차 생성하기
        UI_TrainImage(false);
        Current_Train_Information();

        //기차 변경하기
        Check_Init_TrainCard();
        Director_Init_TrainChange();
        //파츠 변경하기
        Check_Init_TrainTurretPartCard();
        Check_Init_TrainBoosterPartCard();
        Director_Init_TrainPartChange();
        //토글 Init
        Train_ToggleStart();
        Turret_Part_ToggleStart();
        Booster_Part_ToggleStart();
        //기차 업그레이드
        //Upgrade_Before_After_Text();

        //기차 구매하기
        List_TrainType_Num = 0;
        Init_TrainBuy();
        Train_Name_Buy_Text.StringReference.TableReference = "ExcelData_Table_St";
        Train_Information_Buy_Text.StringReference.TableReference = "ExcelData_Table_St";

        //기차 업그레이드

        Train_Name_Upgrade_Text.StringReference.TableReference = "ExcelData_Table_St";
        Train_Information_Upgrade_Text.StringReference.TableReference = "ExcelData_Table_St";

        //패시브 업그레이드
        for (int i = 0; i < 6; i++)
        {
            Passive_Upgrade_Text(i);
        }
    }

    private void Update()
    {
        if(local_Index != localData.Local_Index)
        {
            DropDown_Option_Change();
            local_Index = localData.Local_Index;
        }
    }

    //UI 기차 생성하기
    private void UI_TrainImage(bool Add)
    {
        int num = 0;
        if (!Add)
        {
            GameObject train;
            foreach (int trainNum in trainData.Train_Num)
            {
                if(trainNum == 51)
                {
                    train = Instantiate(Resources.Load<GameObject>("TrainObject_UI/51_" + trainData.SA_TrainTurretData.Train_Turret_Num[UI_Init_Train_Turret_Num]), UI_TrainList);
                    UI_Init_Train_Turret_Num++;
                }
                else if(trainNum == 52)
                {
                    train = Instantiate(Resources.Load<GameObject>("TrainObject_UI/52_" + trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Init_Train_Booster_Num]), UI_TrainList);
                    UI_Init_Train_Booster_Num++;
                }
                else
                {
                    train = Instantiate(Resources.Load<GameObject>("TrainObject_UI/" + trainNum), UI_TrainList);
                }
                 
                train.name = train.name.Replace("(Clone)", "");
                if (num != 0) // 처음에만 실행
                {
                    train.SetActive(false);
                }

                if (num == 0)
                {
                    GameObject trainButton = Instantiate(Train_Button[0], UI_TrainButtonList);
                    int ButtonNum = num;
                    trainButton.GetComponent<Button>().onClick.AddListener(() => Button_TrainNum(ButtonNum + 1));
                    trainButton.GetComponent<Station_Maintenance_TrainNum_Button>().ChangeNumSprite(ButtonNum);
                }
                else if (num == trainData.Train_Num.Count - 1)
                {
                    GameObject trainButton = Instantiate(Train_Button[2], UI_TrainButtonList);
                    int ButtonNum = num;
                    trainButton.GetComponent<Button>().onClick.AddListener(() => Button_TrainNum(ButtonNum + 1));
                    trainButton.GetComponent<Station_Maintenance_TrainNum_Button>().ChangeNumSprite(ButtonNum);
                }
                else
                {
                    GameObject trainButton = Instantiate(Train_Button[1], UI_TrainButtonList);
                    int ButtonNum = num;
                    trainButton.GetComponent<Button>().onClick.AddListener(() => Button_TrainNum(ButtonNum + 1));
                    trainButton.GetComponent<Station_Maintenance_TrainNum_Button>().ChangeNumSprite(ButtonNum);
                }
                num++;
            }
            UI_TrainButtonList.GetChild(0).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(true);
        }
        else
        {
            num = UI_TrainButtonList.childCount - 1;
            Destroy(UI_TrainButtonList.GetChild(UI_TrainButtonList.childCount - 1).gameObject);
            GameObject trainButton = Instantiate(Train_Button[1], UI_TrainButtonList);
            int ButtonNum = num;
            trainButton.GetComponent<Button>().onClick.AddListener(() => Button_TrainNum(ButtonNum));
            trainButton.GetComponent<Station_Maintenance_TrainNum_Button>().ChangeNumSprite(ButtonNum);
            num++;
            
            trainButton = Instantiate(Train_Button[2], UI_TrainButtonList);
            ButtonNum = num;
            trainButton.GetComponent<Button>().onClick.AddListener(() => Button_TrainNum(ButtonNum + 1));
            trainButton.GetComponent<Station_Maintenance_TrainNum_Button>().ChangeNumSprite(ButtonNum);

            int beforeNum = UI_Train_Num;
            UI_TrainButtonList.GetChild(beforeNum).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(false);
            UI_TrainButtonList.GetChild(UI_TrainButtonList.childCount-1).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(true);
        }
    }
    public void Button_Train_LR(bool flag)
    {
        int beforeNum = UI_Train_Num;
        if (flag) //left
        {
            if (UI_Train_Num + 1 > UI_TrainList.childCount - 1)
            {
                UI_Train_Num = UI_TrainList.childCount - 1;
            }
            else
            {
                UI_Train_Num++;
            }
        }
        else //right
        {
            if (UI_Train_Num - 1 < 0)
            {
                UI_Train_Num = 0;
            }
            else
            {
                UI_Train_Num--;
            }
        }
        UI_TrainList.GetChild(beforeNum).gameObject.SetActive(false);
        UI_TrainList.GetChild(UI_Train_Num).gameObject.SetActive(true);
        UI_TrainButtonList.GetChild(beforeNum).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(false);
        UI_TrainButtonList.GetChild(UI_Train_Num).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(true);
        Check_Change_Button_Interactable();
        Check_Part_Flag();
        //Check_Upgrade_Button_Interactable();
        Current_Train_Information();
        //업그레이드 부분도 포함
        //Upgrade_Before_After_Text();
    } //버튼에 참조

    public void UI_TrainLevel()
    {
        if (trainData.Train_Num[UI_Train_Num] == 51)
        {
            Info_Train_Turret_Part train = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]];

            UI_TrainLevel_Text.text =
               "  Lv." + (trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num] + 1) % 10;
              
        }
        else if (trainData.Train_Num[UI_Train_Num] == 52)
        {
            Info_Train_Booster_Part train = trainData.EX_Game_Data.Information_Train_Booster_Part[trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]];

            UI_TrainLevel_Text.text =
                " Lv." + (trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num] + 1) % 10;
        }
        else
        {
            Info_Train train = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]];

            UI_TrainLevel_Text.text =
                "  Lv." + (trainData.Train_Num[UI_Train_Num] + 1) % 10;
        }
    }


    void Button_TrainNum(int Num)
    {
        int beforeNum = UI_Train_Num;
        UI_Train_Num = Num - 1;
        UI_TrainList.GetChild(beforeNum).gameObject.SetActive(false);
        UI_TrainList.GetChild(UI_Train_Num).gameObject.SetActive(true);
        UI_TrainButtonList.GetChild(beforeNum).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(false);
        UI_TrainButtonList.GetChild(UI_Train_Num).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(true);
        Check_Change_Button_Interactable();
        Check_Part_Flag();
        //Check_Upgrade_Button_Interactable();
        //Upgrade_Before_After_Text();
    }

    public void Current_Train_Information()
    {
        string[] name = UI_TrainList.GetChild(UI_Train_Num).name.Split('_');
        int trainNum = int.Parse(name[0]);
        int trainNum2;
        if (name.Length > 1)
        {
            trainNum2 = int.Parse(name[1]);
        }
        else
        {
            trainNum2 = 0;
        }

        if (trainNum == 51)
        {
           // UI_Train_Information_Text.StringReference.TableEntryReference = "Train_Turret_Information_" + (trainNum2 / 10);
           /* UI_Train_Information_Text.text = trainData.EX_Game_Data.Information_Train_Turret_Part[trainNum2].Train_Information.Replace("\\n", "\n")
                + trainData.EX_Game_Data.Information_Train_Turret_Part[trainNum2].Train_Select_Information.Replace("\\n", "\n").Replace("\\t", "\t");*/
        }
        else if (trainNum == 52)
        {
           // UI_Train_Information_Text.StringReference.TableEntryReference = "Train_Booster_Information_" + (trainNum2 / 10);
          /*  UI_Train_Information_Text.text = trainData.EX_Game_Data.Information_Train_Booster_Part[trainNum2].Train_Information.Replace("\\n", "\n")
              + trainData.EX_Game_Data.Information_Train_Booster_Part[trainNum2].Train_Select_Information.Replace("\\n", "\n").Replace("\\t", "\t");*/
        }
        else
        {
/*            if (trainNum < 50)
            {
                UI_Train_Information_Text.StringReference.TableEntryReference = "Train_Information_" + (trainNum / 10);
            }
            else
            {
                UI_Train_Information_Text.StringReference.TableEntryReference = "Train_Information_" + trainNum;
            }*/
            /*  UI_Train_Information_Text.text = trainData.EX_Game_Data.Information_Train[trainNum].Train_Information.Replace("\\n", "\n")
                 + trainData.EX_Game_Data.Information_Train[trainNum].Train_Select_Information.Replace("\\n", "\n").Replace("\\t", "\t");*/
        }
    }

   

    //기차 변경하기
    public void Director_Init_TrainChange()
    {
        //max 체크
        Check_Trian_Add();
        //card 체크
        //Train_Change_Num = trainData.Train_Change_Num;
        if (Train_Change_Content.childCount != Train_Change_Num.Count)
        {
            for (int i = 0; i < Train_Change_Content.childCount; i++)
            {
                Destroy(Train_Change_Content.GetChild(i).gameObject);
                Train_Toggle.Clear(); // 초기화 하지 않으면, 남아있는 메모리 때문에 버그 걸림
            }
            Check_Init_TrainCard();
            Train_ToggleStart();
        }
        ScrollRect_ChangeTrain.normalizedPosition = Vector2.zero;
        //change 체크
        Change_Button.interactable = false;
        for (int i = 0; i < Train_Toggle.Count; i++)
        {
            Train_Toggle[i].isOn = false;
        }
        Cost_Add_Text.text = trainData.EX_Level_Data.Level_Max_EngineTier[trainData.Train_Num.Count].Cost_Add_Train.ToString() + "G";
    } //전체 초기화

    private void Check_Init_TrainCard()
    {
        RectTransform ContentSize = Train_Change_Content.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(-500 + (390 * (Train_Change_Num.Count-1)), 350);
        foreach (int num in Train_Change_Num)
        {
            if(trainData.SA_TrainData.SA_TrainChangeNum(num) == -1)
            {
                Train_Card.GetComponent<TrainMaintenance_Train_Card>().Train_Num = num;
            }
            else
            {
                Train_Card.GetComponent<TrainMaintenance_Train_Card>().Train_Num = trainData.SA_TrainData.SA_TrainChangeNum(num);
            }
            GameObject Card = Instantiate(Train_Card, Train_Change_Content);
            Card.name = num.ToString();
            Train_Toggle.Add(Card.GetComponentInChildren<Toggle>());
        }
    }

    private void Train_ToggleStart()
    {
        foreach (Toggle toggle in Train_Toggle)
        {
            toggle.onValueChanged.AddListener(Train_OnToggleValueChange);
        }
    }

    private void Train_OnToggleValueChange(bool isOn)
    {
        if (isOn) // 하나라도 클릭 되어 있을 경우, 플래그가 켜지는 방식.
        {
            for (int i = 0; i < Train_Toggle.Count; i++)
            {
                if (Train_Toggle[i].isOn)
                {
                    TrainMaintenance_Train_Card Card = Train_Change_Content.GetChild(i).GetComponent<TrainMaintenance_Train_Card>();
                    Toggle_Train_Num = Card.Train_Num;
                }

                ChangeFlag = true;
                Check_Change_Button_Interactable();
                Check_Trian_Add();
            }
        }
        else
        {
            ChangeFlag = false;
            Check_Change_Button_Interactable();
            Check_Trian_Add();
        }
    }

    public void Button_Train_Change()
    {
        Equip_Part_Flag = false;
        if(Toggle_Train_Num == 51)
        {
            if (Train_Turret_Part_Change_Num.Count == 0)
            {
                Ban_Part_Window();
            }
            else
            {
                ScrollRect_Turret_Part.gameObject.SetActive(true);
                ScrollRect_Booster_Part.gameObject.SetActive(false);
                Click_Part_Button();
            }
        }
        else if(Toggle_Train_Num == 52)
        {
            if (Train_Booster_Part_Change_Num.Count == 0)
            {
                Ban_Part_Window();
            }
            else
            {
                ScrollRect_Turret_Part.gameObject.SetActive(false);
                ScrollRect_Booster_Part.gameObject.SetActive(true);
                Click_Part_Button();
            }
        }
        else
        {
            int changeNum = trainData.SA_TrainData.SA_TrainChangeNum(Toggle_Train_Num); // -> Toggle_Train_Num 같은 경우, 0레벨의 기차숫자로 가져오기 때문에, 재수정이 필요.
            trainData.SA_TrainData.SA_Train_Change(UI_Train_Num, changeNum); //임시로 저장
            Destroy(UI_TrainList.GetChild(UI_Train_Num).gameObject);
            GameObject changeTrain = Instantiate(Resources.Load<GameObject>("TrainObject_UI/" + changeNum), UI_TrainList);
            changeTrain.name = Toggle_Train_Num.ToString();
            changeTrain.transform.SetSiblingIndex(UI_Train_Num);
            if (UI_Train_Turret_Flag)
            {
                trainData.SA_TrainTurretData.SA_Train_Turret_Remove(UI_Train_Turret_Num);
            }
            else if (UI_Train_Booster_Flag)
            {
                trainData.SA_TrainBoosterData.SA_Train_Booster_Remove(UI_Train_Booster_Num);
            }
            //Upgrade_Before_After_Text();
            Check_Change_Button_Interactable();
            Check_Part_Flag();
        }
    }

    public void Button_Train_Add()
    {
        Equip_Part_Flag = true;
        int cost = trainData.EX_Level_Data.Level_Max_EngineTier[trainData.Train_Num.Count].Cost_Add_Train;
        if (playerData.Player_Coin >= cost) // 나중에 레벨에 따라 돈 전환
        {
            UI_TrainButtonList.GetChild(UI_Train_Num).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(false); //버튼 체크 해제
            playerData.Player_Buy_Coin(1000); // 구매
            if(Toggle_Train_Num == 51)
            {
                if (Train_Turret_Part_Change_Num.Count == 0)
                {
                    Ban_Part_Window();
                }
                else
                {
                    ScrollRect_Turret_Part.gameObject.SetActive(true);
                    ScrollRect_Booster_Part.gameObject.SetActive(false);
                    Click_Part_Button();
                }
            }
            else if(Toggle_Train_Num == 52)
            {
                if(Train_Booster_Part_Change_Num.Count == 0)
                {
                    Ban_Part_Window();
                }
                else
                {
                    ScrollRect_Turret_Part.gameObject.SetActive(false);
                    ScrollRect_Booster_Part.gameObject.SetActive(true);
                    Click_Part_Button();
                }
            }
            else
            {
                int Train_changeNum = trainData.SA_TrainData.SA_TrainChangeNum(Toggle_Train_Num);
                trainData.SA_TrainData.SA_Train_Add(Train_changeNum);

                UI_TrainList.GetChild(UI_Train_Num).gameObject.SetActive(false);
                UI_Train_Num = UI_TrainList.childCount;
                GameObject NewTrain = Instantiate(Resources.Load<GameObject>("TrainObject_UI/" + Train_changeNum), UI_TrainList);
                NewTrain.name = NewTrain.name.Replace("(Clone)", "");
                UI_TrainImage(true);
                Check_Trian_Add();
                ////Upgrade_Before_After_Text();
                Check_Player_Coin_Point();
                Check_Part_Flag();
            }
        }
        else
        {
            Ban_Player_Coin_Point(true);
        }

    }
    public void Click_Part_Button()
    {
        Part_Window_Flag = true;
        UI_Train_Part_Window.SetActive(true);
    }

    public void Click_Part_Back_Button()
    {
        UI_Train_Part_Text.StringReference.TableEntryReference = null;
        UI_Train_Part_Text.GetComponent<TextMeshProUGUI>().text = "";
        //UI_Train_Part_Text.text = "<size=50>파츠 정보</size>";
        if (Toggle_Train_Num == 51)
        {
            foreach(Toggle toggle in Turret_Part_Toggle)
            {
                if (toggle.isOn)
                {
                    toggle.isOn = false;
                }
            }
        }else if(Toggle_Train_Num == 52)
        {
            foreach (Toggle toggle in Booster_Part_Toggle)
            {
                if (toggle.isOn)
                {
                    toggle.isOn = false;
                }
            }
        }
        Part_Window_Flag = false;
        Part_Change_Button.interactable = false;
        UI_Train_Part_Window.SetActive(false);
    }

    private void Check_Trian_Add()
    {
        Max_Train = trainData.Level_Train_MaxTrain + 2;
        UI_TrainMax_Text.text = "Train MAX : " + Max_Train;
        Cost_Add_Text.text = trainData.EX_Level_Data.Level_Max_EngineTier[trainData.Train_Num.Count].Cost_Add_Train.ToString() + "G";
        if (ChangeFlag)
        {
            if(Toggle_Train_Num == 52 && trainData.Train_Num.Contains(52))
            {
                Add_Button.interactable = false;
            }
            else
            {
                Add_Button.interactable = (trainData.Train_Num.Count < Max_Train) ? true : false;
            }
        }
        else
        {
            Add_Button.interactable = false;
        }
    }

    private void Check_Change_Button_Interactable()
    {
        if (UI_Train_Num == 0)
        {
            Change_Button.interactable = (ChangeFlag) ? false : false;
        }
        else
        {
            if(UI_TrainList.GetChild(UI_Train_Num).name == Toggle_Train_Num.ToString())
            {
                Change_Button.interactable = (ChangeFlag) ? false : false;
            }
            else
            {
                if(Toggle_Train_Num == 52 && trainData.Train_Num.Contains(52))
                {
                    if(trainData.SA_TrainData.Train_Num[UI_Train_Num] == 52)
                    {
                        Change_Button.interactable = true;
                    }
                    else
                    {
                        Change_Button.interactable = false;
                    }
                }
                else
                {
                    Change_Button.interactable = (ChangeFlag) ? true : false;
                }
            }
        }
    }

    private void Check_Part_Flag()
    {
        string[] PartNum = UI_TrainList.GetChild(UI_Train_Num).name.Split("_");
        if (PartNum[0].Equals("51"))
        {
            UI_Train_Booster_Flag = false;
            UI_Train_Turret_Flag = true;
            UI_Train_Turret_Num = 0;
            for (int i = 0; i < UI_Train_Num; i++)
            {
                if (trainData.Train_Num[i] == 51)
                {
                    UI_Train_Turret_Num++;
                }
            }
        }
        else if(PartNum[0].Equals("52"))
        {
            UI_Train_Booster_Flag = true;
            UI_Train_Turret_Flag = false;
            UI_Train_Booster_Num = 0;
            for(int i = 0; i < UI_Train_Num; i++)
            {
                if (trainData.Train_Num[i] == 52)
                {
                    UI_Train_Booster_Num++;
                }
            }
        }
        else
        {
            UI_Train_Turret_Flag = false;
            UI_Train_Booster_Flag = false;
        }
    }

    private void Check_Upgrade_Button_TrainChange()
    {
        for (int i = 0; i < Train_Change_Content.childCount; i++)
        {
            Destroy(Train_Change_Content.GetChild(i).gameObject);
            Train_Toggle.Clear(); // 초기화 하지 않으면, 남아있는 메모리 때문에 버그 걸림
        }
        Check_Init_TrainCard();
        Train_ToggleStart();
    }
    //파츠 변경하기
    public void Director_Init_TrainPartChange()
    {
        //Train_Turret_Part_Change_Num = trainData.Train_Turret_Part_Change_Num;
        //Train_Booster_Part_Change_Num = trainData.Train_Booster_Part_Change_Num;
        if(Turret_Part_Content.childCount != Train_Turret_Part_Change_Num.Count)
        {
            for(int i = 0; i < Turret_Part_Content.childCount; i++)
            {
                Destroy(Turret_Part_Content.GetChild(i).gameObject);
                Turret_Part_Toggle.Clear();
            }
            Check_Init_TrainTurretPartCard();
            Turret_Part_ToggleStart();
        }
        if (Booster_Part_Content.childCount != Train_Booster_Part_Change_Num.Count)
        {
            for (int i = 0; i < Booster_Part_Content.childCount; i++)
            {
                Destroy(Booster_Part_Content.GetChild(i).gameObject);
                Booster_Part_Toggle.Clear();
            }
            Check_Init_TrainBoosterPartCard();
            Booster_Part_ToggleStart();
        }

        ScrollRect_Turret_Part.normalizedPosition = Vector2.right;
        ScrollRect_Booster_Part.normalizedPosition = Vector2.right;
        for (int i = 0; i < Turret_Part_Toggle.Count; i++)
        {
            Turret_Part_Toggle[i].isOn = false;
        }

        for(int i = 0; i < Booster_Part_Toggle.Count; i++)
        {
            Booster_Part_Toggle[i].isOn = false;       
        }
    }

    private void Check_Init_TrainTurretPartCard()
    {
        RectTransform ContentSize = Turret_Part_Content.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(-1750 + ( 420 * Train_Turret_Part_Change_Num.Count), 0);
        foreach(int num in Train_Turret_Part_Change_Num)
        {
            if(trainData.SA_TrainTurretData.SA_Train_Turret_ChangeNum(num) == -1)
            {
                Part_Card.GetComponent<Train_Part_Card>().Train_Type_Num = 51;
                Part_Card.GetComponent<Train_Part_Card>().Train_Part_Num = num;
            }
            else
            {
                Part_Card.GetComponent<Train_Part_Card>().Train_Type_Num = 51;
                Part_Card.GetComponent<Train_Part_Card>().Train_Part_Num = trainData.SA_TrainTurretData.SA_Train_Turret_ChangeNum(num);
            }
            GameObject Card = Instantiate(Part_Card, Turret_Part_Content);
            Card.name = "51_" + num;
            Turret_Part_Toggle.Add(Card.GetComponentInChildren<Toggle>());
        }
    }

    private void Check_Init_TrainBoosterPartCard()
    {
        RectTransform ContentSize = Booster_Part_Content.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(-1750 + (420 * Train_Turret_Part_Change_Num.Count), 0);
        foreach (int num in Train_Booster_Part_Change_Num)
        {
            if (trainData.SA_TrainBoosterData.SA_Train_Booster_ChangeNum(num) == -1)
            {
                Part_Card.GetComponent<Train_Part_Card>().Train_Type_Num = 52;
                Part_Card.GetComponent<Train_Part_Card>().Train_Part_Num = num;
            }
            else
            {
                Part_Card.GetComponent<Train_Part_Card>().Train_Type_Num = 52;
                Part_Card.GetComponent<Train_Part_Card>().Train_Part_Num = trainData.SA_TrainBoosterData.SA_Train_Booster_ChangeNum(num);
            }
            GameObject Card = Instantiate(Part_Card, Booster_Part_Content);
            Card.name = "52_" + num;
            Booster_Part_Toggle.Add(Card.GetComponentInChildren<Toggle>());
        }
    }

    private void Turret_Part_ToggleStart()
    {
        foreach (Toggle toggle in Turret_Part_Toggle)
        {
            toggle.onValueChanged.AddListener(Turret_Part_OnToggleValueChange);
        }
    }

    private void Booster_Part_ToggleStart()
    {
        foreach (Toggle toggle in Booster_Part_Toggle)
        {
            toggle.onValueChanged.AddListener(Booster_Part_OnToggleValueChange);
        }
    }

    private void Turret_Part_OnToggleValueChange(bool isOn)
    {
        if (isOn)
        {
            for(int i = 0; i < Turret_Part_Toggle.Count; i++)
            {
                if (Turret_Part_Toggle[i].isOn)
                {
                    Train_Part_Card Card = Turret_Part_Content.GetChild(i).GetComponent<Train_Part_Card>();
                    Toggle_Turret_Part_Num = Card.Train_Part_Num;
                    UI_Train_Part_Text.StringReference.TableEntryReference = "Train_Turret_Information_" + (Toggle_Turret_Part_Num / 10);
                    //UI_Train_Part_Text.text = "<size=50>파츠 정보</size>\n\n" + trainData.EX_Game_Data.Information_Train_Turret_Part[Toggle_Turret_Part_Num].Train_Information.Replace("\\n", "\n");
                    //텍스트
                }
                Part_Change_Button.interactable = true;
            }
        }
        else
        {
            Part_Change_Button.interactable = false;
        }
    }

    private void Booster_Part_OnToggleValueChange(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < Booster_Part_Toggle.Count; i++)
            {
                if (Booster_Part_Toggle[i].isOn)
                {
                    Train_Part_Card Card = Booster_Part_Content.GetChild(i).GetComponent<Train_Part_Card>();
                    Toggle_Booster_Part_Num = Card.Train_Part_Num;
                    UI_Train_Part_Text.StringReference.TableEntryReference = "Train_Booster_Information_" + (Toggle_Booster_Part_Num / 10);
                    //UI_Train_Part_Text.text = "<size=50>파츠 정보</size>\n\n" + trainData.EX_Game_Data.Information_Train_Booster_Part[Toggle_Booster_Part_Num].Train_Information.Replace("\\n", "\n");
                }
            }
            Part_Change_Button.interactable = true;
        }
        else
        {
            Part_Change_Button.interactable = false;
        }
    }

    public void Button_Equip_Part() // 참이면 추가, 불이면 교체
    {
        if (Equip_Part_Flag) // 기차 추가
        {
            if(Toggle_Train_Num == 51)
            {
                int TurretPart_changeNum = trainData.SA_TrainTurretData.SA_Train_Turret_ChangeNum(Toggle_Turret_Part_Num);
                trainData.SA_TrainData.SA_Train_Add(51);
                trainData.SA_TrainTurretData.SA_Train_Turret_Add(TurretPart_changeNum);

                UI_TrainList.GetChild(UI_Train_Num).gameObject.SetActive(false);
                UI_Train_Num = UI_TrainList.childCount;
                GameObject NewTrain = Instantiate(Resources.Load<GameObject>("TrainObject_UI/51_" + TurretPart_changeNum), UI_TrainList);
                NewTrain.name = NewTrain.name.Replace("(Clone)", "");
                UI_TrainImage(true);
                ////Upgrade_Before_After_Text();
                Check_Player_Coin_Point();
                Check_Part_Flag();
                Check_Trian_Add();
                Check_Change_Button_Interactable();
                Click_Part_Back_Button();
            }
            else if(Toggle_Train_Num == 52)
            {
                int BoosterPart_changeNum = trainData.SA_TrainBoosterData.SA_Train_Booster_ChangeNum(Toggle_Booster_Part_Num);
                trainData.SA_TrainData.SA_Train_Add(52);
                trainData.SA_TrainBoosterData.SA_Train_Booster_Add(BoosterPart_changeNum);

                UI_TrainList.GetChild(UI_Train_Num).gameObject.SetActive(false);
                UI_Train_Num = UI_TrainList.childCount;
                GameObject NewTrain = Instantiate(Resources.Load<GameObject>("TrainObject_UI/52_" + BoosterPart_changeNum), UI_TrainList);
                NewTrain.name = NewTrain.name.Replace("(Clone)", "");
                UI_TrainImage(true);
                ////Upgrade_Before_After_Text();
                Check_Player_Coin_Point();
                Check_Part_Flag();
                Check_Trian_Add();
                Check_Change_Button_Interactable();
                Click_Part_Back_Button();
            }
        }
        else // 기차 교체
        {
            if (Toggle_Train_Num == 51)
            {
                int TurretPart_changeNum = trainData.SA_TrainTurretData.SA_Train_Turret_ChangeNum(Toggle_Turret_Part_Num);

                if (trainData.SA_TrainData.Train_Num[UI_Train_Num] == 51)
                {
                    trainData.SA_TrainTurretData.SA_Train_Turret_Change(UI_Train_Turret_Num, TurretPart_changeNum); //-> 교체할 때, 적용
                }
                else if (trainData.SA_TrainData.Train_Num[UI_Train_Num] == 52)
                {
                    trainData.SA_TrainData.SA_Train_Change(UI_Train_Num, 51);
                    trainData.SA_TrainTurretData.SA_Train_Turret_Insert(UI_Train_Turret_Num, TurretPart_changeNum);
                    trainData.SA_TrainBoosterData.SA_Train_Booster_Remove(UI_Train_Booster_Num);
                }
                else
                {
                    trainData.SA_TrainData.SA_Train_Change(UI_Train_Num, 51);
                    trainData.SA_TrainTurretData.SA_Train_Turret_Insert(UI_Train_Turret_Num, TurretPart_changeNum);
                }

                /* Debug.Log("작동_Turret");
                 Debug.Log(UI_Train_Turret_Flag);
                 Debug.Log(UI_Train_Booster_Flag);

                 if (!UI_Train_Turret_Flag)
                 {  //일반기차 -> 파츠 기차로 교체 
                     trainData.SA_TrainData.SA_Train_Change(UI_Train_Num, 51);
                     trainData.SA_TrainTurretData.SA_Train_Turret_Insert(UI_Train_Turret_Num, TurretPart_changeNum);
                 }
                 else // 파츠 기차에서 파츠만 교체할 때 ( 같은 파츠끼리 )
                 {
                     if (trainData.SA_TrainData.Train_Num[UI_Train_Num] == 51)
                     {
                         Debug.Log("같은 파츠_터렛");
                         trainData.SA_TrainTurretData.SA_Train_Turret_Change(UI_Train_Turret_Num, TurretPart_changeNum); //-> 교체할 때, 적용
                     }
                     else
                     {
                         Debug.Log("다른 파츠_터렛");
                         trainData.SA_TrainData.SA_Train_Change(UI_Train_Num, 51);
                         trainData.SA_TrainBoosterData.SA_Train_Booster_Remove(UI_Train_Booster_Num);
                     }
                 }*/
                Destroy(UI_TrainList.GetChild(UI_Train_Num).gameObject);
                GameObject changeTrain = Instantiate(Resources.Load<GameObject>("TrainObject_UI/51_" + TurretPart_changeNum), UI_TrainList);
                changeTrain.name = changeTrain.name.Replace("(Clone)", "");
                changeTrain.transform.SetSiblingIndex(UI_Train_Num);
                ////Upgrade_Before_After_Text();
                Check_Change_Button_Interactable();
                Click_Part_Back_Button();
                Check_Part_Flag();
            }
            else if (Toggle_Train_Num == 52)
            {
                int BoosterPart_changeNum = trainData.SA_TrainBoosterData.SA_Train_Booster_ChangeNum(Toggle_Booster_Part_Num);

                if (trainData.SA_TrainData.Train_Num[UI_Train_Num] == 52)
                {
                    trainData.SA_TrainBoosterData.SA_Train_Booster_Change(UI_Train_Turret_Num, BoosterPart_changeNum); //-> 교체할 때, 적용
                }
                else if (trainData.SA_TrainData.Train_Num[UI_Train_Num] == 51)
                {
                    trainData.SA_TrainData.SA_Train_Change(UI_Train_Num, 52);
                    trainData.SA_TrainBoosterData.SA_Train_Booster_Insert(UI_Train_Booster_Num, BoosterPart_changeNum);
                    trainData.SA_TrainTurretData.SA_Train_Turret_Remove(UI_Train_Turret_Num);
                }
                else
                {
                    trainData.SA_TrainData.SA_Train_Change(UI_Train_Num, 52);
                    trainData.SA_TrainBoosterData.SA_Train_Booster_Insert(UI_Train_Booster_Num, BoosterPart_changeNum);
                }


                /*  Debug.Log("작동_Booster");
                  Debug.Log(UI_Train_Turret_Flag);
                  Debug.Log(UI_Train_Booster_Flag);
                  if (!UI_Train_Booster_Flag)
                  {  //일반기차 -> 파츠 기차로 교체 
                      trainData.SA_TrainData.SA_Train_Change(UI_Train_Num, 52);
                      trainData.SA_TrainBoosterData.SA_Train_Booster_Insert(UI_Train_Booster_Num, BoosterPart_changeNum);
                  }
                  else // 파츠 기차에서 파츠만 교체할 때( 같은 파츠끼리 )
                  {
                      if (trainData.SA_TrainData.Train_Num[UI_Train_Num] == 52)
                      {
                          Debug.Log("같은 파츠_부스터");
                          trainData.SA_TrainBoosterData.SA_Train_Booster_Change(UI_Train_Booster_Num, BoosterPart_changeNum); //-> 교체할 때, 적용
                      }
                      else
                      {
                          Debug.Log("다른 파츠_부스터");
                          trainData.SA_TrainData.SA_Train_Change(UI_Train_Num, 52);
                          trainData.SA_TrainTurretData.SA_Train_Turret_Remove(UI_Train_Turret_Num);
                      }

                  }*/
                Destroy(UI_TrainList.GetChild(UI_Train_Num).gameObject);
                GameObject changeTrain = Instantiate(Resources.Load<GameObject>("TrainObject_UI/52_" + BoosterPart_changeNum), UI_TrainList);
                changeTrain.name = changeTrain.name.Replace("(Clone)", "");
                changeTrain.transform.SetSiblingIndex(UI_Train_Num);
                ////Upgrade_Before_After_Text();
                Check_Change_Button_Interactable();
                Click_Part_Back_Button();
                Check_Part_Flag();
            }
        }
    }

    //기차 업그레이드
/*    public void Direcotr_Init_TrainUpgrade()
    {
        Check_Upgrade_Button_Interactable();
        //Upgrade_Before_After_Text();
    }

    public void Click_Button_Upgrade()
    {
        if (trainData.Train_Num[UI_Train_Num] == 51)
        {
            int cost = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]].Train_Upgrade_Cost;
            //int Material_Count = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]].Material;
            if (playerData.Player_Coin >= cost && itemData.Turret_Train_Material_object.Item_Count >= Material_Count)
            {
                playerData.Player_Buy_Coin(cost);
                //itemData.Turret_Train_Material_object.Item_Count_Down(Material_Count);
                //trainData.Train_Turret_Level_Up(trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num], UI_Train_Turret_Num);
                Upgrade_Train_TrainMaintenance(); // UI 기차 변경
                //Upgrade_Before_After_Text(); // 비포 애프터 변경도 하고 기차 옆의 정보도 변경.
                Check_Upgrade_Button_TrainChange(); //기차 변경하기에서도 변경이 된다.
            }
            else
            {
                Ban_Player_Coin_Point(true);
            }
        }
        else if (trainData.Train_Num[UI_Train_Num] == 52)
        {
            int cost = trainData.EX_Game_Data.Information_Train_Booster_Part[trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]].Train_Upgrade_Cost;
            //int Material_Count = trainData.EX_Game_Data.Information_Train_Booster_Part[trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]].Material;
            if (playerData.Player_Coin >= cost && itemData.Booster_Train_Material_object.Item_Count >= Material_Count)
            {
                playerData.Player_Buy_Coin(cost);
                //itemData.Booster_Train_Material_object.Item_Count_Down(Material_Count);
                //trainData.Train_Booster_Level_Up(trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num], UI_Train_Booster_Num);
                Upgrade_Train_TrainMaintenance(); // UI 기차 변경
                //Upgrade_Before_After_Text(); // 비포 애프터 변경도 하고 기차 옆의 정보도 변경.
                Check_Upgrade_Button_TrainChange(); //기차 변경하기에서도 변경이 된다.
            }
            else
            {
                Ban_Player_Coin_Point(true);
            }
        }
        else
        {
            int cost = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Train_Upgrade_Cost;
            //int Material_Count = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Material;
            if (playerData.Player_Coin >= cost && itemData.Common_Train_Material_object.Item_Count >= Material_Count)
            {
                playerData.Player_Buy_Coin(cost);
                //itemData.Common_Train_Material_object.Item_Count_Down(Material_Count);
                //trainData.Train_Level_Up(trainData.Train_Num[UI_Train_Num], UI_Train_Num);
                Upgrade_Train_TrainMaintenance(); // UI 기차 변경
                //Upgrade_Before_After_Text(); // 비포 애프터 변경도 하고 기차 옆의 정보도 변경.
                Check_Upgrade_Button_TrainChange(); //기차 변경하기에서도 변경이 된다.
            }
            else
            {
                Ban_Player_Coin_Point(true);
            }
        }
    }

    public void Upgrade_Before_After_Text()
    {
        if (trainData.Train_Num[UI_Train_Num] == 51)
        {
            Info_Train_Turret_Part train = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]];
            Before_Text.text =
               "  Lv : " + (trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num] + 1) % 10
               + "\nHP : " + train.Train_HP
               + "\nWeight : " + train.Train_Weight
               + "\nArmor : " + train.Train_Armor;


            if (trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num] % 10 < 9)
            {
                train = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num] + 1];
                After_Text.text =
                    "  Lv : " + (trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num] + 2) % 10
                    + "\nHP : <color=red>" + train.Train_HP
                    + "\n</color>Weight : <color=red>" + train.Train_Weight
                    + "\n</color>Armor : <color=red>" + train.Train_Armor;
            }
            else if (trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num] % 10 == 9)
            {
                After_Text.text = "  Max";
            }
            Material_Image.sprite = itemData.Turret_Train_Material_object.Item_Sprite;
            int Material_InChnace_Count = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]].Material;
            int Material_Inventory_Count = itemData.Turret_Train_Material_object.Item_Count;
            if (Material_Inventory_Count >= Material_InChnace_Count)
            {
                Upgrade_Text[1].text = "<color=green>" + Material_Inventory_Count + "</color>";
            }
            else
            {
                Upgrade_Text[1].text = "<color=red>" + Material_Inventory_Count + "</color>";
            }
            Upgrade_Text[2].text = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]].Material.ToString();
        }
        else if (trainData.Train_Num[UI_Train_Num] == 52)
        {
            Info_Train_Booster_Part train = trainData.EX_Game_Data.Information_Train_Booster_Part[trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]];

            Before_Text.text = 
                " Lv : " + (trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num] + 1) % 10
                + "\nHP : " + train.Train_HP
                + "\nWeight : " + train.Train_Weight
                + "\nArmor : " + train.Train_Armor;

            if (trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num] % 10 < 9)
            {
                train = trainData.EX_Game_Data.Information_Train_Booster_Part[trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]];
                After_Text.text =
                    "  Lv : " + (trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num] + 2) % 10
                    + "\nHP : <color=red>" + train.Train_HP
                    + "\n</color>Weight : <color=red>" + train.Train_Weight
                    + "\n</color>Armor : <color=red>" + train.Train_Armor;
            }
            else if (trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num] % 10 == 9)
            {
                After_Text.text = "  Max";
            }

            Material_Image.sprite = itemData.Booster_Train_Material_object.Item_Sprite;
            int Material_InChnace_Count = trainData.EX_Game_Data.Information_Train_Booster_Part[trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]].Material;
            int Material_Inventory_Count = itemData.Booster_Train_Material_object.Item_Count;
            if (Material_Inventory_Count >= Material_InChnace_Count)
            {
                Upgrade_Text[1].text = "<color=green>" + Material_Inventory_Count + "</color>";
            }
            else
            {
                Upgrade_Text[1].text = "<color=red>" + Material_Inventory_Count + "</color>";
            }
            Upgrade_Text[2].text = trainData.EX_Game_Data.Information_Train_Booster_Part[trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]].Material.ToString();
        }
        else
        {
            Info_Train train = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]];

            Before_Text.text =
                "  Lv : " + (trainData.Train_Num[UI_Train_Num] + 1) % 10
                + "\nHP : " + train.Train_HP
                + "\nWeight : " + train.Train_Weight
                + "\nArmor : " + train.Train_Armor;


            if (trainData.Train_Num[UI_Train_Num] % 10 < 9)
            {
                train = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num] + 1];
                After_Text.text =
                    "  Lv : " + (trainData.Train_Num[UI_Train_Num] + 2) % 10
                    + "\nHP : <color=red>" + train.Train_HP
                    + "\n</color>Weight : <color=red>" + train.Train_Weight
                    + "\n</color>Armor : <color=red>" + train.Train_Armor;
            }
            else if (trainData.Train_Num[UI_Train_Num] % 10 == 9)
            {
                After_Text.text = "  Max";
            }
            Material_Image.sprite = itemData.Common_Train_Material_object.Item_Sprite;
            int Material_InChnace_Count = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Material;
            int Material_Inventory_Count = itemData.Common_Train_Material_object.Item_Count;
            if (Material_Inventory_Count >= Material_InChnace_Count)
            {
                Upgrade_Text[1].text = "<color=green>" + Material_Inventory_Count + "</color>";
            }
            else
            {
                Upgrade_Text[1].text = "<color=red>" + Material_Inventory_Count + "</color>";
            }
            Upgrade_Text[2].text = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Material.ToString();
        }
        UI_TrainLevel();
    }

    private void Upgrade_Train_TrainMaintenance()
    {
        int PartNum_Turret = 0;
        int PartNum_Booster = 0;

        for (int i = 0; i < UI_TrainList.childCount; i++)
        {
            Destroy(UI_TrainList.GetChild(i).gameObject);
        }

        int num = 0;
        foreach (int trainNum in trainData.Train_Num)
        {
            if (trainNum == 51)
            {
                GameObject train = Instantiate(Resources.Load<GameObject>("TrainObject_UI/51_" + trainData.SA_TrainTurretData.Train_Turret_Num[PartNum_Turret]), UI_TrainList);
                train.name = train.name.Replace("(Clone)", "");
                if (num != UI_Train_Num)
                {
                    train.gameObject.SetActive(false);
                }
                num++;
                PartNum_Turret++;
            }
            else if (trainNum == 52)
            {
                GameObject train = Instantiate(Resources.Load<GameObject>("TrainObject_UI/52_" + trainData.SA_TrainBoosterData.Train_Booster_Num[PartNum_Booster]), UI_TrainList);
                train.name = train.name.Replace("(Clone)", "");
                if (num != UI_Train_Num)
                {
                    train.gameObject.SetActive(false);
                }
                num++;
                PartNum_Booster++;
            }
            else
            {
                GameObject train = Instantiate(Resources.Load<GameObject>("TrainObject_UI/" + trainNum), UI_TrainList);
                train.name = train.name.Replace("(Clone)", "");
                if (num != UI_Train_Num)
                {
                    train.gameObject.SetActive(false);
                }
                num++;
            }
        }
        Check_Player_Coin_Point();
        Check_Upgrade_Button_Interactable();
    }

    private void Check_Upgrade_Button_Interactable()
    {
        if (trainData.Train_Num[UI_Train_Num] < 50)
        {
            if(trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Train_Upgrade_Cost != -100)
            {
                Upgrade_Text[0].text = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Train_Upgrade_Cost + "G";
                Upgrade_Button.interactable = true;
            }
            else
            {
                Upgrade_Text[0].text = "MAX";
                Upgrade_Button.interactable = false;
            }
        }else if (trainData.Train_Num[UI_Train_Num] == 52)
        {
            if (trainData.EX_Game_Data.Information_Train_Booster_Part[trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]].Train_Upgrade_Cost != -100)
            {
                Upgrade_Text[0].text = trainData.EX_Game_Data.Information_Train_Booster_Part[trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]].Train_Upgrade_Cost + "G";
                Upgrade_Button.interactable = true;
            }
            else
            {
                Upgrade_Text[0].text = "MAX";
                Upgrade_Button.interactable = false;
            }
        }
        else if (trainData.Train_Num[UI_Train_Num]== 51)
        {
            if(trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]].Train_Upgrade_Cost != -100)
            {
                Upgrade_Text[0].text = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]].Train_Upgrade_Cost + "G";
                Upgrade_Button.interactable = true;
            }
            else
            {
                Upgrade_Text[0].text = "MAX";
                Upgrade_Button.interactable = false;
            }
        }


        if (trainData.Train_Num[UI_Train_Num] < 50 || trainData.Train_Num[UI_Train_Num] == 51 || trainData.Train_Num[UI_Train_Num] == 52)
        {
            Upgrade_Button.interactable = true;
        }
        else
        {
            Upgrade_Button.interactable = false;
        }
    }
*/
    private void Check_Player_Coin_Point()
    {
        transform.GetComponentInParent<StationDirector>().Check_Coin();
    }

    private void Ban_Player_Coin_Point(bool Flag)
    {
        transform.GetComponentInParent<StationDirector>().Check_Ban_CoinPoint(Flag);
    }

    public void Ban_Part_Window()
    {
        Part_Ban.SetActive(true);
    }

    public void Ban_Back_Part_Window()
    {
        Part_Ban.SetActive(false);
    }

    //기차 구매하기
    private void Setting_TrainType_DropDown_Buy()
    {
        TrainBuy_DropDown.ClearOptions();
        List<string> optionList = new List<string>();
        TrainBuy_DropDown.onValueChanged.RemoveAllListeners();
        TrainBuy_DropDown.onValueChanged.AddListener(Change_TrainType_DropDown);
        optionList.Add("A");
        optionList.Add("B");
        optionList.Add("C");
        TrainBuy_DropDown.AddOptions(optionList);
        TrainBuy_DropDown.value = 0;
    }

    private void Change_TrainType_DropDown(int value)
    {
        List_Before_TrainType_Num = List_TrainType_Num;
        List_TrainType_Num = value;
        Train_Before_Buy_Num = Train_Buy_Num;
        Train_Buy_Num = 0;
        if (value == 0)
        {
            //TrainPart_Lock_Object.SetActive(false);

            CommonTrain_List_Transform.gameObject.SetActive(true);
            TurretTrain_List_Transform.gameObject.SetActive(false);
            BoosterTrain_List_Transform.gameObject.SetActive(false);
        }else if(value == 1)
        {
            //TrainPart_Lock_Object.SetActive(!trainData.SA_TrainData.Train_Buy_Num.Contains(51));

            CommonTrain_List_Transform.gameObject.SetActive(false);
            TurretTrain_List_Transform.gameObject.SetActive(true);
            BoosterTrain_List_Transform.gameObject.SetActive(false);
        }
        else if(value == 2)
        {
            //TrainPart_Lock_Object.SetActive(!trainData.SA_TrainData.Train_Buy_Num.Contains(52));

            CommonTrain_List_Transform.gameObject.SetActive(false);
            TurretTrain_List_Transform.gameObject.SetActive(false);
            BoosterTrain_List_Transform.gameObject.SetActive(true);
        }
        
        Change_Train();
    }

    private void Setting_TrainImage()
    {
        CommonTrain_Image = new Sprite[CommonTrain_NumberArray.Length];
        TurretTrain_Image = new Sprite[TurretTrain_NumberArray.Length];
        BoosterTrain_Image = new Sprite[BoosterTrain_NumberArray.Length];
        Train_List_Button.GetComponent<TrainList_Button>().director = this;
        for(int i = 0; i <  CommonTrain_NumberArray.Length; i++)
        {
            CommonTrain_Image[i] = Resources.Load<Sprite>("Sprite/Train/Train_" + CommonTrain_NumberArray[i]);
            GameObject listButton = Instantiate(Train_List_Button, CommonTrain_List_Transform);
            listButton.GetComponent<TrainList_Button>().listNum = i;
            listButton.name = CommonTrain_NumberArray[i].ToString();
        }
        for(int i = 0; i < TurretTrain_NumberArray.Length; i++)
        {
            TurretTrain_Image[i] = Resources.Load<Sprite>("Sprite/Train/Train_51_" + TurretTrain_NumberArray[i]);
            GameObject listButton = Instantiate(Train_List_Button, TurretTrain_List_Transform);
            listButton.GetComponent<TrainList_Button>().listNum = i;
            listButton.name = TurretTrain_NumberArray[i].ToString();
        }
        for(int i = 0; i < BoosterTrain_NumberArray.Length; i++)
        {
            BoosterTrain_Image[i] = Resources.Load<Sprite>("Sprite/Train/Train_52_" + BoosterTrain_NumberArray[i]);
            GameObject listButton = Instantiate(Train_List_Button, BoosterTrain_List_Transform);
            listButton.GetComponent<TrainList_Button>().listNum = i;
            listButton.name = BoosterTrain_NumberArray[i].ToString();
        }
    }
    public void Click_TrainList(int listNum)
    {
        List_Before_TrainType_Num = List_TrainType_Num;
        Train_Before_Buy_Num = Train_Buy_Num;
        Train_Buy_Num = listNum;
        Change_Train();
    }

    private void Init_TrainBuy()
    {
        Train_Buy_Num = 0;
        Change_Train();
    }

    public void Click_Next_TrainButton()
    {
        List_Before_TrainType_Num = List_TrainType_Num;
        Train_Before_Buy_Num = Train_Buy_Num;

        int Max = 0;
        if(List_TrainType_Num == 0)
        {
            Max = CommonTrain_NumberArray.Length - 1;
        }
        else if(List_TrainType_Num == 1)
        {
            Max = TurretTrain_NumberArray.Length - 1;
        }
        else if (List_TrainType_Num == 2)
        {
            Max = BoosterTrain_NumberArray.Length - 1;
        }

        if (Train_Buy_Num < Max )
        {
            Train_Buy_Num++;
        }
        else
        {
            Train_Buy_Num = 0;
        }
        Change_Train();
    }

    public void Click_Prev_TrainBUtton()
    {
        List_Before_TrainType_Num = List_TrainType_Num;
        Train_Before_Buy_Num = Train_Buy_Num;

        int Max = 0;
        if (List_TrainType_Num == 0)
        {
            Max = CommonTrain_NumberArray.Length - 1;
        }
        else if (List_TrainType_Num == 1)
        {
            Max = TurretTrain_NumberArray.Length - 1;
        }
        else if (List_TrainType_Num == 2)
        {
            Max = BoosterTrain_NumberArray.Length - 1;
        }

        if (Train_Buy_Num > 0)
        {
            Train_Buy_Num--;
        }
        else
        {
            Train_Buy_Num = Max ;
        }
        Change_Train();
    }

    private void Change_Train()
    {
        int trainNum = 0;
        if(List_TrainType_Num == 0)
        {
            Train_MainImage.sprite = CommonTrain_Image[Train_Buy_Num];
            trainNum = CommonTrain_NumberArray[Train_Buy_Num];
        }
        else if (List_TrainType_Num == 1)
        {
            Train_MainImage.sprite = TurretTrain_Image[Train_Buy_Num];
            trainNum = TurretTrain_NumberArray[Train_Buy_Num];
        }
        else if (List_TrainType_Num == 2)
        {
            Train_MainImage.sprite = BoosterTrain_Image[Train_Buy_Num];
            trainNum = TurretTrain_NumberArray[Train_Buy_Num];
        }

        Check_TrainState_Slider_Buy();
        Change_NextTrianSprite();

        int train_pride;

        if(List_TrainType_Num == 0)
        {
            if (trainNum < 50)
            {
                Train_Name_Buy_Text.StringReference.TableEntryReference = "Train_Name_" + (trainNum / 10);
                Train_Information_Buy_Text.StringReference.TableEntryReference = "Train_Information_" + (trainNum / 10);
            }
            else
            {
                Train_Name_Buy_Text.StringReference.TableEntryReference = "Train_Name_" + trainNum;
                Train_Information_Buy_Text.StringReference.TableEntryReference = "Train_Information_" + trainNum;
            }
            train_pride = trainData.EX_Game_Data.Information_Train[trainNum].Train_Buy_Cost;
            Train_Pride_Text.text = train_pride.ToString();
            Train_BuyButton.interactable = !trainData.SA_TrainData.Train_Buy_Num.Contains(trainNum);
        }
        else if (List_TrainType_Num == 1)
        {
            Train_Name_Buy_Text.StringReference.TableEntryReference = "Train_Turret_Name_" + (trainNum / 10);
            Train_Information_Buy_Text.StringReference.TableEntryReference = "Train_Turret_Information_" + (trainNum / 10);
            train_pride = trainData.EX_Game_Data.Information_Train_Turret_Part[trainNum].Train_Buy_Cost;
            Train_Pride_Text.text = train_pride.ToString();
            Train_BuyButton.interactable = !trainData.SA_TrainTurretData.Train_Turret_Buy_Num.Contains(trainNum);
        }
        else if(List_TrainType_Num == 2)
        {
            Train_Name_Buy_Text.StringReference.TableEntryReference = "Train_Booster_Name_" + (trainNum / 10);
            Train_Information_Buy_Text.StringReference.TableEntryReference = "Train_Booster_Information_" + (trainNum / 10);
            train_pride = trainData.EX_Game_Data.Information_Train_Booster_Part[trainNum].Train_Buy_Cost;
            Train_Pride_Text.text = train_pride.ToString();
            Train_BuyButton.interactable = !trainData.SA_TrainBoosterData.Train_Booster_Buy_Num.Contains(trainNum);
        }

        //-----------리스트버튼-------
        Check_TrainType_ListButton();
    }

    private void Check_TrainType_ListButton()
    {
        if (List_Before_TrainType_Num == 0)
        {
            CommonTrain_List_Transform.GetChild(Train_Before_Buy_Num).GetComponent<TrainList_Button>().ChangeButton(true);
        }
        else if (List_Before_TrainType_Num == 1)
        {
            TurretTrain_List_Transform.GetChild(Train_Before_Buy_Num).GetComponent<TrainList_Button>().ChangeButton(true);
        }
        else if (List_Before_TrainType_Num == 2)
        {
            BoosterTrain_List_Transform.GetChild(Train_Before_Buy_Num).GetComponent<TrainList_Button>().ChangeButton(true);
        }

        if (List_TrainType_Num == 0)
        {
            CommonTrain_List_Transform.GetChild(Train_Buy_Num).GetComponent<TrainList_Button>().ChangeButton(false);
        }
        else if (List_TrainType_Num == 1)
        {
            TurretTrain_List_Transform.GetChild(Train_Buy_Num).GetComponent<TrainList_Button>().ChangeButton(false);
        }
        else if (List_TrainType_Num == 2)
        {
            BoosterTrain_List_Transform.GetChild(Train_Buy_Num).GetComponent<TrainList_Button>().ChangeButton(false);
        }
        List_Before_TrainType_Num =  List_TrainType_Num;
    }

    private void Change_NextTrianSprite()
    {
        int TrainNextNum1 = Train_Buy_Num + 1;
        int TrainNextNum2 = Train_Buy_Num + 2;

        int max = 0;
        if(List_TrainType_Num == 0)
        {
            max = CommonTrain_NumberArray.Length - 1;
        }
        else if(List_TrainType_Num == 1)
        {
            max = TurretTrain_NumberArray.Length - 1;
        }
        else if(List_TrainType_Num == 2)
        {
            max = BoosterTrain_NumberArray.Length - 1;
        }


        if (TrainNextNum2  > max)
        {
            if(TrainNextNum1 > max)
            {
                TrainNextNum1 = 0;
                TrainNextNum2 = 1;
            }
            else
            {
                TrainNextNum2 = 0;
            }
        }


        if (List_TrainType_Num == 0)
        {
            Train_NextImage_1.sprite = CommonTrain_Image[TrainNextNum1];
            Train_NextImage_2.sprite = CommonTrain_Image[TrainNextNum2];
        }
        else if (List_TrainType_Num == 1)
        {
            Train_NextImage_1.sprite = TurretTrain_Image[TrainNextNum1];
            Train_NextImage_2.sprite = TurretTrain_Image[TrainNextNum2];
        }
        else if (List_TrainType_Num == 2)
        {
            Train_NextImage_1.sprite = BoosterTrain_Image[TrainNextNum1];
            Train_NextImage_2.sprite = BoosterTrain_Image[TrainNextNum2];
        }
    }

    public void Click_Buy_Train()
    {
        int TrainNum = 0;
        int cost = 0;
        if (List_TrainType_Num == 0)
        {
            TrainNum = CommonTrain_NumberArray[Train_Buy_Num];
            cost = trainData.EX_Game_Data.Information_Train[TrainNum].Train_Buy_Cost;
            if (playerData.Player_Coin >= cost)
            {
                playerData.Player_Buy_Coin(cost);
                trainData.SA_TrainData.SA_Train_Buy(TrainNum);
               // trainData.Check_Buy_Train(TrainNum);
                Check_Player_Coin_Point();
                Train_BuyButton.interactable = !trainData.SA_TrainData.Train_Buy_Num.Contains(TrainNum);
                Instantiate_AfterTrainBuy(1, TrainNum);
            }
            else
            {
                Open_Warning_Window();
            }

        }
        else if (List_TrainType_Num == 1)
        {
            TrainNum = TurretTrain_NumberArray[Train_Buy_Num];
            cost = trainData.EX_Game_Data.Information_Train_Turret_Part[TrainNum].Train_Buy_Cost;
            if (playerData.Player_Coin >= cost)
            {
                playerData.Player_Buy_Coin(cost);
                trainData.SA_TrainTurretData.SA_Train_Turret_Buy(TrainNum);
                //trainData.Check_Buy_Turret(TrainNum);
                Check_Player_Coin_Point();
                Train_BuyButton.interactable = !trainData.SA_TrainTurretData.Train_Turret_Buy_Num.Contains(TrainNum);
                Instantiate_AfterTrainBuy(2, TrainNum);
            }
            else
            {
                Open_Warning_Window();
            }

        }
        else if (List_TrainType_Num == 2)
        {
            TrainNum = BoosterTrain_NumberArray[Train_Buy_Num];
            cost = trainData.EX_Game_Data.Information_Train_Booster_Part[TrainNum].Train_Buy_Cost;
            if (playerData.Player_Coin >= cost)
            {
                playerData.Player_Buy_Coin(cost);
                trainData.SA_TrainBoosterData.SA_Train_Booster_Buy(TrainNum);
                //trainData.Check_Buy_Booster(TrainNum);
                Check_Player_Coin_Point();
                Train_BuyButton.interactable = !trainData.SA_TrainBoosterData.Train_Booster_Buy_Num.Contains(TrainNum);
                Instantiate_AfterTrainBuy(3, TrainNum);
            }
            else
            {
                Open_Warning_Window();
            }
        }
    }

    private void Check_TrainState_Slider_Buy()
    {
        int TrainNum = 0;
        float EX_HP = 0;
        float EX_Weight = 0;
        float EX_Armor = 0;
        if (List_TrainType_Num == 0)
        {
            TrainNum = CommonTrain_NumberArray[Train_Buy_Num];
            Info_Train trainData_Info = trainData.EX_Game_Data.Information_Train[TrainNum];
            EX_HP = 1 - (trainData_Info.Train_HP / MaxHP);
            EX_Weight = 1 - (trainData_Info.Train_Weight / MaxWeight);
            EX_Armor = 1 - (trainData_Info.Train_Armor / MaxArmor);
        }
        else if(List_TrainType_Num == 1)
        {
            TrainNum = TurretTrain_NumberArray[Train_Buy_Num];
            Info_Train_Turret_Part trainData_Info = trainData.EX_Game_Data.Information_Train_Turret_Part[TrainNum];
            EX_HP = 1 - (trainData_Info.Train_HP / MaxHP);
            EX_Weight = 1 - (trainData_Info.Train_Weight / MaxWeight);
            EX_Armor = 1 - (trainData_Info.Train_Armor / MaxArmor);
        }
        else if(List_TrainType_Num == 2)
        {
            TrainNum = BoosterTrain_NumberArray[Train_Buy_Num];
            Info_Train_Booster_Part trainData_Info = trainData.EX_Game_Data.Information_Train_Booster_Part[TrainNum];
            EX_HP = 1 - (trainData_Info.Train_HP / MaxHP);
            EX_Weight = 1 - (trainData_Info.Train_Weight / MaxWeight);
            EX_Armor = 1 - (trainData_Info.Train_Armor / MaxArmor);
        }

        Slider_Buy_HP.value = EX_HP;
        Slider_Buy_Weight.value = EX_Weight;
        Slider_Buy_Armor.value = EX_Armor;
    }
    //기차 업그레이드

    void Setting_TrainUpgarde()
    {
        UpgradeWindow.SetActive(false);
    }

    void Setting_TrainUpgradeList_Button()
    {
        bool TurretTrain_Flag = false;
        bool BoosterTrain_Flag = false;
        List<int> TrainList = new List<int> { sa_trainData.SA_TrainChangeNum(0), sa_trainData.SA_TrainChangeNum(10) };
        TrainList.AddRange(sa_trainData.Train_Buy_Num);
        TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Director = this;

        foreach (int i in TrainList)
        {
            TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Train_Num = sa_trainData.SA_TrainChangeNum(i);
            Instantiate(TrainUpgradeList_Button_Object, TrainUgpradeList_Content[0]);
            Instantiate(TrainUpgradeList_Button_Object, TrainUgpradeList_Content[1]);
        }

        Resize_Train_List_Content(1);

        TurretTrain_Flag = sa_trainturretData.Train_Turret_Buy_Num.Any();
        BoosterTrain_Flag = sa_trainboosterData.Train_Booster_Buy_Num.Any();

        if (TurretTrain_Flag)
        {
            TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Train_Num = 51;
            TrainList = sa_trainturretData.Train_Turret_Buy_Num;
       
            foreach(int i in TrainList)
            {
                TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Train_Num2 = sa_trainturretData.SA_Train_Turret_ChangeNum(i);
                Instantiate(TrainUpgradeList_Button_Object, TrainUgpradeList_Content[0]);
                Instantiate(TrainUpgradeList_Button_Object, TrainUgpradeList_Content[2]);
            }
            Resize_Train_List_Content(2);
        }

        if (BoosterTrain_Flag)
        {
            TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Train_Num = 52;
            TrainList = sa_trainboosterData.Train_Booster_Buy_Num;

            foreach (int i in TrainList)
            {
                TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Train_Num2 = sa_trainboosterData.SA_Train_Booster_ChangeNum(i);
                Instantiate(TrainUpgradeList_Button_Object, TrainUgpradeList_Content[0]);
                Instantiate(TrainUpgradeList_Button_Object, TrainUgpradeList_Content[3]);
            }
            Resize_Train_List_Content(3);
        }
        Resize_Train_List_Content(0);
    }

    private void Resize_Train_List_Content(int i)
    {
        RectTransform ContentSize = TrainUgpradeList_Content[i].GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(-500 + (92 * TrainUgpradeList_Content[i].childCount), 53);
    }

    private void Setting_TrainType_DropDown_Upgrade()
    {
        TrainUpgradeList_DropDown.ClearOptions();
        List<string> optionList = new List<String>();
        TrainUpgradeList_DropDown.onValueChanged.RemoveAllListeners();
        TrainUpgradeList_DropDown.onValueChanged.AddListener(Change_TrianType_DropDown_Upgrade);
        optionList.Add("A");
        optionList.Add("B");
        optionList.Add("C");
        optionList.Add("D");
        TrainUpgradeList_DropDown.AddOptions(optionList);
        TrainUpgradeList_DropDown.value = 0;
        TrainUpgradeList_BeforeNum = 0;

        for(int i = 0; i < TrainUgpradeList_Content.Length; i++)
        {
            if (i != 0)
            {
                TrainUgpradeList_Content[i].gameObject.SetActive(false);
            }
        }
    }

    private void Change_TrianType_DropDown_Upgrade(int i)
    {
        TrainUgpradeList_Content[TrainUpgradeList_BeforeNum].gameObject.SetActive(false);
        TrainUgpradeList_Content[i].gameObject.SetActive(true);
        TrainUpgradeList_BeforeNum = i;
    }

    private void Instantiate_AfterTrainBuy(int i, int trainNum)
    {
        TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Director = this;
        if (i == 1)
        {
            TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Train_Num = sa_trainData.SA_TrainChangeNum(trainNum);
        }
        else if(i == 2)
        {
            TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Train_Num = 51;
            TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Train_Num2 = sa_trainturretData.SA_Train_Turret_ChangeNum(trainNum);
        }
        else if(i == 3)
        {
            TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Train_Num = 52;
            TrainUpgradeList_Button_Object.GetComponent<TrainUpgradeList_Button>().Train_Num2 = sa_trainboosterData.SA_Train_Booster_ChangeNum(trainNum);
        }
        Instantiate(TrainUpgradeList_Button_Object, TrainUgpradeList_Content[0]);
        Instantiate(TrainUpgradeList_Button_Object, TrainUgpradeList_Content[i]);
        Resize_Train_List_Content(0);
        Resize_Train_List_Content(i);
    }

    public void TrainUpgradeList_Button_Click(int Train_Num, int Train_Num2)
    {
        Train_Upgrade_Num1 = Train_Num;
        Train_Upgrade_Num2 = Train_Num2;

        Check_TrainChange_Upgrade();
        Check_TrainState_Slider_Upgrade();
    }

    void Check_TrainChange_Upgrade()
    {
        if (!UpgradeWindow.activeSelf)
        {
            UpgradeWindow.SetActive(true);
        }

        if(Train_Upgrade_Num1 == 51)
        {
            Train_MainImage_Upgrade.sprite = Resources.Load<Sprite>("Sprite/Train/Train_51_" + Train_Upgrade_Num2 / 10 * 10);
            
            Train_Name_Upgrade_Text.StringReference.TableEntryReference = "Train_Turret_Name_" + (Train_Upgrade_Num2 / 10);
            Train_Information_Upgrade_Text.StringReference.TableEntryReference = "Train_Turret_Information_" + (Train_Upgrade_Num2 / 10);
            TrainUpgrade_trainLevel = sa_trainturretData.SA_Train_Turret_ChangeNum(Train_Upgrade_Num2);
            TrainUpgrade_cost = trainData.EX_Game_Data.Information_Train_Turret_Part[TrainUpgrade_trainLevel].Train_Upgrade_Cost;
        }
        else if(Train_Upgrade_Num1 == 52)
        {
            Train_MainImage_Upgrade.sprite = Resources.Load<Sprite>("Sprite/Train/Train_52_" + Train_Upgrade_Num2 / 10 * 10);

            Train_Name_Upgrade_Text.StringReference.TableEntryReference = "Train_Booster_Name_" + (Train_Upgrade_Num2 / 10);
            Train_Information_Upgrade_Text.StringReference.TableEntryReference = "Train_Booster_Information_" + (Train_Upgrade_Num2 / 10);
            TrainUpgrade_trainLevel = sa_trainboosterData.SA_Train_Booster_ChangeNum(Train_Upgrade_Num2);
            TrainUpgrade_cost = trainData.EX_Game_Data.Information_Train_Booster_Part[TrainUpgrade_trainLevel].Train_Upgrade_Cost;
        }
        else
        {
            Train_MainImage_Upgrade.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + Train_Upgrade_Num1);

            Train_Name_Upgrade_Text.StringReference.TableEntryReference = "Train_Name_" + (Train_Upgrade_Num1 / 10);
            Train_Information_Upgrade_Text.StringReference.TableEntryReference = "Train_Information_" + (Train_Upgrade_Num1 / 10);
            TrainUpgrade_trainLevel = sa_trainData.SA_TrainChangeNum(Train_Upgrade_Num1);
            TrainUpgrade_cost = trainData.EX_Game_Data.Information_Train[TrainUpgrade_trainLevel].Train_Upgrade_Cost;
        }

        if (TrainUpgrade_cost != -100)
        {
            Train_Upgrade_Button.interactable = true;
            Train_Upgrade_CostText.text = TrainUpgrade_cost.ToString();
        }
        else
        {
            Train_Upgrade_Button.interactable = false;
            Train_Upgrade_CostText.text = "Max";
        }
    }

    private void Check_TrainState_Slider_Upgrade()
    {
        int Plus_HP;
        int Plus_Weight;
        int Plus_Armor;

        float EX_HP = 0;
        float EX_Weight = 0;
        float EX_Armor = 0;

        float EX_HP2 = 0;
        float EX_Weight2 = 0;
        float EX_Armor2 = 0;

        if (Train_Upgrade_Num1 == 51)
        {
            int num2 = sa_trainturretData.SA_Train_Turret_ChangeNum(Train_Upgrade_Num2);
            Info_Train_Turret_Part trainData_Info = trainData.EX_Game_Data.Information_Train_Turret_Part[num2];
            Info_Train_Turret_Part trainData_Info2 = null;

            int index = num2 % 10;
            Before_LevelImage.sprite = Level_Sprite[index];

            EX_HP = 1 - (trainData_Info.Train_HP / MaxHP);
            EX_Weight = 1 - (trainData_Info.Train_Weight / MaxWeight);
            EX_Armor = 1 - (trainData_Info.Train_Armor / MaxArmor);

            if (index % 10 < 9)
            {
                After_LevelImage.sprite = Level_Sprite[index + 1];
                trainData_Info2 = trainData.EX_Game_Data.Information_Train_Turret_Part[num2+1];
                EX_HP2 = 1 - (trainData_Info2.Train_HP / MaxHP);
                EX_Weight2 = 1 - (trainData_Info2.Train_Weight / MaxWeight);
                EX_Armor2 = 1 - (trainData_Info2.Train_Armor / MaxArmor);
            }
            else //max
            {
                After_LevelImage.sprite = Level_Sprite[index];
                trainData_Info2 = trainData.EX_Game_Data.Information_Train_Turret_Part[num2];
                EX_HP2 = EX_HP;
                EX_Weight2 = EX_Weight;
                EX_Armor2 = EX_Armor;
            }

            Plus_HP = trainData_Info2.Train_HP - trainData_Info.Train_HP;
            Plus_Weight = trainData_Info2.Train_Weight - trainData_Info.Train_Weight;
            Plus_Armor = trainData_Info2.Train_Armor - trainData_Info.Train_Armor;
        }
        else if (Train_Upgrade_Num1 == 52)
        {
            int num2 = sa_trainboosterData.SA_Train_Booster_ChangeNum(Train_Upgrade_Num2);
            Info_Train_Booster_Part trainData_Info = trainData.EX_Game_Data.Information_Train_Booster_Part[Train_Upgrade_Num2];
            Info_Train_Booster_Part trainData_Info2 = null;

            EX_HP = 1 - (trainData_Info.Train_HP / MaxHP);
            EX_Weight = 1 - (trainData_Info.Train_Weight / MaxWeight);
            EX_Armor = 1 - (trainData_Info.Train_Armor / MaxArmor);

            int index = num2 % 10;
            Before_LevelImage.sprite = Level_Sprite[index];

            if (num2 % 10 < 5)
            {
                After_LevelImage.sprite = Level_Sprite[index+1];
                trainData_Info2 = trainData.EX_Game_Data.Information_Train_Booster_Part[num2 + 1];
                EX_HP2 = 1 - (trainData_Info2.Train_HP / MaxHP);
                EX_Weight2 = 1 - (trainData_Info2.Train_Weight / MaxWeight);
                EX_Armor2 = 1 - (trainData_Info2.Train_Armor / MaxArmor);

            }
            else //max
            {
                After_LevelImage.sprite = Level_Sprite[index];
                trainData_Info2 = trainData.EX_Game_Data.Information_Train_Booster_Part[num2];
                EX_HP2 = EX_HP;
                EX_Weight2 = EX_Weight;
                EX_Armor2 = EX_Armor;
            }

            Plus_HP = trainData_Info2.Train_HP - trainData_Info.Train_HP;
            Plus_Weight = trainData_Info2.Train_Weight - trainData_Info.Train_Weight;
            Plus_Armor = trainData_Info2.Train_Armor - trainData_Info.Train_Armor;
        }
        else
        {
            int num = sa_trainData.SA_TrainChangeNum(Train_Upgrade_Num1);
            Info_Train trainData_Info = trainData.EX_Game_Data.Information_Train[Train_Upgrade_Num1];
            Info_Train trainData_Info2 = null;

            EX_HP = 1 - (trainData_Info.Train_HP / MaxHP);
            EX_Weight = 1 - (trainData_Info.Train_Weight / MaxWeight);
            EX_Armor = 1 - (trainData_Info.Train_Armor / MaxArmor);

            int index = num % 10;
            Before_LevelImage.sprite = Level_Sprite[index];

            if (num % 10 < 5)
            {
                After_LevelImage.sprite = Level_Sprite[index + 1];
                trainData_Info2 = trainData.EX_Game_Data.Information_Train[num + 1];
                EX_HP2 = 1 - (trainData_Info2.Train_HP / MaxHP);
                EX_Weight2 = 1 - (trainData_Info2.Train_Weight / MaxWeight);
                EX_Armor2 = 1 - (trainData_Info2.Train_Armor / MaxArmor);

            }
            else //max
            {
                After_LevelImage.sprite = Level_Sprite[index];
                trainData_Info2 = trainData.EX_Game_Data.Information_Train[num];
                EX_HP2 = EX_HP;
                EX_Weight2 = EX_Weight;
                EX_Armor2 = EX_Armor;
            }

            Plus_HP = trainData_Info2.Train_HP - trainData_Info.Train_HP;
            Plus_Weight = trainData_Info2.Train_Weight - trainData_Info.Train_Weight;
            Plus_Armor = trainData_Info2.Train_Armor - trainData_Info.Train_Armor;
        }

        Slider_Upgrade_Before_HP[0].value = (float)Math.Round(EX_HP,2);
        Slider_Upgrade_Before_HP[1].value = (float)Math.Round(EX_HP, 2);
        Slider_Upgrade_Before_Weight[0].value = (float)Math.Round(EX_Weight, 2);
        Slider_Upgrade_Before_Weight[1].value = (float)Math.Round(EX_Weight, 2);
        Slider_Upgrade_Before_Armor[0].value = (float)Math.Round(EX_Armor, 2);
        Slider_Upgrade_Before_Armor[1].value = (float)Math.Round(EX_Armor, 2);
        Slider_Upgrade_After_HP.value = (float)Math.Round(EX_HP2, 2);
        Slider_Upgrade_After_Weight.value = (float)Math.Round(EX_Weight2, 2);
        Slider_Upgrade_After_Armor.value = (float)Math.Round(EX_Armor2, 2);

        Plus_HP_Text.text = "+" +Plus_HP;
        Plus_Weight_Text.text = "+" + Plus_Weight;
        Plus_Armor_Text.text = "+" + Plus_Armor;
    }

    public void Click_Train_Upgrade()
    {
        if(Train_Upgrade_Num1 == 51)
        {
            if (playerData.Player_Coin >= TrainUpgrade_cost)
            {
                playerData.Player_Buy_Coin(TrainUpgrade_cost);
                trainData.Train_Turret_Level_Up(Train_Upgrade_Num2);
                Train_Upgrade_Num2++;
                Check_TrainChange_Upgrade();
                Check_TrainState_Slider_Upgrade();
            }
            else
            {
                Open_Warning_Window();
            }
        }
        else if(Train_Upgrade_Num1 == 52)
        {
            if(playerData.Player_Coin >= TrainUpgrade_cost)
            {
                playerData.Player_Buy_Coin(TrainUpgrade_cost);
                trainData.Train_Booster_Level_Up(Train_Upgrade_Num2);
                Train_Upgrade_Num2++;
                Check_TrainChange_Upgrade();
                Check_TrainState_Slider_Upgrade();
            }
            else
            {
                Open_Warning_Window();
            }
        }
        else
        {
            if(playerData.Player_Coin >= TrainUpgrade_cost)
            {
                playerData.Player_Buy_Coin(TrainUpgrade_cost);
                trainData.Train_Level_Up(Train_Upgrade_Num1);
                Train_Upgrade_Num1++;
                Check_TrainChange_Upgrade();
                Check_TrainState_Slider_Upgrade();
            }
            else
            {
                Open_Warning_Window();
            }
        }
    }

    //패시브 업그레이드
    private void Passive_Upgrade_Text(int num)
    {
        if (num == 0)
        {
            if (trainData.Level_Train_EngineTier == trainData.Max_Train_EngineTier)
            {
                Passive_Level_Text[0].text = "Lv.MAX";
                Passive_Cost_Text[0].text = "";
                Passive_Button[0].interactable = false;
            }
            else
            {
                Passive_Level_Text[0].text = "Lv." + trainData.Level_Train_EngineTier;
                Passive_Cost_Text[0].text = trainData.Cost_Train_EngineTier + "G";
            }
        }
        else if (num == 1)
        {
            if (trainData.Level_Train_MaxTrain == trainData.Max_Train_MaxTrain)
            {
                Passive_Level_Text[1].text = "Lv.MAX";
                Passive_Cost_Text[1].text = "";
                Passive_Button[1].interactable = false;
            }
            else
            {
                Passive_Level_Text[1].text = "Lv." + trainData.Level_Train_MaxTrain;
                Passive_Cost_Text[1].text = trainData.Cost_Train_MaxTrain + "G";
            }
        }
        else if (num == 2)
        {
            if (trainData.Level_Train_MaxMercenary == trainData.Max_Train_MaxMercenary)
            {
                Passive_Level_Text[2].text = "Lv.MAX";
                Passive_Cost_Text[2].text = "";
                Passive_Button[2].interactable = false;
            }
            else
            {
                Passive_Level_Text[2].text = "Lv." + trainData.Level_Train_MaxMercenary;
                Passive_Cost_Text[2].text = trainData.Cost_Train_MaxMercenary + "G";
            }
        }
        else if (num == 3)
        {
            if (trainData.Level_Train_MaxSpeed == trainData.Max_Train_MaxSpeed)
            {
                Passive_Level_Text[3].text = "Lv.MAX";
                Passive_Cost_Text[3].text = "";
                Passive_Button[3].interactable = false;
            }
            else
            {
                Passive_Level_Text[3].text = "Lv." + trainData.Level_Train_MaxSpeed;
                Passive_Cost_Text[3].text = trainData.Cost_Train_MaxSpeed + "G";

            }
        }
        else if (num == 4)
        {
            if (trainData.Level_Train_Armor == trainData.Max_Train_Armor)
            {
                Passive_Level_Text[4].text = "Lv.MAX";
                Passive_Cost_Text[4].text = "";
                Passive_Button[4].interactable = false;
            }
            else
            {
                Passive_Level_Text[4].text = "Lv." + trainData.Level_Train_Armor;
                Passive_Cost_Text[4].text = trainData.Cost_Train_Armor + "G";

            }
        }
        else if (num == 5)
        {
            if (trainData.Level_Train_Efficient == trainData.Max_Train_Efficient)
            {
                Passive_Level_Text[5].text = "Lv.MAX";
                Passive_Cost_Text[5].text = "";
                Passive_Button[5].interactable = false;
            }
            else
            {
                Passive_Level_Text[5].text = "Lv." + trainData.Level_Train_Efficient;
                Passive_Cost_Text[5].text = trainData.Cost_Train_Efficient + "G";
            }
        }
    }

    public void Click_Passive_Upgrade(int i)//LevelNum : 0 = Tier / 1 = Speed / 2 = Armor / 3 = Efficient
    {
        if (playerData.Player_Coin >= trainData.Check_Cost_Train(i))
        {
            playerData.Player_Buy_Coin(trainData.Check_Cost_Train(i)); //먼저 차감 후, 업그레이드가 된다.
            trainData.Passive_Level_Up(i);
            Passive_Upgrade_Text(i);
            Check_Player_Coin_Point();
            // 여기에 맥스 조절하는 부분이 없다.
            Check_Trian_Add(); // 엔진 티어의 레벨에 따라 기차 추가 여부가 달라짐
        }
        else
        {
            Ban_Player_Coin_Point(true);
        }
    }

    public void Open_Warning_Window()
    {
        Warning_Coin_Window.SetActive(true);
    }

    public void Close_Warning_Window()
    {
        Warning_Coin_Window.SetActive(false);
    }

    void DropDown_Option_Change()
    {
        TMP_Dropdown options_1 = TrainBuy_DropDown;
        TMP_Dropdown options_2 = TrainUpgradeList_DropDown;

        options_1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalString_TrainType[options_1.value+1].GetLocalizedString();
        options_2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalString_TrainType[options_2.value].GetLocalizedString();

        for(int i = 1; i < 4; i++)
        {
            options_1.options[i-1].text = LocalString_TrainType[i].GetLocalizedString();
        }

        for(int i = 0; i < 4; i++)
        {
            options_2.options[i].text = LocalString_TrainType[i].GetLocalizedString();
        }
    }
}