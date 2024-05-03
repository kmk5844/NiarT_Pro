using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_LocalData", menuName = "Scriptable/LocalData", order = 4)]
public class SA_LocalData : ScriptableObject
{
    [SerializeField]
    private int local_index;
    public int Local_Index { get { return local_index; } }

    public void SA_Change_Local(int index)
    {
        local_index = index;
    }
}