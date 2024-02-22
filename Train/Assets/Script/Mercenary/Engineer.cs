using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : Mercenary
{
    Rigidbody2D rigid;
    Train train;
    bool move_Work;
    bool isRepairing;
    bool isCalling;
    float train_HpParsent;
    [Header("수리 속도 및 기차 수리량")]
    [SerializeField] private int repairDelay;
    [SerializeField] private int repairAmount;
    [Header("수리하러 갈때의 움직임")]
    [SerializeField] private int move_work_speed;
    [Header("OO%이하의 기차인 경우 수리")]
    [SerializeField] private int repairTrain_Parsent;
    Vector3 PlayerPosition;
    protected override void Start()
    {
        base.Start();
        act = Active.move;
        rigid = GetComponent<Rigidbody2D>();
        move_Work = true;
        isCalling = false;
    }

    void Update()
    {
        Debug.DrawRay(rigid.position, Vector3.down, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Platform"));
        train = rayHit.collider.GetComponentInParent<Train>();
        train_HpParsent = (float)train.cur_HP / (float)train.Train_HP * 100f;

        if(HP <= 0 && act != Active.die)
        {
            act = Active.die;
            isDying = true;
        }
        else if (act != Active.work && Stamina >= 50 && train_HpParsent < repairTrain_Parsent && !train.isReparing && act != Active.call && act != Active.die)
        {
            train.isReparing = true;
            act = Active.work;
        }
        else if(Stamina <= 0 || act == Active.work && train_HpParsent > repairTrain_Parsent)
        {
            train.isReparing = false;
            act = Active.weak;
            move_Work = true;
        }

        if(act == Active.move)
        {
            if (isCalling)
            {
                isCalling = false;
            }
            base.non_combatant_Move();
        }else if(act == Active.work && !isCalling)
        {
            if (move_Work)
            {
                if (transform.position.x < train.transform.position.x - 0.2)
                {
                    move_X = 0.01f;
                    sprite.flipX = false;
                    transform.Translate(move_X * move_work_speed, -1, 0);
                }
                else if(transform.position.x > train.transform.position.x + 0.2)
                {
                    move_X = -0.01f;
                    sprite.flipX = true;
                    transform.Translate(move_X * move_work_speed, -1, 0);
                }
                else
                {
                    move_Work = false;
                }
            }
            else if(!move_Work)
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
        }else if(act == Active.die && isDying )
        {
            Debug.Log("여기서 애니메이션 구현한다!");
            if (train.isReparing)
            {
                train.isReparing = false;
            }
            isDying = false;
        }else if(act == Active.weak)
        {
            if(!isRefreshing_weak)
            {
                StartCoroutine(Refresh_Weak());
            }else if(Stamina >= 70){
                act = Active.move;
            }
        }else if(act == Active.call)
        {
            isCalling = true;
            PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            if (transform.position.x < PlayerPosition.x - 0.5)
            {
                move_X = 0.01f;
                sprite.flipX = false;
                transform.Translate(move_X * 6f, 0, 0);
            }
            else if (transform.position.x > PlayerPosition.x + 0.5)
            {
                move_X = -0.01f;
                sprite.flipX = true;
                transform.Translate(move_X * 6f, 0, 0);
            }
            else
            {
                act = Active.move;
                GameObject.Find("MercenaryDirector").GetComponent<MercenaryDirector>().Engineer_Call_End();
            }
        }
    }

    public void PlayerEngineerCall()
    {
        if(act == Active.work)
        {
            train.isReparing = false;
        }
        act = Active.call;
    }

    IEnumerator Repair()
    {
        isRepairing = true;

        yield return new WaitForSeconds(repairDelay);
        if (Stamina - useStamina < 0)
        {
            Stamina = 0;
        }
        else
        {
            Stamina -= useStamina;
        }
        train.cur_HP += repairAmount;
        isRepairing = false;
    }
}