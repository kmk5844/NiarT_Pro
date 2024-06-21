using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short_Ranged_New : Mercenary_New
{
    bool zeroFlag;
    public int unit_Attack;
    public float unit_Attack_Delay;
    Short_Ranged_KillZone killzone;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        act = Active.move;
        killzone = GetComponentInChildren<Short_Ranged_KillZone>();
    }

    private void Update()
    {
        Check_GameType();
        if(Mer_GameType == GameType.Playing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
            if(act == Active.move)
            {
                if (!killzone.enabled)
                {
                    killzone.enabled = true;
                }
            }
            if(act == Active.work)
            {
                if(workCount > Max_workCount)
                {
                    act = Active.refresh;
                }
            }
            if (act == Active.refresh)
            {
                if (!isRefreshing)
                {
                    StartCoroutine(Refresh());
                }
                if (killzone.enabled)
                {
                    killzone.enabled = false;
                }
            }
            if (act == Active.die && isDying)
            {
                killzone.enabled = false;
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
            else if (act == Active.die)
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
    public void Level_AddStatus_ShortRanged(List<Info_Level_Mercenary_Short_ranged> type, int level)
    {
        unit_Attack = type[level].Unit_Attack;
        unit_Attack_Delay = type[level].Unit_Atk_Delay;
    }
}
