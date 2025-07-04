using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldTranningDirector : MonoBehaviour
{
    [Header("���丮")]
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
        Debug.Log("���X���X");
        yield return StartCoroutine(FadeTo(1f));
        yield return new WaitForSeconds(5f);
        Reward();
        yield return StartCoroutine(FadeTo(0f)); // ���� (�����)
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

        // ���� �� ��Ȯ�� �����ֱ�
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
                // ���ݷ� ����
                break;
            case 1:
                // ������ ����
                break;
            case 2:
                // ���� ����
                break;
            case 3:
                // ���ݼӵ� ����
                break;
            case 4:
                // �̵��ӵ� ����
                break;
            case 5:
                // ü�� ȸ��
                break;
            case 6:
                // ü�� ����
                break;
            case 7:
                // ü�� ���� + ���ݷ� ����
                break;
            case 8:
                // ü�� ���� + ���ݼӵ� ����
                break;
            case 9:
                // ü�� ���� + ���� ����
                break;
            case 10:
                // �ƹ��� ȿ���� ����.
                break;
        }
        Debug.Log("����");
    }


    /*
    �Ʒ��ϱ� ��ư Ŭ���Ѵ�.
    
    ���� ����
    - ���ݷ� ����
    - ������ ����
    - ���� ����
    - ���ݼӵ� ����
    - �̵��ӵ� ����
    - ü�� ȸ��
    - ü�� ���� ( ���� Ʈ���̴����� ���� ü�� ���� )
    - ü�� ����
    
    �Ͻ������� �����ϴ� �Ʒ��̴�.
    */
}
