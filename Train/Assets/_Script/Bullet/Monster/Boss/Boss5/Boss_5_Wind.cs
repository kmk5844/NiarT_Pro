using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_5_Wind : MonsterBullet
{
    bool ScarecrowFlag;
    MonsterDirector monsterdirector;

    Vector2 vector;

    bool playerFlag;
    bool AngleFlag;

    protected override void Start()
    {
        base.Start();
        ScarecrowFlag = false;
        monsterdirector = FindObjectOfType<MonsterDirector>();
        if (monsterdirector.Item_Scarecrow_List.Count > 0)
        {
            ScarecrowFlag = true;
            int num = Random.Range(0, monsterdirector.Item_Scarecrow_List.Count);
            player_target = monsterdirector.Item_Scarecrow_List[num].transform;
        }
        Bullet_Fire();
        StartCoroutine(AcerSKill());
        Destroy(gameObject, 7f);
    }

    void Bullet_Fire()
    {
        if (playerFlag)
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
        }
        else if (AngleFlag)
        {
            rid.velocity = vector.normalized * Speed;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, rid.velocity);
        }
    }

    public void SetPlayer()
    {
        playerFlag = true;
    }

    public void SetAngle(float angle)
    {
        AngleFlag = true;
        float radians = angle * Mathf.Deg2Rad; // 각도를 라디안으로 변환
        Vector2 originalVector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)); // 벡터값

        vector = new Vector2(originalVector.y, -originalVector.x);
    }

    IEnumerator AcerSKill()
    {
        atk = atk / 2;
        float duration = 1f;
        Vector2 startScale_Size = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localScale = Vector2.Lerp(startScale_Size, startScale_Size * 60f, t);
            yield return null;
        }

        transform.localScale = startScale_Size * 60f;
    }
}
