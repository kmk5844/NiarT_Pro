using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBackGround : MonoBehaviour
{
    public ScrollRect scrollView;
    public RectTransform content;
    public Button leftButton;
    public Button rightButton;

    int currentIndex = 1;
    bool isMoving = false;

    private void Start()
    {
        content.anchoredPosition = new Vector2(-((currentIndex * 1920) + 960), 540);
        leftButton.onClick.AddListener(ShowPreviousBackGround);
        rightButton.onClick.AddListener(ShowNextBackGround);
    }

    void ShowPreviousBackGround()
    {
        if(currentIndex > 0 && !isMoving)
        {
            isMoving = true;
            currentIndex--;
            StartCoroutine(SmoothMoveContent(currentIndex));
        }
    }

    void ShowNextBackGround()
    {
        if(currentIndex < 2 && !isMoving)
        {
            isMoving = true;
            currentIndex++;
            StartCoroutine(SmoothMoveContent(currentIndex));
        }
    }

    IEnumerator SmoothMoveContent(int targetIndex)
    {
        float startX = content.anchoredPosition.x;
        float targetX = (targetIndex * 1920) + 960; // 각 배경사진의 가로 길이만큼 이동

        float duration = 0.3f; // 이동에 걸리는 시간 (초)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float newX = Mathf.Lerp(startX, -targetX, t);
            content.anchoredPosition = new Vector2(newX, 540);
            yield return null;
        }

        content.anchoredPosition = new Vector2(-targetX, 540);
        isMoving = false;
    }
}
