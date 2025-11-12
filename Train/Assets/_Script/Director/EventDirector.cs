using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDirector : MonoBehaviour
{
    public GameDirector gamedirector;
    public Player player;
    public SA_Event SA_Event_;
    public GameObject Dron;
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

            if (SA_Event_.OldTranningFlag)
            {
                num = SA_Event_.OldTranning_Num;
                OldTranningStart();
            }

            if (SA_Event_.SupplyStationFlag)
            {
                SupplyStationStart();
            }
        }
    }

    void OasisStart()
    {
        player.OasisPlayerSetting(num);
    }

    void StormStart()
    {
        gamedirector.StormDebuff();
    }

    void OldTranningStart()
    {
        player.OldTranningSetting(num);
    }

    void SupplyStationStart()
    {
        StartCoroutine(UseDron());
    }

    IEnumerator UseDron()
    {
        yield return new WaitForSeconds(5f);
        Dron.GetComponentInChildren<Supply_Train_Dron>().SupplyDron_SetData(SA_Event_.SupplyStation_Grade, SA_Event_.SupplyStation_Count, false);
        Instantiate(Dron);
        SA_Event_.SupplyStationOff();
    }
}