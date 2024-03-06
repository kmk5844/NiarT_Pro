using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Station_PlayerData : MonoBehaviour
{
    [Header("플레이어 데이터 모아놓은 스크립터블")]
    public Train_DataTable EX_Data;
    public SA_PlayerData SA_PlayerData;

    [Header("플레이어 데이터")]
    public int Level_Player_Atk;
    public int Level_Player_AtkDelay;
    public int Level_Player_HP;
    public int Level_Player_Armor;
    public int Level_Player_Speed;

    public int Max_Player_Atk;
    public int Max_Player_AtkDelay;
    public int Max_Player_HP;
    public int Max_Player_Armor;
    public int Max_Player_Speed;

    public int Cost_Player_Atk;
    public int Cost_Player_AtkDelay;
    public int Cost_Player_HP;
    public int Cost_Player_Armor;
    public int Cost_Player_Speed;

    public int Player_Coin;
    public int Player_Point;

    private void Awake()
    {
        Check_Level_Player();
        Max_Player_Atk = EX_Data.Information_Level[Data_Index("Level_Player_Atk")].Max_Level;
        Max_Player_AtkDelay = EX_Data.Information_Level[Data_Index("Level_Player_AtkDelay")].Max_Level;
        Max_Player_HP = EX_Data.Information_Level[Data_Index("Level_Player_HP")].Max_Level;
        Max_Player_Armor = EX_Data.Information_Level[Data_Index("Level_Player_Armor")].Max_Level;
        Max_Player_Speed = EX_Data.Information_Level[Data_Index("Level_Player_Speed")].Max_Level;
    }

    private void Check_Level_Player()
    {
        Level_Player_Atk = SA_PlayerData.Level_Player_Atk;
        Level_Player_AtkDelay = SA_PlayerData.Level_Player_AtkDelay;
        Level_Player_HP = SA_PlayerData.Level_Player_HP;
        Level_Player_Armor = SA_PlayerData.Level_Player_Armor;
        Level_Player_Speed = SA_PlayerData.Level_Player_Speed;

        Cost_Player_Atk = EX_Data.Information_LevelCost[Level_Player_Atk].Cost_Level_Player_Atk;
        Cost_Player_AtkDelay = EX_Data.Information_LevelCost[Level_Player_AtkDelay].Cost_Level_Player_AtkDelay;
        Cost_Player_HP = EX_Data.Information_LevelCost[Level_Player_HP].Cost_Level_Player_HP;
        Cost_Player_Armor = EX_Data.Information_LevelCost[Level_Player_Armor].Cost_Level_Player_Armor;
        Cost_Player_Speed = EX_Data.Information_LevelCost[Level_Player_Speed].Cost_Level_Player_Speed;
    }

    public void Player_Level_Up(int LevelNum)//LevelNum : 0 = Atk / 1= AtkDealy / 2 = HP / 3 = Armor / 4 = Speed
    {
        SA_PlayerData.SA_Player_Level_Up(LevelNum);
        Check_Level_Player();
    }

    public int Data_Index(string str)
    {
        int index = EX_Data.Information_Level.FindIndex(x => x.Level_Name.Equals(str));
        return index;
    }
}
