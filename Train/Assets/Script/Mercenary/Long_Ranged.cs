using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_Ranged : Mercenary
{
    public Active act;
    [Header("공격력")]
    public int unit_Attack;
    [Header("공격속도")]
    public float unit_Attack_Delay;
    [Header("공격할 때, 이동속도")]
    [SerializeField]
    float workSpeed;
    [Header("0일 때, 이동속도")]
    float zeroSpeed;

    bool zeroFlag;

    Long_RangedShoot shoot;
    protected override void Start()
    {
        base.Start();
        act = Active.move;
        zeroSpeed = 0.5f;
        zeroFlag = false;
        shoot = gameObject.GetComponentInChildren<Long_RangedShoot>();
    }

    private void Update()
    {
        if(Stamina == 0 && act == Active.work && !zeroFlag)
        {
            StartCoroutine(zeroRefresh());
        }

        if (act == Active.move)
        {
            base.move();
        }else if(act == Active.work){
            if (transform.position.x > MaxMove_X)
            {
                move_X *= -1f;
                sprite.flipX = true;
            }
            else if (transform.position.x < MinMove_X)
            {
                move_X *= -1f;
                sprite.flipX = false;
            }

            if (zeroFlag)
            {
                transform.Translate(move_X * zeroSpeed, 0, 0);
            }
            else
            {
                transform.Translate(move_X * workSpeed, 0, 0);
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
        if (Flag)
        {
            act = Active.work;
        }
        else
        {
            act = Active.move;
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