using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PixelCrushers.AnimatorSaver;

public class PrintingPressDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject PrintingPressWindow;
    public GameObject CheckWindow;
    public GameObject SelectStage;

    [Header("Data")]
    public SA_PlayerData playerData;
    bool startFlag;
    [SerializeField]
    PrintObject[] printobject;
    float fadeDuration = 2f;
    int clickNum;
    int minPersent;
    int maxPersent;
    int Persent;

    [Header("UI")]
    public Button[] button;
    public Button NextButton;
    public Image targetImage;
    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        PrintingPressWindow.SetActive(false);
    }
    private void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 0;
        }

        int i = 0;
        
        foreach(Button btn  in button)
        {
            int num = i;
            btn.onClick.AddListener(()=>ClickPrint(num));
            if(playerData.Coin > printobject[i].coin)
            {
                btn.interactable = true;
            }
            else
            {
                btn.interactable = false;
            }
            i++;
        }
    }

    private void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }
    void StartEvent()
    {
        PrintingPressWindow.SetActive(true);
        startFlag = true;
    }

    public void PrintingPressEnd()
    {
        SelectStage.SetActive(true);
    }
    public void ClickPrint(int num)
    {
        NextButton.gameObject.SetActive(false);
        clickNum = num;
        playerData.SA_Buy_Coin(printobject[clickNum].coin);
        minPersent = printobject[clickNum].minPersent;
        maxPersent = printobject[clickNum].maxPersent;
        Persent = Random.Range(minPersent, maxPersent);
        foreach(Button btn in button)
        {
            btn.interactable = false;
        }
        StartCoroutine(PrintAni());
    }

    IEnumerator PrintAni()
    {
        Debug.Log("흥보하는 중...");
        yield return StartCoroutine(FadeTo(1f));
        yield return new WaitForSeconds(5f);
        Result();
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

    void Result()
    {
        int rand = Random.Range(0, 101);
        if(rand < Persent)
        {
            Debug.Log("성공");
            int rewardCoin = Random.Range(printobject[clickNum].rewardMinCoin, printobject[clickNum].rewardMaxCoin);
            playerData.SA_Get_Coin(rewardCoin);
        }
        else
        {
            Debug.Log("실패");
        }
    }
}

[System.Serializable]
public struct PrintObject
{
    public int minPersent;
    public int maxPersent;
    public int coin;
    public int rewardMinCoin;
    public int rewardMaxCoin;
}