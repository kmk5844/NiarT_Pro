using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;

    [SerializeField]
    Image[] progressBar;
    int num;
    private void Start()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        num = 0;
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            //if (op.progress < 0.9f)
            if(num < progressBar.Length-1)
            {
                progressBar[num].fillAmount = Mathf.Lerp(progressBar[num].fillAmount, 1f, timer);
                if (progressBar[num].fillAmount == 1f)
                {
                    timer = 0f;
                    num++;
                }
            }
            else
            {
                if(op.progress < 0.9f)
                {
                    progressBar[num].fillAmount = Mathf.Lerp(progressBar[num].fillAmount, op.progress, timer);
                    if (progressBar[num].fillAmount >= op.progress)
                    {
                        timer = 0f;
                    }
                }
                else
                {
                    progressBar[num].fillAmount = Mathf.Lerp(progressBar[num].fillAmount, 1f, timer);
                    if (progressBar[num].fillAmount == 1.0f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }


                /*                if (op.progress < 0.9f)
                                {
                                    progressBar[num].fillAmount = Mathf.Lerp(progressBar[num].fillAmount, op.progress, timer);
                                    if (progressBar[num].fillAmount >= op.progress)
                                    {
                                        timer = 0f;
                                    }
                                }
                                else
                                {
                                    progressBar[num].fillAmount = Mathf.Lerp(progressBar[num].fillAmount, 1f, timer);
                                    if (progressBar[num].fillAmount == 1.0f)
                                    {
                                        op.allowSceneActivation = true;
                                        yield break;
                                    }
                                }*/
            }
        }
        
    }
}
