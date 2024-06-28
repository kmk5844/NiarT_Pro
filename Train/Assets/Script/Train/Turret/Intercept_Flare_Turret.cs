using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intercept_Flare_Turret : Turret
{
    public Transform BulletObject;
    public float max_X;
    public float min_X;
    public float max_Y;
    Vector3 RandomPos;
    bool BulletFlag;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (!BulletFlag)
        {
            StartCoroutine(BulletFire());
        }
    }

    IEnumerator BulletFire()
    {
        BulletFlag = true;
        yield return  new WaitForSeconds((train_Attack_Delay - Item_Attack_Delay));
        float Random_X = Random.Range(min_X, max_X);
        RandomPos = new Vector3(transform.position.x + Random_X, transform.position.y + max_Y, 0);
        Instantiate(BulletObject, RandomPos, transform.rotation, Bullet_List);
        BulletFlag = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector2(transform.position.x + min_X, transform.position.y + max_Y), new Vector2(transform.position.x + max_X, transform.position.y + max_Y));
    }
}