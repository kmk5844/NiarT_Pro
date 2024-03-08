using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station_MercenaryData : MonoBehaviour
{
    [Header("용병 데이터 모아놓은 스크립터블")]
    public Game_DataTable EX_Game_Data;
    public Level_DataTable EX_Level_Data;
    public SA_MercenaryData SA_MercenaryData;

    [Header("용병 데이터")]
    public List<int> Mercenary_Num;
    public int Level_Mercenary_Engine_Driver;
    public int Level_Mercenary_Engineer;
    public int Level_Mercenary_Long_Ranged;
    public int Level_Mercenary_Short_Ranged;
    public int Level_Mercenary_Medic;

    public int Cost_Mercenary_Engine_Driver;
    public int Cost_Mercenary_Engineer;
    public int Cost_Mercenary_Long_Ranged;
    public int Cost_Mercenary_Short_Ranged;
    public int Cost_Mercenary_Medic;

    public int Max_Mercenary_Engine_Driver;
    public int Max_Mercenary_Engineer;
    public int Max_Mercenary_Long_Ranged;
    public int Max_Mercenary_Short_Ranged;
    public int Max_Mercenary_Medic;

    private void Awake()
    {
        Mercenary_Num = SA_MercenaryData.Mercenary_Num;
        Check_Level_Mercenary();
    }

    private void Check_Level_Mercenary()
    {
        Level_Mercenary_Engine_Driver = SA_MercenaryData.Level_Engine_Driver;
        Level_Mercenary_Engineer = SA_MercenaryData.Level_Engineer;
        Level_Mercenary_Long_Ranged = SA_MercenaryData.Level_Long_Ranged;
        Level_Mercenary_Short_Ranged = SA_MercenaryData.Level_Short_Ranged;
        Level_Mercenary_Medic = SA_MercenaryData.Level_Medic;

        Max_Mercenary_Engine_Driver = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Engine_Driver", 0)].Max_Level;
        Max_Mercenary_Engineer = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Engineer", 0)].Max_Level;
        Max_Mercenary_Long_Ranged = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Long_Ranged", 0)].Max_Level;
        Max_Mercenary_Short_Ranged = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Short_Ranged", 0)].Max_Level;
        Max_Mercenary_Medic = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Medic", 0)].Max_Level;

        Cost_Mercenary_Engine_Driver = EX_Game_Data.Information_Mercenary[Data_Info("Engine_Driver", 1)].Mercenary_Pride; // 상점 구매용
        Cost_Mercenary_Engineer = EX_Game_Data.Information_Mercenary[Data_Info("Engineer", 1)].Mercenary_Pride;
        Cost_Mercenary_Long_Ranged = EX_Game_Data.Information_Mercenary[Data_Info("Long_Ranged", 1)].Mercenary_Pride;
        Cost_Mercenary_Short_Ranged = EX_Game_Data.Information_Mercenary[Data_Info("Short_Ranged", 1)].Mercenary_Pride;
        Cost_Mercenary_Medic = EX_Game_Data.Information_Mercenary[Data_Info("Medic", 1)].Mercenary_Pride;
    }

    public int Data_Info(string M_type, int i)
    {
        int index;
        if (i == 0)
        {
            index = EX_Level_Data.Information_Level.FindIndex(X => X.Level_Name.Equals(M_type));
            return index;
        }
        else if(i == 1)
        {
            index = EX_Game_Data.Information_Mercenary.FindIndex(x => x.Type.Equals(M_type));
            return index;
        }
        return 0;
    }// 재설정 Type이 아닌 이름으로 검색해야 한다.

    public void Mercenary_Level_Up(int LevelNum)
    {
        SA_MercenaryData.SA_Mercenary_Level_Up(LevelNum);
        Check_Level_Mercenary();
    }

}
