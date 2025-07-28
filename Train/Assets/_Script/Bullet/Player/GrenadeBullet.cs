using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GrenadeBullet : Bullet
{
    public float explosionDelay = 1f;       // ���߱��� ��� �ð�
    public float explosionRadius = 4f;    // ���� �ݰ�
    bool exploded = false;

    protected override void Start()
    {
        base.Start();
        rid.gravityScale = 3f; // ��ź�� ���̰� �Ʒ��� �־���
        Bullet_Player();
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        if (exploded) return;
        exploded = true;

/*        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);*/

        // ���� �ݰ� ���� ���Ϳ��� ������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                Monster monster = collider.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.Damage_Monster_Item(atk);
                }
            }
        }
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
        Destroy(gameObject);
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

        StartCoroutine(ExplodeAfterDelay());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Explode();
        }
    }
}
