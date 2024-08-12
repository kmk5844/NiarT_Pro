using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mercenary : MonoBehaviour
{
    public mercenaryType Type;
    protected GameType Mer_GameType;
    protected GameDirector gameDirector;
    protected MercenaryDirector mercenaryDirector;
    protected SA_MercenaryData SA_MercenaryData;
    protected Level_DataTable EX_Level_Data;

    protected Transform Unit_Scale;
    protected float Unit_Scale_X;
    protected float Unit_Scale_Y;
    protected float Unit_Scale_Z;

    protected Transform Train_List;
    int TrainCount;
    protected float Move_X;
    protected float MinMove_X;
    protected float MaxMove_X;
    protected float Move_Y;

    [Header("용병 정보")]
    [SerializeField]
    protected Active act;
    public int HP;
    [HideInInspector] 
    public int MaxHP;
    [HideInInspector]
    public int atk;
    protected float moveSpeed;
    protected Rigidbody2D rb2D;
    float refreshStartTime;
    float Refresh_Delay;
    float Min_Refresh_Delay;
    float Max_Refresh_Delay;
    protected int workCount;
    [SerializeField]
    protected int Max_workCount;

    //전투 용병 Move
    bool isCombatantWalking;
    bool isCombatantIdling;
    float Combatant_Idle_LastTime;
    int Combatant_Move_x;
    Vector2 Combatant_BeforePosition;

    //휴식
    protected bool isRefreshing;
    protected bool isDying;
    public bool isHealWithMedic;

    int def;
    float era;
    float def_constant;

    [Header("UI")]
    public GameObject HP_Waring_Object;
    public Image HP_Guage;
    public GameObject CoolTime_Guage_Object;
    public Image CoolTime_Guage;

    //Item부분
    protected int Item_workCount_UP;
    protected float Item_Refresh_Delay;
    protected float Item_Refresh_DelayPercent;

    protected virtual void Awake()
    {
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        mercenaryDirector = GameObject.Find("MercenaryDirector").GetComponent<MercenaryDirector>();
        SA_MercenaryData = gameDirector.SA_MercenaryData;
        EX_Level_Data = gameDirector.EX_LevelData;
        Train_List = gameDirector.Train_List;
        Data_Index();
    }

    protected virtual void Start()
    {
        TrainCount = Train_List.childCount;
        rb2D = GetComponent<Rigidbody2D>();
        Refresh_Delay = 0f;
        Item_Refresh_Delay = 0f;
        Item_workCount_UP = 0;
        Move_X = 1f;
        MaxMove_X = 3f;
        MinMove_X = (-4.97f + (-10.94f * (TrainCount - 1)));
        Move_Y = -0.94f;
        transform.position = new Vector2(Random.Range(MinMove_X, MaxMove_X), Move_Y);
        Unit_Scale = transform.GetChild(0);
        Unit_Scale_X = Unit_Scale.localScale.x;
        Unit_Scale_Y = Unit_Scale.localScale.y;
        Unit_Scale_Z = Unit_Scale.localScale.z;
        def_constant = 100;
        era = 1f - (float)def / def_constant;
        MaxHP = HP;

        isCombatantWalking = false;
        isCombatantIdling = false;
        isRefreshing = false;

        HP_Waring_Object.SetActive(false);
        HP_Guage.fillAmount = 1f;
        CoolTime_Guage_Object.SetActive(false);
        CoolTime_Guage.fillAmount = 0f;
    }

    protected virtual void Update()
    {
        Check_GameType();

        if (Check_HpParsent() < 30)
        {
            HP_Waring_Object.SetActive(true);
        }
        else
        {
            HP_Waring_Object.SetActive(false);
        }

        if (isRefreshing)
        {
            float elpsedTime = Time.time - refreshStartTime;
            float totalDuration = Mathf.Clamp(refreshStartTime + (Refresh_Delay - Item_Refresh_Delay), refreshStartTime, float.MaxValue) - refreshStartTime;
            float currentFillAmount = Mathf.Lerp(0, 1, Mathf.Clamp01(elpsedTime / totalDuration));
            CoolTime_Guage.fillAmount = currentFillAmount;
        }

        HP_Guage.fillAmount = Check_HpParsent() / 100f;
    }

    protected void Combatant_Move()
    {
        if (Move_X > 0)
        {
            Unit_Scale.localScale = new Vector3(-Unit_Scale_X, Unit_Scale_Y, Unit_Scale_Z);
        }
        else
        {
            Unit_Scale.localScale = new Vector3(Unit_Scale_X, Unit_Scale_Y, Unit_Scale_Z);
        }

        if (!isCombatantWalking && !isCombatantIdling) //어디로 갈지 거리 계산
        {
            if (transform.position.x > MaxMove_X - 7)
            {
                Combatant_Move_x = Random.Range(-6, 0);
            }
            else if (transform.position.x < MinMove_X + 7)
            {
                Combatant_Move_x = Random.Range(0, 6);
            }
            else
            {
                Combatant_Move_x = Random.Range(-3, 3);
            }

            if (Combatant_Move_x > 0)
            {
                Move_X = 1f;
            }
            else if (Combatant_Move_x < 0)
            {
                Move_X = -1f;
            }
            else if (Combatant_Move_x == 0 || Combatant_Move_x == 1)
            {
                Combatant_Move_x += 1;
                Move_X = 1f;
            }
            else if (Combatant_Move_x == -1)
            {
                Combatant_Move_x -= 1;
                Move_X = 1f;
            }

            Combatant_BeforePosition = transform.position;
            isCombatantWalking = true;
        }
        else if (isCombatantWalking) // 걷고있을 때
        {
            if (Combatant_Move_x > 0 && transform.position.x > Combatant_BeforePosition.x + Combatant_Move_x)
            {
                isCombatantWalking = false;
            }
            else if (Combatant_Move_x < 0 && transform.position.x < Combatant_BeforePosition.x + Combatant_Move_x)
            {
                isCombatantWalking = false;
            }
            else if (transform.position.x < MinMove_X)
            {
                isCombatantWalking = false;
            }
            else if (transform.position.x > MaxMove_X)
            {
                isCombatantWalking = false;
            }

            if (isCombatantWalking)
            {
                rb2D.velocity = new Vector2(Move_X * moveSpeed, rb2D.velocity.y);
            }
            else
            {
                Combatant_Idle();
            }
        }
        else if (!isCombatantWalking && isCombatantIdling)
        {
            rb2D.velocity = Vector2.zero;
            Combatant_Idle();
        }
        else if (isCombatantIdling && (transform.position.x < MinMove_X || transform.position.x > MaxMove_X))
        {
            if (transform.position.x < MinMove_X)
            {
                Move_X = 1f;
            }
            else if (transform.position.x > MaxMove_X)
            {
                Move_X = -1f;
            }
            rb2D.velocity = new Vector2(Move_X * moveSpeed, rb2D.velocity.y);
        }
    }
    protected void Combatant_Idle()
    {
        if (!isCombatantIdling)
        {
            isCombatantIdling = true;
            Combatant_Idle_LastTime = Time.time;
        }
        else if (Time.time >= Combatant_Idle_LastTime + Random.Range(1, 4))
        {
            isCombatantIdling = false;
        }
    }

    protected void non_combatant_Flip()
    {
        if (Move_X > 0)
        {
            Unit_Scale.localScale = new Vector3(-Unit_Scale_X, Unit_Scale_Y, Unit_Scale_Z);
        }
        else
        {
            Unit_Scale.localScale = new Vector3(Unit_Scale_X, Unit_Scale_Y, Unit_Scale_Z);
        }

    }

    protected void non_combatant_Move()
    {
        non_combatant_Flip();
        if (transform.position.x > MaxMove_X)
        {
            Move_X = -1f;
        }
        else if (transform.position.x < MinMove_X)
        {
            Move_X = 1f;
        }

        rb2D.velocity = new Vector2(Move_X * moveSpeed, rb2D.velocity.y);
    }

    protected IEnumerator Refresh()
    {
        isRefreshing = true;
        CoolTime_Guage.fillAmount = 0f;
        CoolTime_Guage_Object.SetActive(true);
        refreshStartTime = Time.time;
        Refresh_Delay = Random.Range(Min_Refresh_Delay, Max_Refresh_Delay);
        rb2D.velocity = Vector2.zero;

        if (Item_Refresh_DelayPercent == 0)
        {
            Item_Refresh_Delay = 0;
        }
        else
        {
            Item_Refresh_Delay = Refresh_Delay * (Item_Refresh_DelayPercent / 100f);
        }
        yield return new WaitForSeconds(Refresh_Delay - Item_Refresh_Delay);
        workCount = 0;
        CoolTime_Guage_Object.SetActive(false);
        act = Active.move;
        isRefreshing = false;
    }

    protected IEnumerator Revive(int Heal_HpParsent)
    {
        act = Active.revive;
        HP = MaxHP * Heal_HpParsent / 100;
        Debug.Log("부활 : " + gameObject.name);
        yield return new WaitForSeconds(4);
        act = Active.move;
    }

    public void workCountUP()
    {
        workCount++;
    }

    public float Check_MoveX()
    {
        return Move_X;
    }

    public bool Check_Live()
    {
        if(act == Active.die)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void Check_GameType()
    {
        if(Mer_GameType != gameDirector.gameType)
        {
            Mer_GameType = gameDirector.gameType;
        }
    }

    public float Check_HpParsent()
    {
        return (float)HP / (float)MaxHP * 100f;
    }

    public void Mer_Buff_HP(int Buff_HP, bool flag)
    {
        if (flag)
        {
            HP += Buff_HP;
            MaxHP += Buff_HP;
        }
        else
        {
            HP -= Buff_HP;
            MaxHP -= Buff_HP;
        }
    }

    public void Mer_Buff_Def(int buff_Def, bool flag)
    {
        if (flag)
        {
            def += buff_Def;
        }
        else
        {
            def -= buff_Def;
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            int damageTaken = Mathf.RoundToInt(collision.GetComponent<MonsterBullet>().atk * era);
            if (HP - damageTaken < 0)
            {
                HP = 0;
                act = Active.die;
            }
            else
            {
                HP -= damageTaken;
            }
            Destroy(collision.gameObject);
        }
    }

    public Active mercenaryActive_Check()
    {
        return act;
    }

    public void mercenaryActive_Change(Active killZone_act)
    {
        act = killZone_act;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(MinMove_X, -1.3f, 0), new Vector3(MaxMove_X, -1.3f, 0));
    }

    public void Data_Index()
    {
        switch (Type)
        {
            case mercenaryType.Engine_Driver:
                HP = EX_Level_Data.Level_Mercenary_Engine_Driver[SA_MercenaryData.Level_Engine_Driver].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Engine_Driver[SA_MercenaryData.Level_Engine_Driver].MoveSpeed;
                Max_workCount = EX_Level_Data.Level_Mercenary_Engine_Driver[SA_MercenaryData.Level_Engine_Driver].Max_WorkCount;
                Min_Refresh_Delay = EX_Level_Data.Level_Mercenary_Engine_Driver[SA_MercenaryData.Level_Engine_Driver].Min_Refresh_Delay;
                Max_Refresh_Delay = EX_Level_Data.Level_Mercenary_Engine_Driver[SA_MercenaryData.Level_Engine_Driver].Max_Refresh_Delay;
                def = EX_Level_Data.Level_Mercenary_Engine_Driver[SA_MercenaryData.Level_Engine_Driver].Def;
                GetComponent<Engine_Driver>().Level_AddStatus_Engine_Driver(EX_Level_Data.Level_Mercenary_Engine_Driver, SA_MercenaryData.Level_Engine_Driver);
                break;
            case mercenaryType.Engineer:
                HP = EX_Level_Data.Level_Mercenary_Engineer[SA_MercenaryData.Level_Engineer].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Engineer[SA_MercenaryData.Level_Engineer].MoveSpeed;
                Max_workCount = EX_Level_Data.Level_Mercenary_Engineer[SA_MercenaryData.Level_Engineer].Max_WorkCount;
                Min_Refresh_Delay = EX_Level_Data.Level_Mercenary_Engineer[SA_MercenaryData.Level_Engineer].Min_Refresh_Delay;
                Max_Refresh_Delay = EX_Level_Data.Level_Mercenary_Engineer[SA_MercenaryData.Level_Engineer].Max_Refresh_Delay;
                def = EX_Level_Data.Level_Mercenary_Engineer[SA_MercenaryData.Level_Engineer].Def;
                GetComponent<Engineer>().Level_AddStatus_Engineer(EX_Level_Data.Level_Mercenary_Engineer, SA_MercenaryData.Level_Engineer);
                break;
            case mercenaryType.Long_Ranged:
                HP = EX_Level_Data.Level_Mercenary_Long_Ranged[SA_MercenaryData.Level_Long_Ranged].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Long_Ranged[SA_MercenaryData.Level_Long_Ranged].MoveSpeed;
                Max_workCount = EX_Level_Data.Level_Mercenary_Long_Ranged[SA_MercenaryData.Level_Long_Ranged].Max_WorkCount;
                Min_Refresh_Delay = EX_Level_Data.Level_Mercenary_Long_Ranged[SA_MercenaryData.Level_Long_Ranged].Min_Refresh_Delay;
                Max_Refresh_Delay = EX_Level_Data.Level_Mercenary_Long_Ranged[SA_MercenaryData.Level_Long_Ranged].Max_Refresh_Delay;
                def = EX_Level_Data.Level_Mercenary_Long_Ranged[SA_MercenaryData.Level_Long_Ranged].Def;
                GetComponent<Long_Ranged>().Level_AddStatus_LongRanged(EX_Level_Data.Level_Mercenary_Long_Ranged, SA_MercenaryData.Level_Long_Ranged);
                break;
            case mercenaryType.Short_Ranged:
                HP = EX_Level_Data.Level_Mercenary_Short_Ranged[SA_MercenaryData.Level_Short_Ranged].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Short_Ranged[SA_MercenaryData.Level_Short_Ranged].MoveSpeed;
                Max_workCount = EX_Level_Data.Level_Mercenary_Short_Ranged[SA_MercenaryData.Level_Short_Ranged].Max_WorkCount;
                Min_Refresh_Delay = EX_Level_Data.Level_Mercenary_Short_Ranged[SA_MercenaryData.Level_Short_Ranged].Min_Refresh_Delay;
                Max_Refresh_Delay = EX_Level_Data.Level_Mercenary_Short_Ranged[SA_MercenaryData.Level_Short_Ranged].Max_Refresh_Delay;
                def = EX_Level_Data.Level_Mercenary_Short_Ranged[SA_MercenaryData.Level_Short_Ranged].Def;
                GetComponent<Short_Ranged>().Level_AddStatus_ShortRanged(EX_Level_Data.Level_Mercenary_Short_Ranged, SA_MercenaryData.Level_Short_Ranged);
                break;
            case mercenaryType.Medic:
                HP = EX_Level_Data.Level_Mercenary_Medic[SA_MercenaryData.Level_Medic].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Medic[SA_MercenaryData.Level_Medic].MoveSpeed;
                Max_workCount = EX_Level_Data.Level_Mercenary_Medic[SA_MercenaryData.Level_Medic].Max_WorkCount;
                Min_Refresh_Delay = EX_Level_Data.Level_Mercenary_Medic[SA_MercenaryData.Level_Medic].Min_Refresh_Delay;
                Max_Refresh_Delay = EX_Level_Data.Level_Mercenary_Medic[SA_MercenaryData.Level_Medic].Max_Refresh_Delay;
                def = EX_Level_Data.Level_Mercenary_Medic[SA_MercenaryData.Level_Medic].Def;
                GetComponent<Medic>().Level_AddStatus_Medic(EX_Level_Data.Level_Mercenary_Medic, SA_MercenaryData.Level_Medic);
                break;
            case mercenaryType.Bard:
                HP = EX_Level_Data.Level_Mercenary_Bard[SA_MercenaryData.Level_Bard].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Bard[SA_MercenaryData.Level_Bard].MoveSpeed;
                Max_workCount = EX_Level_Data.Level_Mercenary_Bard[SA_MercenaryData.Level_Bard].Max_WorkCount;
                Min_Refresh_Delay = EX_Level_Data.Level_Mercenary_Bard[SA_MercenaryData.Level_Bard].Min_Refresh_Delay;
                Max_Refresh_Delay = EX_Level_Data.Level_Mercenary_Bard[SA_MercenaryData.Level_Bard].Max_Refresh_Delay;
                def = EX_Level_Data.Level_Mercenary_Bard[SA_MercenaryData.Level_Bard].Def;
                GetComponent<Bard>().Level_AddStatus_Bard(EX_Level_Data.Level_Mercenary_Bard, SA_MercenaryData.Level_Bard); //특수 스탯
                break;
            case mercenaryType.CowBoy:
                HP = EX_Level_Data.Level_Mercenary_CowBoy[SA_MercenaryData.Level_CowBoy].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_CowBoy[SA_MercenaryData.Level_CowBoy].MoveSpeed;
                Max_workCount = EX_Level_Data.Level_Mercenary_CowBoy[SA_MercenaryData.Level_CowBoy].Max_WorkCount;
                Min_Refresh_Delay = EX_Level_Data.Level_Mercenary_CowBoy[SA_MercenaryData.Level_CowBoy].Min_Refresh_Delay;
                Max_Refresh_Delay = EX_Level_Data.Level_Mercenary_CowBoy[SA_MercenaryData.Level_CowBoy].Max_Refresh_Delay;
                def = EX_Level_Data.Level_Mercenary_CowBoy[SA_MercenaryData.Level_CowBoy].Def;
                //카우보이의 개별적인 특수 스탯이 없음.
                break;
        }
    }

    //Item부분
    public void Item_Snack(float HPpercent)
    {
        HP += (int)(MaxHP * (HPpercent / 100f));
    }

    public IEnumerator Item_Fatigue_Reliever(int workcount, float refreshPercent, int delayTime)
    {
        Item_workCount_UP = workcount;
        Item_Refresh_DelayPercent = refreshPercent;
        yield return new WaitForSeconds(delayTime);
        Item_workCount_UP = 0;
        Item_Refresh_DelayPercent = 0;
    }

    public IEnumerator Item_Gloves_Expertise(float refreshPercent, int delayTime)
    {
        Item_Refresh_DelayPercent = refreshPercent;
        yield return new WaitForSeconds(delayTime);
        Item_Refresh_DelayPercent = 0;
    }

    public IEnumerator Item_Bear(int count, int delayTime)
    {
        Item_workCount_UP = count;
        yield return new WaitForSeconds(delayTime);
        Item_workCount_UP = 0;
    }
}
public enum Active
{
    move,   // 이동
    work,   // 작업
    die,    // 죽음
    revive, // 부활
    refresh,   //휴식
    call,   //플레이어 호출
    Game_Wait, // 인게임의 기다림 
    //플레이어 상호작용 추가하면 -> 플레이어 향해 가서 상호작용 한다
}

public enum mercenaryType
{
    Engineer,
    Long_Ranged,
    Short_Ranged,
    Medic,
    Engine_Driver,
    Bard,
    CowBoy,
}