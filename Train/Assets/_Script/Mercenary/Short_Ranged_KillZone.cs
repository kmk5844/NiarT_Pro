using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short_Ranged_KillZone : MonoBehaviour
{
    Short_Ranged unit;
    Active unit_act;

    void Start()
    {
        unit = GetComponentInParent<Short_Ranged>();
    }

    void Update()
    {
        unit_act = unit.mercenaryActive_Check();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(unit_act == Active.move)
        {
            if (collision.CompareTag("Monster"))
            {
                if(unit.Target == null)
                {
                    unit.Target = collision.transform;
                    unit.lockFlag = true;
                }
            }
        }
    }
/*    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (unit.Target == null)
            {
                unit.Target = collision.transform;
            }
            if (collision.transform != unit.Target)
            {
                return;
            }
        }
    }*/


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (collision.transform == unit.Target && unit.lockFlag)
            {
                unit.Target = null;
                unit.lockFlag = false;
            }
        }
    }
}
