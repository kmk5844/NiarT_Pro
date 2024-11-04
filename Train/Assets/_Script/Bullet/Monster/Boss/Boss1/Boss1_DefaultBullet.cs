using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_DefaultBullet : MonsterBullet
{
    GameObject BombParticle;
    float force;
    protected override void Start()
    {
        BombParticle = Resources.Load<GameObject>("Bullet/10_Effect");
        base.Start();
        force = Random.Range(5f, 7f);
        rid.AddForce(Vector2.one * force, ForceMode2D.Impulse);
    }

    public void BombDestory()
    {
        Instantiate(BombParticle, transform.localPosition, Quaternion.identity);
        Destroy(gameObject);
    }

}
