using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station_ItemData : MonoBehaviour
{
    public SA_ItemList SA_ItemList;
    public SA_ItemData SA_Player_ItemData;

    [Header("장착하고 있는 아이템")]
    [SerializeField]
    ItemDataObject Equipment_Item_1;
    [SerializeField]
    ItemDataObject Equipment_Item_2;
    [SerializeField]
    ItemDataObject Equipment_Item_3;

    [Header("인벤토리에 들어있는 아이템")]
    public List<ItemDataObject> Equipment_Inventory_ItemList;
    public List<ItemDataObject> Common_Inventory_ItemList; 
    public List<ItemDataObject> Box_Inventory_ItemList; 
    public List<ItemDataObject> Material_Inventory_ItemList; 
    public List<ItemDataObject> Quest_Inventory_ItemList; 

    private void Awake()
    {
        Check_Inventory_Item();

        Equipment_Item_1 = SA_Player_ItemData.Equiped_Item[0];
        Equipment_Item_2 = SA_Player_ItemData.Equiped_Item[1];
        Equipment_Item_3 = SA_Player_ItemData.Equiped_Item[2];
    }

    void Check_Inventory_Item()
    {
        foreach(ItemDataObject item in SA_ItemList.Item)
        {
            if(item.Item_Count != 0)
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
            }
        }
    }
}
