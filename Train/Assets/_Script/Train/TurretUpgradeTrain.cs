using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradeTrain : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;

    public float elapsed;
    public float SpawnTime;
    float Persent;
    float delayTime;

    float lastTime;
    public bool useflag;

    void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector;

        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
        Persent = float.Parse(trainData.trainData_Special_String[1]);
        delayTime = float.Parse(trainData.trainData_Special_String[2]);
    }

    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            elapsed += Time.deltaTime;

            // ������ �� ������ ��ȯ
            int elapsedSeconds = Mathf.FloorToInt(elapsed);
            int spawnSeconds = Mathf.FloorToInt(SpawnTime);

            // �ð��� ����ϸ� useflag ��� (�� ����)
            if (!useflag && elapsedSeconds >= spawnSeconds)
            {
                useflag = true;
            }
        }
    }
    public void ClickTrain()
    {
        gameDirector.Item_Use_Train_Turret_All_SpeedUP(Persent,delayTime);

        useflag = false;
        elapsed = 0;
    }
}
