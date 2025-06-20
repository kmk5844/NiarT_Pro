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

    public int Select_Dic_Story_Num;

    public void Editor_StoryList_Init()
    {
        storylist.Clear();
    }

    public void PlayGame_StoryList_Init()
    {
        for (int i = 0; i < storylist.Count; i++)
        {
            if (storylist[i].StoryDataUse)
            {
                storylist[i].Init();
            }
            else
            {
                break;
            }
        }
    }

    public IEnumerator PlayGame_StoryList_InitAsync(MonoBehaviour runner)
    {
        for(int i = 0; i < storylist.Count; i++)
        {
            if (storylist[i].StoryDataUse)
            {
                 storylist[i].InitSync(runner);
            }
            else
            {
                break;
            }

            if(i % 5 == 0)
            {
                yield return new WaitForSeconds(0.01f);
            }
        }

        yield return null;
    }

    public void PlayGame_StoryList_Load()
    {
        for (int i = 0; i < storylist.Count; i++)
        {
            if (storylist[i].StoryDataUse)
            {
                storylist[i].Load();
            }
            else
            {
                break;
            }
        }
    }


    public IEnumerator PlayGame_StoryList_LoadSync(MonoBehaviour runner)
    {
        for (int i = 0; i < storylist.Count; i++)
        {
            if(!ES3.KeyExists("Story_" + i + "_StoryDataUse"))
            {
                break;
            }

            storylist[i].Load_DataUse();
            if (storylist[i].StoryDataUse)
            {
                storylist[i].LoadSync_Start(runner);
            }
            else
            {
                break;
            }
            yield return null;

        }

        yield return null;
    }

    public void StoryList_InsertObject(StoryDataObject newobject)
    {
        storylist.Add(newobject);
    }
}
