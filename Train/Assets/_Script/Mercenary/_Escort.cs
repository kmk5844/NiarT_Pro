using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Escort : Mercenary
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        act = Active.move;
    }

    protected override void Update()
    {
        base.Update();

        if (Mer_GameType == GameType.Playing || Mer_GameType == GameType.Boss || Mer_GameType == GameType.Refreshing)
        {
            if(HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
                gameDirector.GetComponent<GameDirector>().MissionFail();
            }
        }
    }

    private void FixedUpdate()
    {
        if (Mer_GameType == GameType.Playing || Mer_GameType == GameType.Boss || Mer_GameType == GameType.Refreshing)
        {
            if (act == Active.move)
            {
                base.Combatant_Move();
            }
        }
        else if(Mer_GameType == GameType.Ending)
        {
            act = Active.Game_Wait;
            rb2D.velocity = Vector2.zero;
        }
    }

    public void EscortSetSetting(int _hp, int _armor, int _moveSpeed)
    {
        base.HP = _hp;
        base.def = _armor;
        base.moveSpeed = _moveSpeed;
    }
}
