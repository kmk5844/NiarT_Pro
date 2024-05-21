using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_1 : Monster
{
    [SerializeField]
    Vector3 monster_SpawnPos;
    Vector3 movement;
    int xPos;

    [Header("속도, 최대 길이")] // 몬스터 무브를 변경해야할 가능성이 높음
    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;

    protected override void Start()
    {
        base.Start();
        transform.position = new Vector3(transform.position.x, -0.625f, transform.position.z);
        monster_SpawnPos = transform.position;

        speed =  0.1f;
        max_xPos = Random.Range(1, 3);

        xPos = -1;
    }

    private void Update()
    {
        if(xPos > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        //MonsterMove();
        BulletFire(xPos);
    }

    private void FixedUpdate()
    {
        MonsterMove();
    }

    void MonsterMove()
    {
        if (monster_SpawnPos.x - max_xPos > transform.position.x)
        {
            xPos = 1;
        }
        else if (monster_SpawnPos.x + max_xPos < transform.position.x)
        {
            xPos = -1;
        }
        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
