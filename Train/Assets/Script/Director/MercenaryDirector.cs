using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryDirector : MonoBehaviour
{
    Player player;
    public Transform Mercenary_List;
    List<GameObject> Engineer_List;
    List<GameObject> Medic_List;
    
    public int Engineer_Num;
    public int Medic_Num;
    public int Engineer_live_Num;
    public int Medic_live_Num;
    public int last_Engineer;
    public int last_Medic;


    public bool isEngineerCall;
    public bool isMedicCall;
    bool isChecklive;

    void Start()
    {
        Check_List();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        isEngineerCall = false; isMedicCall = false;
        Engineer_live_Num = 0;
        Medic_live_Num = 0;
    }

    void Update()
    {
        if (!isChecklive)
        {
            StartCoroutine(Check_Live_Unit());
        }

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
        if(Engineer_live_Num == 0)
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
                Engineer_List[last_Engineer].GetComponent<Engineer>().PlayerEngineerCall();
                isEngineerCall = true;
            }
        }
    }
    public void Medic_Call()
    {
        if(Medic_live_Num == 0)
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

            if (!isMedicCall)
            {
                Medic_List[last_Medic].GetComponent<Medic>().PlayerMedicCall();
                isMedicCall = true;
            }
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
                case mercenaryType.Medic :
                    Medic_Num++; break;
            }
        }
        Add_List();
    }

    public void Add_List()
    {
        Engineer_List = new List<GameObject>(Engineer_Num);
        Medic_List = new List<GameObject>(Medic_Num);

        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            switch (Mercenary_List.GetChild(i).GetComponent<Mercenary_Type>().mercenary_type)
            {
                case mercenaryType.Engineer:
                    Engineer_List.Add(Mercenary_List.GetChild(i).gameObject);
                    break;
                case mercenaryType.Medic:
                    Medic_List.Add(Mercenary_List.GetChild(i).gameObject);
                    break;
            }
        }
    }

    IEnumerator Check_Live_Unit()
    {
        isChecklive = true;
        yield return new WaitForSeconds(1);
        int num = 0;

        for(int i= 0; i < Engineer_Num; i++)
        {
            if (Engineer_List[i].GetComponent<Engineer>().Check_Live())
            {
                last_Engineer = i;
                num++;
            }
        }
        Engineer_live_Num = num;

        num = 0;
        for(int j = 0; j < Medic_Num; j++)
        {
            if (Medic_List[j].GetComponent<Medic>().Check_Live())
            {
                last_Medic = j;
                num++;
            }
        }
        Medic_live_Num = num;
        isChecklive = false;
    }
}
