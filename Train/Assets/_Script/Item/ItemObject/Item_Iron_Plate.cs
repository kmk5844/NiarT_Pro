using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Iron_Plate : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Sprite[] sprites;
    public Train_InGame Train;
    int spriteNum = -1;
    public int HP;
    int MaxHP = 1500;
    public void Set(Train_InGame train, int hp, int sp_num)
    {
        Train = train;
        HP = hp;
        changeSprite(sp_num);
    }
    public void Heal(int hp)
    {
        if(hp > MaxHP)
        {
            MaxHP = hp;
        }

        HP += hp;
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
    }

    public void changeSprite(int sp)
    {
        if (sp > spriteNum) // sp가 현재보다 클 때만
        {
            spriteNum = sp;
            sprite.sprite = sprites[spriteNum];
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