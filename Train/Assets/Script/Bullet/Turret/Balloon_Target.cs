using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon_Target : MonoBehaviour
{
    Balloon_TurretBullet bullet;

    private void Start()
    {
        bullet = GetComponentInParent<Balloon_TurretBullet>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            bullet.monster_target = collision.transform;
        }
    }
}
