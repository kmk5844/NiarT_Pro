using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Tutorial_Bullet : MonoBehaviour
{
    Rigidbody2D rid;
    Vector2 dir;
    float Speed;
    // Start is called before the first frame update
    void Start()
    {
        rid = GetComponent<Rigidbody2D>();
        dir = new Vector3(0, -1, 0);
        Speed = 4f;
        BulletFire();
    }

    void BulletFire()
    {
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }
}
