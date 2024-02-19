using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medic : Mercenary
{
    Rigidbody2D rigid;
    RaycastHit2D rayHit;
    Transform unit;

    [Header("일하는 플래그")]
    [SerializeField]
    bool work_HP;
    [SerializeField]
    bool work_Stamina;
    [SerializeField]
    bool work_Revive;

    float checkHP;
    float checkStamina;
    [Header("회복량")]
    public int Heal_HpAmount;
    public int Heal_StaminaAmount;
    public int Heal_ReviveAmount;
    [Header("OO%이하의 동료인 경우 힐")]
    public int Heal_HpParsent;

    bool isHeal_HP;
    bool isHeal_Stamina;
    bool isHeal_Revive;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        act = Active.move;
        rigid = GetComponent<Rigidbody2D>();
        checkHP = 200;
        checkStamina = 200;

        work_HP = false; 
        work_Stamina = false; 
        work_Revive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rayHit.collider != null)
        {
            if (rayHit.collider.GetComponent<Mercenary_Type>())
            {
                checkHP = rayHit.collider.GetComponent<Mercenary_Type>().medic_checkHpParsent;
                checkStamina = rayHit.collider.GetComponent<Mercenary_Type>().medic_checkStaminaParsent;
            }
            else
            {
                checkHP = 200;
                checkStamina = 200;
            }
        }

        if (checkHP != 0 && checkHP < Heal_HpParsent)
        {
            if(rayHit.collider != null)
            {
                unit = rayHit.collider.GetComponent<Transform>();
            }
            //힐
            act = Active.work;
            work_HP = true;
        }else if(checkHP == 0)
        {
            if (rayHit.collider != null)
            {
                unit = rayHit.collider.GetComponent<Transform>();
            }
            //부활
            act = Active.work;
            work_Revive = true;
        }else if(checkStamina == 0)
        {
            //추가 스테미나
            if (rayHit.collider != null)
            {
                unit = rayHit.collider.GetComponent<Transform>();
            }
            act = Active.work;
            work_Stamina = true;
        }


        if (act == Active.move)
        {
            if (move_X > 0)
            {
                sprite.flipX = false;
                Debug.DrawRay(rigid.position, Vector3.right, Color.yellow); //나중에 좌우 전환시 변경
                rayHit = Physics2D.Raycast(rigid.position, Vector3.right, 1f);
            }
            else
            {
                sprite.flipX = true;
                Debug.DrawRay(rigid.position, Vector3.left, Color.yellow); //나중에 좌우 전환시 변경
                rayHit = Physics2D.Raycast(rigid.position, Vector3.left, 1f);
            }
            base.non_combatant_Move();
        }else if(act == Active.work)
        {
            if (work_HP)
            {
                sprite.flipX = false;
                transform.position = new Vector3(unit.position.x - 1 ,unit.position.y,unit.position.z);
                if (!isHeal_HP)
                {
                    if(checkHP > Heal_HpParsent)
                    {
                        act = Active.move;
                        work_HP = false;
                    }
                    StartCoroutine(Heal_HP());
                }

            }else if (work_Revive)
            {
                sprite.flipX = false;
                transform.position = new Vector3(unit.position.x - 1, unit.position.y, unit.position.z);
                if (!isHeal_Revive)
                {
                    StartCoroutine(Heal_Revive());
                }
            }
            else if(work_Stamina)
            {
                sprite.flipX = false;
                transform.position = new Vector3(unit.position.x - 1, unit.position.y, unit.position.z);
                if (!isHeal_Stamina)
                {
                    if (checkStamina != 0)
                    {
                        act = Active.move;
                        work_Stamina = false;
                    }
                    StartCoroutine(Heal_Stamina());
                }
            }
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

        IEnumerator Heal_Stamina()
        {
            isHeal_Stamina = true;
            yield return new WaitForSeconds(2);
            if(Stamina - useStamina < 0)
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
            act = Active.move;
        }
    }
}
