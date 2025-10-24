using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bomb_Monster : MonsterBullet
{
    protected override void Start()
    {
        atk = GetComponentInParent<Monster>().Bullet_Atk;
        slow = GetComponentInParent<Monster>().Bullet_Slow;
    }
}