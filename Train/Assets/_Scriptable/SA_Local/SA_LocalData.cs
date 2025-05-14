using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_LocalData", menuName = "Scriptable/LocalData", order = 4)]
public class SA_LocalData : ScriptableObject
{
    [SerializeField]
    private int local_index;
    public int Local_Index { get { return local_index; } }

    [SerializeField]
    private List<string> local_language;
    public List<string> Local_Language { get { return local_language; } }

    public void SA_Change_Local(int index)
    {
        local_index = index;
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("SA_LocalData_local_index", local_index);
    }

    public void Load()
    {
        try
        {
            local_index = PlayerPrefs.GetInt("SA_LocalData_local_index");
        }
        catch
        {
            GameManager.Instance.FILE_Critical();
        }
    }

    public void Init()
    {
        local_index = 0;
        Save();
    }
}