using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDirector : MonoBehaviour
{
    public GameDirector gamedirector;
    public Player player;
    public SA_Event SA_Event_;


    int num = 0;

    public void CheckEvnet()
    {
        if (SA_Event_.EventFlag)
        {
            if (SA_Event_.OasisFlag)
            {
                num = SA_Event_.Oasis_Num;
                OasisStart();
            }

            if (SA_Event_.StormFlag)
            {
                StormStart();
            }
        }
    }

    public void OasisStart()
    {
        player.OasisPlayerSetting(num);
    }

    public void StormStart()
    {
        gamedirector.StormDebuff();
    }
}