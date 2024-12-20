using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoTutorial : MonoBehaviour
{
    public SA_PlayerData playerData;
    public SA_Tutorial tutorialData;
    int PlayerStageNum;
    //-> 그걸 이용해서 bool 판단

    public AudioClip Tutorial_BGM;

    int Count;
    int MaxCount;
    public int InGame_Max_Num;

    string st = "";
    public LocalizeSpriteEvent Image;

    bool ClickFlag;

    public TextMeshProUGUI nextText;

    // Start is called before the first frame update
    void Start()
    {
        MMSoundManagerSoundPlayEvent.Trigger(Tutorial_BGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
        ClickFlag = true;
        PlayerStageNum = playerData.New_Stage;

        Count = 0;
        MaxCount = 0;

        Image.AssetReference.TableReference = "Tutorial_Table_Asset";

        st = "In_";
        MaxCount = InGame_Max_Num;

        Image.AssetReference.TableEntryReference = st + Count;

        StartCoroutine(ClickDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && ClickFlag)
        {
            ChangeImage();
            StartCoroutine(ClickDelay());
        }
    }

    IEnumerator ClickDelay()
    {
        ClickFlag = false;
        nextText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        nextText.gameObject.SetActive(true);
        ClickFlag = true;
    }

    void ChangeImage()
    {
        Count++;
        if (Count < MaxCount)
        {
            Image.AssetReference.TableEntryReference = st + Count;
        }
        else
        {
            SceneManager.LoadSceneAsync("InGame");

            tutorialData.ChangeFlag(-1);
        }

/*        if(T_InGame_F_Station) // InGame
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
        }*/
    }
}
