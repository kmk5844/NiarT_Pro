using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short_Ranged_KillZone : MonoBehaviour
{
    Short_Ranged unit;
    public bool attack_Flag;
    float lastTime;

    void Start()
    {
        unit = GetComponentInParent<Short_Ranged>();
        lastTime = 0;
    }

    void Update()
    {
        if(Time.time >= lastTime + unit.unit_Attack_Delay)
        {
            attack_Flag = true;
            lastTime = Time.time;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (attack_Flag)
        {
            if (collision.CompareTag("Monster"))
            {
                Destroy(collision.gameObject);
                attack_Flag = false;
            }
        }
    }
}
