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
    public GameObject ScarecrowObject_Ground;
    GameObject scarecrow_ground;

    public GameObject ScarecrowObject_Sky;
    GameObject[] scarecrow_sky;

    int Scarecrow_count;
    
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

    public int distance;
    public int max_distance;

    bool ClearFlag;

    private void Awake()
    {
        Fuel = 60000;
        Max_Fuel = Fuel;
        Max_Speed = 280;
        max_distance = 14;
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
               
                if (!ClearFlag)
                {
                    distance++;//1
                    StartCoroutine(Clear(Tutorial_List.T_Move));
                    ClearFlag = true;
                }
                //T_Flag = true;
            }
        }

        if(tutorialList == Tutorial_List.T_Move)
        {
            if (T_Flag)
            {
                player.T_MoveFlag = true;
                T_Flag = false;
            }

            if (Input.GetButton("Horizontal"))
            {
                clickTime += Time.deltaTime;
            }

            if (clickTime > 2f)
            {
                clickTime = 0f;
                if (!ClearFlag)
                {
                    distance++;//2
                    StartCoroutine(Clear(Tutorial_List.T_Jump));
                    ClearFlag = true;
                }
                //T_Flag = true;
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
                if (!ClearFlag)
                {
                    distance++;//3
                    StartCoroutine(Clear(Tutorial_List.T_Fire));
                    ClearFlag = true;
                }
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
                if (!ClearFlag)
                {
                    distance++;//4
                    StartCoroutine(Clear(Tutorial_List.T_Fire_Kill));
                    ClearFlag = true;
                }
            }
        }

        if(tutorialList == Tutorial_List.T_Fire_Kill)
        {
            if (T_Flag)
            {
                scarecrow_ground = Instantiate(ScarecrowObject_Ground);
                T_Flag = false;
            }

            if(scarecrow_ground == null && !scarecrow_DestoryFlag && !T_Flag)
            {
                scarecrow_DestoryFlag = true;
                uiDirector.skill_changeIcon(true);
                if (!ClearFlag)
                {
                    distance++;//5
                    StartCoroutine(Clear(Tutorial_List.T_Skill_Q));
                    ClearFlag = true;
                }
            }
        }

        if(tutorialList == Tutorial_List.T_Skill_Q)
        {
            if (T_Flag)
            {
                speed += 150;
                T_Flag = false; 
            }

            if (!player.T_Skill_Q)
            {
                if (speed < Max_Speed)
                {
                    speed += (Time.deltaTime * 20f);
                    Fuel -= (Time.deltaTime * 1000f);
                }
                else
                {
                    Fuel -= (Time.deltaTime * 1000f);
                    player.T_Skill_Q = true;
                }
            }
            else
            {
                if (player.T_Skill_Q_Click)
                {
                    if (speed < Max_Speed + 200)
                    {
                        speed += (Time.deltaTime * 40f);
                        Fuel -= (Time.deltaTime * 1000f);
                    }
                    else
                    {
                        player.T_Skill_Q_End = true;
                    }
                }
            }

            if (player.T_Skill_Q_End)
            {
                uiDirector.skill_changeIcon(false);
                if (!ClearFlag)
                {
                    distance++;//6
                    StartCoroutine(Clear(Tutorial_List.T_Skill_E));
                    ClearFlag = true;
                }
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
                if (!ClearFlag)
                {
                    distance++;//7
                    StartCoroutine(Clear(Tutorial_List.T_Spawn_Item));
                    ClearFlag = true;
                }
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
                if (!ClearFlag)
                {
                    distance++;//8
                    StartCoroutine(Clear(Tutorial_List.T_Use_Item));
                    ClearFlag = true;
                }
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
                if (!ClearFlag)
                {
                    distance++;//9
                    StartCoroutine(Clear(Tutorial_List.T_Train));
                    ClearFlag = true;
                }
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
                scarecrow_sky = new GameObject[5];
                if (!ClearFlag)
                {
                    distance++;//10
                    StartCoroutine(Clear(Tutorial_List.T_Monster));
                    ClearFlag = true;
                }
            }
        }

        if(tutorialList == Tutorial_List.T_Monster)
        {
            if (T_Flag)
            {
                for(int i = 0; i < 5; i++)
                {
                    scarecrow_sky[i] = Instantiate(ScarecrowObject_Sky, new Vector2(0 - (6 * i), 16), Quaternion.identity);
                    Scarecrow_count++;
                }
                T_Flag = false;
            }

            if(Scarecrow_count == 0 && !T_Flag)
            {
                if (!ClearFlag)
                {
                    distance++;//11
                    StartCoroutine(Clear(Tutorial_List.T_Fuel));
                    ClearFlag = true;
                }
            }
        }

        if (tutorialList == Tutorial_List.T_Fuel)
        {
            if (T_Flag)
            {
                T_Flag = false;
            }

            if (Fuel > 0)
            {
                Fuel -= (Time.deltaTime * 3000f);
            }
            else
            {
                if (!ClearFlag)
                {
                    distance++;//12
                    StartCoroutine(Clear(Tutorial_List.T_Lose));
                    ClearFlag = true;
                }
            }
        }

        if(tutorialList == Tutorial_List.T_Lose)
        {
            if (T_Flag)
            {
                T_Flag = false;
            }

            if(player.PlayerHP > 0)
            {
                player.PlayerHP -= (int)(Time.deltaTime * 1000f);
                speed = 480f;
            }
            else
            {
                if(speed > 0)
                {
                    
                    speed -= (Time.deltaTime * 100f);
                }
                else
                {
                    if (!ClearFlag)
                    {
                        distance++;//13
                        StartCoroutine(Clear(Tutorial_List.T_Win));
                        ClearFlag = true;
                    }
                }
            }
        }

        if (tutorialList == Tutorial_List.T_Win)
        {
            if (T_Flag)
            {
                T_Flag = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                distance++;//14
            }

            if(distance > max_distance)
            {
                if (!ClearFlag)
                {
                    StartCoroutine(Clear(Tutorial_List.End));
                    ClearFlag = true;
                }
            }
        }

        if(tutorialList == Tutorial_List.End)
        {
            if (T_Flag)
            {
                Debug.Log("End");
                T_Flag = false;
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

    public void Get_Score(int _score, int _gold, bool flag)
    {
        score += _score;
        gold += _gold;
        if (flag)
        {
            Scarecrow_count--;
        }
    }

    public IEnumerator Tutorial_Train_MaxSpeed_Change(int addSpeed, float during)
    {
        speed += addSpeed;
        yield return new WaitForSeconds(during);
        speed -= addSpeed;
    }

    IEnumerator Clear(Tutorial_List list)
    {
        uiDirector.ClearObject.SetActive(true);
        uiDirector.Compelte_Object.SetActive(true);
        yield return new WaitForSeconds(1f);

        uiDirector.changeText(distance);
        //uiDirector.Title_Text.text = distance.ToString();
        
        yield return new WaitForSeconds(0.5f);
        uiDirector.Compelte_Object.SetActive(false);
        uiDirector.ClearObject.SetActive(false);
        tutorialList = list;
        T_Flag = true;
        ClearFlag = false;
    }


    public enum Tutorial_List
    {
        Start,

        T_UI_Information,
        T_Move,
        T_Jump,
        T_Fire,
        T_Fire_Kill,
        T_Skill_Q,
        T_Skill_E,
        T_Spawn_Item,
        T_Use_Item,
        T_Train,
        T_Monster,
        T_Fuel,
        T_Lose,
        T_Win,

        End,
    }
}