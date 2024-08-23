using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto_TurretBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
        Bullet_Auto_Turret();
    }

    void Bullet_Auto_Turret()
    {
        rid.velocity = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z) * Vector2.right * Speed;
        Destroy(gameObject, 3f);
    }
}
