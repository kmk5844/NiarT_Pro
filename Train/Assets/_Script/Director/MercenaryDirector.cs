using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryDirector : MonoBehaviour
{
    public GameDirector gameDirector;
    public SA_MercenaryData mercenaryData;
    public Level_DataTable EX_LevelData;
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

    [Header("미션")]
    public GameObject EscortUnit;
    public bool EscortFlag;
    int EscortHP;
    int EscortArmor;
    int EscortMoveSpeed;

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

/*        if (Input.GetKeyDown(KeyCode.LeftShift) && !isEngineerCall)
        {
            Engineer_Call();
        }*/
    }

    void InGame_SpawnMercenary()
    {
        bool checkFlag;
        for (int i = 0; i < Mercenary_Num.Count; i++)
        {
            checkFlag = false;
            if(Mercenary_Num[i] != -1)
            {
                checkFlag = true;
            }

            if (checkFlag)
            {
                GameObject MercenaryObject = Instantiate(Resources.Load<GameObject>("MercenaryObject/" + Mercenary_Num[i]), Mercenary_List);
                MercenaryObject.name = MercenaryObject.GetComponent<Mercenary>().Type.ToString();
            }
        }

        if (EscortFlag)
        {
            EscortUnit.GetComponent<_Escort>().EscortSetSetting(EscortHP, EscortArmor, EscortMoveSpeed);
            GameObject merobj = Instantiate(EscortUnit, Mercenary_List);
            merobj.name = "Escort";
        }

        Mercenary_Spawn_Flag = true;
    }

    public void Engineer_Call()
    {

    }

    public void Check_List()
    {
        bool checkFlag = false;
        Engineer_List = new List<GameObject>();
        for (int i = 0; i <  Mercenary_List.childCount; i++)
        {
            if(Mercenary_List.GetChild(i).GetComponent<Mercenary>().Type == mercenaryType.Engineer)
            {
                Engineer_Num++;
                Engineer_List.Add(Mercenary_List.GetChild(i).gameObject);
                if (!checkFlag)
                {
                    checkFlag = true;
                }
            }
        }

        if (checkFlag)
        {
            int hp = EX_LevelData.Level_Mercenary_Engineer[mercenaryData.Level_Engineer].Repair_Train_Parsent;
            int coolTime = EX_LevelData.Level_Mercenary_Engineer[mercenaryData.Level_Engineer].Repair_Train_CoolTime;
            gameDirector.EngineerSet(hp, coolTime);
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
    public void Item_Use_Snack()
    {
        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            Mercenary_List.GetChild(i).GetComponent<Mercenary>().Item_Snack();
        }
    }

    public void Item_Use_Fatigue_Reliever(int count, float refreshPercent,int delayTime)
    {
        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            StartCoroutine(Mercenary_List.GetChild(i).GetComponent<Mercenary>().Item_Fatigue_Reliever(count, refreshPercent, delayTime));
        }
    }
    /*  
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
        }*/

    public void SetEscort(int HP, int Armor, int MoveSpeed)
    {
        EscortFlag = true;
        EscortHP = HP;
        EscortArmor = Armor;
        EscortMoveSpeed = MoveSpeed;
    }
}
