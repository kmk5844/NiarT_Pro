using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class MercenaryDirector : MonoBehaviour
{
    public SA_MercenaryData mercenaryData;
    public List<int> Mercenary_Num;

    public Transform Mercenary_List;
    Player player;
    List<GameObject> Engineer_List;
    
    int Engineer_Num;
    int Engineer_live_Num;
    int last_Engineer;

    bool isEngineerCall;
    bool isEngineerCall_Second;
    bool isChecklive;

    public bool Mercenary_Spawn_Flag;
    float Player_X_Pos;

    void Start()
    {
        Mercenary_Spawn_Flag = false;
        Mercenary_Num = mercenaryData.Mercenary_Num;
        InGame_SpawnMercenary();
        Check_List();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        isEngineerCall = false;
        Engineer_live_Num = 0;
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
    }

    void InGame_SpawnMercenary()
    {
        for (int i = 0; i < Mercenary_Num.Count; i++)
        {
            GameObject MercenaryObject = Instantiate(Resources.Load<GameObject>("MercenaryObject/" + Mercenary_Num[i]), Mercenary_List);
            MercenaryObject.name = MercenaryObject.GetComponent<Mercenary>().Type.ToString();
        }
        Mercenary_Spawn_Flag = true;
    }

    public void Engineer_Call()
    {
        if(Engineer_live_Num != 0)
        {
            for (int i = 0; i < Engineer_Num; i++)
            {
                if (Engineer_List[i].GetComponent<Engineer>().Check_Work() == 0)
                {
                    Engineer_List[i].GetComponent<Engineer>().PlayerEngineerCall(player.transform.position);
                    isEngineerCall = true;
                    break;
                }
            }
            if (!isEngineerCall)
            {
                for(int i = 0; i < Engineer_Num; i++)
                {
                    if (Engineer_List[i].GetComponent<Engineer>().Check_Work() == 0 || Engineer_List[i].GetComponent<Engineer>().Check_Work() == 1)
                    {
                        Engineer_List[i].GetComponent<Engineer>().PlayerEngineerCall(player.transform.position);
                        isEngineerCall_Second = true;
                        break;
                    }
                }
                if (!isEngineerCall_Second)
                {
                    Debug.Log("쉬고 있습니다.");
                }
            }
        }
        else
        {
            Debug.Log("호출할 엔지니어가 없습니다.");
        }
    }

    public void Call_End_Engineer()
    {
        isEngineerCall = false;
    }
    public void Check_List()
    {
        for(int i = 0; i <  Mercenary_List.childCount; i++)
        {
            if(Mercenary_List.GetChild(i).GetComponent<Mercenary>().Type == mercenaryType.Engineer)
            {
                Engineer_Num++;
            }
        }
        Add_List();
    }//상호작용 하는 애들은 엔지니어랑 메딕 밖에 없다.

    public void Add_List()
    {
        Engineer_List = new List<GameObject>(Engineer_Num);

        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            if(Mercenary_List.GetChild(i).GetComponent<Mercenary>().Type == mercenaryType.Engineer)
            {
                Engineer_List.Add(Mercenary_List.GetChild(i).gameObject);
            }
        }
    }

    IEnumerator Check_Live_Unit() //die제외한 나머지를 체크한다.
    {
        isChecklive = true;
        yield return new WaitForSeconds(0.1f);
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
        isChecklive = false;
    }

    //아이템부분
    public void Item_Use_Snack(float HPpercent)
    {
        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            Mercenary_List.GetChild(i).GetComponent<Mercenary>().Item_Snack(HPpercent);
        }
    }

    public void Item_Use_Fatigue_Reliever(int count, float refreshPercent,int delayTime)
    {
        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            StartCoroutine(Mercenary_List.GetChild(i).GetComponent<Mercenary>().Item_Fatigue_Reliever(count, refreshPercent, delayTime));
        }
    }

    public void Item_Use_Gloves_Expertise(float refreshPercent, int delayTime)
    {
        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            StartCoroutine(Mercenary_List.GetChild(i).GetComponent<Mercenary>().Item_Gloves_Expertise(refreshPercent, delayTime));
        }
    } 
    
    public void Item_Use_Bear(int workCount, int delayTime)
    {
        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            StartCoroutine(Mercenary_List.GetChild(i).GetComponent<Mercenary>().Item_Bear(workCount, delayTime));
        }
    }
}
