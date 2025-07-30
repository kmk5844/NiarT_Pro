using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair_Train : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;
    public GameObject RepairRobot;
    public float SpawnTime;
    public bool SpawnFlag;
    public float lastTime;


    // Start is called before the first frame update
    void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector.GetComponent<GameDirector>();

    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if(Time.time > lastTime + SpawnTime)
            {
                Instantiate(RepairRobot, transform.position, Quaternion.identity);
                lastTime = Time.time;
            }
        }
    }
}
