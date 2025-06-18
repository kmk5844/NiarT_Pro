using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class DictionaryDirector : MonoBehaviour
{
    public Game_DataTable Data_Game;
    public SA_Monster SA_Monster_;
    public SA_ItemList SA_ItemList_;
    [Space(20)]
    [Header("공통 UI")]
    //토글 작업
    public GameObject test;
    public DicButton dicButton;
    

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

    private void Start()
    {
        Spawn_MonsterButton();
        Spawn_ItemButton();
        SettingUI();
        SettingLocal();
    }

    void SettingLocal()
    {
        ShowMonster_Name.StringReference.TableReference = "ExcelData_Table_St";
        ShowMonster_Information.StringReference.TableReference = "ExcelData_Table_St";
        ShowItem_Name.StringReference.TableReference = "ItemData_Table_St";
        ShowItem_Information.StringReference.TableReference = "ItemData_Table_St";

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
            ShowMonster_Image.sprite = Resources.Load<Sprite>("Dictionary/Monster_On_" + num);
            ShowMonster_Name.StringReference.TableEntryReference = "Monster_" + num;
            ShowMonster_Information.StringReference.TableEntryReference = "Monster_Info_" + num;
        }
        else
        {
            ShowMonster_Image.sprite = Resources.Load<Sprite>("Dictionary/Boss_On_" + num);
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
            obj.name = num_ + "_" + Data_Game.Information_Item[i].Item_Name;
        }
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
}