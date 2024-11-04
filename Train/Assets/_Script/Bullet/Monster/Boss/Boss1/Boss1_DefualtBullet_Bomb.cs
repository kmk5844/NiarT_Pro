using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_DefualtBullet_Bomb : MonsterBullet
{
    protected override void Start()
    {
        atk = GetComponentInParent<Boss1_DefaultBullet>().atk;
        slow = GetComponentInParent<Boss1_DefaultBullet>().slow;
    }


}