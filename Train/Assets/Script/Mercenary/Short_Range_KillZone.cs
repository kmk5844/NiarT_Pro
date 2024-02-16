using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

public class Short_Range_KillZone : MonoBehaviour
{
    Short_Ranged unit;

    private void Start()
    {
       unit = GetComponentInParent<Short_Ranged>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            unit.kill_Stamina();
            Destroy(collision.gameObject);
        }
    }
}