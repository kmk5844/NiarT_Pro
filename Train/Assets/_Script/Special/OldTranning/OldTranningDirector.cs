using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class OldTranningDirector : MonoBehaviour
{
    [Header("스토리")]
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
        yield return StartCoroutine(FadeTo(0f)); // 투명 (밝아짐)
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
        RewardNum = Random.Range(0, 11);
        eventData.OldTrannningOn(RewardNum);
        CheckWindowText.StringReference.TableEntryReference = "OldTranning_TranningReward_" + RewardNum;
        /*

                switch (RewardNum)
                {
                    case 0:
                        // 공격력 증가
                        CheckWindowText.text = "공격력 증가!";
                        //Debug.Log("공격력 증가");
                        break;
                    case 1:
                        // 점프력 증가
                        CheckWindowText.text = "점프력 증가!";
                        //Debug.Log("점프력 증가");
                        break;
                    case 2:
                        // 방어력 증가
                        CheckWindowText.text = "방어력 증가!";
                        //Debug.Log("방어력 증가");
                        break;
                    case 3:
                        // 공격속도 증가
                        CheckWindowText.text = "공격속도 증가!";
                        //Debug.Log("공격속도 증가");
                        break;
                    case 4:
                        // 이동속도 증가
                        CheckWindowText.text = "이동속도 증가!";
                        //Debug.Log("이동속도 증가");
                        break;
                    case 5:
                        // 체력 회복
                        CheckWindowText.text = "체력 회복!";
                        //Debug.Log("체력 증가");
                        break;
                    case 6:
                        // 체력 감소
                        CheckWindowText.text = "체력 감소!";
                        //Debug.Log("체력 감소");
                        break;
                    case 7:
                        // 체력 감소 + 공격력 증가
                        CheckWindowText.text = "체력 감소! 공격력 증가!";
                        //Debug.Log("체력 감소");
                        //Debug.Log("공격력 증가");
                        break;
                    case 8:
                        // 체력 감소 + 공격속도 증가
                        CheckWindowText.text = "체력 감소! 공격속도 증가!";
                        //Debug.Log("체력 감소");
                        //Debug.Log("공격속도 증가");
                        break;
                    case 9:
                        // 체력 감소 + 방어력 증가
                        CheckWindowText.text = "체력 감소! 방어력 증가!";
                        //Debug.Log("체력 감소");
                        //Debug.Log("방어력 증가");
                        break;
                    case 10:
                        // 아무런 효과가 없다.
                        CheckWindowText.text = "아무런 효과가 없다.";
                        //Debug.Log("X");
                        break;
                }*/
    }


    /*
        case 0:
            //다음 스테이지 종료까지 플레이어 공격력 증가
            Bullet_Atk += ((Bullet_Atk * 15) / 100);
            Default_Atk = Bullet_Atk;
            break;
        case 1:
            //다음 스테이지 종료까지 플레이어 점프력 강화
            jumpSpeed += ((jumpSpeed * 5) / 100);
            break;
        case 2:
            //다음 스테이지 종료까지 플레이어 방어력 증가
            Player_Armor += ((Player_Armor * 15) / 100);
            break;
        case 3:
            //다음 스테이지 종료까지 플레이어 공격속도 증가
            Bullet_Delay -= ((Bullet_Delay * 15) / 100);
            break;
        case 4:
            //다음 스테이지 종료까지 플레이어 이동속도 증가
            moveSpeed += ((moveSpeed * 15) / 100);
            break;
        case 5:
            //플레이어 회복
            Player_HP += ((Player_HP * 15) / 100);
            break;
        case 6:
            //플레이어 HP 감소
            Player_HP -= ((Player_HP * 15) / 100);
            break;
        case 7:
            //플레이어 감소, 다음 스테이지 종료까지 플레이어 공격력 증가
            Player_HP -= ((Player_HP * 15) / 100);    
            Bullet_Atk += ((Bullet_Atk * 15) / 100);
            Default_Atk = Bullet_Atk;
            break;           
        case 8:
            //플레이어 감소, 공격속도 증가
            Player_HP -= ((Player_HP * 15) / 100);
            Bullet_Delay -= ((Bullet_Delay * 15) / 100);
            break;
        case 9:
            //플레이어 HP 감소, 방어력 증가
            Player_HP -= ((Player_HP * 15) / 100);
            Player_Armor += ((Player_Armor * 15) / 100);
            break;*/
}
