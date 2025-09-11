using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair_Train : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;
    Transform TrainList_InGame;

    public float elapsed;
    public float pausedElapsed;
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
        gameDirector = trainData.gameDirector;
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
            if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
            {
                if (!trainData.DestoryFlag)
                {
                    // 프레임 단위 누적
                    elapsed += Time.deltaTime;

                    // 강제로 초 단위로 변환
                    int elapsedSeconds = Mathf.FloorToInt(elapsed);
                    int spawnSeconds = Mathf.FloorToInt(SpawnTime);

                    while (elapsedSeconds >= spawnSeconds)
                    {
                        SpawnRepair();

                        // 초 단위 차감
                        elapsedSeconds -= spawnSeconds;

                        // elasped도 차감
                        elapsed -= spawnSeconds;
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
            else if (gameDirector.gameType == GameType.Refreshing)
            {
                // Refresh 상태에서는 누적 시간을 그대로 유지
                pausedElapsed = Time.time - lastTime;
            }
        }
    }

    public void SpawnRepair()
    {
        GameObject robot = Instantiate(RepairRobotObject, transform.position, Quaternion.identity);
        GameObject train = TrainList_InGame.GetChild(TargetNum).gameObject;
        robot.GetComponent<RepairRobot>().Set(train, HealParsent, HealCount);
    }
}
