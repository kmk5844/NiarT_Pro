using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_ItemData", menuName = "Scriptable/SA_Item/Data", order = 8)]

public class SA_ItemData : ScriptableObject
{
    public ItemDataObject EmptyObject;
    [SerializeField]
    private List<int> equiped_item;
    public List<int> Equiped_Item { get { return equiped_item; } }

    [SerializeField]
    private List<int> equiped_item_count;
    public List<int> Equiped_Item_Count { get { return equiped_item_count;} }

    public void UseEquipedItem(int num)
    {
        if (equiped_item_count[num] == 1)
        {
            equiped_item_count[num] -= 1;
            
            equiped_item.RemoveAt(num);
            equiped_item.Insert(num, -1);
        }
        else
        {
            equiped_item_count[num] -= 1;
        }
        Save();
    }


    public void Empty_Item(int num)
    {
        equiped_item[num] = -1;
        equiped_item_count[num] = 0;
        Save();
    }
    public void Equip_Item(int num, ItemDataObject item, int count)
    {
        equiped_item[num] = item.Num;
        equiped_item_count[num] = count;
        Save();
    }

    public void Check_AfterSell_EquipItem(int num)
    {
        int equipNum = -1;
        bool flag = false;
        foreach(int itemNum in equiped_item)
        {
            equipNum++;
            if(itemNum == num)
            {
                flag = true;
                break;
            }
        }

        if (flag)
        {
            Empty_Item(equipNum);
        }
        Save();
    }

    public void Save()
    {
        ES3.Save<List<int>>(name + "_Equiped_Item", equiped_item);
        ES3.Save(name + "_Equiped_ItemCount", equiped_item_count);
    }
    
    public IEnumerator SaveSync()
    {
        ES3.Save<List<int>>(name + "_Equiped_Item", equiped_item);
        yield return new WaitForSeconds(0.001f);
        ES3.Save(name + "_Equiped_ItemCount", equiped_item_count);
        yield return new WaitForSeconds(0.001f);
    }
    public void Load()
    {
        equiped_item = ES3.Load<List<int>>(name + "_Equiped_Item", new List<int> { -1, -1, -1 });
        equiped_item_count = ES3.Load<List<int>>(name + "_Equiped_ItemCount", new List<int> { 0, 0, 0 });
    }

    public IEnumerator LoadSync()
    {
        equiped_item = ES3.Load<List<int>>(name + "_Equiped_Item", new List<int> {-1,-1,-1 });
        equiped_item_count = ES3.Load<List<int>>(name + "_Equiped_ItemCount", new List<int> { 0, 0, 0 });
        yield return new WaitForSeconds(0.001f);
    }

    public void Init()
    {
        for(int i = 0; i < equiped_item.Count; i++)
        {
            equiped_item[i] = -1;
            equiped_item_count[i] = 0;
        }
        Save();
    }

    public IEnumerator InitAsync(MonoBehaviour runner)
    {
        for (int i = 0; i < equiped_item.Count; i++)
        {
            equiped_item[i] = -1;
            equiped_item_count[i] = 0;

            if(i % 5 == 0)
            {
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield return runner.StartCoroutine(SaveSync());

        yield return null;
    }

    public void Test(int num, int itemNum, int count)
    {
        equiped_item[num] = itemNum;
        equiped_item_count[num] = count;
        Save();
    }
}