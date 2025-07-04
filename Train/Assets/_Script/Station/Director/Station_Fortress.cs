 using UnityEngine.Localization.Components;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Station_Fortress : MonoBehaviour
{
    [Header("메인 디렉터")]
    public StationDirector mainDirector;

    [Header("데이터 모음")]
    public GameObject Player_DataObject;
    Station_PlayerData playerData;
    //public GameObject Train_DataObject;
/*    public Station_TrainData trainData;
    public GameObject Mercenary_DataObject;
    Station_MercenaryData mercenaryData;
    public GameObject Item_DataObject;
    Station_ItemData itemData;
    public GameObject Inventory_Director;
    Station_Inventory inventorydirector;*/

/*    List<int> Mercenary_Buy_NumList;// 구매한 리스트
    List<int> Mercenary_Position_NumList;// 배치하고 있는 리스트
*/
    [Header("플레이어 업그레이드 윈도우")]
    public LocalizeStringEvent Player_Name;
    public TextMeshProUGUI[] PlayerUP_Text;
    public TextMeshProUGUI[] PlayerUPCost_Text;
    public Button [] PlayerUP_Button;
    int playerNum;
    int lockoff_playerNum;
    public Image PlayerHead;
    public Sprite[] PlayerHead_Image;
    
    public TextMeshProUGUI[] Player_Information;

    public GameObject Warning_Coin_Window;

    public bool Tranining_BanFlag;
    bool UpgradeFlag;
    int UpgradeCardNum;

 /*   [Header("용병 업그레이드 윈도우")]
    public Button MercenaryUpgrade_Button;
    [SerializeField]
    int Mercenary_Upgrade_Num;
    int Mercenary_Upgrade_ToggleNum;
    public ScrollRect ScrollRect_Mercenary_Upgrade; 
    public Transform Mercenary_Upgrade_Content;
    public GameObject Mercenary_Upgrade_Card;

    public GameObject Mercenary_Information_Upgarade_Object;
    public TextMeshProUGUI Before_Mercenary_Information;
    public TextMeshProUGUI After_Mercenary_Information;
    public Image Mercenary_Material_Image;
    public TextMeshProUGUI[] Mercenary_Upgrade_Text;
    [SerializeField]
    List<Toggle> Mercenary_Upgrade_Toggle;

    [Header("용병 배치 윈도우")]
    public ScrollRect ScrollRect_Mercenary_Position;
    public Transform Mercenary_Position_Content;
    public Transform Mercenary_On_Board_Card_List;
    public GameObject Mercenary_Position_Card;
    public GameObject Mercenary_On_Board_Card;
    public TextMeshProUGUI Mercenary_Position_Information;
    int MaxMercenary;
    int Mercenary_TotalNum;
    [SerializeField]
    List<GameObject> CardList;
    [SerializeField]
    TMP_Dropdown DropDown_EngineDriver_Type;
    [SerializeField]
    TMP_Dropdown DropDown_Bard_Type;

    [SerializeField]
    LocalizedString[] local_string;*/
    void Start()
    {
        //데이터 수집
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        //trainData = Train_DataObject.GetComponent<Station_TrainData>();
        //MaxMercenary = trainData.Max_Train_MaxMercenary + 1;
        //Debug.Log(EngineTier_MaxMercenary);
/*        mercenaryData = Mercenary_DataObject.GetComponent<Station_MercenaryData>();
        itemData = Item_DataObject.GetComponent<Station_ItemData>();
        inventorydirector = Inventory_Director.GetComponent<Station_Inventory>();
        Mercenary_Buy_NumList = mercenaryData.SA_MercenaryData.Mercenary_Buy_Num;
        Mercenary_Position_NumList = mercenaryData.SA_MercenaryData.Mercenary_Num;*/
        //플레이어 업그레이드 윈도우
        playerNum = 0;

        lockoff_playerNum = playerData.SA_PlayerData.SA_CheckCharecter_Num(); 

        Player_Name.StringReference.TableReference = "ExcelData_Table_St";
        Player_Name.StringReference.TableEntryReference = "Player_Name_" + playerNum;
        Player_Information_Text();
        for(int i = 0; i < 5; i++)
        {
            Player_Button_Text(i);
        }
/*        //용병 업그레이드 윈도우
        Check_Init_MecenaryUpgradeCard();
        Mercenary_Information_Upgarade_Object.SetActive(false);
        Mercenary_Material_Image.sprite = itemData.Mercenary_Material_object.Item_Sprite;
        OnToggleStart();

        //용병 배치 윈도우
        Mercenary_TotalNum = mercenaryData.SA_MercenaryData.Mercenary_Num.Count;
        Check_Init_MercenaryPositionCard();
        Mercenary_Position_List_Init_Card();
        Mercenary_Position_Max_Text();*/
    }

    private void Update()
    {
        if (UpgradeFlag)
        {
            Check_Player_Coin_Point();
            Player_Information_Text();
            Player_Button_Text(UpgradeCardNum);
            UpgradeFlag = false;
        }
    }

    //플레이어 업그레이드
    private void Player_Button_Text(int num)
    {
        if (playerData.Level_Player_Atk == playerData.Max_Player_Atk)
        {
            PlayerUP_Text[0].text = "MAX";
            PlayerUPCost_Text[0].text = "----";
            PlayerUP_Button[0].interactable = false;
        }
        else
        {
            PlayerUP_Text[0].text = "Lv." + playerData.Level_Player_Atk;
            PlayerUPCost_Text[0].text = playerData.Cost_Player_Atk + " G";
        }

        if (playerData.Level_Player_AtkDelay == playerData.Max_Player_AtkDelay)
        {
            PlayerUP_Text[1].text = "MAX";
            PlayerUPCost_Text[1].text = "----";
            PlayerUP_Button[1].interactable = false;
        }
        else
        {
            PlayerUP_Text[1].text = "Lv." + playerData.Level_Player_AtkDelay;
            PlayerUPCost_Text[1].text = playerData.Cost_Player_AtkDelay + " G";
        }

        if (playerData.Level_Player_Armor == playerData.Max_Player_Armor)
        {
            PlayerUP_Text[2].text = "MAX";
            PlayerUPCost_Text[2].text = "----";
            PlayerUP_Button[2].interactable = false;
        }
        else
        {
            PlayerUP_Text[2].text = "Lv." + playerData.Level_Player_Armor;
            PlayerUPCost_Text[2].text = playerData.Cost_Player_Armor + " G";
        }

        if (playerData.Level_Player_Speed == playerData.Max_Player_Speed)
        {
            PlayerUP_Text[3].text = "MAX";
            PlayerUPCost_Text[3].text = "----";
            PlayerUP_Button[3].interactable = false;
        }
        else
        {
            PlayerUP_Text[3].text = "Lv." + playerData.Level_Player_Speed;
            PlayerUPCost_Text[3].text = playerData.Cost_Player_Speed + " G";
        }

        if (playerData.Level_Player_HP == playerData.Max_Player_HP)
        {
            PlayerUP_Text[4].text = "MAX";
            PlayerUPCost_Text[4].text = "----";
            PlayerUP_Button[4].interactable = false;
        }
        else
        {
            PlayerUP_Text[4].text = "Lv." + playerData.Level_Player_HP;
            PlayerUPCost_Text[4].text = playerData.Cost_Player_HP + " G";
        }
    }

    private void Player_Information_Text()
    {
        PlayerHead.sprite = PlayerHead_Image[playerNum];
        Player_Name.StringReference.TableEntryReference = "Player_Name_" + playerNum;
        Player_Information[0].text = playerData.EX_Game_Data.Information_Player[playerNum].Player_Atk + "<color=#05FF00> + " + (playerData.EX_Game_Data.Information_Player[playerNum].Player_Atk * playerData.Level_Player_Atk * 5) / 100 + "</color>";
        Player_Information[1].text = playerData.EX_Game_Data.Information_Player[playerNum].Player_Delay + "<color=#05FF00> - " + ((playerData.EX_Game_Data.Information_Player[playerNum].Player_Delay * playerData.Level_Player_AtkDelay) / 50).ToString("F3") + "</color>";
        Player_Information[2].text = playerData.EX_Game_Data.Information_Player[playerNum].Player_Armor + "<color=#05FF00> + " + ((playerData.EX_Game_Data.Information_Player[playerNum].Player_Armor * playerData.Level_Player_Armor * 10) / 100) + "</color>";
        Player_Information[3].text = playerData.EX_Game_Data.Information_Player[playerNum].Player_MoveSpeed + "<color=#05FF00> + " + ((float)(playerData.EX_Game_Data.Information_Player[playerNum].Player_MoveSpeed * playerData.Level_Player_Speed) / 50).ToString("F2") + "</color>";
        Player_Information[4].text = playerData.EX_Game_Data.Information_Player[playerNum].Player_HP + "<color=#05FF00> + " + ((playerData.EX_Game_Data.Information_Player[playerNum].Player_HP * playerData.Level_Player_HP * 10) / 100) + "</color>";
        for (int i = 0; i < Player_Information.Length; i++)
        {
            Player_Information[i].ForceMeshUpdate();
        }
    }

    public void Click_Player_Next()
    {
        if(lockoff_playerNum - 1!= playerNum)
        {
            playerNum++;
        }
        else
        {
            playerNum = 0;
        }
        Player_Information_Text();
    }

    public void Click_Player_Prev()
    {
        if(playerNum != 0)
        {
            playerNum--;
        }
        else
        {
            playerNum = lockoff_playerNum - 1;
        }
        Player_Information_Text();
    }

    public void Click_Player_Upgrade(int i)//LevelNum : 0 = Atk / 1= AtkDealy / 2 = Armor / 3 = Speed / 4 = HP
    {
        int coin = playerData.Check_Cost_Player(i);
        if (playerData.Player_Coin >= coin)
        {
            UpgradeCardNum = i;
            mainDirector.BuySoundSFX(true);
            UnityMainThreadExecutor.ExecuteOnMainThread(() => playerUpgrade(i, coin));
        }
        else
        {
            mainDirector.BuySoundSFX(false);
            Open_Warning_Window();
        }
    }

    async void playerUpgrade(int CardNum, int coin)
    {
        await Task.Run(() =>
        {
            playerData.Player_Buy_Coin(coin);
            playerData.Player_Level_Up(CardNum);
            UpgradeFlag = true;
        });
    }

  /*  //용병 업그레이드
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
        ScrollRect_Mercenary_Upgrade.normalizedPosition = Vector2.up;
    }
    private void Check_Init_MecenaryUpgradeCard()
    {
        RectTransform ContentSize = Mercenary_Upgrade_Content.GetComponent<RectTransform>();
        foreach(int num in Mercenary_Buy_NumList)
        {
            Mercenary_Upgrade_Card.GetComponent<TrainingRoom_Mercenary_Upgrade_Card>().Mercenary_Num = num;
            GameObject Card = Instantiate(Mercenary_Upgrade_Card, Mercenary_Upgrade_Content);
            Card.name = num.ToString();
            Mercenary_Upgrade_Toggle.Add(Card.GetComponentInChildren<Toggle>());
        }
        Mercenary_Upgrade_Text[1].text = "0";
        Mercenary_Upgrade_Text[2].text = "0";
        ResizedContent_V(Mercenary_Upgrade_Content, ScrollRect_Mercenary_Upgrade);
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
                    Mercenary_Upgrade_Information_Text(num);
                    Mercenary_Upgrade_Num = num;
                }
            }
            Mercenary_Information_Upgarade_Object.SetActive(true);
            MercenaryUpgrade_Button.interactable = mercenaryData.Check_MaxLevel(num);
        }
        else
        {
            Mercenary_Upgrade_Text[0].text = "0G";
            Mercenary_Information_Upgarade_Object.SetActive(false);
            MercenaryUpgrade_Button.interactable = false;
        }
    }
    private void Mercenary_Upgrade_Information_Text(int i = -1)
    {
        *//*{
             0 : 최대 스피드 증가량
             1 : 연료 효율성 증가량
             2 : 기차 방어력 증가량
             3 : 수리 속도
             4 : 수리 회복량
             5 : 수리 가능한 최소 HP
             6 : 공격력
             7 : 공격 속도
             8 : 회복량
             9 : 회복 가능한 최소 HP
            10 : 유닛 체력 증가량
            11 : 유닛 공격력 증갸량
            12 : 유닛 방어력 증가량
            13 : 행동력
        }*//*
        int Material_Max_Count = itemData.Mercenary_Material_object.Item_Count;
        if (i == 0)
        {
            var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Engine_Driver[mercenaryData.Level_Mercenary[i]];
            var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Engine_Driver[mercenaryData.Level_Mercenary[i] + 1];
            Before_Mercenary_Information.text =
                        local_string[0].GetLocalizedString()+ " : " + data_before.Level_Type_Speed +
                        "\n"+ local_string[1].GetLocalizedString() + " : " + data_before.Level_Type_Fuel +
                        "\n"+ local_string[2].GetLocalizedString() + " : " + data_before.Level_Type_Def;
            if (mercenaryData.Level_Mercenary[i] + 1 == mercenaryData.Max_Mercenary[i] + 1)
            {
                After_Mercenary_Information.text =
                        "MAX";
                Mercenary_Upgrade_Text[0].text = "MAX";
                Mercenary_Upgrade_Text[1].text = "MAX";
                Mercenary_Upgrade_Text[2].text = "MAX";
            }
            else
            {
                After_Mercenary_Information.text =
                        local_string[0].GetLocalizedString() + " : " + data_after.Level_Type_Speed +
                        "\n" + local_string[1].GetLocalizedString() + " : " + data_after.Level_Type_Fuel +
                        "\n" + local_string[2].GetLocalizedString() + " : " + data_after.Level_Type_Def;
                Mercenary_Upgrade_Text[0].text = mercenaryData.EX_Level_Data.Level_Mercenary_Engine_Driver[mercenaryData.Level_Mercenary[i]].Upgrade_Cost + "G";
                if(Material_Max_Count >= mercenaryData.EX_Level_Data.Level_Mercenary_Engine_Driver[mercenaryData.Level_Mercenary[i]].Material)
                {
                    Mercenary_Upgrade_Text[1].text = Material_Max_Count.ToString();
                }
                else
                {
                    Mercenary_Upgrade_Text[1].text = "<color=red>" + Material_Max_Count + "</color>";
                }
                Mercenary_Upgrade_Text[2].text = mercenaryData.EX_Level_Data.Level_Mercenary_Engine_Driver[mercenaryData.Level_Mercenary[i]].Material.ToString();
            }
        }
        else if (i == 1)
        {
            var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Engineer[mercenaryData.Level_Mercenary[i]];
            var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Engineer[mercenaryData.Level_Mercenary[i] + 1];
            Before_Mercenary_Information.text =
                        local_string[3].GetLocalizedString() + " : " + data_before.Repair_Delay +
                        "\n" + local_string[4].GetLocalizedString() + " : " + data_before.Repair_Amount +
                        "\n" + local_string[5].GetLocalizedString() + " : " + data_before.Repair_Train_Parsent;
            if (mercenaryData.Level_Mercenary[i] + 1 == mercenaryData.Max_Mercenary[i] + 1)
            {
                After_Mercenary_Information.text =
                        "MAX";
                Mercenary_Upgrade_Text[0].text = "MAX";
                Mercenary_Upgrade_Text[1].text = "MAX";
                Mercenary_Upgrade_Text[2].text = "MAX";
            }
            else
            {
                After_Mercenary_Information.text =
                        local_string[3].GetLocalizedString() + " : " + data_after.Repair_Delay +
                        "\n" + local_string[4].GetLocalizedString() + " : " + data_after.Repair_Amount +
                        "\n" + local_string[5].GetLocalizedString() + " : " + data_after.Repair_Train_Parsent;
                Mercenary_Upgrade_Text[0].text = mercenaryData.EX_Level_Data.Level_Mercenary_Engineer[mercenaryData.Level_Mercenary[i]].Upgrade_Cost + "G";
                if (Material_Max_Count >= mercenaryData.EX_Level_Data.Level_Mercenary_Engineer[mercenaryData.Level_Mercenary[i]].Material)
                {
                    Mercenary_Upgrade_Text[1].text = Material_Max_Count.ToString();
                }
                else
                {
                    Mercenary_Upgrade_Text[1].text = "<color=red>" + Material_Max_Count + "</color>";
                }
                Mercenary_Upgrade_Text[2].text = mercenaryData.EX_Level_Data.Level_Mercenary_Engineer[mercenaryData.Level_Mercenary[i]].Material.ToString();
            }
        }
        else if (i == 2)
        {
            var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Long_Ranged[mercenaryData.Level_Mercenary[i]];
            var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Long_Ranged[mercenaryData.Level_Mercenary[i] + 1];
            Before_Mercenary_Information.text =
                        local_string[6].GetLocalizedString() + " : " + data_before.Unit_Attack +
                        "\n" + local_string[7].GetLocalizedString() + " : " + data_before.Unit_Atk_Delay;
            if (mercenaryData.Level_Mercenary[i] + 1 == mercenaryData.Max_Mercenary[i] + 1)
            {
                After_Mercenary_Information.text =
                        "MAX";
                Mercenary_Upgrade_Text[0].text = "MAX";
                Mercenary_Upgrade_Text[1].text = "MAX";
                Mercenary_Upgrade_Text[2].text = "MAX";
            }
            else
            {
                After_Mercenary_Information.text =
                        local_string[6].GetLocalizedString() + " : " + data_after.Unit_Attack +
                        "\n" + local_string[7].GetLocalizedString() + " : " + data_after.Unit_Atk_Delay;
                Mercenary_Upgrade_Text[0].text = mercenaryData.EX_Level_Data.Level_Mercenary_Long_Ranged[mercenaryData.Level_Mercenary[i]].Upgrade_Cost + "G";
                if (Material_Max_Count >= mercenaryData.EX_Level_Data.Level_Mercenary_Long_Ranged[mercenaryData.Level_Mercenary[i]].Material)
                {
                    Mercenary_Upgrade_Text[1].text = Material_Max_Count.ToString();
                }
                else
                {
                    Mercenary_Upgrade_Text[1].text = "<color=red>" + Material_Max_Count + "</color>";
                }
                Mercenary_Upgrade_Text[2].text = mercenaryData.EX_Level_Data.Level_Mercenary_Long_Ranged[mercenaryData.Level_Mercenary[i]].Material.ToString();
            }
        }
        else if (i == 3)
        {
            var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Short_Ranged[mercenaryData.Level_Mercenary[i]];
            var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Short_Ranged[mercenaryData.Level_Mercenary[i] + 1];
            Before_Mercenary_Information.text =
                       local_string[6].GetLocalizedString() + " : " + data_before.Unit_Attack +
                        "\n" + local_string[7].GetLocalizedString() + " : " + data_before.Unit_Atk_Delay;
            if (mercenaryData.Level_Mercenary[i] + 1 == mercenaryData.Max_Mercenary[i] + 1)
            {
                After_Mercenary_Information.text =
                        "MAX";
                Mercenary_Upgrade_Text[0].text = "MAX";
                Mercenary_Upgrade_Text[1].text = "MAX";
                Mercenary_Upgrade_Text[2].text = "MAX";
            }
            else
            {
                After_Mercenary_Information.text =
                        local_string[6].GetLocalizedString() + " : " + data_after.Unit_Attack +
                        "\n" + local_string[7].GetLocalizedString() + " : " + data_after.Unit_Atk_Delay;
                Mercenary_Upgrade_Text[0].text = mercenaryData.EX_Level_Data.Level_Mercenary_Short_Ranged[mercenaryData.Level_Mercenary[i]].Upgrade_Cost + "G";
                if (Material_Max_Count >= mercenaryData.EX_Level_Data.Level_Mercenary_Short_Ranged[mercenaryData.Level_Mercenary[i]].Material)
                {
                    Mercenary_Upgrade_Text[1].text = Material_Max_Count.ToString();
                }
                else
                {
                    Mercenary_Upgrade_Text[1].text = "<color=red>" + Material_Max_Count + "</color>";
                }
                Mercenary_Upgrade_Text[2].text = mercenaryData.EX_Level_Data.Level_Mercenary_Short_Ranged[mercenaryData.Level_Mercenary[i]].Material.ToString();
            }
        }
        else if (i == 4)
        {
            var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Medic[mercenaryData.Level_Mercenary[i]];
            var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Medic[mercenaryData.Level_Mercenary[i] + 1];
            Before_Mercenary_Information.text =
                        local_string[8].GetLocalizedString() + " : " + data_before.Heal_Hp_Amount +
                        "\n" + local_string[9].GetLocalizedString() + " : " + data_before.Heal_HP_Parsent;
            if (mercenaryData.Level_Mercenary[i] + 1 == mercenaryData.Max_Mercenary[i] + 1)
            {
                After_Mercenary_Information.text =
                        "MAX";
                Mercenary_Upgrade_Text[0].text = "MAX";
                Mercenary_Upgrade_Text[1].text = "MAX";
                Mercenary_Upgrade_Text[2].text = "MAX";
            }
            else
            {
                After_Mercenary_Information.text =
                       local_string[8].GetLocalizedString() + " : " + data_after.Heal_Hp_Amount +
                        "\n" + local_string[9].GetLocalizedString() + " : " + data_after.Heal_HP_Parsent;
                Mercenary_Upgrade_Text[0].text = mercenaryData.EX_Level_Data.Level_Mercenary_Medic[mercenaryData.Level_Mercenary[i]].Upgrade_Cost + "G";
                if (Material_Max_Count >= mercenaryData.EX_Level_Data.Level_Mercenary_Medic[mercenaryData.Level_Mercenary[i]].Material)
                {
                    Mercenary_Upgrade_Text[1].text = Material_Max_Count.ToString();
                }
                else
                {
                    Mercenary_Upgrade_Text[1].text = "<color=red>" + Material_Max_Count + "</color>";
                }
                Mercenary_Upgrade_Text[2].text = mercenaryData.EX_Level_Data.Level_Mercenary_Medic[mercenaryData.Level_Mercenary[i]].Material.ToString();
            }
        }
        else if(i == 5)
        {
            var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_Bard[mercenaryData.Level_Mercenary[i]];
            var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_Bard[mercenaryData.Level_Mercenary[i] + 1];
            Before_Mercenary_Information.text =
                        local_string[10].GetLocalizedString() + " : " + data_before.Level_Type_HP_Buff +
                        "\n" + local_string[11].GetLocalizedString() + " : " + data_before.Level_Type_Atk_Buff +
                        "\n" + local_string[12].GetLocalizedString() + " : " + data_before.Level_Type_Def_Buff;
            if (mercenaryData.Level_Mercenary[i] + 1 == mercenaryData.Max_Mercenary[i] + 1)
            {
                After_Mercenary_Information.text =
                        "MAX";
                Mercenary_Upgrade_Text[0].text = "MAX";
                Mercenary_Upgrade_Text[1].text = "MAX";
                Mercenary_Upgrade_Text[2].text = "MAX";

            }
            else
            {
                After_Mercenary_Information.text =
                        local_string[10].GetLocalizedString() + " : " + data_after.Level_Type_HP_Buff +
                        "\n" + local_string[11].GetLocalizedString() + " : " + data_after.Level_Type_Atk_Buff +
                        "\n" + local_string[12].GetLocalizedString() + " : " + data_after.Level_Type_Def_Buff;
                Mercenary_Upgrade_Text[0].text = mercenaryData.EX_Level_Data.Level_Mercenary_Bard[mercenaryData.Level_Mercenary[i]].Upgrade_Cost + "G";
                if (Material_Max_Count >= mercenaryData.EX_Level_Data.Level_Mercenary_Bard[mercenaryData.Level_Mercenary[i]].Material)
                {
                    Mercenary_Upgrade_Text[1].text = Material_Max_Count.ToString();
                }
                else
                {
                    Mercenary_Upgrade_Text[1].text = "<color=red>" + Material_Max_Count + "</color>";
                }
                Mercenary_Upgrade_Text[2].text = mercenaryData.EX_Level_Data.Level_Mercenary_Bard[mercenaryData.Level_Mercenary[i]].Material.ToString();
            }
        }
        else if(i == 6)
        {
            var data_before = mercenaryData.EX_Level_Data.Level_Mercenary_CowBoy[mercenaryData.Level_Mercenary[i]];
            var data_after = mercenaryData.EX_Level_Data.Level_Mercenary_CowBoy[mercenaryData.Level_Mercenary[i] + 1];
            int count_level = (mercenaryData.Level_Mercenary[i] / 2) + 1;

            Before_Mercenary_Information.text =
                 local_string[13].GetLocalizedString() + " : " + data_before.Max_WorkCount;*//* +
                 "\n" + local_string[14].GetLocalizedString() + " " +data_before.NextLevel_WorkCount
                 + " " + local_string[15].GetLocalizedString();*//*
            if (mercenaryData.Level_Mercenary[i] + 1 == mercenaryData.Max_Mercenary[i] + 1)
            {
                After_Mercenary_Information.text =
                        "MAX";
                Mercenary_Upgrade_Text[0].text = "MAX"; 
            }
            else
            {
                After_Mercenary_Information.text =
                    local_string[13].GetLocalizedString() + " : " + data_after.Max_WorkCount;
                Mercenary_Upgrade_Text[0].text = mercenaryData.EX_Level_Data.Level_Mercenary_CowBoy[mercenaryData.Level_Mercenary[i]].Upgrade_Cost + "G";
                if (Material_Max_Count >= mercenaryData.EX_Level_Data.Level_Mercenary_CowBoy[mercenaryData.Level_Mercenary[i]].Material)
                {
                    Mercenary_Upgrade_Text[1].text = Material_Max_Count.ToString();
                }
                else
                {
                    Mercenary_Upgrade_Text[1].text = "<color=red>" + Material_Max_Count + "</color>";
                }
                Mercenary_Upgrade_Text[2].text = mercenaryData.EX_Level_Data.Level_Mercenary_CowBoy[mercenaryData.Level_Mercenary[i]].Material.ToString();
            }
        }
        MercenaryUpgrade_Button.interactable = mercenaryData.Check_MaxLevel(i);
    }
    public void Mercenary_Level_Up()
    {
        int Material_Upgrade_Count = mercenaryData.Check_Material_Mercenary(Mercenary_Upgrade_Num); 
        int Material_Inventory_Count = itemData.Mercenary_Material_object.Item_Count;

        if (playerData.Player_Coin >= mercenaryData.Check_Cost_Mercenary(Mercenary_Upgrade_Num) && Material_Inventory_Count >= Material_Upgrade_Count)
        {
            playerData.Player_Buy_Coin(mercenaryData.Check_Cost_Mercenary(Mercenary_Upgrade_Num));
            itemData.Mercenary_Material_object.Item_Count_Down(Material_Upgrade_Count);
            mercenaryData.Mercenary_Level_Up(Mercenary_Upgrade_Num);
            Mercenary_Upgrade_Content.GetChild(Mercenary_Upgrade_ToggleNum).GetComponent<TrainingRoom_Mercenary_Upgrade_Card>().Card_LevleUP();
            Check_Player_Coin_Point();
            //inventorydirector.Check_ItemList(false, itemData.Mercenary_Material_object);
            Mercenary_Upgrade_Information_Text(Mercenary_Upgrade_Num);
        }
        else
        {
            Ban_Player_Coin_Point(true);
        }
    }

    // 용병 배치
    public void Director_Init_MercenaryPosition()
    {
        MaxMercenary = trainData.Max_Train_MaxMercenary + 1;
        Mercenary_Position_Max_Text();
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
        ScrollRect_Mercenary_Position.normalizedPosition = Vector2.up;
        Mercenary_Check_Button();// 카드 비활성화 되어 있는 경우 카운트를 세지 않음. 그렇기에 버튼 체크해야한다.
    }

    public void ResizedContent_V(Transform ScrollContent, ScrollRect Scrollrect)
    {
        GridLayoutGroup Grid = ScrollContent.GetComponent<GridLayoutGroup>();
        Vector2 cellSize = Grid.cellSize;
        Vector2 spacing = Grid.spacing;

        float hight = (cellSize.y + spacing.y) * mercenaryData.SA_MercenaryData.Mercenary_Buy_Num.Count;
        RectTransform ContentSize = ScrollContent.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(ContentSize.sizeDelta.x, hight);
        Scrollrect.normalizedPosition = Vector2.up;
    }

    public void Check_Init_MercenaryPositionCard()
    {
        RectTransform ContentSize = Mercenary_Position_Content.GetComponent<RectTransform>();
        ContentSize.sizeDelta = new Vector2(272 + (272 * (Mercenary_Buy_NumList.Count - 1)), ContentSize.sizeDelta.y);
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
            if(num == 5)
            {
                DropDown_Bard_Type = SubCard.dropDown.GetComponent<TMP_Dropdown>();
                DropDown_Bard_Type.onValueChanged.AddListener(Mercenary_Position_Bard_DropDown);
            }
        }
        Mercenary_Check_Button();
    }

    public void Mercenary_PositionUP(TrainingRoom_Mercenary_Position_Card Card, int i)
    {
        mercenaryData.SA_MercenaryData.SA_Mercenary_Num_Plus(i);
        Mercenary_TotalNum = mercenaryData.SA_MercenaryData.Mercenary_Num.Count;
        Card.Plus_Count();
        Mercenary_Check_Button();
        Mercenary_Position_List_Plus_Card(i);
        Mercenary_Position_Max_Text();
    }

    public void Mercenary_PositionDown(TrainingRoom_Mercenary_Position_Card Card, int i)
    {
        mercenaryData.SA_MercenaryData.SA_Mercenary_Num_Remove(i);
        Mercenary_TotalNum = mercenaryData.SA_MercenaryData.Mercenary_Num.Count;
        Card.Minus_Count();
        Mercenary_Check_Button();
        Mercenary_Position_List_Minus_Card(i);
        Mercenary_Position_Max_Text();
    }

    private void Mercenary_Position_Max_Text()
    {
        Mercenary_Position_Information.text = MaxMercenary.ToString();
    }
    private void Mercenary_Position_List_Init_Card()
    {
        for (int i = 0; i < Mercenary_Position_NumList.Count; i++)
        {
            GameObject Card = Instantiate(Mercenary_On_Board_Card, Mercenary_On_Board_Card_List);
            int num = Mercenary_Position_NumList[i];
            Card.GetComponent<Mercenary_On_Board_Card>().Mercenary_Num = num;
            Card.name = num.ToString();
        }
    }

    private void Mercenary_Position_List_Plus_Card(int num)
    {
        GameObject Card = Instantiate(Mercenary_On_Board_Card, Mercenary_On_Board_Card_List);
        Card.GetComponent<Mercenary_On_Board_Card>().Mercenary_Num = num;
        Card.name = num.ToString();
    }

    private void Mercenary_Position_List_Minus_Card(int num)
    {
        for(int i = 0; i < Mercenary_On_Board_Card_List.childCount; i++)
        {
            if(Mercenary_On_Board_Card_List.GetChild(i).name.Equals(num.ToString()))
            {
                Destroy(Mercenary_On_Board_Card_List.GetChild(i).gameObject);
                break;
            }
        }
    }

    public void Mercenary_Check_Button()
    {
        if (Mercenary_TotalNum < MaxMercenary) // 초과 하지 않을 때,
        {
            for (int i = 0; i < CardList.Count; i++)
            {
                int Count = CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().Mercenary_Num_Count;
                int Max_Count = CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().Mercenary_Max_Count;
                if (Count == 0)
                {
                    CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().PlusButton.interactable = true;
                    CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().MinusButton.interactable = false;
                }
                else if (Count != 0)
                {
                    if (Max_Count != -1)
                    {
                        if (Count == Max_Count)
                        {
                            CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().PlusButton.interactable = false;
                            CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().MinusButton.interactable = true;
                        }
                        else
                        {
                            CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().PlusButton.interactable = true;
                            CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().MinusButton.interactable = true;
                        }
                    }
                    else
                    {
                        CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().PlusButton.interactable = true;
                        CardList[i].GetComponent<TrainingRoom_Mercenary_Position_Card>().MinusButton.interactable = true;
                    }
                }
            }
        }
        else // 초과할 때
        {
            for (int i = 0; i < CardList.Count; i++)
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
    }
    public void Mercenary_Position_EngineDriver_DropDown(int value)
    {
        mercenaryData.SA_MercenaryData.SA_Change_EngineDriver_Type(value);
    }

    public void Mercenary_Position_Bard_DropDown(int value)
    {
        mercenaryData.SA_MercenaryData.SA_Change_Bard_Type(value);
    }
*/
    private void Check_Player_Coin_Point()
    {
        transform.GetComponentInParent<StationDirector>().Check_Coin();
    }

    public void Open_Warning_Window()
    {
        Tranining_BanFlag = true;
        Warning_Coin_Window.SetActive(true);
    }

    public void Close_Warning_Window()
    {
        Tranining_BanFlag = false;
        Warning_Coin_Window.SetActive(false);
    }
}