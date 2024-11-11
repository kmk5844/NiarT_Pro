using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_StoryList", menuName = "Scriptable/SA_Story", order = 11)]
public class SA_StoryLIst : ScriptableObject
{
    [SerializeField]
    private int story_num;
    public int Story_Num { get { return story_num; } }

    [SerializeField]
    private List<StoryDataObject> storylist;
    public List<StoryDataObject>StoryList {  get { return storylist; } }

    public void Editor_StoryList_Init()
    {
        storylist.Clear();
    }

    public void PlayGame_StoryList_Init()
    {
        for(int i = 0; i < storylist.Count; i++)
        {
            storylist[i].Init();
        }
    }

    public void PlayGame_StoryList_Load()
    {
        for (int i = 0; i < storylist.Count; i++)
        {
            storylist[i].Load();
        }
    }

    public void StoryList_InsertObject(StoryDataObject newobject)
    {
        storylist.Add(newobject);
    }
}
