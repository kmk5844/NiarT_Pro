using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet_59 : MonsterBullet
{
    [SerializeField]
    Vector2 vector;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Bullet_Fire());
        Destroy(gameObject, 10f);
    }

    IEnumerator Bullet_Fire()
    {
        SetAngle_And_Fire(180f);
        rid.velocity = vector.normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        yield return new WaitForSeconds(1.5f);
        After_Bullet_Fire();
    }

    void After_Bullet_Fire()
    {
        transform.position = new Vector2(player_target.transform.position.x, transform.position.y);
        rid.velocity = Vector2.zero;
        SetAngle_And_Fire(0f);
        rid.velocity = vector.normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
    }

    public void SetAngle_And_Fire(float angle)
    {
        float radians = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        Vector2 originalVector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)); // 벡터값

        vector = new Vector2(originalVector.y, -originalVector.x);
    }
}
