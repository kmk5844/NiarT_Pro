using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GamePlay_Tutorial_Director : MonoBehaviour
{
    public Tutorial_UIDirector uiDirector;
    public Tutorial_Player player;
    public Tutorial_List tutorialList;
    bool T_Flag;
    public float clickTime;
    bool clickFlag;
    public GameObject ScarecrowObject;
    GameObject scarecrow;
    bool scarecrow_DestoryFlag;

    public GameObject SpawnItemObject;
    public GameObject TrainObject;

    public int score;
    public int gold;

    public float speed;
    public float Max_Speed;

    public float Fuel;
    [HideInInspector]
    public float Max_Fuel;

    private void Awake()
    {
        Fuel = 50000;
        Max_Fuel = Fuel;
        Max_Speed = 280;
    }

    private void Start()
    {
        score = 0;
        gold = 0;
        scarecrow_DestoryFlag = false;
        T_Flag = true;
        tutorialList = Tutorial_List.T_UI_Information;
    }

    private void Update()
    {
        if(tutorialList == Tutorial_List.T_UI_Information)
        {
            if (Input.GetMouseButtonDown(0))
            {
                uiDirector.nextTutorial();
            }

            if (uiDirector.checkFlag())
            {
                uiDirector.lastTutorial();
                tutorialList = Tutorial_List.T_Move_A;
                T_Flag = true;
            }
        }

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

            if (clickTime > 1.5f)
            {
                tutorialList = Tutorial_List.T_Move_D;
                clickFlag = false;
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

            if(player.T_FireCount > 2)
            {
                tutorialList = Tutorial_List.T_Fire_Kill;
                T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.T_Fire_Kill)
        {
            if (T_Flag)
            {
                scarecrow = Instantiate(ScarecrowObject);
                T_Flag = false;
            }

            if(scarecrow == null && !scarecrow_DestoryFlag)
            {
                scarecrow_DestoryFlag = true;
                uiDirector.skill_changeIcon(true);
                tutorialList = Tutorial_List.T_Skill_Q;
                T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.T_Skill_Q)
        {
            if (T_Flag)
            {
                T_Flag = false; 
            }

/*            if (!player.T_Skill_Q)
            {
                if (speed < Max_Speed)
                {
                    speed += (Time.deltaTime * 1f);
                    Fuel -= (Time.deltaTime * 1f);
                }
                else
                {
                    Fuel -= (Time.deltaTime * 1f);
                    player.T_Skill_Q = true;
                }
            }
            else
            {
                if(speed < Max_Speed)
                {
                    speed += (Time.deltaTime * 2f);
                    Fuel -= (Time.deltaTime * 1f);
                }
                else
                {
                    Fuel -= (Time.deltaTime * 1f);
                }
            }*/


            if(player.T_Skill_Q_End)
            {
                uiDirector.skill_changeIcon(false);
                tutorialList = Tutorial_List.T_Skill_E;
                T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.T_Skill_E)
        {
            if (T_Flag)
            {
                player.T_Skill_E = true;
                T_Flag = false;
            }

            if (player.T_Skill_E_End)
            {
                tutorialList = Tutorial_List.T_Spawn_Item;
                T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.T_Spawn_Item)
        {
            if (T_Flag)
            {
                Instantiate(SpawnItemObject);
                T_Flag = false;
            }

            if (player.T_SpawnItem_Flag)
            {
                uiDirector.item_changeIcon(true);
                tutorialList = Tutorial_List.T_Use_Item;
                T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.T_Use_Item)
        {
            if (T_Flag)
            {
                player.T_UseItem = true;
                T_Flag = false;
            }

            if (player.T_UseItem_Flag)
            {
                uiDirector.item_changeIcon(false);
                tutorialList = Tutorial_List.T_Train;
                T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.T_Train)
        {
            if (T_Flag)
            {
                TrainObject.SetActive(true);
                player.T_Train = true;
                T_Flag = false;
            }

            if (player.T_Train_Flag)
            {
                Debug.Log("End");
            }
        }
    }

    public void UseItem(ItemDataObject item)
    {
        if(item.Num == 41)
        {
            player.PlayerHP_Item(false);
            StartCoroutine(player.Item_41());
        }else if(item.Num == 2)
        {
            player.PlayerHP_Item(true);
        }
    }

    public void Get_Score(int _score, int _gold)
    {
        score += _score;
        gold += _gold;
    }

    public enum Tutorial_List
    {
        Start,

        T_UI_Information,
        T_Move_A,
        T_Move_D,
        T_Jump,
        T_Fire,
        T_Fire_Kill,
        T_Skill_Q,
        T_Skill_E,
        T_Spawn_Item,
        T_Use_Item,
        T_Train,
        T_GameSystem,
        End,
        Refresh,
    }
}