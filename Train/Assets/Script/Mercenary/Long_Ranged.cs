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
        }

        if (Stamina == 0 && act == Active.work && !zeroFlag)
        {
            StartCoroutine(zeroRefresh());
        }

        if (act == Active.move)
        {
            base.combatant_Move();
        }else if(act == Active.work){
            if (!zeroFlag)
            {
                transform.Translate(move_X * workSpeed, 0, 0);
            }
            else
            {
                transform.Translate(0, 0, 0);
            }
        }else if(act == Active.die)
        {
            transform.GetComponentInChildren<Long_RangedShoot>().enabled = false;
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
        if (Flag)
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

    IEnumerator zeroRefresh()
    {
        zeroFlag = true;
        shoot.isDelaying = true;
        yield return new WaitForSeconds(5f);
        Stamina += 50;
        shoot.isDelaying = false;
        zeroFlag = false;
    }
}