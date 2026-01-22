using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingBackGround : MonoBehaviour
{
    public bool MatAndSpriteFlag = true;
    public float baseSpeed = 0.5f; // 기본 이동 속도
    [Header("Mat")]
    [Range(0.03f, 1f)]
    public float parallaxSpeed = 1f; // 속도 비율 조정용
    private GameObject[] backgrounds;
    private Material[] mat;
    private float[] backSpeed; // 각 배경별 속도 비율
    private float offset;

    [Header("Sprite")]
    public float depth = 0.5f; 
    public float wrapWidth = 20f; 
    Vector3 startPos; 
    public bool leftAndRight = false;

    void Start()
    {
        if (MatAndSpriteFlag)
        {
            int backCount = transform.childCount;
            backgrounds = new GameObject[backCount];
            mat = new Material[backCount];
            backSpeed = new float[backCount];

            float farthestZ = float.MinValue;

            // 각 자식 배경과 머티리얼 초기화
            for (int i = 0; i < backCount; i++)
            {
                backgrounds[i] = transform.GetChild(i).gameObject;
                mat[i] = backgrounds[i].GetComponent<Renderer>().material;

                // Z 값 최대값 계산
                if (backgrounds[i].transform.position.z > farthestZ)
                    farthestZ = backgrounds[i].transform.position.z;
            }

            // 각 배경별 속도 비율 계산 (멀리 있는 배경일수록 느리게)
            for (int i = 0; i < backCount; i++)
            {
                backSpeed[i] = 1f - (backgrounds[i].transform.position.z / farthestZ);
                backSpeed[i] = Mathf.Clamp(backSpeed[i], 0.1f, 1f); // 너무 느린 거 방지
            }
        }
        else
        {
            startPos = transform.localPosition;
        }

    }

    void Update()
    {
        if (MatAndSpriteFlag)
        {
            offset += Time.deltaTime * baseSpeed;

            for (int i = 0; i < mat.Length; i++)
            {
                // 각 배경별 속도 적용
                float speed = backSpeed[i] * parallaxSpeed;
                mat[i].SetTextureOffset("_MainTex", new Vector2(offset * speed, 0));
            }
        }
        else
        {
            float move = baseSpeed * depth * Time.deltaTime;

            // 배경 왼쪽 이동
            if (!leftAndRight)
            {
                transform.Translate(Vector3.left * move);
            }
            else
            {
                transform.Translate(Vector3.right * move);
            }

            // 무한 루프 처리
            if (transform.localPosition.x <= startPos.x - wrapWidth)
            {
                transform.localPosition += Vector3.right * wrapWidth * 2f;
            }
        }
        
    }
}
