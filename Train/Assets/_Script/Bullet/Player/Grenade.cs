using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Grenade : Bullet
{
    bool bombFlag;
    float cutline;

    // Start is called before the first frame update
    protected override void Start()
    {
        cutline = (MonsterDirector.MinPos_Sky.y + MonsterDirector.MaxPos_Sky.y / 2);
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

    private void FixedUpdate()
    {
        if (!bombFlag && transform.position.y > cutline)
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!bombFlag)
        {
            Explode();
        }
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 6);
        {
            foreach (Collider2D collider in colliders)
            {
                // 대상이 유닛 타입일 경우에만 데미지 적용
                if (collider.CompareTag("Monster"))
                {
                    collider.GetComponent<Monster>().Damage_Monster_Item(atk);
                }
            }
            bombFlag = true;
        }

        gameObject.GetComponentInParent<SpriteRenderer>().enabled = false;
        gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
        //여기서 폭탄 애니메이션 
        Destroy(gameObject, 2f);
    }

}