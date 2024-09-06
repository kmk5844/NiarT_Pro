using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFeel : MonoBehaviour
{
    public MMF_Player player;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player?.PlayFeedbacks();
        }
    }
}
