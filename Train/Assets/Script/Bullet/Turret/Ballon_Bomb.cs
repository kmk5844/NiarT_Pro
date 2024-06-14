using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon_Bomb : MonoBehaviour
{
    Balloon_TurretBullet Turret;
    bool bomb_flag;
    void Start()
    {
        Turret = GetComponentInParent<Balloon_TurretBullet>();
        bomb_flag = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);
        if (!bomb_flag)
        {
            foreach (Collider2D collider in colliders)
            {
                // ����� ���� Ÿ���� ��쿡�� ������ ����
                if (collider.CompareTag("Monster"))
                {
                    collider.GetComponent<Monster>().Damage_Monster_Bomb(Turret.atk);
                }
            }
            bomb_flag = true;
        }

        gameObject.GetComponentInParent<SpriteRenderer>().enabled = false;
        //���⼭ ��ź �ִϸ��̼� 
        Destroy(Turret.gameObject, 3f);
    }

}
