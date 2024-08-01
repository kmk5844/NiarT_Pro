using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_0_Skill3_SubBullet : MonoBehaviour
{
    Rigidbody2D rid;
    Vector2 dir;
    float Speed;

    private void Start()
    {
        rid = GetComponent<Rigidbody2D>();
        Speed = 12f;
        Bullet_Fire();
    }

    void Bullet_Fire()
    {
        float rotationAngle = transform.eulerAngles.z;
        Quaternion rotation = Quaternion.Euler(0, 0, rotationAngle);
        dir = rotation * Vector2.right;
        rid.velocity = dir.normalized * Speed;
        Destroy(gameObject, 5f);
    }
}
