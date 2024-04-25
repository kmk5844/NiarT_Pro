using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDirector : MonoBehaviour
{
    public GameObject UI_Option;
    public void Click_Stroy_Mode()
    {
        LoadingManager.LoadScene("Station");
    }

    public void Click_Infinite_Mode()
    {

    }

    public void Click_Option_Mode()
    {
        UI_Option.SetActive(true);
    }

    public void Click_Option_Back()
    {
        UI_Option.SetActive(false);
    }
    public void Click_Exit_Mode()
    {
        Application.Quit();
    }
}
