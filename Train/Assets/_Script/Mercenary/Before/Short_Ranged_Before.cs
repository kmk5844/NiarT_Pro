/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short_Ranged_Before : Mercenary_Before
{
    bool zeroFlag;
    [Header("Ÿ�Ը����� �߰� ����")]
    [Header("���ݷ�")]
    public int unit_Attack;
    [Header("���� ��Ÿ��")]
    public float unit_Attack_Delay;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        Type = mercenaryType.Short_Ranged;
        act = Active.move;
    }

    private void FixedUpdate()
    {
        if (M_gameType == GameType.Playing)
        {
            if (act == Active.move)
            {
                base.combatant_Move();
            }
            else if (act == Active.die)
            {
                rb2D.velocity = Vector2.zero;
            }
        }
        else if (M_gameType == GameType.Ending)
        {
            act = Active.Game_Wait;
            rb2D.velocity = Vector2.zero;
        }
    }

    void Update()
    {
        Check_GameType();

        if (M_gameType == GameType.Playing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
*//*            else if (Stamina <= 0 && act != Active.die)
            {
                act = Active.weak;
            }*//*

            if (act == Active.work)
            {
                if (transform.position.x < MinMove_X || transform.position.x > MaxMove_X)
                {
                    move_X *= -1;
                }
            }
            else if (act == Active.die && isDying)
            {
                Debug.Log("���⼭ �ִϸ��̼� �����Ѵ�!2");
                transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = false;
                isDying = false;
            }
            else if (act == Active.refresh)
            {
*//*                StartCoroutine(Refresh_Weak());
                transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = false;
                if (Stamina >= 70)
                {
                    transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = true;
                    act = Active.move;
                }*//*
            }
            else if (act == Active.revive)
            {
                transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = true;
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

    public void Level_AddStatus_ShortRanged(List<Info_Level_Mercenary_Short_ranged> type, int level)
    {
        unit_Attack = type[level].Unit_Attack;
        unit_Attack_Delay = type[level].Unit_Atk_Delay;
    }

*//*    public void Attack_Stamina()
    {
        if (Stamina - useStamina < 0)
        {
            Stamina = 0;
        }
        else
        {
            Stamina -= useStamina;
        }
    }*//*
}*/