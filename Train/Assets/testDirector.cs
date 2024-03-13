using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testDirector : MonoBehaviour
{
    public GameObject player; //üũ HP
    Player playerHP;
    public Transform MercenaryList; //üũ HP�� üũ ���׹̳�
    public GameObject gameobject; // ���ǵ�� ���� �Ÿ� �׸��� ��ǥ �Ÿ�
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
