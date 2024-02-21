using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryDirector : MonoBehaviour
{
    Player player;
    public Transform Mercenary_List;
    List<GameObject> Engineer_List;
    List<GameObject> Long_Ranged_List;
    List<GameObject> Short_Ranged_List;
    List<GameObject> Medic_List;
    List<GameObject> Engineer_Driver_List;
    
    int Engineer_Num;
    int Long_Ranged_Num;
    int Short_Ranged_Num;
    int Medic_Num;
    int Engine_Dirver_Num;

    public bool isEngineerCall;
    public bool isMedicCall;

    void Start()
    {
        Check_List();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        isEngineerCall = false; isMedicCall = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isEngineerCall)
        {
            Engineer_Call();
        }
        else if(player.check_HpParsent() < 70 && Input .GetKeyDown(KeyCode.E) && !isMedicCall) {
            Medic_Call();
        }
    }

    public void Engineer_Call()
    {
        if(Engineer_Num == 0)
        {
            Debug.Log("엔지니어 없자나");
        }
        else
        {
            for (int i = 0; i < Engineer_Num; i++)
            {
                if (Engineer_List[i].GetComponent<Engineer>().Check_Work())
                {
                    Engineer_List[i].GetComponent<Engineer>().PlayerEngineerCall();
                    isEngineerCall = true;
                    break;
                } //있으면 오라고 콜함, 없으면 다음
            }

            if(!isEngineerCall)
            {
                Engineer_List[0].GetComponent<Engineer>().PlayerEngineerCall();
                isEngineerCall = true;
            }
        }
    }
    public void Medic_Call()
    {
        if(Medic_Num == 0)
        {
            Debug.Log("메딕 없자나");
        }else
        {
            for(int i = 0; i < Medic_Num; i++)
            {
                if (Medic_List[i].GetComponent<Medic>().Check_Work())
                {
                    Medic_List[i].GetComponent<Medic>().PlayerMedicCall();
                    isMedicCall = true;
                    break;
                }
            }
        }
        if (!isMedicCall)
        {
            Medic_List[0].GetComponent<Medic>().PlayerMedicCall();
            isMedicCall = true;
        }
    }

    public void Engineer_Call_End()
    {
        isEngineerCall = false;
    }

    public void Medic_Call_End()
    {
        isMedicCall = false;
    }
    public void Check_List()
    {
        for(int i = 0; i <  Mercenary_List.childCount; i++)
        {
            switch (Mercenary_List.GetChild(i).GetComponent<Mercenary_Type>().mercenary_type)
            {
                case mercenaryType.Engineer:
                    Engineer_Num++;break;
                case mercenaryType.Long_Ranged:
                    Long_Ranged_Num++; break;
                case mercenaryType.Short_Ranged:
                    Short_Ranged_Num++; break;
                case mercenaryType.Medic :
                    Medic_Num++; break;
                case mercenaryType.Engine_Driver:
                    Engine_Dirver_Num++; break;
            }
        }
        Add_List();
    }

    public void Add_List()
    {
        Engineer_List = new List<GameObject>(Engineer_Num);
        Long_Ranged_List = new List<GameObject>(Long_Ranged_Num);
        Short_Ranged_List = new List<GameObject>(Short_Ranged_Num);
        Medic_List = new List<GameObject>(Medic_Num);
        Engineer_Driver_List = new List<GameObject>();

        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            switch (Mercenary_List.GetChild(i).GetComponent<Mercenary_Type>().mercenary_type)
            {
                case mercenaryType.Engineer:
                    Engineer_List.Add(Mercenary_List.GetChild(i).gameObject);
                    break;
                case mercenaryType.Long_Ranged:
                    Long_Ranged_List.Add(Mercenary_List.GetChild(i).gameObject);
                    break;
                case mercenaryType.Short_Ranged:
                    Short_Ranged_List.Add(Mercenary_List.GetChild(i).gameObject);
                    break;
                case mercenaryType.Medic:
                    Medic_List.Add(Mercenary_List.GetChild(i).gameObject);
                    break;
                case mercenaryType.Engine_Driver:
                    Engineer_Driver_List.Add(Mercenary_List.GetChild(i).gameObject);
                    break;
            }
        }
    }
}
