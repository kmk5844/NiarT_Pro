using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldTranningDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject OldTranningWindow;
    //public GameObject CheckWindow;
    public GameObject SelectStage;

    [Header("Data")]

    public SA_Event eventData;
    bool startFlag;

    public Button TranningButton;
    public Image targetImage;
    float fadeDuration = 2f;
    public GameObject CheckWindow;
    int RewardNum;

/*    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        BlackMarketWindow.SetActive(false);
    }*/

    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
/*        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }*/
    }
    void StartEvent()
    {
        OldTranningWindow.SetActive(true);
        startFlag = true;
    }

    public void BlackMarketEnd()
    {
        SelectStage.SetActive(true);
    }


    public void ClickTranning()
    {
        StartCoroutine(Tranning());
    }
    
    IEnumerator Tranning()
    {
        Debug.Log("으쌰으쌰");
        yield return StartCoroutine(FadeTo(1f));
        yield return new WaitForSeconds(5f);
        Reward();
        yield return StartCoroutine(FadeTo(0f)); // 투명 (밝아짐)
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = targetImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);

            Color color = targetImage.color;
            color.a = newAlpha;
            targetImage.color = color;

            yield return null;
        }

        // 최종 값 정확히 맞춰주기
        Color finalColor = targetImage.color;
        finalColor.a = targetAlpha;
        targetImage.color = finalColor;

        if (targetAlpha == 0f)
        {
            CheckWindow.SetActive(true);
        }
    }

    void Reward()
    {
        Debug.Log(RewardNum);
        switch (RewardNum)
        {
            case 0:
                // 공격력 증가
                break;
            case 1:
                // 점프력 증가
                break;
            case 2:
                // 방어력 증가
                break;
            case 3:
                // 공격속도 증가
                break;
            case 4:
                // 이동속도 증가
                break;
            case 5:
                // 체력 회복
                break;
            case 6:
                // 체력 감소
                break;
            case 7:
                // 체력 감소 + 공격력 증가
                break;
            case 8:
                // 체력 감소 + 공격속도 증가
                break;
            case 9:
                // 체력 감소 + 방어력 증가
                break;
            case 10:
                // 아무런 효과가 없다.
                break;
        }
        Debug.Log("보상");
    }


    /*
    훈련하기 버튼 클릭한다.
    
    버프 내용
    - 공격력 증가
    - 점프력 증가
    - 방어력 증가
    - 공격속도 증가
    - 이동속도 증가
    - 체력 회복
    - 체력 감소 ( 과한 트레이닝으로 인한 체력 감소 )
    - 체력 감소
    
    일시적으로 증가하는 훈련이다.
    */
}
