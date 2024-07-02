using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short_Ranged : Mercenary
{
    public int unit_Attack;
    public float unit_Attack_Delay;
    Short_Ranged_KillZone attackZone;
    [SerializeField]
    BoxCollider2D Zone_Collider;
    [SerializeField]
    BoxCollider2D Attack_Collider;
    bool attackFlag;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        act = Active.move;
        attackFlag = false;
        attackZone = GetComponentInChildren<Short_Ranged_KillZone>();
        Zone_Collider = attackZone.GetComponent<BoxCollider2D>();
        Attack_Collider = attackZone.transform.GetChild(0).GetComponentInChildren<BoxCollider2D>();
        
        Zone_Collider.enabled = true;
        Attack_Collider.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
        if(Mer_GameType == GameType.Playing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
            if(act == Active.move)
            {
                if (!Zone_Collider.enabled)
                {
                    Zone_Collider.enabled = true;
                }
            }
            if (act == Active.work)
            {
                if(workCount >= Max_workCount + base.Item_workCount_UP)
                {
                    act = Active.refresh;
                }
                if (!attackFlag)
                {
                    StartCoroutine(Attack());
                }
            }
            if (act == Active.refresh)
            {
                if (!isRefreshing)
                {
                    StartCoroutine(Refresh());
                }
                if (Zone_Collider.enabled)
                {
                    Zone_Collider.enabled = false;
                }
            }
            if (act == Active.die && isDying)
            {
                Zone_Collider.enabled = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Mer_GameType == GameType.Playing)
        {
            if (act == Active.move)
            {
                base.Combatant_Move();
            }
            if (act == Active.work)
            {
                rb2D.velocity = Vector2.zero;
            }
            if (act == Active.refresh)
            {
                rb2D.velocity = Vector2.zero;
            }

            if (act == Active.die)
            {
                rb2D.velocity = Vector2.zero;
            }
        }
        else if (Mer_GameType == GameType.Ending)
        {
            act = Active.Game_Wait;
            rb2D.velocity = Vector2.zero;
        }
    }

    public void M_Buff_Atk(int buff_atk, bool flag)
    {
        if (flag)
        {
            unit_Attack += buff_atk;
        }
        else
        {
            unit_Attack -= buff_atk;
        }
    }

    IEnumerator Attack()
    {
        attackFlag = true;
        yield return new WaitForSeconds(0.5f);
        Attack_Collider.enabled = true;
        workCount++;
        yield return new WaitForSeconds(0.5f);
        Attack_Collider.enabled = false;
        attackFlag = false;
    }
    public void Level_AddStatus_ShortRanged(List<Info_Level_Mercenary_Short_ranged> type, int level)
    {
        unit_Attack = type[level].Unit_Attack;
        unit_Attack_Delay = type[level].Unit_Atk_Delay;
    }
}