using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHit_Player : MonoBehaviour
{
    Player player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            player.MonsterHit(bullet.atk);
            Destroy(collision.gameObject);
        }
    }
}
