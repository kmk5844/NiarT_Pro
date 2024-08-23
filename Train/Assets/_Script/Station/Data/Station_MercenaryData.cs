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
    public int[] Level_Mercenary = new int[7];
    public int[] Cost_Mercenary = new int[7];
    public int[] Max_Mercenary = new int[7];

    [Header("기존 상점 리스트")]
    public List<int> Mercenary_Store_Num;

    private void Awake()
    {
        Check_Level_Mercenary();
        Check_Store_List();
    }

    private void Check_Level_Mercenary()
    {
        Level_Mercenary[0] = SA_MercenaryData.Level_Engine_Driver;
        Level_Mercenary[1] = SA_MercenaryData.Level_Engineer;
        Level_Mercenary[2] = SA_MercenaryData.Level_Long_Ranged;
        Level_Mercenary[3] = SA_MercenaryData.Level_Short_Ranged;
        Level_Mercenary[4] = SA_MercenaryData.Level_Medic;
        Level_Mercenary[5] = SA_MercenaryData.Level_Bard;
        Level_Mercenary[6] = SA_MercenaryData.Level_CowBoy;

        Max_Mercenary[0] = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Engine_Driver", 0)].Max_Level;
        Max_Mercenary[1] = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Engineer", 0)].Max_Level;
        Max_Mercenary[2] = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Long_Ranged", 0)].Max_Level;
        Max_Mercenary[3] = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Short_Ranged", 0)].Max_Level;
        Max_Mercenary[4] = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Medic", 0)].Max_Level;
        Max_Mercenary[5] = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_Bard", 0)].Max_Level;
        Max_Mercenary[6] = EX_Level_Data.Information_Level[Data_Info("Level_Mercenary_CowBoy", 0)].Max_Level;

        Cost_Mercenary[0] = EX_Game_Data.Information_Mercenary[Data_Info("Engine_Driver", 1)].Mercenary_Pride; // 상점 구매용
        Cost_Mercenary[1] = EX_Game_Data.Information_Mercenary[Data_Info("Engineer", 1)].Mercenary_Pride;
        Cost_Mercenary[2] = EX_Game_Data.Information_Mercenary[Data_Info("Long_Ranged", 1)].Mercenary_Pride;
        Cost_Mercenary[3] = EX_Game_Data.Information_Mercenary[Data_Info("Short_Ranged", 1)].Mercenary_Pride;
        Cost_Mercenary[4] = EX_Game_Data.Information_Mercenary[Data_Info("Medic", 1)].Mercenary_Pride;
        Cost_Mercenary[5] = EX_Game_Data.Information_Mercenary[Data_Info("Bard", 1)].Mercenary_Pride;
        Cost_Mercenary[6] = EX_Game_Data.Information_Mercenary[Data_Info("CowBoy", 1)].Mercenary_Pride;
    }

    public void Check_Store_List()
    {
        foreach (Info_Mercenary Mercenary in EX_Game_Data.Information_Mercenary)
        {
            if (Mercenary.Store)
            {
                Mercenary_Store_Num.Add(Mercenary.Number);
            }
        }
    }

    public int Data_Info(string M_type, int i)
    {
        int index;
        if (i == 0) // 이름
        {
            index = EX_Level_Data.Information_Level.FindIndex(X => X.Level_Name.Equals(M_type));
            return index;
        }
        else if(i == 1) // Type
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

    public bool Check_MaxLevel(int mercenaryNum)
    {
        return Level_Mercenary[mercenaryNum] != Max_Mercenary[mercenaryNum] ? true : false;
    }


    public int Mercenary_Find_Level(int mercenaryNum)
    {
        return Level_Mercenary[mercenaryNum];
    }

    public int Check_Cost_Mercenary(int Num)
    {
        switch (Num)
        {
            case 0:
                return EX_Level_Data.Level_Mercenary_Engine_Driver[Level_Mercenary[Num]].Upgrade_Cost;
            case 1:
                return EX_Level_Data.Level_Mercenary_Engineer[Level_Mercenary[Num]].Upgrade_Cost;
            case 2:
                return EX_Level_Data.Level_Mercenary_Long_Ranged[Level_Mercenary[Num]].Upgrade_Cost;
            case 3:
                return EX_Level_Data.Level_Mercenary_Short_Ranged[Level_Mercenary[Num]].Upgrade_Cost;
            case 4:
                return EX_Level_Data.Level_Mercenary_Medic[Level_Mercenary[Num]].Upgrade_Cost;
            case 5:
                return EX_Level_Data.Level_Mercenary_Bard[Level_Mercenary[Num]].Upgrade_Cost;
            case 6:
                return EX_Level_Data.Level_Mercenary_CowBoy[Level_Mercenary[Num]].Upgrade_Cost;
        }
        return -1;
    }

    public int Check_Material_Mercenary(int Num)
    {
        switch (Num)
        {
            case 0:
                return EX_Level_Data.Level_Mercenary_Engine_Driver[Level_Mercenary[Num]].Material;
            case 1:
                return EX_Level_Data.Level_Mercenary_Engineer[Level_Mercenary[Num]].Material;
            case 2:
                return EX_Level_Data.Level_Mercenary_Long_Ranged[Level_Mercenary[Num]].Material;
            case 3:
                return EX_Level_Data.Level_Mercenary_Short_Ranged[Level_Mercenary[Num]].Material;
            case 4:
                return EX_Level_Data.Level_Mercenary_Medic[Level_Mercenary[Num]].Material;
            case 5:
                return EX_Level_Data.Level_Mercenary_Bard[Level_Mercenary[Num]].Material;
            case 6:
                return EX_Level_Data.Level_Mercenary_CowBoy[Level_Mercenary[Num]].Material;
        }
        return -1;
    }
}
