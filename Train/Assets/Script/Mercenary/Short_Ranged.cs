using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short_Ranged : Mercenary
{
    bool zeroFlag;
    [Header("���� ��Ÿ��")]
    public int Attack_CoolTime;

    protected override void Start()
    {
        base.Start();
        act = Active.move;
    }

    void Update()
    {
        if (HP <= 0 && act != Active.die)
        {
            act = Active.die;
            isDying = true;
        }else if(Stamina <= 0 && act != Active.die)
        {
            act = Active.weak;
        }

        if (act == Active.move)
        {
            base.combatant_Move();
        }else if(act == Active.work)
        {

        }else if(act == Active.die && isDying)
        {
            Debug.Log("���⼭ �ִϸ��̼� �����Ѵ�!2");
            transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = false;
            isDying = false;
        }else if(act == Active.weak)
        {
            StartCoroutine(Refresh_Weak());
            transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = false;
            if (Stamina >= 70)
            {
                transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = true;
                act = Active.move;
            }
        }else if(act == Active.revive)
        {
            transform.GetComponentInChildren<Short_Ranged_KillZone>().enabled = true;
        }
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