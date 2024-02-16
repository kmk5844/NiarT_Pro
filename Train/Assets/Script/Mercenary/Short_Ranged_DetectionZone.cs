using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

public class Short_Ranged_DetectionZone : MonoBehaviour
{
    Transform Target;
    public bool Target_Flag;
    Short_Ranged unit;

    void Start()
    {
        unit = GetComponentInParent<Short_Ranged>();
    }

    private void Update()
    {
        if (Target != null)
        {
            Target_Flag = true;
        }
        else
        {
            Target_Flag = false;
        }

        unit.TargetFlag(Target_Flag);
        
        if(Target_Flag)
        {
            unit.TargetPosition(Target);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (Target == null)
            {
                Target = collision.transform;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (Target == null)
            {
                Target = collision.transform;
            }
            if (collision.transform != Target)
            {
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (collision.transform == Target)
            {
                Target = null;
            }
        }
    }
}