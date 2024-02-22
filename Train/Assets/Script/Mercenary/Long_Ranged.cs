using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_Ranged : Mercenary
{
    [Header("공격력")]
    public int unit_Attack;
    [Header("공격속도")]
    public float unit_Attack_Delay;
    [Header("공격할 때, 이동속도")]
    [SerializeField]
    float workSpeed;
    public bool zeroFlag;

    Long_RangedShoot shoot;
    protected override void Start()
    {
        base.Start();
        act = Active.move;
        zeroFlag = false;
        shoot = gameObject.GetComponentInChildren<Long_RangedShoot>();
    }

    private void Update()
    {
        if (HP <= 0 && act != Active.die)
        {
            act = Active.die;
            isDying = true;
        }else if (Stamina == 0 && act == Active.work && !zeroFlag)
        {
            act = Active.weak;
        }

        if (act == Active.move)
        {
            base.combatant_Move();
        }else if(act == Active.work){
            transform.Translate(move_X * workSpeed, 0, 0);
        }else if(act == Active.die && isDying)
        {
            Debug.Log("여기서 애니메이션 구현한다!2");
            transform.GetComponentInChildren<Long_RangedShoot>().enabled = false;
            isDying = false;
        }else if(act == Active.weak)
        {
            zeroFlag = true;
            shoot.isDelaying = true;
            StartCoroutine(Refresh_Weak());
            if (Stamina >= 70)
            {
                shoot.isDelaying = false;
                zeroFlag = false;
            }
        }
    }

    public void Shoot_Stamina()
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

    public void TargetFlag(bool Flag)
    {
        if (Flag && act != Active.weak)
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
}