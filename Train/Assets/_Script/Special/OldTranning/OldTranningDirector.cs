using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
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

    [Header("Sprite")]
    public Sprite[] PlayerableImage;

    [Header("UI")]
    public Image PlayerImage;
    public Button TranningButton;
    public Button NextStageButton;
    public Image targetImage;
    public LocalizeStringEvent CheckWindowText;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
    }

    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 0;
        }
        PlayerImage.sprite = PlayerableImage[playerData.Player_Num];
        CheckWindowText.StringReference.TableReference = "SpecialStage_St";
        OldTranningWindow.SetActive(false);
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

    public void ClickTranning()
    {
        StartCoroutine(Tranning());
    }
    
    IEnumerator Tranning()
    {
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
        RewardNum = Random.Range(0, 11);
        eventData.OldTrannningOn(RewardNum);
        CheckWindowText.StringReference.TableEntryReference = "OldTranning_TranningReward_" + RewardNum;
        /*

                switch (RewardNum)
                {
                    case 0:
                        // ���ݷ� ����
                        CheckWindowText.text = "���ݷ� ����!";
                        //Debug.Log("���ݷ� ����");
                        break;
                    case 1:
                        // ������ ����
                        CheckWindowText.text = "������ ����!";
                        //Debug.Log("������ ����");
                        break;
                    case 2:
                        // ���� ����
                        CheckWindowText.text = "���� ����!";
                        //Debug.Log("���� ����");
                        break;
                    case 3:
                        // ���ݼӵ� ����
                        CheckWindowText.text = "���ݼӵ� ����!";
                        //Debug.Log("���ݼӵ� ����");
                        break;
                    case 4:
                        // �̵��ӵ� ����
                        CheckWindowText.text = "�̵��ӵ� ����!";
                        //Debug.Log("�̵��ӵ� ����");
                        break;
                    case 5:
                        // ü�� ȸ��
                        CheckWindowText.text = "ü�� ȸ��!";
                        //Debug.Log("ü�� ����");
                        break;
                    case 6:
                        // ü�� ����
                        CheckWindowText.text = "ü�� ����!";
                        //Debug.Log("ü�� ����");
                        break;
                    case 7:
                        // ü�� ���� + ���ݷ� ����
                        CheckWindowText.text = "ü�� ����! ���ݷ� ����!";
                        //Debug.Log("ü�� ����");
                        //Debug.Log("���ݷ� ����");
                        break;
                    case 8:
                        // ü�� ���� + ���ݼӵ� ����
                        CheckWindowText.text = "ü�� ����! ���ݼӵ� ����!";
                        //Debug.Log("ü�� ����");
                        //Debug.Log("���ݼӵ� ����");
                        break;
                    case 9:
                        // ü�� ���� + ���� ����
                        CheckWindowText.text = "ü�� ����! ���� ����!";
                        //Debug.Log("ü�� ����");
                        //Debug.Log("���� ����");
                        break;
                    case 10:
                        // �ƹ��� ȿ���� ����.
                        CheckWindowText.text = "�ƹ��� ȿ���� ����.";
                        //Debug.Log("X");
                        break;
                }*/
    }


    /*
        case 0:
            //���� �������� ������� �÷��̾� ���ݷ� ����
            Bullet_Atk += ((Bullet_Atk * 15) / 100);
            Default_Atk = Bullet_Atk;
            break;
        case 1:
            //���� �������� ������� �÷��̾� ������ ��ȭ
            jumpSpeed += ((jumpSpeed * 5) / 100);
            break;
        case 2:
            //���� �������� ������� �÷��̾� ���� ����
            Player_Armor += ((Player_Armor * 15) / 100);
            break;
        case 3:
            //���� �������� ������� �÷��̾� ���ݼӵ� ����
            Bullet_Delay -= ((Bullet_Delay * 15) / 100);
            break;
        case 4:
            //���� �������� ������� �÷��̾� �̵��ӵ� ����
            moveSpeed += ((moveSpeed * 15) / 100);
            break;
        case 5:
            //�÷��̾� ȸ��
            Player_HP += ((Player_HP * 15) / 100);
            break;
        case 6:
            //�÷��̾� HP ����
            Player_HP -= ((Player_HP * 15) / 100);
            break;
        case 7:
            //�÷��̾� ����, ���� �������� ������� �÷��̾� ���ݷ� ����
            Player_HP -= ((Player_HP * 15) / 100);    
            Bullet_Atk += ((Bullet_Atk * 15) / 100);
            Default_Atk = Bullet_Atk;
            break;           
        case 8:
            //�÷��̾� ����, ���ݼӵ� ����
            Player_HP -= ((Player_HP * 15) / 100);
            Bullet_Delay -= ((Bullet_Delay * 15) / 100);
            break;
        case 9:
            //�÷��̾� HP ����, ���� ����
            Player_HP -= ((Player_HP * 15) / 100);
            Player_Armor += ((Player_Armor * 15) / 100);
            break;*/
}
