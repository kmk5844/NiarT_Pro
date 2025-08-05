using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar_Door : MonoBehaviour
{
    public int num;

    Hangar_Train director;

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
        if (collision.CompareTag("Player"))
        { 
            director.ExitDoor();
        }
    }
}
