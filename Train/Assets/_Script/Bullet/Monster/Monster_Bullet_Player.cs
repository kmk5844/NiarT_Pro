using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet_Player : MonsterBullet
{
    bool ScarecrowFlag;
    MonsterDirector monsterdirector;

    protected override void Start()
    {
        base.Start();
        ScarecrowFlag = false;
        monsterdirector = FindObjectOfType<MonsterDirector>();
        if(monsterdirector.Item_Scarecrow_List.Count > 0)
        {
            ScarecrowFlag = true;
            int num = Random.Range(0, monsterdirector.Item_Scarecrow_List.Count);
            player_target = monsterdirector.Item_Scarecrow_List[num].transform;
        }
        Bullet_Fire();
    }

    void Bullet_Fire()
    {
        float Rx = Random.Range(player_target.position.x - 20, player_target.position.x + 20);
        if (!ScarecrowFlag)
        {
            if (!player_target.GetComponent<Player>().isHealing)
            {
                dir = (player_target.transform.position - transform.position).normalized;
            }
            else
            {
                dir = (new Vector3(Rx, -1, 0) - transform.position).normalized;
            }
        }
        else
        {
            dir = (player_target.transform.position - transform.position).normalized;
        }

        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        Destroy(gameObject, 5f);
    }
}
