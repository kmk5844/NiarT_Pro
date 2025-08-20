using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DictionaryDirector : MonoBehaviour
{
    public Game_DataTable Data_Game;
    public SA_Monster SA_Monster_;
    public SA_ItemList SA_ItemList_;
    public SA_StoryLIst SA_StoryList_;
    [Space(20)]
    [Header("공통 UI")]
    //토글 작업
    public ToggleGroup DicToggleGroup;
    public GameObject[] DicWindow;

    [Space(20)]
    [Header("버튼 생성")]
    public DicButton dicButton;
    public DicButton_Story dicStoryButton;

    [Space(20)]
    [Header("몬스터 도감")]
    public Transform monsterTransform;
    public Image ShowMonster_Image;
    public LocalizeStringEvent ShowMonster_Name;
    public LocalizeStringEvent ShowMonster_Information;

    [Space(20)]
    [Header("아이템 도감")]
    //public MonsterDicButton ItemButton;
    public Transform itemTransform;
    public Image ShowItem_Image;
    public LocalizeStringEvent ShowItem_Name;
    public LocalizeStringEvent ShowItem_Information;

    [Space(20)]
    [Header("스토리 도감")]
    public Transform storyTransform;
    public Button EnterButton;
    public LocalizeStringEvent Story_Title;
    public LocalizeStringEvent Story_SubTitle;
    public LocalizeStringEvent Story_Plot;

    bool storyFlag;
    int storyNum;

    private void Start()
    {
        SettingToggle();
        Spawn_MonsterButton();
        Spawn_ItemButton();
        Spawn_StoryButton();
        SettingUI();
        SettingLocal();

        Dic_Init();
    }

    void SettingToggle()
    {
        foreach(var tog in DicToggleGroup.GetComponentsInChildren<Toggle>())
        {
            tog.onValueChanged.AddListener(ToggleChangeWindow);
        }
    }

    void ToggleChangeWindow(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < DicToggleGroup.transform.childCount; i++)
            {
                if (DicToggleGroup.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    DicWindow[i].SetActive(true);
                    DicToggleGroup.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    DicWindow[i].SetActive(false);
                    DicToggleGroup.transform.GetChild(i).GetComponent<Toggle>().interactable = true;
                }
            } 
        }
    }

    void SettingLocal()
    {
        ShowMonster_Name.StringReference.TableReference = "ExcelData_Table_St";
        ShowMonster_Information.StringReference.TableReference = "ExcelData_Table_St";
        ShowItem_Name.StringReference.TableReference = "ItemData_Table_St";
        ShowItem_Information.StringReference.TableReference = "ItemData_Table_St";
        Story_Title.StringReference.TableReference = "Story_St";
        Story_SubTitle.StringReference.TableReference = "Story_St";
        Story_Plot.StringReference.TableReference = "Story_St";
    }

    void SettingUI()
    {
        ShowMonster_Image.gameObject.SetActive(false);
        ShowMonster_Name.gameObject.SetActive(false);
        ShowMonster_Information.gameObject.SetActive(false);
        ShowItem_Image.gameObject.SetActive(false);
        ShowItem_Name.gameObject.SetActive(false);
        ShowItem_Information.gameObject.SetActive(false);
    }

    private void Spawn_MonsterButton()
    {
        int monster_count = SA_Monster_.Monster_Dic.Count;
        int num_ = 0;
        DicButton obj;
        for(int i = 0; i < monster_count; i++)
        {
            num_ = SA_Monster_.Monster_Dic[i].monster_num;
            obj = Instantiate(dicButton, monsterTransform);
            obj.SettingMonsterDicButton(this, SA_Monster_, i, false);
            obj.name = num_ + "_" + Data_Game.Information_Monster[i].Monster_Name;
        }

        int boss_count = SA_Monster_.Boss_Dic.Count;
        for (int i = 0; i < boss_count; i++)
        {
            num_ = SA_Monster_.Boss_Dic[i].monster_num;
            obj = Instantiate(dicButton, monsterTransform);
            obj.SettingMonsterDicButton(this, SA_Monster_, i, true);
            obj.name = num_ + "_" + Data_Game.Information_Boss[i].Monster_Name;
        }
        Resize(monsterTransform, monster_count + boss_count, 3);
    }
    public void ShowMonsterInformation(int num, bool boss)
    {
        if (!ShowMonster_Image.gameObject.activeSelf)
        {
            ShowMonster_Image.gameObject.SetActive(true);
            ShowMonster_Name.gameObject.SetActive(true);
            ShowMonster_Information.gameObject.SetActive(true);
        }

        if (!boss)
        {
            ShowMonster_Image.sprite = Resources.Load<Sprite>("Dictionary/Monster/Monster_On_" + num);
            ShowMonster_Name.StringReference.TableEntryReference = "Monster_" + num;
            ShowMonster_Information.StringReference.TableEntryReference = "Monster_Info_" + num;
        }
        else
        {
            ShowMonster_Image.sprite = Resources.Load<Sprite>("Dictionary/Monster/Boss_On_" + num);
            ShowMonster_Name.StringReference.TableEntryReference = "Boss_" + num;
            ShowMonster_Information.StringReference.TableEntryReference = "Boss_Info_" + num;
        }
    }

    private void Spawn_ItemButton()
    {
        int item_count = SA_ItemList_.Item_Dic_List.Count;
        int num_ = 0;
        DicButton obj;
        for (int i = 0; i < item_count; i++)
        {
            num_ = SA_ItemList_.Item_Dic_List[i].item_num;
            obj = Instantiate(dicButton, itemTransform);
            obj.SettingItemDicButton(this, SA_ItemList_, i);
            obj.name = num_ + "_" + Data_Game.Information_Item[num_].Item_Name;
        }

        Resize(itemTransform, item_count, 3);

    }
    public void ShowItemInformation(int num)
    {
        if (!ShowItem_Image.gameObject.activeSelf)
        {
            ShowItem_Image.gameObject.SetActive(true);
            ShowItem_Name.gameObject.SetActive(true);
            ShowItem_Information.gameObject.SetActive(true);
        }

        ShowItem_Image.sprite = Resources.Load<Sprite>("ItemIcon/" + num);
        ShowItem_Name.StringReference.TableEntryReference = "Item_Name_" + num;
        ShowItem_Information.StringReference.TableEntryReference = "Item_Information_" + num;
    }

    private void Spawn_StoryButton()
    {
        int story_count = SA_StoryList_.StoryList.Count;
        DicButton_Story obj;

        //cutScnene;
        obj = Instantiate(dicStoryButton, storyTransform);
        obj.SettingStoryButton_CutScene(this);

        for (int i = 0 ; i < story_count; i++) {
            obj = Instantiate(dicStoryButton, storyTransform);
            obj.SettingStoryButton(this, SA_StoryList_, i);
        }

        Resize(storyTransform, story_count+1, 1);
    }

    public void SetStory(bool flag, int num = -1)
    {
        storyFlag = flag;
        storyNum = num;

        if (!EnterButton.interactable)
        {
            EnterButton.interactable = true;
        }
        Story_Title.StringReference.TableEntryReference = "Story_Title_" + num;
        Story_SubTitle.StringReference.TableEntryReference = "Story_SubTitle_" + num;
        Story_Plot.StringReference.TableEntryReference = "Story_Plot_" + num;
    }

    public void Enter_StoryMode()
    {
        Debug.Log(storyFlag);
        if (storyFlag)
        {
            SceneManager.LoadScene("CutScene", LoadSceneMode.Additive);
        }
        else
        {
            SA_StoryList_.Select_Dic_Story_Num = storyNum;
            SceneManager.LoadScene("Story", LoadSceneMode.Additive);
        }
    }

    public void Dic_Init()
    {
        SettingUI();
        DicToggleGroup.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
        EnterButton.interactable = false;
    }

    public void Resize(Transform trans, int count, int cellCount)
    {
        RectTransform rectTransform = trans.GetComponent<RectTransform>();
        GridLayoutGroup grid = trans.GetComponent<GridLayoutGroup>();

        // 한 줄에 들어가는 셀 개수 계산
        int rowCount = Mathf.CeilToInt(count / (float)cellCount);

        // 전체 높이 = 위아래 패딩 + (rowCount × 셀 높이) + (rowCount-1) × Spacing
        float height = grid.padding.top
                     + grid.padding.bottom
                     + rowCount * grid.cellSize.y
                     + (rowCount - 1) * grid.spacing.y;

        Vector2 size = rectTransform.sizeDelta;
        size.y = height;
        rectTransform.sizeDelta = size;
    }
}