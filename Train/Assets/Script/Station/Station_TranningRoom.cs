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
    int Mercenary_Upgrade_Num;
    public Toggle[] Mercenary_Upgrade_Toggle;
    public TextMeshProUGUI[] Mercenary_Level_Text;
    public TextMeshProUGUI Before_Mercenary_Information;
    public TextMeshProUGUI After_Mercenary_Information;
    public Button MercenaryUpgrade_Button;
    TextMeshProUGUI MercenaryUpgrade_Button_Text;

    [Header("용병 배치 윈도우")]
    public TextMeshProUGUI Mercenary_Position_Information;
    public TextMeshProUGUI[] Mercenary_Information_SubText;
    public Button[] Plus_Button;
    public Button[] Minus_Button;
    List<int> Mercenary_NumList;
    int EngineTier_MaxMercenary;
    int[] Mercenary_Num;
    int Mercenary_TotalNum;
    public TMP_Dropdown DropDown_EngineDriver_Type;
    void Start()
    {
        //데이터 수집
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        //플레이어 업그레이드 윈도우
        Player_Text(true);
        //용병 업그레이드 윈도우
        MercenaryUpgrade_Button_Text = MercenaryUpgrade_Button.GetComponentInChildren<TextMeshProUGUI>();
        Before_Mercenary_Information.text = "<size=30>Before_Click Mercenary</size>";
        After_Mercenary_Information.text = "<size=30>Before_Click Mercenary</size>";
        //용병 배치 윈도우
        Mercenary_NumList = mercenaryData.Mercenary_Num;
        Mercenary_Num = new int[Plus_Button.Length];
        for(int i = 0; i < Mercenary_Num.Length; i++)
        {
            Mercenary_Num[i] = 0;
        }
        for (int i = 0; i < Mercenary_NumList.Count; i++)
        {
            Mercenary_Num[Mercenary_NumList[i]]++;
        }
        for(int i = 0; i < Mercenary_Num.Length; i++)
        {
            Mercenary_TotalNum += Mercenary_Num[i]; 
        }    
        EngineTier_MaxMercenary = trainData.Max_Train_MaxMercenary;
        Mecenary_Position_Button();
        Mercenary_Position_Text();
        Mercenary_Position_EngineDriver_Type();

        OnToggleStart();
    }

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
        playerData.Player_Level_Up(i);
        Player_Text(false, i);
    }

    private void OnToggleStart()
    {
        foreach (var toggle in Mercenary_Upgrade_Toggle)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
        Mercenary_Level_Text[0].text = "Lv." + mercenaryData.Level_Mercenary_Engine_Driver;
        Mercenary_Level_Text[1].text = "Lv." + mercenaryData.Level_Mercenary_Engineer;
        Mercenary_Level_Text[2].text = "Lv." + mercenaryData.Level_Mercenary_Long_Ranged;
        Mercenary_Level_Text[3].text = "Lv." + mercenaryData.Level_Mercenary_Short_Ranged;
        Mercenary_Level_Text[4].text = "Lv." + mercenaryData.Level_Mercenary_Medic;
    }

    private void OnToggleValueChanged(bool isOn)
    {
        for (int i = 0; i < Mercenary_Upgrade_Toggle.Length; i++)
        {
            if (Mercenary_Upgrade_Toggle[i].isOn)
            {
                Mercenary_Upgrade_Information_Text(i);
                Mercenary_Upgrade_Num = i;
            }
        }
    }

    public void Mercenary_Level_Up()
    {
        mercenaryData.Mercenary_Level_Up(Mercenary_Upgrade_Num);

        Mercenary_Level_Text[0].text = "Lv." + mercenaryData.Level_Mercenary_Engine_Driver;
        Mercenary_Level_Text[1].text = "Lv." + mercenaryData.Level_Mercenary_Engineer;
        Mercenary_Level_Text[2].text = "Lv." + mercenaryData.Level_Mercenary_Long_Ranged;
        Mercenary_Level_Text[3].text = "Lv." + mercenaryData.Level_Mercenary_Short_Ranged;
        Mercenary_Level_Text[4].text = "Lv." + mercenaryData.Level_Mercenary_Medic;
        Mercenary_Upgrade_Information_Text(Mercenary_Upgrade_Num);
    }

    private void Mercenary_Upgrade_Information_Text(int i)
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
                MercenaryUpgrade_Button.interactable = false;
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
                MercenaryUpgrade_Button.interactable = true;
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
                MercenaryUpgrade_Button.interactable = false;
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
                MercenaryUpgrade_Button.interactable = true;
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
                MercenaryUpgrade_Button.interactable = false;
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
                MercenaryUpgrade_Button.interactable = true;
                MercenaryUpgrade_Button_Text.text = "-" + mercenaryData.EX_Level_Data.Information_LevelCost[mercenaryData.Level_Mercenary_Long_Ranged].Cost_Level_Mercenary_Long_Ranged
                                                       + "G\nUpgrade";
            }
        } else if (i == 3)
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
            if (mercenaryData.Level_Mercenary_Short_Ranged + 1 == mercenaryData.Max_Mercenary_Short_Ranged+1)
            {
                After_Mercenary_Information.text =
                       "<size=45>After</size>" +
                       "\nMAX";
                MercenaryUpgrade_Button.interactable = false;
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
                MercenaryUpgrade_Button.interactable = true;
                MercenaryUpgrade_Button_Text.text = "-" + mercenaryData.EX_Level_Data.Information_LevelCost[mercenaryData.Level_Mercenary_Short_Ranged].Cost_Level_Mercenary_Short_Ranged
                                                       + "G\nUpgrade";
            }
        }
        else if(i == 4)
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
            if(mercenaryData.Level_Mercenary_Medic + 1 == mercenaryData.Max_Mercenary_Medic + 1)
            {
                After_Mercenary_Information.text =
                       "<size=45>After</size>" +
                       "\nMAX";
                MercenaryUpgrade_Button.interactable = false;
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
                MercenaryUpgrade_Button.interactable = true;
                MercenaryUpgrade_Button_Text.text = "-" + mercenaryData.EX_Level_Data.Information_LevelCost[mercenaryData.Level_Mercenary_Medic].Cost_Level_Mercenary_Medic
                                                       + "G\nUpgrade";
            }
        }
    }

    private void Mercenary_Position(bool List_Up_Down, int M_Num) //버튼 관리도 한다
    {
        if (List_Up_Down)
        {
            Mercenary_NumList.Add(M_Num);
            mercenaryData.Mercenary_Num = Mercenary_NumList;
        }
        else if(!List_Up_Down)
        {
            Mercenary_NumList.Remove(M_Num);
            mercenaryData.Mercenary_Num = Mercenary_NumList;
        }

        Mercenary_TotalNum = 0;
        for (int i = 0; i < Mercenary_Num.Length; i++) {
            Mercenary_TotalNum += Mercenary_Num[i];
        }
        Mercenary_Position_Text();
        Mecenary_Position_Button();
    }

    private void Mecenary_Position_Button()
    {
        if(Mercenary_TotalNum == EngineTier_MaxMercenary)
        {
            foreach(Button btn in Plus_Button)
            {
                btn.interactable = false;
            }
            for(int i = 0; i < Minus_Button.Length; i++)
            {
                int M_Num = 0;
                M_Num = Mercenary_Num[i];

                if (M_Num == 0)
                {
                    Minus_Button[i].interactable = false;
                }
                else
                {
                    Minus_Button[i].interactable = true;
                }
            }
        }
        else
        {
            if (Mercenary_Num[0] == 1)
            {
                Plus_Button[0].interactable = false;
                Minus_Button[0].interactable = true;
            }
            else
            {
                Plus_Button[0].interactable = true;
                Minus_Button[0].interactable = false;
            }

            for (int i = 1; i < Plus_Button.Length; i++)
            {
                int M_Num = 0;
                M_Num = Mercenary_Num[i];
                
                if (M_Num == 0)
                {
                    Minus_Button[i].interactable = false;
                }
                else
                {
                    Minus_Button[i].interactable = true;
                }
                Plus_Button[i].interactable = true;
            }
        }
    }

    public void Mercenary_PositionUP(int i)
    {
        Mercenary_Num[i]++;
        Mercenary_Position(true, i);
    }

    public void Mercenary_PositionDown(int i)
    {
        Mercenary_Num[i]--;
        Mercenary_Position(false, i);
    }

    private void Mercenary_Position_Text()
    {
        Mercenary_Position_Information.text =
            "<color=red><size=45> MAX Mercenary Count\n</size>" +
            "Max : " + EngineTier_MaxMercenary + "</color>\n" + Mercenary_Position_List();

        for(int i = 0; i < Mercenary_Information_SubText.Length; i++)
        {
            if(i == 0) { 
                Mercenary_Information_SubText[i].text =
                    mercenaryData.EX_Game_Data.Information_Mercenary[i].Name + "\n" + Mercenary_Num[i] + " <color=red>/Max 1</color>";
            }
            else
            {
                Mercenary_Information_SubText[i].text =
                    mercenaryData.EX_Game_Data.Information_Mercenary[i].Name + "\n" + Mercenary_Num[i];
            }
        }
    }

    private string Mercenary_Position_List()
    {
        string str = "";
        for(int i = 0; i < Mercenary_NumList.Count; i++)
        {
            str += (i+1) + ". " + mercenaryData.EX_Game_Data.Information_Mercenary[Mercenary_NumList[i]].Name + "\n";
        }

        return str;
    }

    private void Mercenary_Position_EngineDriver_Type()
    {
        if(mercenaryData.SA_MercenaryData.Engine_DriverType == Engine_Driver_Type.speed)
        {
            DropDown_EngineDriver_Type.value = 0;
        }else if(mercenaryData.SA_MercenaryData.Engine_DriverType == Engine_Driver_Type.fuel)
        {
            DropDown_EngineDriver_Type.value = 1;
        }
        else if (mercenaryData.SA_MercenaryData.Engine_DriverType == Engine_Driver_Type.def)
        {
            DropDown_EngineDriver_Type.value = 2;
        }
    }
    public void Mercenary_Position_EngineDriver_DropDown()
    {
        mercenaryData.SA_MercenaryData.SA_Change_EngineDriver_Type(DropDown_EngineDriver_Type.value);
    }
}