using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DicButton : MonoBehaviour
{
    public GameObject Monster;
    public Image dicimage;
    public GameObject Item;
    public Image Itemimage;

    DictionaryDirector dicDirector;
    //몬스터
    SA_Monster sa_monster;
    
    public int num;
    bool bossFlag;
    Button btn;
    bool flag;
    //아이템
    SA_ItemList sa_itemlist;

    private void Start()
    {
        //btn.interactable = sa_monster.Monster_Dic[num].monster_dic_flag;
    }

    public void SettingMonsterDicButton(DictionaryDirector _dicDirector, SA_Monster _monsterData, int _num, bool _boss)
    {
        Monster.SetActive(true);
        Item.SetActive(false);

        dicDirector = _dicDirector;
        sa_monster = _monsterData;
        num = _num;
        bossFlag = _boss;
        btn = GetComponent<Button>();
        if (!bossFlag)
        {
            flag = sa_monster.Monster_Dic[num].monster_dic_flag;
            btn.interactable = flag;
            if (flag)
            {
                dicimage.sprite = Resources.Load<Sprite>("Dictionary/Monster/Monster_On_" + num);
            }
            else
            {
                dicimage.sprite = Resources.Load<Sprite>("Dictionary/Monster/Monster_Off_" + num);
            }
        }
        else
        {
            flag = sa_monster.Boss_Dic[num].monster_dic_flag;
            btn.interactable = flag;
            if (flag)
            {
                dicimage.sprite = Resources.Load<Sprite>("Dictionary/Monster/Boss_On_" + num);
            }
            else
            {
                dicimage.sprite = Resources.Load<Sprite>("Dictionary/Monster/Boss_Off_" + num);
            }
        }
        AddButton(0);
    }

    public void SettingItemDicButton(DictionaryDirector _dicDirector, SA_ItemList _list, int index)
    {
        Monster.SetActive(false);
        Item.SetActive(true);

        dicDirector = _dicDirector;
        sa_itemlist = _list;
        num = _list.Item_Dic_List[index].item_num;
        btn = GetComponent<Button>();

        flag = sa_itemlist.Item_Dic_List[index].item_dic_flag;
        btn.interactable = flag;
        if (flag)
        {
            Itemimage.sprite = Resources.Load<Sprite>("Dictionary/Item/Item_On_" + num);
        }
        else
        {
            Itemimage.sprite = Resources.Load<Sprite>("Dictionary/Item/Item_Off_" + num);
        }
        AddButton(1);
    }

    public void AddButton(int _num)
    {
        // 0 : 몬스터 / 1 : 아이템 / 2 : 스토리
        if(_num == 0)
        {
            btn.onClick.AddListener(() => dicDirector.ShowMonsterInformation(num, bossFlag));
        }
        else if(_num == 1)
        {
            btn.onClick.AddListener(() => dicDirector.ShowItemInformation(num));
        }
    }
}