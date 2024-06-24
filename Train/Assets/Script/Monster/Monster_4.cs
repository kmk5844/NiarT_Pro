using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_4 : Monster
{
    [SerializeField]
    Vector3 monster_SpawnPos;
    Vector3 movement;
    float xPos;
    float yPos;

    [Header("속도, 최대 길이")] // 몬스터 무브를 변경해야할 가능성이 높음
    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;

    float max_yPos;
    bool spawn_Flag;

    protected override void Start()
    {
        base.Start();
        spawn_Flag = false;
        transform.position = new Vector3(21, 5, 0.25f);
        monster_SpawnPos = transform.position;

        speed = 5;
        max_yPos = 1;
        max_xPos = 6;

        xPos = -1f;
        yPos = 1f;
    }

    protected override void Update()
    {
        //MonsterMove();
    }

    protected override void FixedUpdate()
    {
        if(transform.position.x > max_xPos)
        {
            MonsterMove();
        }
        else
        {
            if (!spawn_Flag)
            {
                speed = 0.5f;
                monster_SpawnPos = transform.position;
                spawn_Flag = true;
            }
            DemoBulletFire();
            MonsterMove_H();
        }
    }

    void MonsterMove()
    {
        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * speed * Time.deltaTime);
    }

    void MonsterMove_H()
    {
        if (monster_SpawnPos.y + max_yPos < transform.position.y)
        {
            yPos = -1f;
        }
        else if (monster_SpawnPos.y - max_yPos > transform.position.y)
        {
            yPos = 1f;
        }
        movement = new Vector3(0f, yPos, 0f);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
