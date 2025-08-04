using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hangar_Train : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;

    public float SpawnTime;
    public int[] coin = new int[3];

    public float lastTime;

    public bool useFlag;
    public bool doorFlag;
    int doorNum = -1;

    public Collider2D[] DoorCollider;
    bool changeFlag = false;    


    private void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector.GetComponent<GameDirector>();
        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
        SpawnTime = 5f;
        coin[0] = int.Parse(trainData.trainData_Special_String[1]);
        coin[1] = int.Parse(trainData.trainData_Special_String[2]);
        coin[2] = int.Parse(trainData.trainData_Special_String[3]);
        foreach(Collider2D door in DoorCollider)
        {
            door.GetComponent<Hangar_Door>().Set(this);
        }
    }
    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if (Time.time > lastTime + SpawnTime)
            {
                useFlag = true;
                if(changeFlag == false)
                {
                    for(int i = 0; i < DoorCollider.Length; i++)
                    {
                        DoorCollider[i].enabled = true;
                    }
                    changeFlag = true;
                }
            }
        }


    }

    public void ClickWeapon()
    {
        switch (doorNum)
        {
            case 0:
                Debug.Log("Weapon 0 Clicked");
                break;
            case 1:
                Debug.Log("Weapon 1 Clicked");
                break;
            case 2:
                Debug.Log("Weapon 2 Clicked");
                break;
        }

        useFlag = false;
        changeFlag = false;
        for (int i = 0; i < DoorCollider.Length; i++)
        {
            DoorCollider[i].enabled = true;
        }
        ExitDoor();
        lastTime = Time.time;
    }

    public void EnterDoor(int num)
    {
        doorFlag = true;
        doorNum = num;
    }

    public void ExitDoor()
    {
        doorFlag = false;
        doorNum = -1;
    }
}
