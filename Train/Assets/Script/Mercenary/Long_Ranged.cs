using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Long_Ranged : Mercenary
{
    [Header("Ÿ�Ը����� �߰� ����")]
    [Header("���ݷ�")]
    public int unit_Attack;
    [Header("���ݼӵ�")]
    public float unit_Attack_Delay;
    [Header("������ ��, �̵��ӵ�")]
    [SerializeField]
    float workSpeed;
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
        shoot = gameObject.GetComponentInChildren<Long_RangedShoot>();
    }

    private void Update()
    {
        Check_GameType();

        if(M_gameType == GameType.Playing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
            else if (Stamina == 0 && act == Active.work && !zeroFlag)
            {
                act = Active.weak;
            }

           
            if (act == Active.die && isDying)
            {
                Debug.Log("���⼭ �ִϸ��̼� �����Ѵ�!2");
                transform.GetComponentInChildren<Long_RangedShoot>().enabled = false;
                isDying = false;
            }
            else if (act == Active.weak)
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
            else if (act == Active.revive)
            {
                transform.GetComponentInChildren<Long_RangedShoot>().enabled = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if(M_gameType == GameType.Playing)
        {
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
                rb2D.velocity = new Vector2(move_X * 2f, rb2D.velocity.y);
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
    public void Level_AddStatus_LongRanged(List<Info_Level_Mercenary_Long_Ranged> type, int level)
    {
        unit_Attack = type[level].Unit_Attack;
        unit_Attack_Delay = type[level].Unit_Atk_Delay;
        workSpeed = type[level].WorkSpeed;
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