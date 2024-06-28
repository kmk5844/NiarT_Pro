using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon_Bomb : MonoBehaviour
{
    Balloon_TurretBullet Ballon;
    bool bomb_flag;
    void Start()
    {
        Ballon = GetComponentInParent<Balloon_TurretBullet>();
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
                    collider.GetComponent<Monster>().Damage_Monster_BombAndDron(Ballon.atk);
                }
            }
            bomb_flag = true;
        }

        gameObject.GetComponentInParent<SpriteRenderer>().enabled = false;
        //���⼭ ��ź �ִϸ��̼� 
        Destroy(Ballon.gameObject, 2f);
    }
}
