using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class Engineer : Mercenary
{
    Rigidbody2D rigid;
    Train_InGame train;
    public bool move_Work;
    bool isRepairing;
    public bool isCalling;
    float train_HpParsent;

    [SerializeField]
    private int repairDelay;
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
        rigid = GetComponent<Rigidbody2D>();
        move_Work = true;
        TrainSpawnFlag = false;
        isCalling = false;
    }

    protected override void Update()
    {
        base.Update();
        if (!TrainSpawnFlag)
        {
            TrainSpawnFlag = gameDirector.SpawnTrainFlag;
        }else{
            Debug.DrawRay(rigid.position, Vector3.down, Color.green);
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Platform"));

            train = rayHit.collider.GetComponentInParent<Train_InGame>();
            train_HpParsent = (float)train.Train_HP / (float)train.Max_Train_HP * 100f;
        }

        base.non_combatant_Flip();

        if (Mer_GameType == GameType.Playing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
            if (act == Active.move) // ������ �ʴ� �ൿ, ������ �ʴ� �ൿ
            {
                if(train != null)
                {
                    //Ʈ���� ü���� ���� ü�� ���Ϸ� �������� ����, ���� �������� �ƴ϶��
                    if (train_HpParsent < repaireTrain_parsent && !train.isReparing)
                    {
                        // Ʈ������ 0�� �ƴҶ��� 0���� ��
                        if (train_HpParsent != 0)
                        {
                            train.isReparing = true;
                            act = Active.work;
                        }
                        else if (train_HpParsent == 0)
                        {
                            // Ʈ������ ���� �����ϴٸ�
                            if (train.isRepairable)
                            {
                                train.isRepairable = true;
                                act = Active.work;
                            }
                        }
                    }
                }
            }
            else if (act == Active.work)
            {
                //Ʈ������ ���� ü�� �̻��� ��, ��ȯ
                if (train_HpParsent > repaireTrain_parsent)
                {
                    train.isReparing = false;
                    act = Active.refresh;
                    move_Work = true;
                }

                //ȣ���� ���� �ʾ��� ��
                if (!isCalling)
                {
                    if (!move_Work) // �������� ���� ��
                    {
                        if (!train.isReparing) // ���� ���� �÷��װ� �������� ��
                        {
                            train.isReparing = true;
                        }
                        if (!isRepairing) // �ڽ��� ���� ����
                        {
                            StartCoroutine(Repair());
                        }
                    }
                }
            }else if (act == Active.refresh)
            {
                if (!isRefreshing)
                {
                    StartCoroutine(Refresh());
                }
                else
                {
                    if (!move_Work)
                    {
                        move_Work = true;
                    }
                }
            }
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
        if (Mer_GameType == GameType.Playing){
            if (act == Active.move){
                base.non_combatant_Move();
            }else if (act == Active.work){
                if (!isCalling)
                {
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
                }

            }else if(act == Active.call)
            {
                isCalling = true;
                move_Work = true;
                if (transform.position.x < Player_X_Position.x - 0.5f)
                {
                    Move_X = 1f;
                    rb2D.velocity = new Vector2(Move_X * 6f, rb2D.velocity.y);

                }
                else if (transform.position.x > Player_X_Position.x + 0.5f)
                {
                    Move_X = -1f;
                    rb2D.velocity = new Vector2(Move_X * 6f, rb2D.velocity.y);
                }
                else
                {
                    isCalling = false;
                    act = Active.move;
                    mercenaryDirector.Call_End_Engineer();
                }
            }else if(act == Active.refresh)
            {
                rb2D.velocity = Vector2.zero;
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

    public void PlayerEngineerCall(Vector3 PlayerCall_XPos)
    {
        if (act == Active.work)
        {
            train.isReparing = false;
        }
        Player_X_Position = new Vector3(PlayerCall_XPos.x, transform.position.y, transform.position.z);
        act = Active.call;
    }

    IEnumerator Repair()
    {
        isRepairing = true;
        workCount++;
        if (workCount >= Max_workCount + base.Item_workCount_UP)
        {
            act = Active.refresh;
            if (train.isReparing)
            {
                train.isReparing = false;
            }
        }
        else
        {
            train.Train_HP += repairAmount;
        }
        yield return new WaitForSeconds(repairDelay);
        isRepairing = false;
    }

    public int Check_Work()
    {
        if (act == Active.move)
        {
            return 0;
        }
        else if (act == Active.work)
        {
            return 1;
        }
        else if(act == Active.refresh)
        {
            return 2;
        }
        else
        {
            return -1;
        }
    }
}
