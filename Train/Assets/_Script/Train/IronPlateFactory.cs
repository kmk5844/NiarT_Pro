using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronPlateFactory : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;

    public float elapsed;
    public float SpawnTime;

    float lastTime;
    public bool useflag;

    public ParticleSystem UseEffect;

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
            elapsed += Time.deltaTime;

            // 강제로 초 단위로 변환
            int elapsedSeconds = Mathf.FloorToInt(elapsed);
            int spawnSeconds = Mathf.FloorToInt(SpawnTime);

            // 시간이 충분하면 useflag 허용 (한 번만)
            if (!useflag && elapsedSeconds >= spawnSeconds)
            {
                useflag = true;
            }
        }

    }
    public void ClickTrain()
    {
        int HP = Random.Range(1, 3);
        HP *= 100;

        UseEffect.Play();
        gameDirector.Item_Spawn_Train_BulletproofPlate(HP, 0);

        useflag = false;
        elapsed = 0;
    }
}