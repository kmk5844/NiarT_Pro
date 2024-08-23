using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Monster_Bullet_LR_Parabola : MonsterBullet
{
    Vector3 FireZone;
    float RandomXPos;
    float Bullet_Time;

    public Transform SpriteObject;
    public float gravity = -9.81f; // �߷� ���ӵ�
    private Vector3 velocity; // �ӵ��� ������ ����
    private bool isFalling = false; // ���� ���¸� ��Ÿ���� ����


    protected override void Start()
    {
        base.Start();
        Bullet_Time = 0f;
        FireZone = transform.position;
        RandomXPos = Random.Range(1f, 4f);
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        if(Bullet_Time < 1f)
        {
            Bullet_Time += Time.deltaTime;
            float tx = Bullet_Time / 1f;
            float x = 0;
            float y = Mathf.Sin(Mathf.PI * tx); 
            float z = 0;
            if (x_scale == 1)
            {
                x = Mathf.Lerp(FireZone.x, FireZone.x + RandomXPos, tx);
                z = Mathf.Lerp(45f, -90f, tx);
            }
            else
            {
                x = Mathf.Lerp(FireZone.x, FireZone.x - RandomXPos, tx);
                z = Mathf.Lerp(135f, 270f, tx);
            }
            transform.position = new Vector3(x, y, 0);
            SpriteObject.localRotation = Quaternion.Euler(0, 0, z);

            if (Bullet_Time >= 1f)
            {
                isFalling = true;
                velocity = new Vector3((x_scale == 1 ? RandomXPos : -RandomXPos) / 1f, Mathf.Cos(Mathf.PI * tx) * Mathf.PI, 0);
            }
        }

        if (isFalling)
        {
            // �߷��� ������ �޾� ����ؼ� ���������� ��
            velocity.y += gravity * Time.deltaTime; // �߷� ���ӵ��� ����
            transform.position += velocity * Time.deltaTime; // �ӵ��� ���� ��ġ ����
        }
    }
}
