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
            Speed = Random.Range(30f, 50f);
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
            // ������ ������ �߰��ϱ� ���� ȸ�� ���͸� ���
            float angleOffset = Random.Range(-12f, 12f); // ������ ���� ���� ���� (-10������ 10�� ����)
            dir = Quaternion.Euler(0, 0, angleOffset) * dir;

            // �Ѿ� ȸ�� ����
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90 + angleOffset);

            // �ӵ� ����
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }
        Destroy(gameObject, 3f);
    }
}
