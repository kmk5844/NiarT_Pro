using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay_Tutorial_Director : MonoBehaviour
{
    public Tutorial_Player player;
    public Tutorial_List tutorialList;
    bool T_Flag;
    float clickTime;
    bool clickFlag;

    private void Start()
    {
        T_Flag = true;
        tutorialList = Tutorial_List.T_Move_A;
    }

    private void Update()
    {
        if(tutorialList == Tutorial_List.T_Move_A)
        {
            if (T_Flag)
            {
                player.T_MoveFlag = true;
                player.T_Move_Flag_A = true;
                T_Flag = false;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                clickFlag = true;
            }

            if (clickFlag)
            {
                clickTime += Time.deltaTime;

            }

            if (Input.GetKeyUp(KeyCode.A)) { 
                clickFlag = false;
            }

            if(clickTime > 1.5f)
            {
                tutorialList = Tutorial_List.T_Move_D;
                clickTime = 0f;
                T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.T_Move_D)
        {
            if (T_Flag)
            {
                player.T_Move_Flag_D = true;
                T_Flag = false;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                clickFlag = true;
            }

            if (clickFlag)
            {
                clickTime += Time.deltaTime;

            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                clickFlag = false;
            }

            if (clickTime > 1.5f)
            {
                tutorialList = Tutorial_List.T_Jump;
                T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.T_Jump)
        {
            if (T_Flag)
            {
                player.T_JumpFlag = true;
                T_Flag = false;
            }

            if(player.T_JumpCount > 2 && !player.jumpFlag)
            {
                tutorialList = Tutorial_List.T_Fire;
                T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.T_Fire)
        {
            if(T_Flag)
            {
                player.T_FireFlag = true;
                T_Flag = false;
            }

            if(player.T_FireCount > 5)
            {
                tutorialList = Tutorial_List.End;
                T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.End)
        {
            if (T_Flag)
            {
                Debug.Log("Á¾·á");
                T_Flag = false;
            }
        }
    }


    public enum Tutorial_List
    {
        Start,

        T_Move_A,
        T_Move_D,
        T_Jump,
        T_Fire,
        T_Fire_Kill,

        End,

        Refresh,
    }
}