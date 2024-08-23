/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine_Driver_Before : Mercenary_Before
{
    [SerializeField]
    Engine_Driver_Type Driver_Type;
    GameDirector gamedirector;
    bool isSurvival;
    [Header("타입마다의 추가 스탯")]
    [SerializeField]
    int Level_Speed;
    [SerializeField]
    int Level_Fuel;
    [SerializeField]
    int Level_Def;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        Type = mercenaryType.Engine_Driver;
        act = Active.work;
        transform.position = new Vector3(-4.4f, move_Y, 0);
        isSurvival = true;

        Driver_Type = SA_MercenaryData.Engine_Driver_Type;
        switch (Driver_Type)
        {
            case Engine_Driver_Type.speed:
                gamedirector.Engine_Driver_Passive(Engine_Driver_Type.speed, Level_Speed, true);
                break;
            case Engine_Driver_Type.fuel:
                gamedirector.Engine_Driver_Passive(Engine_Driver_Type.fuel, Level_Fuel, true);
                break;
            case Engine_Driver_Type.def:
                for(int i = 0; i < Train_List.childCount; i++)
                {
                    Train_List.GetChild(i).GetComponent<Train_InGame>().Train_Armor += Level_Def; 
                }
                break;
        }
    }

    void Update()
    {
        Check_GameType();

        if (M_gameType == GameType.Playing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }

            if (act == Active.revive && !isSurvival)
            {
                switch (Driver_Type)
                {
                    case Engine_Driver_Type.speed:
                        gamedirector.Engine_Driver_Passive(Engine_Driver_Type.speed, Level_Speed, true);
                        break;
                    case Engine_Driver_Type.fuel:
                        gamedirector.Engine_Driver_Passive(Engine_Driver_Type.fuel, Level_Fuel, true);
                        break;
                    case Engine_Driver_Type.def:
                        for (int i = 0; i < Train_List.childCount; i++)
                        {
                            Train_List.GetChild(i).GetComponent<Train_InGame>().Train_Armor += 10;
                        }
                        break;
                }
                isSurvival = true;
            }
            else if (act == Active.die && isDying)
            {
                //Debug.Log("여기서 애니메이션 구현한다!5");
                switch (Driver_Type)
                {
                    case Engine_Driver_Type.speed:
                        gamedirector.Engine_Driver_Passive(Engine_Driver_Type.speed, Level_Speed, false);
                        break;
                    case Engine_Driver_Type.fuel:
                        gamedirector.Engine_Driver_Passive(Engine_Driver_Type.fuel, Level_Fuel, false);
                        break;
                    case Engine_Driver_Type.def:
                        for (int i = 0; i < Train_List.childCount; i++)
                        {
                            Train_List.GetChild(i).GetComponent<Train_InGame>().Train_Armor -= 10;
                        }
                        break;
                }

                isSurvival = false;
                isDying = false;
            }
            *//*            else if (Stamina <= 0)
            {
                act = Active.weak;
            }
           if (act == Active.work)
            {
                //조건과 스킬을 어떤식으로 사용하면 좋은지
                Debug.Log("스킬과 스테미나 사용");
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
        else if (M_gameType == GameType.Ending)
        {
            act = Active.Game_Wait;
        }
    }

    public void Level_AddStatus_Engine_Driver(List<Info_Level_Mercenary_Engine_Driver> type, int level)
    {
        Level_Speed = type[level].Level_Type_Speed;
        Level_Fuel = type[level].Level_Type_Fuel;
        Level_Def = type[level].Level_Type_Def;
    }
}
*/