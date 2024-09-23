using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_TrainFloor : MonoBehaviour
{
    Train_InGame train;
    FillDirector fill_Director;
    public GameObject Hit_Effect;
   
    private void Start()
    {
        train = transform.GetComponentInParent<Train_InGame>();
        fill_Director = train.gameDirector.GetComponent<GameDirector>().fill_director;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            MonsterBullet bullet = collision.gameObject.GetComponent<MonsterBullet>();
            fill_Director.PlayFill(0);
            train.Train_MonsterHit(bullet);
            Instantiate(Hit_Effect, collision.transform.localPosition, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
