using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamAuthManager : MonoBehaviour
{
    public static SteamAuthManager Instance { get; private set; }
    public bool IsSignedIn { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
