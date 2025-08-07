using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medic : Mercenary
{
    public Transform unit;

    bool work_HP;
    public float CheckHP;
    int Heal_HpAmount;
    int Heal_HpParsent;

    bool isHeal_HP;


    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        act = Active.move;
    }

    protected override void Update()
    {
        base.Update();
        if (act != Active.work)
        {
            non_combatant_Flip();
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
                if (work_HP)
                {
                    if (!isHeal_HP)
                    {
                        CheckHP = unit.GetComponent<Mercenary_Type>().medic_checkHpParsent;
                        if (CheckHP > Heal_HpParsent)
                        {
                            work_HP = false;
                            unit.GetComponentInParent<Mercenary>().isHealWithMedic = false;
                        }
                        StartCoroutine(Heal_HP());
                    }
                }
                else
                {
                    if (act != Active.move)
                    {
                        act = Active.move;
                    }
                }
            }
            else if (act == Active.die && isDying)
            {
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
                base.non_combatant_Move();
            }
            else if (act == Active.work)
            {
                if (unit.GetComponentInParent<Mercenary>().Check_MoveX() > 0)
                {
                    Unit_Scale.localScale = new Vector3(-Unit_Scale_X, Unit_Scale_Y, Unit_Scale_Z);
                    transform.position = new Vector3(unit.position.x - 0.6f, Move_Y, 0);
                }
                else
                {
                    Unit_Scale.localScale = new Vector3(Unit_Scale_X, Unit_Scale_Y, Unit_Scale_Z);
                    transform.position = new Vector3(unit.position.x + 0.6f, Move_Y, 0);
                }
            }
            else if (act == Active.die)
            {
                rb2D.velocity = Vector2.zero;
            }
        }
        else if (Mer_GameType == GameType.Ending)
        {
            act = Active.Game_Wait;
            rb2D.velocity = Vector2.zero;
        }
    }

    IEnumerator Heal_HP()
    {
        isHeal_HP = true;
        unit.GetComponent<Mercenary_Type>().Heal_HP(Heal_HpAmount);
        Debug.Log(Heal_HpAmount);
        yield return new WaitForSeconds(0.5f);
        isHeal_HP = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision != null && collision.CompareTag("Mercenary"))
        {
            if (act != Active.work)
            {
                CheckHP = collision.GetComponent<Mercenary_Type>().medic_checkHpParsent;

                if (!work_HP)
                {
                    unit = collision.GetComponent<Transform>();
                }

                if (!unit.GetComponentInParent<Mercenary>().isHealWithMedic)
                {
                    if (CheckHP > 0 && CheckHP < Heal_HpParsent) //Heal_HPParset가 이하면 work로 변경된다. / 0이하 이거나
                    {
                        if (act != Active.work)
                        {
                            unit.GetComponentInParent<Mercenary>().isHealWithMedic = true;
                            act = Active.work;
                            work_HP = true;
                        }
                    }
                }
            }
        }
    }

    public void Level_AddStatus_Medic(List<Info_Level_Mercenary_Medic> type, int level)
    {
        Heal_HpAmount = type[level].Heal_Hp_Amount;
        Heal_HpParsent = type[level].Heal_HP_Parsent;
    }
}
