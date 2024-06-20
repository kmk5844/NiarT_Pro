using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBoy : Mercenary
{
    public float unity_Attack_Delay;
    float binding_Time;
    bool binding_Flag;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        act  = Active.move;
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
        }
    }

    private void FixedUpdate()
    {
        if (M_gameType == GameType.Playing)
        {
            if(act == Active.move)
            {
                base.combatant_Move();
            }else if(act == Active.work)
            {
                rb2D.velocity = Vector2.zero;

            }else if(act == Active.die)
            {
                rb2D.velocity = Vector2.zero;
            }
        }else if(M_gameType == GameType.Ending)
        {
            act = Active.Game_Wait;
            rb2D.velocity = Vector2.zero;
        }
    }

}