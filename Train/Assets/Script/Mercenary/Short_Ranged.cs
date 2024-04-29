using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short_Ranged : Mercenary
{
    bool zeroFlag;
    [Header("타입마다의 추가 스탯")]
    [Header("공격력")]
    public int unit_Attack;
    [Header("공격 쿨타임")]
    public float unit_Attack_Delay;

    protected override void Start()
    {
        base.Start();
        act = Active.move;
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
            else if (Stamina <= 0 && act != Active.die)
            {
                act = Active.weak;
            }

            if (act == Active.move)
            {
                base.combatant_Move();
            }
            else if (act == Active.work)
            {
                if (transform.position.x < MinMove_X || transform.position.x > MaxMove_X)
                {
                    move_X *= -1;
                }
            }
            else if (act == Active.die && isDying)
            {
                Debug.Log("여기서 애니메이션 구현한다!2");
                transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = false;
                isDying = false;
            }
            else if (act == Active.weak)
            {
                StartCoroutine(Refresh_Weak());
                transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = false;
                if (Stamina >= 70)
                {
                    transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = true;
                    act = Active.move;
                }
            }
            else if (act == Active.revive)
            {
                transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = true;
            }
        }
    }

    public void Level_AddStatus_ShortRanged(List<Info_Level_Mercenary_Short_ranged> type, int level)
    {
        unit_Attack = type[level].Unit_Attack;
        unit_Attack_Delay = type[level].Unit_Atk_Delay;
    }

    public void Attack_Stamina()
    {
        if (Stamina - useStamina < 0)
        {
            Stamina = 0;
        }
        else
        {
            Stamina -= useStamina;
        }
    }
}