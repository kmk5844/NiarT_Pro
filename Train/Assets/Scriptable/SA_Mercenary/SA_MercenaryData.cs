using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_MercenaryData", menuName = "Scriptable/MercenaryData", order = 3)]
public class SA_MercenaryData : ScriptableObject
{
    [SerializeField]
    private List<int> mercenary_num;
    public List<int> Mercenary_Num { get { return mercenary_num; } }

    [SerializeField]
    private int level_engine_driver;
    public int Level_Engine_Driver { get {  return level_engine_driver; } }

    [SerializeField]
    private int level_engineer;
    public int Level_Engineer { get { return level_engineer; } }

    [SerializeField]
    private int level_long_ranged;
    public int Level_Long_Ranged { get {  return level_long_ranged; } }

    [SerializeField]
    private int level_short_ranged;
    public int Level_Short_Ranged { get { return level_short_ranged; } }

    [SerializeField]
    private int level_medic;
    public int Level_Medic { get { return level_medic; } }

    [SerializeField]
    private Engine_Driver_Type engine_driver_type;
    public Engine_Driver_Type Engine_DriverType { get { return engine_driver_type; } }


    public void SA_Change_EngineDriver_Type(int Num)
    {
        if(Num == 0)
        {
            engine_driver_type = Engine_Driver_Type.speed;
        }
        else if(Num == 1)
        {
            engine_driver_type = Engine_Driver_Type.fuel;
        }
        else if(Num == 2)
        {
            engine_driver_type = Engine_Driver_Type.def;
        }
    }

    public void SA_Mercenary_Level_Up(int Num)
    {
        switch (Num)
        {
            case 0:
                level_engine_driver++;
                break;
            case 1:
                level_engineer++;
                break;
            case 2:
                level_long_ranged++;
                break;
            case 3:
                level_short_ranged++;
                break;
            case 4:
                level_medic++;
                break;
        }
    }
}
