using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class SteamAchievement : MonoBehaviour
{
    public static SteamAchievement instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 초기화
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 파괴
        }
    }

    public void Achieve(string apiName)
    {
        if (SteamAPI.Init())
        {
            Steamworks.SteamUserStats.GetAchievement(apiName, out bool isAchieved);

            if (!isAchieved)
            {
                SteamUserStats.SetAchievement(apiName);
                SteamUserStats.StoreStats();
            }
        }
        else
        {
            Debug.LogError("Steam API가 초기화되지 않았습니다.");
        }
    }
}