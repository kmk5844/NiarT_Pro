using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_ItemList", menuName = "Scriptable/SA_Item/List", order = 9)]

public class SA_ItemList : ScriptableObject
{
    [SerializeField]
    private List<ItemDataObject> item;
    public List<ItemDataObject> Item { get { return item; } }

    [Header("BoxType_Materail")]
    [SerializeField]
    private List<ItemDataObject> random_material_itemlist;
    public List<ItemDataObject> Random_Material_ItemList { get { return random_material_itemlist; } }
    
    [Header("BoxType_Item")]
    [SerializeField]
    private List<ItemDataObject> common_item_itemlist;
    public List<ItemDataObject> Common_Item_ItemList { get { return common_item_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> rare_item_itemlist;
    public List<ItemDataObject> Rare_Item_ItemList { get { return rare_item_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> unique_item_itemlist;
    public List<ItemDataObject> Unique_Item_ItemList { get { return unique_item_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> epic_item_itemlist;
    public List<ItemDataObject> Epic_Item_ItemList { get { return epic_item_itemlist; } }

    [Header("BoxType_Materail&Item")]
    [SerializeField]
    private List<ItemDataObject> common_box_itemlist;
    public List<ItemDataObject> Common_Box_ItemList { get { return common_box_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> rare_box_itemlist;
    public List<ItemDataObject> Rare_Box_ItemList { get { return rare_box_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> unique_box_itemlist;
    public List<ItemDataObject> Unique_Box_ItemList { get { return unique_box_itemlist; } }
    [SerializeField]
    private List<ItemDataObject> epic_box_itemlist;
    public List<ItemDataObject> Epic_Box_ItemList { get { return epic_box_itemlist; } }

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
    public void ItemList_Init()
    {
        item.Clear();
        random_material_itemlist.Clear();

        common_item_itemlist.Clear();
        rare_item_itemlist.Clear();
        unique_item_itemlist.Clear();
        epic_item_itemlist.Clear();

        common_box_itemlist.Clear();
        rare_box_itemlist.Clear();
        unique_box_itemlist.Clear();
        epic_box_itemlist.Clear();

        common_supply_itemlist.Clear();
        rare_supply_itemlist.Clear();
        unique_supply_itemlist.Clear();
        epic_supply_itemlist.Clear();
    }

    public void ItemList_InsertObject(ItemDataObject newobjcet)
    {
        item.Add(newobjcet);

        if(newobjcet.Box_Type == Information_Item_Box_Type.Material)
        {
            random_material_itemlist.Add(newobjcet);
        }

        switch (newobjcet.Item_Rarity_Type)
        {
            case Information_Item_Rarity_Type.Common:
                if(newobjcet.Box_Type == Information_Item_Box_Type.Item)
                {
                    if(newobjcet.Item_Type == Information_Item_Type.Equipment)
                    {
                        common_item_itemlist.Add(newobjcet);
                    }
                }
                if(newobjcet.Box_Type != Information_Item_Box_Type.None)
                {
                    common_box_itemlist.Add(newobjcet);
                }
                break;
            case Information_Item_Rarity_Type.Rare:
                if (newobjcet.Box_Type == Information_Item_Box_Type.Item)
                {
                    if (newobjcet.Item_Type == Information_Item_Type.Equipment)
                    {
                        rare_item_itemlist.Add(newobjcet);
                    }
                }
                if (newobjcet.Box_Type != Information_Item_Box_Type.None)
                {
                    rare_box_itemlist.Add(newobjcet);
                }
                break;
            case Information_Item_Rarity_Type.Unique:
                if (newobjcet.Box_Type == Information_Item_Box_Type.Item)
                {
                    if (newobjcet.Item_Type == Information_Item_Type.Equipment)
                    {
                        unique_item_itemlist.Add(newobjcet);
                    }
                }
                if (newobjcet.Box_Type != Information_Item_Box_Type.None)
                {
                    unique_box_itemlist.Add(newobjcet);
                }
                break;
            case Information_Item_Rarity_Type.Epic:
                if (newobjcet.Box_Type == Information_Item_Box_Type.Item)
                {
                    if (newobjcet.Item_Type == Information_Item_Type.Equipment)
                    {
                        epic_item_itemlist.Add(newobjcet);
                    }
                }
                if (newobjcet.Box_Type != Information_Item_Box_Type.None)
                {
                    epic_box_itemlist.Add(newobjcet);
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
}