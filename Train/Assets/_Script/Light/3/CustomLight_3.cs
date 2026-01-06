using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLight_3 : MonoBehaviour
{
    public parallex BackGround;
    CustomLightList_3 customLightList;

    Vector2 initPos;
    public float WrapRange = 5.35f;
    public int ParallaxIndex;   // 동기화할 배경 레이어
    public float CustomSpeed;


    void Start()
    {
        customLightList = GetComponentInParent<CustomLightList_3>();
        initPos = transform.position;
    }

    void Update()
    {
        float trainSpeed = customLightList.Speed / 100f;

        float baseSpeed =
            BackGround.backSpeed[ParallaxIndex] *
            BackGround.parallaxSpeed;

        float worldSpeed = (baseSpeed + trainSpeed) * CustomSpeed;

        // 배경과 동일한 이동
        transform.Translate(Vector2.left * worldSpeed * Time.deltaTime);

        if (transform.position.x <= initPos.x - WrapRange)
        {
            transform.position += Vector3.right * WrapRange * 2f;
        }
    }
}