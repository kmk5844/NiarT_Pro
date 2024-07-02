using System.IO;
using UnityEngine;

public class ItemDataObject : ScriptableObject
{
    [SerializeField]
    private int num;
    public int Num { get { return num; } }
    [SerializeField]
    private string item_name;
    public string Item_Name {  get { return item_name; } }
    [SerializeField]
    private Information_Item_Type item_type;
    public Information_Item_Type Item_Type { get { return item_type; } }
    [SerializeField]
    private string item_information;
    public string Item_Information {  get { return item_information; } }
    [SerializeField]
    private Information_Item_Box_Type box_type;
    public Information_Item_Box_Type Box_Type { get { return box_type; } }
    [SerializeField]
    private bool buy_flag;
    public bool Buy_Flag { get { return buy_flag; } }
    [SerializeField]
    private bool sell_flag;
    public bool Sell_Flag { get { return sell_flag; } }
    [SerializeField]
    private int item_buy_pride;
    public int Item_Buy_Pride { get { return item_buy_pride; } }
    [SerializeField]
    private int item_sell_pride;
    public int Item_Sell_Pride { get { return item_sell_pride; } }
    [SerializeField]
    private bool supply_monster;
    public bool Supply_Monster { get { return supply_monster; } }
    [SerializeField]
    private int item_count;
    public int Item_Count { get { return item_count; } }


    public void Auto_Item_Insert(
        int _num, string _item_name, Information_Item_Type _item_type, string _item_information,
        Information_Item_Box_Type _box_type, bool _buy_flag, bool _sell_flag, int _buy_pride,
        int _sell_pride, bool _supply_monster, int _item_count
        )
    {
        num = _num;
        item_name = _item_name;
        item_type = _item_type;
        item_information = _item_information;
        box_type = _box_type;
        buy_flag = _buy_flag;
        sell_flag = _sell_flag;
        item_buy_pride = _buy_pride;
        item_sell_pride = _sell_pride;
        supply_monster = _supply_monster;
        item_count = _item_count;
    }

    public void Item_Count_Down() // 사용하거나, 팔 때
    {
        item_count--;
    }

    public void Item_Count_UP() // 사거나 보급 아이템에서 먹었을 때
    {
        item_count++;
    }
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