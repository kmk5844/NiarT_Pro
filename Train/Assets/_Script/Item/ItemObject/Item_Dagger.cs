using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Dagger : Bullet
{
    protected override void Start()
    {
        base.Start();

        rid.velocity = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z) * Vector2.right * Speed;
        Destroy(gameObject, 3f);    
    }
}
