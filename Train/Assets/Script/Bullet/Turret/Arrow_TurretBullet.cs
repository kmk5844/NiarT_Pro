using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_TurretBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
        Bullet_Arrow_Turret();
    }

    private void Update()
    {
        Vector2 gravity = Vector2.down * 9.81f;
        Vector2 velocity = rid.velocity;
        velocity += gravity * Time.deltaTime;
        rid.velocity = velocity;
        transform.right = rid.velocity;
    }

    void Bullet_Arrow_Turret()
    {
        rid.velocity = transform.right * Speed;
        Destroy(gameObject, 3f);
    }
}
