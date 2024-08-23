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
    public float gravity = -9.81f; // 중력 가속도
    private Vector3 velocity; // 속도를 저장할 변수
    private bool isFalling = false; // 낙하 상태를 나타내는 변수


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
            // 중력의 영향을 받아 계속해서 떨어지도록 함
            velocity.y += gravity * Time.deltaTime; // 중력 가속도를 적용
            transform.position += velocity * Time.deltaTime; // 속도를 통해 위치 변경
        }
    }
}
