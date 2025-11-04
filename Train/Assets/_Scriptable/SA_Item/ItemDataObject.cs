using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.Localization.SmartFormat.GlobalVariables;

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
    private Information_Item_Rarity_Type item_rarity_type;
    public Information_Item_Rarity_Type Item_Rarity_Type { get { return item_rarity_type; } }

    [SerializeField]
    private bool buy_flag;
    public bool Buy_Flag { get { return buy_flag; } }
    [SerializeField]
    private bool sell_flag;
    public bool Sell_Flag { get { return sell_flag; } }
    [SerializeField]

    private bool supply_monster;
    public bool Supply_Monster { get { return supply_monster; } }

    [SerializeField]
    private int item_buy_pride;
    public int Item_Buy_Pride { get { return item_buy_pride; } }
    [SerializeField]
    private int item_sell_pride;
    public int Item_Sell_Pride { get { return item_sell_pride; } }

    [SerializeField]
    private int max_equip;
    public int Max_Equip { get {  return max_equip; } }

    [SerializeField]
    private float cool_time;
    public float Cool_Time { get { return cool_time; } }

    [SerializeField]
    private Sprite item_sprite;
    public Sprite Item_Sprite { get { return item_sprite; } }

    [SerializeField]
    private int item_count;
    public int Item_Count { get { return item_count; } }

    [SerializeField]
    private bool infinite_flag;
    public bool Infinite_Flag { get { return infinite_flag; } }

    public void Auto_Item_Insert(
        int _num, string _item_name, Information_Item_Type _item_type, string _item_information,
        Information_Item_Rarity_Type _rarrity_type, string buy_sell_supply_flag,
        string buy_sell_pride, int _max_equip, float _cool_time ,int _item_count, bool _infiniteflag
        )
    {
        num = _num;
        item_name = _item_name;
        item_type = _item_type;
        item_information = _item_information;
        item_rarity_type = _rarrity_type;

        string[] flag = buy_sell_supply_flag.Split(',');
        string[] pride = buy_sell_pride.Split(",");

        if (flag[0].Equals("true"))
        {
            buy_flag = true;
        }
        else
        {
            buy_flag = false;
        }

        if (flag[1].Equals("true"))
        {
            sell_flag = true;
        }
        else
        {
            sell_flag = false;
        }

        if (flag[2].Equals("true"))
        {
            supply_monster = true;
        }
        else
        {
            supply_monster = false;
        }

        item_buy_pride = int.Parse(pride[0]);
        item_sell_pride = int.Parse(pride[1]);

        max_equip = _max_equip;
        cool_time = _cool_time;
        item_sprite = Resources.Load<Sprite>("ItemIcon/" + num);
        item_count = _item_count;
        infinite_flag = _infiniteflag;
    }

    public void Item_Count_Down() // 사용하거나, 팔 때
    {
        item_count -= 1;
        Save();
    }

    public void Item_Count_UP() // 사거나 보급 아이템에서 먹었을 때
    {
        item_count += 1;
        Save();
    }

    public void Item_Count_UP(int num)
    {
        item_count += num;
        Save();
    }

    public void Item_Count_Down(int num)
    {
        item_count -= num;
        Save();
    }

    public void Save()
    {
        string save_itemName = name + "_ItemCount";
        ES3.Save(save_itemName, item_count);
    }

    public void Load()
    {
        string save_itemName = name + "_ItemCount";
        item_count = ES3.Load(save_itemName, Item_Count);
    }

    public void Init()
    {
        item_count = 0;
        string save_itemName = name + "_ItemCount";
        ES3.Save(save_itemName, item_count);
    }

    public IEnumerator InitSync()
    {
        item_count = 0;
        string save_itemName = name + "_ItemCount";
        ES3.Save(save_itemName, item_count);
        yield return new WaitForSeconds(0.001f);
    }
}

public enum Information_Item_Type { 
    Equipment,
    Immediate,
    Weapon,
    Inventory,
    Quset,
    Empty
}

public enum Information_Item_Rarity_Type
{
    Common,
    Rare,
    Unique,
    Epic,
    Legendary,
    Empty,
    
    Error
}