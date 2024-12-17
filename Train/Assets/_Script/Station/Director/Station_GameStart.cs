using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Localization.Components;

public class Station_GameStart : MonoBehaviour
{
    [Header("데이터 모음")]
    public GameObject Player_DataObject;
    Station_PlayerData playerData;
    SA_StageList stageData;
    public GameObject Train_DataObject;
    Station_TrainData trainData;
    public GameObject Item_DataObject;
    Station_ItemData itemData;
    
    [Header("UI")]
    public ItemEquip_Object itemEquip_object;
    public ItemList_Tooltip itemTooltip_object;
    public Transform ItemList_Window;
    public Button[] Equiped_Button;
    public TextMeshProUGUI Stage_Text;
    public GameObject[] Click_Image;

    [Header("아이템")]
    public GameObject Inventory_Window;
    public GameObject ItemCount_Window;
    public Image ItemImage;
    public LocalizeStringEvent ItemNameText;
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI MaxText;
    public Button Button_Plus;
    public Button Button_Minus;
    public Button Button_Equip;
    public Button[] Button_ItemCountChange;
    public Button[] Button_ItemEmpty;

    [Header("스테이지 선택")]
    public Transform RouteMap_Content;
    public GameObject LevelStage_Object;
    GameObject[] LevelStage_Button;
    public GameObject[] FullMap_Stage_Button;
    public GameObject FullMap_Window;
    public FullMap_Director FullMap_Director;
    public Button[] prevAndnextRoute_Button;
    public Button GameStart_Button;
    public LocalizeStringEvent GameStart_Information_Text;
    public TextMeshProUGUI Score_Text;

    public ItemList_Grade[] Reward_Image;
    public GameObject[] Clear_Object;

    int Fuel_Count;
    int Item_Count; // 가지고 나갈 갯수
    int Max_Count; // 최대 갯수
    int itemObject_Count; // 게임오브젝트가 가지고 있는 갯수

    int Equipment_Button_Num;
    [HideInInspector]
    public bool EquipItemListFlag;
    [HideInInspector]
    public bool EquipItemWindowFlag;
    [HideInInspector]
    public bool FullMapFlag;
    bool FullMap_FirstOpenFlag;

    public int Select_StageNum;
    int Last_StageNum;

    private void Start()
    {
        trainData = Train_DataObject.GetComponent<Station_TrainData>();
        itemData = Item_DataObject.GetComponent<Station_ItemData>();
        playerData = Player_DataObject.GetComponent<Station_PlayerData>();
        stageData = playerData.SA_StageList;
        itemEquip_object.GameStartDirector = GetComponent<Station_GameStart>();
        itemEquip_object.item_tooltip_object = itemTooltip_object;
        GameStart_Information_Text.StringReference.TableReference = "Station_Table_St";
        Last_StageNum = -1;
        EquipItemListFlag = false;
        EquipItemWindowFlag = false;
        FullMapFlag = false;

        RouteMap_Content.GetComponent<RectTransform>().sizeDelta = new Vector2(1820 + (520 * (stageData.Stage.Count-1)), 425);
        LevelStage_Button = new GameObject[stageData.Stage.Count];
        for (int i = 0; i < stageData.Stage.Count; i++)
        {
            Vector2 pos = new Vector2(910 + (520 * i), -500f / 2);
            LevelStage_Object.GetComponent<StageButton_Route>().stageData = stageData.Stage[i];
            LevelStage_Object.GetComponent<StageButton_Route>().gamestartDirection = this;
            GameObject obj = Instantiate(LevelStage_Object, RouteMap_Content);
            obj.transform.localPosition = pos;
            obj.name = "Stage_Button_" + stageData.Stage[i].Stage_Num;
            if (i == stageData.Stage.Count - 1)
            {
                obj.GetComponent<StageButton_Route>().RoadObject.SetActive(false);
            }
            LevelStage_Button[i] = obj;
        }

        Select_StageNum = playerData.SA_PlayerData.New_Stage;
        StageButton_Click(Select_StageNum);

        for (int i = 0; i < Equiped_Button.Length; i++)
        {
            Equiped_ImageAndCount(i);

        }

        Button_ItemCountChange[0].onClick.AddListener(() => Open_ItemCountWindow(0, true));
        Button_ItemCountChange[1].onClick.AddListener(() => Open_ItemCountWindow(1, true));
        Button_ItemCountChange[2].onClick.AddListener(() => Open_ItemCountWindow(2, true));
        Spawn_Item();
    }

