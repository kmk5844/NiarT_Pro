using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet_LR_Streight : MonsterBullet
{
    public bool flag = false;
    public SpriteRenderer spriteRenderer;
    public bool Bossflag = false;
    protected override void Start()
    {
        base.Start();
        Bullet_Fire();
    }

    private void Update()
    {
        if(transform.position.x > MonsterDirector.MaxPos_Ground.x -1f || transform.position.x < MonsterDirector.MinPos_Ground.x + 1f)
        {
            Destroy(gameObject);
        }
    }


    void Bullet_Fire()
    {
        if (x_scale == 1)
        {
            dir = new Vector3(1, 0, 0);
            if (flag)
            {
                spriteRenderer.flipY = false;
            }
        }
        else if (x_scale == -1)
        {
            dir = new Vector3(-1, 0, 0);
            if (flag)
            {
                spriteRenderer.flipY = true;
            }
        }
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }
}
