using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class testPlayer : MonoBehaviour
{
    private bool itemEaten = false;
    bool isClicked = false;

    float ClickTime = 0f;
    float MaxClickTime = 5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            isClicked = false;
        }

        if (isClicked && itemEaten)
        {
            ClickTime += Time.deltaTime;
            Debug.Log(ClickTime);
            if(ClickTime > MaxClickTime)
            {
                ResetItem();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Test_Monster"))
        {
            EatItem();
        }
    }

    void EatItem()
    {
        itemEaten = true;
        Debug.Log("Item Eaten - Flag is True");
    }

    void ResetItem()
    {
        itemEaten = false;
        ClickTime = 0f;
        Debug.Log("Flag set to False - Max duration reached");
    }
}
