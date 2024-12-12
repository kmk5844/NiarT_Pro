using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubStageSelectDirector : MonoBehaviour
{
    //UI겸해서 데이터 관리
    public SA_PlayerData playerData;
    public SA_StageList stageList;
    public Quest_DataTable EX_QuestData;
    public SA_MissionData missionData;
    public Station_ItemData itemListData;

    MissionDataObject SelectSubStageData;
    int SelectSubStageNum;

    [Header("UI")]
    public TextMeshProUGUI UI_MissionInformation;
    public GameObject UI_Content;

    public GameObject UI_SubStageInformationWindow;
    public TextMeshProUGUI UI_SubStageInformationText;

    public GameObject UI_MissionCancelWindow;
    int stageNum;
    int missionNum;

    [Header("UI_ItemCount")]
    public GameObject UI_ItemCount;
    public Image UI_ItemIcon;
    public Slider UI_ItemCountSlider;
    public TextMeshProUGUI UI_ItemCountText;
    public Button UI_ItemCount_YesButton;
    public Button UI_ItemCount_NoButton;
    ItemEquip_Object Count_ItemEquipObjcet;
    UnityEngine.Events.UnityAction listner;


    [Header("아이템 관리")]
    public Transform Inventory_ItemList;
    public GameObject Inventory_ItemObject;
    public ItemList_Tooltip Inventory_ItemTooltip;
    public ItemEquip_Object[] Equip_ItemObject;
    public Button[] ChangeCount_ItemObject;
    public Transform Inventory_DragItemList;
    public GameObject Inventory_DragObject;
    public GameObject DragingItemObject;
    public ItemDataObject Draging_Item;
    [HideInInspector]
    public bool HoldAndDragFlag;
    [HideInInspector]
    public float mouseHoldAndDragNotTime;
    [HideInInspector]
    public int DragItemCount;
    [HideInInspector]
    public ItemDataObject EmptyItemObject;
    public GameObject BeforeHoldItem;
    public GameObject EndHoldItem;


    private void Start()
    {
        //itemListData = GetComponent<Station_ItemData>();
        DragItemCount = 0;

        foreach (ItemDataObject item in itemListData.Equipment_Inventory_ItemList)
        {
            Inventory_DragObject.GetComponent<Image>().sprite = item.Item_Sprite;
            GameObject drag = Instantiate(Inventory_DragObject, Inventory_DragItemList);
            drag.SetActive(false);
            Inventory_ItemObject.GetComponent<ItemEquip_Object>().SetSetting(item, Inventory_ItemTooltip, drag, this);
            Instantiate(Inventory_ItemObject, Inventory_ItemList);
        }

        int itemNum = 0;
        for(int i = 0; i < 3; i++)
        {
            itemNum = itemListData.SA_Player_ItemData.Equiped_Item[i];
            ItemDataObject equiped_item;
            if (itemNum == -1)
            {
                equiped_item = itemListData.SA_Player_ItemData.EmptyObject;
            }
            else
            {
                equiped_item = itemListData.SA_ItemList.Item[itemNum];
            }
            Inventory_DragObject.GetComponent<Image>().sprite = equiped_item.Item_Sprite;
            GameObject drag = Instantiate(Inventory_DragObject, Inventory_DragItemList);
            drag.SetActive(false);
            Equip_ItemObject[i].SetSetting(equiped_item, Inventory_ItemTooltip, drag, this);
        }
        EmptyItemObject = itemListData.SA_Player_ItemData.EmptyObject;

    }

    public void Update()
    {
        if (HoldAndDragFlag)
        {
            if(mouseHoldAndDragNotTime < 0.003f)
            {
                mouseHoldAndDragNotTime += Time.deltaTime;
            }
            else
            {
                Draging_Item = null;
                HoldAndDragFlag = false;
            }
        }
    }

    public void OnEnable()
    {
        int missionNum = playerData.Mission_Num;
        int selectStageNum = playerData.Select_Stage;
        string searchString = playerData.Select_Stage + "," + missionNum;
        int missionInformation_Num = EX_QuestData.Q_List.FindIndex(x => x.Stage_Mission.Equals(searchString));

        UI_MissionInformation.text = EX_QuestData.Q_List[missionInformation_Num].Quest_Information;
        GameObject StageListObject = Resources.Load<GameObject>("UI_SubStageList/" + selectStageNum + "_Stage/" + missionNum);
        if(UI_Content.transform.childCount < 1)
        {
            Instantiate(StageListObject, UI_Content.transform);
        }
    }
    public void Open_SelectSubStage_Information(MissionDataObject mission)
    {
        UI_SubStageInformationWindow.SetActive(true);
        UI_SubStageInformationText.text = "stage type : " + mission.SubStage_Type + "\nstage distance : " + mission.Distance;
        SelectSubStageData = mission;
        SelectSubStageNum = mission.SubStage_Num;
        playerData.SA_SelectSubStage(SelectSubStageNum);
    }

    public void Close_SelectSubStage_Information()
    {
        UI_SubStageInformationWindow.SetActive(false);
    }

    public void Start_SelectSubStage()
    {
        if(SceneManager.GetActiveScene().name != "CharacterSelect")
        {
            SceneManager.LoadScene("CharacterSelect");
        }
        else
        {
            this.gameObject.SetActive(false);
            UI_SubStageInformationWindow.SetActive(false);
        }
    }

    public void OpenItemCountWindow(ItemEquip_Object item, bool Flag)
    {
        Count_ItemEquipObjcet = item;
        UI_ItemIcon.sprite = Count_ItemEquipObjcet.item.Item_Sprite;
        UI_ItemCountSlider.minValue = 1;
        if(Count_ItemEquipObjcet.item.Max_Equip > Count_ItemEquipObjcet.item.Item_Count)
        {
            UI_ItemCountSlider.maxValue = Count_ItemEquipObjcet.item.Item_Count;
        }
        else
        {
            UI_ItemCountSlider.maxValue = Count_ItemEquipObjcet.item.Max_Equip;
        }
        UI_ItemCountSlider.value = 1;
        UI_ItemCount.SetActive(true);
        listner = () => ItemCount_YesButton(Count_ItemEquipObjcet, Flag);
        UI_ItemCount_YesButton.onClick.AddListener(listner);
    }
    public void CloseItemCountWindow()
    {
        UI_ItemCount.SetActive(false);
        UI_ItemCount_YesButton.onClick.RemoveListener(listner);
    }

    public void OpenCountChange(int objectNum)
    {
        OpenItemCountWindow(Equip_ItemObject[objectNum],false);
    }

    public void ItemCount_YesButton(ItemEquip_Object item, bool Flag)
    {
        int itemCount = (int)UI_ItemCountSlider.value;
        int listIndex = itemListData.SA_Player_ItemData.Equiped_Item.IndexOf(item.item.Num);
        if (Flag)
        {
            if (listIndex != -1)
            {
                if (item.EquipObjectNum != listIndex)
                {
                    Equip_ItemObject[listIndex].Init_Item();
                }
            }
        }
        itemListData.SA_Player_ItemData.Equip_Item(item.EquipObjectNum, item.item, itemCount);
        item.Equip_Item(); //장착 아이템 오브젝트 정보 변경
        CloseItemCountWindow();
    }

    public void ClickMissionCancel()
    {
        UI_MissionCancelWindow.SetActive(true);
        stageNum = playerData.Select_Stage;
        missionNum = playerData.Mission_Num;
    }

    public void Yes_MissionCancel()
    {
        playerData.SA_GameLoseCoin(60f);
        missionData.SubStage_Lose(stageNum, missionNum);
        SceneManager.LoadScene("Station");
    }

    public void No_MissionCancel()
    { 
        UI_MissionCancelWindow.SetActive(false);
    }
}