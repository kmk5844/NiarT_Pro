using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Short_Ranged_KillZone : MonoBehaviour
{
    float flipX;
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
        flipX = unit.Check_moveX();
        if (flipX > 0)
        {
            transform.position = new Vector3(unit.transform.position.x + 1f, unit.transform.position.y, 0);
        }else if(flipX < 0)
        {
            transform.position = new Vector3(unit.transform.position.x - 1f, unit.transform.position.y, 0);
        }

        if(Time.time >= lastTime + unit.Attack_CoolTime)
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
                unit.Attack_Stamina();
                Destroy(collision.gameObject);
                attack_Flag = false;
            }
        }

    }
}