    public void StageButton_Click(int num)
    {
        Last_StageNum = Select_StageNum;
        Select_StageNum = num;
        Vector2 pos = new Vector2(-LevelStage_Button[num].transform.localPosition.x + 910, RouteMap_Content.localPosition.y);
        RouteMap_Content.localPosition = pos;
        if (Last_StageNum != -1)
        {
            LevelStage_Button[Last_StageNum].GetComponent<StageButton_Route>().MarkObject.SetActive(false);
        }

        LevelStage_Button[num].GetComponent<StageButton_Route>().MarkObject.SetActive(true);
        Chnage_Stage_Information();
        CheckRoute_Button();
    }
    public void Chnage_Stage_Information()
    {
        Stage_Text.text = "Stage " + (Select_StageNum + 1);
        //Score_Text.text = "Score : " + stageData.Stage[Select_StageNum].Player_Score;
/*        string[] ItemList = stageData.Stage[Select_StageNum].Reward_Item.Split(',');
        int itemNum;*/
/*        for(int i = 0; i <  ItemList.Length; i++)
        {
            itemNum = int.Parse(ItemList[i]);
            if(itemNum == -1)
            {
                Reward_Image[i].Item = itemData.SA_ItemList.EmptyItem;
                Reward_Image[i].item_Change_Flag = true;
            }
            else
            {
                Reward_Image[i].Item = itemData.SA_ItemList.Item[itemNum];
                Reward_Image[i].item_Change_Flag = true;
            }
        }*/
        for(int i = 0; i < Clear_Object.Length; i++)
        {
                Clear_Object[i].SetActive(false);
        }
/*        int grade_num = Check_PlayerGrade();
        if(grade_num != -1)
        {
            for(int i = 0; i < grade_num + 1; i++)
            {
                Clear_Object[i].SetActive(true);
            }
        }*/
    }

/*    int Check_PlayerGrade()
    {
        switch (stageData.Stage[Select_StageNum].Player_Grade) {
            case StageDataObject.Grade.S:
                return 4;
            case StageDataObject.Grade.A:
                return 3;
            case StageDataObject.Grade.B:
                return 2;
            case StageDataObject.Grade.C:
                return 1;
            case StageDataObject.Grade.D:
                return 0;
            case StageDataObject.Grade.F:
                return -1;
        }
        return -1;
    }*/

    public void PrevRoute_Button()
    {
        int num = Select_StageNum - 1;
        StageButton_Click(num);
    }

    public void NextRoute_Button()
    {
        int num = Select_StageNum + 1;
        StageButton_Click(num);
    }

    void CheckRoute_Button()
    {
        if(Select_StageNum == 0)
        {
            prevAndnextRoute_Button[0].interactable = false;
        }
        else
        {
            prevAndnextRoute_Button[0].interactable = true;
        }

        if (Select_StageNum == playerData.SA_PlayerData.New_Stage)
        {
            prevAndnextRoute_Button[1].interactable = false;
        }
        else
        {
            prevAndnextRoute_Button[1].interactable = true;
        }
    }

    private void Equiped_ImageAndCount(int buttonNum)
    {
        int num = itemData.SA_Player_ItemData.Equiped_Item[buttonNum];
        if (num == -1)
        {
            Equiped_Button[buttonNum].GetComponent<Image>().sprite = itemData.SA_Player_ItemData.EmptyObject.Item_Sprite;
            Equiped_Button[buttonNum].GetComponentInChildren<TextMeshProUGUI>().text = "";
            Button_ItemCountChange[buttonNum].gameObject.SetActive(false);
            Button_ItemEmpty[buttonNum].gameObject.SetActive(false);
        }
        else
        {
            Equiped_Button[buttonNum].GetComponent<Image>().sprite = itemData.SA_ItemList.Item[num].Item_Sprite;
            Equiped_Button[buttonNum].GetComponentInChildren<TextMeshProUGUI>().text = itemData.SA_Player_ItemData.Equiped_Item_Count[buttonNum].ToString();
            Button_ItemCountChange[buttonNum].gameObject.SetActive(true);
            Button_ItemEmpty[buttonNum].gameObject.SetActive(true);
        }
    }

