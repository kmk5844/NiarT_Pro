using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombBalloon_Turret : Turret
{
    public Transform BulletObject;
    Transform Balloon_List; // Bullet에 넣어도 되지만, 풍선은 따로 제한해야되기 때문에 Ballon으로 이용.
    bool BallonFlag;
    public float max_X;
    public float min_X;
    public float max_Y;
    Vector3 RandomPos;

    protected override void Start()
    {
        base.Start();
        trainData = transform.GetComponentInParent<Train_InGame>();
        Balloon_List = GameObject.Find("Balloon_List").GetComponent<Transform>();
        BulletObject.GetComponent<Bullet>().atk = trainData.Train_Attack;
        train_Attack_Delay = trainData.Train_Attack_Delay;
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
        yield return new WaitForSeconds((train_Attack_Delay - Item_Attack_Delay));
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
