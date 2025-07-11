using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_ItemList", menuName = "Scriptable/SA_Item/List_Test", order = 9)]

public class SA_ItemList_Test : ScriptableObject
{
    [SerializeField]
    private ItemDataObject_Test emptyitem;
    public ItemDataObject_Test EmptyItem { get { return emptyitem;  } }

    [SerializeField]
    private List<ItemDataObject_Test> item;
    public List<ItemDataObject_Test> Item { get { return item; } }

    [Header("SupplyRarityType")]
    [SerializeField]
    private List<ItemDataObject_Test> common_supply_itemlist;
    public List<ItemDataObject_Test> Common_Supply_ItemList { get { return common_supply_itemlist; } }
    [SerializeField]
    private List<ItemDataObject_Test> rare_supply_itemlist;
    public List<ItemDataObject_Test> Rare_Supply_ItemList { get { return rare_supply_itemlist; } }
    [SerializeField]
    private List<ItemDataObject_Test> unique_supply_itemlist;
    public List<ItemDataObject_Test> Unique_Supply_ItemList { get { return unique_supply_itemlist; } }
    [SerializeField]
    private List<ItemDataObject_Test> epic_supply_itemlist;
    public List<ItemDataObject_Test> Epic_Supply_ItemList { get { return epic_supply_itemlist; } }
    [SerializeField]
    private List<ItemDataObject_Test> legendary_supply_itemlist;
    public List<ItemDataObject_Test> Legendary_Supply_ItemList { get { return  legendary_supply_itemlist; } }

    [Header("Dictionary")]
    [SerializeField]
    private List<Item_Dic_Def> item_dic_list;
    public List<Item_Dic_Def> Item_Dic_List { get { return item_dic_list; } }
    public void Editor_ItemList_Init()
    {
        item.Clear();

        common_supply_itemlist.Clear();
        rare_supply_itemlist.Clear();
        unique_supply_itemlist.Clear();
        epic_supply_itemlist.Clear();

        item_dic_list.Clear();
    }

    public void PlayGame_ItemList_Init()
    {
        foreach(ItemDataObject_Test _item in Item)
        {
            _item.Init();
        }
    }

    public IEnumerator PlayGame_ItemList_InitAsync(MonoBehaviour runner)
    {
        int count = 0;
        foreach (ItemDataObject_Test _item in Item)
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

    public void ItemList_EmptyObject(ItemDataObject_Test item)
    {
        emptyitem = item;
    }

    public void ItemList_InsertObject(ItemDataObject_Test newobjcet)
    {
        item.Add(newobjcet);

        if (newobjcet.Item_Type == Information_Item_Type_Test.Equipment)
        {
            Item_Dic_Def _item = new Item_Dic_Def();
            _item.item_num = newobjcet.Num;
            _item.item_dic_flag = true;
            item_dic_list.Add(_item);
        }
        else if (newobjcet.Item_Type == Information_Item_Type_Test.Immediate || newobjcet.Item_Type == Information_Item_Type_Test.Inventory)
        {
            Item_Dic_Def _item = new Item_Dic_Def();
            _item.item_num = newobjcet.Num;
            _item.item_dic_flag = false;
            item_dic_list.Add(_item);
        }

        if (newobjcet.Supply_Monster)
        {
            switch (newobjcet.Item_Rarity_Type)
            {
                case Information_Item_Rarity_Type_Test.Common:
                    common_supply_itemlist.Add(newobjcet);
                    break;
                case Information_Item_Rarity_Type_Test.Rare:
                    rare_supply_itemlist.Add(newobjcet);
                    break;
                case Information_Item_Rarity_Type_Test.Unique:
                    unique_supply_itemlist.Add(newobjcet);
                    break;
                case Information_Item_Rarity_Type_Test.Epic:
                    epic_supply_itemlist.Add(newobjcet);
                    break;
                case Information_Item_Rarity_Type_Test.Legendary:
                    Legendary_Supply_ItemList.Add(newobjcet);
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
            ES3.Save<bool>("Item_Dic_Flag_T_" + item_num, item_dic_flag);
        }

        public void Load(bool boss)
        {
            item_dic_flag = ES3.Load<bool>("Item_Dic_Flag_T_" + item_num, false);
        }
    }
}