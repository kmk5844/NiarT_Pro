using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GrenadeBullet : Bullet
{
    public Transform SpriteObject;
    public float explosionDelay = 1f;       // 폭발까지 대기 시간
    public float explosionRadius = 4f;    // 폭발 반경
    bool exploded = false;

    protected override void Start()
    {
        base.Start();
        rid.gravityScale = 3f; // 유탄이 무겁게 아래로 휘어짐
        Bullet_Player();
    }

    private void Update()
    {
        if (rid != null && SpriteObject != null)
        {
            // 현재 속도 방향에 맞게 스프라이트 회전
            float angle = Mathf.Atan2(rid.velocity.y, rid.velocity.x) * Mathf.Rad2Deg;
            SpriteObject.localRotation = Quaternion.Euler(0, 0, angle);
        }
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

        // 폭발 반경 내의 몬스터에게 데미지
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
