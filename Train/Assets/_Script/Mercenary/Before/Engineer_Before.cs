/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer_Before : Mercenary_Before
{
    MercenaryDirector mercenaryDirector;
    Transform player;

    Rigidbody2D rigid;
    Train_InGame train;
    bool move_Work;
    bool isRepairing;
    public bool isCalling;
    float train_HpParsent;
    [Header("수리 속도 및 기차 수리량")]
    [SerializeField] private int repairDelay;
    [SerializeField] private int repairAmount;
    [Header("수리하러 갈때의 움직임")]
    [SerializeField] private int move_work_speed;
    [Header("OO%이하의 기차인 경우 수리")]
    [SerializeField] private int repairTrain_Parsent;
    Vector3 Player_X_Position;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        Type = mercenaryType.Engineer;
        act = Active.move;
        rigid = GetComponent<Rigidbody2D>();
        move_Work = true;
        isCalling = false;
        mercenaryDirector = GameObject.Find("MercenaryDirector").GetComponent<MercenaryDirector>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Debug.DrawRay(rigid.position, Vector3.down, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Platform"));

        train = rayHit.collider.GetComponentInParent<Train_InGame>();
        train_HpParsent = (float)train.cur_HP / (float)train.Train_HP * 100f;

        Check_GameType();

        if (move_X > 0)
        {
            Unit_Scale.localScale = new Vector3(-Unit_Scale_X, Unit_Scale_Y, Unit_Scale_Z);
        }
        else
        {
            Unit_Scale.localScale = new Vector3(Unit_Scale_X, Unit_Scale_Y, Unit_Scale_Z);
        }

        if (M_gameType == GameType.Playing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
            else if (act != Active.work *//*&& Stamina >= 50*//* && train_HpParsent < repairTrain_Parsent && !train.isReparing && act != Active.call && act != Active.die && train_HpParsent != 0)
            {
                train.isReparing = true;
                act = Active.work;
            }
            else if (act != Active.work *//*&& Stamina >= 50*//* && train_HpParsent < repairTrain_Parsent && !train.isReparing && act != Active.call && act != Active.die && train_HpParsent == 0)
            {
                if (train.isRepairable)
                {
                    train.isReparing = true;
                    act = Active.work;
                }
            }
            *//*else if (Stamina <= 0)
            {
                train.isReparing = false;
                act = Active.weak;
                move_Work = true;
            }*//*
            else if (act == Active.work && train_HpParsent > repairTrain_Parsent)
            {
                train.isReparing = false;
                act = Active.move;
                move_Work = true;
            }

            if (act == Active.work && !isCalling)
            {
                if (!move_Work)
                {
                    if (!train.isReparing)
                    {
                        train.isReparing = true;
                    }
                    if (!isRepairing)
                    {
                        StartCoroutine(Repair());
                    }
                }
            }
            else if (act == Active.die && isDying)
            {
                //Debug.Log("여기서 애니메이션 구현한다!");
                if (train.isReparing)
                {
                    train.isReparing = false;
                }
                isDying = false;
            }
            else if (act == Active.refresh)
            {
                *//*if (!isRefreshing_weak)
                {
                    StartCoroutine(Refresh_Weak());
                }
                else if (Stamina >= 70)
                {
                    act = Active.move;
                }*//*
            }
            
        }
    }

    void FixedUpdate()
    {
        if (M_gameType == GameType.Playing)
        {
            if (act == Active.move)
            {
                base.non_combatant_Move();
            }
            else if (act == Active.work && !isCalling)
            {
                if (move_Work)
                {
                    if (transform.position.x < train.transform.position.x - 0.2)
                    {
                        move_X = 1f;
                        rb2D.velocity = new Vector2(move_X * moveSpeed, rb2D.velocity.y);
                    }
                    else if (transform.position.x > train.transform.position.x + 0.2)
                    {
                        move_X = -1f;
                        rb2D.velocity = new Vector2(move_X * moveSpeed, rb2D.velocity.y);
                    }
                    else
                    {
                        move_Work = false;
                    }
                }
            }
            else if (act == Active.call)
            {
                isCalling = true;
                if (transform.position.x < Player_X_Position.x - 0.5)
                {
                    move_X = 1f;
                    rb2D.velocity = new Vector2(move_X * 6f, rb2D.velocity.y);

                }
                else if (transform.position.x > Player_X_Position.x + 0.5)
                {
                    move_X = -1f;
                    rb2D.velocity = new Vector2(move_X * 6f, rb2D.velocity.y);

                }
                else
                {
                    isCalling = false;
                    act = Active.move;
                    //mercenaryDirector.Call_End(mercenaryType.Engineer);
                }
            }
            else if (act == Active.die)
            {
                rb2D.velocity = Vector2.zero;
            }
        }
        else if (M_gameType == GameType.Ending)
        {
            act = Active.Game_Wait;
            rb2D.velocity = Vector2.zero;
        }
    }
    public void Level_AddStatus_Engineer(List<Info_Level_Mercenary_Engineer> type, int level)
    {
        repairDelay = type[level].Repair_Delay;
        repairAmount = type[level].Repair_Amount;
        repairTrain_Parsent = type[level].Repair_Train_Parsent;
    }

    public void PlayerEngineerCall(Vector3 PlayerCall_XPos)
    {
        if(act == Active.work)
        {
            train.isReparing = false;
        }
        Player_X_Position = new Vector3(PlayerCall_XPos.x, transform.position.y, transform.position.z);
        act = Active.call;
    }

    IEnumerator Repair()
    {
        isRepairing = true;

        yield return new WaitForSeconds(repairDelay);
*//*        if (Stamina - useStamina < 0)
        {
            Stamina = 0;
        }
        else
        {
            Stamina -= useStamina;
        }*//*
        train.cur_HP += repairAmount;
        isRepairing = false;
    }
}*/