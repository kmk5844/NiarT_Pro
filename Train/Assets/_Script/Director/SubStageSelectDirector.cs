using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
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
    Animator ani;

    MissionDataObject SelectSubStageData;
    int SelectSubStageNum;
    [SerializeField]
    List<int> NextSubStageNum;

    [Header("UI")]
    public TextMeshProUGUI UI_MissionInformation;
    public TextMeshProUGUI UI_MainStageText;

    public GameObject UI_MapTab;
    public GameObject UI_SubStageSelect;
    public GameObject UI_ItemTab;
    public GameObject UI_MissionCancelWindow;

    public Button UI_Title_StageButton;
    public Button UI_Title_ItemButton;

    public Button UI_NextButton;
    public Button UI_PrevButton;

    public int missionNum;
    public int stageNum;
    public int selectNum;

    [Header("UI_StageInformation")]
    public GameObject StageInitButton;
    public GameObject InformationObject;

    [Header("UI_ItemInformation")]
    public Image UI_Info_ItemIcon;
    public LocalizeStringEvent UI_Info_ItemNameText;
    public LocalizeStringEvent UI_Info_ItemInformationText;

    [Header("UI_ItemCount")]
    public GameObject UI_ItemCount;
    public Image UI_ItemIcon;
    public Slider UI_ItemCountSlider;
    public TextMeshProUGUI UI_ItemCountText;
    public TextMeshProUGUI UI_ItemMaxText;
    public Button UI_ItemCount_YesButton;
    public Button UI_ItemCount_NoButton;
    ItemEquip_Object Count_ItemEquipObjcet;
    UnityEngine.Events.UnityAction listner_Button;

    [Header("아이템 관리")]
    public Transform Inventory_ItemList;
    public GameObject Inventory_ItemObject;
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

    [Header("Option")]
    public GameObject Option;

    private void Start()
    {
        //itemListData = GetComponent<Station_ItemData>();
        ani = GetComponent<Animator>();
        DragItemCount = 0;
        selectNum = -1;

        Instantiate_Item();

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
            Equip_ItemObject[i].SetSetting(equiped_item, drag, this);
        }
        EmptyItemObject = itemListData.SA_Player_ItemData.EmptyObject;
        UI_Info_ItemIcon.sprite = EmptyItemObject.Item_Sprite;
        UI_Info_ItemNameText.StringReference.TableReference = "ItemData_Table_St";
        UI_Info_ItemInformationText.StringReference.TableReference = "ItemData_Table_St";
        UI_Info_ItemNameText.StringReference.TableEntryReference = "Item_Name_-1";
        UI_Info_ItemInformationText.StringReference.TableEntryReference = "Item_Information_-1";

        UI_MainStageText.text = "Stage" + (playerData.Select_Stage + 1);

        UI_NextButton.interactable = false;
        UI_PrevButton.interactable = false;
        StageInitButton.SetActive(false);
    }

    private void OnDisable()
    {
        if (UI_MissionCancelWindow.activeSelf)
        {
            UI_MissionCancelWindow.SetActive(false);
        }
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
        if(UI_SubStageSelect.transform.childCount < 2)
        {
            Instantiate(StageListObject, UI_SubStageSelect.transform);
        }
    }

    public void Open_SelectSubStage(MissionDataObject mission)
    {
        SelectSubStageData = mission;
        SelectSubStageNum = mission.SubStage_Num;
        playerData.SA_SelectSubStage(SelectSubStageNum);
        SpecialStage_Check();
    }

    public void Open_ItemTab()
    {
        ani.SetBool("Select",true);
        ani.SetTrigger("AniTrigger");
        UI_Title_StageButton.interactable = false;
        UI_Title_ItemButton.interactable = true;
        UI_NextButton.interactable = false;
        UI_PrevButton.interactable = true;
    }

    public void Open_SelectSubStage()
    {
        ani.SetBool("Select", false);
        ani.SetTrigger("AniTrigger");
        UI_Title_StageButton.interactable = true;
        UI_Title_ItemButton.interactable = false;
        UI_NextButton.interactable = true;
        UI_PrevButton.interactable = false;
    }

    public void Start_SelectSubStage()
    {
        if(SelectSubStageData.SubStage_Type == SubStageType.SimpleStation)
        {
            SpeacialStage_Clear();
            SpecialStage_LockOff();
            SceneManager.LoadScene("SimpleStation");
        }
        else // 전투
        {
            if (SceneManager.GetActiveScene().name != "CharacterSelect")
            {
                SceneManager.LoadScene("CharacterSelect");
            }
            else
            {
                this.gameObject.SetActive(false);
                UI_ItemTab.SetActive(false);
            }
        }
    }

    void SpeacialStage_Clear()
    {
        SelectSubStageData.SubStage_Clear();
    }

    void SpecialStage_LockOff()
    {
        foreach(int subStageNum in NextSubStageNum)
        {
            if(subStageNum != -1)
            {
                MissionDataObject mission = missionData.missionStage(missionNum, stageNum, subStageNum);
                mission.SubStageLockOff();
            }
        }
    }

    void SpecialStage_Check()
    {
        string[] nextSubStageList = SelectSubStageData.Open_SubStageNum.Split(',');
        if(NextSubStageNum == null)
        {
            NextSubStageNum = new List<int>();
        }
        else
        {
            NextSubStageNum.Clear();
        }
        
        foreach (string sub in nextSubStageList)
        {
            NextSubStageNum.Add(int.Parse(sub));
        }
    }

    public void OpenItemCountWindow(ItemEquip_Object item, bool Flag)
    {
        Count_ItemEquipObjcet = item;
        UI_ItemIcon.sprite = Count_ItemEquipObjcet.item.Item_Sprite;
        UI_ItemCountSlider.minValue = 0;
        if(Count_ItemEquipObjcet.item.Max_Equip > Count_ItemEquipObjcet.item.Item_Count)
        {
            UI_ItemCountSlider.maxValue = Count_ItemEquipObjcet.item.Item_Count;
        }
        else
        {
            UI_ItemCountSlider.maxValue = Count_ItemEquipObjcet.item.Max_Equip;
        }
        UI_ItemCountSlider.value = 0;

        UI_ItemCountText.color = Color.black;
        UI_ItemCountText.text = "0";
        UI_ItemMaxText.text = ((int)UI_ItemCountSlider.maxValue).ToString();
        UI_ItemCountSlider.onValueChanged.AddListener(ItemCount_ChangeText);

        UI_ItemCount.SetActive(true);
        listner_Button = () => ItemCount_YesButton(Count_ItemEquipObjcet, Flag);
        UI_ItemCount_YesButton.onClick.AddListener(listner_Button);
    }
    public void CloseItemCountWindow()
    {
        UI_ItemCount.SetActive(false);
        UI_ItemCountSlider.onValueChanged.RemoveListener(ItemCount_ChangeText);
        UI_ItemCount_YesButton.onClick.RemoveListener(listner_Button);
    }

    public void OpenCountChange(int objectNum)
    {
        OpenItemCountWindow(Equip_ItemObject[objectNum],false);
    }

    public void ItemCount_YesButton(ItemEquip_Object item, bool Flag)
    {
        int itemCount = (int)UI_ItemCountSlider.value;
        if(itemCount == 0)
        {
            CloseItemCountWindow();
        }
        else
        {
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
    }

    void ItemCount_ChangeText(float value)
    {
        if(value <= 0)
        {
            UI_ItemCountText.color = Color.black;
        }
        else
        {
            UI_ItemCountText.color = Color.red;
        }
        UI_ItemCountText.text = value.ToString();
    }
 
    public void ClickMissionCancel()
    {
        UI_MissionCancelWindow.SetActive(true);
        stageNum = playerData.Select_Stage;
        missionNum = playerData.Mission_Num;
    }

    public void Yes_MissionCancel()
    {
        playerData.SA_MissionPlaying(false);
        playerData.SA_GameLoseCoin(60f);
        missionData.SubStage_Lose(stageNum, missionNum);
        GameObject gm = GameObject.Find("SelectMission");
        Destroy(gm);
        SceneManager.LoadScene("Station");
    }

    public void No_MissionCancel()
    { 
        UI_MissionCancelWindow.SetActive(false);
    }

    public void ItemInformation_Setting(Sprite itemSprite, int itemNum)
    {
        UI_Info_ItemIcon.sprite = itemSprite;
        UI_Info_ItemNameText.StringReference.TableEntryReference = "Item_Name_" + itemNum;
        UI_Info_ItemInformationText.StringReference.TableEntryReference = "Item_Information_" + itemNum;
    }

    public void ClickSubStage(GameObject _informationObject)
    {
        InformationObject = _informationObject;
        StageInitButton.SetActive(true);
    }

    public void CancelSubStage()
    {
        InformationObject.SetActive(false);
        InformationObject = null;
        StageInitButton.SetActive(false);
    }

    public void OpenOption()
    {
        Option.SetActive(true);
    }

    //구매 및 판매시, 발동
    public void Check_Item()
    {
        foreach(ItemEquip_Object item in Inventory_ItemList.GetComponentsInChildren<ItemEquip_Object>())
        {
            Destroy(item.gameObject);
        }
        Instantiate_Item();
    }

    private void Instantiate_Item()
    {
        foreach (ItemDataObject item in itemListData.Equipment_Inventory_ItemList)
        {
            Inventory_DragObject.GetComponent<Image>().sprite = item.Item_Sprite;
            GameObject drag = Instantiate(Inventory_DragObject, Inventory_DragItemList);
            drag.SetActive(false);
            Inventory_ItemObject.GetComponent<ItemEquip_Object>().SetSetting(item, drag, this);
            Instantiate(Inventory_ItemObject, Inventory_ItemList);
        }
    }
}