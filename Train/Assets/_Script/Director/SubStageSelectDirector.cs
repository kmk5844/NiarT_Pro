using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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
    Station_ItemData itemListData;

    MissionDataObject SelectSubStageData;

    [Header("UI")]
    public TextMeshProUGUI UI_MissionInformation;
    public GameObject UI_Content;

    public GameObject UI_SubStageInformationWindow;
    public TextMeshProUGUI UI_SubStageInformationText;

    [Header("아이템 관리")]
    public Transform Inventory_ItemList;
    public GameObject Inventory_ItemObject;
    public ItemList_Tooltip Inventory_ItemTooltip;
    public ItemEquip_Object[] Equip_ItemObject;
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

    private void Awake()
    {
        itemListData = GetComponent<Station_ItemData>();
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
        selectStageNum = 0;
        missionNum = 0;
        GameObject StageListObject = Resources.Load<GameObject>("UI_SubStageList/" + selectStageNum + "_Stage/" + missionNum);
        Instantiate(StageListObject, UI_Content.transform);
    }
    public void Open_SelectSubStage_Information(MissionDataObject mission)
    {
        UI_SubStageInformationWindow.SetActive(true);
        UI_SubStageInformationText.text = "stage type : " + mission.SubStage_Type + "\nstage distance : " + mission.Distance;
        SelectSubStageData = mission;
    }

    public void Close_SelectSubStage_Information()
    {
        UI_SubStageInformationWindow.SetActive(false);
    }

    public void Start_SelectSubStage()
    {
        playerData.SA_SelectSubStage(SelectSubStageData.SubStage_Num);
        SceneManager.LoadScene("CharacterSelect");
    }

    public void OpenCount()
    {
        Debug.Log("유아이 오픈");
    }


    public void ChangeItem(int objectNum, ItemDataObject item, int itemCount)
    {

    }
}