/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard_Before : Mercenary_Before
{
    [SerializeField]
    Bard_Type bard_type;
    bool isSurvival;
    Transform Mercenary_List;

    [SerializeField]
    int Level_HP;
    [SerializeField]
    int Level_Atk;
    [SerializeField]
    int Level_Def;

    Player player;

    bool Flag;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        Type = mercenaryType.Bard;
        act = Active.work;
        Flag = false;
        transform.position = new Vector3(-2f, move_Y, 0);
        
        Mercenary_List = GameObject.Find("Mercenary_List").transform;

        bard_type = SA_MercenaryData.Bard_Type;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Check_GameType();
        *//*if(MercenaryDirector.Mercenary_Spawn_Flag == true && !Flag)
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
                    player.P_Buff(bard_type, Level_Def  , true);
                    for (int i = 0; i < Mercenary_List.childCount; i++)
                    {
                        Mercenary_List.GetChild(i).GetComponent<Mercenary_Type>().Buff_Def(Level_Def, true);
                    }
                    break;
            }
            Flag = true;
        }*//*
        
        if (M_gameType == GameType.Playing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
            if(act == Active.revive && !isSurvival)
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
                isSurvival = true;
            }else if(act == Active.die && isDying)
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
                act = Active.work;
                isSurvival = false;
                isDying = false;
                *//*            else if (Stamina <= 0)
            {
                act = Active.weak;
            }

            if (act == Active.work)
            {
                //
            }
            else if (act == Active.weak)
            {
                if (!isRefreshing_weak)
                {
                    StartCoroutine(Refresh_Weak());
                }
                else if (Stamina >= 70)
                {
                    act = Active.move;
                }
            }
            else *//*
            }
        }else if(M_gameType == GameType.Ending)
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
}*/