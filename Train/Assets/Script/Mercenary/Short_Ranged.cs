using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short_Ranged : Mercenary
{
    Vector3 targetPosition;
    [Header("잡으러 가는 속도")]
    public int work_Speed;
    public GameObject killzone;
    bool zeroFlag;

    protected override void Start()
    {
        base.Start();
        act = Active.move;
        zeroFlag = false;
        isRefreshing_weak = false;
    }

    void Update()
    {
        if(HP <= 0 && act != Active.die)
        {             
            act = Active.die;
            isDying = true;
        }
        else if(Stamina == 0 && !zeroFlag)
        {
            act = Active.weak;
        }

        if (transform.position.x > MaxMove_X)
        {
            transform.position = new Vector3(MaxMove_X - 1, -1, 0);
            move_X = -0.01f;
            sprite.flipX = true;
            transform.Translate(move_X * moveSpeed, 0, 0);
        }
        else if (transform.position.x < MinMove_X)
        {
            transform.position = new Vector3(MinMove_X + 1, -1, 0);
            move_X = 0.01f;
            sprite.flipX = false;
            transform.Translate(move_X * moveSpeed, 0, 0);
        }

        if (act == Active.move)
        {
            if (zeroFlag)
            {
                transform.Translate(Vector3.zero);
            }
            else
            {
                base.combatant_Move();
            }
        }else if(act == Active.work)
        {
            if (zeroFlag)
            {
                transform.Translate(Vector3.zero);
            }
            else
            {
                run_Target();
            }
        }else if(act == Active.die && isDying)
        {
            Debug.Log("여기서 애니메이션 구현한다!3");
            transform.GetComponentInChildren<Short_Ranged_DetectionZone>().enabled = false;
            transform.GetComponentInChildren<Short_Range_KillZone>().enabled = false;
            isDying = false;
        }else if(act == Active.weak)
        {
            if (!isRefreshing_weak)
            {
                zeroFlag = true;
                killzone.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(Refresh_Weak());
            }
            else if(Stamina >= 70)
            {
                killzone.GetComponent<BoxCollider2D>().enabled = true;
                zeroFlag = false;
                act = Active.move;
            }

        }
    }

    public void TargetPosition(Transform Target)
    {
        targetPosition = Target.position;
    }

    public void kill_Stamina()
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

    void run_Target()
    {
        if(targetPosition.x > transform.position.x)
        {
            move_X = 0.01f;
            sprite.flipX = false;
            transform.Translate(move_X * work_Speed, 0, 0);
        }
        else
        {
            move_X = -0.01f;
            sprite.flipX = true;
            transform.Translate(move_X * work_Speed, 0, 0);
        }
    }
    public void TargetFlag(bool Flag)
    {
        if(act != Active.weak)
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
    }
}