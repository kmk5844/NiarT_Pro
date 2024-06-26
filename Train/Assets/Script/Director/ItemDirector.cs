using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDirector : MonoBehaviour
{
    public SA_ItemData itemData;
    UseItem useitem;
    private void Start()
    {
        useitem = GetComponent<UseItem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (itemData.Equiped_Item[0].Item_Type != Information_Item_Type.None)
            {
                useitem.UseEquipItem(itemData.Equiped_Item[0].Num);
                itemData.UseEquipedItem(0);
            }
            else
            {
                Debug.Log("아이템이 비어있습니다.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (itemData.Equiped_Item[1].Item_Type != Information_Item_Type.None)
            {
                useitem.UseEquipItem(itemData.Equiped_Item[1].Num);
                itemData.UseEquipedItem(1);
            }
            else
            {
                Debug.Log("아이템이 비어있습니다.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (itemData.Equiped_Item[2].Item_Type != Information_Item_Type.None)
            {
                useitem.UseEquipItem(itemData.Equiped_Item[2].Num);
                itemData.UseEquipedItem(2);
            }
            else
            {
                Debug.Log("아이템이 비어있습니다.");
            }
        }
    }
}