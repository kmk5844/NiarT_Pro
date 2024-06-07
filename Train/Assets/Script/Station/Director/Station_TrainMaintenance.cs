using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Station_TrainMaintenance : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Player_DataObject;
    Station_PlayerData playerData;

    [Header("UI에서 나타나는 기차")]
    public Transform UI_TrainList;
    public int UI_Train_Num;
    public Transform UI_TrainButtonList;
    public GameObject[] Train_Button;
    public TextMeshProUGUI UI_Train_Information_Text;
    int UI_Train_Turret_Num;
    int UI_Train_Booster_Num;

    [Header("패시브 업그레이드 윈도우")]
    public TextMeshProUGUI[] Passive_Text;
    public Button[] Passive_Button;

    [Header("기차 변경 윈도우")]
    public Button Change_Button;
    public Button Add_Button;
    [SerializeField]
    List<int> Train_Change_Num;
    public ScrollRect ScrollRect_ChangeTrain;
    public Transform Train_Change_Content;
    public GameObject Train_Card;
    int Engine_Tier_Max_Train;
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
    public GameObject Part_Card;
    public Transform Turret_Part_Content;
    public Transform Booster_Part_Content;
    int Toggle_Turret_Part_Num;
    int Toggle_Booster_Part_Num;
    [SerializeField]
    List<Toggle> Turret_Part_Toggle;
    [SerializeField]
    List<Toggle> Booster_Part_Toggle;
    public Button Part_Change_Button;

    [Header("기차 업그레이드 윈도우")]
    public TextMeshProUGUI Before_Text;
    public TextMeshProUGUI After_Text;
    public Button Upgrade_Button;
    public TextMeshProUGUI Upgrade_Text;

    private void Start()
    {
        UI_Train_Num = 0;
        UI_Train_Turret_Num = 0;
        UI_Train_Booster_Num = 0;
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        Engine_Tier_Max_Train = trainData.Max_Train_MaxTrain;
        Train_Change_Num = trainData.Train_Change_Num;
        Train_Turret_Part_Change_Num = trainData.Train_Turret_Part_Change_Num;
        Train_Booster_Part_Change_Num = trainData.Train_Booster_Part_Change_Num;
        //UI 기차 생성하기
        UI_TrainImage(false);
        Current_Train_Information();
        //패시브 업그레이드
        Passive_Upgrade_Text(true);
        //기차 변경하기
        Check_Init_TrainCard();
        Director_Init_TrainChange();
        //파츠 변경하기
        Check_Init_TrainTurretPartCard();
        Check_Init_TrainBoosterPartCard();
        Director_Init_TrainPartChange();
        Train_ToggleStart();
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
                    train = Instantiate(Resources.Load<GameObject>("TrainObject_UI/51_" + trainData.SA_TrainTurretData.Train_Turret_Num[UI_Train_Turret_Num]), UI_TrainList);
                    UI_Train_Turret_Num++;
                }
                else if(trainNum == 52)
                {
                    train = Instantiate(Resources.Load<GameObject>("TrainObject_UI/52_" + trainData.SA_TrainBoosterData.Train_Booster_Num[UI_Train_Booster_Num]), UI_TrainList);
                    UI_Train_Booster_Num++;
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
        Check_Upgrade_Button_Interactable();

        Current_Train_Information();
        //업그레이드 부분도 포함
        Upgrade_Before_After_Text();
    } //버튼에 참조

    void Button_TrainNum(int Num)
    {
        int beforeNum = UI_Train_Num;
        UI_Train_Num = Num - 1;
        UI_TrainList.GetChild(beforeNum).gameObject.SetActive(false);
        UI_TrainList.GetChild(UI_Train_Num).gameObject.SetActive(true);
        UI_TrainButtonList.GetChild(beforeNum).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(false);
        UI_TrainButtonList.GetChild(UI_Train_Num).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(true);
        Check_Change_Button_Interactable();
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
            UI_Train_Information_Text.text = trainData.EX_Game_Data.Information_Train_Turret_Part[trainNum2].Train_Information.Replace("\\n", "\n") +
                trainData.EX_Game_Data.Information_Train_Turret_Part[trainNum2].Train_Select_Information.Replace("\\n", "\n").Replace("\\t", "\t");
        }
        else if (trainNum == 52)
        {
            UI_Train_Information_Text.text = trainData.EX_Game_Data.Information_Train_Booster_Part[trainNum2].Train_Information.Replace("\\n", "\n") +
               trainData.EX_Game_Data.Information_Train_Booster_Part[trainNum2].Train_Select_Information.Replace("\\n", "\n").Replace("\\t", "\t");
        }
        else
        {
            UI_Train_Information_Text.text = trainData.EX_Game_Data.Information_Train[trainNum].Train_Information.Replace("\\n", "\n") +
            trainData.EX_Game_Data.Information_Train[trainNum].Train_Select_Information.Replace("\\n", "\n").Replace("\\t", "\t");
        }
    }

    //패시브 업그레이드
    private void Passive_Upgrade_Text(bool All, int num = 0)
    {
        if (All)
        {
            if (trainData.Level_Train_EngineTier == trainData.Max_Train_EngineTier)
            {
                Passive_Text[0].text = "Lv.MAX\n0G";
                Passive_Button[0].interactable = false;
            }
            else
            {
                Passive_Text[0].text = "Lv." + trainData.Level_Train_EngineTier + "\n" + trainData.Cost_Train_EngineTier + "G";
            }
            if (trainData.Level_Train_MaxSpeed == trainData.Max_Train_MaxSpeed)
            {
                Passive_Text[1].text = "Lv.MAX\n0G";
                Passive_Button[1].interactable = false;
            }
            else
            {
                Passive_Text[1].text = "Lv." + trainData.Level_Train_MaxSpeed + "\n" + trainData.Cost_Train_MaxSpeed + "G";
            }
            if (trainData.Level_Train_Armor == trainData.Max_Train_Armor)
            {
                Passive_Text[2].text = "Lv.MAX\n0G";
                Passive_Button[2].interactable = false;
            }
            else
            {
                Passive_Text[2].text = "Lv." + trainData.Level_Train_Armor + "\n" + trainData.Cost_Train_Armor + "G";
            }
            if (trainData.Level_Train_Efficient == trainData.Max_Train_Efficient)
            {
                Passive_Text[3].text = "Lv.MAX\n0G";
                Passive_Button[3].interactable = false;
            }
            else
            {
                Passive_Text[3].text = "Lv." + trainData.Level_Train_Efficient + "\n" + trainData.Cost_Train_Efficient + "G";
            }
        }
        else
        {
            if (num == 0)
            {
                if (trainData.Level_Train_EngineTier == trainData.Max_Train_EngineTier)
                {
                    Passive_Text[0].text = "Lv.MAX\n0G";
                    Passive_Button[0].interactable = false;
                }
                else
                {
                    Passive_Text[0].text = "Lv." + trainData.Level_Train_EngineTier + "\n" + trainData.Cost_Train_EngineTier + "G";
                }
            }
            else if (num == 1)
            {
                if (trainData.Level_Train_MaxSpeed == trainData.Max_Train_MaxSpeed)
                {
                    Passive_Text[1].text = "Lv.MAX\n0G";
                    Passive_Button[1].interactable = false;
                }
                else
                {
                    Passive_Text[1].text = "Lv." + trainData.Level_Train_MaxSpeed + "\n" + trainData.Cost_Train_MaxSpeed + "G";
                }
            }
            else if (num == 2)
            {
                if (trainData.Level_Train_Armor == trainData.Max_Train_Armor)
                {
                    Passive_Text[2].text = "Lv.MAX\n0G";
                    Passive_Button[2].interactable = false;
                }
                else
                {
                    Passive_Text[2].text = "Lv." + trainData.Level_Train_Armor + "\n" + trainData.Cost_Train_Armor + "G";
                }
            }
            else if (num == 3)
            {
                if (trainData.Level_Train_Efficient == trainData.Max_Train_Efficient)
                {
                    Passive_Text[3].text = "Lv.MAX\n0G";
                    Passive_Button[3].interactable = false;
                }
                else
                {
                    Passive_Text[3].text = "Lv." + trainData.Level_Train_Efficient + "\n" + trainData.Cost_Train_Efficient + "G";
                }
            }
        }
    }

    public void Click_Passive_Upgrade(int i)//LevelNum : 0 = Tier / 1 = Speed / 2 = Armor / 3 = Efficient
    {
        if(playerData.Player_Coin >= trainData.Check_Cost_Train(i))
        {
            playerData.Player_Buy_Coin(trainData.Check_Cost_Train(i)); //먼저 차감 후, 업그레이드가 된다.
            trainData.Passive_Level_Up(i);
            Passive_Upgrade_Text(false, i);
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
        //Cost_Add_Text.text = trainData.EX_Game_Data.Information_Train[100].Train_Change_Cost.ToString() + "G";
    } //전체 초기화

    private void Check_Init_TrainCard()
    {
        RectTransform ContentSize = Train_Change_Content.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(300 * Train_Change_Num.Count, ContentSize.sizeDelta.y);
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
            }
        }
        else
        {
            ChangeFlag = false;
            Check_Change_Button_Interactable();
        }
    }

    public void Button_Train_Change()
    {
        if(Toggle_Train_Num == 51)
        {
            ScrollRect_Turret_Part.gameObject.SetActive(true);
            ScrollRect_Booster_Part.gameObject.SetActive(false);
            Click_Part_Button();
        }
        else if(Toggle_Train_Num == 52)
        {
            ScrollRect_Turret_Part.gameObject.SetActive(false);
            ScrollRect_Booster_Part.gameObject.SetActive(true);
            Click_Part_Button();
        }
        else
        {
            int changeNum = trainData.SA_TrainData.SA_TrainChangeNum(Toggle_Train_Num); // -> Toggle_Train_Num 같은 경우, 0레벨의 기차숫자로 가져오기 때문에, 재수정이 필요.
            trainData.SA_TrainData.SA_Train_Change(UI_Train_Num, changeNum); //임시로 저장
            Destroy(UI_TrainList.GetChild(UI_Train_Num).gameObject);
            GameObject changeTrain = Instantiate(Resources.Load<GameObject>("TrainObject_UI/" + changeNum), UI_TrainList);
            changeTrain.name = Toggle_Train_Num.ToString();
            changeTrain.transform.SetSiblingIndex(UI_Train_Num);
            Upgrade_Before_After_Text();
            Check_Change_Button_Interactable();
        }
    }

    public void Button_Train_Add()
    {
/*        if(playerData.Player_Coin >= trainData.EX_Game_Data.Information_Train[100].Train_Change_Cost)
        {
            UI_TrainButtonList.GetChild(UI_Train_Num).GetComponent<Station_Maintenance_TrainNum_Button>().ChekcButton(false);
            playerData.Player_Buy_Coin(trainData.EX_Game_Data.Information_Train[100].Train_Change_Cost);
            trainData.SA_TrainData.SA_Train_Add(100); //empty Trian
            UI_TrainList.GetChild(UI_Train_Num).gameObject.SetActive(false);
            UI_Train_Num = UI_TrainList.childCount;
            GameObject EmptyTrain = Instantiate(Resources.Load<GameObject>("TrainObject_UI/100"), UI_TrainList);
            EmptyTrain.name = 100.ToString();
            UI_TrainImage(true);
            Check_Trian_Add();
            Upgrade_Before_After_Text();
            Check_Player_Coin_Point();
        }
        else
        {
            Ban_Player_Coin_Point(true);
        }*/
    }
    public void Click_Part_Button()
    {
        UI_Train_Part_Window.SetActive(true);
    }

    public void Click_Part_Back_Button()
    {
        UI_Train_Part_Window.SetActive(false);
    }

    private void Check_Trian_Add()
    {
        Engine_Tier_Max_Train = trainData.Max_Train_MaxTrain;
        Add_Button.interactable = (trainData.Train_Num.Count < Engine_Tier_Max_Train) ? true : false;
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
                Change_Button.interactable = (ChangeFlag) ? true : false;
            }
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
            for (int i = 0; i < Turret_Part_Content.childCount; i++)
            {
                Destroy(Turret_Part_Content.GetChild(i).gameObject);
                Turret_Part_Toggle.Clear();
            }
            Check_Init_TrainBoosterPartCard();
            Booster_Part_ToggleStart();
        }

        ScrollRect_Turret_Part.normalizedPosition = Vector2.zero;
        ScrollRect_Booster_Part.normalizedPosition = Vector2.zero;
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
        ContentSize.sizeDelta = new Vector2(500 * Train_Turret_Part_Change_Num.Count, ContentSize.sizeDelta.y);
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
        ContentSize.sizeDelta = new Vector2(500 * Train_Booster_Part_Change_Num.Count, ContentSize.sizeDelta.y);
        foreach (int num in Train_Booster_Part_Change_Num)
        {
            if (trainData.SA_TrainBoosterData.SA_TrainChangeNum(num) == -1)
            {
                Part_Card.GetComponent<Train_Part_Card>().Train_Type_Num = 52;
                Part_Card.GetComponent<Train_Part_Card>().Train_Part_Num = num;
            }
            else
            {
                Part_Card.GetComponent<Train_Part_Card>().Train_Type_Num = 52;
                Part_Card.GetComponent<Train_Part_Card>().Train_Part_Num = trainData.SA_TrainTurretData.SA_Train_Turret_ChangeNum(num);
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
        foreach (Toggle toggle in Turret_Part_Toggle)
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
            for (int i = 0; i < Turret_Part_Toggle.Count; i++)
            {
                if (Turret_Part_Toggle[i].isOn)
                {
                    Train_Part_Card Card = Booster_Part_Content.GetChild(i).GetComponent<Train_Part_Card>();
                    Toggle_Booster_Part_Num = Card.Train_Part_Num;
                }
            }
            Part_Change_Button.interactable = true;
        }
        else
        {
            Part_Change_Button.interactable = false;
        }
    }

    public void Button_Change_Part()
    {
        if(Toggle_Train_Num == 51)
        {
            Debug.Log("장착 될 예정");
        }else if (Toggle_Train_Num == 52)
        {
            //일단 보류
        }
    }


    //기차 업그레이드
    public void Direcotr_Init_TrainUpgrade()
    {
        Check_Upgrade_Button_Interactable();
    }

    public void Click_Button_Upgrade()
    {
        if (playerData.Player_Coin >= trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Train_Upgrade_Cost)
        {
            playerData.Player_Buy_Coin(trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Train_Upgrade_Cost);
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

    public void Upgrade_Before_After_Text()
    {
        Info_Train train = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]];

        Before_Text.text =
            "  Lv : " + (trainData.Train_Num[UI_Train_Num] + 1) % 10
            + "\nName : " + train.Train_Name
            + "\nHP : " + train.Train_HP
            + "\nWeight : " + train.Train_Weight
            + "\nArmor : " + train.Train_Armor;

        if (trainData.Train_Num[UI_Train_Num] >= 90)
        {
            After_Text.text = "  Specail Train";
        }
        else
        {
            if (trainData.Train_Num[UI_Train_Num] % 10 < 9)
            {
                train = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num] + 1];
                After_Text.text =
                    "  Lv : " + (trainData.Train_Num[UI_Train_Num] + 2) % 10
                    + "\nName : " + train.Train_Name
                    + "\nHP : <color=red>" + train.Train_HP
                    + "\n</color>Weight : <color=red>" + train.Train_Weight
                    + "\n</color>Armor : <color=red>" + train.Train_Armor;
            }
            else if (trainData.Train_Num[UI_Train_Num] % 10 == 9)
            {
                After_Text.text = "  Max";
            }

        }
        Upgrade_Text.text = trainData.EX_Game_Data.Information_Train[trainData.Train_Num[UI_Train_Num]].Train_Upgrade_Cost.ToString() + "G";
    }

    private void Upgrade_Train_TrainMaintenance()
    {
        for (int i = 0; i < UI_TrainList.childCount; i++)
        {
            Destroy(UI_TrainList.GetChild(i).gameObject);
        }

        int num = 0;
        foreach (int trainNum in trainData.Train_Num)
        {
            GameObject train = Instantiate(Resources.Load<GameObject>("TrainObject_UI/" + trainNum), UI_TrainList);
            train.name = trainNum.ToString();
            if (num != UI_Train_Num)
            {
                train.gameObject.SetActive(false);
            }
            num++;
        }
        Check_Player_Coin_Point();
    }

    private void Check_Upgrade_Button_Interactable()
    {
        Upgrade_Button.interactable = (trainData.Train_Num[UI_Train_Num] < 90) ? true : false;
    }

    private void Check_Player_Coin_Point()
    {
        transform.GetComponentInParent<StationDirector>().Check_CoinAndPoint();
    }

    private void Ban_Player_Coin_Point(bool Flag)
    {
        transform.GetComponentInParent<StationDirector>().Check_Ban_CoinPoint(Flag);
    }
}