    private void Spawn_Item()
    {
        foreach (ItemDataObject item in itemData.Equipment_Inventory_ItemList)
        {
            itemEquip_object.item = item;
            if (itemData.SA_Player_ItemData.Equiped_Item[0] == item.Num)
            {
                itemEquip_object.item_equip = true;
            }else if (itemData.SA_Player_ItemData.Equiped_Item[1] == item.Num)
            {
                itemEquip_object.item_equip = true;
            }else if (itemData.SA_Player_ItemData.Equiped_Item[2] == item.Num)
            {
                itemEquip_object.item_equip = true;
            }
            else
            {
                itemEquip_object.item_equip = false;
            }
            Instantiate(itemEquip_object, ItemList_Window);
        }
    }

    public void Check_Train()
    {
        Fuel_Count = 0;
        for(int i = 0; i < trainData.Train_Num.Count; i++)
        {
            if(trainData.Train_Num[i] / 10 == 1)
            {
                Fuel_Count++;
            }
        }

        if(Fuel_Count == 0)
        {
            GameStart_Information_Text.GetComponent<TextMeshProUGUI>().color = Color.red;
            GameStart_Information_Text.StringReference.TableEntryReference = "UI_GameStart_Start_Information_Text_0";
            //TrainText.text = "원활한 게임 플레이를 위해\n최소 연료 기차 한 대가 필요합니다.";
            GameStart_Button.interactable = false;
        }
        else
        {
            GameStart_Information_Text.GetComponent<TextMeshProUGUI>().color = Color.white;
            GameStart_Information_Text.StringReference.TableEntryReference = "UI_GameStart_Start_Information_Text_1";
            //TrainText.text = "게임 시작이 가능합니다.";
            GameStart_Button.interactable = true;
        }
    }

    public void Click_GameStart()
    {
        playerData.SA_PlayerData.SA_SelectLevel(Select_StageNum);
        SceneManager.LoadScene("MissionSelect");

        /*try
        { 
            GameManager.Instance.BeforeGameStart_Enter();
        }
        catch {
        }*/
        //LoadingManager.LoadScene("CharacterSelect");
    }

    public void Open_Inventory_Window(int num)
    {
        if (EquipItemListFlag)
        {
            Click_Image[Equipment_Button_Num].SetActive(false);
        }
        Equipment_Button_Num = num;
        Click_Image[num].SetActive(true);
        StartCoroutine(Open_Inventory_Window_Ani(num));
    }

