using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;


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
    public LocalizeStringEvent UI_Train_Information_Text;
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

    [Header("기차 구매 윈도우")]
    

    [Header("기차 업그레이드 윈도우")]
    public TextMeshProUGUI Before_Text;
    public TextMeshProUGUI After_Text;
    public Button Upgrade_Button;
    public Image Material_Image;
    public TextMeshProUGUI[] Upgrade_Text; // 0 : Coin, 1 : 가지고 있는 재료템, 2: 재료 충족 조건

    [Header("패시브 업그레이드 윈도우")]
    public TextMeshProUGUI[] Passive_Level_Text;
    public TextMeshProUGUI[] Passive_Cost_Text;
    public Button[] Passive_Button;

    private void Start()
    {
        Part_Window_Flag = false;
        UI_Train_Num = 0;
        UI_Init_Train_Turret_Num = 0;
        UI_Init_Train_Booster_Num = 0;
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        itemData = Item_DataObject.GetComponent<Station_ItemData>();
        Max_Train = trainData.Level_Train_MaxTrain + 2;
        UI_TrainMax_Text.text = "Train MAX : " + Max_Train;
        Train_Change_Num = trainData.Train_Change_Num;
        Train_Turret_Part_Change_Num = trainData.Train_Turret_Part_Change_Num;
        Train_Booster_Part_Change_Num = trainData.Train_Booster_Part_Change_Num;
        UI_Train_Information_Text.StringReference.TableReference = "ExcelData_Table_St";
        UI_Train_Part_Text.StringReference.TableReference = "ExcelData_Table_St";
        //UI 기차 생성하기
        UI_TrainImage(false);
        Current_Train_Information();
        //패시브 업그레이드
        for(int i = 0; i < 5; i++)
        {
            Passive_Upgrade_Text(i);
        }
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
        Upgrade_Before_After_Text();
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
        Check_Upgrade_Button_Interactable();
        Current_Train_Information();
        //업그레이드 부분도 포함
        Upgrade_Before_After_Text();
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
        Check_Upgrade_Button_Interactable();
        Upgrade_Before_After_Text();
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
            UI_Train_Information_Text.StringReference.TableEntryReference = "Train_Turret_Information_" + (trainNum2 / 10);
           /* UI_Train_Information_Text.text = trainData.EX_Game_Data.Information_Train_Turret_Part[trainNum2].Train_Information.Replace("\\n", "\n")
                + trainData.EX_Game_Data.Information_Train_Turret_Part[trainNum2].Train_Select_Information.Replace("\\n", "\n").Replace("\\t", "\t");*/
        }
        else if (trainNum == 52)
        {
            UI_Train_Information_Text.StringReference.TableEntryReference = "Train_Booster_Information_" + (trainNum2 / 10);
          /*  UI_Train_Information_Text.text = trainData.EX_Game_Data.Information_Train_Booster_Part[trainNum2].Train_Information.Replace("\\n", "\n")
              + trainData.EX_Game_Data.Information_Train_Booster_Part[trainNum2].Train_Select_Information.Replace("\\n", "\n").Replace("\\t", "\t");*/
        }
        else
        {
            if(trainNum < 50)
            {
                UI_Train_Information_Text.StringReference.TableEntryReference = "Train_Information_" + (trainNum / 10);
            }
            else
            {
                UI_Train_Information_Text.StringReference.TableEntryReference = "Train_Information_" + trainNum;
            }
          /*  UI_Train_Information_Text.text = trainData.EX_Game_Data.Information_Train[trainNum].Train_Information.Replace("\\n", "\n")
               + trainData.EX_Game_Data.Information_Train[trainNum].Train_Select_Information.Replace("\\n", "\n").Replace("\\t", "\t");*/
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
        else if(num == 1)
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
        else if(num == 2)
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
        if(playerData.Player_Coin >= trainData.Check_Cost_Train(i))
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

    //기차 변경하기
    public void Director_Init_TrainChange()
    {
        //max 체크
        Check_Trian_Add();
        //card 체크
        Train_Change_Num = trainData.Train_Change_Num;
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
            Upgrade_Before_After_Text();
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
                Upgrade_Before_After_Text();
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
        Train_Turret_Part_Change_Num = trainData.Train_Turret_Part_Change_Num;
        Train_Booster_Part_Change_Num = trainData.Train_Booster_Part_Change_Num;
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
                Upgrade_Before_After_Text();
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
                Upgrade_Before_After_Text();
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
                Upgrade_Before_After_Text();
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
                Upgrade_Before_After_Text();
                Check_Change_Button_Interactable();
                Click_Part_Back_Button();
                Check_Part_Flag();
            }
        }
    }

    //기차 업그레이드
    public void Direcotr_Init_TrainUpgrade()
    {
        Check_Upgrade_Button_Interactable();
        Upgrade_Before_After_Text();
    }

    public void Click_Button_Upgrade()
    {
        if (trainData.Train_Num[UI_Train_Num] == 51)
        {
            int cost = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]].Train_Upgrade_Cost;
            //int Material_Count = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]].Material;
            if (playerData.Player_Coin >= cost/* && itemData.Turret_Train_Material_object.Item_Count >= Material_Count*/)
            {
                playerData.Player_Buy_Coin(cost);
                //itemData.Turret_Train_Material_object.Item_Count_Down(Material_Count);
                trainData.Train_Turret_Level_Up(trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num], UI_Train_Turret_Num);
                Upgrade_Train_TrainMaintenance(); // UI 기차 변경
                Upgrade_Before_After_Text(); // 비포 애프터 변경도 하고 기차 옆의 정보도 변경.
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
            if (playerData.Player_Coin >= cost/* && itemData.Booster_Train_Material_object.Item_Count >= Material_Count*/)
            {
                playerData.Player_Buy_Coin(cost);
                //itemData.Booster_Train_Material_object.Item_Count_Down(Material_Count);
                trainData.Train_Booster_Level_Up(trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num], UI_Train_Booster_Num);
                Upgrade_Train_TrainMaintenance(); // UI 기차 변경
                Upgrade_Before_After_Text(); // 비포 애프터 변경도 하고 기차 옆의 정보도 변경.
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
            if (playerData.Player_Coin >= cost /*&& itemData.Common_Train_Material_object.Item_Count >= Material_Count*/)
            {
                playerData.Player_Buy_Coin(cost);
                //itemData.Common_Train_Material_object.Item_Count_Down(Material_Count);
                trainData.Train_Level_Up(trainData.Train_Num[UI_Train_Num], UI_Train_Num);
                Upgrade_Train_TrainMaintenance(); // UI 기차 변경
                Upgrade_Before_After_Text(); // 비포 애프터 변경도 하고 기차 옆의 정보도 변경.
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
/*            Material_Image.sprite = itemData.Turret_Train_Material_object.Item_Sprite;
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
            Upgrade_Text[2].text = trainData.EX_Game_Data.Information_Train_Turret_Part[trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]].Material.ToString();*/
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

/*            Material_Image.sprite = itemData.Booster_Train_Material_object.Item_Sprite;
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
            Upgrade_Text[2].text = trainData.EX_Game_Data.Information_Train_Booster_Part[trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]].Material.ToString();*/
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
/*            Material_Image.sprite = itemData.Common_Train_Material_object.Item_Sprite;
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
            Upgrade_Text[2].text = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Material.ToString();*/
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


/*        if(trainData.Train_Num[UI_Train_Num] < 50 || trainData.Train_Num[UI_Train_Num] == 51 || trainData.Train_Num[UI_Train_Num] == 52)
        {
            Upgrade_Button.interactable = true;
        }
        else
        {
            Upgrade_Button.interactable = false;
        }*/
    }

    private void Check_Player_Coin_Point()
    {
        transform.GetComponentInParent<StationDirector>().Check_CoinAndPoint();
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
}