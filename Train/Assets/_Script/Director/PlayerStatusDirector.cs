using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusDirector : MonoBehaviour
{
    //public GameDirector gamedirector;
    Player player;

    int origin_Atk;
    float origin_AtkDelay;
    int origin_Armor;
    float orgin_moveSpeed;

    int now_ATK;
    float now_AtkDelay;
    int now_Armor;
    float now_moveSpeed;


    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        Debug.Log("Atk : " + origin_Atk  + " + " + (now_ATK - origin_Atk));
        Debug.Log("AtkDelay : " + origin_AtkDelay + " + " + (now_AtkDelay - origin_AtkDelay));
        Debug.Log("Armor : " + origin_Armor + " + " + (now_Armor - origin_Armor));
        Debug.Log("moveSpeed : " + orgin_moveSpeed + " + " + (now_moveSpeed - orgin_moveSpeed));
    }

    public void SetOriginStatus(int atk, float atkDelay, int armor, float moveSpeed)
    {
        origin_Atk = atk;
        origin_AtkDelay = atkDelay;
        origin_Armor = armor;
        orgin_moveSpeed = moveSpeed;
    }

    public void GetNowSatus(int atk, float atkDelay, int armor, float moveSpeed)
    {
        now_ATK = atk;
        now_AtkDelay = atkDelay;
        now_Armor = armor;
        now_moveSpeed = moveSpeed;
    }
}