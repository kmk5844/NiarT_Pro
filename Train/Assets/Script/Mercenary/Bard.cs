using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard : Mercenary
{
    [SerializeField]
    Bard_Type bard_type;
    bool isSurvival;

    int Level_HP;
    int Level_Atk;
    int Level_Def;

    Transform Mercenary_List;
    Player player;

    bool buffFlag;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        Mercenary_List = mercenaryDirector.Mercenary_List;
        act = Active.work;
        buffFlag = false;
        transform.position = new Vector2(-2f, Move_Y);
        bard_type = gameDirector.SA_MercenaryData.Bard_Type;
        player = gameDirector.player;
    }

    protected override void Update()
    {
        base.Update();
        if(mercenaryDirector.Mercenary_Spawn_Flag == true && !buffFlag)
        {
            Bard_Survival_Buff();
            buffFlag = true;
        }

        if(Mer_GameType == GameType.Playing)
        {
            if(HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
            if(act == Active.revive && !isSurvival)
            {
                Bard_Survival_Buff();
                isSurvival = true;
            }else if(act == Active.die && isDying)
            {
                Bard_Die_Buff();
                isSurvival = false;
                isDying = false;
            }
        }
        else if (Mer_GameType == GameType.Ending)
        {
            act = Active.Game_Wait;
        }
    }

    public void Level_AddStatus_Bard(List<Info_Level_Mercenary_Bard> type, int level)
    {
        Level_HP = type[level].Level_Type_HP_Buff;
        Level_Atk = type[level].Level_Type_Atk_Buff;
        Level_Def = type[level].Level_Type_Def_Buff;
    }

    void Bard_Survival_Buff()
    {
        switch (bard_type)
        {
            case Bard_Type.HP_Buff:
                player.P_Buff(bard_type, Level_HP, true);
                for (int i = 0; i < Mercenary_List.childCount; i++)
                {
                    Mercenary_List.GetChild(i).GetComponent<Mercenary_Type>().Buff_HP(Level_HP, true);
                }
                break;
            case Bard_Type.Atk_Buff:
                player.P_Buff(bard_type, Level_Atk, true);
                for (int i = 0; i < Mercenary_List.childCount; i++)
                {
                    Mercenary_List.GetChild(i).GetComponent<Mercenary_Type>().Buff_Atk(Level_Atk, true);
                }
                break;
            case Bard_Type.Def_Buff:
                player.P_Buff(bard_type, Level_Def, true);
                for (int i = 0; i < Mercenary_List.childCount; i++)
                {
                    Mercenary_List.GetChild(i).GetComponent<Mercenary_Type>().Buff_Def(Level_Def, true);
                }
                break;
        }
    }

    void Bard_Die_Buff()
    {
        switch (bard_type)
        {
            case Bard_Type.HP_Buff:
                player.P_Buff(bard_type, Level_HP, false);
                for (int i = 0; i < Mercenary_List.childCount; i++)
                {
                    Mercenary_List.GetChild(i).GetComponent<Mercenary_Type>().Buff_HP(Level_HP, false);
                }
                break;
            case Bard_Type.Atk_Buff:
                player.P_Buff(bard_type, Level_Atk, false);
                for (int i = 0; i < Mercenary_List.childCount; i++)
                {
                    Mercenary_List.GetChild(i).GetComponent<Mercenary_Type>().Buff_Atk(Level_Atk, false);
                }
                break;
            case Bard_Type.Def_Buff:
                player.P_Buff(bard_type, Level_Def, false);
                for (int i = 0; i < Mercenary_List.childCount; i++)
                {
                    Mercenary_List.GetChild(i).GetComponent<Mercenary_Type>().Buff_Def(Level_Def, false);
                }
                break;
        }
    }
}
public enum Bard_Type
{
    HP_Buff,
    Atk_Buff,
    Def_Buff
}