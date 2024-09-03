using ES3Types;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_PlayerData", menuName = "Scriptable/PlayerData", order = 1)]

public class SA_PlayerData : ScriptableObject
{
    [SerializeField]
    Game_DataTable EX_GameData;
    [SerializeField]
    private int player_num;
    public int Player_Num { get { return player_num; } }

    [SerializeField]
    public int Atk { get { return EX_GameData.Information_Player[Player_Num].Player_Atk; } }
    [SerializeField]
    public int Armor { get { return EX_GameData.Information_Player[Player_Num].Player_Armor; } }
    [SerializeField]
    public float Delay { get { return EX_GameData.Information_Player[Player_Num].Player_Delay; } }
    [SerializeField]
    public int HP { get { return EX_GameData.Information_Player[Player_Num].Player_HP; } }
    [SerializeField]
    public float MoveSpeed { get { return EX_GameData.Information_Player[Player_Num].Player_MoveSpeed; } }
    [SerializeField]
    public GameObject Bullet { get { return Resources.Load<GameObject>(EX_GameData.Information_Player[Player_Num].Player_Bullet); } }

    [SerializeField]
    public string Skill_CoolTime {  get { return EX_GameData.Information_Player[Player_Num].Player_Skill_CoolTime; } }

    [SerializeField]
    public string Skill_During { get { return EX_GameData.Information_Player[Player_Num].Player_Skill_During; } }

    [Header("레벨")]
    [SerializeField]
    private int level_atk;
    public int Level_Player_Atk { get {  return level_atk; } }
    [SerializeField]
    private int level_atkdelay;
    public int Level_Player_AtkDelay { get { return level_atkdelay; } }
    [SerializeField]
    private int level_hp;
    public int Level_Player_HP { get { return level_hp; } }
    [SerializeField]
    private int level_armor;
    public int Level_Player_Armor { get { return level_armor; } }
    [SerializeField]
    private int level_speed;
    public int Level_Player_Speed { get { return level_speed; } }
    [Header("재화")]
    [SerializeField]
    private int coin;
    public int Coin { get { return coin; } }
    [SerializeField]
    private int point;
    public int Point { get { return point; } }

    [Header("스테이지")]
    [SerializeField]
    private int new_stage;
    public int New_Stage { get { return new_stage; } }
    [SerializeField]
    private int select_stage;
    public int Select_Stage { get { return  select_stage; } }

    [SerializeField]
    private bool[] character_lockoff;
    public bool[] Character_LockOff {  get {  return character_lockoff; } }

    public void SA_GameWinReward(int R_Coin, int R_Point)
    {
        if (select_stage == 0 && new_stage == 0) // 바로 스토리 넘어가는 특수상황일 경우
        {
            select_stage = 1;
            new_stage = 1;
        }else if (select_stage == new_stage)
        {
            new_stage++;
        }
        coin += R_Coin;
        point += R_Point;
        Save();
    }

    public void SA_GameLoseReward(int R_Coin)
    {
        coin += R_Coin;
        Save();
    }

    public void SA_Click_Playable(int i)
    {
        player_num = i;
        Save();
    }

    public void SA_Buy_Coin(int R_Coin)
    {
        coin -= R_Coin;
        Save();
    }

    public void SA_Get_Coin(int R_Coin)
    {
        coin += R_Coin;
        Save();
    }

    public void SA_Use_Point(int R_Point)
    {
        point -= R_Point;
        Save();
    }

    public void SA_Get_Point(int R_Point)
    {
        point += R_Point;
        Save();
    }

    public void SA_SelectLevel(int num)
    {
        select_stage = num;
        Save();
    }

    public void SA_CharecterCheck()
    {
        if(new_stage == 5)
        {
            character_lockoff[1] = true;
        }
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetInt("SA_PlayerData_Data_level_atk", level_atk);
        PlayerPrefs.SetInt("SA_PlayerData_Data_level_atkdelay", level_atkdelay);
        PlayerPrefs.SetInt("SA_PlayerData_Data_level_player_hp", level_hp);
        PlayerPrefs.SetInt("SA_PlayerData_Data_level_player_armor", level_armor);
        PlayerPrefs.SetInt("SA_PlayerData_Data_levle_speed", level_speed);
        PlayerPrefs.SetInt("SA_PlayerData_Data_coin", coin);
        PlayerPrefs.SetInt("SA_PlayerData_Data_point", point);
        PlayerPrefs.SetInt("SA_PlayerData_Data_new_stage", new_stage);
        PlayerPrefs.SetInt("SA_PlayerData_Data_select_stage", select_stage);
        ES3.Save<bool[]>("SA_PlayerData_Data_LockOff", character_lockoff);
    }

    public void Load()
    {
        level_atk = PlayerPrefs.GetInt("SA_PlayerData_Data_level_atk");
        level_atkdelay = PlayerPrefs.GetInt("SA_PlayerData_Data_level_atkdelay");
        level_hp = PlayerPrefs.GetInt("SA_PlayerData_Data_level_player_hp");
        level_armor = PlayerPrefs.GetInt("SA_PlayerData_Data_level_player_armor");
        level_speed = PlayerPrefs.GetInt("SA_PlayerData_Data_levle_speed");
        coin = PlayerPrefs.GetInt("SA_PlayerData_Data_coin");
        point = PlayerPrefs.GetInt("SA_PlayerData_Data_point");
        new_stage = PlayerPrefs.GetInt("SA_PlayerData_Data_new_stage");
        new_stage = PlayerPrefs.GetInt("SA_PlayerData_Data_select_stage");
        character_lockoff = ES3.Load<bool[]>("SA_PlayerData_Data_LockOff");
    }

    public void Init()
    {
        player_num = 0;

        level_atk = 0;
        level_atkdelay = 0;
        level_hp = 0;
        level_armor = 0;
        level_speed = 0;
        coin = 0;
        point = 0;
        new_stage = 0;

        select_stage = 0;

        Character_LockOff[0] = true;
        for(int i = 1; i < 5; i++)
        {
            Character_LockOff[i] = false;
        }

        Save();
    }

    public void SA_Player_Level_Up(int LevelNum)//LevelNum : 0 = Atk / 1= AtkDealy / 2 = HP / 3 = Armor / 4 = Speed
    {
        switch(LevelNum)
        {
            case (0):
                level_atk++;
                break;
            case (1):
                level_atkdelay++;
                break;
            case (2):
                level_hp++;
                break;
            case (3):
                level_armor++;
                break;
            case (4):
                level_speed++;
                break;
        }
    }
}