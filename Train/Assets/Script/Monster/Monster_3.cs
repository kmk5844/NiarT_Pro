using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_3 : Monster
{
    [SerializeField]
    Vector3 monster_SpawnPos;
    Vector3 movement;
    float xPos;

    [Header("속도, 최대 길이")] // 몬스터 무브를 변경해야할 가능성이 높음
    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;

    [SerializeField]
    float frequency;
    [SerializeField]
    float amplitude;



    protected override void Start()
    {
        base.Start();

        monster_SpawnPos = transform.position;

        speed = Random.Range(3f, 7f);
        max_xPos = Random.Range(1, 9);
        frequency = Random.Range(5f, 15f);
        amplitude = Random.Range(0.5f, 1.5f);


        xPos = -1f;
    }

    private void Update()
    {
        BulletFire();
        FlipMonster();
    }

    private void FixedUpdate()
    {
        MonsterMove();
    }

    void MonsterMove()
    {
        float yPos = Mathf.Sin(Time.time * frequency) * amplitude;

        if (monster_SpawnPos.x - max_xPos > transform.position.x)
        {
            xPos = 1f;
        }
        else if (monster_SpawnPos.x + max_xPos < transform.position.x)
        {
            xPos = -1f;
        }
        movement = new Vector3(xPos, yPos, 0f);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
