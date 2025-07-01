using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_EventData", menuName = "Scriptable/SA_EventData", order = 13)]
public class SA_Event : ScriptableObject
{
    [SerializeField]
    private bool eventflag;
    public bool EventFlag { get { return eventflag; } }

    [SerializeField]
    private bool foodflag;
    public bool FoodFlag { get { return foodflag; } }

    [SerializeField]
    private bool food_heal_flag;
    public bool Food_Heal_Flag { get { return food_heal_flag; } }

    [SerializeField]
    private int food_num;
    public int Food_Num { get { return food_num; } }
    //------------------------------------------------------------------------------

    [SerializeField]
    private bool oasisflag;
    public bool OasisFlag { get {return oasisflag; } }

    [SerializeField]
    private int oasis_num;
    public int Oasis_Num { get { return oasis_num; } }

    //--------------------------------------------------------------------------------

    [SerializeField]
    private bool stormflag;
    public bool StormFlag { get { return stormflag; } }

    [SerializeField]
    public void SA_ChoiceFood(int num)
    {
        eventflag = true;
        foodflag = true;
        food_num = num;
        Save("Food");
    }

    public void SA_HealFood()
    {
        food_heal_flag = true;
        Save("Food");
    }

    public void SA_EventFlag_Off()
    {
        eventflag = false;
        foodflag = false;
        food_heal_flag = false;
        Save("Food");
    }

    public void Choice_Oasis(int num)
    {
        eventflag = true;
        oasisflag = true;
        oasis_num = num;
        Save("Oasis");
    }

    public void OasisOff()
    {
        eventflag = false;
        oasisflag = false;
        Save("Oasis");
    }

    public void StormDebuff()
    {
        eventflag = true;
        stormflag = true;
        Save("Storm");
    }

    public void StormOff()
    {
        eventflag = false;
        stormflag = false;
        Save("Storm");
    }


    public void Save(string _string)
    {
        ES3.Save<bool>("SA_EventData_Data_EventFlag", eventflag);
        switch (_string)
        {
            case "Food":
        ES3.Save<bool>("SA_EventData_Data_Food_Heal_Flag", food_heal_flag);
        ES3.Save<int>("SA_EventData_Data_Food_Num", food_num);
                break;
            case "Oasis":
        ES3.Save<bool>("SA_EventData_Data_Oasis_Flag", oasisflag);
        ES3.Save<int>("SA_EventData_Data_Oasis_Num", oasis_num);
                break;
            case "Storm":
        ES3.Save<bool>("SA_EventData_Data_Storm_Flag", stormflag);
                break;
        }
    }

    public void Save()
    {
        ES3.Save<bool>("SA_EventData_Data_EventFlag", eventflag);
        ES3.Save<bool>("SA_EventData_Data_Food_Heal_Flag", food_heal_flag);
        ES3.Save<int>("SA_EventData_Data_Food_Num", food_num);
        ES3.Save<bool>("SA_EventData_Data_Oasis_Flag", oasisflag);
        ES3.Save<int>("SA_EventData_Data_Oasis_Num", oasis_num);
        ES3.Save<bool>("SA_EventData_Data_Storm_Flag", stormflag);
    }

    public IEnumerator SaveSync()
    {
        ES3.Save<bool>("SA_EventData_Data_EventFlag", eventflag);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<bool>("SA_EventData_Data_Food_Heal_Flag", food_heal_flag);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_EventData_Data_Food_Num", food_num);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<bool>("SA_EventData_Data_Oasis_Flag", oasisflag);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_EventData_Data_Oasis_Num", oasis_num);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<bool>("SA_EventData_Data_Storm_Flag", stormflag);
        yield return new WaitForSeconds(0.001f);


    }

    public void Load()
    {
        eventflag = ES3.Load<bool>("SA_EventData_Data_EventFlag", false);
        food_heal_flag = ES3.Load<bool>("SA_EventData_Data_Food_Heal_Flag", false);
        food_num = ES3.Load<int>("SA_EventData_Data_Food_Num", 0);
        oasisflag = ES3.Load<bool>("SA_EventData_Data_Oasis_Flag", false);
        oasis_num = ES3.Load<int>("SA_EventData_Data_Oasis_Num", 0);
        stormflag = ES3.Load<bool>("SA_EventData_Data_Storm_Flag", false);
    }


    public IEnumerator LoadSync()
    {
        eventflag = ES3.Load<bool>("SA_EventData_Data_EventFlag", false);
        yield return new WaitForSeconds(0.001f);
        food_heal_flag = ES3.Load<bool>("SA_EventData_Data_Food_Heal_Flag", false);
        yield return new WaitForSeconds(0.001f);
        food_num = ES3.Load<int>("SA_EventData_Data_Food_Num", 0);
        yield return new WaitForSeconds(0.001f);
        oasisflag = ES3.Load<bool>("SA_EventData_Data_Oasis_Flag", false);
        yield return new WaitForSeconds(0.001f);
        oasis_num = ES3.Load<int>("SA_EventData_Data_Oasis_Num", 0);
        yield return new WaitForSeconds(0.001f);
        stormflag = ES3.Load<bool>("SA_EventData_Data_Storm_Flag", false);
        yield return new WaitForSeconds(0.001f);
    }

    public void Init()
    {
        eventflag = false;
        food_heal_flag = false;
        food_num = 0;
        oasisflag = false;
        oasis_num = 0;
        stormflag = false;
        Save();
    }

    public IEnumerator InitAsync()
    {
        Init();
        yield return null;
    }
}
