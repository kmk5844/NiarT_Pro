using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBalloon_Turret : MonoBehaviour
{
    public Transform BulletObject;
    Transform Balloon_List;
    //Train_InGame trainData;
    float train_Attack_Delay;
    float lastTime;
    public float max_X;
    public float min_X;
    public float max_Y;
    Vector3 RandomPos;

    void Start()
    {
        //trainData = transform.GetComponentInParent<Train_InGame>();
        Balloon_List = GameObject.Find("Balloon_List").GetComponent<Transform>();
        //BulletObject.GetComponent<Bullet>().atk = trainData.Train_Attack;
        //train_Attack_Delay = trainData.Train_Attack_Delay;
        train_Attack_Delay = 5;
        lastTime = 0;
    }
    void Update()
    {
        if(Balloon_List.childCount < 2)
        {
            BulletFire();
        }
        else
        {
            lastTime = Time.time;
        }
    }

    void BulletFire()
    {
        float Random_X = Random.Range(min_X, max_X);
        RandomPos = new Vector3(Random_X, max_Y, 0);
        if (Time.time >= lastTime + train_Attack_Delay)
        {
            Instantiate(BulletObject, RandomPos, transform.rotation, Balloon_List);
            lastTime = Time.time;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector2(min_X, max_Y), new Vector2(max_X, max_Y));
    }
}
