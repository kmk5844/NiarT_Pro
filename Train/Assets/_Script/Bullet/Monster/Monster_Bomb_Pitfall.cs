using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bomb_Pitfall : MonsterBullet
{
    public float DestoryDelay = 5f;
    public GameObject BombCollider;
    GameObject BombParticle;

    // Start is called before the first frame update
    protected override void Start()
    {
        BombCollider.SetActive(false);
        BombParticle = Resources.Load<GameObject>("Bullet/10_Effect");
        base.Start();
        Destroy(gameObject, DestoryDelay);
    }
    void BombDestory()
    {
        BombCollider.SetActive(true);
        Instantiate(BombParticle, transform.localPosition, Quaternion.identity);
        Destroy(gameObject, 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BombDestory();
        }
    }
}
