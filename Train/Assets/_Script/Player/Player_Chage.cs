using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Chage : MonoBehaviour
{
    [Header("스프라이트 모음")]
    public Sprite[] MariGold_Sprite;
    public Sprite[] Peyote_Sprite;

    [Header("플레이어")]
    public SpriteRenderer Body;
    public SpriteRenderer[] Foot;
    public GameObject[] Gun;
    public Transform[] FireZone;

    public void ChangePlayer(int num)
    {
        switch (num)
        {
            case 0:
                Body.sprite = MariGold_Sprite[0];
                Foot[0].sprite = MariGold_Sprite[1];
                Foot[1].sprite = MariGold_Sprite[2];
                Gun[0].SetActive(true);
                break;
            case 1:
                Body.sprite = Peyote_Sprite[0];
                Foot[0].sprite = Peyote_Sprite[1];
                Foot[1].sprite = Peyote_Sprite[2];
                Gun[1].SetActive(true);
                break;
        }
    }

    public Transform Set_FireZone(int num)
    {
        return FireZone[num];
    }
}
