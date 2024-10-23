using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Train : MonoBehaviour
{
    public float Train_HP;
    float Train_Max_HP;
    public float Train_MP;
    float Train_Max_MP;
    public bool interactionFlag;
    public bool Player_Interaction_Flag;
    bool FullMP;
    Tutorial_Player player;
    GamePlay_Tutorial_Director director;

    public Image TrainHP_Image;
    public Image TrainMP_Image;

    private void Awake()
    {
        Train_HP = 5000;
        Train_Max_HP = Train_HP;
        if (interactionFlag) // 수동 포탑 기차
        {
            Train_MP = 0;
            Train_Max_MP = 1000;
        }
        else // 일반기차
        {
            Train_MP = 1000;
            Train_Max_MP = Train_MP;
        }
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Tutorial_Player>();
        director = GameObject.Find("TutorialDirector").GetComponent<GamePlay_Tutorial_Director>();
    }

    private void Update()
    {
        TrainHP_Image.fillAmount = Train_HP / Train_Max_HP;
        TrainMP_Image.fillAmount = Train_MP / Train_Max_MP;

        if (interactionFlag)
        {
            if (!FullMP)
            {
                if(Train_MP < Train_Max_MP)
                {
                    Train_MP += (100f * Time.deltaTime);
                    director.Fuel -= (1000f * Time.deltaTime);
                }
                else
                {
                    Train_MP = Train_Max_MP;
                    Player_Interaction_Flag = true;
                    FullMP = true;
                }
            }
        }
    }

    public void TrainHIt(int damage, int slow)
    {
        Train_HP -= damage;
        director.speed -= slow;
    }

    public void UseTurret()
    {
        StartCoroutine(GetComponentInChildren<Tutorial_Turret>().useTurret(player));
        Train_MP = 0;
    }
}
