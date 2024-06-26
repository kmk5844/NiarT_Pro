using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant_ScareCrow : MonoBehaviour
{
    public int HP;

    private void Start()
    {
        HP = 1000;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            int monsterAtk = collision.GetComponent<Bullet>().atk;
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
