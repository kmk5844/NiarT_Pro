using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class SteamAchievement : MonoBehaviour
{
    public static SteamAchievement instance;

    private void Awake()
    {
        instance = this;
    }

    public void Achieve(string apiName)
    {
        if (SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetAchievement(apiName, out bool isAchieved);

            if (!isAchieved)
            {
                SteamUserStats.SetAchievement(apiName);
                SteamUserStats.StoreStats();
            }
        }
    }
}