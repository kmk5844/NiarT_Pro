using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDirector : MonoBehaviour
{

    // Start is called before the first frame update
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
}
