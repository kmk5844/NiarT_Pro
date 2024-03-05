using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine_Driver : Mercenary
{
    public Engine_Driver_Type Dirver_Type;
    GameDirector Gd;
    bool isSurvival;
    protected override void Start()
    {
        base.Start();
        transform.position = new Vector3(-2, -1, 0);
        Gd = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        isSurvival = true;

        switch (Dirver_Type){
            case Engine_Driver_Type.speed:
                Gd.Engine_Driver_Passive(Engine_Driver_Type.speed, 10, true);
                break;
            case Engine_Driver_Type.fuel:
                Gd.Engine_Driver_Passive(Engine_Driver_Type.fuel, 4, true);
                break;
            case Engine_Driver_Type.def:
                for(int i = 0; i < Train_List.childCount; i++)
                {
                    Train_List.GetChild(i).GetComponent<Train_InGame>().Train_Anmor += 10; 
                }
                break;
        }
    }

    void Update()
    {
        if (HP <= 0 && act != Active.die)
        {
            act = Active.die;
            isDying = true;
        }else if(Stamina <= 0)
        {
            act = Active.weak;
        }

        if (act == Active.work)
        {
            //조건과 스킬을 어떤식으로 사용하면 좋은지
            Debug.Log("스킬과 스테미나 사용");
        }else if(act == Active.weak)
        {
            if (!isRefreshing_weak)
            {
                StartCoroutine(Refresh_Weak());
            }else if(Stamina >= 70)
            {
                act = Active.move;
            }
        }else if(act == Active.revive && !isSurvival)
        {
            switch (Dirver_Type)
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
                        Train_List.GetChild(i).GetComponent<Train_InGame>().Train_Anmor += 10;
                    }
                    break;
            }
            isSurvival = true;
        }
        else if (act == Active.die && isDying)
        {
            Debug.Log("여기서 애니메이션 구현한다!5");
            switch (Dirver_Type)
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
                        Train_List.GetChild(i).GetComponent<Train_InGame>().Train_Anmor -= 10;
                    }
                    break;
            }
            isSurvival = false;
            isDying = false;
        }
    }
}

public enum Engine_Driver_Type
{
    speed,
    fuel,
    def
}