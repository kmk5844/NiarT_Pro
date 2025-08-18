using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronPlateFactory : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;

    public float SpawnTime;

    float lastTime;
    public bool useflag;

    void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector.GetComponent<GameDirector>();


        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
    }

    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if (Time.time > lastTime + SpawnTime && !useflag)
            {
                useflag = true;
            }
        }
    }
    public void ClickTrain()
    {
        int HP = Random.Range(1, 3);
        HP *= 100;

        gameDirector.Item_Spawn_Train_BulletproofPlate(HP, 0);

        useflag = false;
        lastTime = Time.time;
    }
}