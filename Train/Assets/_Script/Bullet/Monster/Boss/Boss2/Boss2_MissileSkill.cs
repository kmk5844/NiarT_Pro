using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_MissileSkill : MonsterBullet
{
    public bool PlayerTarget;
    GameObject BombParticle;
    public GameObject BombOjbect;

    protected override void Start()
    {
        atk = 100;
        slow = 2;
        Speed = 20;
        BombParticle = Resources.Load<GameObject>("Bullet/Boss2_Skill0_Effect");
        BombOjbect.SetActive(false);
        base.Start();
        Bullet_Fire();
    }

    void Bullet_Fire()
    {
        if (!PlayerTarget)
        {
            dir = new Vector3(0, -1, 0);
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        }
        else
        {
            float Rx = Random.Range(player_target.position.x - 20, player_target.position.x + 20);
            if (!player_target.GetComponent<Player>().isHealing)
            {
                dir = (player_target.transform.position - transform.position).normalized;
            }
            else
            {
                dir = (new Vector3(Rx, -1, 0) - transform.position).normalized;
            }
            rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        }
    }

    public void Bomb()
    {
        Instantiate(BombParticle, transform.localPosition, Quaternion.identity);
        BombOjbect.SetActive(true);
    }
}