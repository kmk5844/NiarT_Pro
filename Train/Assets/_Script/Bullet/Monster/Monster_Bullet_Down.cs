using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet_Down : MonsterBullet
{
    public bool X_Flag;
    [SerializeField]
    public float X_Dir;

    protected override void Start()
    {
        base.Start();
        if (!X_Flag)
        {
            X_Dir = 0;
        }
        Bullet_Fire();
    }

    void Bullet_Fire()
    {
        dir = new Vector3(X_Dir, -1, 0);
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }
}
