using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_ItemList", menuName = "Scriptable/SA_Item/List", order = 9)]

public class SA_ItemList : ScriptableObject
{
    [SerializeField]
    private ItemDataObject emptyitem;
    public ItemDataObject EmptyItem { get { return emptyitem;  } }

    [SerializeField]
    private List<ItemDataObject> item;
    public List<ItemDataObject> Item { get { return item; } }

    [Header("StoreRarityType")]
    [SerializeField]
    private List<ItemDataObject> common_store_itemlist;
    public List<ItemDataObject> Common_Store_ItemList { get { return common_store_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> rare_store_itemlist;
    public List<ItemDataObject> Rare_Store_ItemList { get { return rare_store_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> unique_store_itemlist;
    public List<ItemDataObject> Unique_Store_ItemList { get { return unique_store_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> epic_store_itemlist;
    public List<ItemDataObject> Epic_Store_ItemList { get { return epic_store_itemlist; } }

    [Header("SupplyRarityType")]
    [SerializeField]
    private List<ItemDataObject> common_supply_itemlist;
    public List<ItemDataObject> Common_Supply_ItemList { get { return common_supply_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> rare_supply_itemlist;
    public List<ItemDataObject> Rare_Supply_ItemList { get { return rare_supply_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> unique_supply_itemlist;
    public List<ItemDataObject> Unique_Supply_ItemList { get { return unique_supply_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> epic_supply_itemlist;
    public List<ItemDataObject> Epic_Supply_ItemList { get { return epic_supply_itemlist; } }

    [Header("Dictionary")]
    [SerializeField]
    private List<Item_Dic_Def> item_dic_list;

    public List<Item_Dic_Def> Item_Dic_List { get { return item_dic_list; } }
    public void Editor_ItemList_Init()
    {
        item.Clear();

        common_store_itemlist.Clear();
        rare_store_itemlist.Clear();
        unique_store_itemlist.Clear();
        epic_store_itemlist.Clear();

        common_supply_itemlist.Clear();
        rare_supply_itemlist.Clear();
        unique_supply_itemlist.Clear();
        epic_supply_itemlist.Clear();

        item_dic_list.Clear();
    }

    public void PlayGame_ItemList_Init()
    {
        foreach(ItemDataObject _item in Item)
        {
            _item.Init();
        }
    }

    public IEnumerator PlayGame_ItemList_InitAsync(MonoBehaviour runner)
    {
        int count = 0;
        foreach (ItemDataObject _item in Item)
        {
            yield return runner.StartCoroutine(_item.InitSync());
            count++;

            if(count % 5 == 0)
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield return null;
    }

    public void ItemList_EmptyObject(ItemDataObject item)
    {
        emptyitem = item;
    }

    public void ItemList_InsertObject(ItemDataObject newobjcet)
    {
        item.Add(newobjcet);
        if(newobjcet.Num != 0 && newobjcet.Num  != 12 && newobjcet.Num != 37)
        {
            if (newobjcet.Item_Type == Information_Item_Type.Equipment)
            {
                Item_Dic_Def _item = new Item_Dic_Def();
                _item.item_num = newobjcet.Num;
                _item.item_dic_flag = true;
                item_dic_list.Add(_item);
            }
            else if (newobjcet.Item_Type == Information_Item_Type.Immediate || newobjcet.Item_Type == Information_Item_Type.Inventory)
            {
                Item_Dic_Def _item = new Item_Dic_Def();
                _item.item_num = newobjcet.Num;
                _item.item_dic_flag = false;
                item_dic_list.Add(_item);
            }
        }

        switch (newobjcet.Item_Rarity_Type)
        {
            case Information_Item_Rarity_Type.Common:
                if (newobjcet.Box_Type != Information_Item_Box_Type.None)
                {
                    common_store_itemlist.Add(newobjcet);
                }
                break;
            case Information_Item_Rarity_Type.Rare:
                if (newobjcet.Box_Type != Information_Item_Box_Type.None)
                {
                    rare_store_itemlist.Add(newobjcet);
                }
                break;
            case Information_Item_Rarity_Type.Unique:
                if (newobjcet.Box_Type != Information_Item_Box_Type.None)
                {
                    unique_store_itemlist.Add(newobjcet);
                }
                break;
            case Information_Item_Rarity_Type.Epic:
                if (newobjcet.Box_Type != Information_Item_Box_Type.None)
                {
                    epic_store_itemlist.Add(newobjcet);
                }
                break;
        }

        if (newobjcet.Supply_Monster)
        {
            switch (newobjcet.Item_Rarity_Type)
            {
                case Information_Item_Rarity_Type.Common:
                    common_supply_itemlist.Add(newobjcet);
                    break;
                case Information_Item_Rarity_Type.Rare:
                    rare_supply_itemlist.Add(newobjcet);
                    break;
                case Information_Item_Rarity_Type.Unique:
                    unique_supply_itemlist.Add(newobjcet);
                    break;
                case Information_Item_Rarity_Type.Epic:
                    epic_supply_itemlist.Add(newobjcet);
                    break;
            }
        }
    }

    [Serializable]
    public struct Item_Dic_Def
    {
        [SerializeField]
        public int item_num;

        public bool item_dic_flag;

        public void ChangeMonster()
        {
            if (!item_dic_flag)
            {
                item_dic_flag = true;
                Save();
            }
        }

        public void Save()
        {
            ES3.Save<bool>("Item_Dic_Flag_" + item_num, item_dic_flag);
        }

        public void Load(bool boss)
        {
            item_dic_flag = ES3.Load<bool>("Item_Dic_Flag_" + item_num, false);
        }
    }
}