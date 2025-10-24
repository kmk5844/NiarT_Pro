using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bomb_Bomb : MonsterBullet
{
    protected override void Start()
    {
        MonsterBullet parentBullet = transform.parent.GetComponent<MonsterBullet>();
        if (parentBullet != null)
        {
            atk = parentBullet.atk;
            slow = parentBullet.slow;
        }
    }
}