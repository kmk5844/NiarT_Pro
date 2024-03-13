using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testDirector : MonoBehaviour
{
    public GameObject player; //체크 HP
    Player playerHP;
    public Transform MercenaryList; //체크 HP와 체크 스테미나
    public GameObject gameobject; // 스피드와 현재 거리 그리고 목표 거리
    GameDirector gameDirector;
    public TextMeshProUGUI[] text;
    float player_HP;
    float[] mercenary_HP;
    float[] mercenary_Stamina;
    int speed;
    int distance;
    int target_distance;

    private void Start()
    {
        playerHP = player.GetComponent<Player>();
        gameDirector = gameobject.GetComponent<GameDirector>();
        text[3].text = gameDirector.Destination_Distance.ToString();
    }

    private void Update()
    {
        text[0].text = "Player HP : " + playerHP.Check_HpParsent() + "%";
        text[1].text = "Speed : " + gameDirector.TrainSpeed.ToString();
        text[2].text = "Distance : " + gameDirector.TrainDistance.ToString();
    }
}
