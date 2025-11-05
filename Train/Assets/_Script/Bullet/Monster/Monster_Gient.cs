using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Gient : MonsterBullet
{
    public float DestoryDelay = 7f;
    public float MaxScale = 4f;
    public CapsuleCollider2D Collider;
    public Transform bulletSprite;

    protected override void Start()
    {
        base.Start();
        Bullet_Fire();
        StartCoroutine(ScaleUp(MaxScale, 1f));
    }

    void Bullet_Fire()
    {
        float Rx = Random.Range(player_target.position.x - 10, player_target.position.x + 10);
        dir = (new Vector3(Rx, -1, 0) - transform.position).normalized;
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }

    IEnumerator ScaleUp(float targetScale, float duration)
    {
        Vector2 startScale_Size = Collider.size;
        Vector2 startScale_Sprite = bulletSprite.localScale;
        Vector2 endScale_Size = new Vector2(Collider.size.x * targetScale, startScale_Size.y);
        Vector2 endScale_Sprite = new Vector2(bulletSprite.localScale.x * targetScale, startScale_Sprite.y);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            Collider.size = Vector2.Lerp(startScale_Size, endScale_Size, t);
            bulletSprite.localScale = Vector2.Lerp(startScale_Sprite, endScale_Sprite, t);
            yield return null;
        }

        Collider.size = endScale_Size;
        bulletSprite.localScale = endScale_Sprite;
    }
}
