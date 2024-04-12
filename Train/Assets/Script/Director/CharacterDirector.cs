using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterDirector : MonoBehaviour
{
    public SA_PlayerData playerData;
    
    public void Click_PlayableButton(int i)
    {
        playerData.SA_Click_Playable(i);
        StartCoroutine(Start_Game());
    }

    IEnumerator Start_Game()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("InGame");
    }
}
