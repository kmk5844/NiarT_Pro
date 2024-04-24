using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator Loading;

    #region ΩÃ±€≈Ê
    private static GameManager instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    public void Loading_Scene(string SceneName)
    {
        StartCoroutine(LoadScene(SceneName));
    }

    IEnumerator LoadScene(string SceneName)
    {
        Loading.gameObject.SetActive(true);
        Loading.SetTrigger("Start_Loading");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(SceneName);
        Loading.SetTrigger("End_Loading");
    }
}
