using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : Mercenary
{
    Train_InGame train;
    public bool move_Work;
    bool isRepairing;
    float train_HpParsent;

    [SerializeField]
    private float repairDelay;
    [SerializeField]
    private int repairAmount;
    [SerializeField]
    private int move_work_Speed;
    [SerializeField]
    private int repaireTrain_parsent;
    Vector2 Player_X_Position;

    bool TrainSpawnFlag;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        act = Active.move;
        move_Work = true;
        TrainSpawnFlag = false;
    }

    protected override void Update()
    {
        base.Update();
        if (!TrainSpawnFlag)
        {
            TrainSpawnFlag = gameDirector.SpawnTrainFlag;
            train = null;
        }/*else{ // 원래 지나가면서 트레인이 낮은 피라면, 수리로 돌입하는 쪽이였다.
            Debug.DrawRay(rb2D.position, Vector3.down, Color.green);
            RaycastHit2D rayHit = Physics2D.Raycast(rb2D.position, Vector3.down, 1f, LayerMask.GetMask("Platform"));

            if(rayHit.collider != null) // null이 아니라면으로 처리 -> 그렇지 않으면 오류 발생
            {
                train = rayHit.collider.GetComponentInParent<Train_InGame>();
                train_HpParsent = (float)train.Train_HP / (float)train.Max_Train_HP * 100f;
            }
        }*/
        //base.non_combatant_Flip();

        if (Mer_GameType == GameType.Playing || Mer_GameType == GameType.Boss || Mer_GameType == GameType.Refreshing)
        {
            if (HP <= 0 && act != Active.die)
            {
                HP = 0;
                act = Active.die;
                isDying = true;
            }

            if (act == Active.work)
            {
                //트레인이 일정 체력 이상일 때, 전환
                if (train_HpParsent > repaireTrain_parsent)
                {
                    train.isReparing = false;
                    //act = Active.refresh;
                    act = Active.move;
                    move_Work = true;
                }

                //호출을 하지 않았을 때
                if (!move_Work) // 움직이지 않을 때
                {
                    if (!train.isReparing) // 수리 중인 플래그가 꺼져있을 때
                    {
                        train.isReparing = true;
                    }

                    if (!isRepairing) // 자신이 수리 중임
                    {
                        StartCoroutine(Repair());
                    }
                }
            }

            /*if (act == Active.move) // 콜하지 않는 행동, 일하지 않는 행동
            {

                if (train != null) // null이 아닐 때,
                {
                    //트레인 체력이 일정 체력 이하로 떨어졌을 때와, 기차 수리중이 아니라면
                    if (train_HpParsent < repaireTrain_parsent && !train.isReparing)
                    {
                        // 트레인이 0퍼 아닐때와 0퍼일 때
                        if (train_HpParsent != 0)
                        {
                            train.isReparing = true;
                            act = Active.work;
                        }
                        else if (train_HpParsent == 0)
                        {
                            // 트레인이 수리 가능하다면
                            if (train.isRepairable)
                            {
                                train.isRepairable = true;
                                act = Active.work;
                            }
                        }
                    }
                }
            }*/
            if (act == Active.die && isDying)
            {
                if (train.isReparing)
                {
                    train.isReparing = false;
                }
                isDying = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Mer_GameType == GameType.Playing || Mer_GameType == GameType.Boss || Mer_GameType == GameType.Refreshing)
        {
            if (act == Active.move)
            {
                base.Combatant_Move();
            }else if (act == Active.work){
                //기차쪽으로 달려가는
                if (move_Work)
                {
                    if(transform.position.x < train.transform.position.x - 0.2)
                    {
                        Move_X = 1f;
                        rb2D.velocity = new Vector2(Move_X * moveSpeed, rb2D.velocity.y);
                    }else if(transform.position.x > train.transform.position.x + 0.2)
                    {
                        Move_X = -1f;
                        rb2D.velocity = new Vector2(Move_X * moveSpeed, rb2D.velocity.y);
                    }
                    else
                    {
                        rb2D.velocity = Vector2.zero;
                        move_Work = false;
                    }
                }
            }else if(act == Active.die)
            {
                rb2D.velocity = Vector2.zero;
            }
        }
        
        if(Mer_GameType == GameType.Ending)
        {
            act = Active.Game_Wait;
            rb2D.velocity = Vector2.zero;
        }
    }
    public void Level_AddStatus_Engineer(List<Info_Level_Mercenary_Engineer> type, int level)
    {
        repairDelay = type[level].Repair_Delay;
        repairAmount = type[level].Repair_Amount;
        repaireTrain_parsent = type[level].Repair_Train_Parsent;
    }

/*    public void PlayerEngineerCall(Vector3 PlayerCall_XPos)
    {
        if (act == Active.work)
        {
            train.isReparing = false;
        }
        Player_X_Position = new Vector3(PlayerCall_XPos.x, transform.position.y, transform.position.z);
    }*/

    IEnumerator Repair()
    {
        isRepairing = true;
        train.Train_HP += repairAmount;
        yield return new WaitForSeconds(repairDelay);
        isRepairing = false;
    }

/*    public int Check_Work()
    {
        if (act == Active.move)
        {
            return 0;
        }
        else if (act == Active.work)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }*/
}
