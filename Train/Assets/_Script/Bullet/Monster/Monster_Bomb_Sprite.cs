using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bomb_Sprite : MonsterBullet
{
    protected override void Start()
    {
        atk = GetComponentInParent<Monster_Bullet_Angle>().atk;
        slow = GetComponentInParent<Monster_Bullet_Angle>().slow;
    }
}