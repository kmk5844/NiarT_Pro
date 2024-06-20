using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary_New : MonoBehaviour
{
    public mercenaryType Type;
    protected GameType Mer_GameType;
    protected GameDirector gameDirector;
    protected MercenaryDirector mercenaryDirector;
    protected SA_MercenaryData SA_MercenaryData;
    protected Level_DataTable EX_Level_Data;

    Transform Unit_Scale;
    protected float Unit_Scale_X;
    protected float Unit_Scale_Y;
    protected float Unit_Scale_Z;

    protected Transform Train_List;
    int TrainCount;
    protected float Move_X;
    protected float MinMove_X;
    protected float MaxMove_X;
    protected float Move_Y;

    [Header("�뺴 ����")]
    [SerializeField]
    protected Active act;
    public int HP;
    [HideInInspector] 
    public int MaxHP;
    protected float moveSpeed;
    protected Rigidbody2D rb2D;
    public float Refresh_Delay;

    //���� �뺴 Move
    bool isCombatantWalking;
    bool isCombatantIdling;
    float Combatant_Idle_LastTime;
    int Combatant_Move_x;
    Vector2 Combatant_BeforePosition;

    //�޽�
    bool isRefreshing;
    protected bool isDying;


    [Header("���� ����")]
    int def;
    float era;
    float def_constant;

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
        Move_X = 1f;
        MaxMove_X = 4f;
        MinMove_X = -10.94f * (TrainCount - 1) - 5f;
        Move_Y = -0.95f;
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


        if (!isCombatantWalking && !isCombatantIdling) //���� ���� �Ÿ� ���
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
        else if (isCombatantWalking) // �Ȱ����� ��
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
        // �������� ������ ���ڴٴ� ��ǥ���� �ǰ�
        yield return new WaitForSeconds(Refresh_Delay);
        act = Active.move;
        isRefreshing = false;
    }

    protected IEnumerator Revive(int Heal_HpParsent)
    {
        act = Active.revive;
        HP = MaxHP * Heal_HpParsent / 100;
        Debug.Log("��Ȱ : " + gameObject.name);
        yield return new WaitForSeconds(4);
        act = Active.move;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            int damageTaken = Mathf.RoundToInt(collision.GetComponent<Bullet>().atk * era);
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
                def = EX_Level_Data.Level_Mercenary_Engine_Driver[SA_MercenaryData.Level_Engine_Driver].Def;
                GetComponent<Engine_Driver_New>().Level_AddStatus_Engine_Driver(EX_Level_Data.Level_Mercenary_Engine_Driver, SA_MercenaryData.Level_Engine_Driver);
                break;
            case mercenaryType.Engineer:
                HP = EX_Level_Data.Level_Mercenary_Engineer[SA_MercenaryData.Level_Engineer].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Engineer[SA_MercenaryData.Level_Engineer].MoveSpeed;
                def = EX_Level_Data.Level_Mercenary_Engineer[SA_MercenaryData.Level_Engineer].Def;
                GetComponent<Engineer>().Level_AddStatus_Engineer(EX_Level_Data.Level_Mercenary_Engineer, SA_MercenaryData.Level_Engineer);
                break;
            case mercenaryType.Long_Ranged:
                HP = EX_Level_Data.Level_Mercenary_Long_Ranged[SA_MercenaryData.Level_Long_Ranged].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Long_Ranged[SA_MercenaryData.Level_Long_Ranged].MoveSpeed;
                def = EX_Level_Data.Level_Mercenary_Long_Ranged[SA_MercenaryData.Level_Long_Ranged].Def;
                GetComponent<Long_Ranged>().Level_AddStatus_LongRanged(EX_Level_Data.Level_Mercenary_Long_Ranged, SA_MercenaryData.Level_Long_Ranged);
                break;
            case mercenaryType.Short_Ranged:
                HP = EX_Level_Data.Level_Mercenary_Short_Ranged[SA_MercenaryData.Level_Short_Ranged].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Short_Ranged[SA_MercenaryData.Level_Short_Ranged].MoveSpeed;
                def = EX_Level_Data.Level_Mercenary_Short_Ranged[SA_MercenaryData.Level_Short_Ranged].Def;
                GetComponent<Short_Ranged>().Level_AddStatus_ShortRanged(EX_Level_Data.Level_Mercenary_Short_Ranged, SA_MercenaryData.Level_Short_Ranged);
                break;
            case mercenaryType.Medic:
                HP = EX_Level_Data.Level_Mercenary_Medic[SA_MercenaryData.Level_Medic].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Medic[SA_MercenaryData.Level_Medic].MoveSpeed;
                def = EX_Level_Data.Level_Mercenary_Medic[SA_MercenaryData.Level_Medic].Def;
                GetComponent<Medic>().Level_AddStatus_Medic(EX_Level_Data.Level_Mercenary_Medic, SA_MercenaryData.Level_Medic);
                break;
            case mercenaryType.Bard:
                HP = EX_Level_Data.Level_Mercenary_Bard[SA_MercenaryData.Level_Bard].HP;
                moveSpeed = EX_Level_Data.Level_Mercenary_Bard[SA_MercenaryData.Level_Bard].MoveSpeed;
                def = EX_Level_Data.Level_Mercenary_Bard[SA_MercenaryData.Level_Bard].Def;
                GetComponent<Bard_New>().Level_AddStatus_Bard(EX_Level_Data.Level_Mercenary_Bard, SA_MercenaryData.Level_Bard);
                break;
        }
    }
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