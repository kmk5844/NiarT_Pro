using System.Collections.Generic;
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
            CreatObjectsFromList();
        }
    }

    public Game_DataTable DataTable;
    void CreatObjectsFromList()
    {
        List<Info_Item> itemList = DataTable.Information_Item;
        foreach(Info_Item item in itemList)
        {
            SA_ItemData itemObject = new SA_ItemData();
            itemObject.Num = item.Num;
            itemObject.Item_Name = item.Item_Name.Replace("^", " ");
            itemObject.Item_Type = item.Item_Type;
            itemObject.Itme_Information = item.Item_Information;
            itemObject.Box_Type = item.Box_Type;
            itemObject.Buy_Flag = item.Buy_Flag;
            itemObject.Sell_Flag = item.Sell_Flag;
            itemObject.Item_Buy_Pride = item.Item_Buy_Pride;
            itemObject.Item_Sell_Pride = item.Item_Sell_Pride;
            itemObject.Supply_Monster = item.Supply_Monster;

            AssetDatabase.CreateAsset(itemObject,  "Assets/Scriptable/SA_Item/Item_Object/" + item.Item_Name.Replace("^", "_") + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}
