using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class DrowsinessShelterDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject ShelterWindow;
    public GameObject CheckWindow;
    public GameObject SelectStage;

    [Header("UI")]
    public Image targetImage;
    float fadeDuration = 2f;
    public Button SleepButton;
    public TextMeshProUGUI playerHPText;
    public GameObject[] BlackBoard;


    [Header("데이터")]
    public SA_PlayerData playerData;
    public Game_DataTable game_DataTable;
    int originPlayerHP;
    int beforePlayerHP;
    int curretPlayerHP;
    int Max_HP;
    bool startFlag;

    private void Awake()
    {
        Special_Story.Story_Init(null, 6, 0, 0);
        ShelterWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }
        originPlayerHP = game_DataTable.Information_Player[playerData.Player_Num].Player_HP;
        curretPlayerHP = ES3.Load<int>("Player_Curret_HP");
        beforePlayerHP = curretPlayerHP;
        Max_HP = originPlayerHP + (((originPlayerHP * playerData.Level_Player_HP) * 10) / 100);

        BlackBoard[0].SetActive(true);
        BlackBoard[1].SetActive(false);

        CheckWindow.SetActive(false);
        SleepButton.onClick.AddListener(() => Click_Rest());
    }

    // Update is called once per frame
    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }

    private void StartEvent()
    {
        ShelterWindow.SetActive(true);
        startFlag = true;
    }

    public void Click_Rest()
    {
        BlackBoard[0].SetActive(false);
        BlackBoard[1].SetActive(true);
        StartCoroutine(Sleep());
        SleepButton.interactable = false;
    }

    IEnumerator Sleep()
    {
        yield return StartCoroutine(FadeTo(1f));
        yield return new WaitForSeconds(5f);

        curretPlayerHP += (int)(Max_HP * 0.2f);

        // 최대 체력 초과 방지
        if (curretPlayerHP > Max_HP)
        {
            curretPlayerHP = Max_HP;
        }

        ES3.Save<int>("Player_Curret_HP", curretPlayerHP);
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

        if(targetAlpha == 0f)
        {
            playerHPText.text = beforePlayerHP + "<color=red>  ->  " + curretPlayerHP + "</color>";
            CheckWindow.SetActive(true);
        }
    }
    public void Click_NextButton()
    {
        /*MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, 10);
        MMSoundManagerSoundPlayEvent.Trigger(MissionSelectBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID: 20);*/
        SelectStage.SetActive(true);
    }
}
