using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet_Bomb : MonsterBullet
{
    GameObject BombParticle;
    // Start is called before the first frame update
    protected override void Start()
    {
        BombParticle = Resources.Load<GameObject>("Bullet/10_Effect");
        base.Start();
    }

    public void BombDestory()
    {
        Instantiate(BombParticle, transform.localPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}