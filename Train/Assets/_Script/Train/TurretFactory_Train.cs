using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFactory_Train : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;

    public float SpawnTime;
    public int TurretCount;

    public float lastTime;

    public GameObject[] Spawn_TurretObject;

    private void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector.GetComponent<GameDirector>();

        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
        TurretCount = int.Parse(trainData.trainData_Special_String[1]);
    }

    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if (Time.time > lastTime + SpawnTime)
            {
                
                lastTime = Time.time;
            }
        }
    }

    public void SpawnTurret()
    {
        for (int i = 0; i < TurretCount; i++)
        {
            // 랜덤하게 생성
            int RandomNum = Random.Range(0, 2);

            float posx = Random.Range(MonsterDirector.MinPos_Ground.x, MonsterDirector.MaxPos_Ground.x);

            Vector2 pos = new Vector2(posx, transform.position.y);

            Instantiate(Spawn_TurretObject[RandomNum], pos, Quaternion.identity);
            /*switch (RandomNum)
            {
                
            }*/

        }
    }
}