using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss_0_Bullet : MonsterBullet
{
    public Vector2 targetPosition;
    public Vector2 Bullet_Init_Position;

    float Bullet_Time;
    public Transform spriteRotation;
    protected override void Start()
    {
        base.Start();
        Bullet_Time = 0f;
        Bullet_Init_Position = transform.position;
        targetPosition = player_target.position;
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        if (Bullet_Time < 1.2f)
        {
            Bullet_Time += Time.deltaTime;
            float t = Bullet_Time / 1.2f;

            float x = Mathf.Lerp(Bullet_Init_Position.x, targetPosition.x, t);
            float y = 6 * Mathf.Sin(Mathf.PI * t);
            //Mathf.Lerp(Bullet_Init_Position.y, Bullet_Init_Position.y, t); //+ 6 * Mathf.Sin(Mathf.PI * t);

            float z = Mathf.Lerp(45f, -90f, t);

            transform.position = new Vector3(x, y, 0);
            spriteRotation.localRotation = Quaternion.Euler(0, 0, z);
        }
        else
        {
            transform.Translate(4 *Vector2.down * Time.deltaTime);
        }
    }
}
