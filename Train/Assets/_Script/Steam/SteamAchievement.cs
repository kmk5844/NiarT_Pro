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
        // �̱��� �ʱ�ȭ
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ� �ı�
        }
    }

    public void Achieve(string apiName)
    {
        if (SteamAPI.Init())
        {
            SteamUserStats.SetAchievement(apiName);
            SteamUserStats.StoreStats();  // ������ ����
            Debug.Log($"���� '{apiName}' ���� ����!");
        }
        else
        {
            Debug.LogError("Steam API�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
        }
    }
}