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
    public void SA_ChoiceFood(int num)
    {
        eventflag = true;
        foodflag = true;
        food_num = num;
        Save();
    }

    public void SA_HealFood()
    {
        food_heal_flag = true;
        Save();
    }

    public void SA_EventFlag_Off()
    {
        eventflag = false;
        foodflag = false;
        food_heal_flag = false;
        Save();
    }

    public void Choice_Oasis(int num)
    {
        oasisflag = true;
        oasis_num = num;
        Save();
    }

    public void OasisOff()
    {
        eventflag = false;
        oasisflag = false;
        Save();
    }

    public void Save()
    {
        ES3.Save<bool>("SA_EventData_Data_EventFlag", eventflag);
        ES3.Save<bool>("SA_EventData_Data_Food_Heal_Flag", food_heal_flag);
        ES3.Save<int>("SA_EventData_Data_Food_Num", food_num);
        ES3.Save<int>("SA_EventData_Data_Oasis_Num", oasis_num);
    }

    public IEnumerator SaveSync()
    {
        ES3.Save<bool>("SA_EventData_Data_EventFlag", eventflag);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<bool>("SA_EventData_Data_Food_Heal_Flag", food_heal_flag);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_EventData_Data_Food_Num", food_num);
        yield return new WaitForSeconds(0.001f);
        ES3.Save<int>("SA_EventData_Data_Oasis_Num", oasis_num);
        yield return new WaitForSeconds(0.001f);

    }

    public void Load()
    {
        eventflag = ES3.Load<bool>("SA_EventData_Data_EventFlag", false);
        food_heal_flag = ES3.Load<bool>("SA_EventData_Data_Food_Heal_Flag", false);
        food_num = ES3.Load<int>("SA_EventData_Data_Food_Num", 0);
        oasis_num = ES3.Load<int>("SA_EventData_Data_Oasis_Num", 0);
    }

    public void Init()
    {
        eventflag = false;
        food_heal_flag = false;
        food_num = 0;
        oasis_num = 0;
        Save();
    }
}
