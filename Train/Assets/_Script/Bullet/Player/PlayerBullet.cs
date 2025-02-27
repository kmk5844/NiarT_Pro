using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public bool ShotGunFlag;
    public bool RifleFlag;
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

        if (RifleFlag)
        {
            float angleOffset = Random.Range(-1.8f, 1.8f); // ������ ���� ���� ���� (-10������ 10�� ����)
            dir = Quaternion.Euler(0, 0, angleOffset) * dir;
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90 + angleOffset);
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }
        else if (ShotGunFlag)
        {
            // ������ ������ �߰��ϱ� ���� ȸ�� ���͸� ���
            float angleOffset = Random.Range(-9f, 9f); // ������ ���� ���� ���� (-10������ 10�� ����)
            dir = Quaternion.Euler(0, 0, angleOffset) * dir;

            // �Ѿ� ȸ�� ����
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90 + angleOffset);

            // �ӵ� ����
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }
        else
        {
            Vector3 rotation = transform.position - mousePos;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90);
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }

        Destroy(gameObject, 3f);
    }
}
