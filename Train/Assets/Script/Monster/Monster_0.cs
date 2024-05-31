using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_0 : Monster
{
    [SerializeField]
    Vector3 monster_SpawnPos;
    Vector3 movement;
    float xPos;

    [Header("�ӵ�, �ִ� ����")] // ���� ���긦 �����ؾ��� ���ɼ��� ����
    [SerializeField]
    float speed;
    [SerializeField]
    float max_xPos;

    protected override void Start()
    {
        base.Start();

        monster_SpawnPos = transform.position;

        speed = Random.Range(0.5f, 2f);
        max_xPos = Random.Range(1, 9);

        xPos = -1f;
    }

    private void Update()
    {
        Total_GameType();
        if (monster_gametype == Monster_GameType.Fighting)
        {
            BulletFire();
            FlipMonster();
        }
        else if(monster_gametype == Monster_GameType.GameEnding)
        {
            Monster_Ending();
        }
    }

    private void FixedUpdate()
    {
        MonsterMove();
    }

    void MonsterMove()
    {
        if (monster_SpawnPos.x - max_xPos > transform.position.x)
        {
            xPos = 1f;
        }
        else if (monster_SpawnPos.x + max_xPos < transform.position.x)
        {
            xPos = -1f;
        }
        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
