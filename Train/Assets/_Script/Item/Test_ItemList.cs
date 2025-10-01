using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ItemList : MonoBehaviour
{
    public int MinItme;
    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            TEST_ITem item = transform.GetChild(i).GetComponent<TEST_ITem>();
            item.transform.position = new Vector2(-3 - (i * 0.5f), 0f);
            //Debug.Log(MinItme + i);
            item.Num = MinItme + i;
        }
    }
}
