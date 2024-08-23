using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterDirector : MonoBehaviour
{
    public SA_PlayerData playerData;
    public AudioClip selectBGM;

    private void Start()
    {
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
