using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_Ranged_New : Mercenary_New
{
    public int unit_Attack;
    public float unit_Attack_Delay;

    public bool zeroFlag;
    Long_RangedShoot shoot;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        act = Active.move;
        zeroFlag = false;
        shoot = GetComponentInChildren<Long_RangedShoot>();
    }

    private void Update()
    {
        Check_GameType();

        if(Mer_GameType == GameType.Playing)
        {
            if (HP <= 0&& act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
            if(act == Active.move)
            {
                if (!shoot.enabled)
                {
                    shoot.enabled = true;
                }
            }
            if (act == Active.work)
            {
                if(workCount > Max_workCount)
                {
                    act = Active.refresh;
                }

            }
            if(act == Active.refresh)
            {
                if (!isRefreshing)
                {
                    StartCoroutine(Refresh());
                }
                if (shoot.enabled)
                {
                    shoot.enabled = false;
                }
            }
            if(act == Active.die && isDying)
            {
                shoot.enabled = false;
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
            else if (act == Active.work)
            {
                if (transform.position.x < MinMove_X || transform.position.x > MaxMove_X)
                {
                    Move_X *= -1;
                }
                rb2D.velocity = Vector2.zero;
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

    public void Level_AddStatus_LongRanged(List<Info_Level_Mercenary_Long_Ranged> type, int level)
    {
        unit_Attack = type[level].Unit_Attack;
        unit_Attack_Delay = type[level].Unit_Atk_Delay;
        //workSpeed = type[level].WorkSpeed;
    }
    public void TargetFlag(bool Flag)
    {
        if (Flag && act != Active.refresh)
        {
            act = Active.work;
        }
        else
        {
            if (!zeroFlag)
            {
                act = Active.move;
            }
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
}
