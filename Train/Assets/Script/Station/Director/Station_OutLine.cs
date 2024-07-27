using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station_OutLine : MonoBehaviour
{
    Image img;
    private void Start()
    {
        img = GetComponent<Image>();
        img.material.SetFloat("_OutlineEnabled", 0);
    }

    public void OutLine_ON()
    {
        img.material.SetFloat("_OutlineEnabled", 1);
    }

    public void OntLine_Off()
    {
        img.material.SetFloat("_OutlineEnabled", 0);
    }
}
