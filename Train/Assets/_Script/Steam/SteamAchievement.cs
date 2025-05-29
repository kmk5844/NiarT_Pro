using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.SocialPlatforms.Impl;
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
            SteamUserStats.SetAchievement(apiName);
            SteamUserStats.StoreStats();  // 서버에 저장
            Debug.Log($"업적 '{apiName}' 해제 성공!");
        }
        else
        {
            Debug.LogError("Steam API가 초기화되지 않았습니다.");
        }
    }
}