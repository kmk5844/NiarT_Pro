using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_ItemList", menuName = "Scriptable/SA_Item/List", order = 9)]

public class SA_ItemList : ScriptableObject
{
    [SerializeField]
    private List<ItemDataObject> item;
    public List<ItemDataObject> Item { get { return item; } }

    public void ItemList_Init()
    {
        item.Clear();
    }

    public void ItemList_InsertObject(ItemDataObject newobjcet)
    {
        item.Add(newobjcet);
    }
}