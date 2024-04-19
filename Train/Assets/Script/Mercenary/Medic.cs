using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medic : Mercenary
{
    public Transform unit;

    [Header("일하는 플래그")]
    [SerializeField]
    bool work_HP;
    [SerializeField]
    bool work_Stamina;
    [SerializeField]
    bool work_Revive;

    float checkHP;
    float checkStamina;
    [Header ("타입마다의 추가 스탯")]
    [Header("회복량")]
    public int Heal_HpAmount;
    public int Heal_StaminaAmount;
    public int Heal_ReviveAmount;
    [Header("OO%이하의 동료인 경우 힐")]
    public int Heal_HpParsent;

    bool isHeal_HP;
    bool isHeal_Stamina;
    bool isHeal_Revive;
    Vector3 PlayerPosition;

    protected override void Start()
    {
        base.Start();
        act = Active.move;
        checkHP = 200;
        checkStamina = 200;

        work_HP = false; 
        work_Stamina = false; 
        work_Revive = false;
    }

    // Update is called once per frame
    void Update()
    {
        Check_GameType();

        if (M_gameType == GameType.Playing)
        {
            if (HP <= 0 && act != Active.die)
            {
                act = Active.die;
                isDying = true;
            }

            if (Stamina <= 0 && act == Active.work)
            {
                work_HP = false;
                work_Revive = false;
                work_Stamina = false;
                unit.GetComponentInParent<Mercenary>().isHealWithMedic = false;
                act = Active.weak;
            }

            if (act == Active.move)
            {
                transform.position = new Vector3(transform.position.x, move_Y, 0);
                if (move_X > 0)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }
                base.non_combatant_Move();
            }
            else if (act == Active.work)
            {
                if (unit.GetComponentInParent<Mercenary>().Check_moveX() > 0)
                {
                    sprite.flipX = false;
                    transform.position = new Vector3(unit.position.x - 1, move_Y, 0);
                }
                else
                {
                    sprite.flipX = true;
                    transform.position = new Vector3(unit.position.x + 1, move_Y, 0);
                }

                if (work_HP)
                {
                    if (!isHeal_HP)
                    {
                        if (checkHP > Heal_HpParsent)
                        {
                            act = Active.move;
                            work_HP = false;
                            unit.GetComponentInParent<Mercenary>().isHealWithMedic = false;
                        }
                        StartCoroutine(Heal_HP());
                    }

                }
                else if (work_Revive)
                {
                    if (!isHeal_Revive)
                    {
                        StartCoroutine(Heal_Revive());
                    }
                }
                else if (work_Stamina)
                {
                    if (!isHeal_Stamina)
                    {
                        if (checkStamina != 0)
                        {
                            act = Active.move;
                            work_Stamina = false;
                            unit.GetComponentInParent<Mercenary>().isHealWithMedic = false;
                        }
                        StartCoroutine(Heal_Stamina());
                    }
                }
            }
            else if (act == Active.die && isDying)
            {
                Debug.Log("여기서 애니메이션 구현한다!4");
                isDying = false;
            }
            else if (act == Active.weak)
            {
                if (!isRefreshing_weak)
                {
                    StartCoroutine(Refresh_Weak());
                }
                else if (Stamina >= 70)
                {
                    act = Active.move;
                }
            }
            else if (act == Active.call)
            {
                PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
                if (transform.position.x < PlayerPosition.x - 1.5)
                {
                    move_X = 0.01f;
                    sprite.flipX = false;
                    transform.Translate(move_X * 6f, 0, 0);
                }
                else if (transform.position.x > PlayerPosition.x + 1.5)
                {
                    move_X = -0.01f;
                    sprite.flipX = true;
                    transform.Translate(move_X * 6f, 0, 0);
                }
                else
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player.GetComponent<Player>().Check_moveX() > 0)
                    {
                        sprite.flipX = false;
                        transform.position = new Vector3(PlayerPosition.x - 1, PlayerPosition.y, PlayerPosition.z);
                    }
                    else
                    {
                        sprite.flipX = true;
                        transform.position = new Vector3(PlayerPosition.x + 1, PlayerPosition.y, PlayerPosition.z);
                    }

                    if (!isHeal_HP)
                    {
                        if (Stamina <= 0 || player.GetComponent<Player>().Check_HpParsent() > Heal_HpParsent)
                        {
                            act = Active.move;
                            work_HP = false;
                            GameObject.Find("MercenaryDirector").GetComponent<MercenaryDirector>().Call_End(mercenaryType.Medic);
                        }
                        StartCoroutine(Heal_PlayerHP(player));
                    }
                }
            }
        }
    }

    public void PlayerMedicCall()
    {
        if(act == Active.work)
        {
            unit.GetComponentInParent<Mercenary>().isHealWithMedic = false;
        }
        work_HP = true;
        act = Active.call;
    }
    IEnumerator Heal_HP()
    {
        isHeal_HP = true;
        yield return new WaitForSeconds(2);
        if (Stamina - useStamina < 0)
        {
            Stamina = 0;
        }
        else
        {
            Stamina -= useStamina;
        }
        unit.GetComponent<Mercenary_Type>().Heal_HP(Heal_HpAmount);
        isHeal_HP = false;
    }

    IEnumerator Heal_PlayerHP(GameObject player)
    {
        isHeal_HP = true;
        yield return new WaitForSeconds(2);
        if (Stamina - useStamina < 0)
        {
            Stamina = 0;
        }
        else
        {
            Stamina -= useStamina;
        }
        player.GetComponent<Player>().Heal_HP(Heal_HpAmount);
        isHeal_HP = false;
    }

    IEnumerator Heal_Stamina()
    {
        isHeal_Stamina = true;
        yield return new WaitForSeconds(2);
        if (Stamina - useStamina < 0)
        {
            Stamina = 0;
        }
        else
        {
            Stamina -= useStamina;
        }
        unit.GetComponent<Mercenary_Type>().Heal_Stamina(Heal_StaminaAmount);
        isHeal_Stamina = false;
    }

    IEnumerator Heal_Revive()
    {
        isHeal_Revive = true;
        yield return new WaitForSeconds(2);
        if (Stamina - useStamina < 0)
        {
            Stamina = 0;
        }
        else
        {
            Stamina -= useStamina;
        }
        unit.GetComponent<Mercenary_Type>().Heal_Revive(Heal_ReviveAmount);
        isHeal_Revive = false;
        unit.GetComponentInParent<Mercenary>().isHealWithMedic = false;
        act = Active.move;
        work_Revive = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && act != Active.call)
        {
            if (collision.CompareTag("Mercenary"))
            {
                checkHP = collision.GetComponent<Mercenary_Type>().medic_checkHpParsent;
                checkStamina = collision.GetComponent<Mercenary_Type>().medic_checkStaminaParsent;

                if (!work_HP && !work_Revive && !work_Stamina)
                {
                    unit = collision.GetComponent<Transform>();
                }

                if (!unit.GetComponentInParent<Mercenary>().isHealWithMedic)
                {
                    if (checkHP != 0 && checkHP < Heal_HpParsent)
                    {
                        //힐
                        if (act != Active.work)
                        {
                            unit.GetComponentInParent<Mercenary>().isHealWithMedic = true;
                            act = Active.work;
                            work_HP = true;
                        }
                    }
                    else if (checkHP == 0)
                    {
                        //부활
                        if (act != Active.work)
                        {
                            unit.GetComponentInParent<Mercenary>().isHealWithMedic = true;
                            act = Active.work;
                            work_Revive = true;
                        }
                    }
                    else if (checkStamina == 0)
                    {
                        //추가 스테미나
                        if (act != Active.work)
                        {
                            unit.GetComponentInParent<Mercenary>().isHealWithMedic = true;
                            act = Active.work;
                            work_Stamina = true;
                        }
                    }
                }
            }
            else
            {
                checkHP = 200;
                checkStamina = 200;
            }
        }
        else
        {
            checkHP = 200;
            checkStamina = 200;
        }
    }

    public void Level_AddStatus_Medic(List<Info_Level_Mercenary_Medic> type, int level)
    {
        Heal_HpAmount = type[level].Heal_Hp_Amount;
        Heal_StaminaAmount = type[level].Heal_Stamina_Amount;
        Heal_ReviveAmount = type[level].Heal_Revive_Amount;
        Heal_HpParsent = type[level].Heal_HP_Parsent;
    }
}
