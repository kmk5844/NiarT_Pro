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
    private bool firstflag;
    public bool FirstFlag { get { return firstflag; } }

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

    [Header("미션")]
    [SerializeField]
    private int mission_num;
    public int Mission_Num { get { return mission_num; } }
    [Header("스테이지")] // Main
    [SerializeField]
    private int new_stage;
    public int New_Stage { get { return new_stage; } }
    [SerializeField]
    private int select_stage;
    public int Select_Stage { get { return  select_stage; } }

    [Header("서브 스테이지")]
    private int select_sub_stage;
    public int Select_Sub_Stage { get { return select_sub_stage; } }


    [Header("스토리")]
    [SerializeField]
    private int story_num;
    public int Story_Num { get { return story_num; } }

    [SerializeField]
    private bool[] character_lockoff;
    public bool[] Character_LockOff {  get {  return character_lockoff; } }

    [SerializeField]
    private bool station_tutorial;
    public bool Station_Tutorial {  get { return station_tutorial; } }

    

    public void SA_GameWinReward(int R_Coin, int R_Point)
    {
/*        if (select_stage == 0 && new_stage == 0) // 바로 스토리 넘어가는 특수상황일 경우
        {
            new_stage = 1;
            select_stage = 1;
        }else if (select_stage == new_stage)
        {
            new_stage++;
        }*/
        coin += R_Coin;
        point += R_Point;
        Save();
    }

    public void SA_Test()
    {
        new_stage++;
        coin = 999999;
        point = 999999;
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

    public void SA_SelectSubStage(int substagenum)
    {
        select_sub_stage = substagenum;
        Save();
    }

    public void SA_CharecterCheck()
    {
        if(new_stage == 4)
        {
            character_lockoff[1] = true;
        }
        Save();
    }

    public int SA_CheckCharecter_Num()
    {
        int i = 0;
        foreach(bool flag in character_lockoff)
        {
            if (flag)
            {
                i++;
            }
            else
            {
                break;
            }
        }
        return i;
    }

    public void SA_CheckFirstFlag()
    {
        if (!firstflag)
        {
            firstflag = true;
            Save();
        }
    }

    public void SA_StoryEnd()
    {
        story_num++;
        Save();
    }

    public void SA_StoryUnEnd()
    {
        story_num--;
        Save();
    }

    public void SA_StoryNum_Chnage(int i)
    {
        story_num = i;
        Save();
    }

    public void SA_ClickMission(int i)
    {
        mission_num = i;
        Save();
    }

    private void Save()
    {
        ES3.Save<bool>("SA_PlayerData_Data_FirstFlag", firstflag);
        ES3.Save<int>("SA_PlayerData_Data_level_atk", level_atk);
        ES3.Save<int>("SA_PlayerData_Data_level_atkdelay", level_atkdelay);
        ES3.Save<int>("SA_PlayerData_Data_level_player_hp", level_hp);
        ES3.Save<int>("SA_PlayerData_Data_level_player_armor", level_armor);
        ES3.Save<int>("SA_PlayerData_Data_levle_speed", level_speed);
        ES3.Save<int>("SA_PlayerData_Data_coin", coin);
        ES3.Save<int>("SA_PlayerData_Data_point", point);
        ES3.Save<int>("SA_PlayerData_Data_new_stage", new_stage);
        ES3.Save<int>("SA_PlayerData_Data_mission_num", mission_num);
        ES3.Save<bool[]>("SA_PlayerData_Data_LockOff", character_lockoff);
        ES3.Save<int>("SA_PlayerData_Data_Story_Num", story_num);
        ES3.Save<bool>("SA_PlayerData_Data_Station_Tutorial", station_tutorial);
    }

    public void Load()
    { 
        firstflag = ES3.Load<bool>("SA_PlayerData_Data_FirstFlag");
      
        level_atk = ES3.Load<int>("SA_PlayerData_Data_level_atk");
        level_atkdelay = ES3.Load<int>("SA_PlayerData_Data_level_atkdelay");
        level_hp = ES3.Load<int>("SA_PlayerData_Data_level_player_hp");
        level_armor = ES3.Load<int>("SA_PlayerData_Data_level_player_armor");
        level_speed = ES3.Load<int>("SA_PlayerData_Data_levle_speed");
        coin = ES3.Load<int>("SA_PlayerData_Data_coin");
        point = ES3.Load<int>("SA_PlayerData_Data_point");
        new_stage = ES3.Load<int>("SA_PlayerData_Data_new_stage");
        select_stage = new_stage;
        mission_num = ES3.Load<int>("SA_PlayerData_Data_mission_num");
        character_lockoff = ES3.Load<bool[]>("SA_PlayerData_Data_LockOff");
        story_num = ES3.Load<int>("SA_PlayerData_Data_Story_Num");
        station_tutorial = ES3.Load<bool>("SA_PlayerData_Data_Station_Tutorial");
    }

    public void Init()
    {
        player_num = 0;

        firstflag = false;

        level_atk = 0;
        level_atkdelay = 0;
        level_hp = 0;
        level_armor = 0;
        level_speed = 0;
        coin = 0;
        point = 0;
        new_stage = 0;

        select_stage = 0;
        story_num = 0;
        Character_LockOff[0] = true;
        station_tutorial = false;
        for (int i = 1; i < 5; i++)
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