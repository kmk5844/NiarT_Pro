using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station_MercenaryData : MonoBehaviour
{
    [Header("�뺴 ������ ��Ƴ��� ��ũ���ͺ�")]
    public Train_DataTable EX_Data;
    public SA_MercenaryData SA_MercenaryData;

    [Header("�뺴 ������")]
    public List<int> Mercenary_Num;
    public int Cost_Mercenary_Engine_Driver;
    public int Cost_Mercenary_Engineer;
    public int Cost_Mercenary_Long_Ranged;
    public int Cost_Mercenary_Short_Ranged;
    public int Cost_Mercenary_Medic;

    private void Start()
    {
        Mercenary_Num = SA_MercenaryData.Mercenary_Num;
        Cost_Mercenary_Engine_Driver = EX_Data.Information_Mercenary[Data_Info("Engine_Driver")].Mercenary_Pride;
        Cost_Mercenary_Engineer = EX_Data.Information_Mercenary[Data_Info("Engineer")].Mercenary_Pride;
        Cost_Mercenary_Long_Ranged = EX_Data.Information_Mercenary[Data_Info("Long_Ranged")].Mercenary_Pride;
        Cost_Mercenary_Short_Ranged = EX_Data.Information_Mercenary[Data_Info("Short_Ranged")].Mercenary_Pride;
        Cost_Mercenary_Medic = EX_Data.Information_Mercenary[Data_Info("Medic")].Mercenary_Pride;
    }

    public int Data_Info(string M_type)
    {
        int index = EX_Data.Information_Mercenary.FindIndex(x => x.Type.Equals(M_type));
        return index;
    }// �缳�� Type�� �ƴ� �̸����� �˻��ؾ� �Ѵ�.
}
