using PixelCrushers.DialogueSystem.Articy.Articy_4_0;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Auto_ScritableObject : EditorWindow
{
    [MenuItem("Tools/Create Auto ScritableObject")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Auto_ScritableObject));
    }
    string itemCount = "";
    string itemCount_Num = "";
    string itemCount_Single = "";

    private string[] Type = { "Item", "Stage", "Story" };

    //bool showBtn = true;
    int selectType;

    public Game_DataTable DataTable_Game;
    public Story_DataTable DataTable_Story;
    public SA_ItemList SA_ItemList_;
    public SA_StageList SA_StageList_;
    public SA_StoryLIst SA_StoryList_;
   
    private void OnGUI()
    {
        DataTable_Game = (Game_DataTable)EditorGUILayout.ObjectField("GameDataTable_", DataTable_Game, typeof(Game_DataTable), false);
        DataTable_Story = (Story_DataTable)EditorGUILayout.ObjectField("StoryDataTable_", DataTable_Story, typeof(Story_DataTable), false);

        selectType = EditorGUILayout.Popup("Type", selectType, Type);
        //showBtn = EditorGUILayout.Toggle("Item", showBtn);
        if (selectType == 0)
        {
            SA_ItemList_ = (SA_ItemList)EditorGUILayout.ObjectField("SA_ItemList_", SA_ItemList_, typeof(SA_ItemList), false);
            if (GUILayout.Button("Create Auto Item"))
            {
                SA_ItemList_.Editor_ItemList_Init();
                DeleteAllFilesInFolder_Item();
                CreatObjectsFromList_Item();
            }

            if (GUILayout.Button("Init Auto Item"))
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

            if (GUILayout.Button("Count Auto ItemSingle"))
            {
                Add_ItemSingleCount(itemCount_Num, itemCount_Single);
            }
        }
        else if (selectType == 1)
        {
            SA_StageList_ = (SA_StageList)EditorGUILayout.ObjectField("SA_StageList_", SA_StageList_, typeof(SA_StageList), false);

            if (GUILayout.Button("Create Auto Stage"))
            {
                SA_StageList_.Editor_StageList_Init();
                DeleteAllFilesInFolder_Stage();
                CreatObjectFromList_Stage();
            }
        }else if(selectType == 2)
        {
            SA_StoryList_ = (SA_StoryLIst)EditorGUILayout.ObjectField("SA_StoryList_", SA_StoryList_, typeof(SA_StoryLIst), false);

            if(GUILayout.Button("Create Auto Story"))
            {
                SA_StoryList_.Editor_StoryList_Init();
                DeleteAllFilesInFolder_Story();
                CreatObjectFromList_Story();
            }
        }
    }



    //아이템

    public static void DeleteAllFilesInFolder_Item()
    {
        string[] filePaths = Directory.GetFiles("Assets/_Scriptable/SA_Item/Item_Object/");
        foreach (string filePath in filePaths)
        {
            File.Delete(filePath);
        }
        // Refresh the Unity Editor to reflect changes
        AssetDatabase.Refresh();
    }

    void CreatObjectsFromList_Item()
    {
        List<Info_Item> itemList = DataTable_Game.Information_Item;

        //Empty ItemObject
        ItemDataObject itemObject = ScriptableObject.CreateInstance<ItemDataObject>();
        itemObject.Auto_Item_Insert(
            -1,
            "Empty Item",
            "비어있는 아이템",
            Information_Item_Type.Empty,
            "비어있는 아이템",
            Information_Item_Box_Type.Empty,
            Information_Item_Rarity_Type.Empty,
            false,
            false,
            false,
            0,
            0,
            false,
            0,
            0,
            0
            );
        AssetDatabase.CreateAsset(itemObject, "Assets/_Scriptable/SA_Item/Item_Object/EmptyItemObject.asset");
        AssetDatabase.SaveAssets();
        SA_ItemList_.ItemList_EmptyObject(itemObject);

        foreach (Info_Item item in itemList)
        {
            itemObject = ScriptableObject.CreateInstance<ItemDataObject>();
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

            AssetDatabase.CreateAsset(itemObject,  "Assets/_Scriptable/SA_Item/Item_Object/" +item.Num +"_"+ item.Item_Id.Replace("^", "_") + ".asset");
            AssetDatabase.SaveAssets();
            SA_ItemList_.ItemList_InsertObject(itemObject);
        }
        UnityEditor.EditorUtility.SetDirty(SA_ItemList_);
    }

    void Init_ItemCount()
    {
        foreach(ItemDataObject item in SA_ItemList_.Item)
        {
            item.Init();
            UnityEditor.EditorUtility.SetDirty(item);
        }
    }

    void Add_ItemCount(string count_s)
    {
        int count = int.Parse(count_s);
        foreach (ItemDataObject item in SA_ItemList_.Item)
        {
            item.Item_Count_UP(count);
            UnityEditor.EditorUtility.SetDirty(item);
        }
    }

    void Add_ItemSingleCount(string itemNum, string count_s)
    {
        int Num = int.Parse(itemNum);
        int count = int.Parse(count_s);

        ItemDataObject item = SA_ItemList_.Item[Num];
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

    //stage
    public static void DeleteAllFilesInFolder_Stage()
    {
        string[] filePaths = Directory.GetFiles("Assets/_Scriptable/SA_Stage/Stage_Object/");
        foreach (string filePath in filePaths)
        {
            File.Delete(filePath);
        }

        AssetDatabase.Refresh();
    }

    void CreatObjectFromList_Stage()
    {
        List<Info_Stage> StageList = DataTable_Game.Information_Stage;
        foreach(Info_Stage stage in StageList)
        {
            StageDataObject stageObject = ScriptableObject.CreateInstance<StageDataObject>();
            stageObject.Auto_Stage_Insert(
                stage.Number,
                stage.Destination_Distance,
                stage.Emerging_Monster,
                stage.Monster_Count,
                stage.Reward_Point,
                stage.Reward_Item,
                stage.Reward_ItemCount,
                stage.D_Grade,
                stage.C_Grade,
                stage.B_Grade,
                stage.A_Grade,
                stage.S_Grade,
                stage.Boss_Flag,
                stage.Emerging_Boss,
                stage.Boss_Monster_Count,
                stage.Boss_Distance
                );

            AssetDatabase.CreateAsset(stageObject, "Assets/_Scriptable/SA_Stage/Stage_Object/SDO_Stage_" + stage.Number + ".asset");
            AssetDatabase.SaveAssets();
            SA_StageList_.StageList_InsterObject(stageObject);
        }
        UnityEditor.EditorUtility.SetDirty(SA_StageList_);
    }


    //story
    public static void DeleteAllFilesInFolder_Story()
    {
        string[] filePaths = Directory.GetFiles("Assets/_Scriptable/SA_Story/Story_Object/");
        foreach (string filePath in filePaths)
        {
            File.Delete(filePath);
        }
        // Refresh the Unity Editor to reflect changes
        AssetDatabase.Refresh();
    }

    void CreatObjectFromList_Story()
    {
        List<Story_Branch_Entity> StoryList = DataTable_Story.Story_Branch;
        foreach(Story_Branch_Entity story in StoryList)
        {
            StoryDataObject storyObject = ScriptableObject.CreateInstance<StoryDataObject>();
            storyObject.Auto_StoryData_Insert(
               story.Story_Num,
               story.Branch_Index,
               story.BackGround,
               story.Story_End,
               story.Story_Title_Kr,
               story.Story_Title_En,
               story.Story_Title_Jp
               );

            AssetDatabase.CreateAsset(storyObject, "Assets/_Scriptable/SA_Story/Story_Object/SDO_Story_" + story.Story_Num + ".asset");
            AssetDatabase.SaveAssets();
            SA_StoryList_.StoryList_InsertObject(storyObject);
        }
        UnityEditor.EditorUtility.SetDirty(SA_StoryList_);
    }
}