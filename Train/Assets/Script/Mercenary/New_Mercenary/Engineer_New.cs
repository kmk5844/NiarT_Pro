using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer_New : Mercenary_New
{
    Transform player;

    Rigidbody2D rigid;
    Train_InGame train;
    bool move_Work;
    bool isRepairing;
    public bool isCalling;
    float train_HpParsent;

    private int repairDelay;
    private int repairAmount;
    private int move_work_Speed;
    private int repaireTrain_parsent;
    Vector2 Player_X_Positino;

    bool TrainSpawnFlag;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        player = gameDirector.player.transform;
        act = Active.move;
        rigid = GetComponent<Rigidbody2D>();
        move_Work = true;
        TrainSpawnFlag = false;
        isCalling = false;
    }

    private void Update()
    {
        if (!TrainSpawnFlag)
        {
            TrainSpawnFlag = gameDirector.SpawnTrainFlag;
        }else{
            Debug.DrawRay(rigid.position, Vector3.down, Color.green);
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Platform"));

            train = rayHit.collider.GetComponentInParent<Train_InGame>();
            train_HpParsent = (float)train.cur_HP / (float)train.Train_HP * 100f;
        }

        Check_GameType();
        base.non_combatant_Flip();
        
        if(Mer_GameType == GameType.Playing)
        {
            if(HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }
        }
    }
}
