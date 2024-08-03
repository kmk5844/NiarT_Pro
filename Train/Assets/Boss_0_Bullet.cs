using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_0_Bullet : MonoBehaviour
{
    public Vector2 targetPosition;
    public Vector2 Bullet_Init_Position;

    float Bullet_Time;
    float Min_Y;
    public Transform spriteRotation;
    private void Start()
    {
        Bullet_Time = 0f;
        Bullet_Init_Position = transform.position;
        Min_Y = MonsterDirector.MinPos_Ground.y;
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
