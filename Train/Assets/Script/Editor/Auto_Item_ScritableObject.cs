using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Auto_Item_ScritableObject : EditorWindow
{
    [MenuItem("Tools/Create Auto Item")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Auto_Item_ScritableObject));
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Create Auto Item"))
        {
            SA_ItemList.ItemList_Init();
            DeleteAllFilesInFolder();
            CreatObjectsFromList();
        }
    }

    public Game_DataTable DataTable;
    public SA_ItemList SA_ItemList;

    public static void DeleteAllFilesInFolder()
    {
        string[] filePaths = Directory.GetFiles("Assets/Scriptable/SA_Item/Item_Object/");
        foreach (string filePath in filePaths)
        {
            File.Delete(filePath);
        }

        // Refresh the Unity Editor to reflect changes
        AssetDatabase.Refresh();
    }

    void CreatObjectsFromList()
    {
        List<Info_Item> itemList = DataTable.Information_Item;
        foreach(Info_Item item in itemList)
        {
            ItemDataObject itemObject = ScriptableObject.CreateInstance<ItemDataObject>();
            itemObject.Auto_Item_Insert(
                item.Num,
                item.Item_Id.Replace("^", ""),
                item.Item_Name,
                CheckItemType(item.Item_Type),
                item.Item_Information,
                CheckItemBoxType(item.Box_Type),
                CheckItemRarityType(item.Rarity_Type),
                item.Use_Flag,
                item.Buy_Flag,
                item.Sell_Flag,
                item.Item_Buy_Pride,
                item.Item_Sell_Pride,
                item.Supply_Monster,
                0
                );

            AssetDatabase.CreateAsset(itemObject,  "Assets/Scriptable/SA_Item/Item_Object/" +item.Num +"_"+ item.Item_Id.Replace("^", "_") + ".asset");
            AssetDatabase.SaveAssets();
            SA_ItemList.ItemList_InsertObject(itemObject);
        }
        UnityEditor.EditorUtility.SetDirty(SA_ItemList);
    }

    Information_Item_Type CheckItemType(string itemtype)
    {
        switch (itemtype)
        {
            case "Equipment":
                return Information_Item_Type.Equipment;
            case "Immediate":
                return Information_Item_Type.Immediate;
            case "Weapon":
                return Information_Item_Type.Weapon;
            case "Material":
                return Information_Item_Type.Material;
            case "Box":
                return Information_Item_Type.Box;
            case "Inventory":
                return Information_Item_Type.Inventory;
            case "Quset":
                return Information_Item_Type.Quset;
        }
        return Information_Item_Type.None;
    }

    Information_Item_Box_Type CheckItemBoxType(string boxtype)
    {
        switch (boxtype)
        {
            case "Item":
                return Information_Item_Box_Type.Item;
            case "Material":
                return Information_Item_Box_Type.Material;
            case "None":
                return Information_Item_Box_Type.None;
        }
        return Information_Item_Box_Type.None;
    }

    Information_Item_Rarity_Type CheckItemRarityType(string raritytype)
    {
        switch (raritytype)
        {
            case "Common":
                return Information_Item_Rarity_Type.Common;
            case "Rare":
                return Information_Item_Rarity_Type.Rare;
            case "Unique":
                return Information_Item_Rarity_Type.Unique;
            case "Epic":
                return Information_Item_Rarity_Type.Epic;
        }
        return Information_Item_Rarity_Type.Error;
    }
}