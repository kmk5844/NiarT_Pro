using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Iron_Plate : MonoBehaviour
{
    public Train_InGame Train;
    public int HP;
    int MaxHP = 1500;

    public void Set(Train_InGame train, int hp)
    {
        Train = train;
        HP = hp;
    }

    public void Heal(int hp)
    {
        HP += hp;
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet")){
            int atk = collision.GetComponent<MonsterBullet>().atk;
            // Destroy the bullet
            Destroy(collision.gameObject);
            HP -= atk;
            if (HP <= 0)
            {
                Train.Item_Spawn_Iron_Destory();
                Destroy(gameObject);
            }
        }
    }
}
