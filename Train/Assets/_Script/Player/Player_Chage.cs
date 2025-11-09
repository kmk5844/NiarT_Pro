using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Chage : MonoBehaviour
{
    public bool isPlayable;

    [Header("스프라이트 모음")]
    public Sprite[] MariGold_Sprite;
    public Sprite[] Peyote_Sprite;
    public Sprite[] Aster_Sprite;
    public Sprite[] Rodanthe_Sprite;
    public Sprite[] Acer_Sprite;

    [Header("플레이어")]
    public SpriteRenderer Body;
    public SpriteRenderer[] Foot;
    public GameObject[] Gun;
    public Transform[] FireZone;

    public GameObject Sage_Add_Cloth;
    public GameObject Acer_Add_Cloth;
    public void ChangePlayer(int num)
    {
        Sage_Add_Cloth.SetActive(false);
        Acer_Add_Cloth.SetActive(false);

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
                Sage_Add_Cloth.SetActive(true);
                break;
            case 2:
                Body.sprite = Aster_Sprite[0];
                Foot[0].sprite = Aster_Sprite[1];
                Foot[1].sprite = Aster_Sprite[2];
                Gun[2].SetActive(true);
                break;
            case 3:
                Body.sprite = Rodanthe_Sprite[0];
                Foot[0].sprite = Rodanthe_Sprite[1];
                Foot[1].sprite = Rodanthe_Sprite[2];
                Gun[3].SetActive(true);
                break;
            case 4:
                Body.sprite = Acer_Sprite[0];
                Foot[0].sprite = Acer_Sprite[1];
                Foot[1].sprite = Acer_Sprite[2];
                Gun[4].SetActive(true);
                Acer_Add_Cloth.SetActive(true);
                break;
        }
    }

    public void ChangePlayer_NoneGun(int num, SpriteRenderer[] playerBody)
    {
        switch (num)
        {
            case 0:
                //Debug.Log("메리골드");
                playerBody[0].sprite = MariGold_Sprite[0]; // Body
                playerBody[1].sprite = MariGold_Sprite[1]; // Foot_L
                playerBody[2].sprite = MariGold_Sprite[2]; // Foot_R
                playerBody[3].sprite = MariGold_Sprite[3]; // Arm_L
                playerBody[4].sprite = MariGold_Sprite[4]; // Arm_R
                break;
            case 1:
                //Debug.Log("세이지");
                playerBody[0].sprite = Peyote_Sprite[0]; // Body
                playerBody[1].sprite = Peyote_Sprite[1]; // Foot_L
                playerBody[2].sprite = Peyote_Sprite[2]; // Foot_R
                playerBody[3].sprite = Peyote_Sprite[3]; // Arm_L
                playerBody[4].sprite = Peyote_Sprite[4]; // Arm_R
                break;
            case 2:
                //Debug.Log("애스터");
                playerBody[0].sprite = Aster_Sprite[0]; // Body
                playerBody[1].sprite = Aster_Sprite[1]; // Foot_L
                playerBody[2].sprite = Aster_Sprite[2]; // Foot_R
                playerBody[3].sprite = Aster_Sprite[3]; // Arm_L
                playerBody[4].sprite = Aster_Sprite[4]; // Arm_R
                break;
            case 3:
                //Debug.Log("로단테");
                playerBody[0].sprite = Rodanthe_Sprite[0]; // Body
                playerBody[1].sprite = Rodanthe_Sprite[1]; // Foot_L
                playerBody[2].sprite = Rodanthe_Sprite[2]; // Foot_R
                playerBody[3].sprite = Rodanthe_Sprite[3]; // Arm_L
                playerBody[4].sprite = Rodanthe_Sprite[4]; // Arm_R
                break;
            case 4:
                //Debug.Log("에이서");
                playerBody[0].sprite = Acer_Sprite[0]; // Body
                playerBody[1].sprite = Acer_Sprite[1]; // Foot_L
                playerBody[2].sprite = Acer_Sprite[2]; // Foot_R
                playerBody[3].sprite = Acer_Sprite[3]; // Arm_L
                playerBody[4].sprite = Acer_Sprite[4]; // Arm_R
                break;
        }
    }

    public Transform Set_FireZone(int num)
    {
        return FireZone[num];
    }
}
