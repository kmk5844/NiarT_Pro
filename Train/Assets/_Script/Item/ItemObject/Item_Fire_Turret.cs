using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Fire_Turret : MonoBehaviour
{
    public GameObject Turret;
    public Fire_TurretBullet Fire;
    public int atk;
    public float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        Fire.atk = atk;
        Destroy(Turret, spawnTime);
    }
}
