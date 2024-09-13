using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet_LR_Streight : MonsterBullet
{
    protected override void Start()
    {
        base.Start();
        Bullet_Fire();
    }

    void Bullet_Fire()
    {
        if (x_scale == 1)
        {
            dir = new Vector3(1, 0, 0);
        }
        else if (x_scale == -1)
        {
            dir = new Vector3(-1, 0, 0);
        }
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }
 
}
