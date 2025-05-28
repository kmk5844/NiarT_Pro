using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_End : MonoBehaviour
{
    private void Start()
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadingManager.LoadScene("1.MainMenu");    
        }
    }
}