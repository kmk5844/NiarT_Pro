using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_MissileDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            GetComponentInParent<Boss2_MissileSkill>().Bomb();
        }
    }
}