using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare_Bomb : MonoBehaviour
{
    Flare_TurretBullet Bullet;
    public ParticleSystem BombEffect;
    bool bomb_flag;
    void Start()
    {
        Bullet = GetComponentInParent<Flare_TurretBullet>();
        bomb_flag = false;
    }

    private void Update()
    {
        if (BombEffect.isStopped)
        {
            Destroy(Bullet.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);
        if (!bomb_flag)
        {
            foreach(Collider2D collider in colliders)
            {
                if (collider.CompareTag("Monster_Bullet"))
                {
                    Destroy(collider.gameObject);
                }
            }
            bomb_flag = true;
        }
    }
}
