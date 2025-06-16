using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SA_Monster", menuName = "Scriptable/SA_Monster", order = 12)]
public class SA_Monster : ScriptableObject
{
    [SerializeField]
    private List<Monster_Dic_Def> monster_dic;
    public List<Monster_Dic_Def> Monster_Dic { get { return monster_dic; } }

    public void Editor_Init()
    {
        monster_dic.Clear();
    }

    public void Editor_Add(int num)
    {
        Monster_Dic_Def monsterInfo = new Monster_Dic_Def();
        monsterInfo.monster_num = num;
        monsterInfo.monster_dic_flag = false;

        monster_dic.Add(monsterInfo);
    }

    [Serializable]
    public struct Monster_Dic_Def{
        [SerializeField]
        public int monster_num;

        public bool monster_dic_flag;
        
        public void Change()
        {
            if (!monster_dic_flag)
            {
                monster_dic_flag = true;
                Save();
            }
        }

        public void Save()
        {
            ES3.Save<bool>("Monster_Dic_Flag_" + monster_num, monster_dic_flag);
        }

        public void Load()
        {
            monster_dic_flag = ES3.Load<bool>("Monster_Dic_Flag_" + monster_num, false);
        }
    }
}
