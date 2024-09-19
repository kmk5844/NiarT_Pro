using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet_None : MonsterBullet
{
    protected override void Start()
    {
        base.Start();
        Bullet_Fire();
    }

    void Bullet_Fire()
    {
        float Rx = Random.Range(player_target.position.x - 10, player_target.position.x + 10);
        dir = (new Vector3(Rx, -1, 0) - transform.position).normalized;
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }
}
