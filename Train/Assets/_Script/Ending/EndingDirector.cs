using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingDirector : MonoBehaviour
{
    void Start()
    {
        if (SteamAchievement.instance != null)
        {
            SteamAchievement.instance.Achieve("EARLY_END");
        }
        else
        {
            Debug.Log("EARLY_END");
        }

    }

    public void EndEvent()
    {
        try
        {
            LoadingManager.LoadScene("1.MainMenu");
        }
        catch
        {
            SceneManager.LoadScene("1.MainMenu");
        }
    }
}
