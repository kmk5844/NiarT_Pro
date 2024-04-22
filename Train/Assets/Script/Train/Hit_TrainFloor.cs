using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_TrainFloor : MonoBehaviour
{
    Train_InGame train;

    private void Start()
    {
        train = transform.GetComponentInParent<Train_InGame>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            MonsterBullet bullet = collision.gameObject.GetComponent<MonsterBullet>();
            train.Train_MonsterHit(bullet);
            Destroy(collision.gameObject);
        }
    }
}
