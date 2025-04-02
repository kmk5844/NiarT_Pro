using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_TutorialData", menuName = "Scriptable/SA_Tutorial", order = 11)]
public class SA_Tutorial : ScriptableObject
{
    [SerializeField]
    private bool tu_station;
    public bool Tu_Station { get {  return tu_station; } }

    [SerializeField]
    private bool tu_maintenance;
    public bool Tu_Maintenance { get { return tu_maintenance; } }  

    [SerializeField]
    private bool tu_store;
    public bool Tu_Store {  get { return tu_store; } }

    [SerializeField]
    private bool tu_traning;
    public bool Tu_Traning { get { return tu_traning; } }


    [SerializeField]
    private bool tu_mapselect;
    public bool Tu_MapSelect { get {  return tu_mapselect; } }

    [SerializeField]
    private bool tu_translate;
    public bool Tu_Translate { get { return tu_translate; } }

    public void ChangeFlag(int i)
    {
        if (i == 0)
        {
            tu_station = true;
        }
        else if (i == 1)
        {
            tu_maintenance = true;
        } else if (i == 2)
        {
            tu_store = true;
        } else if (i == 3)
        {
            tu_traning = true;
        }else if(i == 4)
        {
            tu_mapselect = true;
        }else if(i == 5)
        {
            tu_translate = true;
        }
        Save();
    }


    public void Init()
    {
        tu_station = false;
        tu_maintenance = false;
        tu_store = false;
        tu_traning = false;
        tu_mapselect = false;
        tu_translate = false;
        Save();
    }

    public IEnumerator InitAsync()
    {
        tu_station = false;
        tu_maintenance = false;
        tu_store = false;
        tu_traning = false;
        tu_mapselect = false;
        tu_translate = false;
        Save();
        yield return null;
    }


    public void Save()
    {
        ES3.Save("tu_station_flag", tu_station);
        ES3.Save("tu_maintenance_flag", tu_maintenance);
        ES3.Save("tu_store_flag", tu_store);
        ES3.Save("tu_traning_flag", tu_traning);
        ES3.Save("tu_mapselect_flag", tu_mapselect);
        ES3.Save("tu_translate_flag", tu_translate);
    }

    public void Load()
    {
        tu_station = ES3.Load<bool>("tu_station_flag");
        tu_maintenance = ES3.Load<bool>("tu_maintenance_flag");
        tu_store = ES3.Load<bool>("tu_store_flag");
        tu_traning = ES3.Load<bool>("tu_traning_flag");
        tu_mapselect = ES3.Load<bool>("tu_mapselect_flag");
        tu_translate = ES3.Load<bool>("tu_translate_flag");
    }
}
