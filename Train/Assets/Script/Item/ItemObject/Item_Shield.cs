using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shield : MonoBehaviour
{
    public int HP;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            int monsterAtk = collision.GetComponent<MonsterBullet>().atk;
            if (HP - monsterAtk < 0)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
            else
            {
                HP -= monsterAtk;
                Debug.Log(monsterAtk);
            }
            Destroy(collision.gameObject);
        }
    }
}
