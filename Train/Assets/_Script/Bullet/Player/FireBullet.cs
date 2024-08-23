using MoreMountains.Feel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
        atk -= 10;
        Bullet_Player();
    }

    void Bullet_Player()
    {
        Camera _cam = Camera.main;

        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        dir = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        Destroy(gameObject, 3f);
    }
}
