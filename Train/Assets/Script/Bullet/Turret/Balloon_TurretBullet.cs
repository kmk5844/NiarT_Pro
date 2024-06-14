using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon_TurretBullet : Bullet
{
    float Random_X; // �¿� ��鵵�� �۾� ����
    float Random_Y;
    public float maxY;
    public Transform monster_target;
    bool SpawnFlag;

    public GameObject Bomb;

    protected override void Start()
    {
        base.Start();
        Bomb.SetActive(false);
        SpawnFlag = true;
        Random_Y = Random.Range(0f, 1f);
        rid.velocity =  Vector2.up * Speed;
    }
    private void FixedUpdate()
    {
        if (SpawnFlag)
        {
            if (transform.position.y > maxY) {
                SpawnFlag = false;
            }
        }
        else
        {
            if (monster_target != null)
            {
                // Ÿ�� ���� ���
                Vector3 targetDirection = (monster_target.position - transform.position).normalized;

                // Rigidbody�� ȸ���� �������� �ʵ��� ����
                transform.eulerAngles = Vector3.zero;

                // Ÿ�� �������� �̵�
                rid.velocity = targetDirection * Speed;
            }
            else
            {
                if(transform.position.y > maxY + Random_Y)
                {
                    rid.velocity =  Vector2.down * 0.4f;
                }else if(transform.position.y < maxY - Random_Y){
                    rid.velocity =  Vector2.up * 0.4f;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Bomb.SetActive(true);
        }
    }
}
