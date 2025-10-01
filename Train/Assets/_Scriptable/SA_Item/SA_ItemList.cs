using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_ItemList", menuName = "Scriptable/SA_Item/List_Test", order = 9)]

public class SA_ItemList : ScriptableObject
{
    [SerializeField]
    private ItemDataObject emptyitem;
    public ItemDataObject EmptyItem { get { return emptyitem;  } }

    [SerializeField]
    private List<ItemDataObject> item;
    public List<ItemDataObject> Item { get { return item; } }

    [SerializeField]
    private List<ItemDataObject> equiped_item_list;
    public List<ItemDataObject> Equiped_Item_List { get { return equiped_item_list; } }

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
    [SerializeField]
    private List<ItemDataObject> epic_supply_itemlist_NonWeapon;
    public List<ItemDataObject> Epic_Supply_ItemList_NonWeapon { get { return epic_supply_itemlist_NonWeapon; } }
    [SerializeField]
    private List<ItemDataObject> legendary_supply_itemlist;
    public List<ItemDataObject> Legendary_Supply_ItemList { get { return  legendary_supply_itemlist; } }

    [Header("Dictionary")]
    [SerializeField]
    private List<Item_Dic_Def> item_dic_list;
    public List<Item_Dic_Def> Item_Dic_List { get { return item_dic_list; } }
    public void Editor_ItemList_Init()
    {
        item.Clear();

        equiped_item_list.Clear();
        common_supply_itemlist.Clear();
        rare_supply_itemlist.Clear();
        unique_supply_itemlist.Clear();
        epic_supply_itemlist.Clear();
        epic_supply_itemlist_NonWeapon.Clear();

        legendary_supply_itemlist.Clear();

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

        if (newobjcet.Item_Type == Information_Item_Type.Equipment)
        {
            equiped_item_list.Add(newobjcet);
            Item_Dic_Def _item = new Item_Dic_Def();
            _item.item_num = newobjcet.Num;
            _item.item_dic_flag = true;
            item_dic_list.Add(_item);
        }
        else if (newobjcet.Item_Type == Information_Item_Type.Immediate)
        {
            Item_Dic_Def _item = new Item_Dic_Def();
            _item.item_num = newobjcet.Num;
            _item.item_dic_flag = false;
            item_dic_list.Add(_item);
        }
        else if (newobjcet.Item_Type == Information_Item_Type.Weapon)
        {
            Item_Dic_Def _item = new Item_Dic_Def();
            _item.item_num = newobjcet.Num;
            _item.item_dic_flag = false;
            item_dic_list.Add(_item);
        }
        else if (newobjcet.Item_Type == Information_Item_Type.Inventory)
        {
            equiped_item_list.Add(newobjcet);
            Item_Dic_Def _item = new Item_Dic_Def();
            _item.item_num = newobjcet.Num;
            _item.item_dic_flag = false;
            item_dic_list.Add(_item);
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
                    if (!newobjcet.Item_Type.Equals(Information_Item_Type.Weapon))
                    {
                        epic_supply_itemlist_NonWeapon.Add(newobjcet);
                    }
                    break;
                case Information_Item_Rarity_Type.Legendary:
                    Legendary_Supply_ItemList.Add(newobjcet);
                    break;
            }
        }
    }

    public IEnumerator LoadDicSync()
    {
        foreach (Item_Dic_Def item_dic in item_dic_list)
        {
            item_dic.Load(); // Assuming boss items have numbers >= 1000
            yield return new WaitForSeconds(0.001f); // 비동기 처리를 위해 약간의 대기 시간 추가
        }
    }

    public IEnumerator InitDicSync()
    {
        foreach (Item_Dic_Def item_dic in item_dic_list)
        {
            if(item_dic.item_dic_flag)
            {
                int num = item_dic.item_num;
                if (item[num].Item_Type != Information_Item_Type.Equipment)
                {
                    item_dic.item_dic_flag = false;
                    item_dic.Save();
                }

            }
            yield return new WaitForSeconds(0.001f); // 비동기 처리를 위해 약간의 대기 시간 추가
        }
    }


    [Serializable]
    public class Item_Dic_Def
    {
        [SerializeField]
        public int item_num;

        public bool item_dic_flag;

        public void ChangeItem()
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

        public void Load()
        {
            if(item_num >=0 && item_num < 29)
            {
                item_dic_flag = ES3.Load<bool>("Item_Dic_Flag_T_" + item_num, true);
            }
            else if(item_num >= 29)
            {
                item_dic_flag = ES3.Load<bool>("Item_Dic_Flag_T_" + item_num, false);
            }
        }
    }
}