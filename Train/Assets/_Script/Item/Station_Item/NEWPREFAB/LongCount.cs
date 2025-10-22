using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongCount : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Button btn;
    private float pressTime = 0f; // ���� �ð�
    private bool isPressing = false; // ���� ����
    private bool isCounting = false; // ī��Ʈ ���� ����
    private float countTimer = 0f; // 0.1�� ī��Ʈ Ÿ�̸�
    private readonly float requiredPressTime = 0.5f; // 0.5�� ���� ����
    private readonly float countPressTime = 0.1f; // 0.1�ʸ��� ī��Ʈ

    void Start()
    {
        btn = GetComponent<Button>();
    }

    void Update()
    {
        if (isPressing)
        {
            pressTime += Time.deltaTime;

            // 0.5�� �̻� ������ �� ī��Ʈ ����
            if (pressTime >= requiredPressTime && !isCounting)
            {
                isCounting = true;
                countTimer = countPressTime; // ��� ù ī��Ʈ �����ϵ���
            }

            // ī��Ʈ ���̸� 0.1�ʸ��� ī��Ʈ ����
            if (isCounting)
            {
                countTimer -= Time.deltaTime;
                if (countTimer <= 0f)
                {
                    btn.onClick.Invoke();
                    countTimer = countPressTime; // 0.1�ʸ��� �缳��
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
        isCounting = false; // ��ư �� �� ī��Ʈ ����
        pressTime = 0f;
        countTimer = 0f;
    }
}
