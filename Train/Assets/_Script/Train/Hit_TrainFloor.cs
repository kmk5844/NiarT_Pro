using Cinemachine;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_TrainFloor : MonoBehaviour
{
    Train_InGame train;
    public GameObject Hit_Effect;
    GameObject impulse_Object;
    CinemachineImpulseSource impulseSource;
    private void Start()
    {
        train = transform.GetComponentInParent<Train_InGame>();
        impulse_Object = GameObject.Find("ImpulseObject");
        impulseSource = impulse_Object.GetComponent<CinemachineImpulseSource>();
    }

    public void HitTrain(Monster monster)
    {
        CameraShakeManager.instance.CameraShake(impulseSource);
        train.Train_MonsterHit(monster);

        float rnd = Random.Range(0f, 2f);
        Vector2 pos = new Vector2(monster.transform.localPosition.x, monster.transform.localPosition.y);

        Instantiate(Hit_Effect, pos, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            MonsterBullet bullet = collision.gameObject.GetComponent<MonsterBullet>();
            CameraShakeManager.instance.CameraShake(impulseSource);
            train.Train_MonsterHit(bullet);

            float rnd = Random.Range(0f, 2f);
            Vector2 pos = new Vector2(collision.transform.localPosition.x, collision.transform.localPosition.y + rnd);

            if (!collision.gameObject.name.Equals("BombCollider")){
                Instantiate(Hit_Effect, pos, Quaternion.identity);
            }
            Destroy(collision.gameObject);
        }
    }
}
