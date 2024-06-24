using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;

    [SerializeField]
    bool FadeFlag;
    [SerializeField]
    Slider Loading_Slider;
    [SerializeField]
    Image FadeImage;
/*    [SerializeField]
    float FadeInDuration = 0.5f;*/
    [SerializeField]

    private void Start()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        StartCoroutine(LoadScene_LoadingBar());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene_LoadingBar()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                Loading_Slider.value = Mathf.Lerp(0f, op.progress, timer * 32);
                if (Loading_Slider.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                Loading_Slider.value = Mathf.Lerp(Loading_Slider.value, 1f, timer);
                if (Loading_Slider.value >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}