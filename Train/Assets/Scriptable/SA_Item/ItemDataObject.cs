using UnityEngine;

public class ItemDataObject : ScriptableObject
{
    public int Num;
    public string Item_Name;
    public Information_Item_Type Item_Type;
    public string Item_Information;
    public Information_Item_Box_Type Box_Type;
    public bool Buy_Flag;
    public bool Sell_Flag;
    public int Item_Buy_Pride;
    public int Item_Sell_Pride;
    public bool Supply_Monster;
}

public enum Information_Item_Type { 
    Equipment,
    Immediate,
    Weapon,
    Material,
    Box,
    Inventory,
    Quset,
    None
}

public enum Information_Item_Box_Type { 
    Item,
    Material,
    None
}