using JetBrains.Annotations;
using MoreMountains.Feel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : Bullet
{ 
    GameObject Target;
    bool TargetFlag;

    int Count;

    protected override void Start()
    {
        base.Start();
        TargetFlag = false;
        Bullet_Player();
    }

    private void FixedUpdate()
    {
        if (TargetFlag)
        {
            if (Target != null)
            {
                // 타겟 방향으로 회전
                transform.up = (Target.transform.position - transform.position).normalized;

                // 타겟 방향으로 이동
                rid.velocity = transform.up * Speed;
            }
        }
    }

    void Bullet_Player()
    {
        Camera _cam = Camera.main;

        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        dir = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        Destroy(gameObject, 5f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<Monster>().Damage_Monster_BombAndDron(atk/3);
            Count++;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 15);
            TargetFlag = false;
            Target = null;
            foreach(Collider2D collider in colliders)
            {
                if (collider.CompareTag("Monster") && collider.gameObject != collision.gameObject)
                {
                    Target = collider.gameObject;
                    TargetFlag = true;
                    break;
                }
            }

            if (Count > 2)
            {
                Destroy(gameObject);
            }
        }
    }
}
