using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster45_PlayerZone : MonoBehaviour
{
    Monster_45 monster_45;

    private void Start()
    {
        monster_45 = GetComponentInParent<Monster_45>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && monster_45.monster_gametype != Monster_GameType.Die)
        {
            monster_45.AttackTrigger();
        }
    }
}
