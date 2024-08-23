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
                unit.mercenaryActive_Change(Active.work);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(unit_act == Active.work)
        {
            if (collision.CompareTag("Monster"))
            {
                unit.mercenaryActive_Change(Active.move);
            }
        }
    }
}
