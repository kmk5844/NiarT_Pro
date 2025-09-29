using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Mini_Turret_Director : MonoBehaviour
{
    public GameObject turret;
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
            Item_Rope_Turret turret_2 = turret.GetComponent<Item_Rope_Turret>();
            turret_2.SpawnTime = delayTime;
        }
        else if (num == 2)
        {
            Item_Fire_Turret turret_3 = turret.GetComponent<Item_Fire_Turret>();
            turret_3.spawnTime = delayTime;
            turret_3.atk = Attack;
        }
        else if (num == 3)
        {
            Item_Flare_Turret turret_4 = turret.GetComponent<Item_Flare_Turret>();
            turret_4.SpawnTime = delayTime;

        }
        else if (num == 4)
        {
            Item_Missile_Turret turret_5 = turret.GetComponent<Item_Missile_Turret>();
            turret_5.Attack = Attack;
            turret_5.SpawnTime = delayTime;
            turret_5.Attack_Delay = AttackDelay;
        }
        else if (num == 5)
        {
            Item_Missile_Turret turret_5 = turret.GetComponent<Item_Missile_Turret>();
            turret_5.Attack = Attack;
            turret_5.SpawnTime = delayTime;
            turret_5.Attack_Delay = AttackDelay;
        }
        else if (num == 6)
        {
            Item_Laser_Turret turret_6 = turret.GetComponent<Item_Laser_Turret>();
            turret_6.attack = Attack;
            if (AttackDelay == 0)
            {
                turret_6.laserType = false;
                turret_6.groundType = false;
            }
            else
            {
                turret_6.laserType = true;
                turret_6.groundType = false;
            }
        }
        else if (num == 7)
        {
            Item_Laser_Turret turret_6 = turret.GetComponent<Item_Laser_Turret>();
            turret_6.attack = Attack;
            turret_6.SpawnDelay = delayTime;
            turret_6.groundType = true;
        }
    }
}
