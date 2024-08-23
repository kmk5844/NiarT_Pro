using MoreMountains.Feel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBangBullet : Bullet
{
    bool bombFlag;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Bullet_Player();
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
        Destroy(gameObject, 6f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);
        if (!bombFlag)
        {
            foreach (Collider2D collider in colliders)
            {
                // ����� ���� Ÿ���� ��쿡�� ������ ����
                if (collider.CompareTag("Monster"))
                {
                    collider.GetComponent<Monster>().Item_FlashBang(5);
                }
            }
            bombFlag = true;
        }

        gameObject.GetComponentInParent<SpriteRenderer>().enabled = false;
        gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
        //���⼭ ��ź �ִϸ��̼� 
        Destroy(gameObject, 2f);
    }
}
