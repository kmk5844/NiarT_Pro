using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterDirector : MonoBehaviour
{
    public SA_PlayerData playerData;
    public AudioClip selectBGM;
    public Button[] CharacterButton;

    public GameObject SubStageSelectObject;
    public GameObject ClickProtectionPanelObject; //클릭 방지용
    bool PanelFlag;

    private void Start()
    {
        PanelFlag = false;
        playerData.SA_CharecterCheck();

        if (SubStageSelectObject.activeSelf)
        {
            SubStageSelectObject.SetActive(false);
        }

        for (int i = 0; i < CharacterButton.Length; i++)
        {
            CharacterButton[i].interactable = playerData.Character_LockOff[i];
        }
        MMSoundManagerSoundPlayEvent.Trigger(selectBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
    }

    private void Update()
    {
        if(!SubStageSelectObject.activeSelf && PanelFlag)
        {
            StartCoroutine(PanelOff());
        }
    }

    public void Click_PlayableButton(int i)
    {
        playerData.SA_Click_Playable(i);
        StartCoroutine(Start_Game());  
    }

    IEnumerator Start_Game()
    {
        yield return new WaitForSeconds(1f);
        LoadingManager.LoadScene("InGame");
    }

    public void Station_Button()
    {
        PanelFlag = true;
        SubStageSelectObject.SetActive(true);
        ClickProtectionPanelObject.SetActive(true);
    }

    //급한 클릭으로 인해 방지용
    IEnumerator PanelOff()
    {
        PanelFlag = false;
        yield return new WaitForSeconds(1f);
        ClickProtectionPanelObject.SetActive(false);
    }
}