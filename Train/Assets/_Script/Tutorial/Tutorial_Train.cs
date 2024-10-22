using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Train : MonoBehaviour
{
    public int Train_HP;
    public float Train_MP;
    float Train_Max_MP;
    public bool interactionFlag;
    public bool Player_Interaction_Flag;
    bool FullMP;
    Tutorial_Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Tutorial_Player>();
        Train_HP = 5000;
        if (interactionFlag)
        {
            Train_MP = 0;
            Train_Max_MP = 1000;
            
        }
        else
        {
            Train_MP = 5000;
            Train_Max_MP = 5000;
        }
    }

    private void Update()
    {
        if (interactionFlag)
        {
            if (!FullMP)
            {
                if(Train_MP < Train_Max_MP)
                {
                    Train_MP += (100f * Time.deltaTime);
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

    public void UseTurret()
    {
        StartCoroutine(GetComponentInChildren<Tutorial_Turret>().useTurret(player));
        Train_MP = 0;
    }
}
