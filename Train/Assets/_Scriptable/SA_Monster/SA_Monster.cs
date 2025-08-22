using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[CreateAssetMenu(fileName = "SA_Monster", menuName = "Scriptable/SA_Monster", order = 12)]
public class SA_Monster : ScriptableObject
{
    [SerializeField]
    private List<Monster_Dic_Def> monster_dic;
    public List<Monster_Dic_Def> Monster_Dic { get { return monster_dic; } }

    [SerializeField]
    private List<Monster_Dic_Def> boss_dic;
    public List<Monster_Dic_Def> Boss_Dic {  get { return boss_dic; } } 

    public void Editor_Init()
    {
        monster_dic.Clear();
        boss_dic.Clear();
    }

    public void Editor_Add(int num, bool boss)
    {
        Monster_Dic_Def monsterInfo = new Monster_Dic_Def();
        monsterInfo.monster_num = num;
        monsterInfo.monster_dic_flag = false;

        if (!boss)
        {
            monster_dic.Add(monsterInfo);
        }
        else
        {
            boss_dic.Add(monsterInfo);
        }
    }

    public IEnumerator LoadSync()
    {
        foreach (var monster in monster_dic)
        {
            monster.Load(false);
            yield return new WaitForSeconds(0.001f); // 비동기 처리를 위해 약간의 대기 시간 추가
        }

        foreach (var boss in boss_dic)
        {
            boss.Load(true);
            yield return new WaitForSeconds(0.001f); // 비동기 처리를 위해 약간의 대기 시간 추가
        }
    }

    public IEnumerator InitAsync()
    {
        foreach (var monster in monster_dic)
        {
            if (monster.monster_dic_flag)
            {
                monster.Init(false);
                yield return new WaitForSeconds(0.001f); // 비동기 처리를 위해 약간의 대기 시간 추가
            }
        }

        foreach (var boss in boss_dic)
        {
            if (boss.monster_dic_flag)
            {
                boss.Init(true);
                yield return new WaitForSeconds(0.001f); // 비동기 처리를 위해 약간의 대기 시간 추가
            }
        }
    }

    [Serializable]
    public class Monster_Dic_Def{
        [SerializeField]
        public int monster_num;

        public bool monster_dic_flag;
        
        public void ChangeMonster(bool flag)
        {
            if (!monster_dic_flag)
            {
                monster_dic_flag = true;
                //Debug.Log(monster_dic_flag);
                Save(flag);
            }
        }

        public void Save(bool boss)
        {
            if (!boss)
            {
                ES3.Save<bool>("Monster_Dic_Flag_" + monster_num, monster_dic_flag);
            }
            else
            {
                ES3.Save<bool>("Boss_Dic_Flag_" + monster_num, monster_dic_flag);
            }
        }

        public void Load(bool boss)
        {
            if (!boss)
            {
                monster_dic_flag = ES3.Load<bool>("Monster_Dic_Flag_" + monster_num, false);
            }
            else
            {
                monster_dic_flag = ES3.Load<bool>("Boss_Dic_Flag_" + monster_num, false);
            }
        }

        public void Init(bool boss)
        {
            monster_dic_flag = false;
            Save(boss);
        }
    }
}
