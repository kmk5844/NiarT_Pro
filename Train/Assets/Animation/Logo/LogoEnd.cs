using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoEnd : MonoBehaviour
{
    void Start()
    {
        //���� �Ҹ� �߰�
    }

    public void Logo_End()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
