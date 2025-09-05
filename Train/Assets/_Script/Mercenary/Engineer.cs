using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engineer : Mercenary
{
    Train_InGame train;
    int trainHealAmount;
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

    public ParticleSystem repair_Effect;

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
        }
        
        if(train != null)
        {
            train_HpParsent = (float)train.Train_HP / (float)train.Max_Train_HP * 100f;
        }
        
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
                //Ʈ������ ���� ü�� �̻��� ��, ��ȯ
                if (train_HpParsent > repaireTrain_parsent)
                {
                    train.isReparing = false;
                    train.RepairEnd();
                    train = null;
                    //act = Active.refresh;
                    act = Active.move;
                    move_Work = true;
                }

                //ȣ���� ���� �ʾ��� ��
                if (!move_Work) // �������� ���� ��
                {
                    if (!train.isReparing) // ���� ���� �÷��װ� �������� ��
                    {
                        
                        train.isReparing = true;
                    }

                    if (!isRepairing) // �ڽ��� ���� ����
                    {
                        StartCoroutine(Repair(trainHealAmount));
                    }
                }
            }

            if (act == Active.die && isDying)
            {
                if (train.isReparing)
                {
                    train.isReparing = false;
                    train.EngineerDie();
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
                //���������� �޷�����
                if (move_Work)
                {
                    if(transform.position.x < train.transform.position.x - 0.2)
                    {
                        Move_X = 1f;
                        rb2D.velocity = new Vector2(Move_X * (moveSpeed * 2), rb2D.velocity.y);
                    }
                    else if(transform.position.x > train.transform.position.x + 0.2)
                    {
                        Move_X = -1f;
                        rb2D.velocity = new Vector2(Move_X * (moveSpeed * 2), rb2D.velocity.y);
                    }
                    else
                    {
                        rb2D.velocity = Vector2.zero;
                        move_Work = false;
                    }
                    ChnageImage_Move();
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
        repairAmount = type[level].Repair_Amount;
        repaireTrain_parsent = type[level].Repair_Train_MaxHpPersent;
    }

    IEnumerator Repair(int Heal)
    {
        isRepairing = true;
        train.Train_HP += Heal;
        repair_Effect.Play();
        yield return new WaitForSeconds(0.5f);
        isRepairing = false;
    }

    public bool Check_Work() // �̵��� �ϴ� ������� ȣ��
    {
        if (act == Active.move)
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    public void Set_Train(Train_InGame trainobj)
    {
        train = trainobj;
        trainHealAmount = train.Max_Train_HP * repairAmount / 100;
        act = Active.work;
    }
}
