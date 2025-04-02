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
    private int level_bard;
    public int Level_Bard { get { return level_bard; } }

    [SerializeField]
    private int level_cowboy;
    public int Level_CowBoy { get { return level_cowboy; } }

    [SerializeField]
    private Engine_Driver_Type engine_driver_type;
    public Engine_Driver_Type Engine_Driver_Type { get { return engine_driver_type; } }

    [SerializeField]
    private Bard_Type bard_type;
    public Bard_Type Bard_Type { get { return bard_type; } }

    [SerializeField]
    private List<int> mercenary_buy_num;
    public List<int> Mercenary_Buy_Num { get { return mercenary_buy_num; } }

    public void SA_Change_EngineDriver_Type(int DropDown_Num)
    {
        if(DropDown_Num == 0)
        {
            engine_driver_type = Engine_Driver_Type.speed;
        }
        else if(DropDown_Num == 1)
        {
            engine_driver_type = Engine_Driver_Type.fuel;
        }
        else if(DropDown_Num == 2)
        {
            engine_driver_type = Engine_Driver_Type.def;
        }
        Save();
    }

    public int SA_Get_EngineDriver_Type_DropDown_Value()
    {
        if (engine_driver_type == Engine_Driver_Type.speed)
        {
            return 0;
        }else if (engine_driver_type == Engine_Driver_Type.fuel)
        {
            return 1;
        }else if(engine_driver_type == Engine_Driver_Type.def)
        {
            return 2;
        }
        else
        {
            return -1;
        }
    }

    public void SA_Change_Bard_Type(int DropDown_Num)
    {
        if(DropDown_Num == 0)
        {
            bard_type = Bard_Type.HP_Buff;
        }
        else if(DropDown_Num == 1)
        {
            bard_type = Bard_Type.Atk_Buff;
        }else if(DropDown_Num == 2)
        {
            bard_type = Bard_Type.Def_Buff;
        }
        Save();
    }

    public int SA_Get_Bard_Type_DropDown_Value()
    {
        if (bard_type == Bard_Type.HP_Buff)
        {
            return 0;
        }
        else if (bard_type == Bard_Type.Atk_Buff)
        {
            return 1;
        }
        else if (bard_type == Bard_Type.Def_Buff)
        {
            return 2;
        }
        else
        {
            return -1;
        }
    }

    public void SA_Mercenary_Num_Plus(int i)
    {
        mercenary_num.Add(i);
        Save();
    }

    public void SA_Mercenary_Change(int index, int MercenaryNum)
    {
        mercenary_num[index] = MercenaryNum;
        Save();
    }

    public void SA_Mercenary_Num_Remove(int i)
    {
        mercenary_num.Remove(i);
        Save();
    }

    public void SA_Mercenary_Buy(int Num)
    {
        mercenary_buy_num.Add(Num);
        Save();
    }
    public void SA_Mercenary_Level_Up(int Num)
    {
        switch (Num)
        {
            case 0:
                level_engine_driver++;
                Save();
                break;
            case 1:
                level_engineer++;
                Save();
                break;
            case 2:
                level_long_ranged++;
                Save();
                break;
            case 3:
                level_short_ranged++;
                Save();
                break;
            case 4:
                level_medic++;
                Save();
                break;
            case 5:
                level_bard++;
                Save();
                break;
            case 6:
                level_cowboy++;
                Save();
                break;
        }
    }
    private void Save()
    {
        ES3.Save("SA_Mercenary_Data_Data_mercenary_num", mercenary_num);
        ES3.Save("SA_Mercenary_Data_Data_level_engine_driver", level_engine_driver);
        ES3.Save("SA_Mercenary_Data_Data_level_engineer", level_engineer);
        ES3.Save("SA_Mercenary_Data_Data_level_long_ranged", level_long_ranged);
        ES3.Save("SA_Mercenary_Data_Data_level_short_ranged", level_short_ranged);
        ES3.Save("SA_Mercenary_Data_Data_level_medic", level_medic);
        ES3.Save<Engine_Driver_Type>("SA_Mercenary_Data_engine_driver_type", engine_driver_type);
        ES3.Save("SA_Mercenary_Data_Data_mercenary_buy_num", mercenary_buy_num);
        //데모버전 이후
        ES3.Save<Bard_Type>("SA_Mercenary_Data_bard_type", bard_type);
        ES3.Save("SA_Mercenary_Data_Data_level_bard", level_bard);
        ES3.Save("SA_Mercenary_Data_Data_level_cowboy", level_cowboy);

    }

    public void Load()
    {
        mercenary_num = ES3.Load<List<int>>("SA_Mercenary_Data_Data_mercenary_num");
        engine_driver_type = ES3.Load<Engine_Driver_Type>("SA_Mercenary_Data_engine_driver_type");
        mercenary_buy_num = ES3.Load<List<int>>("SA_Mercenary_Data_Data_mercenary_buy_num");
        level_engine_driver = ES3.Load<int>("SA_Mercenary_Data_Data_level_engine_driver");
        level_engineer = ES3.Load<int>("SA_Mercenary_Data_Data_level_engineer");
        level_long_ranged = ES3.Load<int>("SA_Mercenary_Data_Data_level_long_ranged");
        level_short_ranged = ES3.Load<int>("SA_Mercenary_Data_Data_level_short_ranged");
        level_medic = ES3.Load<int>("SA_Mercenary_Data_Data_level_medic");
        //데모버전 이후
        bard_type = ES3.Load<Bard_Type>("SA_Mercenary_Data_bard_type");
        level_medic = ES3.Load<int>("SA_Mercenary_Data_Data_level_bard");
        level_cowboy = ES3.Load<int>("SA_Mercenary_Data_Data_level_cowboy");
    }

    public void Init()
    {
        mercenary_num.Clear();
        engine_driver_type = Engine_Driver_Type.speed;
        bard_type = Bard_Type.HP_Buff;
        mercenary_buy_num.Clear();
        level_engine_driver = 0;
        level_engineer = 0;
        level_long_ranged = 0;
        level_short_ranged = 0;
        level_medic = 0;
        level_bard = 0;
        level_cowboy = 0;
        Save();
    }
    public IEnumerator InitAsync()
    {
        mercenary_num.Clear();
        engine_driver_type = Engine_Driver_Type.speed;
        bard_type = Bard_Type.HP_Buff;
        mercenary_buy_num.Clear();
        level_engine_driver = 0;
        level_engineer = 0;
        level_long_ranged = 0;
        level_short_ranged = 0;
        level_medic = 0;
        level_bard = 0;
        level_cowboy = 0;
        Save();
        yield return null;
    }
}
