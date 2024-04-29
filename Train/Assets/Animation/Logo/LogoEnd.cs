using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoEnd : MonoBehaviour
{
    void Start()
    {
        //기차 소리 추가
    }

    public void Logo_End()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
