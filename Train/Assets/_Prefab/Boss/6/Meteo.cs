using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : MonsterBullet
{
    bool ScarecrowFlag;
    MonsterDirector monsterdirector;
    public SpriteRenderer spriteRenderer;

    Vector2 maxScale = new Vector2(40, 40);
    protected override void Start()
    {
        base.Start();
        ScarecrowFlag = false;
        monsterdirector = FindObjectOfType<MonsterDirector>();

        StartCoroutine(Big());
        Destroy(gameObject, 12f);
    }

    IEnumerator Big()
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < 2)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / 2;
            transform.localScale = Vector3.Lerp(startScale, maxScale, t);

            yield return null;
        }

        // 오차 방지
        transform.localScale = maxScale;
        yield return new WaitForSeconds(1f);
        if (monsterdirector.Item_Scarecrow_List.Count > 0)
        {
            ScarecrowFlag = true;
            int num = Random.Range(0, monsterdirector.Item_Scarecrow_List.Count);
            player_target = monsterdirector.Item_Scarecrow_List[num].transform;
        }
        PlayerFire();
    }

    void PlayerFire()
    {
        spriteRenderer.flipY = true;
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
