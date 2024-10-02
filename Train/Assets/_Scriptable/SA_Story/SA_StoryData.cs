using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            case 3:
                LoadingManager.LoadScene("Story");
                break;
            case 4:
                LoadingManager.LoadScene("Story");
                break;
            case 5:
                LoadingManager.LoadScene("Story");
                break;
            case 7:
                LoadingManager.LoadScene("Story");
                break;
            case 8:
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
                LoadingManager.LoadScene("Demo_Tutorial");
                break;
            case 1:
                LoadingManager.LoadScene("InGame");
                break;
            case 2:
                LoadingManager.LoadScene("Demo_Tutorial");
                break;
            case 3:
                LoadingManager.LoadScene("Station");
                break;
            case 4:
                LoadingManager.LoadScene("Station");
                break;
            case 5:
                LoadingManager.LoadScene("Station");
                break;
            case 7:
                LoadingManager.LoadScene("Station");
                break;
            case 8:
                LoadingManager.LoadScene("Demo_End");
                break;
        }
    }
    
    public void End_Tutorial(int PlayerData)
    {
        switch (PlayerData)
        {
            case 0:
                SceneManager.LoadSceneAsync("InGame");
                //LoadingManager.LoadScene("InGame");
                break;
            case 2:
                SceneManager.LoadSceneAsync("Station");
                //LoadingManager.LoadScene("Station");
                break;
        }
    }

    public void End_Demo(int PlayerData)
    {
        if (PlayerData == 8)
        {
            LoadingManager.LoadScene("Demo_End");
        }
    }
}