using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrowsinessShelterDirector : MonoBehaviour
{
    [Header("½ºÅä¸®")]
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

    //[Header("µ¥ÀÌÅÍ")]
    int beforePlayerHP;
    int playerHP;
    bool startFlag;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        ShelterWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }

        playerHP = ES3.Load<int>("Player_Curret_HP");
        beforePlayerHP = playerHP;

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
        StartCoroutine(Sleep());
        SleepButton.interactable = false;
    }

    IEnumerator Sleep()
    {
        Debug.Log("ÄðÄðÄð");
        yield return StartCoroutine(FadeTo(1f));
        yield return new WaitForSeconds(5f);
        playerHP = playerHP * 120 / 100;
        ES3.Save<int>("Player_Curret_HP", playerHP);
        yield return StartCoroutine(FadeTo(0f)); // Åõ¸í (¹à¾ÆÁü)
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

        // ÃÖÁ¾ °ª Á¤È®È÷ ¸ÂÃçÁÖ±â
        Color finalColor = targetImage.color;
        finalColor.a = targetAlpha;
        targetImage.color = finalColor;

        if(targetAlpha == 0f)
        {
            CheckWindow.SetActive(true);
            Debug.Log(beforePlayerHP);
            Debug.Log(playerHP);
        }
    }
    public void Click_NextButton()
    {
        /*MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, 10);
        MMSoundManagerSoundPlayEvent.Trigger(MissionSelectBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID: 20);*/
        SelectStage.SetActive(true);
    }
}
