using UnityEngine;
[CreateAssetMenu(fileName = "SA_ItemData", menuName = "Scriptable/ItemData", order = 8)]
public class SA_ItemData : ScriptableObject
{
    public int Num;
    public string Item_Name;
    public string Item_Type;
    public string Itme_Information;
    public string Box_Type;
    public bool Buy_Flag;
    public bool Sell_Flag;
    public int Item_Buy_Pride;
    public int Item_Sell_Pride;
    public bool Supply_Monster;
}