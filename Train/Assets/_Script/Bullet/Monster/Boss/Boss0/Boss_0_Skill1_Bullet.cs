using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_0_Skill1_Bullet : MonsterBullet
{
    protected override void Start()
    {
        base.Start();
        transform.position = new Vector2(transform.position.x, 4.65f);
    }

    public void BulletAniEnd()
    {
        Destroy(gameObject);
    }
}
