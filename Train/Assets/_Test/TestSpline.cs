using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TestSpline : MonoBehaviour
{
    private void Start()
    {
        SplineInstantiate sp = GetComponent<SplineInstantiate>();
        for (int i = 0; i < 20; i++)
        {
            Debug.Log(sp.transform.GetChild(0).GetChild(i).name);
        }
    }
}
