using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare_TurretBullet : Bullet
{
    float Random_X;
    float Random_Y;
    public float RandomBombTime;
    public SpriteRenderer TurretSprite;

    public GameObject Bomb;
    float lastTime;
    bool BombFlag;

    protected override void Start()
    {
        base.Start();
        BombFlag = false;
        Speed = Random.Range(1f, 1.5f);
        RandomBombTime = Random.Range(2f, 3f);
        Random_X = Random.Range(-1f, 1f);
        Random_Y = Random.Range(0.5f, 1.5f);
        rid.velocity = new Vector2 (Random_X, Random_Y);
        rid.velocity *= Speed;
        Bomb.SetActive(false);
        lastTime = Time.time;
    }

    private void Update()
    {
        if(Time.time > lastTime + RandomBombTime && !BombFlag)
        {
            BombFlag = true;
            rid.velocity = Vector2.zero;
            TurretSprite.enabled = false;
            Bomb.SetActive(true);
        }
    }
}
