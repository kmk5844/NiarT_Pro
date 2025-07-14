using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Mini_Turret_Director : MonoBehaviour
{
    public Item_Mini_Turret turret;
    public float delayTime;
    public void Set(float delayTime, int Attack, float AttackDelay)
    {
        turret.SpawnTime = delayTime;
        turret.Attack = Attack;
        turret.AttackDelay = AttackDelay;   
    }
}
