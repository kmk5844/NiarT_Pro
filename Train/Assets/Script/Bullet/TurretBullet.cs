using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
        Bullet_Turret();
    }

    void Bullet_Turret()
    {
        rid.velocity = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z) * Vector2.right * Speed;
        Destroy(gameObject, 3f);
    }
}
