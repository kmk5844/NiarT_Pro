using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_MercenaryData", menuName = "Scriptable/MercenaryData", order = 3)]
public class SA_MercenaryData : ScriptableObject
{
    [SerializeField]
    private List<int> mercenary_num;
    public List<int> Mercenary_Num { get { return mercenary_num; } }
}
