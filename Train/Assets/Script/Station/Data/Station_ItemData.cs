using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station_ItemData : MonoBehaviour
{
    public SA_ItemList SA_ItemList;
    public SA_ItemData SA_Player_ItemData;

    [Header("인벤토리에 들어있는 아이템")]
    public List<ItemDataObject> Equipment_Inventory_ItemList;
    public List<ItemDataObject> Common_Inventory_ItemList; 
    public List<ItemDataObject> Box_Inventory_ItemList; 
    public List<ItemDataObject> Material_Inventory_ItemList; 
    public List<ItemDataObject> Quest_Inventory_ItemList;

    [Header("재료 변화에 사용할 아이템")]
    public ItemDataObject ConvertionMaterial_object;
    public ItemDataObject Mercenary_Material_object;
    public ItemDataObject Common_Train_Material_object;
    public ItemDataObject Turret_Train_Material_object;
    public ItemDataObject Booster_Train_Material_object;

    [Header("상점에 들어있는 아이템 + 팔 수 있는 것까지")]
    public List<ItemDataObject> Store_Buy_itemList;
    public List<ItemDataObject> Store_Sell_itemList;
    private void Awake()
    {
        Check_Inventory_Item();
    }

    void Check_Inventory_Item()
    {
        foreach(ItemDataObject item in SA_ItemList.Item)
        {
            if(item.Item_Count != 0) // 가지고 있는 인벤토리
            {
                if(item.Item_Type == Information_Item_Type.Equipment)
                {
                    Equipment_Inventory_ItemList.Add(item);
                }
                if(item.Item_Type == Information_Item_Type.Inventory)
                {
                    Common_Inventory_ItemList.Add(item);
                }
                if(item.Item_Type == Information_Item_Type.Box)
                {
                    Box_Inventory_ItemList.Add(item);
                }
                if(item.Item_Type == Information_Item_Type.Material)
                {
                    Material_Inventory_ItemList.Add(item);
                }
                if(item.Item_Type == Information_Item_Type.Quset)
                {
                    Quest_Inventory_ItemList.Add(item);
                }

                if (item.Sell_Flag)
                {
                    Store_Sell_itemList.Add(item);
                }
            }

            //강화 재료 변환 전용
            if(item.Num == 49)
            {
                Mercenary_Material_object = item;
            }
            if(item.Num == 50)
            {
                Common_Train_Material_object = item;
            }
            if(item.Num == 51)
            {
                Turret_Train_Material_object = item;
            }
            if(item.Num == 52)
            {
                Booster_Train_Material_object = item;
            }
            if( item.Num == 53)
            {
                ConvertionMaterial_object = item;
            }

            if(item.Buy_Flag) // 상점 전용
            {
                Store_Buy_itemList.Add(item);
            }
        }
    }

    public void Plus_Inventory_Item(ItemDataObject item)
    {
        if (item.Item_Type == Information_Item_Type.Equipment)
        {
            Equipment_Inventory_ItemList.Add(item);
        }
        if (item.Item_Type == Information_Item_Type.Inventory)
        {
            Common_Inventory_ItemList.Add(item);
        }
        if (item.Item_Type == Information_Item_Type.Box)
        {
            Box_Inventory_ItemList.Add(item);
        }
        if (item.Item_Type == Information_Item_Type.Material)
        {
            Material_Inventory_ItemList.Add(item);
        }
        if (item.Item_Type == Information_Item_Type.Quset)
        {
            Quest_Inventory_ItemList.Add(item);
        }

        if (item.Sell_Flag)
        {
            Store_Sell_itemList.Add(item);
        }
    }

    public void Minus_Inventory_Item(ItemDataObject item)
    {
        if (item.Item_Type == Information_Item_Type.Equipment)
        {
            Equipment_Inventory_ItemList.Remove(item);
        }
        if (item.Item_Type == Information_Item_Type.Inventory)
        {
            Common_Inventory_ItemList.Remove(item);
        }
        if (item.Item_Type == Information_Item_Type.Box)
        {
            Box_Inventory_ItemList.Remove(item);
        }
        if (item.Item_Type == Information_Item_Type.Material)
        {
            Material_Inventory_ItemList.Remove(item);
        }
        if (item.Item_Type == Information_Item_Type.Quset)
        {
            Quest_Inventory_ItemList.Remove(item);
        }

        if (item.Sell_Flag)
        {
            Store_Sell_itemList.Remove(item);
        }
    }
}
