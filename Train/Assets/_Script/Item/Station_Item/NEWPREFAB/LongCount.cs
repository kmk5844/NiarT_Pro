using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongCount : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Button btn;
    private float pressTime = 0f; // 누른 시간
    private bool isPressing = false; // 누름 상태
    private bool isCounting = false; // 카운트 시작 여부
    private float countTimer = 0f; // 0.1초 카운트 타이머
    private readonly float requiredPressTime = 0.5f; // 0.5초 누름 시작
    private readonly float countPressTime = 0.1f; // 0.1초마다 카운트

    void Start()
    {
        btn = GetComponent<Button>();
    }

    void Update()
    {
        if (isPressing)
        {
            pressTime += Time.deltaTime;

            // 0.5초 이상 눌렀을 때 카운트 시작
            if (pressTime >= requiredPressTime && !isCounting)
            {
                isCounting = true;
                countTimer = countPressTime; // 즉시 첫 카운트 가능하도록
            }

            // 카운트 중이면 0.1초마다 카운트 증가
            if (isCounting)
            {
                countTimer -= Time.deltaTime;
                if (countTimer <= 0f)
                {
                    btn.onClick.Invoke();
                    countTimer = countPressTime; // 0.1초마다 재설정
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
        pressTime = 0f;
        countTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressing = false;
        isCounting = false; // 버튼 뗄 때 카운트 중지
        pressTime = 0f;
        countTimer = 0f;
    }
}
