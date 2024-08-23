using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Claymore : MonoBehaviour
{
    GameObject BombObject;

    private void Start()
    {
        BombObject = transform.GetChild(0).gameObject;
        BombObject.GetComponent<Item_Claymore_Bomb>().Claymore = gameObject;
        BombObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            BombObject.SetActive(true);
        }
    }
}
