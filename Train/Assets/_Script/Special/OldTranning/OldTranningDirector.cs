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
    public GameObject CheckWindow;
    public GameObject SelectStage;

    [Header("Data")]
    public SA_PlayerData playerData;
    public SA_Event eventData;
    bool startFlag;
    float fadeDuration = 2f;
    int RewardNum;

    [Header("UI")]
    public Button TranningButton;
    public Button NextStageButton;
    public Image targetImage;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        OldTranningWindow.SetActive(false);
    }

    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 0;
        }

        CheckTranningButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }
    void StartEvent()
    {
        OldTranningWindow.SetActive(true);
        startFlag = true;
    }

    public void OldTranningEnd()
    {
        SelectStage.SetActive(true);
    }

    void CheckTranningButton()
    {
        if(playerData.Coin > 10000)
        {
            TranningButton.interactable = true;
        }
        else
        {
            TranningButton.interactable = false;
        }
    }

    public void ClickTranning()
    {
        playerData.SA_Buy_Coin(10000);
        StartCoroutine(Tranning());
    }
    
    IEnumerator Tranning()
    {
        Debug.Log("���X���X");
        NextStageButton.gameObject.SetActive(false);
        TranningButton.interactable = false;
        yield return StartCoroutine(FadeTo(1f));
        yield return new WaitForSeconds(5f);
        Reward();
        yield return StartCoroutine(FadeTo(0f)); // ���� (�����)
        yield return new WaitForSeconds(1f);
        CheckWindow.SetActive(true);
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
        eventData.OldTrannningOn(RewardNum);
        switch (RewardNum)
        {
            case 0:
                // ���ݷ� ����
                Debug.Log("���ݷ� ����");
                break;
            case 1:
                // ������ ����
                Debug.Log("������ ����");
                break;
            case 2:
                // ���� ����
                Debug.Log("���� ����");
                break;
            case 3:
                // ���ݼӵ� ����
                Debug.Log("���ݼӵ� ����");
                break;
            case 4:
                // �̵��ӵ� ����
                Debug.Log("�̵��ӵ� ����");
                break;
            case 5:
                // ü�� ȸ��
                Debug.Log("ü�� ����");
                break;
            case 6:
                // ü�� ����
                Debug.Log("ü�� ����");
                break;
            case 7:
                // ü�� ���� + ���ݷ� ����
                Debug.Log("ü�� ����");
                Debug.Log("���ݷ� ����");
                break;
            case 8:
                // ü�� ���� + ���ݼӵ� ����
                Debug.Log("ü�� ����");
                Debug.Log("���ݼӵ� ����");
                break;
            case 9:
                // ü�� ���� + ���� ����
                Debug.Log("ü�� ����");
                Debug.Log("���� ����");
                break;
            case 10:
                // �ƹ��� ȿ���� ����.
                Debug.Log("X");
                break;
        }
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
