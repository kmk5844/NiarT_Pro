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
    string itemCount = "";
    string itemCount_Num = "";
    string itemCount_Single = "";
    private void OnGUI()
    {
        if (GUILayout.Button("Create Auto Item"))
        {
            SA_ItemList.ItemList_Init();
            DeleteAllFilesInFolder();
            CreatObjectsFromList();
        }

        if(GUILayout.Button("Init Auto Item"))
        {
            Init_ItemCount();
        }

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("아이템 개수 적어주세요");
            itemCount = EditorGUILayout.TextField(itemCount, GUILayout.Width(200));
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Count Auto Item"))
        {
            Init_ItemCount();
            Add_ItemCount(itemCount);
        }


        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("아이템 넘버와 개수 적어주세요");
            itemCount_Num = EditorGUILayout.TextField(itemCount_Num, GUILayout.Width(200));
            itemCount_Single = EditorGUILayout.TextField(itemCount_Single, GUILayout.Width(200));
        }
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Count Auto ItemSingle"))
        {
            Add_ItemSingleCount(itemCount_Num, itemCount_Single);
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
                item.Max_Equip,
                item.Cool_Time,
                0
                );

            AssetDatabase.CreateAsset(itemObject,  "Assets/Scriptable/SA_Item/Item_Object/" +item.Num +"_"+ item.Item_Id.Replace("^", "_") + ".asset");
            AssetDatabase.SaveAssets();
            SA_ItemList.ItemList_InsertObject(itemObject);
        }
        UnityEditor.EditorUtility.SetDirty(SA_ItemList);
    }

    void Init_ItemCount()
    {
        foreach(ItemDataObject item in SA_ItemList.Item)
        {
            item.Init();
            UnityEditor.EditorUtility.SetDirty(item);
        }
    }

    void Add_ItemCount(string count_s)
    {
        int count = int.Parse(count_s);
        foreach (ItemDataObject item in SA_ItemList.Item)
        {
            item.Item_Count_UP(count);
            UnityEditor.EditorUtility.SetDirty(item);
        }
    }

    void Add_ItemSingleCount(string itemNum, string count_s)
    {
        int Num = int.Parse(itemNum);
        int count = int.Parse(count_s);

        ItemDataObject item = SA_ItemList.Item[Num];
        item.Init();
        item.Item_Count_UP(count);
        UnityEditor.EditorUtility.SetDirty(item);
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
        return Information_Item_Type.Empty;
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