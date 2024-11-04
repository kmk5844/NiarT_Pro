using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_BombObject : MonoBehaviour
{
    Monster_ShortAtk shortAtk_Information;
    public bool xPos_L;
    public GameObject BulletObject;

    public void BulletFire()
    {
        Instantiate(BulletObject);
    }

    void AniEnd()
    {
        Destroy(gameObject);
    }
}