using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_ItemData", menuName = "Scriptable/SA_Item/Data", order = 8)]

public class SA_ItemData : ScriptableObject
{
    public ItemDataObject EmptyObject;
    [SerializeField]
    private List<ItemDataObject> equiped_item;
    public List<ItemDataObject> Equiped_Item { get { return equiped_item; } }


    public void UseEquipedItem(int num)
    {
        equiped_item.RemoveAt(num);
        equiped_item.Insert(num, EmptyObject);
    }
}