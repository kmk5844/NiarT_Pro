using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillDirector : MonoBehaviour
{
    public MMF_Player[] fill;

    public void PlayFill(int i)
    {
        fill[i]?.PlayFeedbacks();
    }
}