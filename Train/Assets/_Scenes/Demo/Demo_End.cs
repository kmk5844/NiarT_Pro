using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_End : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.Game_DataReset();
        }
    }
}
