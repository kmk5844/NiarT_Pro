using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar_Door : MonoBehaviour
{
    public int num;

    Hangar_Train director;
    BoxCollider2D col;

    public GameObject CanEffect;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (col.enabled)
        {
            CanEffect.SetActive(true);
        }
        else
        {
            CanEffect.SetActive(false);
        }
    }

    public void Set(Hangar_Train direcotr_)
    {
        director = direcotr_;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            director.EnterDoor(num);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && col.enabled)
        { 
            director.ExitDoor();
        }
    }
}
