using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBoy : Mercenary
{
    public float unity_Attack_Delay;
    public bool refreshFlag;
    CowBoy_Grap grap;
    CircleCollider2D grapZone;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        act  = Active.move;
        grap = GetComponentInChildren<CowBoy_Grap>();
        grapZone = grap.GetComponent<CircleCollider2D>();
    }

    protected override void Update()
    {
        base.Update();
        if(workCount >= Max_workCount + base.Item_workCount_UP)
        {
            refreshFlag = true;
        }
        else
        {
            refreshFlag = false;
        }


        if(Mer_GameType == GameType.Playing || Mer_GameType == GameType.Boss || Mer_GameType == GameType.Refreshing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
            if(act == Active.move)
            {
                if (grapZone.enabled == false)
                {
                    grapZone.enabled = true;
                }
            }

            if (act == Active.refresh)
            {
                if (grapZone.enabled == true)
                {
                    grapZone.enabled = false;
                }

                if (!isRefreshing)
                {
                    StartCoroutine(Refresh());
                }
            }

            if(act == Active.die)
            {
                if (grapZone.enabled == true)
                {
                    grapZone.enabled = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Mer_GameType == GameType.Playing || Mer_GameType == GameType.Boss)
        {
            if (act == Active.move)
            {
                base.Combatant_Move();
            }
            else if (act == Active.work)
            {
                rb2D.velocity = Vector2.zero;
            }
            else if (act == Active.refresh)
            {
                rb2D.velocity = Vector2.zero;
            }
            else if (act == Active.die)
            {
                rb2D.velocity = Vector2.zero;
            }
        }else if(Mer_GameType == GameType.Ending)
        {
            act = Active.Game_Wait;
            rb2D.velocity = Vector2.zero;
        }
    }
}