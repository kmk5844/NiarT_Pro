using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shield : MonoBehaviour
{
    public int HP;
    public ParticleSystem HitEffect;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            Vector2 pos = new Vector2(collision.transform.localPosition.x, collision.transform.localPosition.y);
            Instantiate(HitEffect, pos, Quaternion.identity);

            int monsterAtk = collision.GetComponent<MonsterBullet>().atk;
            if (HP - monsterAtk < 0)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            else
            {
                HP -= monsterAtk;
            }
            Destroy(collision.gameObject);
        }
    }
}
