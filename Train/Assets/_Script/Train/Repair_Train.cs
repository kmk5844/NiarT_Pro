using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair_Train : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;
    Transform TrainList_InGame;
    
    public float SpawnTime;
    public float HealParsent;
    public int HealCount;

    public GameObject RepairRobotObject;
    public float lastTime;

    //public GameObject TargetTrain;
    public int TargetNum;
    public float TargetHP = -1;

    bool Targeting;

    // Start is called before the first frame update
    void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector.GetComponent<GameDirector>();
        TrainList_InGame = gameDirector.Train_List;

        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
        HealParsent = float.Parse(trainData.trainData_Special_String[1]);
        HealCount = int.Parse(trainData.trainData_Special_String[2]);

        Targeting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if(Time.time > lastTime + SpawnTime)
            {
                GameObject robot = Instantiate(RepairRobotObject, transform.position, Quaternion.identity);
                GameObject train = TrainList_InGame.GetChild(TargetNum).gameObject;
                robot.GetComponent<RepairRobot>().Set(train, HealParsent, HealCount);
                lastTime = Time.time;
            }

            if (!Targeting)
            {
                Targeting = true;
                for (int i = 0; i < TrainList_InGame.childCount; i++)
                {
                    float HP = TrainList_InGame.GetChild(i).GetComponent<Train_InGame>().HP_Parsent;

                    if (TargetHP == -1)
                    {
                        TargetHP = HP;
                        TargetNum = i;
                    }
                    else
                    {
                        if (TargetHP > HP)
                        {
                            TargetHP = HP;
                            TargetNum = i;
                        }
                    }
                }
                Targeting = false;
            }
        }
    }
}
