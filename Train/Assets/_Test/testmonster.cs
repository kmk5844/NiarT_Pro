using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmonster : MonoBehaviour
{
    public void Damage_Monster_Bomb(int Bomb_Atk)
    {
        Debug.Log(gameObject.name + " : " + Bomb_Atk);
    }

    private void OnCollisionEnter2D(Collision2D collision) // 공통적으로 적용해야됨.
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            Debug.Log(this.gameObject.name + "_C : " + collision.gameObject.GetComponent<Bullet>().atk);
            Destroy(collision.gameObject);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player_Bullet"))
        {
            Debug.Log(this.gameObject.name + "_T : " + collision.gameObject.GetComponent<Bullet>().atk);
            Destroy(collision.gameObject);
        }
    }
}
