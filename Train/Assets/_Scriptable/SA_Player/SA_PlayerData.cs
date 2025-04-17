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

    [SerializeField]
    public int MaxFireCount {  get { return EX_GameData.Information_Player[Player_Num].Player_MaxFireCount; } }

    [SerializeField]
    public float ReloadTime { get { return EX_GameData.Information_Player[Player_Num].Player_ReloadTime; } }

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
/*    [SerializeField]
    private int point;
    public int Point { get { return point; } }*/

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
    [SerializeField]
    private bool mission_playing;
    public bool Mission_Playing { get {  return mission_playing; } }

    [SerializeField]
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

    [Header("이벤트")]
    [SerializeField]
    private bool eventflag;
    public bool EventFlag {  get { return eventflag; } }

    [SerializeField]
    private bool food_heal_flag;
    public bool Food_Heal_Flag { get { return food_heal_flag; } }

    [SerializeField]
    private int food_num;
    public int Food_Num { get { return food_num; } }

    public void SA_GameWinReward(bool lastStage, int R_Coin)
    {
        /*
         if (select_stage == 0 && new_stage == 0) // 바로 스토리 넘어가는 특수상황일 경우
        {
            new_stage = 1;
            select_stage = 1;
        }else if (select_stage == new_stage)
        {
            new_stage++;
        }*/
        if (lastStage) // lastStage가 이라면
        {
            if (select_stage == new_stage)
            {
                new_stage++;
            }
        }
        coin += R_Coin;
        //point += R_Point;
        Save();
    }

    public void SA_Test()
    {
        //new_stage++;
        coin = 999999;
        //point = 999999;
        Save();
    }

    public void SA_GameLoseCoin(float R_CoinPercent)
    {
        coin -= (int)(coin * (R_CoinPercent / 100f)); 
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
        Save_Solo("Coin");
    }

    public void SA_Get_Coin(int R_Coin)
    {
        coin += R_Coin;
        Save_Solo("Coin");
    }

    public void SA_Loss_Coin_Persent(int persent)
    {
        coin -= coin * (persent / 100);
        Save_Solo("Coin");
    }

    public void SA_Loss_HP_Persent(int persent)
    {
        int curret_hp;
        try
        {
            curret_hp = ES3.Load<int>("Player_Curret_HP");
        }
        catch
        {
            curret_hp = 5000;
        }

        int hp = curret_hp - curret_hp * (persent / 100);
        ES3.Save<int>("Player_Curret_HP", hp);
    }

