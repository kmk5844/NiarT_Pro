using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine_Driver : Mercenary
{
    [SerializeField]
    Engine_Driver_Type Driver_Type;
    GameDirector Gd;
    bool isSurvival;
    [Header("타입마다의 추가 스탯")]
    [SerializeField]
    int Level_Speed;
    [SerializeField]
    int Level_Fuel;
    [SerializeField]
    int Level_Def;
    protected override void Start()
    {
        base.Start();
        transform.position = new Vector3(-2, move_Y, 0);
        Gd = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        isSurvival = true;

        Driver_Type = SA_MercenaryData.Engine_Driver_Type;
        switch (Driver_Type)
        {
            case Engine_Driver_Type.speed:
                Gd.Engine_Driver_Passive(Engine_Driver_Type.speed, Level_Speed, true);
                break;
            case Engine_Driver_Type.fuel:
                Gd.Engine_Driver_Passive(Engine_Driver_Type.fuel, Level_Fuel, true);
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
            else if (Stamina <= 0)
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
            else if (act == Active.revive && !isSurvival)
            {
                switch (Driver_Type)
                {
                    case Engine_Driver_Type.speed:
                        Gd.Engine_Driver_Passive(Engine_Driver_Type.speed, 10, true);
                        break;
                    case Engine_Driver_Type.fuel:
                        Gd.Engine_Driver_Passive(Engine_Driver_Type.fuel, 4, true);
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
                Debug.Log("여기서 애니메이션 구현한다!5");
                switch (Driver_Type)
                {
                    case Engine_Driver_Type.speed:
                        Gd.Engine_Driver_Passive(Engine_Driver_Type.speed, 10, false);
                        break;
                    case Engine_Driver_Type.fuel:
                        Gd.Engine_Driver_Passive(Engine_Driver_Type.fuel, 4, false);
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