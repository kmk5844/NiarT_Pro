using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDirector : MonoBehaviour
{
    public GameObject GameDirector_Object;
    GameDirector gamedirector;
    [Header("전체적인 UI 오브젝트")]
    public GameObject Game_UI;
    public GameObject Win_UI;
    public GameObject Lose_UI;

    [Header("Win UI 관련된 텍스트")]
    public TextMeshProUGUI Stage_Text;
    public TextMeshProUGUI Score_Text;
    public TextMeshProUGUI Reward_Coin_Text;
    public TextMeshProUGUI Reward_Point_Text;
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

    public void Win_Text(int StageNum, string StageName, int Score, int Coin, int Point)
    {
        Stage_Text.text = "Stage" + StageNum + " : " + StageName;
        Score_Text.text = "Total Score : " + Score;
        Reward_Coin_Text.text = "Reward Coin : " + Coin;
        Reward_Point_Text.text = "Reward : Point : " + Point;
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
