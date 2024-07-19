using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBackGround : MonoBehaviour
{
    public Transform gameobject;
    public GameObject image;

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            Instantiate(image, gameobject);
        }
    }
}
