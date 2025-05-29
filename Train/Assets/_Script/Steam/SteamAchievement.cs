using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
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
            Steamworks.SteamUserStats.GetAchievement(apiName, out bool isAchieved);

            if (!isAchieved)
            {
                SteamUserStats.SetAchievement(apiName);
                SteamUserStats.StoreStats();
            }
        }
        else
        {
            Debug.LogError("Steam API�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
        }
    }
}