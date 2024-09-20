using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet_Angle : MonsterBullet
{
    [SerializeField]
    Vector2 vector;

    protected override void Start()
    {
        base.Start();
        Bullet_Fire();
    }

    void Bullet_Fire()
    {
        rid.velocity = vector.normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }

    public void SetAngle_And_Fire(float angle)
    {
        float radians = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        Vector2 originalVector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)); // 벡터값

        vector = new Vector2(originalVector.y, -originalVector.x);
    }
}
