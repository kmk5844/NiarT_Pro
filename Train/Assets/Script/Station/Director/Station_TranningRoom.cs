using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Station_TranningRoom : MonoBehaviour
{
   [Header("데이터 모음")]
    public GameObject Player_DataObject;
    Station_PlayerData playerData;
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Mercenary_DataObject;
    Station_MercenaryData mercenaryData;
    List<int> Mercenary_Buy_NumList;// 구매한 리스트
    List<int> Mercenary_Position_NumList;// 배치하고 있는 리스트


    [Header("플레이어 업그레이드 윈도우")]
    public TextMeshProUGUI PlayerUP_Text_0;
    public Button PlayerUP_Button_0;
    public TextMeshProUGUI PlayerUP_Text_1;
    public Button PlayerUP_Button_1;
    public TextMeshProUGUI PlayerUP_Text_2;
    public Button PlayerUP_Button_2;
    public TextMeshProUGUI PlayerUP_Text_3;
    public Button PlayerUP_Button_3;
    public TextMeshProUGUI PlayerUP_Text_4;
    public Button PlayerUP_Button_4;

    [Header("용병 업그레이드 윈도우")]

    public Button MercenaryUpgrade_Button;
    TextMeshProUGUI MercenaryUpgrade_Button_Text;
    [SerializeField]
    int Mercenary_Upgrade_Num;
    int Mercenary_Upgrade_ToggleNum;
    public ScrollRect ScrollRect_Mercenary_Upgrade; 
    public Transform Mercenary_Upgrade_Content;
    public GameObject Mercenary_Upgrade_Card;
    public TextMeshProUGUI Before_Mercenary_Information;
    public TextMeshProUGUI After_Mercenary_Information;
    [SerializeField]
    List<Toggle> Mercenary_Upgrade_Toggle;

    [Header("용병 배치 윈도우")]
    public ScrollRect ScrollRect_Mercenary_Position;
    public Transform Mercenary_Position_Content;
    public GameObject Mercenary_Position_Card;
    public TextMeshProUGUI Mercenary_Position_Information;
    int EngineTier_MaxMercenary;
    int Mercenary_TotalNum;
    [SerializeField]
    List<GameObject> CardList;
    [SerializeField]
    TMP_Dropdown DropDown_EngineDriver_Type;
    void Start()
    {
        //데이터 수집
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        Mercenary_Buy_NumList = mercenaryData.SA_MercenaryData.Mercenary_Buy_Num;
        Mercenary_Position_NumList = mercenaryData.SA_MercenaryData.Mercenary_Num;
        //플레이어 업그레이드 윈도우
        Player_Text(true);
        //용병 업그레이드 윈도우
        Check_Init_MecenaryUpgradeCard();
        MercenaryUpgrade_Button_Text = MercenaryUpgrade_Button.GetComponentInChildren<TextMeshProUGUI>();
        Before_Mercenary_Information.text = "<size=30>Before_Click Mercenary</size>";
        After_Mercenary_Information.text = "<size=30>Before_Click Mercenary</size>";
        OnToggleStart();

        //용병 배치 윈도우
        Mercenary_TotalNum = mercenaryData.Mercenary_Num.Count;
        EngineTier_MaxMercenary = trainData.Max_Train_MaxMercenary;
        Mercenary_Position_Text();
        Check_Init_MercenaryPositionCard();
    }

    //플레이어 업그레이드
    private void Player_Text(bool All, int num = 0)
    {
        if (All)
        {
            if (playerData.Level_Player_Atk == playerData.Max_Player_Atk)
            {
                PlayerUP_Text_0.text = "Lv.MAX\n0Pt";
                PlayerUP_Button_0.interactable = false;
            }
            else
            {
                PlayerUP_Text_0.text = "Lv." + playerData.Level_Player_Atk + "\n" + playerData.Cost_Player_Atk + "Pt";
            }
            if (playerData.Level_Player_AtkDelay == playerData.Max_Player_AtkDelay)
            {
                PlayerUP_Text_1.text = "Lv.MAX\n0Pt";
                PlayerUP_Button_1.interactable = false;
            }
            else
            {
                PlayerUP_Text_1.text = "Lv." + playerData.Level_Player_AtkDelay + "\n" + playerData.Cost_Player_AtkDelay + "Pt";
            }
            if (playerData.Level_Player_HP == playerData.Max_Player_HP)
            {
                PlayerUP_Text_2.text = "Lv.MAX\n0Pt";
                PlayerUP_Button_2.interactable = false;
            }
            else
            {
                PlayerUP_Text_2.text = "Lv." + playerData.Level_Player_HP + "\n" + playerData.Cost_Player_HP + "Pt";
            }
            if (playerData.Level_Player_Armor == playerData.Max_Player_Armor)
            {
                PlayerUP_Text_3.text = "Lv.MAX\n0Pt";
                PlayerUP_Button_3.interactable = false;
            }
            else
            {
                PlayerUP_Text_3.text = "Lv." + playerData.Level_Player_Armor + "\n" + playerData.Cost_Player_Armor + "Pt";
            }
            if (playerData.Level_Player_Speed == playerData.Max_Player_Speed)
            {
                PlayerUP_Text_4.text = "Lv.MAX\n0Pt";
                PlayerUP_Button_4.interactable = false;
            }
            else
            {
                PlayerUP_Text_4.text = "Lv." + playerData.Level_Player_Speed + "\n" + playerData.Cost_Player_Speed + "Pt";
            }
        }
        else
        {
            if(num == 0)
            {
                if (playerData.Level_Player_Atk == playerData.Max_Player_Atk)
                {
                    PlayerUP_Text_0.text = "Lv.MAX\n0Pt";
                    PlayerUP_Button_0.interactable = false;
                }
                else
                {
                    PlayerUP_Text_0.text = "Lv." + playerData.Level_Player_Atk + "\n" + playerData.Cost_Player_Atk + "Pt";
                }
            }else if(num == 1)
            {
                if (playerData.Level_Player_AtkDelay == playerData.Max_Player_AtkDelay)
                {
                    PlayerUP_Text_1.text = "Lv.MAX\n0Pt";
                    PlayerUP_Button_1.interactable = false;
                }
                else
                {
                    PlayerUP_Text_1.text = "Lv." + playerData.Level_Player_AtkDelay + "\n" + playerData.Cost_Player_AtkDelay + "Pt";
                }
            }
            else if(num == 2)
            {
                if (playerData.Level_Player_HP == playerData.Max_Player_HP)
                {
                    PlayerUP_Text_2.text = "Lv.MAX\n0Pt";
                    PlayerUP_Button_2.interactable = false;
                }
                else
                {
                    PlayerUP_Text_2.text = "Lv." + playerData.Level_Player_HP + "\n" + playerData.Cost_Player_HP + "Pt";
                }
            }
            else if(num == 3)
            {
                if (playerData.Level_Player_Armor == playerData.Max_Player_Armor)
                {
                    PlayerUP_Text_3.text = "Lv.MAX\n0Pt";
                    PlayerUP_Button_3.interactable = false;
                }
                else
                {
                    PlayerUP_Text_3.text = "Lv." + playerData.Level_Player_Armor + "\n" + playerData.Cost_Player_Armor + "Pt";
                }
            }
            else if(num == 4)
            {
                if (playerData.Level_Player_Speed == playerData.Max_Player_Speed)
                {
                    PlayerUP_Text_4.text = "Lv.MAX\n0Pt";
                    PlayerUP_Button_4.interactable = false;
                }
                else
                {
                    PlayerUP_Text_4.text = "Lv." + playerData.Level_Player_Speed + "\n" + playerData.Cost_Player_Speed + "Pt";
                }
            }
        }
    }

    public void Click_Player_Upgrade(int i)//LevelNum : 0 = Atk / 1= AtkDealy / 2 = HP / 3 = Armor / 4 = Speed
    {
        playerData.Player_Use_Point(playerData.Check_Cost_Player(i));
        playerData.Player_Level_Up(i);
        Check_Player_Coin_Point();
        Player_Text(false, i);
    }

    //용병 업그레이드
    public void Director_Init_MercenaryUpgrade()
    {
        Mercenary_Buy_NumList = mercenaryData.SA_MercenaryData.Mercenary_Buy_Num;
        if (Mercenary_Upgrade_Content.childCount != Mercenary_Buy_NumList.Count)
        {
            for(int i = 0; i < Mercenary_Upgrade_Content.childCount; i++)
            {
                Destroy(Mercenary_Upgrade_Content.GetChild(i).gameObject);
                Mercenary_Upgrade_Toggle.Clear();
            }
            Check_Init_MecenaryUpgradeCard();
            OnToggleStart();
        }

        MercenaryUpgrade_Button.interactable = false;
        foreach (Toggle tog in Mercenary_Upgrade_Toggle)
        {
            tog.isOn = false;
        }
        ScrollRect_Mercenary_Upgrade.normalizedPosition = Vector2.zero;
    }
    private void Check_Init_MecenaryUpgradeCard()
    {
        RectTransform ContentSize = Mercenary_Upgrade_Content.GetComponent<RectTransform>();
        if(ScrollRect_Mercenary_Upgrade.GetComponent<RectTransform>().sizeDelta.x <= 200 * Mercenary_Buy_NumList.Count + 70)
        {
            ContentSize.sizeDelta = new Vector2(100 * Mercenary_Buy_NumList.Count - 150, ContentSize.sizeDelta.y);
        }
        foreach(int num in Mercenary_Buy_NumList)
        {
            Mercenary_Upgrade_Card.GetComponent<TrainingRoom_Mercenary_Upgrade_Card>().Mercenary_Num = num;
            GameObject Card = Instantiate(Mercenary_Upgrade_Card, Mercenary_Upgrade_Content);
            Card.name = num.ToString();
            Mercenary_Upgrade_Toggle.Add(Card.GetComponentInChildren<Toggle>());
        }
    }
    private void OnToggleStart()
    {
        foreach(Toggle toggle in Mercenary_Upgrade_Toggle)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }
    private void OnToggleValueChanged(bool isOn)
    {
        int num = -1;
        if (isOn)
        {
            for (int i = 0; i < Mercenary_Upgrade_Toggle.Count; i++)
            {
                if (Mercenary_Upgrade_Toggle[i].isOn)
                {
                    Mercenary_Upgrade_ToggleNum = i;
                    num = Mercenary_Upgrade_Toggle[i].GetComponentInParent<TrainingRoom_Mercenary_Upgrade_Card>().Mercenary_Num;
                    Mercenary_Upgrade_Information_Text(true ,num);
                    Mercenary_Upgrade_Num = num;
                }
            }
            MercenaryUpgrade_Button.interactable = mercenaryData.Check_MaxLevel(num);
        }
        else
        {
            Mercenary_Upgrade_Information_Text(false);
            MercenaryUpgrade_Button.interactable = false;
        }
    }
    private void Mercenary_Upgrade_Information_Text(bool flag, int i = -1)
    {
        if (flag)
        {
            if (i == 0)
            {
                var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Engine_Driver[mercenaryData.Level_Mercenary_Engine_Driver];
                var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Engine_Driver[mercenaryData.Level_Mercenary_Engine_Driver + 1];
                Before_Mercenary_Information.text =
                            "<size=45>Before</size>" +
                            "\nHP = " + data_before.HP +
                            "\nStamina = " + data_before.Stamina +
                            "\nMoveSpeed = " + data_before.MoveSpeed +
                            "\nRefresh_Amount = " + data_before.Refresh_Amount +
                            "\nRefresh_Delay = " + data_before.Refresh_Delay +
                            "\nDef = " + data_before.Def +
                            "\nUse_Stamina = " + data_before.Use_Stamina +
                            "\nLevel_Speed = " + data_before.Level_Type_Speed +
                            "\nLevel_Fuel = " + data_before.Level_Type_Fuel +
                            "\nLevel_Def = " + data_before.Level_Type_Def;
                if (mercenaryData.Level_Mercenary_Engine_Driver + 1 == mercenaryData.Max_Mercenary_Engine_Driver + 1)
                {
                    After_Mercenary_Information.text =
                           "<size=45>After</size>" +
                           "\nMAX";
                    MercenaryUpgrade_Button_Text.text = "MAX";
                }
                else
                {
                    After_Mercenary_Information.text =
                            "<size=45>After</size>" +
                            "\nHP = " + data_after.HP +
                            "\nStamina = " + data_after.Stamina +
                            "\nMoveSpeed = " + data_after.MoveSpeed +
                            "\nRefresh_Amount = " + data_after.Refresh_Amount +
                            "\nRefresh_Delay = " + data_after.Refresh_Delay +
                            "\nDef = " + data_after.Def +
                            "\nUse_Stamina = " + data_after.Use_Stamina +
                            "\nLevel_Speed = " + data_after.Level_Type_Speed +
                            "\nLevel_Fuel = " + data_after.Level_Type_Fuel +
                            "\nLevel_Def = " + data_after.Level_Type_Def;
                    MercenaryUpgrade_Button_Text.text = "-" + mercenaryData.EX_Level_Data.Information_LevelCost[mercenaryData.Level_Mercenary_Engine_Driver].Cost_Level_Mercenary_Engine_Driver
                                                           + "G\nUpgrade";
                }
            }
            else if (i == 1)
            {
                var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Engineer[mercenaryData.Level_Mercenary_Engineer];
                var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Engineer[mercenaryData.Level_Mercenary_Engineer + 1];
                Before_Mercenary_Information.text =
                            "<size=45>Before</size>" +
                            "\nHP = " + data_before.HP +
                            "\nStamina = " + data_before.Stamina +
                            "\nMoveSpeed = " + data_before.MoveSpeed +
                            "\nRefresh_Amount = " + data_before.Refresh_Amount +
                            "\nRefresh_Delay = " + data_before.Refresh_Delay +
                            "\nDef = " + data_before.Def +
                            "\nUse_Stamina = " + data_before.Use_Stamina +
                            "\nRepair_Delay = " + data_before.Repair_Delay +
                            "\nRepair_Amount = " + data_before.Repair_Amount +
                            "\nRepair_Train_Parsent = " + data_before.Repair_Train_Parsent;
                if (mercenaryData.Level_Mercenary_Engineer + 1 == mercenaryData.Max_Mercenary_Engineer + 1)
                {
                    After_Mercenary_Information.text =
                           "<size=45>After</size>" +
                           "\nMAX";
                    MercenaryUpgrade_Button_Text.text = "MAX";
                }
                else
                {
                    After_Mercenary_Information.text =
                            "<size=45>After</size>" +
                            "\nHP = " + data_after.HP +
                            "\nStamina = " + data_after.Stamina +
                            "\nMoveSpeed = " + data_after.MoveSpeed +
                            "\nRefresh_Amount = " + data_after.Refresh_Amount +
                            "\nRefresh_Delay = " + data_after.Refresh_Delay +
                            "\nDef = " + data_after.Def +
                            "\nUse_Stamina = " + data_after.Use_Stamina +
                            "\nRepair_Delay = " + data_after.Repair_Delay +
                            "\nRepair_Amount = " + data_after.Repair_Amount +
                            "\nRepair_Train_Parsent = " + data_after.Repair_Train_Parsent;
                    MercenaryUpgrade_Button_Text.text = "-" + mercenaryData.EX_Level_Data.Information_LevelCost[mercenaryData.Level_Mercenary_Engineer].Cost_Level_Mercenary_Engineer
                                                           + "G\nUpgrade";
                }
            }
            else if (i == 2)
            {
                var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Long_Ranged[mercenaryData.Level_Mercenary_Long_Ranged];
                var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Long_Ranged[mercenaryData.Level_Mercenary_Long_Ranged + 1];
                Before_Mercenary_Information.text =
                            "<size=45>Before</size>" +
                            "\nHP = " + data_before.HP +
                            "\nStamina = " + data_before.Stamina +
                            "\nMoveSpeed = " + data_before.MoveSpeed +
                            "\nRefresh_Amount = " + data_before.Refresh_Amount +
                            "\nRefresh_Delay = " + data_before.Refresh_Delay +
                            "\nDef = " + data_before.Def +
                            "\nUse_Stamina = " + data_before.Use_Stamina +
                            "\nUnit_Attack = " + data_before.Unit_Attack +
                            "\nUnit_Atk_Delay = " + data_before.Unit_Atk_Delay +
                            "\nWorkSpeed = " + data_before.WorkSpeed;
                if (mercenaryData.Level_Mercenary_Long_Ranged + 1 == mercenaryData.Max_Mercenary_Long_Ranged + 1)
                {
                    After_Mercenary_Information.text =
                           "<size=45>After</size>" +
                           "\nMAX";
                    MercenaryUpgrade_Button_Text.text = "MAX";
                }
                else
                {
                    After_Mercenary_Information.text =
                            "<size=45>After</size>" +
                            "\nHP = " + data_after.HP +
                            "\nStamina = " + data_after.Stamina +
                            "\nMoveSpeed = " + data_after.MoveSpeed +
                            "\nRefresh_Amount = " + data_after.Refresh_Amount +
                            "\nRefresh_Delay = " + data_after.Refresh_Delay +
                            "\nDef = " + data_after.Def +
                            "\nUse_Stamina = " + data_after.Use_Stamina +
                            "\nUnit_Attack = " + data_after.Unit_Attack +
                            "\nUnit_Atk_Delay = " + data_after.Unit_Atk_Delay +
                            "\nWorkSpeed = " + data_after.WorkSpeed;
                    MercenaryUpgrade_Button_Text.text = "-" + mercenaryData.EX_Level_Data.Information_LevelCost[mercenaryData.Level_Mercenary_Long_Ranged].Cost_Level_Mercenary_Long_Ranged
                                                           + "G\nUpgrade";
                }
            }
            else if (i == 3)
            {
                var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Short_Ranged[mercenaryData.Level_Mercenary_Short_Ranged];
                var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Short_Ranged[mercenaryData.Level_Mercenary_Short_Ranged + 1];
                Before_Mercenary_Information.text =
                            "<size=45>Before</size>" +
                            "\nHP = " + data_before.HP +
                            "\nStamina = " + data_before.Stamina +
                            "\nMoveSpeed = " + data_before.MoveSpeed +
                            "\nRefresh_Amount = " + data_before.Refresh_Amount +
                            "\nRefresh_Delay = " + data_before.Refresh_Delay +
                            "\nDef = " + data_before.Def +
                            "\nUse_Stamina = " + data_before.Use_Stamina +
                            "\nUnit_Attack = " + data_before.Unit_Attack +
                            "\nUnit_Atk_Delay = " + data_before.Unit_Atk_Delay;
                if (mercenaryData.Level_Mercenary_Short_Ranged + 1 == mercenaryData.Max_Mercenary_Short_Ranged + 1)
                {
                    After_Mercenary_Information.text =
                           "<size=45>After</size>" +
                           "\nMAX";
                    MercenaryUpgrade_Button_Text.text = "MAX";
                }
                else
                {
                    After_Mercenary_Information.text =
                            "<size=45>After</size>" +
                            "\nHP = " + data_after.HP +
                            "\nStamina = " + data_after.Stamina +
                            "\nMoveSpeed = " + data_after.MoveSpeed +
                            "\nRefresh_Amount = " + data_after.Refresh_Amount +
                            "\nRefresh_Delay = " + data_after.Refresh_Delay +
                            "\nDef = " + data_after.Def +
                            "\nUse_Stamina = " + data_after.Use_Stamina +
                            "\nUnit_Attack = " + data_after.Unit_Attack +
                            "\nUnit_Atk_Delay = " + data_after.Unit_Atk_Delay;
                    MercenaryUpgrade_Button_Text.text = "-" + mercenaryData.EX_Level_Data.Information_LevelCost[mercenaryData.Level_Mercenary_Short_Ranged].Cost_Level_Mercenary_Short_Ranged
                                                           + "G\nUpgrade";
                }
            }
            else if (i == 4)
            {
                var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Medic[mercenaryData.Level_Mercenary_Medic];
                var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Medic[mercenaryData.Level_Mercenary_Medic + 1];
                Before_Mercenary_Information.text =
                            "<size=45>Before</size>" +
                            "\nHP = " + data_before.HP +
                            "\nStamina = " + data_before.Stamina +
                            "\nMoveSpeed = " + data_before.MoveSpeed +
                            "\nRefresh_Amount = " + data_before.Refresh_Amount +
                            "\nRefresh_Delay = " + data_before.Refresh_Delay +
                            "\nDef = " + data_before.Def +
                            "\nUse_Stamina = " + data_before.Use_Stamina +
                            "\nHeal_HP_Amount = " + data_before.Heal_Hp_Amount +
                            "\nHeal_Stamina_Amount = " + data_before.Heal_Stamina_Amount +
                            "\nHeal_Revive_Amount = " + data_before.Heal_Revive_Amount +
                            "\nHeal_HP_Parsent = " + data_before.Heal_HP_Parsent;
                if (mercenaryData.Level_Mercenary_Medic + 1 == mercenaryData.Max_Mercenary_Medic + 1)
                {
                    After_Mercenary_Information.text =
                           "<size=45>After</size>" +
                           "\nMAX";
                    MercenaryUpgrade_Button_Text.text = "MAX";
                }
                else
                {
                    After_Mercenary_Information.text =
                            "<size=45>After</size>" +
                            "\nHP = " + data_after.HP +
                            "\nStamina = " + data_after.Stamina +
                            "\nMoveSpeed = " + data_after.MoveSpeed +
                            "\nRefresh_Amount = " + data_after.Refresh_Amount +
                            "\nRefresh_Delay = " + data_after.Refresh_Delay +
                            "\nDef = " + data_after.Def +
                            "\nUse_Stamina = " + data_after.Use_Stamina +
                            "\nHeal_Stamina_Amount = " + data_after.Heal_Stamina_Amount +
                            "\nHeal_Revive_Amount = " + data_after.Heal_Revive_Amount +
                            "\nHeal_HP_Parsent = " + data_after.Heal_HP_Parsent;
                    MercenaryUpgrade_Button_Text.text = "-" + mercenaryData.EX_Level_Data.Information_LevelCost[mercenaryData.Level_Mercenary_Medic].Cost_Level_Mercenary_Medic
                                                           + "G\nUpgrade";
                }
            }
            MercenaryUpgrade_Button.interactable = mercenaryData.Check_MaxLevel(i);
        }
        else
        {
            Before_Mercenary_Information.text = "Choice Mercenary";
            After_Mercenary_Information.text = "Choice Mercenary";
        }
    }
    public void Mercenary_Level_Up()
    {
        playerData.Player_Buy_Coin(mercenaryData.Check_Cost_Mercenary(Mercenary_Upgrade_Num));
        mercenaryData.Mercenary_Level_Up(Mercenary_Upgrade_Num);
        Mercenary_Upgrade_Content.GetChild(Mercenary_Upgrade_ToggleNum).GetComponent<TrainingRoom_Mercenary_Upgrade_Card>().Card_LevleUP();
        Check_Player_Coin_Point();
        Mercenary_Upgrade_Information_Text(true, Mercenary_Upgrade_Num);
    }

    // 용병 배치
    public void Director_Init_MercenaryPosition()
    {
        Mercenary_Buy_NumList = mercenaryData.SA_MercenaryData.Mercenary_Buy_Num;
        if (Mercenary_Position_Content.childCount != Mercenary_Buy_NumList.Count)
        {
            for (int i = 0; i < Mercenary_Position_Content.childCount; i++)
            {
                Destroy(Mercenary_Position_Content.GetChild(i).gameObject);
                CardList.Clear();
            }
            Check_Init_MercenaryPositionCard();
        }
        ScrollRect_Mercenary_Position.normalizedPosition = Vector2.zero;
        Mercenary_Check_Button();// 카드 비활성화 되어 있는 경우 카운트를 세지 않음. 그렇기에 버튼 체크해야한다.
    }

    public void Check_Init_MercenaryPositionCard()
    {
        RectTransform ContentSize = Mercenary_Position_Content.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(200 * Mercenary_Buy_NumList.Count, ContentSize.sizeDelta.y);
        foreach (int num in Mercenary_Buy_NumList)
        {
            Mercenary_Position_Card.GetComponent<TrainingRoom_Mercenary_Position_Card>().Mercenary_Num = num;
            GameObject Card = Instantiate(Mercenary_Position_Card, Mercenary_Position_Content);
            Card.name = num.ToString();
            CardList.Add(Card);
            TrainingRoom_Mercenary_Position_Card SubCard = Card.GetComponent<TrainingRoom_Mercenary_Position_Card>();
            SubCard.PlusButton.GetComponent<Button>().onClick.AddListener(() => Mercenary_PositionUP(SubCard, num));
            SubCard.MinusButton.GetComponent<Button>().onClick.AddListener(() => Mercenary_PositionDown(SubCard, num));
            if (num == 0)
            {
                DropDown_EngineDriver_Type = SubCard.dropDown.GetComponent<TMP_Dropdown>();
                DropDown_EngineDriver_Type.onValueChanged.AddListener(Mercenary_Position_EngineDriver_DropDown);
            }
        }
    }

    public void Mercenary_PositionUP(TrainingRoom_Mercenary_Position_Card Card, int i)
    {
        mercenaryData.Mercenary_Num.Add(i);
        Mercenary_TotalNum = mercenaryData.Mercenary_Num.Count;
        Card.Plus_Count();
        Mercenary_Check_Button();
        Mercenary_Position_Text();
    }

    public void Mercenary_PositionDown(TrainingRoom_Mercenary_Position_Card Card, int i)
    {
        mercenaryData.Mercenary_Num.Remove(i);
        Mercenary_TotalNum = mercenaryData.Mercenary_Num.Count;
        Card.Minus_Count();
        Mercenary_Check_Button();
        Mercenary_Position_Text();
    }

    private void Mercenary_Position_Text()
    {
        Mercenary_Position_Information.text =
            "<color=red><size=45> MAX Mercenary Count\n</size>" +
            "Max : " + EngineTier_MaxMercenary + "</color>\n" + Mercenary_Position_List_Text();
    }

    private string Mercenary_Position_List_Text()
    {
        string str = "";
        for (int i = 0; i < Mercenary_Position_NumList.Count; i++)
        {
            str += (i + 1) + ". " + mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_Position_NumList[i]].Name + "\n";
        }

        return str;
    }

    private void Mercenary_Check_Button()
    {
        if (Mercenary_TotalNum >= EngineTier_MaxMercenary)
        {
            for(int i = 0; i < CardList.Count; i++)
            {
                CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().PlusButton.interactable = false;
            }

            for (int i = 0; i < CardList.Count; i++)
            {
                int Mercenary_Count = CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().Mercenary_Num_Count;
                if (Mercenary_Count == 0)
                {
                    CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().MinusButton.interactable = false;
                }
                else
                {
                    CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().MinusButton.interactable = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < CardList.Count; i++)
            {
                if (CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().Mercenary_Num == 0)
                {
                    if (CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().Mercenary_Num_Count == 1)
                    {
                        CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().Button_OpenClose(true);
                    }
                    else
                    {
                        CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().Button_OpenClose(false);
                    }
                }
                else
                {
                    CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().PlusButton.interactable = true;
                }
            }

            for (int i = 0; i < CardList.Count; i++)
            {
                if (CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().Mercenary_Num != 0)
                {
                    if (CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().Mercenary_Num_Count == 0)
                    {
                        CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().MinusButton.interactable = false;
                    }
                    else
                    {
                        CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().MinusButton.interactable = true;
                    }
                }
            }
        }
    }

    public void Mercenary_Position_EngineDriver_DropDown(int value)
    {
        mercenaryData.SA_MercenaryData.SA_Change_EngineDriver_Type(value);
    }

    private void Check_Player_Coin_Point()
    {
        transform.GetComponentInParent<StationDirector>().Check_CoinAndPoint();
    }
}