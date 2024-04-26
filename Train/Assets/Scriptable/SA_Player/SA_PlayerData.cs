using System.Collections;
using System.Collections.Generic;
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
    //public Sprite Gun { get { return gun; } }
    [SerializeField]
    public GameObject Bullet { get { return Resources.Load<GameObject>(EX_GameData.Information_Player[Player_Num].Player_Bullet); } }

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
    private int stage;
    public int Stage { get { return stage; } }

    public void SA_GameWinReward(int R_Coin, int R_Point)
    {
        coin += R_Coin;
        point += R_Point;
    }

    public void SA_GameLoseReward(int R_Coin)
    {
        coin += R_Coin;
    }

    public void SA_Click_Playable(int i)
    {
        player_num = i;
    }

    public void SA_Buy_Coin(int R_Coin)
    {
        coin -= R_Coin;
    }

    public void SA_Use_Point(int R_Point)
    {
        point -= R_Point;
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