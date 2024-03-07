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
    public int Cost_Mercenary_Engine_Driver;
    public int Cost_Mercenary_Engineer;
    public int Cost_Mercenary_Long_Ranged;
    public int Cost_Mercenary_Short_Ranged;
    public int Cost_Mercenary_Medic;

    //여기에 MAX가 들어간다

    private void Start()
    {
        Mercenary_Num = SA_MercenaryData.Mercenary_Num;
        Cost_Mercenary_Engine_Driver = EX_Game_Data.Information_Mercenary[Data_Info("Engine_Driver")].Mercenary_Pride; // 상점 구매용
        Cost_Mercenary_Engineer = EX_Game_Data.Information_Mercenary[Data_Info("Engineer")].Mercenary_Pride;
        Cost_Mercenary_Long_Ranged = EX_Game_Data.Information_Mercenary[Data_Info("Long_Ranged")].Mercenary_Pride;
        Cost_Mercenary_Short_Ranged = EX_Game_Data.Information_Mercenary[Data_Info("Short_Ranged")].Mercenary_Pride;
        Cost_Mercenary_Medic = EX_Game_Data.Information_Mercenary[Data_Info("Medic")].Mercenary_Pride;
    }

    public int Data_Info(string M_type)
    {
        int index = EX_Game_Data.Information_Mercenary.FindIndex(x => x.Type.Equals(M_type));
        return index;
    }// 재설정 Type이 아닌 이름으로 검색해야 한다.
}
