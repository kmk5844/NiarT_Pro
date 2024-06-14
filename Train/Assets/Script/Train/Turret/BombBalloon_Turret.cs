using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombBalloon_Turret : MonoBehaviour
{
    public Transform BulletObject;
    Transform Balloon_List;
    Train_InGame trainData;
    float train_Attack_Delay;
    float lastTime;
    bool SpawnFlag;
    bool BallonFlag;
    public float max_X;
    public float min_X;
    public float max_Y;
    Vector3 RandomPos;

    void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        Balloon_List = GameObject.Find("Balloon_List").GetComponent<Transform>();
        BulletObject.GetComponent<Bullet>().atk = trainData.Train_Attack;
        train_Attack_Delay = trainData.Train_Attack_Delay;
        lastTime = 0;
    }
    void Update()
    {
        if(Balloon_List.childCount < 2)
        {
            if (!BallonFlag)
            {
                StartCoroutine(BulletFire());
            }
        }
    }

    IEnumerator BulletFire()
    {
        BallonFlag = true;
        yield return new WaitForSeconds(train_Attack_Delay);
        float Random_X = Random.Range(min_X, max_X);
        RandomPos = new Vector3(transform.position.x + Random_X, transform.position.y + max_Y, 0);
        Instantiate(BulletObject, RandomPos, transform.rotation, Balloon_List);
        BallonFlag = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector2(transform.position.x + min_X, transform.position.y + max_Y), new Vector2(transform.position.x + max_X, transform.position.y + max_Y));
    }
}
