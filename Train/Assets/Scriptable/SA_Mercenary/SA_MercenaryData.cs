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
    public Engine_Driver_Type Engine_Driver_Type { get { return engine_driver_type; } }

    [SerializeField]
    private List<int> mercenary_buy_num;
    public List<int> Mercenary_Buy_Num { get { return mercenary_buy_num; } }

    [SerializeField]
    private Sprite[] mercenary_head_image;
    public Sprite[] Mercenary_Head_Image {  get {  return mercenary_head_image;} }
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
        Save();
    }

    public void SA_Mercenary_Num_Plus(int i)
    {
        mercenary_num.Add(i);
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
        }
    }
    private void Save()
    {
        ES3.Save("SA_Mercenary_Data_Data_mercenary_num", mercenary_num);
        PlayerPrefs.SetInt("SA_Mercenary_Data_Data_level_engine_driver", level_engine_driver);
        PlayerPrefs.SetInt("SA_Mercenary_Data_Data_level_engineer", level_engineer);
        PlayerPrefs.SetInt("SA_Mercenary_Data_Data_level_long_ranged", level_long_ranged);
        PlayerPrefs.SetInt("SA_Mercenary_Data_Data_level_short_ranged", level_short_ranged);
        PlayerPrefs.SetInt("SA_Mercenary_Data_Data_level_medic", level_medic);
        ES3.Save<Engine_Driver_Type>("SA_Mercenary_Data_engine_driver_type", engine_driver_type);
        ES3.Save("SA_Mercenary_Data_Data_mercenary_buy_num", mercenary_buy_num);
    }

    public void Load()
    {
        mercenary_num = ES3.Load<List<int>>("SA_Mercenary_Data_Data_mercenary_num");
        engine_driver_type = ES3.Load<Engine_Driver_Type>("SA_Mercenary_Data_engine_driver_type");
        mercenary_buy_num = ES3.Load<List<int>>("SA_Mercenary_Data_Data_mercenary_buy_num");
        level_engine_driver = PlayerPrefs.GetInt("SA_Mercenary_Data_Data_level_engine_driver");
        level_engineer = PlayerPrefs.GetInt("SA_Mercenary_Data_Data_level_engineer");
        level_long_ranged = PlayerPrefs.GetInt("SA_Mercenary_Data_Data_level_long_ranged");
        level_short_ranged = PlayerPrefs.GetInt("SA_Mercenary_Data_Data_level_short_ranged");
        level_medic = PlayerPrefs.GetInt("SA_Mercenary_Data_Data_level_medic");
    }

    public void Init()
    {
        mercenary_num.Clear();
        engine_driver_type = Engine_Driver_Type.speed;
        mercenary_buy_num.Clear();
        level_engine_driver = 0;
        level_engineer = 0;
        level_long_ranged = 0;
        level_short_ranged = 0;
        level_medic = 0;
        Save();
    }
}
