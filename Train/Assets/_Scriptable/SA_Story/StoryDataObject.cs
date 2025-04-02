using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDataObject : ScriptableObject
{
    [SerializeField]
    private bool storydatause;
    public bool StoryDataUse { get { return storydatause; } }

    [SerializeField]
    private int story_num;
    public int Story_Num {  get { return story_num; } }

    [SerializeField]
    private int branch_index;
    public int Branch_Index { get { return branch_index; } }

    [SerializeField]
    private string background;
    public string Background { get { return background; } }

    [SerializeField]
    private string story_end;
    public string Story_End { get {  return story_end; } }

    [SerializeField]
    private string story_title_kr;
    public string Story_Title_Kr { get { return story_title_kr; } }
    [SerializeField]
    private string story_title_en;
    public string Story_Title_En { get { return story_title_en; } }
    [SerializeField]
    private string story_title_jp;
    public string Story_Title_Jp { get { return story_title_jp; } }

    [SerializeField]
    private bool start_flag;
    public bool Start_Flag { get { return start_flag; } }

    [SerializeField]
    private bool end_flag;
    public bool End_Flag { get { return end_flag; } }

    public void ChangeFlag(bool startAndend)
    {
        if (startAndend)
        {
            start_flag = true;
        }
        else
        {
            end_flag = true;
        }
        Save();
    }

    public void Auto_StoryData_Insert(int _story_num, int _branch_index, string _background, string _story_end, string _story_title_kr, string _story_title_en, string _story_title_jp)
    {
        story_num = _story_num;
        branch_index = _branch_index;
        background = _background;
        story_end = _story_end;
        story_title_kr = _story_title_kr;
        story_title_en = _story_title_en;
        story_title_jp = _story_title_jp;

        start_flag = false;
        end_flag = false;
        Save(true);
    }

    public void Init()
    {
        start_flag = false;
        end_flag = false;
        Save(true);
    }

    public void Save(bool Init = false)
    {
        if(Init == true)
        {
            storydatause = false;
        }
        else
        {
            storydatause = true;
        }
        ES3.Save<bool>("Story_" + story_num + "_StoryDataUse", storydatause);
        ES3.Save<bool>("Story_" + story_num + "_Start_Flag", start_flag);
        ES3.Save<bool>("Story_" + story_num + "_End_Flag", end_flag);
    }

    public void Load()
    {
        storydatause = ES3.Load<bool>("Story_" + story_num + "_StoryDataUse");
        start_flag = ES3.Load<bool>("Story_" + story_num + "_Start_Flag");
        end_flag = ES3.Load<bool>("Story_" + story_num + "_End_Flag");
    }
}