/*    public void SA_Use_Point(int R_Point)
    {
        point -= R_Point;
        Save();
    }

    public void SA_Get_Point(int R_Point)
    {
        point += R_Point;
        Save();
    }*/

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
        if(new_stage == 99)//페요테 해제
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
        mission_playing = true;
        Save_Solo("MissionNum");
        Save_Solo("MissionPlaying");
    }

    public void SA_MissionPlaying(bool flag)
    {
        mission_playing = flag;
        Save();
    }

    public void SA_ChoiceFood(int num)
    {
        eventflag = true;
        food_num = num;
        Save();
    }

    public void SA_HealFood()
    {
        food_heal_flag = true;
        Save();
    }

    public void SA_EventFlag_Off()
    {
        eventflag = false;
        food_heal_flag = false;
        Save();
    }

    private void Save()
    {
        ES3.Save<bool>("SA_PlayerData_Data_FirstFlag", firstflag);
        ES3.Save<int>("SA_PlayerData_Data_level_atk", level_atk);
        ES3.Save<int>("SA_PlayerData_Data_level_atkdelay", level_atkdelay);
        ES3.Save<int>("SA_PlayerData_Data_level_player_hp", level_hp);
        ES3.Save<int>("SA_PlayerData_Data_level_player_armor", level_armor);
        ES3.Save<int>("SA_PlayerData_Data_level_speed", level_speed);
        ES3.Save<int>("SA_PlayerData_Data_coin", coin);
        //ES3.Save<int>("SA_PlayerData_Data_point", point);
        ES3.Save<int>("SA_PlayerData_Data_new_stage", new_stage);
        ES3.Save<int>("SA_PlayerData_Data_mission_num", mission_num);
        ES3.Save<bool[]>("SA_PlayerData_Data_LockOff", character_lockoff);
        ES3.Save<int>("SA_PlayerData_Data_Story_Num", story_num);
        ES3.Save<bool>("SA_PlayerData_Data_Station_Tutorial", station_tutorial);
        ES3.Save<bool>("SA_PlayerData_Data_MissionPlaying", mission_playing);
        ES3.Save<bool>("SA_PlayerData_Data_EventFlag", eventflag);
        ES3.Save<bool>("SA_PlayerData_Data_Food_Heal_Flag", food_heal_flag);
        ES3.Save<int>("SA_PlayerData_Data_Food_Num", food_num);
    }

    private void Save_Solo(string str)
    {
        switch (str)
        {
            case "FirstFlag":
                ES3.Save<bool>("SA_PlayerData_Data_FirstFlag", firstflag);
                break;
            case "Level_Atk":
                ES3.Save<int>("SA_PlayerData_Data_level_atk", level_atk);
                break;
            case "Level_AtkDelay":
                ES3.Save<int>("SA_PlayerData_Data_level_atkdelay", level_atkdelay);
                break;
            case "Level_PlayerHP":
                ES3.Save<int>("SA_PlayerData_Data_level_player_hp", level_hp);
                break;
            case "Level_PlayerArmor":
                ES3.Save<int>("SA_PlayerData_Data_level_player_armor", level_armor);
                break;
            case "Level_PlayerSpeed":
                ES3.Save<int>("SA_PlayerData_Data_level_speed", level_speed);
                break;
            case "Coin":
                ES3.Save<int>("SA_PlayerData_Data_coin", coin);
                break;
            //ES3.Save<int>("SA_PlayerData_Data_point", point);
            case "NewStage":
                ES3.Save<int>("SA_PlayerData_Data_new_stage", new_stage);
                break;
            case "MissionNum":
                ES3.Save<int>("SA_PlayerData_Data_mission_num", mission_num);
                break;
            case "LockOff":
                ES3.Save<bool[]>("SA_PlayerData_Data_LockOff", character_lockoff);
                break;
            case "StoryNum":
                ES3.Save<int>("SA_PlayerData_Data_Story_Num", story_num);
                break;
            case "StationTutorial":
                ES3.Save<bool>("SA_PlayerData_Data_Station_Tutorial", station_tutorial);
                break;
            case "MissionPlaying":
                ES3.Save<bool>("SA_PlayerData_Data_MissionPlaying", mission_playing);
                break;
            case "EventFlag":
                ES3.Save<bool>("SA_PlayerData_Data_EventFlag", eventflag);
                break;
            case "Food_Heal_Flag":
                ES3.Save<bool>("SA_PlayerData_Data_Food_Heal_Flag", food_heal_flag);
                break;
            case "Food_Num":
                ES3.Save<int>("SA_PlayerData_Data_Food_Num", food_num);
                break;
            default:
                Debug.Log("없음 - 오타 수정 요함");
                break;
        }
    }

    private IEnumerator SaveSync()
    {
        ES3.Save<bool>("SA_PlayerData_Data_FirstFlag", firstflag);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_PlayerData_Data_level_atk", level_atk);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_PlayerData_Data_level_atkdelay", level_atkdelay);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_PlayerData_Data_level_player_hp", level_hp);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_PlayerData_Data_level_player_armor", level_armor);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_PlayerData_Data_level_speed", level_speed);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_PlayerData_Data_coin", coin);
        yield return new WaitForSeconds(0.001f);
        //ES3.Save<int>("SA_PlayerData_Data_point", point);
        ES3.Save<int>("SA_PlayerData_Data_new_stage", new_stage);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_PlayerData_Data_mission_num", mission_num);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<bool[]>("SA_PlayerData_Data_LockOff", character_lockoff);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_PlayerData_Data_Story_Num", story_num);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<bool>("SA_PlayerData_Data_Station_Tutorial", station_tutorial);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<bool>("SA_PlayerData_Data_MissionPlaying", mission_playing);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<bool>("SA_PlayerData_Data_EventFlag", eventflag);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<bool>("SA_PlayerData_Data_Food_Heal_Flag", food_heal_flag);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_PlayerData_Data_Food_Num", food_num);
        yield return new WaitForSeconds(0.001f);
    }

    public void Load()
    { 
        firstflag = ES3.Load<bool>("SA_PlayerData_Data_FirstFlag");
        level_atk = ES3.Load<int>("SA_PlayerData_Data_level_atk");
        level_atkdelay = ES3.Load<int>("SA_PlayerData_Data_level_atkdelay");
        level_hp = ES3.Load<int>("SA_PlayerData_Data_level_player_hp");
        level_armor = ES3.Load<int>("SA_PlayerData_Data_level_player_armor");
        level_speed = ES3.Load<int>("SA_PlayerData_Data_level_speed");
        coin = ES3.Load<int>("SA_PlayerData_Data_coin");
        //point = ES3.Load<int>("SA_PlayerData_Data_point");
        new_stage = ES3.Load<int>("SA_PlayerData_Data_new_stage");
        select_stage = new_stage;
        mission_num = ES3.Load<int>("SA_PlayerData_Data_mission_num");
        character_lockoff = ES3.Load<bool[]>("SA_PlayerData_Data_LockOff");
        story_num = ES3.Load<int>("SA_PlayerData_Data_Story_Num");
        station_tutorial = ES3.Load<bool>("SA_PlayerData_Data_Station_Tutorial");
        mission_playing = ES3.Load<bool>("SA_PlayerData_Data_MissionPlaying");
        eventflag = ES3.Load<bool>("SA_PlayerData_Data_EventFlag");
        food_heal_flag = ES3.Load<bool>("SA_PlayerData_Data_Food_Heal_Flag");
        food_num = ES3.Load<int>("SA_PlayerData_Data_Food_Num");
    }


    public IEnumerator LoadSync()
    {
        firstflag = ES3.Load<bool>("SA_PlayerData_Data_FirstFlag");
        yield return new WaitForSeconds(0.001f);
        level_atk = ES3.Load<int>("SA_PlayerData_Data_level_atk");
        yield return new WaitForSeconds(0.001f);
        level_atkdelay = ES3.Load<int>("SA_PlayerData_Data_level_atkdelay");
        yield return new WaitForSeconds(0.001f);
        level_hp = ES3.Load<int>("SA_PlayerData_Data_level_player_hp");
        yield return new WaitForSeconds(0.001f);
        level_armor = ES3.Load<int>("SA_PlayerData_Data_level_player_armor");
        yield return new WaitForSeconds(0.001f);
        level_speed = ES3.Load<int>("SA_PlayerData_Data_levle_speed");
        coin = ES3.Load<int>("SA_PlayerData_Data_coin");
        yield return new WaitForSeconds(0.001f);
        //point = ES3.Load<int>("SA_PlayerData_Data_point");
        yield return new WaitForSeconds(0.001f);
        new_stage = ES3.Load<int>("SA_PlayerData_Data_new_stage");
        yield return new WaitForSeconds(0.001f);
        select_stage = new_stage;
        yield return new WaitForSeconds(0.001f);
        mission_num = ES3.Load<int>("SA_PlayerData_Data_mission_num");
        yield return new WaitForSeconds(0.001f);
        character_lockoff = ES3.Load<bool[]>("SA_PlayerData_Data_LockOff");
        yield return new WaitForSeconds(0.001f);
        story_num = ES3.Load<int>("SA_PlayerData_Data_Story_Num");
        yield return new WaitForSeconds(0.001f);
        station_tutorial = ES3.Load<bool>("SA_PlayerData_Data_Station_Tutorial");
        yield return new WaitForSeconds(0.001f);
        mission_playing = ES3.Load<bool>("SA_PlayerData_Data_MissionPlaying");
        yield return new WaitForSeconds(0.001f);
        eventflag = ES3.Load<bool>("SA_PlayerData_Data_EventFlag");
        yield return new WaitForSeconds(0.001f);
        food_heal_flag = ES3.Load<bool>("SA_PlayerData_Data_Food_Heal_Flag");
        yield return new WaitForSeconds(0.001f);
        food_num = ES3.Load<int>("SA_PlayerData_Data_Food_Num");
        yield return new WaitForSeconds(0.001f);
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
        //point = 0;
        new_stage = 0;

        select_stage = 0;
        story_num = 0;
        Character_LockOff[0] = true;
        station_tutorial = false;
        for (int i = 1; i < 5; i++)
        {
            Character_LockOff[i] = false;
        }
        mission_playing = false;
        eventflag = false;
        food_heal_flag = false;
        food_num = 0;
        Save();
    }

    public IEnumerator InitAsync(MonoBehaviour runner)
    {
        player_num = 0;

        firstflag = false;

        level_atk = 0;
        level_atkdelay = 0;
        level_hp = 0;
        level_armor = 0;
        level_speed = 0;
        coin = 0;
        //point = 0;
        new_stage = 0;

        select_stage = 0;
        story_num = 0;
        Character_LockOff[0] = true;
        station_tutorial = false;
        for (int i = 1; i < 5; i++)
        {
            Character_LockOff[i] = false;
        }
        mission_playing = false;
        eventflag = false;
        food_heal_flag = false;
        food_num = 0;
        runner.StartCoroutine(SaveSync());
        yield return new WaitForSeconds(0.01f);
    }

    public void SA_Player_Level_Up(int LevelNum)//LevelNum : 0 = Atk / 1= AtkDealy / 2 = Armor / 3 = Speed / 4 = hp
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
                level_armor++;
                break;
            case (3):
                level_speed++;
                break;
            case (4):
                level_hp++;
                break;
        }
    }
}