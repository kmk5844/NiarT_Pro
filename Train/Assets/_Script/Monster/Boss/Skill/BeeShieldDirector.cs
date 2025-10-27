using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeShieldDirector : MonoBehaviour
{
    public Monster_Boss_3 Boss;
    public GameObject[] Bee;
    bool[] BeeFlag;

    // Start is called before the first frame update
    void Start()
    {
        BeeFlag = new bool[Bee.Length];
        InitBee();
    }

    public void DestroyBee(int num)
    {
        Bee[num].SetActive(false);
        BeeFlag[num] = false;
        CheckBee();
    }

    void CheckBee()
    {
        for (int i = 0; i < BeeFlag.Length; i++)
        {
            if (BeeFlag[i] == true)
            {
                return;
            }
        }
        Boss.DestroyBeeShiled();
    }

    void InitBee()
    {
        for (int i = 0; i < Bee.Length; i++)
        {
            Bee[i].SetActive(true);
            BeeFlag[i] = true;
        }
    }
}
