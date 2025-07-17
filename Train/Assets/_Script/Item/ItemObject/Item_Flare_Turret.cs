using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Flare_Turret : MonoBehaviour
{
    public GameObject Turret;
    public Transform BulletObject;
    public float SpawnTime;
    float lastTime;
    float Attack_Delay;

    private void Start()
    {
        lastTime = 0;
        Attack_Delay = 2f;
        Destroy(Turret, SpawnTime );
    }

    private void Update()
    {
        BulletFire();
    }

    void BulletFire()
    {
        if (Time.time >= lastTime + Attack_Delay)
        {
            Vector2 Flare_Positinon = new Vector2(transform.position.x + 0.2f,transform.position.y - 0.2f);
            Instantiate(BulletObject, Flare_Positinon, transform.rotation);
            lastTime = Time.time;
        }
    }
}
