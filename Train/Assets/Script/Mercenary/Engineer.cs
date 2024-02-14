using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : Mercenary
{
    public Active act;
    Rigidbody2D rigid;
    Train train;
    bool move_Work;
    bool isRepairing;
    float train_HpParsent;
    protected override void Start()
    {
        base.Start();
        act = Active.move;
        rigid = GetComponent<Rigidbody2D>();
        move_Work = true;
    }

    void Update()
    {
        Debug.DrawRay(rigid.position, Vector3.down, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Platform"));
        train = rayHit.collider.GetComponentInParent<Train>();
        train_HpParsent = (float)train.cur_HP / (float)train.Train_HP * 100f;
        if (act != Active.work && train_HpParsent < 70f)
        {
            act = Active.work;
        }
        
        if(act == Active.work && train_HpParsent > 70f)
        {
            act = Active.move;
        }

        if(act == Active.move)
        {
            base.move();
        }else if(act == Active.work)
        {
            if (move_Work)
            {
                if (transform.position.x < train.transform.position.x - 0.2)
                {
                    move_X = 0.01f;
                    sprite.flipX = false;
                    transform.Translate(move_X * 6f, 0, 0);
                }
                else if(transform.position.x > train.transform.position.x + 0.2)
                {
                    move_X = -0.01f;
                    sprite.flipX = true;
                    transform.Translate(move_X * 6f, 0, 0);
                }
                else
                {
                    move_Work = false;
                }
            }
            else if(!move_Work)
            {
                if (!isRepairing)
                {
                    Debug.Log(train_HpParsent + "%");
                    StartCoroutine(Repair());
                }
            }
        }
    }

    IEnumerator Repair()
    {
        isRepairing = true;
        yield return new WaitForSeconds(1f);
        train.cur_HP += 30;
        isRepairing = false;
    }
}
