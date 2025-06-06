using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PixelCrushers.AnimatorSaver;

public class CharacterDirector : MonoBehaviour
{
    public SA_PlayerData playerData;
    public AudioClip selectBGM;
    public Button[] CharacterButton;
    public GameObject[] Seal_Object;

    private void Start()
    {

        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }

        playerData.SA_CharecterCheck();

        for (int i = 0; i < CharacterButton.Length; i++)
        {
            CharacterButton[i].interactable = playerData.Character_LockOff[i];
            Seal_Object[i].SetActive(!playerData.Character_LockOff[i]);
        }
        MMSoundManagerSoundPlayEvent.Trigger(selectBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
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
}