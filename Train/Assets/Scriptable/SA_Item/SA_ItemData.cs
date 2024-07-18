using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_ItemData", menuName = "Scriptable/SA_Item/Data", order = 8)]

public class SA_ItemData : ScriptableObject
{
    public ItemDataObject EmptyObject;
    [SerializeField]
    private List<int> equiped_item;
    public List<int> Equiped_Item { get { return equiped_item; } }

    [SerializeField]
    private List<int> equiped_item_count;
    public List<int> Equiped_Item_Count { get { return equiped_item_count;} }

    public void UseEquipedItem(int num)
    {
        if (equiped_item_count[num] == 1)
        {
            equiped_item_count[num] -= 1;
            
            equiped_item.RemoveAt(num);
            equiped_item.Insert(num, -1);
        }
        else
        {
            equiped_item_count[num] -= 1;
        }
        Save();
    }


    public void Empty_Item(int num)
    {
        equiped_item[num] = -1;
        equiped_item_count[num] = 0;
        Save();
    }
    public void Equip_Item(int num, ItemDataObject item, int count)
    {
        equiped_item[num] = item.Num;
        equiped_item_count[num] = count;
        Save();
    }

    public void Save()
    {
        ES3.Save<List<int>>(name + "_Equiped_Item", equiped_item);
        ES3.Save(name + "_Equiped_ItemCount", equiped_item_count);
    }
    public void Load()
    {
        equiped_item = ES3.Load<List<int>>(name + "_Equiped_Item");
        equiped_item_count = ES3.Load<List<int>>(name + "_Equiped_ItemCount");
    }

    public void Init()
    {
        for(int i = 0; i < equiped_item.Count; i++)
        {
            equiped_item[i] = -1;
            equiped_item_count[i] = 0;
        }
        Save();
    }
}