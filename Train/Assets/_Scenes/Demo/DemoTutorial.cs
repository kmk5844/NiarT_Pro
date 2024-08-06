using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DemoTutorial : MonoBehaviour
{
    public SA_PlayerData playerData;
    public SA_StoryData storyData;
    int PlayerStageNum;
    //-> 그걸 이용해서 bool 판단

    public AudioClip Tutorial_BGM;

    int InGameCount;
    int StationCount;
    public GameObject[] InGame;
    public GameObject[] Station;
    bool T_InGame_F_Station;

    bool ClickFlag;

    public TextMeshProUGUI nextText;

    // Start is called before the first frame update
    void Start()
    {
        MMSoundManagerSoundPlayEvent.Trigger(Tutorial_BGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
        ClickFlag = true;
        PlayerStageNum = playerData.Stage;

        InGameCount = 0;
        StationCount = 0;

        if(PlayerStageNum == 0)
        {
            T_InGame_F_Station = true;
        }
        else
        {
            T_InGame_F_Station = false;
        }

        if (T_InGame_F_Station)
        {
            InGame[0].SetActive(true);
        }
        else
        {
            Station[0].SetActive(true);
        }
        StartCoroutine(ClickDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ClickFlag)
        {
            ChangeImage();
            StartCoroutine(ClickDelay());
        }
    }

    IEnumerator ClickDelay()
    {
        ClickFlag = false;
        nextText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        nextText.gameObject.SetActive(true);
        ClickFlag = true;
    }

    void ChangeImage()
    {
        if(T_InGame_F_Station) // InGame
        {
            InGameCount++;
            if(InGameCount < InGame.Length)
            {
                InGame[InGameCount - 1].SetActive(false);
                InGame[InGameCount].SetActive(true);
            }
            else
            {
                storyData.End_Tutorial(PlayerStageNum);
            }
        }
        else // Tutorial
        {
            StationCount++;
            if (StationCount < Station.Length)
            {
                Station[StationCount - 1].SetActive(false);
                Station[StationCount].SetActive(true);
            }
            else
            {
                storyData.End_Tutorial(PlayerStageNum);
            }
        }
    }
}
