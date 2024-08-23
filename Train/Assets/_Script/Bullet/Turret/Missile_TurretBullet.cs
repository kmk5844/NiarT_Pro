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
            // Ÿ�� �������� ȸ��
            transform.up = (monster_target.position - transform.position).normalized;

            // Ÿ�� �������� �̵�
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
