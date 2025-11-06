using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SoundDevice : MonoBehaviour
{
    public CircleCollider2D circle2D;
    bool MusicFlag;
    public int atk;
    public float Attack_Delay;
    public ParticleSystem BoomEffect;
    float lastTime;

    void Start()
    {
        MusicFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        BulletFire();
    }

    void BulletFire()
    {
        if (Time.time >= lastTime + Attack_Delay)
        {
            if (!MusicFlag)
            {
                StartCoroutine(Music());
                MusicFlag = true;
            }
        }
    }

    IEnumerator Music()
    {
        BoomEffect.Play();
        float duration = 0.3f; // 성장 시간
        float elapsed = 0f;

        float startRadius = 0f;
        float endRadius = 3f;

        circle2D.radius = 0;
        circle2D.enabled = true;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // 0~1 사이 보간값
            float t = elapsed / duration;

            // 원의 반지름 보간
            circle2D.radius = Mathf.Lerp(startRadius, endRadius, t);

            yield return null;
        }

        // 마지막 보정
        circle2D.radius = endRadius;
        yield return new WaitForSeconds(0.1f); // 필요시 잠깐 대기
        circle2D.enabled = false;
        MusicFlag = false;
        lastTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            if (monster != null)
            {
                monster.Damage_Monster_Item(atk);
                Debug.Log("Sound Device Hit Monster");
            }
        }
    }
}
