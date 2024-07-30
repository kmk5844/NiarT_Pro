using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Boss_0 : MonoBehaviour
{
    public int Boss_HP;
    public int Boss_Atk;

    public float Bullet_Speed;
    public float Bullet_Slow;

    public int Random_Patten_Num;

    [SerializeField]
    Boss_PlayType playType;
    PolygonCollider2D col;

    Vector3 local_Scale;

    private void Start()
    {
        col = transform.GetComponent<PolygonCollider2D>();
        local_Scale = transform.localScale;


        transform.position = new Vector2(MonsterDirector.MaxPos_Sky.x + 9f, MonsterDirector.MinPos_Ground.y);
        playType = Boss_PlayType.Spawn;
        col.enabled = false;
    }

    private void FixedUpdate()
    {
        if(playType == Boss_PlayType.Spawn)
        {
            if(transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-local_Scale.x, local_Scale.y, local_Scale.z);
            }

            transform.Translate(-8f * Time.deltaTime, 0, 0);
            if(transform.position.x < MonsterDirector.MinPos_Ground.x - 5f)
            {
                playType = Boss_PlayType.Move;
                if(transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(local_Scale.x, local_Scale.y, local_Scale.z);
                }
                col.enabled = true;
            }
        }
    }

    enum Boss_PlayType
    {
        Spawn,
        Move,
        Attack,
        Skill,
        Die
    }

    enum Boss_Patern
    {
        Spawn_Egg,
        Mcus_Rampage,
        Spider_Web,
    }
}