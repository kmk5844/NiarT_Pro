using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine_Driver : Mercenary
{
    [SerializeField]
    Engine_Driver_Type Driver_Type;
    bool isSurvival;
    int Level_Speed;
    int Level_Fuel;
    int Level_Def;

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        act = Active.work;
        transform.position = new Vector2(-4.4f, Move_Y);
        isSurvival = true;
        isDying = false;

        Driver_Type = SA_MercenaryData.Engine_Driver_Type;
        EngineDriver_Survival_Buff();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(Mer_GameType == GameType.Playing || Mer_GameType == GameType.Boss || Mer_GameType == GameType.Refreshing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }

            if(act == Active.revive && !isSurvival)
            {
                EngineDriver_Survival_Buff();
                isSurvival = true;
            }

            if(act == Active.die && isDying)
            {
                EngineDriver_Die_Buff();
                isSurvival = false;
                isDying = false;
            }
        }else if(Mer_GameType == GameType.Ending)
        {
            act = Active.Game_Wait;
        }
    }

    void EngineDriver_Survival_Buff() {
        switch (Driver_Type)
        {
            case Engine_Driver_Type.speed:
                gameDirector.Engine_Driver_Passive(Engine_Driver_Type.speed, Level_Speed, true);
                break;
            case Engine_Driver_Type.fuel:
                gameDirector.Engine_Driver_Passive(Engine_Driver_Type.fuel, Level_Fuel, true);
                break;
            case Engine_Driver_Type.def:
                for (int i = 0; i < Train_List.childCount; i++)
                {
                    Train_List.GetChild(i).GetComponent<Train_InGame>().Train_Armor += Level_Def;
                }
                break;
        }
    }

    void EngineDriver_Die_Buff()
    {
        switch (Driver_Type)
        {
            case Engine_Driver_Type.speed:
                gameDirector.Engine_Driver_Passive(Engine_Driver_Type.speed, Level_Speed, false);
                break;
            case Engine_Driver_Type.fuel:
                gameDirector.Engine_Driver_Passive(Engine_Driver_Type.fuel, Level_Fuel, false);
                break;
            case Engine_Driver_Type.def:
                for (int i = 0; i < Train_List.childCount; i++)
                {
                    Train_List.GetChild(i).GetComponent<Train_InGame>().Train_Armor -= Level_Def;
                }
                break;
        }
    }

    public void Level_AddStatus_Engine_Driver(List<Info_Level_Mercenary_Engine_Driver> type, int level)
    {
        Level_Speed = type[level].Level_Type_Speed;
        Level_Fuel = type[level].Level_Type_Fuel;
        Level_Def = type[level].Level_Type_Def;
    }
}
public enum Engine_Driver_Type
{
    speed,
    fuel,
    def
}
