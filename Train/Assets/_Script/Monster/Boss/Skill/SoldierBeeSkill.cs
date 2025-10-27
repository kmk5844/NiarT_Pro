using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

public class SoldierBeeSkill : MonsterBullet
{
    Vector2 vector;

    protected override void Start()
    {
        base.Start();
    }

    public void SetAngle(int BossAtk, float angle)
    {
        atk = BossAtk;
        Speed = 20f;
        float radians = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        Vector2 originalVector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)); // 벡터값
        vector = new Vector2(originalVector.y, -originalVector.x);
        StartCoroutine(BulletFire());
    }

    public void SetPlayer(int BossAtk, Transform player)
    {
        atk = BossAtk;
        Speed = 30f;
        StartCoroutine(BulletFirePlayer(player));
    }

    IEnumerator BulletFire()
    {
        yield return new WaitForSeconds(0.5f);
        rid.velocity = vector.normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }

    IEnumerator BulletFirePlayer(Transform player)
    {
        yield return new WaitForSeconds(0.5f);
        float Rx = Random.Range(player.position.x - 20, player.position.x + 20);
        if (!player.GetComponent<Player>().isHealing)
        {
            dir = (player.transform.position - transform.position).normalized;
        }
        else
        {
            dir = (new Vector3(Rx, -1, 0) - transform.position).normalized;
        }
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }
}
