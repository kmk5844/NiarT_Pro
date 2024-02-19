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
    }

    void Update()
    {
        if(HP <= 0 && act != Active.die)
        {
            act = Active.die;
        }

        if(Stamina == 0 && !zeroFlag)
        {
            StartCoroutine(zeroRefresh());
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
        }else if(act == Active.die)
        {
            transform.GetComponentInChildren<Short_Ranged_DetectionZone>().enabled = false;
            transform.GetComponentInChildren<Short_Range_KillZone>().enabled = false;
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
        killzone.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(5);
        Stamina += 60;
        zeroFlag = false;
        killzone.GetComponent<BoxCollider2D>().enabled = true;
    }
}