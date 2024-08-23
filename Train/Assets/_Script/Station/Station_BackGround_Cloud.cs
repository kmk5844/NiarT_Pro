using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station_BackGround_Cloud : MonoBehaviour
{
    Material _material;
    float offset;
    [SerializeField]
    float speed;
    void Start()
    {
        _material = GetComponent<Image>().material;
        _material.SetTextureOffset("_MainTex", Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        offset += (Time.deltaTime * speed);
        _material.SetTextureOffset("_MainTex", new Vector2(offset, 0) * speed);
    }
}
