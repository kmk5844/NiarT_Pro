using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_Turret : Turret
{
    public CircleCollider2D circle2D;
    bool MusicFlag;
    int atk;

    protected override void Start()
    {
        base.Start();
        MusicFlag = false;
        atk = trainData.Train_Attack;
    }

    // Update is called once per frame
    void Update()
    {
        BulletFire();
    }

    void BulletFire()
    {
        if (Time.time >= lastTime + (train_Attack_Delay - Item_Attack_Delay))
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
        // 1차 성장: 0.5f ~ 4f
        while (circle2D.radius < 10f)
        {
            circle2D.radius += 0.2f;
            yield return null;
        }

        circle2D.radius = 0.5f;
        yield return new WaitForSeconds(0.5f); // 필요시 잠깐 대기
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
            }
        }
    }
}