using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_ButterFly_Bullet : MonsterBullet
{
    // 랜덤으로 확정될 값들
    float initialYSpeed;
    float gravity;
    float waveAmplitude;
    float waveFrequency;
    float phaseOffset;

    float time;

    protected override void Start()
    {
        base.Start();
        waveAmplitude = Random.Range(5f, 10f);   // 날개 폭
        waveFrequency = Random.Range(6f, 10f);       // 흔들림 속도
        phaseOffset = Random.Range(0f, Mathf.PI * 3f);
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        time += Time.deltaTime;

        float xMove = Speed * x_scale * Time.deltaTime;

        float yMove = Mathf.Sin(time * waveFrequency + phaseOffset)
                      * waveAmplitude * Time.deltaTime;

        transform.position += new Vector3(xMove, yMove, 0f);
    }

}
