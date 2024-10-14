using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public bool ShotGunFlag;
    protected override void Start()
    {
        base.Start();
        if (ShotGunFlag)
        {
            Speed = Random.Range(40f, 55f);
        }
        Bullet_Player();
    }

    void Bullet_Player()
    {
        Camera _cam = Camera.main;

        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        dir = mousePos - transform.position;
        if (!ShotGunFlag)
        {
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90);
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }
        else
        {
            // 무작위 각도를 추가하기 위해 회전 벡터를 사용
            float angleOffset = Random.Range(-9f, 9f); // 무작위 각도 범위 설정 (-10도에서 10도 사이)
            dir = Quaternion.Euler(0, 0, angleOffset) * dir;

            // 총알 회전 설정
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90 + angleOffset);

            // 속도 설정
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }
        Destroy(gameObject, 3f);
    }
}
