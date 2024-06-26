using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyMonster_Item : MonoBehaviour
{
    public ItemDataObject Item;
    UseItem useitemScript;

    private void Start()
    {
        useitemScript = GameObject.Find("ItemDirector").GetComponent<UseItem>();
    }
    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            useitemScript.Get_SupplyItem(Item.Num);
            Destroy(gameObject);
        }
    }
}
