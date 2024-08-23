using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile_TurretBullet : Bullet
{
    public Transform monster_target;
    protected override void Start()
    {
        base.Start();

        Bullet_Missile_Turret();
    }

    private void FixedUpdate()
    {
        if (monster_target != null)
        {
            // 타겟 방향으로 회전
            transform.up = (monster_target.position - transform.position).normalized;

            // 타겟 방향으로 이동
            rid.velocity = transform.up * Speed;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Bullet_Missile_Turret()
    {
        Destroy(gameObject, 10f);
    }
}
