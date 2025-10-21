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
    bool isChecklive;

    public bool Mercenary_Spawn_Flag;
    float Player_X_Pos;

    List<Train_InGame> Train_List;
    bool checkActEngineer;

    [Header("미션")]
    public GameObject EscortUnit;
    public bool EscortFlag;
    int EscortHP;
    int EscortArmor;
    int EscortMoveSpeed;

    private void Awake()
    {
        EscortFlag = false;
    }

    void Start()
    {
        Mercenary_Spawn_Flag = false;
        Mercenary_Num = mercenaryData.Mercenary_Num;
        InGame_SpawnMercenary();
        Check_List();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Train_List = new List<Train_InGame>();
    }

    void Update()
    {
        if (!checkActEngineer && Train_List.Count > 0)
        {
            CheckEngineer_Check();
        }
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

    public void Engineer_Call(Train_InGame train)
    {
        Train_List.Add(train); 
        //트레인 리스트가 남아있다면, 반복적으로 check하여 트레인에 할당이 될 수 있도록 작업
    }

    void CheckEngineer_Check()
    {
        checkActEngineer = true;
        for (int i = 0; i < Engineer_List.Count; i++)
        {
            bool checkFlag = Engineer_List[i].GetComponent<Engineer>().Check_Work(); //checkFlag = move
            if (checkFlag)
            {
                Engineer_List[i].GetComponent<Engineer>().Set_Train(Train_List[0]);
                Train_List.RemoveAt(0); //트레인 리스트에서 제거
                checkActEngineer = false;
                return;
            }
        }
        checkActEngineer = false;
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
            int W_hp = EX_LevelData.Level_Mercenary_Engineer[mercenaryData.Level_Engineer].Repair_Train_WarningParsent;
            int M_hp = EX_LevelData.Level_Mercenary_Engineer[mercenaryData.Level_Engineer].Repair_Train_MaxHpPersent;
            int coolTime = EX_LevelData.Level_Mercenary_Engineer[mercenaryData.Level_Engineer].Repair_Train_CoolTime;
            gameDirector.EngineerSet(W_hp, M_hp, coolTime);
        }
    }

    //아이템부분
    public void Item_Use_Snack()
    {
        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            Mercenary_List.GetChild(i).GetComponent<Mercenary>().Item_Snack();
        }
    }

    public void Item_Use_Fatigue_Reliever()
    {
        for (int i = 0; i < Mercenary_List.childCount; i++)
        {
            Mercenary_List.GetChild(i).GetComponent<Mercenary>().Item_Fatigue_Reliever();
        }
    }

    public void SetEscort(int HP, int Armor, int MoveSpeed)
    {
        EscortFlag = true;
        EscortHP = HP;
        EscortArmor = Armor;
        EscortMoveSpeed = MoveSpeed;
    }
}
