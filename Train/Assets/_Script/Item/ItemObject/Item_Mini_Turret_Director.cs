using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Mini_Turret_Director : MonoBehaviour
{
    public GameObject turret;
    public float delayTime;
    public void Set(int num, float delayTime, int Attack, float AttackDelay)
    {
        if (num == 0)
        {
            Item_Mini_Turret turret_1 = turret.GetComponent<Item_Mini_Turret>();
            turret_1.SpawnTime = delayTime;
            turret_1.Attack = Attack;
            turret_1.AttackDelay = AttackDelay;
        }
        else if (num == 1)
        {
            Item_Flare_Turret turret_2 = turret.GetComponent <Item_Flare_Turret>();
        }
        else if (num == 2)
        {

        }else if(num == 3)
        {

        }else if(num == 4)
        {

        }

    }
}
