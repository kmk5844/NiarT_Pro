using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_StageList", menuName = "Scriptable/SA_Stage", order = 10)]

public class SA_StageList : ScriptableObject

{
    [SerializeField]
    private List<StageDataObject> stage;
    public List<StageDataObject> Stage { get { return stage; } }

    public void Editor_StageList_Init()
    {
        stage.Clear();
    }

    public void PlayGame_StageList_Init()
    {
        for(int i = 0; i < stage.Count; i++)
        {
            stage[i].Init();
        }
    }

    public void StageList_InsterObject(StageDataObject newobject)
    {
        stage.Add(newobject);
    }
}