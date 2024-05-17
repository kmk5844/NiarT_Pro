using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_StoryData", menuName = "Scriptable/StoryData", order = 5)]
public class SA_StoryData : ScriptableObject
{
    [SerializeField]
    private int final_story_index;
    public int Final_Story_Index { get { return final_story_index; } }

    public void Clear_Story()
    {
        final_story_index++;
    }

    public void Start_Story(int PlayerData)
    {
        switch (PlayerData)
        {
            case -1:
                LoadingManager.LoadScene("Station");
                break;
            case 0:
                LoadingManager.LoadScene("Story");
                break;
            case 1:
                LoadingManager.LoadScene("Story");
                break;
            case 2:
                LoadingManager.LoadScene("Story");
                break;

        }
    }

    public void End_Story(int PlayerData)
    {
        switch (PlayerData)
        {
            case -1:
                LoadingManager.LoadScene("Station");
                break;
            case 0:
                LoadingManager.LoadScene("CharacterSelect");
                break;
            case 1:
                LoadingManager.LoadScene("InGame");
                break;
            case 2:
                LoadingManager.LoadScene("Station");
                break;

        }

    }
}