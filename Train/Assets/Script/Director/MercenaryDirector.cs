using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryDirector : MonoBehaviour
{
    public SA_MercenaryData mercenaryData;
    public List<int> Mercenary_Num;

    public Transform Mercenary_List;
    Player player;
    List<GameObject> Engineer_List;
    List<GameObject> Medic_List;
    
    int Engineer_Num;
    int Medic_Num;
    int Engineer_live_Num;
    int Medic_live_Num;
    int last_Engineer;
    int last_Medic;

    public bool isEngineerCall;
    public bool isMedicCall;
    bool isChecklive;

    public bool Mercenary_Spawn_Flag;

    [Header("UI")]
    public Transform Team_1;
    public Transform Team_2;
    public bool Team_Flag;

    float Player_X_Pos;

    void Start()
    {
        Mercenary_Spawn_Flag = false;
        Mercenary_Num = mercenaryData.Mercenary_Num;
        if(Mercenary_Num.Count < 5)
        {
            Team_Flag = false;
        }
        else
        {
            Team_Flag = true;
        }

        for (int i = 0; i < Mercenary_Num.Count; i++)
        {
            GameObject MercenaryObject = Instantiate(Resources.Load<GameObject>("MercenaryObject/" + Mercenary_Num[i] + "_New"), Mercenary_List);
            MercenaryObject.name = MercenaryObject.GetComponent<Mercenary_New>().Type.ToString();
            Spawn_MercenaryUI(MercenaryObject, i, Mercenary_Num[i]);
        }
        Mercenary_Spawn_Flag = true;
        Check_List();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        isEngineerCall = false;
        isMedicCall = false;
        Engineer_live_Num = 0;
        Medic_live_Num = 0;
    }

    void Update()
    {
        if (!isChecklive) //계속 체크해야함.
        {
            StartCoroutine(Check_Live_Unit());
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isEngineerCall)
        {
            Engineer_Call();
        }
        else if(Input .GetKeyDown(KeyCode.E) && !isMedicCall && player.Check_HpParsent() < 70) {
            Medic_Call();
        } // 여기서 수정을 해야함. 70퍼센트 소환은 하지만, 그래도 되지 않는 경우가 발생한다.
    }

    public void Spawn_MercenaryUI(GameObject MercenaryObject, int index, int Merceenary_Num)
    {
        if (!Team_Flag)
        {
            GameObject Mercenary_UI = Instantiate(Resources.Load<GameObject>("InGame_UI/Mercenary"), Team_2);
            Mercenary_UI.GetComponent<Mercenary_UI>().MercenaryObject = MercenaryObject;
            Mercenary_UI.GetComponent<Mercenary_UI>().Mercenary_Num = Merceenary_Num;
        }
        else
        {
            if (index < 4)
            {
                GameObject Mercenary_UI = Instantiate(Resources.Load<GameObject>("InGame_UI/Mercenary"), Team_1);
                Mercenary_UI.GetComponent<Mercenary_UI>().MercenaryObject = MercenaryObject;
                Mercenary_UI.GetComponent<Mercenary_UI>().Mercenary_Num = Merceenary_Num;
            }
            else
            {
                GameObject Mercenary_UI = Instantiate(Resources.Load<GameObject>("InGame_UI/Mercenary"), Team_2);
                Mercenary_UI.GetComponent<Mercenary_UI>().MercenaryObject = MercenaryObject;
                Mercenary_UI.GetComponent<Mercenary_UI>().Mercenary_Num = Merceenary_Num;
            }
        }
    }

    public void Engineer_Call()
    {
        if(Engineer_live_Num != 0)
        {
            for (int i = 0; i < Engineer_Num; i++)
            {
                if (Engineer_List[i].GetComponent<Engineer>().Check_Work())
                {
                    Engineer_List[i].GetComponent<Engineer>().PlayerEngineerCall(player.transform.position);
                    isEngineerCall = true;
                    break;
                } //있으면 오라고 콜함, 없으면 다음
            }
            if(!isEngineerCall)
            {
                Engineer_List[last_Engineer].GetComponent<Engineer>().PlayerEngineerCall(player.transform.position);
                isEngineerCall = true;
            }
        }
        else
        {
            //엔지니어가 없음을 알림.
        }
    }
    public void Medic_Call()
    {
        if (Medic_live_Num != 0)
        {
            for (int i = 0; i < Medic_Num; i++)
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
        else
        {
            //메딕이 없음을 알림.
        }
    }

    public void Call_End(mercenaryType type)
    {
        switch (type)
        {
            case mercenaryType.Engineer:
                isEngineerCall = false;
                break;
            case mercenaryType.Medic:
                isMedicCall = false;
                break;  
        }
    }
    public void Check_List()
    {
        for(int i = 0; i <  Mercenary_List.childCount; i++)
        {
            switch (Mercenary_List.GetChild(i).GetComponent<Mercenary_New>().Type)
            {
                case mercenaryType.Engineer:
                    Engineer_Num++;
                    break;
                case mercenaryType.Medic :
                    Medic_Num++; 
                    break;
            }
        }
        Add_List();
    }//상호작용 하는 애들은 엔지니어랑 메딕 밖에 없다.

    public void Add_List()
    {
        Engineer_List = new List<GameObject>(Engineer_Num);
        Medic_List = new List<GameObject>(Medic_Num);

        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            switch (Mercenary_List.GetChild(i).GetComponent<Mercenary_New>().Type)
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

    IEnumerator Check_Live_Unit() //die제외한 나머지를 체크한다.
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
