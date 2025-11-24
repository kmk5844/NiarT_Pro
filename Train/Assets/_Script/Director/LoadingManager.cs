using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;

    private void Start()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        StartCoroutine(Loading());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync(nextScene);
    }
}