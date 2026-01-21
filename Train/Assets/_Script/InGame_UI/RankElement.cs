using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankElement : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    int rank;
    string playerName;
    int score;

    // Start is called before the first frame update
    void Start()
    {
        rankText.text = rank.ToString();
        nameText.text = playerName;
        scoreText.text = score.ToString();
    }
    
    public void SetElement(int rank, string playerName, int score, bool flag, bool isMine)
    {
        this.rank = rank;
        this.playerName = playerName;
        this.score = score;

        if (flag)
        {
            rankText.text = rank.ToString();
            if (isMine)
            {
                nameText.text = playerName;
            }
            else
            {
                nameText.text = "Player";
            }
            scoreText.text = score.ToString();
        }
    }
}
