using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_2 : Monster
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
    protected override void Start()
    {
        base.Start();
        transform.position = new Vector3(9, transform.position.y, transform.position.z);

        speed = 10;

        xPos = -1f;
        StartCoroutine(DestoryAfterDelay());
    }

    private void Update()
    {
        BulletFire(0);
    }

    private void FixedUpdate()
    {
        MonsterMove();
    }

    private IEnumerator DestoryAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    void MonsterMove()
    {
        movement = new Vector3(xPos, 0f, 0f);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
