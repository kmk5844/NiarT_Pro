using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDirector : MonoBehaviour
{
    public UIDirector uiDirector;
    public SA_ItemData itemData;
    UseItem useitem;
    private void Start()
    {
        try
        {
            itemData.Load();
        }
        catch
        {
            itemData.Init();
        }

        for(int i = 0; i < itemData.Equiped_Item.Count; i++)
        {
            uiDirector.Item_EquipedIcon(i, itemData.Equiped_Item[i].Item_Sprite, itemData.Equiped_Item_Count[i]);
        }
        useitem = GetComponent<UseItem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Change_EquipedItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Change_EquipedItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Change_EquipedItem(2);
        }
    }

    private void Change_EquipedItem(int num)
    {
        if (itemData.Equiped_Item[num].Item_Type != Information_Item_Type.Empty)
        {
            useitem.UseEquipItem(itemData.Equiped_Item[num].Num);
            itemData.UseEquipedItem(num);
            uiDirector.Item_EquipedIcon(num, itemData.Equiped_Item[num].Item_Sprite, itemData.Equiped_Item_Count[num]);
        }
        else
        {
            Debug.Log("사용 아이템이 비어있습니다.");
        }
    }

    public void Get_Supply_Item_Information(Sprite icon, string name, string information)
    {
        uiDirector.ItemInformation_On(icon, name, information);
    }
}