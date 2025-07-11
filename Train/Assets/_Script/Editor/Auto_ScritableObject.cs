using PixelCrushers.DialogueSystem.Articy.Articy_4_0;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

    private string[] Type = { "Item", "Stage", "Quest" ,"Story", "Monster", "Item_Test"};

    //bool showBtn = true;
    int selectType;

    public Game_DataTable DataTable_Game;
    public Story_DataTable DataTable_Story;
    public Quest_DataTable DataTable_Quest;
    public SA_ItemList SA_ItemList_;
    public SA_StageList SA_StageList_;
    public SA_MissionData SA_MissionData_;
    public SA_StoryLIst SA_StoryList_;
    public SA_Monster SA_Monster_;
    public SA_ItemList_Test SA_ItemList_Test_;

    private void OnGUI()
    {
        DataTable_Game = (Game_DataTable)EditorGUILayout.ObjectField("GameDataTable_", DataTable_Game, typeof(Game_DataTable), false);
        DataTable_Story = (Story_DataTable)EditorGUILayout.ObjectField("StoryDataTable_", DataTable_Story, typeof(Story_DataTable), false);
        DataTable_Quest = (Quest_DataTable)EditorGUILayout.ObjectField("QuestDataTable_", DataTable_Quest, typeof(Quest_DataTable), false);

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
        }
        else if(selectType == 2)
        {
            SA_MissionData_ = (SA_MissionData)EditorGUILayout.ObjectField("SA_MissionData_", SA_MissionData_, typeof(SA_MissionData), false);

            if (GUILayout.Button("Create Auto Mission"))
            {
                SA_MissionData_.Editor_MissionList_Init(DataTable_Game.Information_Stage.Count);
                DeleteAllFilesInFolder_Mission();
                CreatObjectFromList_Mission();
            }
        }
        else if(selectType == 3)
        {
            SA_StoryList_ = (SA_StoryLIst)EditorGUILayout.ObjectField("SA_StoryList_", SA_StoryList_, typeof(SA_StoryLIst), false);

            if(GUILayout.Button("Create Auto Story"))
            {
                SA_StoryList_.Editor_StoryList_Init();
                DeleteAllFilesInFolder_Story();
                CreatObjectFromList_Story();
            }
        }else if(selectType == 4)
        {
            SA_Monster_ = (SA_Monster)EditorGUILayout.ObjectField("SA_Monster_", SA_Monster_, typeof(SA_Monster), false);

            if(GUILayout.Button("Create Auto Monster"))
            {
                SA_Monster_.Editor_Init();
                SetMonster();
            }
        }
        else if (selectType == 5)
        {
            SA_ItemList_Test_ = (SA_ItemList_Test)EditorGUILayout.ObjectField("SA_ItemList_Test", SA_ItemList_Test_, typeof(SA_ItemList_Test), false);
            if (GUILayout.Button("Create Auto Item"))
            {
                SA_ItemList_Test_.Editor_ItemList_Init();
                DeleteAllFilesInFolder_Item_Test();
                CreatObjectsFromList_Item_Test();
            }

            if (GUILayout.Button("Init Auto Item"))
            {
                Init_ItemCount_Test();
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("아이템 개수 적어주세요");
                itemCount = EditorGUILayout.TextField(itemCount, GUILayout.Width(200));
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Count Auto Item"))
            {
                Init_ItemCount_Test();
                Add_ItemCount_Test(itemCount);
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
                Add_ItemSingleCount_Test(itemCount_Num, itemCount_Single);
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
        foreach (Info_Stage stage in StageList)
        {
            StageDataObject stageObject = ScriptableObject.CreateInstance<StageDataObject>();
            stageObject.Auto_Stage_Insert(
                stage.Stage,
                stage.MissionList
                ) ;
            Debug.Log(stageObject.Stage_Num);

            /*                stage.Destination_Distance,
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
            */
            if (stage.Stage == 0)
            {
                stageObject.Open_StageChange();
            }

            AssetDatabase.CreateAsset(stageObject, "Assets/_Scriptable/SA_Stage/Stage_Object/SDO_Stage_" + stage.Stage + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SA_StageList_.StageList_InsterObject(stageObject);
        }
        UnityEditor.EditorUtility.SetDirty(SA_StageList_);
    }
    //quest

    public static void DeleteAllFilesInFolder_Mission()
    {
        string[] filePaths = Directory.GetDirectories("Assets/_Scriptable/SA_Mission/Mission_Object");
        foreach(string filePath in filePaths)
        {
            Directory.Delete(filePath, true);
        }
        AssetDatabase.Refresh();
    }


    void CreatObjectFromList_Mission()
    {
        List<Info_Q_Information> Q_Destination = DataTable_Quest.Q_Destination;
        List<Info_Q_Information> Q_Material = DataTable_Quest.Q_Material;
        List<Info_Q_Information> Q_Monster = DataTable_Quest.Q_Monster;
        List<Info_Q_Information> Q_Escort = DataTable_Quest.Q_Escort;
        List<Info_Q_Information> Q_Convoy = DataTable_Quest.Q_Convoy;
        List<Info_Q_Information> Q_Boss = DataTable_Quest.Q_Boss;

        foreach (Info_Q_Information Q_Des in Q_Destination)
        {
            MissionDataObject questObject = ScriptableObject.CreateInstance<MissionDataObject>();
            questObject.Auto_SubStage_Insert(
                0,
                Q_Des.Stage_Num,
                Q_Des.SubStage_Num,
                CheckSubStageType(Q_Des.SubStage_Type),
                Q_Des.Distance,
                Q_Des.Emerging_Monster,
                Q_Des.Monster_Count,
                Q_Des.Open_SubStageNum,
                Q_Des.SubStage_Status,
                Q_Des.StartStageFlag,
                Q_Des.NextStageFlag
                );

            string guide1 = "Assets/_Scriptable/SA_Mission/Mission_Object/0_Destination/Stage" + Q_Des.Stage_Num;

            if (!Directory.Exists(guide1)){
                Directory.CreateDirectory(guide1);
            }
            AssetDatabase.CreateAsset(questObject, guide1 + "/QDO_SubStage_" + "0_" + Q_Des.Stage_Num + "_" + Q_Des.SubStage_Num + "_" + Q_Des.SubStage_Type + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SA_MissionData_.Add_List(Q_Des.Stage_Num, 0, questObject);
        }

        foreach (Info_Q_Information Q_Mat in Q_Material)
        {
            MissionDataObject questObject = ScriptableObject.CreateInstance<MissionDataObject>();
            questObject.Auto_SubStage_Insert(
                1,
                Q_Mat.Stage_Num,
                Q_Mat.SubStage_Num,
                CheckSubStageType(Q_Mat.SubStage_Type),
                Q_Mat.Distance,
                Q_Mat.Emerging_Monster,
                Q_Mat.Monster_Count,
                Q_Mat.Open_SubStageNum,
                Q_Mat.SubStage_Status,
                Q_Mat.StartStageFlag,
                Q_Mat.NextStageFlag
                );

            string guide1 = "Assets/_Scriptable/SA_Mission/Mission_Object/1_Material/Stage" + Q_Mat.Stage_Num;

            if (!Directory.Exists(guide1))
            {
                Directory.CreateDirectory(guide1);
            }
            AssetDatabase.CreateAsset(questObject, guide1 + "/QDO_SubStage_" + "1_" + Q_Mat.Stage_Num + "_" + Q_Mat.SubStage_Num + "_" + Q_Mat.SubStage_Type + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SA_MissionData_.Add_List(Q_Mat.Stage_Num, 1, questObject);

        }

        foreach (Info_Q_Information Q_Mon in Q_Monster)
        {
            MissionDataObject questObject = ScriptableObject.CreateInstance<MissionDataObject>();
            questObject.Auto_SubStage_Insert(
                2,
                Q_Mon.Stage_Num,
                Q_Mon.SubStage_Num,
                CheckSubStageType(Q_Mon.SubStage_Type),
                Q_Mon.Distance,
                Q_Mon.Emerging_Monster,
                Q_Mon.Monster_Count,
                Q_Mon.Open_SubStageNum,
                Q_Mon.SubStage_Status,
                Q_Mon.StartStageFlag,
                Q_Mon.NextStageFlag
                );

            string guide1 = "Assets/_Scriptable/SA_Mission/Mission_Object/2_Monster/Stage" + Q_Mon.Stage_Num;

            if (!Directory.Exists(guide1))
            {
                Directory.CreateDirectory(guide1);
            }
            AssetDatabase.CreateAsset(questObject, guide1 + "/QDO_SubStage_" + "2_" + Q_Mon.Stage_Num + "_" + Q_Mon.SubStage_Num + "_" + Q_Mon.SubStage_Type + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SA_MissionData_.Add_List(Q_Mon.Stage_Num, 2, questObject);
        }

        foreach (Info_Q_Information Q_Esc in Q_Escort)
        {
            MissionDataObject questObject = ScriptableObject.CreateInstance<MissionDataObject>();
            questObject.Auto_SubStage_Insert(
                3,
                Q_Esc.Stage_Num,
                Q_Esc.SubStage_Num,
                CheckSubStageType(Q_Esc.SubStage_Type),
                Q_Esc.Distance,
                Q_Esc.Emerging_Monster,
                Q_Esc.Monster_Count,
                Q_Esc.Open_SubStageNum,
                Q_Esc.SubStage_Status,
                Q_Esc.StartStageFlag,
                Q_Esc.NextStageFlag
                );

            string guide1 = "Assets/_Scriptable/SA_Mission/Mission_Object/3_Escort/Stage" + Q_Esc.Stage_Num;

            if (!Directory.Exists(guide1))
            {
                Directory.CreateDirectory(guide1);
            }
            AssetDatabase.CreateAsset(questObject, guide1 + "/QDO_SubStage_" + "3_" + Q_Esc.Stage_Num + "_" + Q_Esc.SubStage_Num + "_" + Q_Esc.SubStage_Type + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SA_MissionData_.Add_List(Q_Esc.Stage_Num, 3, questObject);
        }

        foreach (Info_Q_Information Q_Con in Q_Convoy)
        {
            MissionDataObject questObject = ScriptableObject.CreateInstance<MissionDataObject>();
            questObject.Auto_SubStage_Insert(
                4,
                Q_Con.Stage_Num,
                Q_Con.SubStage_Num,
                CheckSubStageType(Q_Con.SubStage_Type),
                Q_Con.Distance,
                Q_Con.Emerging_Monster,
                Q_Con.Monster_Count,
                Q_Con.Open_SubStageNum,
                Q_Con.SubStage_Status,
                Q_Con.StartStageFlag,
                Q_Con.NextStageFlag
                );

            string guide1 = "Assets/_Scriptable/SA_Mission/Mission_Object/4_Convoy/Stage" + Q_Con.Stage_Num;

            if (!Directory.Exists(guide1))
            {
                Directory.CreateDirectory(guide1);
            }
            AssetDatabase.CreateAsset(questObject, guide1 + "/QDO_SubStage_" + "4_" + Q_Con.Stage_Num + "_" + Q_Con.SubStage_Num + "_" + Q_Con.SubStage_Type + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SA_MissionData_.Add_List(Q_Con.Stage_Num, 4, questObject);
        }

        foreach (Info_Q_Information Q_Bos in Q_Boss)
        {
            MissionDataObject questObject = ScriptableObject.CreateInstance<MissionDataObject>();
            questObject.Auto_SubStage_Insert(
                5,
                Q_Bos.Stage_Num,
                Q_Bos.SubStage_Num,
                CheckSubStageType(Q_Bos.SubStage_Type),
                Q_Bos.Distance,
                Q_Bos.Emerging_Monster,
                Q_Bos.Monster_Count,
                Q_Bos.Open_SubStageNum,
                Q_Bos.SubStage_Status,
                Q_Bos.StartStageFlag,
                Q_Bos.NextStageFlag
                );

            string guide1 = "Assets/_Scriptable/SA_Mission/Mission_Object/5_Boss/Stage" + Q_Bos.Stage_Num;

            if (!Directory.Exists(guide1))
            {
                Directory.CreateDirectory(guide1);
            }
            AssetDatabase.CreateAsset(questObject, guide1 + "/QDO_SubStage_" + "5_" + Q_Bos.Stage_Num + "_" + Q_Bos.SubStage_Num + "_" + Q_Bos.SubStage_Type + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SA_MissionData_.Add_List(Q_Bos.Stage_Num, 5, questObject);
        }
        UnityEditor.EditorUtility.SetDirty(SA_MissionData_);
    }

    SubStageType CheckSubStageType(string type)
    {
        switch (type)
        {
            case "Normal":
                return SubStageType.Normal;
            case "Hard":
                return SubStageType.Hard;
            case "HardCore":
                return SubStageType.HardCore;
            case "Boss":
                return SubStageType.Boss;
            case "Food":
                return SubStageType.Food;
            case "Treasure":
                return SubStageType.Treasure;
            case "Store":
                return SubStageType.Store;
            case "Maintenance":
                return SubStageType.Maintenance;
            case "SimpleStation":
                return SubStageType.SimpleStation;
        }
        return SubStageType.Error;
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
               story.Story_End//,
/*               story.Story_Title_Kr,
               story.Story_Title_En,
               story.Story_Title_Jp*/
               );

            AssetDatabase.CreateAsset(storyObject, "Assets/_Scriptable/SA_Story/Story_Object/SDO_Story_" + story.Story_Num + ".asset");
            AssetDatabase.SaveAssets();
            SA_StoryList_.StoryList_InsertObject(storyObject);
        }
        UnityEditor.EditorUtility.SetDirty(SA_StoryList_);
    }

    //Monster

    public void SetMonster()
    {
        foreach(Info_Monster monster in DataTable_Game.Information_Monster)
        {
            SA_Monster_.Editor_Add(monster.Number, false);
        }

        foreach(Info_Boss boss in DataTable_Game.Information_Boss)
        {
            SA_Monster_.Editor_Add(boss.Number, true);
        }

        UnityEditor.EditorUtility.SetDirty(SA_StoryList_);
    }



    //ItemList_Test
    public static void DeleteAllFilesInFolder_Item_Test()
    {
        string[] filePaths = Directory.GetFiles("Assets/_Scriptable/SA_Item/Item_Object_Test/");
        foreach (string filePath in filePaths)
        {
            File.Delete(filePath);
        }
        // Refresh the Unity Editor to reflect changes
        AssetDatabase.Refresh();
    }

    void CreatObjectsFromList_Item_Test()
    {
        List<Info_Item_Test> itemList = DataTable_Game.Information_Item2;

        //Empty ItemObject
        ItemDataObject_Test itemObject = ScriptableObject.CreateInstance<ItemDataObject_Test>();
        itemObject.Auto_Item_Insert(
            -1,
            "Empty Item",
            "비어있는 아이템",
            Information_Item_Type_Test.Empty,
            "비어있는 아이템",
            Information_Item_Rarity_Type_Test.Empty,
            "false,false,false",
            "0,0",
            0,
            0,
            0
            );
        AssetDatabase.CreateAsset(itemObject, "Assets/_Scriptable/SA_Item/Item_Object_Test/EmptyItemObject.asset");
        AssetDatabase.SaveAssets();
        SA_ItemList_Test_.ItemList_EmptyObject(itemObject);

        foreach (Info_Item_Test item in itemList)
        {
            itemObject = ScriptableObject.CreateInstance<ItemDataObject_Test>();
            itemObject.Auto_Item_Insert(
                item.Num,
                item.Item_Id.Replace("^", ""),
                item.Item_Name,
                CheckItemType_Test(item.Item_Type),
                item.Item_Information,
                CheckItemRarityType_Test(item.Rarity_Type),
                item.Buy_Sell_Supply_Flag,
                item.Buy_Sell_Pride,
                item.Max_Equip,
                item.Cool_Time,
                0
                );

            AssetDatabase.CreateAsset(itemObject, "Assets/_Scriptable/SA_Item/Item_Object_Test/" + item.Num + "_" + item.Item_Id.Replace("^", "_") + ".asset");
            AssetDatabase.SaveAssets();
            SA_ItemList_Test_.ItemList_InsertObject(itemObject);
        }
        UnityEditor.EditorUtility.SetDirty(SA_ItemList_Test_);
    }


    void Init_ItemCount_Test()
    {
        foreach (ItemDataObject_Test item in SA_ItemList_Test_.Item)
        {
            item.Init();
            UnityEditor.EditorUtility.SetDirty(item);
        }
    }

    void Add_ItemCount_Test(string count_s)
    {
        int count = int.Parse(count_s);
        foreach (ItemDataObject_Test item in SA_ItemList_Test_.Item)
        {
            item.Item_Count_UP(count);
            UnityEditor.EditorUtility.SetDirty(item);
        }
    }

    void Add_ItemSingleCount_Test(string itemNum, string count_s)
    {
        int Num = int.Parse(itemNum);
        int count = int.Parse(count_s);

        ItemDataObject_Test item = SA_ItemList_Test_.Item[Num];
        item.Init();
        item.Item_Count_UP(count);
        UnityEditor.EditorUtility.SetDirty(item);
    }

    Information_Item_Type_Test CheckItemType_Test(string itemtype)
    {
        switch (itemtype)
        {
            case "Equipment":
                return Information_Item_Type_Test.Equipment;
            case "Immediate":
                return Information_Item_Type_Test.Immediate;
            case "Weapon":
                return Information_Item_Type_Test.Weapon;
            case "Inventory":
                return Information_Item_Type_Test.Inventory;
            case "Quset":
                return Information_Item_Type_Test.Quset;
        }
        return Information_Item_Type_Test.Empty;
    }
    Information_Item_Rarity_Type_Test CheckItemRarityType_Test(string raritytype)
    {
        switch (raritytype)
        {
            case "Common":
                return Information_Item_Rarity_Type_Test.Common;
            case "Rare":
                return Information_Item_Rarity_Type_Test.Rare;
            case "Unique":
                return Information_Item_Rarity_Type_Test.Unique;
            case "Epic":
                return Information_Item_Rarity_Type_Test.Epic;
            case "Legendary": 
                return Information_Item_Rarity_Type_Test.Legendary;

        }
        return Information_Item_Rarity_Type_Test.Error;
    }
}