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
    public Image[] Equip_Item_Image;
    public Transform Inventory_DragItemList;
    public GameObject Inventory_DragObject;
    public ItemDataObject Draging_Item;
    public bool DragFlag;

    private void Start()
    {
        DragFlag = false;
        itemListData = GetComponent<Station_ItemData>();

        foreach(ItemDataObject item in itemListData.Equipment_Inventory_ItemList)
        {
            Inventory_DragObject.GetComponent<Image>().sprite = item.Item_Sprite;
            GameObject drag = Instantiate(Inventory_DragObject, Inventory_DragItemList);
            drag.SetActive(false);
            Inventory_ItemObject.GetComponent<ItemEquip_Object>().SetSetting(item, Inventory_ItemTooltip, drag, this);
            Instantiate(Inventory_ItemObject, Inventory_ItemList);
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
}
