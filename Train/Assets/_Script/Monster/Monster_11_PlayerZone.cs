using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_11_PlayerZone : MonoBehaviour
{
    Monster_11 monster_11;


    private void Start()
    {
        monster_11 = GetComponentInParent<Monster_11>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            monster_11.AttackTrigger();
        }
    }
}
