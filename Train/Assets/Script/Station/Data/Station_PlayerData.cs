using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Station_PlayerData : MonoBehaviour
{
    [Header("플레이어 데이터 모아놓은 스크립터블")]
    //public Train_DataTable EX_Game_Data;
    public Level_DataTable EX_Level_Data;
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
        Player_Coin = SA_PlayerData.Coin;
        Player_Point = SA_PlayerData.Point;
    }

    private void Check_Level_Player()
    {
        Level_Player_Atk = SA_PlayerData.Level_Player_Atk;
        Level_Player_AtkDelay = SA_PlayerData.Level_Player_AtkDelay;
        Level_Player_HP = SA_PlayerData.Level_Player_HP;
        Level_Player_Armor = SA_PlayerData.Level_Player_Armor;
        Level_Player_Speed = SA_PlayerData.Level_Player_Speed;

        Max_Player_Atk = EX_Level_Data.Information_Level[Data_Index("Level_Player_Atk")].Max_Level;
        Max_Player_AtkDelay = EX_Level_Data.Information_Level[Data_Index("Level_Player_AtkDelay")].Max_Level;
        Max_Player_HP = EX_Level_Data.Information_Level[Data_Index("Level_Player_HP")].Max_Level;
        Max_Player_Armor = EX_Level_Data.Information_Level[Data_Index("Level_Player_Armor")].Max_Level;
        Max_Player_Speed = EX_Level_Data.Information_Level[Data_Index("Level_Player_Speed")].Max_Level;

        Cost_Player_Atk = EX_Level_Data.Information_LevelCost[Level_Player_Atk].Cost_Level_Player_Atk;
        Cost_Player_AtkDelay = EX_Level_Data.Information_LevelCost[Level_Player_AtkDelay].Cost_Level_Player_AtkDelay;
        Cost_Player_HP = EX_Level_Data.Information_LevelCost[Level_Player_HP].Cost_Level_Player_HP;
        Cost_Player_Armor = EX_Level_Data.Information_LevelCost[Level_Player_Armor].Cost_Level_Player_Armor;
        Cost_Player_Speed = EX_Level_Data.Information_LevelCost[Level_Player_Speed].Cost_Level_Player_Speed;
    }

    public void Player_Level_Up(int LevelNum)//LevelNum : 0 = Atk / 1= AtkDealy / 2 = HP / 3 = Armor / 4 = Speed
    {
        SA_PlayerData.SA_Player_Level_Up(LevelNum);
        Check_Level_Player();
    }

    public void Player_Buy_Coin(int Coin)
    {
        SA_PlayerData.SA_Buy_Coin(Coin);
        Player_Coin = SA_PlayerData.Coin;
    }

    public void Player_Use_Point(int Point)
    {
        SA_PlayerData.SA_Use_Point(Point);
        Player_Point = SA_PlayerData.Point;
    }

    public void Player_Get_Coin(int Coin)
    {
        SA_PlayerData.SA_Get_Coin(Coin);
        Player_Coin = SA_PlayerData.Coin;
    }

    public void Player_Get_Point(int Point)
    {
        SA_PlayerData.SA_Get_Point(Point);
        Player_Point = SA_PlayerData.Point;
    }

    public int Check_Cost_Player(int num)
    {
        switch (num)
        {
            case 0:
                return Cost_Player_Atk;
            case 1:
                return Cost_Player_AtkDelay;
            case 2:
                return Cost_Player_HP;
            case 3:
                return Cost_Player_Armor;
            case 4:
                return Cost_Player_Speed;
        }
        return -1;
    }

    public int Data_Index(string str)
    {
        int index = EX_Level_Data.Information_Level.FindIndex(x => x.Level_Name.Equals(str));
        return index;
    }
}