    IEnumerator Open_Inventory_Window_Ani(int num)
    {
        EquipItemListFlag = true;

        RectTransform Item_Window = Inventory_Window.GetComponent<RectTransform>();
        float startX = Item_Window.anchoredPosition.x;
        float targetX = -620f;
        if(Item_Window.anchoredPosition.x != targetX)
        {
            float duration = 0.1f;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                float newX = Mathf.Lerp(startX, targetX, t);
                Item_Window.anchoredPosition = new Vector2(newX, Item_Window.anchoredPosition.y);
                yield return null;
            }
        }
        else
        {
            //Button_ItemCountChange.onClick.RemoveAllListeners(); // 버그 방지용 (이거 없이도 작동이 되긴 되나 버그 방지용으로 만듦)
            //Button_ItemEmpty.onClick.RemoveAllListeners();
        }

/*        Button_ItemCountChange.onClick.AddListener(() => Open_ItemCountWindow
            (itemData.SA_Player_ItemData.Equiped_Item[num]));*/
        //Button_ItemEmpty.onClick.AddListener(() => Click_EmptyItem(num));
        GameStart_Button.interactable = false;
    }

    public void Close_Inventory_Window()
    {
        EquipItemListFlag = false;
        Click_Image[Equipment_Button_Num].SetActive(false);
        StartCoroutine(Close_Inventory_Window_Ani());
    }

    IEnumerator Close_Inventory_Window_Ani()
    {
        RectTransform Item_Window = Inventory_Window.GetComponent<RectTransform>();
        float startX = Item_Window.anchoredPosition.x;
        float targetX = -1300f;

        float duration = 0.1f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newX = Mathf.Lerp(startX, targetX, t);
            Item_Window.anchoredPosition = new Vector2(newX, Item_Window.anchoredPosition.y);
            yield return null;
        }

        //Button_ItemCountChange.onClick.RemoveAllListeners();
        //Button_ItemEmpty.onClick.RemoveAllListeners();
        Equipment_Button_Num = -1;
        GameStart_Button.interactable = true;
    }

    public void Click_EmptyItem(int num)
    {
        foreach (ItemEquip_Object itemList_Object in ItemList_Window.GetComponentsInChildren<ItemEquip_Object>())
        {
            if (itemList_Object.item.Num == itemData.SA_Player_ItemData.Equiped_Item[num])
            {
                itemList_Object.item_equip = false;
                itemList_Object.Change_EquipFlag();
                break;
            }
        }
        itemData.SA_Player_ItemData.Empty_Item(num);
        Equiped_ImageAndCount(num);
    }

    public void Open_ItemCountWindow(int Num, bool flag) // Ture면 버튼, False면 아이템
    {
        EquipItemWindowFlag = true;
        int item_Num = 0;
        if (flag)
        {
            Equipment_Button_Num = Num;
            item_Num = itemData.SA_Player_ItemData.Equiped_Item[Equipment_Button_Num];
        }
        else
        {
            item_Num = Num;
        }
        ItemDataObject item = itemData.SA_ItemList.Item[item_Num];
        Item_Count = 1;
        ItemImage.sprite = item.Item_Sprite;
        //ItemNameText.text = item.Item_Name;

        ItemNameText.StringReference.TableReference = "ItemData_Table_St";
        ItemNameText.StringReference.TableEntryReference = "Item_Name_" + item.Num;

        CountText.text = Item_Count.ToString();
        itemObject_Count = item.Item_Count;
        Max_Count = item.Max_Equip;
        CheckCount();
        MaxText.text = "Max : " + Max_Count;
        ItemCount_Window.SetActive(true);
        Button_Equip.onClick.AddListener(() => Equip_Item(item));
    }

    public void Equip_Item(ItemDataObject item)
    {
        if (itemData.SA_Player_ItemData.Equiped_Item[Equipment_Button_Num] != -1)
        {
            foreach (ItemEquip_Object itemList_Object in ItemList_Window.GetComponentsInChildren<ItemEquip_Object>())
            {
                if (itemList_Object.item.Num == itemData.SA_Player_ItemData.Equiped_Item[Equipment_Button_Num])
                {
                    itemList_Object.item_equip = false;
                    itemList_Object.Change_EquipFlag();
                    break;
                }
            }
        }
        itemData.SA_Player_ItemData.Equip_Item(Equipment_Button_Num, item, Item_Count);
        Equiped_ImageAndCount(Equipment_Button_Num);

        foreach (ItemEquip_Object itemList_Object in ItemList_Window.GetComponentsInChildren<ItemEquip_Object>())
        {
            if(itemList_Object.item == item)
            {
                itemList_Object.item_equip = true;
                itemList_Object.Change_EquipFlag();
                break;
            }
        }

        Close_Inventory_Window();
        Close_ItemCountWindow();
    }

    public void Close_ItemCountWindow()
    {
        EquipItemWindowFlag = false;
        ItemCount_Window.SetActive(false);
        //Button_ItemCountChange.onClick.RemoveAllListeners();
        Button_Equip.onClick.RemoveAllListeners();
    }
    public void Open_FullMapWindow()
    {
        FullMapFlag = true;
        if (FullMap_FirstOpenFlag)
        {
            FullMap_Director.OpenFullMap();
        }
        else
        {
            FullMap_FirstOpenFlag = true;
        }
        FullMap_Window.SetActive(true);
    }

    public void Close_FullMapWindow()
    {
        FullMapFlag = false;
        FullMap_Window.SetActive(false);
    }

    public void CountPlus()
    {
        Item_Count++;
        CountText.text = Item_Count.ToString();
        CheckCount();
    }

    public void CountMInus()
    {
        Item_Count--;
        CountText.text = Item_Count.ToString();
        CheckCount();
    }

    private void CheckCount()
    {
        if(Item_Count == 1)
        {
            Button_Minus.interactable = false;
        }
        else
        {
            Button_Minus.interactable = true;
        }

        if(itemObject_Count > Max_Count)
        {
            if (Item_Count == Max_Count)
            {
                Button_Plus.interactable = false;
            }
            else
            {
                Button_Plus.interactable = true;
            }
        }
        else
        {
            if(Item_Count == itemObject_Count)
            {
                Button_Plus.interactable = false;
            }
            else
            {
                Button_Plus.interactable = true;
            }
        }
    }

    public void Director_Init_EquipItem()
    {
        foreach(ItemEquip_Object _item in ItemList_Window.GetComponentsInChildren<ItemEquip_Object>())
        {
            Destroy(_item.gameObject);
        }
        Spawn_Item();
        Equiped_ImageAndCount(0);
        Equiped_ImageAndCount(1);
        Equiped_ImageAndCount(2);
    }
}