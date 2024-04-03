using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDirector : MonoBehaviour
{
    public GameObject GameDirector_Object;
    GameDirector gamedirector;

    public GameObject Game_UI;
    public GameObject Win_UI;
    public GameObject Lose_UI;

    private void Start()
    {
        gamedirector = GameDirector_Object.GetComponent<GameDirector>();
    }

    private void Update()
    {

    }

    public void Open_WIN_UI()
    {
        Game_UI.SetActive(false);
        Win_UI.SetActive(true);
    }

    public void Open_Lose_UI()
    {
        Game_UI.SetActive(false);
        Lose_UI.SetActive(true);
    }

    public void Clcik_Station()
    {
        SceneManager.LoadScene("Station");
    }

    public void Click_Retry()
    {
        SceneManager.LoadScene("InGame");
    }
}
