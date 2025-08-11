using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon_TurretBullet : Bullet
{
    float Ballon_Min_Z;
    float Ballon_Max_Z;

    float Random_Y;
    public float maxY;
    public Transform monster_target;
    bool SpawnFlag;

    public GameObject Ballon;
    bool LR_BallonFlag;
    public GameObject Bomb;
    bool BombFlag;

    protected override void Start()
    {
        base.Start();
        Bomb.SetActive(false);
        SpawnFlag = true;
        Random_Y = Random.Range(0f, 1f);
        rid.velocity =  Vector2.up * Speed;
        BombFlag = false;

        Ballon_Min_Z = -0.1f;
        Ballon_Max_Z = 0.1f;
        LR_BallonFlag = true;
        StartCoroutine(SpawnBombTime());
    }
    private void FixedUpdate()
    {
        if(BombFlag == false)
        {
            if (monster_target == null)
            {
                if (LR_BallonFlag)
                {
                    Ballon.transform.Rotate(new Vector3(0, 0, -8f * Time.deltaTime));
                    if (Ballon.transform.localRotation.z < Ballon_Min_Z)
                    {
                        LR_BallonFlag = false;
                    }
                }
                else
                {
                    Ballon.transform.Rotate(new Vector3(0, 0, 8f * Time.deltaTime));
                    if (Ballon.transform.localRotation.z > Ballon_Max_Z)
                    {
                        LR_BallonFlag = true;
                    }
                }
            }
            else
            {
                if (Ballon != null)
                {
                    Ballon.transform.localEulerAngles = Vector3.zero;
                }
            }
        }


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
                if(monster_target.GetComponent<Monster>().monster_gametype == Monster_GameType.Die)
                {
                    monster_target = null;
                    return;
                }
                // 타겟 방향 계산
                Vector3 targetDirection = (monster_target.position - transform.position).normalized;

                // Rigidbody에 회전을 적용하지 않도록 설정
                transform.eulerAngles = Vector3.zero;

                // 타겟 방향으로 이동
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

    IEnumerator SpawnBombTime()
    {
        yield return new WaitForSeconds(6f);
        if (BombFlag)
        {
            Bomb.SetActive(true);
            BombFlag = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Bomb.SetActive(true);
            BombFlag = true;
        }
    }
}
