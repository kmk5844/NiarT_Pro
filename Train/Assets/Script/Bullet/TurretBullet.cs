using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
        Bullet_Turret();
    }
}
